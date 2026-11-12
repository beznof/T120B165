import { createFileRoute, Navigate } from '@tanstack/react-router'
import { SignInScreen } from '../features/auth/SignInScreen'
import { AppFrame } from '../ui/AppFrame'

export const Route = createFileRoute('/login')({
	component: () => (
		<AppFrame requireSignIn={false}>
			{({ authenticated }) =>
				authenticated ? <Navigate to="/" replace /> : <SignInScreen notice="" />
			}
		</AppFrame>
	),
})
