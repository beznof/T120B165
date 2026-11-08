import { useQuery } from '@tanstack/react-query'
import { Link } from '@tanstack/react-router'
import type { Schema } from '../../api/client'
import { CollectionCard } from '../../ui/CollectionCard'
import { type CatalogSearch, languagePath, languagesPath, pagedQuery } from '../catalog/api'
import { CatalogResults, CatalogSearchBar, countLabel } from '../catalog/CatalogBrowse'
import { AddRecordButton, RecordActions, useCatalogEditor } from '../catalog/CatalogEditor'
import { type CatalogContext, CatalogPageHeader } from '../catalog/CatalogFrame'
import { CollectionDisplayToggle, useCollectionDisplay } from '../catalog/CollectionDisplay'

export function LanguagesPage({
	context,
	search,
	onSearch,
	onPageChange,
	onPageSizeChange,
}: {
	context: CatalogContext
	search: CatalogSearch
	onSearch: (value: string) => void
	onPageChange: (page: number) => void
	onPageSizeChange: (size: number) => void
}) {
	const query = useQuery(pagedQuery<Schema['GetManyLanguagesOutput']>(languagesPath, search))
	const editor = useCatalogEditor(context)
	const [display, setDisplay] = useCollectionDisplay()
	const admin = context.account?.role === 'Admin'

	return (
		<>
			<CatalogPageHeader
				title="Languages"
				actions={
					admin && (
						<AddRecordButton
							resource="language"
							onPress={() =>
								editor.open({
									kind: 'name',
									resource: 'language',
									path: languagesPath,
								})
							}
						/>
					)
				}
			/>
			<CatalogSearchBar
				mode={context.mode}
				pageSize={search.pageSize}
				onPageSizeChange={onPageSizeChange}
				placeholder="Find a language"
				label="Search languages"
				search={search.q ?? ''}
				onSearch={onSearch}
				displayControl={<CollectionDisplayToggle value={display} onChange={setDisplay} />}
			/>
			<CatalogResults
				query={query}
				search={search}
				names={{ singular: 'language', plural: 'languages' }}
				onPageChange={onPageChange}
				onClear={search.q ? () => onSearch('') : undefined}
			>
				{(items) => (
					<ul className="collection-grid" data-display={display}>
						{items.map((language) => (
							<li key={language.id}>
								<CollectionCard
									name={language.name}
									image={language.backgroundImageUrl}
									summary={countLabel(
										language.dictionariesCount,
										'dictionary',
										'dictionaries',
									)}
									renderLink={(contents) => (
										<Link
											to="/languages/$languageId"
											params={{ languageId: String(language.id) }}
											aria-label={`Browse ${language.name} dictionaries`}
										>
											{contents}
										</Link>
									)}
									actions={
										admin && (
											<RecordActions
												mode={context.mode}
												resource="language"
												path={languagePath(language.id)}
												record={language}
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
	)
}
