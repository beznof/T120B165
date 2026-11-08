import { ToggleButton, ToggleButtonGroup } from '@heroui/react'
import { LayoutGrid, List } from 'lucide-react'
import { useId, useState } from 'react'
import { Icon } from '../../ui/Icon'

type Display = 'grid' | 'list'

const preferenceKey = 'catalog-collection-display'

function readDisplay(): Display {
	try {
		return localStorage.getItem(preferenceKey) === 'list' ? 'list' : 'grid'
	} catch {
		return 'grid'
	}
}

export function useCollectionDisplay() {
	const [display, setDisplay] = useState(readDisplay)

	function changeDisplay(next: Display) {
		setDisplay(next)

		try {
			localStorage.setItem(preferenceKey, next)
		} catch {
			// Keep the selection for the current page when storage is unavailable.
		}
	}

	return [display, changeDisplay] as const
}

export function CollectionDisplayToggle({
	value,
	onChange,
}: {
	value: Display
	onChange: (display: Display) => void
}) {
	const label = useId()

	return (
		<div className="catalog-display">
			<span className="label" id={label}>
				Display
			</span>
			<ToggleButtonGroup
				aria-labelledby={label}
				selectionMode="single"
				disallowEmptySelection
				selectedKeys={[value]}
				onSelectionChange={([next]) => {
					if (next === 'grid' || next === 'list') {
						onChange(next)
					}
				}}
			>
				<ToggleButton id="grid" isIconOnly aria-label="Grid view">
					<Icon icon={LayoutGrid} size={18} />
				</ToggleButton>
				<ToggleButton id="list" isIconOnly aria-label="List view">
					<Icon icon={List} size={18} />
				</ToggleButton>
			</ToggleButtonGroup>
		</div>
	)
}
