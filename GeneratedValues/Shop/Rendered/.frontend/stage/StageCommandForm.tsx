// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { useMemo, useState } from 'react';
import type { CSSProperties, ReactNode } from 'react';
import type { CommandFormLayout, ExternalComponent, FormColumn, FormField, FormFieldPlacement, FormWidth } from '@cratis/scene.model';
import { FormGenerationMode, FormWidthUnit } from '@cratis/scene.model';
import { Command } from '@cratis/arc/commands';
import { PropertyDescriptor } from '@cratis/arc/reflection';
import { CommandForm as NativeCommandForm } from '@cratis/arc.react/commands';
import { CheckboxField, InputTextField, NumberField, CalendarField } from '@cratis/components/CommandForm';
import { useStageData } from './stageData';

interface SchemaProperty {
    name: string;
    type?: 'text' | 'number' | 'boolean' | 'date';
    required: boolean;
    unsupportedReason?: string;
}

interface NativeFormProperties {
    command: string;
    label: string;
    fields: FormField[];
    route?: string;
    generationMode: FormGenerationMode;
    layout?: CommandFormLayout;
}

interface StageCommandFormProps {
    element: ExternalComponent;
}

interface StageCommandContent {
    [key: string]: unknown;
}

interface JsonSchemaDefinition {
    type?: unknown;
    format?: unknown;
}

type StageCommandConstructor = new () => Command<StageCommandContent, object>;

/** Renders an authored screen form through Cratis Arc's native CommandForm boundary. */
export function StageCommandForm({ element }: StageCommandFormProps) {
    const data = useStageData();
    const form = useMemo(() => formProperties(element), [element]);
    const schema = useMemo(() => schemaProperties(element), [element]);
    const hasSchemaMetadata = typeof element.properties.schema === 'string';
    const fields = useMemo(() => form ? fieldsFor(form, schema) : [], [form, schema]);
    const route = form?.route ?? (form ? data.routes?.commands[form.command] : undefined);
    const argumentValues = useArgumentValues(element, fields, data.resolveArgument);
    const initialValues = useMemo(() => initialCommandValues(fields, data.parameters), [fields, data.parameters]);
    const commandSignature = useMemo(() => route ? commandSignatureFor(route, fields, schema, initialValues) : '', [route, fields, schema, initialValues]);
    const commandType = useMemo(() => route ? commandTypeFor(route, fields, schema, initialValues) : undefined, [route, commandSignature]);
    const renderedFields = useMemo(() => form ? renderFields(form, fields, schema) : undefined, [form, fields, schema]);
    const unsupportedSchemaMessages = useMemo(() => form ? unsupportedSchemaMessagesFor(fields, schema) : [], [form, fields, schema]);
    const unsupportedGeometryMessages = useMemo(() => form ? unsupportedGeometryMessagesFor(form.layout, fields) : [], [form, fields]);
    const [messages, setMessages] = useState<string[]>([]);

    if (!form) return <p className='stage-note'>This form is missing its command metadata.</p>;
    if (!route && !data.routesReady) return <p className='stage-note'>The modeled command “{form.command}” is waiting for Stage routes.</p>;
    if (!route || !commandType) return <p className='stage-note'>The modeled command “{form.command}” is not registered by this Stage.</p>;
    if (fields.length === 0 && !hasSchemaMetadata && element.properties.parameterless !== true) {
        return <UnsupportedForm form={form} messages={[`The modeled command “${form.command}” has no form fields or schema metadata, so Stage will not submit an empty command body.`]} />;
    }

    if (unsupportedSchemaMessages.length > 0) return <UnsupportedForm form={form} messages={unsupportedSchemaMessages} />;

    return (
        <section className='stage-command-form' data-command={form.command} data-generation-mode={form.generationMode}>
            <NativeCommandForm
                command={commandType}
                initialValues={initialValues}
                currentValues={argumentValues}
                validateOn='both'
                validateAllFieldsOnChange
                onFieldValidate={(_command, fieldName, _oldValue, newValue) => requiredMessage(fields, schema, fieldName, newValue)}
                onValidationFailure={validation => setMessages(validation.map(_ => _.message))}
                onException={exceptionMessages => setMessages(exceptionMessages)}
                onFailed={result => setMessages(messagesFromResult(result))}
                onSuccess={() => {
                    setMessages([]);
                    data.refreshAfterCommand();
                }}>
                {unsupportedGeometryMessages.map(message => <p key={message} role='alert' className='stage-form-error'>{message}</p>)}
                {renderedFields}
                {messages.map(message => <p key={message} role='alert' className='stage-form-error'>{message}</p>)}
                <button type='submit'>Execute {form.label}</button>
            </NativeCommandForm>
        </section>
    );
}

function UnsupportedForm({ form, messages }: { form: NativeFormProperties; messages: string[] }) {
    return (
        <section className='stage-command-form' data-command={form.command} data-generation-mode={form.generationMode}>
            {messages.map(message => <p key={message} role='alert' className='stage-form-error'>{message}</p>)}
        </section>
    );
}

function formProperties(element: ExternalComponent): NativeFormProperties | undefined {
    const command = text(element, 'command');
    if (!command) return undefined;

    const fields = Array.isArray(element.properties.fields) ? element.properties.fields.filter(isRecord) as unknown as FormField[] : [];
    return {
        command,
        label: text(element, 'label', command),
        fields,
        route: text(element, 'route') || undefined,
        generationMode: generationModeFor(element.properties.generationMode),
        layout: layoutFor(element.properties.layout),
    };
}

function fieldsFor(form: NativeFormProperties, schema: SchemaProperty[]): FormField[] {
    if (form.generationMode === FormGenerationMode.Manual || form.fields.length > 0) return form.fields;
    return schema.filter(property => !property.unsupportedReason).map(property => ({ name: property.name, label: property.name }));
}

function renderFields(form: NativeFormProperties, fields: FormField[], schema: SchemaProperty[]): ReactNode {
    const layout = form.layout;
    const placements = layout ? placementsFor(layout, fields) : [];
    if (!layout || (layout.columns.length === 0 && placements.length === 0)) return fields.map(field => renderField(field, schema));

    const byName = new Map(fields.map(field => [field.name, field]));
    const columnCount = gridColumnCount(layout, placements);
    const layoutStyle: CSSProperties = {
        gridTemplateColumns: Array.from({ length: columnCount }, (_value, index) => columnTrack(index + 1, layout.columns)).join(' '),
        columnGap: widthToCss(layout.columnGap),
        rowGap: widthToCss(layout.rowGap),
    };
    const placedFields = new Set<string>();

    return (
        <div className='stage-command-form__layout' style={layoutStyle}>
            {placements.map(placement => {
                const field = byName.get(placement.field);
                if (!field) return null;
                placedFields.add(field.name);
                return renderField(field, schema, placement);
            })}
            {fields.filter(field => !placedFields.has(field.name)).map(field => renderField(field, schema))}
        </div>
    );
}

function renderField(field: FormField, schema: SchemaProperty[], placement?: FormFieldPlacement) {
    const property = field.sourceProperty ?? field.name;
    const descriptor = schema.find(_ => _.name === property);
    const title = field.label ?? field.name;
    const accessor = (command: unknown) => (command as Record<string, unknown>)[property];
    const required = descriptor?.required ?? true;
    const fieldElement = (() => {
        switch (descriptor?.type ?? 'text') {
            case 'number': return <NumberField value={accessor} fieldName={property} title={title} required={required} />;
            case 'boolean': return <CheckboxField value={accessor} fieldName={property} label={title} required={required} />;
            case 'date': return <CalendarField value={accessor} fieldName={property} title={title} required={required} showIcon />;
            default: return <InputTextField value={accessor} fieldName={property} title={title} required={required} />;
        }
    })();

    const style = placementStyle(placement);
    return (
        <span
            key={field.name}
            className='stage-command-form__field'
            data-field={field.name}
            data-row={placement?.row}
            data-column={placement?.column}
            data-width-unit={placement?.width?.unit}
            style={style}>
            {fieldElement}
        </span>
    );
}

/**
 * The values a command starts with from the screen it is executed on.
 *
 * A screen navigated to `by workItemId` is about that work item, so a command on it that takes a `workItemId`
 * is about the same one: the parameter is carried in by exact name. Nothing else is guessed - a property the
 * screen has no parameter for stays empty and its required validation still applies, so a command is never
 * submitted for an entity the user did not pick.
 */
function initialCommandValues(fields: FormField[], parameters: Record<string, string>): StageCommandContent {
    const values: StageCommandContent = {};
    for (const field of fields) {
        const property = field.sourceProperty ?? field.name;
        const parameter = parameters[property];
        if (parameter !== undefined && parameter !== '') values[property] = parameter;
    }

    return values;
}

interface ArgumentMapping {
    name: string;
    source: string;
}

/**
 * The values an element's argument mapping supplies, resolved through Scene's argument sources. An authored
 * mapping names where each argument comes from - an input's published `value`, a table's selected item or a
 * data-context path. They are the form's current values rather than its initial ones: when a source changes -
 * another row is selected, the input is edited - the field follows it, and edits in other fields stay. A source
 * that resolves to nothing supplies nothing, so the route parameter, or an empty field, stays.
 */
function useArgumentValues(element: ExternalComponent, fields: FormField[], resolveArgument: (source: string | undefined) => unknown): StageCommandContent | undefined {
    const mapping = useMemo(() => argumentMapping(element, fields), [element, fields]);
    const resolved = mapping.map(argument => resolveArgument(argument.source));

    // `resolved` is a new array on every render; its serialized form says whether a value actually changed.
    const key = JSON.stringify(resolved);
    return useMemo(() => {
        const values = Object.fromEntries(mapping
            .map((argument, index) => [argument.name, resolved[index]] as const)
            .filter(([, value]) => value !== undefined && value !== null && value !== ''));
        return Object.keys(values).length > 0 ? values : undefined;
    }, [mapping, key]);
}

function argumentMapping(element: ExternalComponent, fields: FormField[]): ArgumentMapping[] {
    const mapping = element.properties.arguments;
    if (!Array.isArray(mapping)) return [];

    // Only the command's own members can be seeded; an argument for anything else would never be submitted.
    const members = new Set(fields.map(field => field.sourceProperty ?? field.name));
    return mapping.filter((argument): argument is ArgumentMapping =>
        typeof argument === 'object' && argument !== null &&
        typeof (argument as ArgumentMapping).name === 'string' &&
        typeof (argument as ArgumentMapping).source === 'string' &&
        members.has((argument as ArgumentMapping).name));
}

function commandSignatureFor(route: string, fields: FormField[], schema: SchemaProperty[], initialValues: StageCommandContent): string {
    const schemaByName = new Map(schema.map(property => [property.name, property]));
    return JSON.stringify({
        route: route.replace(/^\//, ''),
        initialValues,
        fields: fields.map(field => {
            const property = field.sourceProperty ?? field.name;
            const schemaProperty = schemaByName.get(property);
            return {
                name: field.name,
                sourceProperty: field.sourceProperty,
                property,
                type: schemaProperty?.type ?? 'text',
                required: schemaProperty?.required ?? true,
            };
        }),
    });
}

function commandTypeFor(route: string, fields: FormField[], schema: SchemaProperty[], initialValues: StageCommandContent): StageCommandConstructor {
    const descriptors = fields.map(field => {
        const property = field.sourceProperty ?? field.name;
        const schemaProperty = schema.find(_ => _.name === property);
        return new PropertyDescriptor(property, constructorFor(schemaProperty?.type), !(schemaProperty?.required ?? true));
    });

    return class StageNativeCommand extends Command<StageCommandContent, object> {
        readonly route = route.replace(/^\//, '');
        readonly propertyDescriptors = descriptors;
        get requestParameters(): string[] { return []; }

        constructor() {
            super(Object, false);
            (this as unknown as { _values: StageCommandContent })._values = { ...initialValues };
            for (const descriptor of descriptors) {
                Object.defineProperty(this, descriptor.name, {
                    configurable: true,
                    enumerable: true,
                    get: () => (this as unknown as { _values: StageCommandContent })._values?.[descriptor.name],
                    set: value => {
                        const target = this as unknown as { _values: StageCommandContent };
                        target._values = { ...(target._values ?? {}), [descriptor.name]: value };
                        this.propertyChanged(descriptor.name);
                    },
                });
            }
        }
    };
}

function generationModeFor(value: unknown): FormGenerationMode {
    return value === FormGenerationMode.Manual ? FormGenerationMode.Manual : FormGenerationMode.Auto;
}

function layoutFor(value: unknown): CommandFormLayout | undefined {
    if (!isRecord(value)) return undefined;
    const layout = value as Partial<CommandFormLayout>;
    return {
        columns: Array.isArray(layout.columns) ? layout.columns.filter(isFormColumn) : [],
        placements: Array.isArray(layout.placements) ? layout.placements.filter(isFormFieldPlacement) : [],
        columnGap: isFormWidth(layout.columnGap) ? layout.columnGap : undefined,
        rowGap: isFormWidth(layout.rowGap) ? layout.rowGap : undefined,
    };
}

function placementsFor(layout: CommandFormLayout, fields: FormField[]): FormFieldPlacement[] {
    const byField = new Map(layout.placements.map(placement => [placement.field, placement]));
    for (const field of fields) {
        if (byField.has(field.name) || !isFormFieldPlacement(field.placement)) continue;
        byField.set(field.name, field.placement);
    }

    return [...byField.values()].sort((left, right) => left.row - right.row || left.column - right.column || left.field.localeCompare(right.field));
}

function gridColumnCount(layout: CommandFormLayout, placements: FormFieldPlacement[]): number {
    return Math.max(
        1,
        ...layout.columns.map(column => column.index),
        ...placements.map(placement => placement.column + (placement.columnSpan ?? 1) - 1),
    );
}

function columnTrack(index: number, columns: FormColumn[]): string {
    const column = columns.find(candidate => candidate.index === index);
    const width = widthToCss(column?.width);
    const minWidth = widthToCss(column?.minWidth);
    const maxWidth = widthToCss(column?.maxWidth);
    if (column?.width?.unit === FormWidthUnit.Fraction && maxWidth) return `minmax(${minWidth ?? '0'}, ${width ?? '1fr'})`;
    if (minWidth || maxWidth) return `minmax(${minWidth ?? '0'}, ${maxWidth && width ? `min(${width}, ${maxWidth})` : maxWidth ?? width ?? '1fr'})`;
    return width ?? '1fr';
}

function placementStyle(placement: FormFieldPlacement | undefined): CSSProperties | undefined {
    if (!placement) return undefined;
    return {
        gridColumn: `${placement.column} / span ${placement.columnSpan ?? 1}`,
        gridRow: `${placement.row} / span ${placement.rowSpan ?? 1}`,
        width: placement.width?.unit === FormWidthUnit.Fraction ? undefined : widthToCss(placement.width),
    };
}

function widthToCss(width: FormWidth | undefined): string | undefined {
    if (!width) return undefined;
    switch (width.unit) {
        case FormWidthUnit.Fraction: return `${width.value ?? 1}fr`;
        case FormWidthUnit.Pixels: return `${width.value ?? 0}px`;
        case FormWidthUnit.Percent: return `${width.value ?? 100}%`;
        case FormWidthUnit.Auto: return 'auto';
    }
}

function constructorFor(type: SchemaProperty['type'] | undefined) {
    switch (type) {
        case 'number': return Number;
        case 'boolean': return Boolean;
        case 'date': return Date;
        default: return String;
    }
}

function requiredMessage(fields: FormField[], schema: SchemaProperty[], fieldName: string, value: unknown): string | undefined {
    const field = fields.find(_ => (_.sourceProperty ?? _.name) === fieldName);
    const property = schema.find(_ => _.name === fieldName);
    if (!(property?.required ?? true)) return undefined;
    if (value === undefined || value === null || value === '') return `${field?.label ?? fieldName} is required`;
    return undefined;
}

function messagesFromResult(result: { validationResults: { message: string }[]; exceptionMessages: string[] }): string[] {
    return [
        ...result.validationResults.map(_ => _.message),
        ...result.exceptionMessages,
    ];
}

function unsupportedSchemaMessagesFor(fields: FormField[], schema: SchemaProperty[]): string[] {
    const schemaByName = new Map(schema.map(property => [property.name, property]));
    return fields.flatMap(field => {
        const property = schemaByName.get(field.sourceProperty ?? field.name);
        return property?.unsupportedReason ? [`The field “${field.label ?? field.name}” uses unsupported schema metadata: ${property.unsupportedReason}.`] : [];
    });
}

function unsupportedGeometryMessagesFor(layout: CommandFormLayout | undefined, fields: FormField[]): string[] {
    if (!layout) return [];

    const placementMessages = placementsFor(layout, fields)
        .filter(placement => placement.width?.unit === FormWidthUnit.Fraction)
        .map(placement => `The field “${placement.field}” uses a fractional field width, which cannot be applied to an individual form control.`);
    const columnMessages = layout.columns
        .filter(column => column.width?.unit === FormWidthUnit.Fraction && !!column.maxWidth)
        .map(column => `Column ${column.index} combines a fractional width with maxWidth; Stage keeps the fractional track and ignores maxWidth for that column.`);

    return [...placementMessages, ...columnMessages];
}

function schemaProperties(element: ExternalComponent): SchemaProperty[] {
    const schema = text(element, 'schema');
    if (!schema) return [];
    try {
        const parsed = JSON.parse(schema) as { required?: unknown; properties?: Record<string, JsonSchemaDefinition> };
        const hasRequired = Object.prototype.hasOwnProperty.call(parsed, 'required');
        const required = Array.isArray(parsed.required) ? new Set(parsed.required.filter((name): name is string => typeof name === 'string')) : new Set<string>();
        return Object.entries(parsed.properties ?? {}).map(([name, definition]) => {
            const resolved = propertyType(definition);
            return {
                name,
                type: resolved.type,
                unsupportedReason: resolved.unsupportedReason,
                required: hasRequired ? required.has(name) : true,
            };
        });
    } catch {
        return [];
    }
}

function propertyType(definition: JsonSchemaDefinition | undefined): Pick<SchemaProperty, 'type' | 'unsupportedReason'> {
    const schemaType = definition?.type;
    if (Array.isArray(schemaType)) {
        const nonNullTypes = schemaType.filter(type => type !== 'null');
        if (nonNullTypes.length === 1) return propertyType({ ...definition, type: nonNullTypes[0] });
        return { unsupportedReason: `union type ${JSON.stringify(schemaType)}` };
    }

    if (schemaType === 'integer' || schemaType === 'number') return { type: 'number' };
    if (schemaType === 'boolean') return { type: 'boolean' };
    if (definition?.format === 'date' || definition?.format === 'date-time') return { type: 'date' };
    if (schemaType === 'array' || schemaType === 'object') return { unsupportedReason: `${schemaType} fields are not supported by the native Stage command form` };
    if (schemaType === undefined || schemaType === 'string') return { type: 'text' };
    return { unsupportedReason: `type ${String(schemaType)}` };
}

function text(element: ExternalComponent, name: string, fallback = ''): string {
    const value = element.properties[name];
    return typeof value === 'string' ? value : fallback;
}

function isFormColumn(value: unknown): value is FormColumn {
    return isRecord(value) && typeof value.index === 'number' && Number.isInteger(value.index) && value.index > 0;
}

function isFormFieldPlacement(value: unknown): value is FormFieldPlacement {
    return isRecord(value) && typeof value.field === 'string' && typeof value.row === 'number' && typeof value.column === 'number' && value.row > 0 && value.column > 0;
}

function isFormWidth(value: unknown): value is FormWidth {
    return isRecord(value) && Object.values(FormWidthUnit).includes(value.unit as FormWidthUnit);
}

function isRecord(value: unknown): value is Record<string, unknown> {
    return value !== null && typeof value === 'object' && !Array.isArray(value);
}
