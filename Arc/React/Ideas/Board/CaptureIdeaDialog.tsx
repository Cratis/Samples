// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { CommandDialog } from '@cratis/components/CommandDialog';
import { InputTextField, TextAreaField } from '@cratis/components/CommandForm';
import { Guid } from '@cratis/fundamentals';

import { CaptureIdea } from './CaptureIdea';

export const CaptureIdeaDialog = () => (
    <CommandDialog<CaptureIdea>
        command={CaptureIdea}
        title="Capture an idea"
        okLabel="Add to board"
        cancelLabel="Not now"
        width="34rem"
        initialValues={{ id: Guid.create(), title: '', summary: '' }}>
        <p className="capture-idea__intro">
            Keep it crisp. A strong title earns attention; a useful summary makes the next conversation easier.
        </p>
        <InputTextField<CaptureIdea>
            value={command => command.title}
            title="Title"
            placeholder="Make local setup self-explanatory"
        />
        <TextAreaField<CaptureIdea>
            value={command => command.summary}
            title="Why it matters"
            placeholder="Describe the outcome, not the implementation."
            rows={5}
        />
    </CommandDialog>
);
