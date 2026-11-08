import { createFileRoute } from '@tanstack/react-router'
import { parseSearch } from '../features/catalog/api'
import { CatalogFrame } from '../features/catalog/CatalogFrame'
import { LanguagesPage } from '../features/languages/LanguagesPage'

export const Route = createFileRoute('/')({
	validateSearch: parseSearch,
	component: LanguagesRoute,
})

function LanguagesRoute() {
	const search = Route.useSearch()
	const navigate = Route.useNavigate()
	return (
		<CatalogFrame>
			{(context) => (
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
			)}
		</CatalogFrame>
	)
}
