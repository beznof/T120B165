import { createFileRoute, Navigate } from '@tanstack/react-router'
import { CatalogSignIn } from '../features/auth/CatalogSignIn'
import { CatalogFrame } from '../features/catalog/CatalogFrame'

export const Route = createFileRoute('/login')({
	component: () => (
		<CatalogFrame requireSignIn={false}>
			{({ authenticated }) =>
				authenticated ? <Navigate to="/" replace /> : <CatalogSignIn notice="" />
			}
		</CatalogFrame>
	),
})
