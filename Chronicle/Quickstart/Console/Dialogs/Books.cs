// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.ObjectModel;
using Terminal.Gui;

namespace Quickstart.Dialogs;

public partial class Books
{
    public Books(Func<Book, Task>? returnAction = default)
    {
        InitializeComponent();

        var button = new Button
        {
            Text = "OK",
            IsDefault = true
        };
        AddButton(button);

        IList<Book> books = [.. Globals.Books.GetAll()];
        _books.Source = books.Count > 0
            ? new ListWrapper<string>([.. books.Select(b => b.Title ?? string.Empty)])
            : new ListWrapper<string>(["(No books in inventory)"]);

        button.Accepting += async (s, e) =>
        {
            var index = _books.SelectedItem;

            if (returnAction is not null && books.Count > 0 && index >= 0 && index < books.Count)
            {
                await returnAction(books[index]);
            }

            Application.RequestStop();
        };
    }
}
