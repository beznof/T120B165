import { createFileRoute, Navigate } from '@tanstack/react-router'
import { RegisterScreen } from '../features/auth/RegisterScreen'
import { AppFrame } from '../ui/layout/AppFrame'

export const Route = createFileRoute('/register')({
	component: () => (
		<AppFrame requireSignIn={false}>
			{({ authenticated }) =>
				authenticated ? <Navigate to="/" replace /> : <RegisterScreen />
			}
		</AppFrame>
	),
})
