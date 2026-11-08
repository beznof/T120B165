import { useQuery } from '@tanstack/react-query'
import { Link } from '@tanstack/react-router'
import type { Schema } from '../../api/client'
import { CollectionCard } from '../../ui/CollectionCard'
import {
	type CatalogSearch,
	canEditDictionary,
	catalogQuery,
	dictionariesPath,
	dictionaryPath,
	languagePath,
	pagedQuery,
} from '../catalog/api'
import {
	CatalogLoadError,
	CatalogLoading,
	CatalogResults,
	CatalogSearchBar,
	CatalogTrail,
	countLabel,
} from '../catalog/CatalogBrowse'
import { AddRecordButton, RecordActions, useCatalogEditor } from '../catalog/CatalogEditor'
import { type CatalogContext, CatalogPageHeader } from '../catalog/CatalogFrame'
import { CollectionDisplayToggle, useCollectionDisplay } from '../catalog/CollectionDisplay'

export function DictionariesPage({
	context,
	languageId,
	search,
	onSearch,
	onPageChange,
	onPageSizeChange,
}: {
	context: CatalogContext
	languageId: string
	search: CatalogSearch
	onSearch: (value: string) => void
	onPageChange: (page: number) => void
	onPageSizeChange: (size: number) => void
}) {
	const language = useQuery(
		catalogQuery<Schema['GetOneLanguageOutput']>(languagePath(languageId)),
	)
	const path = dictionariesPath(languageId)
	const query = useQuery(pagedQuery<Schema['GetManyDictionariesOutput']>(path, search))
	const editor = useCatalogEditor(context)
	const [display, setDisplay] = useCollectionDisplay()
	const canAdd = context.account?.role === 'Teacher' || context.account?.role === 'Admin'

	return (
		<>
			<CatalogTrail>
				<Link to="/">Languages</Link>
			</CatalogTrail>
			{language.isPending ? (
				<CatalogLoading label="Loading language…" />
			) : language.isError ? (
				<CatalogLoadError error={language.error} retry={() => void language.refetch()} />
			) : (
				<>
					<CatalogPageHeader
						title={language.data.name}
						actions={
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
					<CatalogSearchBar
						mode={context.mode}
						pageSize={search.pageSize}
						onPageSizeChange={onPageSizeChange}
						placeholder="Find a dictionary"
						label="Search dictionaries"
						search={search.q ?? ''}
						onSearch={onSearch}
						displayControl={
							<CollectionDisplayToggle value={display} onChange={setDisplay} />
						}
					/>
					<CatalogResults
						query={query}
						search={search}
						names={{ singular: 'dictionary', plural: 'dictionaries' }}
						onPageChange={onPageChange}
						onClear={search.q ? () => onSearch('') : undefined}
					>
						{(items) => (
							<ul className="collection-grid" data-display={display}>
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
					</CatalogResults>
					{editor.dialog}
				</>
			)}
		</>
	)
}
