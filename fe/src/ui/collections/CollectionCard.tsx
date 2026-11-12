import { Card } from '@heroui/react'
import type { ReactNode } from 'react'
import { CoverImage } from './CoverImage'
import './collection-card.css'

export function CollectionCard({
	name,
	image,
	summary,
	actions,
	renderLink,
}: {
	name: string
	image?: string | null
	summary: string
	actions?: ReactNode
	renderLink: (contents: ReactNode) => ReactNode
}) {
	return (
		<Card className="collection-card" data-editable={Boolean(actions)}>
			{renderLink(
				<>
					<CoverImage name={name} image={image} />
					<span className="collection-caption">
						<span className="collection-caption-text">
							<strong className="line-clamp-2" title={name}>
								<bdi>{name}</bdi>
							</strong>
							<small title={summary}>{summary}</small>
						</span>
					</span>
				</>,
			)}
			{actions && <div className="collection-actions">{actions}</div>}
		</Card>
	)
}
