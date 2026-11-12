import {
	Button,
	Description,
	FieldError,
	Input,
	Label,
	Radio,
	RadioGroup,
	TextField,
} from '@heroui/react'
import { Eye, EyeOff } from 'lucide-react'
import { type FormEvent, type ReactNode, useEffect, useRef, useState } from 'react'
import { Icon } from '../../ui/Icon'
import { AuthLayout } from './AuthLayout'

export type AccountScreen = 'sign-in' | 'register'
export type AccountCredentials = { username: string; password: string; role: 0 | 1 }
type FormErrors = { username?: string; password?: string; form?: string }

const accountTypes = [
	{ value: '0', label: 'Learner', description: 'Browse dictionaries' },
	{ value: '1', label: 'Teacher', description: 'Create dictionaries' },
]

export function AuthForm({
	screen,
	title,
	notice,
	accountLink,
	onSubmit,
}: {
	screen: AccountScreen
	title?: string
	notice?: string
	accountLink: ReactNode
	onSubmit: (values: AccountCredentials, signal: AbortSignal) => Promise<FormErrors | undefined>
}) {
	const [passwordVisible, setPasswordVisible] = useState(false)
	const [errors, setErrors] = useState<FormErrors>({})
	const [pending, setPending] = useState(false)
	const submission = useRef<AbortController | null>(null)
	const registering = screen === 'register'
	const submitLabel = registering ? 'Create account' : 'Sign in'
	const pendingLabel = registering ? 'Creating account…' : 'Signing in…'

	useEffect(() => () => submission.current?.abort(), [])

	function clearError(field: 'username' | 'password') {
		if (errors[field] || errors.form) {
			setErrors((current) => ({ ...current, [field]: undefined, form: undefined }))
		}
	}

	async function submit(event: FormEvent<HTMLFormElement>) {
		event.preventDefault()
		if (submission.current) {
			return
		}

		const form = new FormData(event.currentTarget)
		const username = String(form.get('username') ?? '').trim()
		const enteredPassword = String(form.get('password') ?? '')
		const password = registering ? enteredPassword.trim() : enteredPassword
		const nextErrors: FormErrors = {}
		if (!username) {
			nextErrors.username = 'Enter a username.'
		} else if (registering && (username.length < 5 || username.length > 50)) {
			nextErrors.username = 'Use 5–50 characters.'
		}
		if (!password) {
			nextErrors.password = 'Enter a password.'
		} else if (registering && (password.length < 8 || password.length > 64)) {
			nextErrors.password = 'Use 8–64 characters.'
		}
		setErrors(nextErrors)
		if (nextErrors.username || nextErrors.password) {
			return
		}

		const controller = new AbortController()
		submission.current = controller
		setPending(true)
		try {
			const role = form.get('role') === '1' ? 1 : 0
			const failure = await onSubmit({ username, password, role }, controller.signal)
			if (failure && !controller.signal.aborted) {
				setErrors(failure)
			}
		} catch {
			if (!controller.signal.aborted) {
				setErrors({ form: 'Something went wrong. Try again.' })
			}
		} finally {
			submission.current = null
			if (!controller.signal.aborted) {
				setPending(false)
			}
		}
	}

	return (
		<AuthLayout
			title={title ?? (registering ? 'Create an account' : 'Sign in')}
			footer={
				<>
					<span>{registering ? 'Already have an account?' : 'New to Saitynai?'}</span>
					{accountLink}
				</>
			}
		>
			<form className="auth-form" noValidate onSubmit={submit}>
				{notice && <p role="status">{notice}</p>}
				<TextField
					name="username"
					isRequired
					isReadOnly={pending}
					isInvalid={Boolean(errors.username)}
					validationBehavior="aria"
					onChange={() => clearError('username')}
				>
					<Label>Username</Label>
					<Input autoComplete="username" autoCapitalize="none" spellCheck={false} />
					<FieldError>{errors.username}</FieldError>
				</TextField>
				<TextField
					name="password"
					isRequired
					isReadOnly={pending}
					isInvalid={Boolean(errors.password)}
					validationBehavior="aria"
					onChange={() => clearError('password')}
				>
					<Label>Password</Label>
					<div className="auth-password">
						<Input
							type={passwordVisible ? 'text' : 'password'}
							autoComplete={registering ? 'new-password' : 'current-password'}
							autoCapitalize="none"
							spellCheck={false}
						/>
						<Button
							type="button"
							variant="ghost"
							isIconOnly
							aria-label={passwordVisible ? 'Hide password' : 'Show password'}
							onPress={() => setPasswordVisible((visible) => !visible)}
						>
							<Icon icon={passwordVisible ? EyeOff : Eye} size={18} />
						</Button>
					</div>
					<FieldError>{errors.password}</FieldError>
				</TextField>
				{registering && (
					<RadioGroup
						className="auth-types"
						name="role"
						defaultValue="0"
						orientation="horizontal"
						isReadOnly={pending}
					>
						<Label>Account type</Label>
						<div className="auth-role-grid">
							{accountTypes.map(({ value, label, description }) => (
								<Radio key={value} value={value} className="auth-role">
									<Radio.Content>
										<Radio.Control>
											<Radio.Indicator />
										</Radio.Control>
										<div>
											<Label>{label}</Label>
											<Description>{description}</Description>
										</div>
									</Radio.Content>
								</Radio>
							))}
						</div>
					</RadioGroup>
				)}
				{errors.form && (
					<p className="form-error" role="alert">
						{errors.form}
					</p>
				)}
				<Button type="submit" isPending={pending}>
					{pending ? pendingLabel : submitLabel}
				</Button>
			</form>
		</AuthLayout>
	)
}
