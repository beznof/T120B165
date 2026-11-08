import { queryOptions } from '@tanstack/react-query'
import { request, requireData, type Schema } from '../../api/client'
import { type Account, withAccessToken } from '../auth/session'

export const catalogKey = ['catalog'] as const
export const pageSize = 10
export const pageSizeOptions = [10, 20, 30, 50]
export const maxPage = 10_000

export const languagesPath = '/languages'
export const languagePath = (id: string | number) => `${languagesPath}/${id}`
export const dictionariesPath = (languageId: string | number) =>
	`${languagePath(languageId)}/dictionaries`
export const dictionaryPath = (languageId: string | number, dictionaryId: string | number) =>
	`${dictionariesPath(languageId)}/${dictionaryId}`
export const entriesPath = (languageId: string | number, dictionaryId: string | number) =>
	`${dictionaryPath(languageId, dictionaryId)}/entries`

export type CatalogSearch = { q?: string; page?: number; pageSize?: number }
export type EntrySearch = CatalogSearch & { type?: 'word' | 'phrase' }

export function parseSearch(search: Record<string, unknown>): CatalogSearch {
	return {
		pageSize:
			typeof search.pageSize === 'number' && pageSizeOptions.includes(search.pageSize)
				? search.pageSize
				: undefined,
		q: typeof search.q === 'string' ? search.q.trim().slice(0, 500) || undefined : undefined,
		page:
			typeof search.page === 'number' &&
			Number.isInteger(search.page) &&
			search.page > 1 &&
			search.page <= maxPage
				? search.page
				: undefined,
	}
}

export function validId(id: string) {
	return /^[1-9][0-9]*$/.test(id) && Number(id) <= 2_147_483_647
}

export function catalogPageCount(total: number | string, size: number | string) {
	return Math.min(maxPage, Math.max(1, Math.ceil(Number(total) / Number(size))))
}

export function canEditDictionary(account: Account | null, ownerId: number | string) {
	return (
		account?.role === 'Admin' || (account?.role === 'Teacher' && account.id === Number(ownerId))
	)
}

export function catalogQuery<T>(path: string, params?: URLSearchParams) {
	const search = params?.toString()
	return queryOptions({
		queryKey: [...catalogKey, path, search],
		queryFn: ({ signal }) =>
			withAccessToken(async (token) =>
				requireData(
					await request<T>(search ? `${path}?${search}` : path, { token, signal }),
				),
			),
		retry: false,
	})
}

export function pagedQuery<T>(path: string, search: EntrySearch) {
	const params = new URLSearchParams({
		page: String(search.page ?? 1),
		pageSize: String(search.pageSize ?? pageSize),
	})
	if (search.q) {
		params.set('search', search.q)
	}
	if (search.type) {
		params.set('type', search.type === 'word' ? '0' : '1')
	}
	return catalogQuery<T>(path, params)
}

export function saveName(path: string, name: string, editing: boolean) {
	return withAccessToken((token) =>
		request(path, { token, method: editing ? 'PATCH' : 'POST', body: { name } }),
	)
}

export function saveEntry(path: string, body: Schema['CreateEntryRequest'], editing: boolean) {
	return withAccessToken((token) =>
		request(path, { token, method: editing ? 'PATCH' : 'POST', body }),
	)
}

export function removeResource(path: string) {
	return withAccessToken((token) => request(path, { token, method: 'DELETE' }))
}

export function uploadCover(path: string, image: File) {
	const body = new FormData()
	body.append('BackgroundImage', image)
	return withAccessToken((token) =>
		request(`${path}/background-image`, { token, method: 'PUT', body }),
	)
}
