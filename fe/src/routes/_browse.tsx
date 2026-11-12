import { createFileRoute, Outlet } from '@tanstack/react-router'
import { BrowseLayout } from '../features/browse/layout/BrowseLayout'
import { AppFrame } from '../ui/layout/AppFrame'

export const Route = createFileRoute('/_browse')({
	component: () => (
		<AppFrame>
			{(context) => (
				<BrowseLayout context={context}>
					<Outlet />
				</BrowseLayout>
			)}
		</AppFrame>
	),
})
