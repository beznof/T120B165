import { useQuery } from '@tanstack/react-query'
import { Link } from '@tanstack/react-router'
import { useEffect, useRef } from 'react'
import type { Schema } from '../../api/client'
import { type AppContext, PageHeader } from '../../ui/AppFrame'
import {
	ReviewFilters,
	ReviewPrompt,
	ReviewQueue,
	ReviewQueueEntry,
	ReviewStepper,
} from '../../ui/VocabularyReview'
import {
	canEditDictionary,
	dictionaryPath,
	type EntrySearch,
	entriesPath,
	entryPageSize,
	pageCount,
	pagedQuery,
	resourceQuery,
} from '../browse/api'
import {
	Breadcrumbs,
	BrowseLoadError,
	BrowseResults,
	BrowseSearchForm,
} from '../browse/BrowseControls'
import {
	BrowseHeaderSkeleton,
	BrowseLoading,
	EntryReviewSkeleton,
	ReviewRowsSkeleton,
	ReviewStageSkeleton,
} from '../browse/BrowseSkeletons'
import { AddRecordButton, RecordMenu, useResourceEditor } from '../browse/ResourceEditor'

export type EntryReviewSearch = Omit<EntrySearch, 'pageSize'> & { position?: number }

export function EntriesPage({
	context,
	languageId,
	dictionaryId,
	search,
	onSearch,
	onFilter,
	onClear,
	onPageChange,
	onPositionChange,
}: {
	context: AppContext
	languageId: string
	dictionaryId: string
	search: EntryReviewSearch
	onSearch: (value: string) => void
	onFilter: (type: EntrySearch['type']) => void
	onClear: () => void
	onPageChange: (page: number) => void
	onPositionChange: (page: number, position: number) => void
}) {
	const dictionary = useQuery(
		resourceQuery<Schema['GetOneDictionaryOutput']>(dictionaryPath(languageId, dictionaryId)),
	)
	const path = entriesPath(languageId, dictionaryId)
	const query = useQuery(
		pagedQuery<Schema['GetManyEntriesOutput']>(path, { ...search, pageSize: entryPageSize }),
	)
	const editor = useResourceEditor(context)
	const prompt = useRef<HTMLElement>(null)
	const focusOnPage = useRef<number | null>(null)

	const page = search.page ?? 1
	const data = query.isSuccess ? query.data : undefined
	const total = Number(data?.totalCount ?? 0)
	const pages = pageCount(total, entryPageSize)
	const count = data?.items.length ?? 0
	const index = Math.min(search.position ?? 1, count) - 1
	const entry = data?.items[index]
	const canEdit = dictionary.data && canEditDictionary(context.account, dictionary.data.owner.id)

	useEffect(() => {
		if (entry && focusOnPage.current === page && prompt.current) {
			focusOnPage.current = null
			prompt.current.focus({ preventScroll: true })
			prompt.current.scrollIntoView({ block: 'nearest' })
		}
	}, [entry, page])

	function step(direction: -1 | 1) {
		const next = index + direction
		if (next >= 0 && next < count) {
			onPositionChange(page, next + 1)
			return
		}

		const adjacentPage = page + direction
		focusOnPage.current = adjacentPage
		onPositionChange(adjacentPage, direction === 1 ? 1 : entryPageSize)
	}

	function choose(position: number) {
		onPositionChange(page, position)
		prompt.current?.focus({ preventScroll: true })
		prompt.current?.scrollIntoView({ block: 'nearest' })
	}

	return (
		<>
			<Breadcrumbs>
				<Link to="/">Languages</Link>
				<Link to="/languages/$languageId" params={{ languageId }}>
					<bdi>{dictionary.data?.language.name ?? 'Dictionaries'}</bdi>
				</Link>
			</Breadcrumbs>
			{dictionary.isPending ? (
				<BrowseLoading label="Loading dictionary and entries…">
					<BrowseHeaderSkeleton />
					<EntryReviewSkeleton />
				</BrowseLoading>
			) : dictionary.isError ? (
				<BrowseLoadError error={dictionary.error} retry={() => void dictionary.refetch()} />
			) : (
				<>
					<PageHeader
						title={dictionary.data.name}
						actions={
							canEdit && (
								<AddRecordButton
									resource="entry"
									onPress={() => editor.open({ kind: 'entry', path })}
								/>
							)
						}
					/>
					<div className="entry-review review-layout">
						{query.isPending && <ReviewStageSkeleton />}
						{entry && (
							<div className="review-stage">
								<ReviewPrompt
									flip
									ref={prompt}
									entryKey={entry.id}
									term={entry.text}
									translation={entry.translation}
									pronunciation={entry.phoneticTranscription}
									notify={context.notify}
									actions={
										canEdit && (
											<RecordMenu
												mode={context.mode}
												label="Entry actions"
												open={editor.open}
												items={[
													{
														label: 'Edit entry',
														action: {
															kind: 'entry',
															path: `${path}/${entry.id}`,
															entry,
														},
													},
													{
														label: 'Delete entry',
														action: {
															kind: 'delete',
															resource: 'entry',
															path: `${path}/${entry.id}`,
															name: entry.text,
														},
													},
												]}
											/>
										)
									}
								/>
								<ReviewStepper
									position={(page - 1) * entryPageSize + index + 1}
									total={total}
									onPrevious={page > 1 || index > 0 ? () => step(-1) : undefined}
									onNext={
										page < pages || index + 1 < count
											? () => step(1)
											: undefined
									}
								/>
							</div>
						)}
						<ReviewQueue
							tools={
								<>
									<BrowseSearchForm
										placeholder="Find a term"
										label="Search entries"
										search={search.q ?? ''}
										onSearch={onSearch}
									/>
									<ReviewFilters value={search.type} onChange={onFilter} />
								</>
							}
						>
							<BrowseResults
								query={query}
								skeleton={<ReviewRowsSkeleton />}
								search={search}
								names={{ singular: 'entry', plural: 'entries' }}
								onPageChange={onPageChange}
								onClear={search.q || search.type ? onClear : undefined}
							>
								{(items) => (
									<ul>
										{items.map((item, position) => (
											<ReviewQueueEntry
												key={item.id}
												active={entry?.id === item.id}
												onPress={() => choose(position + 1)}
											>
												<bdi>{item.text}</bdi>
											</ReviewQueueEntry>
										))}
									</ul>
								)}
							</BrowseResults>
						</ReviewQueue>
					</div>
					{editor.dialog}
				</>
			)}
		</>
	)
}
