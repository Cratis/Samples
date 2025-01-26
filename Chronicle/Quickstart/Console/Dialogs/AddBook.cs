// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.ComponentModel;
using Terminal.Gui;

namespace Quickstart.Dialogs;

public partial class AddBook
{
    public AddBook()
    {
        InitializeComponent();

        var addButton = new Button
        {
            Text = "Add",
            IsDefault = true
        };
        AddButton(addButton);

        var cancelButton = new Button
        {
            Text = "Cancel",
        };
        AddButton(cancelButton);

        addButton.Accept += AddBookToInventory;
        cancelButton.Accept += Cancel;
    }

    void AddBookToInventory(object? sender, HandledEventArgs e)
    {
        var title = _title.Text;
        var author = _author.Text;
        var isbn = _isbn.Text;

        var bookAddedToInventory = new BookAddedToInventory(title, author, isbn);
        Globals.EventStore.EventLog.Append(Guid.NewGuid(), bookAddedToInventory);
        Application.RequestStop();
    }

    void Cancel(object? sender, HandledEventArgs e) => Application.RequestStop();
}
