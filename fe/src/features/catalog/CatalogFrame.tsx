import { Toast } from '@heroui/react'
import { Info, X } from 'lucide-react'
import { type ReactNode, useState, useSyncExternalStore } from 'react'
import { CatalogShell, type ColorMode } from '../../ui/CatalogShell'
import { Icon } from '../../ui/Icon'
import { CatalogSignIn } from '../auth/CatalogSignIn'
import { type Account, getAccount, logout, onSessionChange, signedIn } from '../auth/session'

export type CatalogContext = {
	mode: ColorMode
	account: Account | null
	authenticated: boolean
	notify: (message: string) => void
}

export function CatalogFrame({
	requireSignIn = true,
	children,
}: {
	requireSignIn?: boolean
	children: (context: CatalogContext) => ReactNode
}) {
	const authenticated = useSyncExternalStore(onSessionChange, signedIn)
	const account = useSyncExternalStore(onSessionChange, getAccount)
	const [notice, setNotice] = useState('')
	const [notifications] = useState(
		() => new Toast.Queue<{ title: string }>({ maxVisibleToasts: 2 }),
	)

	function notify(title: string) {
		notifications.add({ title }, { timeout: 4500 })
	}

	async function signOut() {
		setNotice('')
		try {
			await logout()
		} catch {
			setNotice('Signed out on this device. The service could not be reached.')
		}
	}

	return (
		<CatalogShell authenticated={authenticated} onSignOut={signOut}>
			{(mode) => (
				<>
					{requireSignIn && !authenticated ? (
						<CatalogSignIn title="Sign in to browse" notice={notice} />
					) : (
						children({ mode, account, authenticated, notify })
					)}
					<Toast.Provider queue={notifications} width="min(390px, calc(100vw - 32px))">
						{({ toast }) => (
							<Toast
								toast={toast}
								className="bloom-theme catalog-toast"
								data-mode={mode}
							>
								<Toast.Indicator>
									<Icon icon={Info} size={18} />
								</Toast.Indicator>
								<Toast.Content>
									<Toast.Title>{toast.content.title}</Toast.Title>
								</Toast.Content>
								<Toast.CloseButton aria-label="Dismiss notification">
									<Icon icon={X} size={18} />
								</Toast.CloseButton>
							</Toast>
						)}
					</Toast.Provider>
				</>
			)}
		</CatalogShell>
	)
}

export function CatalogPageHeader({ title, actions }: { title: string; actions?: ReactNode }) {
	return (
		<header className="catalog-page-header">
			<h1>{title}</h1>
			{actions && <div className="catalog-page-actions">{actions}</div>}
		</header>
	)
}
