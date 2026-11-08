import type { components } from './schema.gen'

export type Schema = components['schemas']

type Envelope<T> = Schema['ApiResponse'] & { data?: T | null }

export class ApiError extends Error {
	constructor(
		readonly status: number,
		message: string,
	) {
		super(message)
	}
}

export async function request<T>(
	path: string,
	options: {
		body?: unknown
		method?: 'GET' | 'POST' | 'PATCH' | 'PUT' | 'DELETE'
		token?: string
		signal?: AbortSignal
	} = {},
): Promise<Envelope<T>> {
	const headers = new Headers({ Accept: 'application/json' })
	const multipart = options.body instanceof FormData

	if (options.body !== undefined && !multipart) {
		headers.set('Content-Type', 'application/json')
	}

	if (options.token) {
		headers.set('Authorization', `Bearer ${options.token}`)
	}

	const base = (import.meta.env.VITE_API_BASE_URL || '/api').replace(/\/$/, '')

	const response = await fetch(base + path, {
		method: options.method ?? (options.body === undefined ? 'GET' : 'POST'),
		headers,
		body: multipart ? (options.body as FormData) : JSON.stringify(options.body),
		signal: options.signal,
	})

	const result = await response.json().catch(() => null)

	if (!response.ok) {
		throw new ApiError(
			response.status,
			result?.message || `Request failed (${response.status}).`,
		)
	}

	if (!result || typeof result.message !== 'string') {
		throw new Error('Unexpected API response.')
	}

	return result
}

export function requireData<T>(response: Envelope<T>): T {
	if (response.data == null) {
		throw new Error('Missing data in API response.')
	}

	return response.data
}
