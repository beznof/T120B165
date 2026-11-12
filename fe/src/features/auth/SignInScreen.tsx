import { Link } from '@tanstack/react-router'
import { ApiError } from '../../api/client'
import { type AccountCredentials, AuthForm } from './AuthForm'
import { login } from './session'

export function SignInScreen({ notice, title = 'Sign in' }: { notice: string; title?: string }) {
	async function submit({ username, password }: AccountCredentials) {
		try {
			await login({ username, password })
		} catch (failure) {
			return {
				form:
					failure instanceof ApiError && failure.status === 401
						? 'Check your username and password and try again.'
						: 'The language service could not be reached. Try again in a moment.',
			}
		}
	}

	return (
		<AuthForm
			screen="sign-in"
			title={title}
			notice={notice}
			accountLink={<Link to="/register">Create an account</Link>}
			onSubmit={submit}
		/>
	)
}
