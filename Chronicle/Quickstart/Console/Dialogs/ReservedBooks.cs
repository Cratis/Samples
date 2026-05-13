// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.ObjectModel;
using Terminal.Gui;

namespace Quickstart.Dialogs;

public partial class ReservedBooks
{
    public ReservedBooks(Func<ReservedBook, Task>? returnAction = default)
    {
        InitializeComponent();

        var okButton = new Button
        {
            Text = "OK",
            IsDefault = true
        };
        AddButton(okButton);

        IList<ReservedBook> allBooks = [.. Globals.ReservedBooks.GetAll()];
        _books.Source = allBooks.Count > 0
            ? new ListWrapper<string>([.. allBooks.Select(b => b.Title ?? string.Empty)])
            : new ListWrapper<string>(["(No reserved books)"]);

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
