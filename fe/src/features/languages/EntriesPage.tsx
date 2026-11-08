import { useQuery } from '@tanstack/react-query'
import { Link } from '@tanstack/react-router'
import { useEffect, useRef } from 'react'
import type { Schema } from '../../api/client'
import {
	ReviewFilters,
	ReviewPrompt,
	ReviewQueue,
	ReviewQueueEntry,
	ReviewStepper,
} from '../../ui/VocabularyReview'
import {
	canEditDictionary,
	catalogPageCount,
	catalogQuery,
	dictionaryPath,
	type EntrySearch,
	entriesPath,
	pagedQuery,
	pageSize,
} from '../catalog/api'
import {
	CatalogLoadError,
	CatalogLoading,
	CatalogResults,
	CatalogSearchBar,
	CatalogTrail,
} from '../catalog/CatalogBrowse'
import { AddRecordButton, CatalogRecordMenu, useCatalogEditor } from '../catalog/CatalogEditor'
import { type CatalogContext, CatalogPageHeader } from '../catalog/CatalogFrame'

export type EntryReviewSearch = EntrySearch & { position?: number }

export function EntriesPage({
	context,
	languageId,
	dictionaryId,
	search,
	onSearch,
	onFilter,
	onClear,
	onPageChange,
	onPageSizeChange,
	onPositionChange,
}: {
	context: CatalogContext
	languageId: string
	dictionaryId: string
	search: EntryReviewSearch
	onSearch: (value: string) => void
	onFilter: (type: EntrySearch['type']) => void
	onClear: () => void
	onPageChange: (page: number) => void
	onPageSizeChange: (size: number) => void
	onPositionChange: (page: number, position: number) => void
}) {
	const dictionary = useQuery(
		catalogQuery<Schema['GetOneDictionaryOutput']>(dictionaryPath(languageId, dictionaryId)),
	)
	const path = entriesPath(languageId, dictionaryId)
	const query = useQuery(pagedQuery<Schema['GetManyEntriesOutput']>(path, search))
	const editor = useCatalogEditor(context)
	const prompt = useRef<HTMLElement>(null)
	const focusOnPage = useRef<number | null>(null)

	const page = search.page ?? 1
	const data = query.isSuccess ? query.data : undefined
	const size = Number(data?.pageSize ?? search.pageSize ?? pageSize)
	const total = Number(data?.totalCount ?? 0)
	const pages = catalogPageCount(total, size)
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
		onPositionChange(adjacentPage, direction === 1 ? 1 : size)
	}

	function choose(position: number) {
		onPositionChange(page, position)
		prompt.current?.focus({ preventScroll: true })
		prompt.current?.scrollIntoView({ block: 'nearest' })
	}

	return (
		<>
			<CatalogTrail>
				<Link to="/">Languages</Link>
				<Link to="/languages/$languageId" params={{ languageId }}>
					<bdi>{dictionary.data?.language.name ?? 'Dictionaries'}</bdi>
				</Link>
			</CatalogTrail>
			{dictionary.isPending ? (
				<CatalogLoading label="Loading dictionary…" />
			) : dictionary.isError ? (
				<CatalogLoadError
					error={dictionary.error}
					retry={() => void dictionary.refetch()}
				/>
			) : (
				<>
					<CatalogPageHeader
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
					<div className="catalog-review review-layout">
						{entry && (
							<div className="review-stage">
								<ReviewPrompt
									ref={prompt}
									entryKey={entry.id}
									term={<bdi>{entry.text}</bdi>}
									translation={entry.translation}
									pronunciation={entry.phoneticTranscription}
									notify={context.notify}
									actions={
										canEdit && (
											<CatalogRecordMenu
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
									position={(page - 1) * size + index + 1}
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
									<CatalogSearchBar
										mode={context.mode}
										pageSize={search.pageSize}
										onPageSizeChange={onPageSizeChange}
										placeholder="Find a term"
										label="Search entries"
										search={search.q ?? ''}
										onSearch={onSearch}
									/>
									<ReviewFilters value={search.type} onChange={onFilter} />
								</>
							}
						>
							<CatalogResults
								query={query}
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
							</CatalogResults>
						</ReviewQueue>
					</div>
					{editor.dialog}
				</>
			)}
		</>
	)
}
