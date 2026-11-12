import { Skeleton } from '@heroui/react'
import type { ReactNode } from 'react'
import './browse-skeletons.css'

function LoadingBlock({ className }: { className: string }) {
	return <Skeleton animationType="pulse" className={`browse-skeleton ${className}`} />
}

export function BrowseLoading({ label, children }: { label: string; children: ReactNode }) {
	return (
		<>
			<span className="sr-only" role="status">
				{label}
			</span>
			<div aria-hidden="true">{children}</div>
		</>
	)
}

export function BrowseHeaderSkeleton() {
	return (
		<div className="page-header">
			<LoadingBlock className="h-10 w-56 max-w-full" />
		</div>
	)
}

function SearchFormSkeleton() {
	return (
		<div className="browse-search">
			<LoadingBlock className="browse-search-box browse-skeleton-control" />
			<LoadingBlock className="browse-skeleton-control browse-skeleton-search" />
		</div>
	)
}

export function BrowseSearchSkeleton({ showAction = false }: { showAction?: boolean }) {
	return (
		<div className="browse-controls">
			<div className="browse-query">
				<SearchFormSkeleton />
				<LoadingBlock className="browse-skeleton-control" />
			</div>
			{showAction && <LoadingBlock className="browse-skeleton-control w-36" />}
		</div>
	)
}

export function LanguageRowsSkeleton() {
	return (
		<ul className="language-rows">
			{[1, 2, 3, 4, 5].map((row) => (
				<li key={row}>
					<div className="language-row">
						<Skeleton
							className="browse-skeleton cover-image"
							animationType="pulse"
							data-thumbnail="true"
						/>
						<div className="language-label browse-skeleton-lines">
							<LoadingBlock className="h-4 w-32 max-w-full" />
							<LoadingBlock className="h-3 w-24 max-w-full" />
						</div>
					</div>
				</li>
			))}
		</ul>
	)
}

export function DictionaryShelfSkeleton() {
	return (
		<ul className="collection-grid dictionary-shelf">
			{[1, 2, 3].map((card) => (
				<li key={card}>
					<div className="collection-card">
						<LoadingBlock className="cover-image rounded-none" />
						<div className="collection-caption">
							<div className="browse-skeleton-lines w-full">
								<LoadingBlock className="h-4 w-3/4" />
								<LoadingBlock className="h-3 w-1/2" />
							</div>
						</div>
					</div>
				</li>
			))}
		</ul>
	)
}

export function LibraryRowsSkeleton({
	count,
	dictionaries = false,
}: {
	count: number
	dictionaries?: boolean
}) {
	return Array.from({ length: count }, (_, index) => `loading-${index}`).map((key) => (
		<li key={key} className="library-skeleton-row" aria-hidden="true">
			{dictionaries && <LoadingBlock className="size-4 shrink-0" />}
			<LoadingBlock className="h-3 w-2/3" />
		</li>
	))
}

export function ReviewRowsSkeleton() {
	return (
		<ul>
			{[1, 2, 3, 4, 5].map((row) => (
				<li key={row} className="review-skeleton-row">
					<LoadingBlock className="h-4 w-2/3" />
				</li>
			))}
		</ul>
	)
}

export function ReviewStageSkeleton() {
	return (
		<div className="review-stage" aria-hidden="true">
			<div className="review-prompt browse-skeleton-prompt" data-flip="true">
				<LoadingBlock className="h-10 w-3/5 self-center" />
				<div className="review-answer">
					<div>
						<LoadingBlock className="browse-skeleton-control w-40 max-w-full" />
					</div>
				</div>
			</div>
			<div className="review-stepper">
				<LoadingBlock className="browse-skeleton-control w-20" />
				<LoadingBlock className="h-3 w-12" />
				<LoadingBlock className="browse-skeleton-control w-16" />
			</div>
		</div>
	)
}

export function EntryReviewSkeleton() {
	return (
		<div className="entry-review review-layout">
			<ReviewStageSkeleton />
			<div className="review-queue">
				<LoadingBlock className="mb-2 h-6 w-32 max-w-full" />
				<div className="review-queue-tools">
					<SearchFormSkeleton />
					<div className="review-filters">
						{[1, 2, 3].map((filter) => (
							<LoadingBlock key={filter} className="browse-skeleton-control w-16" />
						))}
					</div>
				</div>
				<ReviewRowsSkeleton />
			</div>
		</div>
	)
}
