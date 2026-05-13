// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

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

        addButton.Accepting += AddBookToInventory;
        cancelButton.Accepting += Cancel;
    }

    async void AddBookToInventory(object? sender, CommandEventArgs e)
    {
        var title = _title.Text;
        var author = _author.Text;
        var isbn = _isbn.Text;

        await Globals.EventStore.EventLog.Append(Guid.NewGuid(), new BookAddedToInventory(title, author, isbn));
        Application.RequestStop();
    }

    void Cancel(object? sender, CommandEventArgs e) => Application.RequestStop();
}
