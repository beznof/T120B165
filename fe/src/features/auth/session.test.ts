import { afterEach, expect, it, vi } from 'vitest'
import { ApiError } from '../../api/client'
import { login, logout, signedIn, withAccessToken } from './session'

function tokens(version: string) {
  return { accessToken: version, refreshToken: `refresh-${version}` }
}
function reply(data: unknown) {
  return Response.json({ message: 'OK', links: {}, data })
}
afterEach(async () => {
  vi.stubGlobal('fetch', async () => reply(null))
  await logout()
  vi.unstubAllGlobals()
})

it('shares a token rotation across concurrent requests and revokes the replacement on logout', async () => {
  const fetcher = vi
    .fn()
    .mockResolvedValueOnce(reply(tokens('old')))
    .mockResolvedValueOnce(reply(tokens('new')))
    .mockResolvedValueOnce(reply(null))
  vi.stubGlobal('fetch', fetcher)
  await login({ username: 'learner', password: 'example' })
  const send = vi.fn(async (token: string) => {
    if (token === 'old') throw new ApiError(401, 'Expired')
    return token
  })
  expect(
    await Promise.all([withAccessToken(send), withAccessToken(send)]),
  ).toEqual(['new', 'new'])
  await logout()
  expect(fetcher.mock.calls.map((call) => call[0])).toEqual([
    '/api/auth/login',
    '/api/auth/refresh',
    '/api/auth/logout',
  ])
  expect(JSON.parse(fetcher.mock.calls[2]?.[1].body)).toEqual({
    refreshToken: 'refresh-new',
  })
  expect(signedIn()).toBe(false)
})

it('preserves the session on forbidden and ends it when refresh is rejected', async () => {
  const fetcher = vi
    .fn()
    .mockResolvedValueOnce(reply(tokens('old')))
    .mockResolvedValueOnce(
      Response.json({ message: 'Expired session' }, { status: 401 }),
    )
  vi.stubGlobal('fetch', fetcher)
  await login({ username: 'learner', password: 'example' })
  const forbidden = new ApiError(403, 'Forbidden')
  await expect(withAccessToken(() => Promise.reject(forbidden))).rejects.toBe(
    forbidden,
  )
  expect(fetcher).toHaveBeenCalledTimes(1)
  expect(signedIn()).toBe(true)
  await expect(
    withAccessToken(() => Promise.reject(new ApiError(401, 'Expired'))),
  ).rejects.toThrow('Expired session')
  expect(signedIn()).toBe(false)
})
