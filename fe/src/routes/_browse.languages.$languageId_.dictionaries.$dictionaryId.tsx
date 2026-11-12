import { createFileRoute, notFound } from '@tanstack/react-router'
import { entryPageSize, parseSearch, validId } from '../features/browse/api'
import { EntriesPage, type EntryReviewSearch } from '../features/browse/entries/EntriesPage'
import { useBrowseContext } from '../features/browse/layout/browse-context'

export const Route = createFileRoute('/_browse/languages/$languageId_/dictionaries/$dictionaryId')({
	validateSearch: (search: Record<string, unknown>): EntryReviewSearch => {
		const { q, page } = parseSearch(search)
		const { type, position } = search
		return {
			q,
			page,
			type: type === 'word' || type === 'phrase' ? type : undefined,
			position:
				typeof position === 'number' &&
				Number.isInteger(position) &&
				position > 1 &&
				position <= entryPageSize
					? position
					: undefined,
		}
	},
	beforeLoad: ({ params }) => {
		if (!validId(params.languageId) || !validId(params.dictionaryId)) {
			throw notFound()
		}
	},
	component: EntriesRoute,
})

function EntriesRoute() {
	const { languageId, dictionaryId } = Route.useParams()
	const search = Route.useSearch()
	const navigate = Route.useNavigate()

	function update(next: EntryReviewSearch, replace = false) {
		void navigate({
			search: (current) => ({ ...current, ...next }),
			resetScroll: false,
			replace,
		})
	}

	const context = useBrowseContext()
	return (
		<EntriesPage
			key={`${languageId}/${dictionaryId}`}
			context={context}
			languageId={languageId}
			dictionaryId={dictionaryId}
			search={search}
			onSearch={(q) => update({ q: q || undefined, page: undefined, position: undefined })}
			onFilter={(type) => update({ type, page: undefined, position: undefined })}
			onClear={() =>
				update({
					q: undefined,
					type: undefined,
					page: undefined,
					position: undefined,
				})
			}
			onPageChange={(page) =>
				update({ page: page === 1 ? undefined : page, position: undefined })
			}
			onPositionChange={(page, position) =>
				update(
					{
						page: page === 1 ? undefined : page,
						position: position === 1 ? undefined : position,
					},
					page === (search.page ?? 1),
				)
			}
		/>
	)
}
