import { queryOptions } from '@tanstack/react-query'
import { request, requireData, type Schema } from '../../api/client'
import { withAccessToken } from '../auth/session'

export function languagesQuery(page: number, pageSize = 20, search = '') {
  return queryOptions({
    queryKey: ['languages', { page, pageSize, search }],
    queryFn: async ({ signal }) => {
      const params = new URLSearchParams({
        page: String(page),
        pageSize: String(pageSize),
      })
      if (search) params.set('search', search)
      return requireData(
        await withAccessToken((token) =>
          request<Schema['GetManyLanguagesOutput']>(`/languages?${params}`, {
            token,
            signal,
          }),
        ),
      )
    },
    retry: false,
  })
}
