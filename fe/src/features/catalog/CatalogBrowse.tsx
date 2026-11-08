import {
	Button,
	Input,
	Label,
	ListBox,
	Pagination,
	Select,
	Spinner,
	TextField,
} from '@heroui/react'
import type { UseQueryResult } from '@tanstack/react-query'
import { Check, ChevronDown, ChevronLeft, ChevronRight, CircleAlert, Search, X } from 'lucide-react'
import { Children, isValidElement, type ReactNode, useRef, useState } from 'react'
import { ApiError } from '../../api/client'
import type { ColorMode } from '../../ui/CatalogShell'
import { Icon } from '../../ui/Icon'
import {
	type CatalogSearch,
	catalogPageCount,
	pageSize as defaultPageSize,
	pageSizeOptions,
} from './api'

const sizeOptions = pageSizeOptions.map((size) => ({ value: String(size), label: String(size) }))

export function CatalogTrail({ children }: { children: ReactNode }) {
	const items = Children.toArray(children)
	return (
		<nav className="catalog-trail" aria-label="Breadcrumbs">
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

function CatalogSelect({
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
			className="catalog-filter"
			value={value}
			onChange={(key) => {
				if (typeof key === 'string') {
					onChange(key)
				}
			}}
		>
			<Label>{label}</Label>
			<Select.Trigger>
				<Select.Value />
				<Select.Indicator>
					<Icon icon={ChevronDown} />
				</Select.Indicator>
			</Select.Trigger>
			<Select.Popover className="bloom-theme catalog-menu" data-mode={mode}>
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

export function CatalogStatus({
	title,
	children,
	icon,
	heading: Heading = 'h2',
}: {
	title: string
	children?: ReactNode
	icon?: ReactNode
	heading?: 'h1' | 'h2'
}) {
	return (
		<div className="catalog-status">
			<span className="catalog-state-icon">
				{icon ?? <Icon icon={CircleAlert} size={22} />}
			</span>
			<Heading>{title}</Heading>
			{children}
		</div>
	)
}

export function CatalogLoadError({ error, retry }: { error: Error; retry: () => void }) {
	if (error instanceof ApiError && error.status === 404) {
		return (
			<CatalogStatus title="Not found">
				<p>
					This item may have been removed. Use the breadcrumbs to return to the catalog.
				</p>
			</CatalogStatus>
		)
	}
	return (
		<CatalogStatus title="Couldn’t load this page">
			<p role="alert">
				{error instanceof ApiError && error.status === 403
					? 'Your account does not have access to this resource.'
					: 'The language service could not be reached. Please try again.'}
			</p>
			<Button variant="secondary" onPress={retry}>
				Try again
			</Button>
		</CatalogStatus>
	)
}

export function CatalogLoading({ label }: { label: string }) {
	return (
		<div className="catalog-loading" role="status">
			<Spinner color="accent" /> {label}
		</div>
	)
}

export function CatalogSearchBar({
	search,
	label,
	onSearch,
	mode,
	pageSize,
	onPageSizeChange,
	placeholder,
	displayControl,
}: {
	mode: ColorMode
	pageSize?: number
	onPageSizeChange: (size: number) => void
	placeholder: string
	search: string
	label: string
	onSearch: (search: string) => void
	displayControl?: ReactNode
}) {
	const [input, setInput] = useState({ search, draft: search })
	const field = useRef<HTMLInputElement>(null)
	const draft = input.search === search ? input.draft : search
	if (input.search !== search) {
		setInput({ search, draft: search })
	}

	return (
		<div className="catalog-query" data-display-control={Boolean(displayControl)}>
			<search>
				<form
					className="catalog-search"
					onSubmit={(event) => {
						event.preventDefault()
						const next = draft.trim()
						setInput({ search, draft: next })
						onSearch(next)
					}}
				>
					<div className="catalog-search-box" data-clear={Boolean(draft || search)}>
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
					<Button type="submit" aria-label="Search">
						<Icon icon={Search} size={17} />
						<span>Search</span>
					</Button>
				</form>
			</search>
			<CatalogSelect
				mode={mode}
				label="Per page"
				value={String(pageSize ?? defaultPageSize)}
				options={sizeOptions}
				onChange={(value) => onPageSizeChange(Number(value))}
			/>
			{displayControl}
		</div>
	)
}

export const numberFormat = new Intl.NumberFormat('en')

export function countLabel(count: number | string, singular: string, plural: string) {
	const value = Number(count)
	return `${numberFormat.format(value)} ${value === 1 ? singular : plural}`
}

export function CatalogResults<T>({
	query,
	search,
	names,
	onPageChange,
	onClear,
	children,
}: {
	query: UseQueryResult<{ items: T[]; totalCount: number | string; pageSize: number | string }>
	search: CatalogSearch
	names: { singular: string; plural: string }
	onPageChange: (page: number) => void
	onClear?: () => void
	children: (items: T[]) => ReactNode
}) {
	const results = useRef<HTMLElement>(null)
	const page = search.page ?? 1
	const data = query.data
	const totalPages = data ? catalogPageCount(data.totalCount, data.pageSize) : 1
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

	return (
		<section
			className="catalog-results"
			aria-label={names.plural}
			aria-busy={query.isRefetching || undefined}
			ref={results}
			tabIndex={-1}
		>
			{query.isPending ? (
				<CatalogLoading label={`Loading ${names.plural}…`} />
			) : query.isError ? (
				<CatalogLoadError error={query.error} retry={() => void query.refetch()} />
			) : page > totalPages ? (
				<CatalogStatus title="This page is out of range">
					<Button variant="secondary" onPress={() => changePage(1)}>
						Go to first page
					</Button>
				</CatalogStatus>
			) : !data?.items.length ? (
				<CatalogStatus
					title={onClear ? `No matching ${names.plural}` : `No ${names.plural} yet`}
					icon={<Icon icon={Search} size={22} />}
				>
					{onClear && (
						<Button
							variant="secondary"
							onPress={() => {
								onClear()
								results.current?.focus({ preventScroll: true })
							}}
						>
							Clear filters
						</Button>
					)}
				</CatalogStatus>
			) : (
				children(data.items)
			)}
			{data && data.items.length > 0 && !query.isError && page <= totalPages && (
				<div className="catalog-pagination">
					<p aria-live="polite">
						{shown} of {countLabel(data.totalCount, names.singular, names.plural)}
					</p>
					{totalPages > 1 && (
						<CatalogPagination
							page={page}
							totalPages={totalPages}
							onPageChange={changePage}
						/>
					)}
				</div>
			)}
		</section>
	)
}

function CatalogPagination({
	page,
	totalPages,
	onPageChange,
}: {
	page: number
	totalPages: number
	onPageChange: (page: number) => void
}) {
	const first = Math.max(1, Math.min(page - 1, totalPages - 2))
	const pages = Array.from({ length: Math.min(3, totalPages) }, (_, index) => first + index)
	return (
		<Pagination aria-label="Catalog pages">
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
