import { ApiError, request, requireData, type Schema } from '../../api/client'

type Tokens = Schema['AuthTokensOutput']
export type Account = { id: number; username: string; role: 'Learner' | 'Teacher' | 'Admin' }
type Session = {
	tokens: Tokens
	account: Account | null
	refreshing?: Promise<Tokens>
}

let current: Session | undefined

const listeners = new Set<() => void>()

export const signedIn = () => Boolean(current)
export const getAccount = () => current?.account ?? null

// Claims guide the UI. The API still authorizes every request.
function readAccount(token: string, fallbackUsername: string): Account | null {
	try {
		const payload = token.split('.')[1]
		if (!payload) {
			return null
		}
		const bytes = Uint8Array.from(
			atob(payload.replaceAll('-', '+').replaceAll('_', '/')),
			(char) => char.charCodeAt(0),
		)
		const { sub, name, role } = JSON.parse(new TextDecoder().decode(bytes))
		const id = Number(sub)
		const username = typeof name === 'string' && name.trim() ? name : fallbackUsername
		return Number.isInteger(id) && id > 0 && ['Learner', 'Teacher', 'Admin'].includes(role)
			? { id, username, role }
			: null
	} catch {
		return null
	}
}

export function onSessionChange(listener: () => void) {
	listeners.add(listener)

	return () => listeners.delete(listener)
}

function setSession(value?: Session) {
	current = value

	for (const listener of listeners) {
		listener()
	}
}

export async function login(body: Schema['LoginRequest']) {
	const tokens = requireData(await request<Tokens>('/auth/login', { body }))

	setSession({ tokens, account: readAccount(tokens.accessToken, body.username) })
}

function refresh(session: Session) {
	session.refreshing ??= request<Tokens>('/auth/refresh', {
		body: {
			refreshToken: session.tokens.refreshToken,
		} satisfies Schema['RefreshRequest'],
	})
		.then(requireData)
		.then((tokens) => {
			session.tokens = tokens

			return tokens
		})
		.catch((error) => {
			if (current === session && error instanceof ApiError && error.status === 401) {
				setSession()
			}

			throw error
		})
		.finally(() => {
			session.refreshing = undefined
		})

	return session.refreshing
}

export async function withAccessToken<T>(send: (token: string) => Promise<T>): Promise<T> {
	const session = current

	if (!session) {
		throw new ApiError(401, 'Please sign in.')
	}

	const token = session.tokens.accessToken

	try {
		return await send(token)
	} catch (error) {
		if (!(error instanceof ApiError) || error.status !== 401 || current !== session) {
			throw error
		}

		if (session.tokens.accessToken === token) {
			await refresh(session)
		}

		if (current !== session) {
			throw error
		}

		try {
			return await send(session.tokens.accessToken)
		} catch (retryError) {
			if (
				current === session &&
				retryError instanceof ApiError &&
				retryError.status === 401
			) {
				setSession()
			}

			throw retryError
		}
	}
}

export async function logout() {
	const session = current

	if (!session) {
		return
	}

	// A refresh already in flight may replace the token we need to revoke.
	try {
		await session.refreshing

		await request('/auth/logout', {
			body: {
				refreshToken: session.tokens.refreshToken,
			} satisfies Schema['LogoutRequest'],
		})
	} finally {
		if (current === session) {
			setSession()
		}
	}
}
