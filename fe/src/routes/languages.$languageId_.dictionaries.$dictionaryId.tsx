import { createFileRoute, notFound } from '@tanstack/react-router'
import { pageSize, parseSearch, validId } from '../features/catalog/api'
import { CatalogFrame } from '../features/catalog/CatalogFrame'
import { EntriesPage, type EntryReviewSearch } from '../features/languages/EntriesPage'

export const Route = createFileRoute('/languages/$languageId_/dictionaries/$dictionaryId')({
	validateSearch: (search: Record<string, unknown>): EntryReviewSearch => {
		const catalog = parseSearch(search)
		const { type, position } = search
		return {
			...catalog,
			type: type === 'word' || type === 'phrase' ? type : undefined,
			position:
				typeof position === 'number' &&
				Number.isInteger(position) &&
				position > 1 &&
				position <= (catalog.pageSize ?? pageSize)
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

	return (
		<CatalogFrame>
			{(context) => (
				<EntriesPage
					key={`${languageId}/${dictionaryId}`}
					context={context}
					languageId={languageId}
					dictionaryId={dictionaryId}
					search={search}
					onSearch={(q) =>
						update({ q: q || undefined, page: undefined, position: undefined })
					}
					onFilter={(type) => update({ type, page: undefined, position: undefined })}
					onClear={() =>
						update({
							q: undefined,
							type: undefined,
							page: undefined,
							position: undefined,
						})
					}
					onPageSizeChange={(size) =>
						update({ pageSize: size, page: undefined, position: undefined })
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
			)}
		</CatalogFrame>
	)
}
