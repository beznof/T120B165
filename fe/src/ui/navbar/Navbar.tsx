import { Button } from '@heroui/react'
import { Link } from '@tanstack/react-router'
import { House, LogOut, Menu, Moon, Sun, X } from 'lucide-react'
import { useEffect, useRef, useState } from 'react'
import type { Account } from '../../features/auth/session'
import { Icon } from '../icons/Icon'
import type { ColorMode } from '../layout/AppShell'
import './navbar.css'

type NavbarProps = {
	mode: ColorMode
	authenticated: boolean
	account: Account | null
	onToggleMode: () => void
	onSignOut: () => Promise<void>
}

export function Navbar({ mode, authenticated, account, onToggleMode, onSignOut }: NavbarProps) {
	const [menuOpen, setMenuOpen] = useState(false)
	const [signingOut, setSigningOut] = useState(false)
	const menuToggle = useRef<HTMLButtonElement>(null)

	useEffect(() => {
		if (!menuOpen) {
			return
		}

		function closeOnEscape(event: KeyboardEvent) {
			if (event.key === 'Escape') {
				setMenuOpen(false)
				menuToggle.current?.focus()
			}
		}

		window.addEventListener('keydown', closeOnEscape)

		return () => window.removeEventListener('keydown', closeOnEscape)
	}, [menuOpen])

	async function signOut() {
		setSigningOut(true)

		try {
			await onSignOut()
			setMenuOpen(false)
		} finally {
			setSigningOut(false)
		}
	}

	return (
		<header className="navbar">
			<div className="app-container navbar-inner">
				<Link className="navbar-home" to="/" onClick={() => setMenuOpen(false)}>
					<span className="navbar-home-mark">
						<Icon icon={House} size={21} />
					</span>
					Home
				</Link>
				<div className="navbar-tools">
					<Button
						className="navbar-tool"
						variant="ghost"
						isIconOnly
						aria-label={
							mode === 'light' ? 'Switch to dark mode' : 'Switch to light mode'
						}
						onPress={onToggleMode}
					>
						<Icon icon={mode === 'light' ? Moon : Sun} />
					</Button>
					<Button
						ref={menuToggle}
						className="navbar-tool navbar-toggle"
						variant="ghost"
						isIconOnly
						aria-label={menuOpen ? 'Close menu' : 'Open menu'}
						aria-expanded={menuOpen}
						aria-controls="navbar-account-menu"
						onPress={() => setMenuOpen(!menuOpen)}
					>
						<Icon icon={menuOpen ? X : Menu} />
					</Button>
				</div>
				<nav
					id="navbar-account-menu"
					className="navbar-account"
					data-expanded={menuOpen}
					aria-label="Account"
				>
					{authenticated ? (
						<>
							{account && (
								<span className="navbar-identity">
									<bdi className="navbar-username" title={account.username}>
										{account.username}
									</bdi>
									<span className="navbar-role">{account.role}</span>
								</span>
							)}
							<Button
								className="navbar-signout"
								variant="ghost"
								isPending={signingOut}
								onPress={signOut}
							>
								Sign out <Icon icon={LogOut} size={18} />
							</Button>
						</>
					) : (
						<>
							<Link
								className="navbar-link"
								to="/login"
								onClick={() => setMenuOpen(false)}
							>
								Sign in
							</Link>
							<Link
								className="navbar-join button--lime"
								to="/register"
								onClick={() => setMenuOpen(false)}
							>
								Register
							</Link>
						</>
					)}
				</nav>
			</div>
		</header>
	)
}
