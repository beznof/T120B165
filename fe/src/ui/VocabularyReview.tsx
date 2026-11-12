import { Button } from '@heroui/react'
import { Copy } from 'lucide-react'
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
	flip = false,
	...answer
}: Answer & {
	entryKey: number | string
	term: string
	actions?: ReactNode
	ref?: Ref<HTMLElement>
	flip?: boolean
}) {
	const title = useId()

	return (
		<article
			className="review-prompt"
			data-flip={flip}
			aria-labelledby={title}
			tabIndex={-1}
			ref={ref}
		>
			<ReviewContent
				key={entryKey}
				title={title}
				term={term}
				actions={actions}
				flip={flip}
				{...answer}
			/>
		</article>
	)
}

function ReviewContent({
	title,
	term,
	actions,
	flip,
	translation,
	pronunciation,
	notify,
}: Answer & {
	title: string
	term: string
	actions?: ReactNode
	flip: boolean
}) {
	const [revealed, setRevealed] = useState(false)
	const answerId = useId()

	const heading = (
		<>
			<h2 id={title} aria-live="polite">
				<bdi>{term}</bdi>
			</h2>
			<ReviewCopyButton value={term} label="Entry" notify={notify} />
		</>
	)
	const meaning = (
		<dl className="review-meaning">
			<div>
				<dt>Translation</dt>
				<dd>
					<bdi>{translation}</bdi>
					<ReviewCopyButton value={translation} label="Translation" notify={notify} />
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
	)
	const reveal = (
		<Button
			variant={revealed ? 'ghost' : 'primary'}
			className={revealed ? undefined : 'button--lime'}
			aria-expanded={revealed}
			aria-controls={answerId}
			onPress={() => setRevealed(!revealed)}
		>
			{revealed ? 'Hide translation' : 'Show translation'}
		</Button>
	)

	if (flip) {
		return (
			<>
				{actions && <div className="review-card-actions">{actions}</div>}
				<div className="review-card-perspective" data-actions={Boolean(actions)}>
					<div className="review-card-turn" data-revealed={revealed}>
						<div
							className="review-word review-card-face"
							aria-hidden={revealed}
							inert={revealed}
						>
							{heading}
						</div>
						<div
							className="review-card-face review-card-translation"
							id={answerId}
							aria-hidden={!revealed}
							inert={!revealed}
						>
							{meaning}
						</div>
					</div>
				</div>
				<div className="review-answer">
					{reveal}
					<span className="sr-only" aria-live="polite">
						{revealed ? `Translation: ${translation}` : ''}
					</span>
				</div>
			</>
		)
	}

	return (
		<>
			<div className="review-word">
				{heading}
				{actions && <div className="review-word-actions">{actions}</div>}
			</div>
			<div className="review-answer">
				<div id={answerId} aria-live="polite">
					{revealed && meaning}
				</div>
				{reveal}
			</div>
		</>
	)
}

function ReviewCopyButton({
	value,
	label,
	notify,
}: {
	value: string
	label: 'Entry' | 'Translation'
	notify: Answer['notify']
}) {
	const name = label.toLowerCase()

	async function copy() {
		try {
			await navigator.clipboard.writeText(value)
			notify(`${label} copied.`)
		} catch {
			notify(`Could not copy. Select the ${name} to copy it.`)
		}
	}

	return (
		<Button variant="ghost" isIconOnly aria-label={`Copy ${name}`} onPress={copy}>
			<Icon icon={Copy} size={17} />
		</Button>
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
				Previous
			</Button>
			<span aria-live="polite">
				{position} of {total}
			</span>
			<Button variant="outline" isDisabled={!onNext} onPress={onNext}>
				Next
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
