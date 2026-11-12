import { createFileRoute } from '@tanstack/react-router'
import { parseSearch } from '../features/browse/api'
import { useBrowseContext } from '../features/browse/browse-context'
import { LanguagesPage } from '../features/languages/LanguagesPage'

export const Route = createFileRoute('/_browse/')({
	validateSearch: parseSearch,
	component: LanguagesRoute,
})

function LanguagesRoute() {
	const search = Route.useSearch()
	const navigate = Route.useNavigate()
	const context = useBrowseContext()
	return (
		<LanguagesPage
			context={context}
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
