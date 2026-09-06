// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { eventType } from '@cratis/chronicle';

/**
 * A visitor has signed the guestbook.
 * This event is the source of truth for every visit — if there is no
 * VisitorArrived event, the visit never happened.
 */
@eventType()
export class VisitorArrived {
    constructor(readonly name: string = '') {}
}

/**
 * A visitor has been welcomed.
 *
 * Appended by {@link ConciergeReactor} as a side effect of a {@link VisitorArrived}
 * event — the program never appends this event directly, which is what makes the
 * reaction visible in the history this sample reads back.
 */
@eventType()
export class VisitorWelcomed {
    constructor(
        readonly name: string = '',
        readonly greeting: string = ''
    ) {}
}
