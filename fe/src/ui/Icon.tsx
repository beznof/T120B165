import type { LucideIcon, LucideProps } from 'lucide-react'

type IconProps = LucideProps & { icon: LucideIcon }

export function Icon({
	icon: Glyph,
	size = 20,
	strokeWidth = 1.75,
	className,
	...props
}: IconProps) {
	const labelled = Boolean(props['aria-label'] || props['aria-labelledby'])

	return (
		<Glyph
			size={size}
			strokeWidth={strokeWidth}
			className={className ? `app-icon ${className}` : 'app-icon'}
			aria-hidden={labelled ? undefined : true}
			role={labelled ? 'img' : undefined}
			{...props}
		/>
	)
}
