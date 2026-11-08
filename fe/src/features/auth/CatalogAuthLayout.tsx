import { type ReactNode, useId } from 'react'

export function CatalogAuthLayout({
	title,
	accountLink,
	description,
	children,
}: {
	title: string
	accountLink: ReactNode
	description?: string
	children: ReactNode
}) {
	const heading = useId()

	return (
		<section className="catalog-auth" aria-labelledby={heading}>
			<header className="catalog-auth-panel">
				<h1 id={heading}>{title}</h1>
				{description && <p>{description}</p>}
				{accountLink}
			</header>
			{children}
		</section>
	)
}
