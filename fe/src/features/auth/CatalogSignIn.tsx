import { Button, Input, Label, Spinner, TextField } from '@heroui/react'
import { Link } from '@tanstack/react-router'
import { ArrowRight } from 'lucide-react'
import { type FormEvent, useState } from 'react'
import { ApiError } from '../../api/client'
import { Icon } from '../../ui/Icon'
import { CatalogAuthLayout } from './CatalogAuthLayout'
import { login } from './session'

export function CatalogSignIn({ notice, title = 'Sign in' }: { notice: string; title?: string }) {
	const [error, setError] = useState('')
	const [pending, setPending] = useState(false)

	async function submit(event: FormEvent<HTMLFormElement>) {
		event.preventDefault()

		if (pending) {
			return
		}

		const form = new FormData(event.currentTarget)
		const username = String(form.get('username') ?? '').trim()
		const password = String(form.get('password') ?? '')
		if (!username || !password) {
			setError('Enter your username and password.')
			return
		}

		setError('')
		setPending(true)

		try {
			await login({ username, password })
		} catch (failure) {
			setError(
				failure instanceof ApiError && failure.status === 401
					? 'Check your username and password and try again.'
					: 'The language service could not be reached. Try again in a moment.',
			)
		} finally {
			setPending(false)
		}
	}

	return (
		<CatalogAuthLayout
			title={title}
			accountLink={<Link to="/register">Create an account</Link>}
		>
			<form className="catalog-auth-form" onSubmit={submit}>
				{notice && <p role="status">{notice}</p>}
				<TextField name="username" isRequired>
					<Label>Username</Label>
					<Input id="catalog-username" autoComplete="username" />
				</TextField>
				<TextField name="password" isRequired>
					<Label>Password</Label>
					<Input type="password" autoComplete="current-password" />
				</TextField>
				{error && (
					<p className="catalog-form-error" role="alert">
						{error}
					</p>
				)}
				<Button type="submit" isPending={pending}>
					{pending ? (
						<>
							<Spinner size="sm" color="current" /> Signing in…
						</>
					) : (
						<>
							Sign in <Icon icon={ArrowRight} />
						</>
					)}
				</Button>
			</form>
		</CatalogAuthLayout>
	)
}
