import { Check } from 'lucide-react'
import { type ReactNode, useId } from 'react'
import { Icon } from '../../ui/icons/Icon'
import './auth.css'

export function AuthLayout({
	title,
	footer,
	complete = false,
	children,
}: {
	title: string
	footer?: ReactNode
	complete?: boolean
	children: ReactNode
}) {
	const heading = useId()

	return (
		<section className="auth" aria-labelledby={heading}>
			<div className="auth-card">
				{complete && (
					<span className="auth-check">
						<Icon icon={Check} />
					</span>
				)}
				<header role={complete ? 'status' : undefined}>
					<h1 id={heading}>{title}</h1>
				</header>
				{children}
				{footer && <div className="auth-switch">{footer}</div>}
			</div>
		</section>
	)
}

export function AuthSuccess({
	title,
	description,
	action,
}: {
	title: string
	description?: string
	action: ReactNode
}) {
	return (
		<AuthLayout title={title} complete>
			<div className="auth-success">
				{description && <p>{description}</p>}
				{action}
			</div>
		</AuthLayout>
	)
}
