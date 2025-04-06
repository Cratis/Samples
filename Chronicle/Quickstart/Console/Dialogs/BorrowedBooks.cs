// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.ObjectModel;
using System.ComponentModel;
using Terminal.Gui;

namespace Quickstart.Dialogs;

public partial class BorrowedBooks
{
    public BorrowedBooks(Action<BorrowedBook>? returnAction = default)
    {
        InitializeComponent();

        var okButton = new Button
        {
            Text = "OK",
            IsDefault = true
        };
        AddButton(okButton);

        var allBooks = Globals.BorrowedBooks.GetAll();
        _books.Source = new ListWrapper<BorrowedBook>([.. allBooks]);

        okButton.Accepting += (s, e) =>
        {
            returnAction?.Invoke(allBooks.ElementAt(_books.SelectedItem));
            Application.RequestStop();
        };
    }
}
