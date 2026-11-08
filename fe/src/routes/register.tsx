import { createFileRoute, Navigate } from '@tanstack/react-router'
import { CatalogRegister } from '../features/auth/CatalogRegister'
import { CatalogFrame } from '../features/catalog/CatalogFrame'

export const Route = createFileRoute('/register')({
	component: () => (
		<CatalogFrame requireSignIn={false}>
			{({ authenticated }) =>
				authenticated ? <Navigate to="/" replace /> : <CatalogRegister />
			}
		</CatalogFrame>
	),
})
