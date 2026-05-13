// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.ObjectModel;
using Terminal.Gui;

namespace Quickstart.Dialogs;

public partial class UserAndBookSelector
{
    readonly IList<User> _allUsers;
    readonly IList<Book> _allBooks;

    public UserAndBookSelector(Func<User, Book, Task>? returnAction = default)
    {
        InitializeComponent();

        var okButton = new Button
        {
            Text = "OK",
            IsDefault = true
        };
        AddButton(okButton);

        _allUsers = [.. Globals.Users.GetAll()];
        _allBooks = [.. Globals.Books.GetAll()];
        _borrowers.Source = _allUsers.Count > 0
            ? new ListWrapper<string>([.. _allUsers.Select(u => u.Name ?? string.Empty)])
            : new ListWrapper<string>(["(No users registered)"]);
        _books.Source = _allBooks.Count > 0
            ? new ListWrapper<string>([.. _allBooks.Select(b => b.Title ?? string.Empty)])
            : new ListWrapper<string>(["(No books in inventory)"]);

        okButton.Accepting += async (s, e) =>
        {
            var userIndex = _borrowers.SelectedItem;
            var bookIndex = _books.SelectedItem;

            if (returnAction is not null
                && _allUsers.Count > 0 && userIndex >= 0 && userIndex < _allUsers.Count
                && _allBooks.Count > 0 && bookIndex >= 0 && bookIndex < _allBooks.Count)
            {
                await returnAction(_allUsers[userIndex], _allBooks[bookIndex]);
            }

            Application.RequestStop();
        };
    }
}
