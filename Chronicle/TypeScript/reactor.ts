// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { reactor, EventContext } from '@cratis/chronicle';
import { VisitorArrived, VisitorWelcomed } from './events';

/**
 * Reacts to visitors signing the guestbook by welcoming them.
 *
 * Reactors are the "if this then that" mechanism of event sourcing: they observe
 * events and produce side effects. Returning an event from a handler appends it —
 * here to the same event source that triggered the reactor — so the welcome
 * becomes a fact in the history alongside the arrival.
 *
 * Key rules:
 * - Handlers must be idempotent — the reactor may be called more than once for the same event.
 * - Never query state inside a reactor; use the event data directly.
 */
@reactor()
export class ConciergeReactor {
    async visitorArrived(event: VisitorArrived, context: EventContext): Promise<VisitorWelcomed> {
        console.log(`[react]  ConciergeReactor saw VisitorArrived('${event.name}') at sequence ${context.sequenceNumber} and responds with a VisitorWelcomed event.`);
        return new VisitorWelcomed(event.name, `Welcome, ${event.name}!`);
    }
}
