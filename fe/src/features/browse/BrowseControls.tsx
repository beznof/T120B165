import { Button, Input, Label, ListBox, Pagination, Select, TextField } from '@heroui/react'
import type { UseQueryResult } from '@tanstack/react-query'
import { Check, ChevronDown, ChevronLeft, ChevronRight, CircleAlert, Search, X } from 'lucide-react'
import { Children, isValidElement, type ReactNode, useRef, useState } from 'react'
import { ApiError } from '../../api/client'
import type { ColorMode } from '../../ui/AppShell'
import { Icon } from '../../ui/Icon'
import { type BrowseSearch, pageSize as defaultPageSize, pageCount, pageSizeOptions } from './api'
import { BrowseLoading } from './BrowseSkeletons'

const sizeOptions = pageSizeOptions.map((size) => ({ value: String(size), label: String(size) }))

export function Breadcrumbs({ children }: { children: ReactNode }) {
	const items = Children.toArray(children)
	return (
		<nav className="breadcrumbs" aria-label="Breadcrumbs">
			<ol>
				{items.map((item, index) => (
					<li key={isValidElement(item) ? item.key : String(item)}>
						{index > 0 && <Icon icon={ChevronRight} size={14} aria-hidden="true" />}
						{item}
					</li>
				))}
			</ol>
		</nav>
	)
}

function BrowseSelect({
	mode,
	label,
	value,
	options,
	onChange,
}: {
	mode: ColorMode
	label: string
	value: string
	options: { value: string; label: string }[]
	onChange: (value: string) => void
}) {
	return (
		<Select
			className="browse-filter"
			value={value}
			onChange={(key) => {
				if (typeof key === 'string') {
					onChange(key)
				}
			}}
		>
			<Label className="sr-only">{label}</Label>
			<Select.Trigger>
				<Select.Value>
					{({ selectedText }) => (
						<>
							{selectedText}
							<span aria-hidden="true"> / page</span>
						</>
					)}
				</Select.Value>
				<Select.Indicator>
					<Icon icon={ChevronDown} />
				</Select.Indicator>
			</Select.Trigger>
			<Select.Popover className="bloom-theme app-menu" data-mode={mode}>
				<ListBox>
					{options.map((option) => (
						<ListBox.Item id={option.value} key={option.value} textValue={option.label}>
							{option.label}
							<ListBox.ItemIndicator>
								{({ isSelected }) => isSelected && <Icon icon={Check} size={16} />}
							</ListBox.ItemIndicator>
						</ListBox.Item>
					))}
				</ListBox>
			</Select.Popover>
		</Select>
	)
}

export function BrowseStatus({
	title,
	children,
	icon,
}: {
	title: string
	children?: ReactNode
	icon?: ReactNode
}) {
	return (
		<div className="browse-status">
			<span className="browse-state-icon">
				{icon ?? <Icon icon={CircleAlert} size={22} />}
			</span>
			<h2>{title}</h2>
			{children}
		</div>
	)
}

export function BrowseLoadError({ error, retry }: { error: Error; retry: () => void }) {
	if (error instanceof ApiError && error.status === 404) {
		return (
			<BrowseStatus title="Not found">
				<p>
					This item may have been removed. Use the breadcrumbs to return to the previous
					list.
				</p>
			</BrowseStatus>
		)
	}
	return (
		<BrowseStatus title="Couldn’t load this page">
			<p role="alert">
				{error instanceof ApiError && error.status === 403
					? 'Your account does not have access to this resource.'
					: 'The language service could not be reached. Please try again.'}
			</p>
			<Button variant="secondary" onPress={retry}>
				Try again
			</Button>
		</BrowseStatus>
	)
}

export function BrowseSearchBar({
	search,
	label,
	onSearch,
	mode,
	pageSize,
	onPageSizeChange,
	placeholder,
	action,
}: {
	mode: ColorMode
	pageSize?: number
	onPageSizeChange: (size: number) => void
	placeholder: string
	search: string
	label: string
	onSearch: (search: string) => void
	action?: ReactNode
}) {
	return (
		<div className="browse-controls">
			<div className="browse-query">
				<BrowseSearchForm
					search={search}
					label={label}
					placeholder={placeholder}
					onSearch={onSearch}
				/>
				<BrowseSelect
					mode={mode}
					label="Results per page"
					value={String(pageSize ?? defaultPageSize)}
					options={sizeOptions}
					onChange={(value) => onPageSizeChange(Number(value))}
				/>
			</div>
			{action}
		</div>
	)
}

export function BrowseSearchForm({
	search,
	label,
	placeholder,
	onSearch,
}: {
	search: string
	label: string
	placeholder: string
	onSearch: (search: string) => void
}) {
	const [input, setInput] = useState({ search, draft: search })
	const field = useRef<HTMLInputElement>(null)
	const draft = input.search === search ? input.draft : search
	if (input.search !== search) {
		setInput({ search, draft: search })
	}

	return (
		<search>
			<form
				className="browse-search"
				onSubmit={(event) => {
					event.preventDefault()
					const next = draft.trim()
					setInput({ search, draft: next })
					onSearch(next)
				}}
			>
				<div className="browse-search-box" data-clear={Boolean(draft || search)}>
					<TextField
						name="q"
						value={draft}
						onChange={(next) => setInput({ search, draft: next })}
					>
						<Label className="sr-only">{label}</Label>
						<Input
							ref={field}
							type="search"
							maxLength={500}
							placeholder={placeholder}
						/>
					</TextField>
					{(draft || search) && (
						<Button
							variant="ghost"
							isIconOnly
							aria-label="Clear search"
							onPress={() => {
								setInput({ search, draft: '' })
								onSearch('')
								field.current?.focus()
							}}
						>
							<Icon icon={X} size={17} />
						</Button>
					)}
				</div>
				<Button className="button--lime" type="submit" aria-label="Search">
					<Icon icon={Search} size={17} />
					<span>Search</span>
				</Button>
			</form>
		</search>
	)
}

export const numberFormat = new Intl.NumberFormat('en')

export function countLabel(count: number | string, singular: string, plural: string) {
	const value = Number(count)
	return `${numberFormat.format(value)} ${value === 1 ? singular : plural}`
}

export function BrowseResults<T>({
	query,
	search,
	names,
	onPageChange,
	paginationLabel,
	onClear,
	skeleton,
	children,
}: {
	query: UseQueryResult<{ items: T[]; totalCount: number | string; pageSize: number | string }>
	search: BrowseSearch
	names: { singular: string; plural: string }
	onPageChange: (page: number) => void
	paginationLabel?: string
	onClear?: () => void
	skeleton: ReactNode
	children: (items: T[]) => ReactNode
}) {
	const results = useRef<HTMLElement>(null)
	const page = search.page ?? 1
	const data = query.data
	const totalPages = data ? pageCount(data.totalCount, data.pageSize) : 1
	const start = (page - 1) * Number(data?.pageSize ?? defaultPageSize) + 1
	const end = start + (data?.items.length ?? 0) - 1
	const shown =
		start === end
			? numberFormat.format(start)
			: `${numberFormat.format(start)}–${numberFormat.format(end)}`

	function changePage(next: number) {
		onPageChange(next)
		results.current?.focus({ preventScroll: true })
		results.current?.scrollIntoView({ block: 'nearest' })
	}

	const clearFiltersButton = onClear && (
		<Button
			variant="secondary"
			onPress={() => {
				onClear()
				results.current?.focus({ preventScroll: true })
			}}
		>
			Clear filters
		</Button>
	)

	return (
		<section
			className="browse-results"
			aria-label={names.plural}
			aria-busy={query.isRefetching || undefined}
			ref={results}
			tabIndex={-1}
		>
			{query.isPending ? (
				<BrowseLoading label={`Loading ${names.plural}…`}>{skeleton}</BrowseLoading>
			) : query.isError ? (
				<BrowseLoadError error={query.error} retry={() => void query.refetch()} />
			) : page > totalPages ? (
				<BrowseStatus title="This page is out of range">
					<Button variant="secondary" onPress={() => changePage(1)}>
						Go to first page
					</Button>
				</BrowseStatus>
			) : !data?.items.length ? (
				<div className="browse-empty">
					<p role="status">No {names.plural} were found.</p>
					{clearFiltersButton}
				</div>
			) : (
				children(data.items)
			)}
			{data && data.items.length > 0 && !query.isError && page <= totalPages && (
				<div className="browse-pagination">
					<p aria-live="polite">
						{shown} of {countLabel(data.totalCount, names.singular, names.plural)}
					</p>
					{totalPages > 1 && (
						<BrowsePagination
							page={page}
							totalPages={totalPages}
							label={paginationLabel}
							onPageChange={changePage}
						/>
					)}
				</div>
			)}
		</section>
	)
}

export function BrowsePagination({
	label = 'Result pages',
	page,
	totalPages,
	onPageChange,
}: {
	label?: string
	page: number
	totalPages: number
	onPageChange: (page: number) => void
}) {
	const first = Math.max(1, Math.min(page - 1, totalPages - 2))
	const pages = Array.from({ length: Math.min(3, totalPages) }, (_, index) => first + index)
	return (
		<Pagination aria-label={label}>
			<Pagination.Content>
				<Pagination.Item>
					<Pagination.Previous
						aria-label="Previous page"
						isDisabled={page <= 1}
						onPress={() => onPageChange(page - 1)}
					>
						<Icon icon={ChevronLeft} />
					</Pagination.Previous>
				</Pagination.Item>
				{pages.map((number) => (
					<Pagination.Item key={number}>
						<Pagination.Link
							aria-label={`Page ${number}`}
							aria-current={number === page ? 'page' : undefined}
							isActive={number === page}
							onPress={() => onPageChange(number)}
						>
							{number}
						</Pagination.Link>
					</Pagination.Item>
				))}
				<Pagination.Item>
					<Pagination.Next
						aria-label="Next page"
						isDisabled={page >= totalPages}
						onPress={() => onPageChange(page + 1)}
					>
						<Icon icon={ChevronRight} />
					</Pagination.Next>
				</Pagination.Item>
			</Pagination.Content>
		</Pagination>
	)
}
