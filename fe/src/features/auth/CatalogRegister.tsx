import { Button, Input, Label, TextField } from '@heroui/react'
import { Link } from '@tanstack/react-router'
import { Check } from 'lucide-react'
import { type FormEvent, useState } from 'react'
import { ApiError, request, type Schema } from '../../api/client'
import { ChoiceGroup } from '../../ui/ChoiceGroup'
import { Icon } from '../../ui/Icon'
import { CatalogStatus } from '../catalog/CatalogBrowse'
import { CatalogAuthLayout } from './CatalogAuthLayout'

export function CatalogRegister() {
	const [error, setError] = useState('')
	const [pending, setPending] = useState(false)
	const [created, setCreated] = useState(false)

	async function submit(event: FormEvent<HTMLFormElement>) {
		event.preventDefault()
		if (pending) {
			return
		}
		const form = new FormData(event.currentTarget)
		const username = String(form.get('username') ?? '').trim()
		const password = String(form.get('password') ?? '').trim()
		const role = form.get('role') === '1' ? 1 : 0
		if (username.length < 5 || password.length < 8) {
			setError('Use at least 5 characters for your username and 8 for your password.')
			return
		}
		setError('')
		setPending(true)
		try {
			await request<Schema['RegisterOutput']>('/auth/register', {
				body: { username, password, role } satisfies Schema['RegisterRequest'],
			})
			setCreated(true)
		} catch (failure) {
			setError(
				failure instanceof ApiError && failure.status < 500
					? failure.message
					: 'The language service could not be reached. Try again.',
			)
		} finally {
			setPending(false)
		}
	}

	if (created) {
		return (
			<CatalogStatus
				heading="h1"
				icon={<Icon icon={Check} size={22} />}
				title="Account created"
			>
				<p>You can now sign in to browse the catalog.</p>
				<Link className="catalog-text-link" to="/login">
					Sign in
				</Link>
			</CatalogStatus>
		)
	}

	return (
		<CatalogAuthLayout
			title="Create an account"
			description="Learners can browse dictionaries. Teachers can also create dictionaries and manage their entries."
			accountLink={<Link to="/login">Already have an account? Sign in</Link>}
		>
			<form className="catalog-auth-form" onSubmit={submit}>
				<TextField name="username" isRequired>
					<Label>Username</Label>
					<Input autoComplete="username" minLength={5} maxLength={50} />
				</TextField>
				<TextField name="password" isRequired>
					<Label>Password</Label>
					<Input
						type="password"
						autoComplete="new-password"
						minLength={8}
						maxLength={64}
					/>
				</TextField>
				<ChoiceGroup
					name="role"
					label="Account type"
					defaultValue="0"
					choices={[
						{ value: '0', label: 'Learner' },
						{ value: '1', label: 'Teacher' },
					]}
				/>
				{error && (
					<p className="catalog-form-error" role="alert">
						{error}
					</p>
				)}
				<Button type="submit" isPending={pending}>
					Create account
				</Button>
			</form>
		</CatalogAuthLayout>
	)
}
