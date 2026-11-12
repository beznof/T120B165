import { useState } from 'react'
import './cover-image.css'

export function CoverImage({
	name,
	image,
	thumbnail = false,
}: {
	name: string
	image?: string | null
	thumbnail?: boolean
}) {
	const [failedImage, setFailedImage] = useState<string>()
	const source = image && image !== failedImage ? image : undefined
	const initial = name.trim().match(/^.\p{M}*/u)?.[0]
	const fallback = thumbnail ? (initial?.toUpperCase() ?? '') : name

	return (
		<span
			className="cover-image"
			data-fallback={!source}
			data-thumbnail={thumbnail}
			aria-hidden="true"
		>
			{source ? (
				<img src={source} alt="" loading="lazy" onError={() => setFailedImage(source)} />
			) : (
				<span className="cover-image-name" data-long={fallback.length > 24} dir="auto">
					{fallback}
				</span>
			)}
		</span>
	)
}
