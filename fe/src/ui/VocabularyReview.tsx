import { Button } from '@heroui/react'
import { ArrowLeft, ArrowRight, Copy } from 'lucide-react'
import { type ReactNode, type Ref, useId, useState } from 'react'
import { Icon } from './Icon'
import '@fontsource-variable/literata'
import './vocabulary-review.css'

type Answer = {
	translation: string
	pronunciation?: string | null
	notify: (message: string) => void
}

export function ReviewPrompt({
	entryKey,
	term,
	actions,
	ref,
	...answer
}: Answer & {
	entryKey: number | string
	term: ReactNode
	actions?: ReactNode
	ref?: Ref<HTMLElement>
}) {
	const title = useId()

	return (
		<article className="review-prompt" aria-labelledby={title} tabIndex={-1} ref={ref}>
			<div className="review-word">
				<h2 id={title} aria-live="polite">
					{term}
				</h2>
				{actions}
			</div>
			<ReviewAnswer key={entryKey} {...answer} />
		</article>
	)
}

function ReviewAnswer({ translation, pronunciation, notify }: Answer) {
	const [revealed, setRevealed] = useState(false)
	const answerId = useId()

	async function copy() {
		try {
			await navigator.clipboard.writeText(translation)
			notify('Translation copied.')
		} catch {
			notify('Could not copy. Select the translation to copy it.')
		}
	}

	return (
		<div className="review-answer">
			<div id={answerId} aria-live="polite">
				{revealed && (
					<dl className="review-meaning">
						<div>
							<dt>Translation</dt>
							<dd>
								<bdi>{translation}</bdi>
								<Button
									variant="ghost"
									isIconOnly
									aria-label="Copy translation"
									onPress={copy}
								>
									<Icon icon={Copy} size={17} />
								</Button>
							</dd>
						</div>
						{pronunciation && (
							<div>
								<dt>Pronunciation</dt>
								<dd>
									<bdi>{pronunciation}</bdi>
								</dd>
							</div>
						)}
					</dl>
				)}
			</div>
			<Button
				className="review-reveal"
				aria-expanded={revealed}
				aria-controls={answerId}
				onPress={() => setRevealed(!revealed)}
			>
				{revealed ? 'Hide translation' : 'Show translation'}
			</Button>
		</div>
	)
}

export function ReviewStepper({
	position,
	total,
	onPrevious,
	onNext,
}: {
	position: number
	total: number
	onPrevious?: () => void
	onNext?: () => void
}) {
	return (
		<nav className="review-stepper" aria-label="Browse entries in order">
			<Button variant="outline" isDisabled={!onPrevious} onPress={onPrevious}>
				<Icon icon={ArrowLeft} size={17} />
				Previous
			</Button>
			<span aria-live="polite">
				{position} of {total}
			</span>
			<Button variant="outline" isDisabled={!onNext} onPress={onNext}>
				Next
				<Icon icon={ArrowRight} size={17} />
			</Button>
		</nav>
	)
}

export function ReviewQueue({ tools, children }: { tools: ReactNode; children: ReactNode }) {
	const title = useId()

	return (
		<section className="review-queue" aria-labelledby={title}>
			<h2 id={title}>Entries to review</h2>
			<div className="review-queue-tools">{tools}</div>
			{children}
		</section>
	)
}

export function ReviewQueueEntry({
	active,
	onPress,
	children,
}: {
	active: boolean
	onPress: () => void
	children: ReactNode
}) {
	return (
		<li>
			<Button
				className="review-queue-entry"
				variant="ghost"
				aria-pressed={active}
				onPress={onPress}
			>
				<span className="review-queue-term line-clamp-2">{children}</span>
			</Button>
		</li>
	)
}

export type ReviewEntryType = 'word' | 'phrase'

const filters = [
	{ value: undefined, label: 'All entries' },
	{ value: 'word', label: 'Words' },
	{ value: 'phrase', label: 'Phrases' },
] as const

export function ReviewFilters({
	value,
	onChange,
}: {
	value?: ReviewEntryType
	onChange: (value?: ReviewEntryType) => void
}) {
	return (
		<fieldset className="review-filters" aria-label="Filter by entry type">
			{filters.map(({ value: filter, label }) => (
				<Button
					key={label}
					variant="ghost"
					aria-pressed={filter === value}
					onPress={() => onChange(filter)}
				>
					{label}
				</Button>
			))}
		</fieldset>
	)
}
