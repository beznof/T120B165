import { Toast } from '@heroui/react'
import { Info, X } from 'lucide-react'
import { type ReactNode, useState, useSyncExternalStore } from 'react'
import { SignInScreen } from '../../features/auth/SignInScreen'
import {
	type Account,
	getAccount,
	logout,
	onSessionChange,
	signedIn,
} from '../../features/auth/session'
import { Icon } from '../icons/Icon'
import { AppShell, type ColorMode } from './AppShell'

export type AppContext = {
	mode: ColorMode
	account: Account | null
	authenticated: boolean
	notify: (message: string) => void
}

export function AppFrame({
	requireSignIn = true,
	children,
}: {
	requireSignIn?: boolean
	children: (context: AppContext) => ReactNode
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
		<AppShell authenticated={authenticated} account={account} onSignOut={signOut}>
			{(mode) => (
				<>
					{requireSignIn && !authenticated ? (
						<SignInScreen title="Sign in to browse" notice={notice} />
					) : (
						children({ mode, account, authenticated, notify })
					)}
					<Toast.Provider queue={notifications} width="min(390px, calc(100vw - 32px))">
						{({ toast }) => (
							<Toast toast={toast} className="bloom-theme app-toast" data-mode={mode}>
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
		</AppShell>
	)
}

export function PageHeader({ title, actions }: { title: string; actions?: ReactNode }) {
	return (
		<header className="page-header">
			<h1>{title}</h1>
			{actions && <div className="page-actions">{actions}</div>}
		</header>
	)
}
