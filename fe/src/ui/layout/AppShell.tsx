import { type ReactNode, useState } from 'react'
import type { Account } from '../../features/auth/session'
import { Icon } from '../icons/Icon'
import { Navbar } from '../navbar/Navbar'
import '@fontsource-variable/atkinson-hyperlegible-next'
import '../../styles/theme.css'
import './app-layout.css'
import { Github } from '../icons/github-icon'

export type ColorMode = 'light' | 'dark'

const modeKey = 'saitynai-color-mode'

function initialMode(): ColorMode {
	try {
		const stored = localStorage.getItem(modeKey)

		if (stored === 'light' || stored === 'dark') {
			return stored
		}
	} catch {
		// Use the system preference when storage is unavailable.
	}

	return window.matchMedia('(prefers-color-scheme: dark)').matches ? 'dark' : 'light'
}

export function AppShell({
	authenticated,
	account,
	onSignOut,
	children,
}: {
	authenticated: boolean
	account: Account | null
	onSignOut: () => Promise<void>
	children: (mode: ColorMode) => ReactNode
}) {
	const [mode, setMode] = useState(initialMode)

	function toggleMode() {
		const next = mode === 'light' ? 'dark' : 'light'
		setMode(next)

		try {
			localStorage.setItem(modeKey, next)
		} catch {
			// The selected mode still applies for this visit.
		}
	}

	return (
		<div className="bloom-theme app-shell" data-mode={mode} lang="en">
			<a className="app-skip-link" href="#app-content">
				Skip to content
			</a>
			<Navbar
				mode={mode}
				onToggleMode={toggleMode}
				authenticated={authenticated}
				account={account}
				onSignOut={onSignOut}
			/>
			<main id="app-content" className="app-container app-main" tabIndex={-1}>
				{children(mode)}
			</main>
			<footer className="app-footer">
				<div className="app-container app-footer-inner">
					<p>Look up words and phrases in other languages.</p>
					<a href="https://github.com/beznof/T120B165" target="_blank" rel="noreferrer">
						<Icon icon={Github} size={18} /> GitHub
					</a>
				</div>
			</footer>
		</div>
	)
}
