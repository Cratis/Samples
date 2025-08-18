// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

export interface FormElementProps {
    children: React.ReactNode;
    icon: React.ReactNode;
}

export const FormElement = (props: FormElementProps) => {
    return (
        <div className="card flex flex-column md:flex-row gap-3">
            <div className="p-inputgroup flex-1">
                <span className="p-inputgroup-addon">
                    {props.icon}
                </span>
                {props.children}
            </div>
        </div>
    )
}
