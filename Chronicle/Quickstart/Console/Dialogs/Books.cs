// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.ObjectModel;
using MongoDB.Driver;
using Terminal.Gui;

namespace Quickstart.Dialogs;

public partial class Books
{
    public Books(Action<Book>? returnAction = default)
    {
        InitializeComponent();

        var button = new Button
        {
            Text = "OK",
            IsDefault = true
        };
        AddButton(button);

        var books = Globals.Books.GetAll();
        _books.Source = new ListWrapper<string>([.. books.Select(_ => _.Title)]);

        button.Accepting += (s, e) =>
        {
            returnAction?.Invoke(books.ElementAt(_books.SelectedItem));
            Application.RequestStop();
        };
    }
}
