import type { QueryClient } from '@tanstack/react-query'
import { createRootRouteWithContext, Link, Outlet } from '@tanstack/react-router'
import { CatalogStatus } from '../features/catalog/CatalogBrowse'
import { CatalogFrame } from '../features/catalog/CatalogFrame'

export const Route = createRootRouteWithContext<{ queryClient: QueryClient }>()({
	component: Outlet,
	notFoundComponent: () => (
		<CatalogFrame requireSignIn={false}>
			{() => (
				<CatalogStatus heading="h1" title="Page not found">
					<p>The address may be incorrect or the page may have moved.</p>
					<Link className="catalog-text-link" to="/">
						Back to languages
					</Link>
				</CatalogStatus>
			)}
		</CatalogFrame>
	),
})
