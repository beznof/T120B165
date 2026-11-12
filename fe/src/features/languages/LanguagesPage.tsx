import { useQuery } from '@tanstack/react-query'
import { Link } from '@tanstack/react-router'
import type { Schema } from '../../api/client'
import { type AppContext, PageHeader } from '../../ui/AppFrame'
import { CoverImage } from '../../ui/CoverImage'
import { type BrowseSearch, languagePath, languagesPath, pagedQuery } from '../browse/api'
import { BrowseResults, BrowseSearchBar, countLabel } from '../browse/BrowseControls'
import { LanguageRowsSkeleton } from '../browse/BrowseSkeletons'
import { AddRecordButton, RecordActions, useResourceEditor } from '../browse/ResourceEditor'

export function LanguagesPage({
	context,
	search,
	onSearch,
	onPageChange,
	onPageSizeChange,
}: {
	context: AppContext
	search: BrowseSearch
	onSearch: (value: string) => void
	onPageChange: (page: number) => void
	onPageSizeChange: (size: number) => void
}) {
	const query = useQuery(pagedQuery<Schema['GetManyLanguagesOutput']>(languagesPath, search))
	const editor = useResourceEditor(context)
	const admin = context.account?.role === 'Admin'

	return (
		<>
			<PageHeader title="Languages" />
			<BrowseSearchBar
				mode={context.mode}
				pageSize={search.pageSize}
				onPageSizeChange={onPageSizeChange}
				placeholder="Find a language"
				label="Search languages"
				search={search.q ?? ''}
				onSearch={onSearch}
				action={
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
			<BrowseResults
				query={query}
				skeleton={<LanguageRowsSkeleton />}
				search={search}
				names={{ singular: 'language', plural: 'languages' }}
				onPageChange={onPageChange}
				onClear={search.q ? () => onSearch('') : undefined}
			>
				{(items) => (
					<ul className="language-rows">
						{items.map((language) => (
							<li key={language.id}>
								<Link
									className="language-row"
									data-editable={admin}
									to="/languages/$languageId"
									params={{ languageId: String(language.id) }}
								>
									<CoverImage
										name={language.name}
										image={language.backgroundImageUrl}
										thumbnail
									/>
									<span className="language-label">
										<strong>
											<bdi>{language.name}</bdi>
										</strong>
										<small>
											{countLabel(
												language.dictionariesCount,
												'dictionary',
												'dictionaries',
											)}
										</small>
									</span>
								</Link>
								{admin && (
									<div className="language-row-actions">
										<RecordActions
											mode={context.mode}
											resource="language"
											path={languagePath(language.id)}
											record={language}
											open={editor.open}
										/>
									</div>
								)}
							</li>
						))}
					</ul>
				)}
			</BrowseResults>
			{editor.dialog}
		</>
	)
}
