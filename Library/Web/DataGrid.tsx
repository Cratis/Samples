// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

export interface DataGridProps {
    title: string;
    whatever?: string;
}


export const DataGrid = (props: DataGridProps) => {
    return (
        <div>
            <h1>DataGrid</h1>
            <h2>{props.title}</h2>
        </div>
    );
};
