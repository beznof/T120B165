import { Button, Dropdown, Input, Label, Modal, TextArea, TextField } from '@heroui/react'
import { useQueryClient } from '@tanstack/react-query'
import { Ellipsis, ImagePlus, Pencil, Plus, Trash2, X } from 'lucide-react'
import { type FormEvent, useEffect, useState } from 'react'
import { ApiError, type Schema } from '../../../api/client'
import { Icon } from '../../../ui/icons/Icon'
import type { AppContext } from '../../../ui/layout/AppFrame'
import type { ColorMode } from '../../../ui/layout/AppShell'
import { browseQueryKey, removeResource, saveEntry, saveName, uploadCover } from '../api'
import { ChoiceGroup } from './ChoiceGroup'
import './resource-editor.css'

type Resource = 'language' | 'dictionary' | 'entry'
type Entry = Schema['GetOneEntryOutput']
type RecordSummary = { name: string; backgroundImageUrl: string | null }

export type EditorAction =
	| { kind: 'name'; path: string; resource: 'language' | 'dictionary'; name?: string }
	| { kind: 'entry'; path: string; entry?: Entry }
	| { kind: 'cover'; path: string; record: RecordSummary }
	| { kind: 'delete'; path: string; resource: Resource; name: string }

function actionTitle(action: EditorAction) {
	switch (action.kind) {
		case 'name':
			return `${action.name === undefined ? 'Add' : 'Edit'} ${action.resource}`
		case 'entry':
			return action.entry ? 'Edit entry' : 'Add entry'
		case 'cover':
			return 'Cover image'
		case 'delete':
			return `Delete ${action.resource}?`
	}
}

function textValue(form: FormData, name: string) {
	return String(form.get(name) ?? '').trim()
}

async function submitAction(action: EditorAction, form: FormData) {
	switch (action.kind) {
		case 'name': {
			const name = textValue(form, 'name')
			if (!name) {
				throw new Error('Enter a name.')
			}
			await saveName(action.path, name, action.name !== undefined)
			return action.name === undefined ? 'Created successfully.' : 'Changes saved.'
		}
		case 'entry': {
			const text = textValue(form, 'text')
			const translation = textValue(form, 'translation')
			if (!text || !translation) {
				throw new Error('Enter the word or phrase and its translation.')
			}
			await saveEntry(
				action.path,
				{
					text,
					translation,
					type: form.get('type') === '1' ? 1 : 0,
					phoneticTranscription: textValue(form, 'phonetics') || null,
				},
				Boolean(action.entry),
			)
			return action.entry ? 'Entry updated.' : 'Entry added.'
		}
		case 'cover': {
			if (form.get('operation') === 'remove') {
				await removeResource(`${action.path}/background-image`)
				return 'Cover image removed.'
			}
			const image = form.get('image')
			if (!(image instanceof File) || !image.size) {
				throw new Error('Choose a JPEG or PNG image.')
			}
			if (!['image/jpeg', 'image/png'].includes(image.type) || image.size > 5 * 1024 * 1024) {
				throw new Error('Choose a JPEG or PNG up to 5 MiB.')
			}
			await uploadCover(action.path, image)
			return 'Cover image saved.'
		}
		case 'delete':
			await removeResource(action.path)
			return 'Deleted successfully.'
	}
}

export function useResourceEditor({ mode, notify }: Pick<AppContext, 'mode' | 'notify'>) {
	const [action, setAction] = useState<EditorAction | null>(null)

	return {
		open: setAction,
		dialog: action && (
			<ResourceEditor
				action={action}
				mode={mode}
				onClose={() => setAction(null)}
				onSaved={(message) => {
					notify(message)
					setAction(null)
				}}
			/>
		),
	}
}

function ResourceEditor({
	action,
	mode,
	onClose,
	onSaved,
}: {
	action: EditorAction
	mode: ColorMode
	onClose: () => void
	onSaved: (message: string) => void
}) {
	const queryClient = useQueryClient()
	const [pending, setPending] = useState(false)
	const [error, setError] = useState('')

	async function submit(event: FormEvent<HTMLFormElement>) {
		event.preventDefault()
		if (pending) {
			return
		}
		const submitter = (event.nativeEvent as SubmitEvent).submitter
		const form = new FormData(event.currentTarget, submitter)
		setPending(true)
		setError('')
		try {
			const message = await submitAction(action, form)
			void queryClient.invalidateQueries({ queryKey: browseQueryKey })
			onSaved(message)
		} catch (failure) {
			setError(
				failure instanceof ApiError && failure.status >= 500
					? 'The service could not save this change. Try again.'
					: failure instanceof Error
						? failure.message
						: 'The change could not be saved.',
			)
			setPending(false)
		}
	}

	return (
		<Modal
			isOpen
			onOpenChange={(open) => {
				if (!open && !pending) {
					onClose()
				}
			}}
		>
			<Modal.Backdrop
				className="bloom-theme editor-overlay"
				data-mode={mode}
				isDismissable={!pending}
				isKeyboardDismissDisabled={pending}
			>
				<Modal.Container placement="center">
					<Modal.Dialog className="editor-dialog">
						<Modal.Header>
							<Modal.Heading>{actionTitle(action)}</Modal.Heading>
							<Button
								variant="ghost"
								isIconOnly
								isDisabled={pending}
								aria-label="Close dialog"
								onPress={onClose}
							>
								<Icon icon={X} />
							</Button>
						</Modal.Header>
						<Modal.Body>
							<form className="editor-form" onSubmit={submit}>
								<EditorFields action={action} />
								{error && (
									<p className="form-error" role="alert">
										{error}
									</p>
								)}
								<Modal.Footer>
									<Button
										variant="outline"
										isDisabled={pending}
										onPress={onClose}
									>
										Cancel
									</Button>
									{action.kind === 'cover' &&
										action.record.backgroundImageUrl && (
											<Button
												variant="outline"
												type="submit"
												name="operation"
												value="remove"
												isDisabled={pending}
											>
												Remove image
											</Button>
										)}
									<Button type="submit" isPending={pending}>
										{action.kind === 'delete'
											? `Delete ${action.resource}`
											: 'Save'}
									</Button>
								</Modal.Footer>
							</form>
						</Modal.Body>
					</Modal.Dialog>
				</Modal.Container>
			</Modal.Backdrop>
		</Modal>
	)
}

function EditorFields({ action }: { action: EditorAction }) {
	switch (action.kind) {
		case 'name':
			return (
				<TextField name="name" defaultValue={action.name} isRequired>
					<Label>Name</Label>
					<Input
						dir="auto"
						minLength={action.resource === 'language' ? 1 : 5}
						maxLength={action.resource === 'language' ? 100 : 20}
					/>
				</TextField>
			)
		case 'entry':
			return (
				<>
					<TextField name="text" defaultValue={action.entry?.text} isRequired>
						<Label>Word or phrase</Label>
						<Input dir="auto" maxLength={500} />
					</TextField>
					<TextField
						name="translation"
						defaultValue={action.entry?.translation}
						isRequired
					>
						<Label>Translation</Label>
						<TextArea dir="auto" maxLength={500} rows={2} />
					</TextField>
					<ChoiceGroup
						name="type"
						label="Entry type"
						defaultValue={String(action.entry?.type ?? 0)}
						choices={[
							{ value: '0', label: 'Word' },
							{ value: '1', label: 'Phrase' },
						]}
					/>
					<TextField
						name="phonetics"
						defaultValue={action.entry?.phoneticTranscription ?? ''}
					>
						<Label>Pronunciation (optional)</Label>
						<Input dir="auto" maxLength={500} />
					</TextField>
				</>
			)
		case 'cover':
			return <CoverFields record={action.record} />
		case 'delete':
			return (
				<p>
					Delete <strong>{action.name}</strong> permanently?{' '}
					{action.resource === 'language'
						? 'Its dictionaries and entries will also be deleted.'
						: action.resource === 'dictionary'
							? 'Its entries will also be deleted.'
							: ''}
				</p>
			)
	}
}

function CoverFields({ record }: { record: RecordSummary }) {
	const [preview, setPreview] = useState<string>()
	const source = preview ?? record.backgroundImageUrl

	useEffect(() => {
		if (preview) {
			return () => URL.revokeObjectURL(preview)
		}
	}, [preview])

	return (
		<>
			{source && (
				<img
					className="editor-cover-preview"
					src={source}
					alt={`Cover for ${record.name}`}
				/>
			)}
			<label className="editor-file">
				Image file
				<input
					type="file"
					name="image"
					accept="image/jpeg,image/png"
					onChange={(event) => {
						const image = event.currentTarget.files?.[0]
						setPreview(
							image && ['image/jpeg', 'image/png'].includes(image.type)
								? URL.createObjectURL(image)
								: undefined,
						)
					}}
				/>
			</label>
			<p className="editor-field-help">
				JPEG or PNG, at least 500 × 500 pixels and up to 5 MiB.
			</p>
		</>
	)
}

export function AddRecordButton({
	resource,
	onPress,
}: {
	resource: Resource
	onPress: () => void
}) {
	return (
		<Button onPress={onPress}>
			<Icon icon={Plus} size={17} /> Add {resource}
		</Button>
	)
}

export function RecordActions({
	mode,
	resource,
	path,
	record,
	open,
}: {
	mode: ColorMode
	resource: 'language' | 'dictionary'
	path: string
	record: RecordSummary
	open: (action: EditorAction) => void
}) {
	return (
		<RecordMenu
			mode={mode}
			label={`${record.name} actions`}
			open={open}
			items={[
				{
					label: `Edit ${resource}`,
					action: { kind: 'name', resource, path, name: record.name },
				},
				{ label: 'Cover image', action: { kind: 'cover', path, record } },
				{
					label: `Delete ${resource}`,
					action: { kind: 'delete', resource, path, name: record.name },
				},
			]}
		/>
	)
}

const actionIcons = { name: Pencil, entry: Pencil, cover: ImagePlus, delete: Trash2 }

export function RecordMenu({
	mode,
	label,
	items,
	open,
}: {
	mode: ColorMode
	label: string
	items: { label: string; action: EditorAction }[]
	open: (action: EditorAction) => void
}) {
	return (
		<Dropdown>
			<Button variant="ghost" isIconOnly aria-label={label}>
				<Icon icon={Ellipsis} />
			</Button>
			<Dropdown.Popover className="bloom-theme app-menu" data-mode={mode}>
				<Dropdown.Menu
					aria-label={label}
					onAction={(key) => {
						const item = items.find(({ label }) => label === key)
						if (item) {
							open(item.action)
						}
					}}
				>
					{items.map(({ label, action }) => (
						<Dropdown.Item
							key={label}
							id={label}
							textValue={label}
							className={action.kind === 'delete' ? 'danger-text' : undefined}
						>
							<Icon icon={actionIcons[action.kind]} size={17} />
							<span>{label}</span>
						</Dropdown.Item>
					))}
				</Dropdown.Menu>
			</Dropdown.Popover>
		</Dropdown>
	)
}
