// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.ObjectModel;
using Terminal.Gui;

namespace Quickstart.Dialogs;

public partial class OverdueBooks
{
    public OverdueBooks(Func<OverdueBook, Task>? returnAction = default)
    {
        InitializeComponent();

        var okButton = new Button
        {
            Text = "OK",
            IsDefault = true
        };
        AddButton(okButton);

        IList<OverdueBook> allBooks = [.. Globals.OverdueBooks.GetAll()];
        _books.Source = allBooks.Count > 0
            ? new ListWrapper<string>([.. allBooks.Select(b => b.Title ?? string.Empty)])
            : new ListWrapper<string>(["(No overdue books)"]);

        okButton.Accepting += async (s, e) =>
        {
            var index = _books.SelectedItem;

            if (returnAction is not null && allBooks.Count > 0 && index >= 0 && index < allBooks.Count)
            {
                await returnAction(allBooks[index]);
            }

            Application.RequestStop();
        };
    }
}
