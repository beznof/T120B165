import { createContext, useContext } from 'react'
import type { AppContext } from '../../../ui/layout/AppFrame'

export const BrowseContext = createContext<AppContext | null>(null)

export function useBrowseContext() {
	const context = useContext(BrowseContext)
	if (!context) {
		throw new Error('Browse pages must be rendered inside BrowseLayout.')
	}
	return context
}
