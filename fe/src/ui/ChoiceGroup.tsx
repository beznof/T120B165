import { Label, Radio, RadioGroup } from '@heroui/react'

export function ChoiceGroup({
	name,
	label,
	defaultValue,
	choices,
}: {
	name: string
	label: string
	defaultValue: string
	choices: { value: string; label: string }[]
}) {
	return (
		<RadioGroup
			className="catalog-choices"
			name={name}
			defaultValue={defaultValue}
			orientation="horizontal"
		>
			<Label>{label}</Label>
			{choices.map((choice) => (
				<Radio key={choice.value} value={choice.value}>
					<Radio.Content>
						<Radio.Control>
							<Radio.Indicator />
						</Radio.Control>
						<Label>{choice.label}</Label>
					</Radio.Content>
				</Radio>
			))}
		</RadioGroup>
	)
}
