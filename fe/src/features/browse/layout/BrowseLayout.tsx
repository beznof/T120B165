import { Button, Tooltip } from '@heroui/react'
import { useParams } from '@tanstack/react-router'
import { PanelLeft } from 'lucide-react'
import { type ReactNode, useId, useState } from 'react'
import { Icon } from '../../../ui/icons/Icon'
import type { AppContext } from '../../../ui/layout/AppFrame'
import { BrowseContext } from './browse-context'
import { LibrarySidebar } from './LibrarySidebar'
import './browse-layout.css'

const desktop = '(min-width: 900px)'

export function BrowseLayout({ context, children }: { context: AppContext; children: ReactNode }) {
	const [open, setOpen] = useState(() => window.matchMedia(desktop).matches)
	const panelId = useId()
	const libraryAction = open ? 'Hide Library' : 'Show Library'
	const { languageId, dictionaryId } = useParams({ strict: false })

	function closeOnMobile() {
		if (!window.matchMedia(desktop).matches) {
			setOpen(false)
		}
	}

	return (
		<BrowseContext.Provider value={context}>
			<div className="browse-layout-toolbar">
				<Tooltip>
					<Button
						variant="ghost"
						isIconOnly
						aria-label={libraryAction}
						aria-controls={panelId}
						aria-expanded={open}
						onPress={() => setOpen(!open)}
					>
						<Icon icon={PanelLeft} size={18} />
					</Button>
					<Tooltip.Content
						placement="right"
						className="bloom-theme"
						data-mode={context.mode}
					>
						{libraryAction}
					</Tooltip.Content>
				</Tooltip>
			</div>
			<div className="browse-layout" data-library-open={open}>
				<aside
					className="library-sidebar"
					id={panelId}
					hidden={!open}
					aria-label="Library shortcuts"
				>
					<LibrarySidebar
						visible={open}
						activeLanguage={languageId}
						activeDictionary={dictionaryId}
						onNavigate={closeOnMobile}
					/>
				</aside>
				<div className="browse-layout-content">{children}</div>
			</div>
		</BrowseContext.Provider>
	)
}
