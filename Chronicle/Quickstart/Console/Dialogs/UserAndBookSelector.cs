// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.ObjectModel;
using System.ComponentModel;
using MongoDB.Driver;
using Terminal.Gui;

namespace Quickstart.Dialogs;

public partial class UserAndBookSelector
{
    readonly IEnumerable<User> _allUsers;
    readonly IEnumerable<Book> _allBooks;

    public UserAndBookSelector(Action<User, Book>? returnAction = default)
    {
        InitializeComponent();

        var okButton = new Button
        {
            Text = "OK",
            IsDefault = true
        };
        AddButton(okButton);

        _allUsers = [..Globals.Users.GetAll()];
        _allBooks = [..Globals.Books.GetAll()];
        _borrowers.Source = new ListWrapper<User>([.. _allUsers]);
        _books.Source = new ListWrapper<Book>([.. _allBooks]);

        okButton.Accepting += (s, e) =>
        {
            returnAction?.Invoke(_allUsers.ElementAt(_borrowers.SelectedItem), _allBooks.ElementAt(_books.SelectedItem));
            Application.RequestStop();
        };
    }
}
