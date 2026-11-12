import { Button } from '@heroui/react'
import { useQuery } from '@tanstack/react-query'
import { Link } from '@tanstack/react-router'
import { BookOpen, ChevronDown, ChevronRight, Ellipsis } from 'lucide-react'
import { useId, useMemo, useState } from 'react'
import type { Schema } from '../../../api/client'
import { Icon } from '../../../ui/icons/Icon'
import { dictionariesPath, languagesPath, pagedQuery } from '../api'
import { countLabel } from '../results/BrowseControls'
import { LibraryRowsSkeleton } from '../results/BrowseSkeletons'

const languageLimit = 8
const dictionaryLimit = 4
const languageNameOrder = new Intl.Collator(undefined, { sensitivity: 'base', numeric: true })

export function LibrarySidebar({
	visible,
	activeLanguage,
	activeDictionary,
	onNavigate,
}: {
	visible: boolean
	activeLanguage?: string
	activeDictionary?: string
	onNavigate: () => void
}) {
	const [expanded, setExpanded] = useState<{ forLanguage?: string; id?: string }>()
	const openLanguage = expanded?.forLanguage === activeLanguage ? expanded?.id : activeLanguage
	const query = useQuery({
		...pagedQuery<Schema['GetManyLanguagesOutput']>(languagesPath, { pageSize: languageLimit }),
		enabled: visible,
	})
	const languages = useMemo(
		() =>
			query.data?.items
				.slice(0, languageLimit)
				.sort((first, second) => languageNameOrder.compare(first.name, second.name)) ?? [],
		[query.data],
	)

	if (!visible) {
		return null
	}

	return (
		<>
			<h2>Library</h2>
			{query.isPending && (
				<span className="sr-only" role="status">
					Loading languages…
				</span>
			)}
			{query.isLoadingError && (
				<>
					<p className="library-sidebar-note" role="alert">
						Couldn’t load languages.
					</p>
					<Button variant="ghost" onPress={() => void query.refetch()}>
						Try again
					</Button>
				</>
			)}
			{query.data && languages.length === 0 && (
				<p className="library-sidebar-note">No languages yet.</p>
			)}
			{(languages.length > 0 || query.isPending) && (
				<ul className="library-sidebar-languages">
					{languages.map((language) => {
						const id = String(language.id)
						return (
							<LibraryLanguage
								key={id}
								language={language}
								active={id === activeLanguage}
								activeDictionary={
									id === activeLanguage ? activeDictionary : undefined
								}
								expanded={id === openLanguage}
								onToggle={() =>
									setExpanded({
										forLanguage: activeLanguage,
										id: id === openLanguage ? undefined : id,
									})
								}
								onNavigate={onNavigate}
							/>
						)
					})}
					{query.isPending && <LibraryRowsSkeleton count={languageLimit} />}
				</ul>
			)}
			{Number(query.data?.totalCount ?? 0) > languageLimit && (
				<Link
					className="library-sidebar-more"
					to="/"
					search={{}}
					aria-label="Show more languages"
					onClick={onNavigate}
				>
					<Icon icon={Ellipsis} size={16} />
					Show more
				</Link>
			)}
		</>
	)
}

function LibraryLanguage({
	language,
	active,
	activeDictionary,
	expanded,
	onToggle,
	onNavigate,
}: {
	language: Schema['GetOneLanguageOutput']
	active: boolean
	activeDictionary?: string
	expanded: boolean
	onToggle: () => void
	onNavigate: () => void
}) {
	const panelId = useId()
	const languageId = String(language.id)
	const hasDictionaries = Number(language.dictionariesCount) > 0
	const open = expanded && hasDictionaries
	const dictionaryPage = useQuery({
		...pagedQuery<Schema['GetManyDictionariesOutput']>(dictionariesPath(languageId), {
			pageSize: dictionaryLimit,
		}),
		enabled: open,
	})
	const items = dictionaryPage.data?.items ?? []
	const count = dictionaryPage.data?.totalCount ?? language.dictionariesCount

	return (
		<li>
			<div className="library-sidebar-language" data-active={active && !activeDictionary}>
				<Link
					to="/languages/$languageId"
					params={{ languageId }}
					activeOptions={{ exact: true, includeSearch: false }}
					onClick={onNavigate}
				>
					<bdi>{language.name}</bdi>
				</Link>
				{hasDictionaries && (
					<Button
						variant="ghost"
						isIconOnly
						aria-label={`${open ? 'Collapse' : 'Expand'} ${language.name}`}
						aria-controls={panelId}
						aria-expanded={open}
						onPress={onToggle}
					>
						<Icon icon={open ? ChevronDown : ChevronRight} size={16} />
					</Button>
				)}
			</div>
			<div id={panelId} hidden={!open}>
				{open && (
					<>
						{dictionaryPage.isPending && (
							<span role="status" className="sr-only">
								Loading dictionaries…
							</span>
						)}
						{dictionaryPage.isError && (
							<Button variant="ghost" onPress={() => void dictionaryPage.refetch()}>
								Retry dictionaries
							</Button>
						)}
						<ul className="library-sidebar-dictionaries">
							{dictionaryPage.isPending && (
								<LibraryRowsSkeleton
									count={Math.min(dictionaryLimit, Number(count))}
									dictionaries
								/>
							)}
							{items.map((item) => (
								<li key={item.id}>
									<Link
										to="/languages/$languageId/dictionaries/$dictionaryId"
										params={{ languageId, dictionaryId: String(item.id) }}
										activeOptions={{ exact: true, includeSearch: false }}
										onClick={onNavigate}
									>
										<Icon icon={BookOpen} size={16} />
										<bdi>{item.name}</bdi>
									</Link>
								</li>
							))}
						</ul>
						{Number(count) > dictionaryLimit && (
							<Link
								className="library-sidebar-more"
								to="/languages/$languageId"
								params={{ languageId }}
								onClick={onNavigate}
							>
								<Icon icon={Ellipsis} size={16} />
								View all {countLabel(count, 'dictionary', 'dictionaries')}
							</Link>
						)}
					</>
				)}
			</div>
		</li>
	)
}
