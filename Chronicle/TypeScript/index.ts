// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import 'reflect-metadata';
import { ChronicleClient, ChronicleOptions } from '@cratis/chronicle';
import { VisitorArrived, VisitorWelcomed } from './events';

// Side-effect import so the @reactor decorator runs and the reactor is
// discovered and registered with the event store on connect.
import './reactor';

/** The single guestbook every visit in this sample is recorded against. */
const GUESTBOOK_ID = 'reception-guestbook';

async function run(): Promise<void> {
    // The sample keeps its artifacts (events, reactor) as flat files next to this
    // one, so discovery only needs to scan the top level — this also keeps the
    // sample's local node_modules out of the scan.
    const discoveryPatterns = ['*.ts', '!*.d.ts', '!*.spec.ts'];
    const options = process.env.CHRONICLE_CONNECTION
        ? ChronicleOptions.fromConnectionString(process.env.CHRONICLE_CONNECTION, { discoveryPatterns })
        : ChronicleOptions.development({ discoveryPatterns });

    const client = new ChronicleClient(options);

    try {
        const store = await client.getEventStore('TypeScriptGuestbook');

        // 1. Append — record the immutable fact that a visitor arrived.
        const name = process.argv[2] ?? 'Ada';
        const result = await store.eventLog.append(GUESTBOOK_ID, new VisitorArrived(name));
        console.log(`[append] VisitorArrived('${name}') appended to '${GUESTBOOK_ID}' at sequence ${result.sequenceNumber.value}.`);

        // 2. React — wait until every observer of the append (the ConciergeReactor)
        //    has either caught up or failed. Reading immediately without waiting can
        //    race the reactor's asynchronous processing and miss its side effect.
        const completion = await result.waitForCompletion();
        if (!completion.isSuccess) {
            console.error(`[react]  ${completion.failedPartitions.length} observer partition(s) failed while catching up on the append.`);
            process.exitCode = 1;
            return;
        }

        // 3. Read — the history now holds both the appended fact and the
        //    reactor's side-effect event.
        const history = await store.eventLog.getForEventSourceIdAndEventTypes(GUESTBOOK_ID, [VisitorArrived, VisitorWelcomed]);
        console.log(`[read]   Guestbook '${GUESTBOOK_ID}' history — ${history.length} event(s):`);
        for (const entry of history) {
            console.log(`  [seq ${entry.context.sequenceNumber}] ${entry.eventType.id.value}: ${JSON.stringify(entry.content)}`);
        }
    } finally {
        client.dispose();
    }

    process.exit(process.exitCode ?? 0);
}

run().catch(error => {
    console.error('Unhandled error:', error);
    process.exit(1);
});
