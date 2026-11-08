import { Button } from '@heroui/react'
import { Link } from '@tanstack/react-router'
import { ArrowUpRight, House, LogOut, Menu, Moon, Sun, X } from 'lucide-react'
import { useEffect, useRef, useState } from 'react'
import type { ColorMode } from './CatalogShell'
import { Icon } from './Icon'
import './catalog-navbar.css'

type CatalogNavbarProps = {
	mode: ColorMode
	authenticated: boolean
	onToggleMode: () => void
	onSignOut: () => Promise<void>
}

export function CatalogNavbar({
	mode,
	authenticated,
	onToggleMode,
	onSignOut,
}: CatalogNavbarProps) {
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
		<header className="catalog-navbar">
			<div className="catalog-container catalog-navbar-inner">
				<Link className="catalog-navbar-home" to="/" onClick={() => setMenuOpen(false)}>
					<span className="catalog-navbar-home-mark">
						<Icon icon={House} size={21} />
					</span>
					Home
				</Link>
				<div className="catalog-navbar-tools">
					<Button
						className="catalog-navbar-tool"
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
						className="catalog-navbar-tool catalog-navbar-toggle"
						variant="ghost"
						isIconOnly
						aria-label={menuOpen ? 'Close menu' : 'Open menu'}
						aria-expanded={menuOpen}
						aria-controls="catalog-account-menu"
						onPress={() => setMenuOpen(!menuOpen)}
					>
						<Icon icon={menuOpen ? X : Menu} />
					</Button>
				</div>
				<nav
					id="catalog-account-menu"
					className="catalog-navbar-account"
					data-expanded={menuOpen}
					aria-label="Account"
				>
					{authenticated ? (
						<Button
							className="catalog-navbar-signout"
							variant="ghost"
							isPending={signingOut}
							onPress={signOut}
						>
							Sign out <Icon icon={LogOut} size={18} />
						</Button>
					) : (
						<>
							<Link
								className="catalog-navbar-link"
								to="/login"
								onClick={() => setMenuOpen(false)}
							>
								Sign in
							</Link>
							<Link
								className="catalog-navbar-join"
								to="/register"
								onClick={() => setMenuOpen(false)}
							>
								Register <Icon icon={ArrowUpRight} size={18} />
							</Link>
						</>
					)}
				</nav>
			</div>
		</header>
	)
}
