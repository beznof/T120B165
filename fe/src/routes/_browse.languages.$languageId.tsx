import { createFileRoute, notFound } from '@tanstack/react-router'
import { parseSearch, validId } from '../features/browse/api'
import { useBrowseContext } from '../features/browse/browse-context'
import { DictionariesPage } from '../features/languages/DictionariesPage'

export const Route = createFileRoute('/_browse/languages/$languageId')({
	validateSearch: parseSearch,
	beforeLoad: ({ params }) => {
		if (!validId(params.languageId)) {
			throw notFound()
		}
	},
	component: DictionariesRoute,
})

function DictionariesRoute() {
	const { languageId } = Route.useParams()
	const search = Route.useSearch()
	const navigate = Route.useNavigate()
	const context = useBrowseContext()
	return (
		<DictionariesPage
			key={languageId}
			context={context}
			languageId={languageId}
			search={search}
			onSearch={(q) =>
				void navigate({ search: { ...search, q: q || undefined, page: undefined } })
			}
			onPageSizeChange={(pageSize) =>
				void navigate({ search: { ...search, pageSize, page: undefined } })
			}
			onPageChange={(page) =>
				void navigate({
					search: { ...search, page: page === 1 ? undefined : page },
				})
			}
		/>
	)
}
