import { useState } from 'react'
import './catalog-cover.css'

export function CatalogCover({ name, image }: { name: string; image?: string | null }) {
	const [failedImage, setFailedImage] = useState<string>()
	const source = image && image !== failedImage ? image : undefined

	return (
		<span className="catalog-cover" data-fallback={!source} aria-hidden="true">
			{source ? (
				<img src={source} alt="" loading="lazy" onError={() => setFailedImage(source)} />
			) : (
				<span className="catalog-cover-name" data-long={name.length > 24} dir="auto">
					{name}
				</span>
			)}
		</span>
	)
}
