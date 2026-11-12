import type { QueryClient } from '@tanstack/react-query'
import { createRootRouteWithContext, Outlet } from '@tanstack/react-router'
import { AppFrame } from '../ui/layout/AppFrame'

export const Route = createRootRouteWithContext<{ queryClient: QueryClient }>()({
	component: Outlet,
	notFoundComponent: () => (
		<AppFrame requireSignIn={false}>
			{() => (
				<h1 className="not-found-title">
					Page <span>not found</span>
				</h1>
			)}
		</AppFrame>
	),
})
