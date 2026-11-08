import { Card } from '@heroui/react'
import { ArrowRight } from 'lucide-react'
import type { ReactNode } from 'react'
import { CatalogCover } from './CatalogCover'
import { Icon } from './Icon'
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
					<CatalogCover name={name} image={image} />
					<span className="collection-caption">
						<span className="collection-caption-text">
							<strong className="line-clamp-2" title={name}>
								<bdi>{name}</bdi>
							</strong>
							<small title={summary}>{summary}</small>
						</span>
						<Icon icon={ArrowRight} size={18} />
					</span>
				</>,
			)}
			{actions && <div className="collection-actions">{actions}</div>}
		</Card>
	)
}
