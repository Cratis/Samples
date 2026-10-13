// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

/** What executing a modeled command came back with. */
export interface StageCommandOutcome {
    isSuccess: boolean;
    response?: unknown;
    validationErrors: { member: string; message: string }[];

    /** Why the command did not run or failed, as a person should read it. Empty on success. */
    messages: string[];
}

/**
 * Posts a modeled command to the route its application registered it under.
 *
 * Arc answers a rejected command with 200 and the reasons, so the status alone does not say whether it worked;
 * reading the body is what makes a failure a failure.
 *
 * @param command The command name, for the messages.
 * @param route The registered route.
 * @param values The command's values.
 * @returns The outcome.
 */
export async function executeStageCommand(command: string, route: string, values: Record<string, unknown>): Promise<StageCommandOutcome> {
    const response = await fetch(route, {
        method: 'POST',
        headers: { 'content-type': 'application/json' },
        body: JSON.stringify(values),
    });

    if (!response.ok) return { isSuccess: false, validationErrors: [], messages: [`“${command}” failed with ${response.status}.`] };

    const result = await response.json() as { isSuccess?: boolean; response?: unknown; validationResults?: { message: string; members: string[] }[]; exceptionMessages?: string[] };
    const validationErrors = (result.validationResults ?? []).map(_ => ({ member: _.members[0] ?? '', message: _.message }));
    const isSuccess = result.isSuccess !== false && validationErrors.length === 0;
    const messages = isSuccess ? [] : [...validationErrors.map(_ => _.message), ...(result.exceptionMessages ?? [])];
    return { isSuccess, response: isSuccess ? result.response : undefined, validationErrors, messages: isSuccess || messages.length > 0 ? messages : [`“${command}” was not accepted.`] };
}
