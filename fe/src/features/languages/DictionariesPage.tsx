import { useQuery } from '@tanstack/react-query'
import { Link } from '@tanstack/react-router'
import type { Schema } from '../../api/client'
import { type AppContext, PageHeader } from '../../ui/AppFrame'
import { CollectionCard } from '../../ui/CollectionCard'
import {
	type BrowseSearch,
	canEditDictionary,
	dictionariesPath,
	dictionaryPath,
	languagePath,
	pagedQuery,
	resourceQuery,
} from '../browse/api'
import {
	Breadcrumbs,
	BrowseLoadError,
	BrowseResults,
	BrowseSearchBar,
	countLabel,
} from '../browse/BrowseControls'
import {
	BrowseHeaderSkeleton,
	BrowseLoading,
	BrowseSearchSkeleton,
	DictionaryShelfSkeleton,
} from '../browse/BrowseSkeletons'
import { AddRecordButton, RecordActions, useResourceEditor } from '../browse/ResourceEditor'

export function DictionariesPage({
	context,
	languageId,
	search,
	onSearch,
	onPageChange,
	onPageSizeChange,
}: {
	context: AppContext
	languageId: string
	search: BrowseSearch
	onSearch: (value: string) => void
	onPageChange: (page: number) => void
	onPageSizeChange: (size: number) => void
}) {
	const language = useQuery(
		resourceQuery<Schema['GetOneLanguageOutput']>(languagePath(languageId)),
	)
	const path = dictionariesPath(languageId)
	const query = useQuery(pagedQuery<Schema['GetManyDictionariesOutput']>(path, search))
	const editor = useResourceEditor(context)
	const canAdd = context.account?.role === 'Teacher' || context.account?.role === 'Admin'

	return (
		<>
			<Breadcrumbs>
				<Link to="/">Languages</Link>
			</Breadcrumbs>
			{language.isPending ? (
				<BrowseLoading label="Loading language and dictionaries…">
					<BrowseHeaderSkeleton />
					<BrowseSearchSkeleton showAction={canAdd} />
					<div className="browse-results">
						<DictionaryShelfSkeleton />
					</div>
				</BrowseLoading>
			) : language.isError ? (
				<BrowseLoadError error={language.error} retry={() => void language.refetch()} />
			) : (
				<>
					<PageHeader title={language.data.name} />
					<BrowseSearchBar
						mode={context.mode}
						pageSize={search.pageSize}
						onPageSizeChange={onPageSizeChange}
						placeholder="Find a dictionary"
						label="Search dictionaries"
						search={search.q ?? ''}
						onSearch={onSearch}
						action={
							canAdd && (
								<AddRecordButton
									resource="dictionary"
									onPress={() =>
										editor.open({ kind: 'name', resource: 'dictionary', path })
									}
								/>
							)
						}
					/>
					<BrowseResults
						query={query}
						skeleton={<DictionaryShelfSkeleton />}
						search={search}
						names={{ singular: 'dictionary', plural: 'dictionaries' }}
						onPageChange={onPageChange}
						onClear={search.q ? () => onSearch('') : undefined}
					>
						{(items) => (
							<ul className="collection-grid dictionary-shelf">
								{items.map((dictionary) => (
									<li key={dictionary.id}>
										<CollectionCard
											name={dictionary.name}
											image={dictionary.backgroundImageUrl}
											summary={`${countLabel(dictionary.entriesCount, 'entry', 'entries')} · ${dictionary.owner.username}`}
											renderLink={(contents) => (
												<Link
													to="/languages/$languageId/dictionaries/$dictionaryId"
													params={{
														languageId,
														dictionaryId: String(dictionary.id),
													}}
													aria-label={`Browse ${dictionary.name} entries`}
												>
													{contents}
												</Link>
											)}
											actions={
												canEditDictionary(
													context.account,
													dictionary.owner.id,
												) && (
													<RecordActions
														mode={context.mode}
														resource="dictionary"
														path={dictionaryPath(
															languageId,
															dictionary.id,
														)}
														record={dictionary}
														open={editor.open}
													/>
												)
											}
										/>
									</li>
								))}
							</ul>
						)}
					</BrowseResults>
					{editor.dialog}
				</>
			)}
		</>
	)
}
