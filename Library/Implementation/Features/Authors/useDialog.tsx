// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { DialogContext, IDialogContext } from '@cratis/applications.react.mvvm/dialogs';
import { DialogResult } from '@cratis/applications.react/dialogs';
import { useCallback, useRef, useState, ComponentType, FC, useMemo } from 'react';

type DialogCallback<T> = (result: DialogResult, value?: T) => void;

export interface DialogProps<T> {
    closeDialog: DialogCallback<T>;
}

type ActualDialogProps<T> = Omit<T, 'closeDialog'>;

export type DialogResponse<T> = [DialogResult, T?];

export function useDialog<TResponse, TProps extends DialogProps<TResponse>>(
    DialogComponent: ComponentType<TProps>
): [
        FC<ActualDialogProps<TProps>>,
        (props?: ActualDialogProps<TProps>) => Promise<[DialogResult, TResponse?]>
    ] {
    const [visible, setVisible] = useState(false);
    const [props, setProps] = useState<ActualDialogProps<TProps> | undefined>();
    const resolverRef = useRef<(result: [DialogResult, TResponse?]) => void | undefined>(undefined);

    const showDialog = useCallback((p?: ActualDialogProps<TProps>) => {
        setProps(p);
        setVisible(true);
        return new Promise<DialogResponse<TResponse>>((resolve) => {
            resolverRef.current = resolve;
        });
    }, []);

    const handleResolve = useCallback((result: DialogResult, value?: TResponse) => {
        resolverRef.current?.([result, value]);
        resolverRef.current = undefined;
        setVisible(false);
    }, []);

    const dialogContextValue = useRef<IDialogContext<TProps, TResponse>>(undefined!);
    dialogContextValue.current = useMemo(() => {
        return {
            request: undefined!,
            actualResolver: undefined!,
            resolver: (response) => handleResolve(DialogResult.Ok, response)
        };
    }, []);

    const DialogWrapper: FC<ActualDialogProps<TProps>> = (extraProps) => {
        return visible ? (
            <DialogContext.Provider value={dialogContextValue.current as unknown as IDialogContext<object, object>}>
                <DialogComponent
                    {...(props as TProps)}
                    {...extraProps}
                    closeDialog={handleResolve}
                />
            </DialogContext.Provider>
        ) : null;
    }

    return [DialogWrapper, showDialog];
}
