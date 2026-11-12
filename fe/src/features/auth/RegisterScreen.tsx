import { Link } from '@tanstack/react-router'
import { useState } from 'react'
import { ApiError, request, type Schema } from '../../api/client'
import { type AccountCredentials, AuthForm } from './AuthForm'
import { AuthSuccess } from './AuthLayout'

export function RegisterScreen() {
	const [created, setCreated] = useState(false)

	async function submit(credentials: AccountCredentials, signal: AbortSignal) {
		try {
			await request<Schema['RegisterOutput']>('/auth/register', {
				body: credentials satisfies Schema['RegisterRequest'],
				signal,
			})
			if (!signal.aborted) {
				setCreated(true)
			}
		} catch (failure) {
			return {
				form:
					failure instanceof ApiError && failure.status < 500
						? failure.message
						: 'The language service could not be reached. Try again.',
			}
		}
	}

	if (created) {
		return (
			<AuthSuccess
				title="Account created"
				description="You can now sign in to browse the library."
				action={
					<Link className="button button--primary" to="/login">
						Sign in
					</Link>
				}
			/>
		)
	}

	return (
		<AuthForm
			screen="register"
			accountLink={<Link to="/login">Sign in</Link>}
			onSubmit={submit}
		/>
	)
}
