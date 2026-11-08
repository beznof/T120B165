import { type ReactNode, useState } from 'react'
import { CatalogNavbar } from './CatalogNavbar'
import { Icon } from './Icon'
import '@fontsource-variable/atkinson-hyperlegible-next'
import './catalog.css'
import { Github } from './github-icon'

export type ColorMode = 'light' | 'dark'

const modeKey = 'catalog-color-mode'

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

export function CatalogShell({
	authenticated,
	onSignOut,
	children,
}: {
	authenticated: boolean
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
		<div className="bloom-theme catalog-shell" data-mode={mode} lang="en">
			<a className="catalog-skip-link" href="#catalog-content">
				Skip to content
			</a>
			<CatalogNavbar
				mode={mode}
				onToggleMode={toggleMode}
				authenticated={authenticated}
				onSignOut={onSignOut}
			/>
			<main id="catalog-content" className="catalog-container catalog-main" tabIndex={-1}>
				{children(mode)}
			</main>
			<footer className="catalog-footer">
				<div className="catalog-container catalog-footer-inner">
					<p>Look up words and phrases in other languages.</p>
					<a href="https://github.com/beznof/T120B165" target="_blank" rel="noreferrer">
						<Icon icon={Github} size={18} /> GitHub
					</a>
				</div>
			</footer>
		</div>
	)
}
