// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.ObjectModel;
using Quickstart.Dialogs;
using Terminal.Gui;

namespace Quickstart;

public partial class Library
{
    public Library()
    {
        InitializeComponent();
        _mainMenu.Source = new ListWrapper<MainMenuItem>(
        [
            new(MainMenuCommand.InitializeWithBooks, "Initialize demo data"),
            new(MainMenuCommand.AddBookToInventory, "Add book to inventory"),
            new(MainMenuCommand.BorrowBook, "Borrow book"),
            new(MainMenuCommand.ReturnBook, "Return book"),
            new(MainMenuCommand.SetBookAsOverdue, "Set book as overdue"),
            new(MainMenuCommand.ReserveBook, "Reserve book"),
            new(MainMenuCommand.CancelReservation, "Cancel reservation"),
            new(MainMenuCommand.ShowBooks, "List books"),
            new(MainMenuCommand.ShowBooksBorrowed, "List borrowed books"),
            new(MainMenuCommand.ShowBooksOverdue, "List overdue books"),
            new(MainMenuCommand.ShowReservations, "List reservations"),
        ]);
        _mainMenu.OpenSelectedItem += PerformCommand;
    }

    void PerformCommand(object? sender, ListViewItemEventArgs e)
    {
        var item = (e.Value as MainMenuItem)!;
        switch (item.Command)
        {
            case MainMenuCommand.InitializeWithBooks:
                _ = Task.Run(async () =>
                {
                    await Globals.DemoData.Initialize();
                    Application.Invoke(() => MessageBox.Query("Demo data initialized", "Demo data has been initialized", "OK"));
                });
                break;
            case MainMenuCommand.AddBookToInventory:
                Application.Run(new AddBook());
                break;
            case MainMenuCommand.BorrowBook:
                Application.Run(new UserAndBookSelector((user, book) => Globals.EventStore.EventLog.Append(book.Id, new BookBorrowed(user.Id))));
                break;
            case MainMenuCommand.ReturnBook:
                Application.Run(new Dialogs.BorrowedBooks(book => Globals.EventStore.EventLog.Append(book.Id, new BookReturned())));
                break;
            case MainMenuCommand.SetBookAsOverdue:
                Application.Run(new Dialogs.BorrowedBooks(book => Globals.EventStore.EventLog.Append(book.Id, new BookOverdue())));
                break;
            case MainMenuCommand.ReserveBook:
                Application.Run(new UserAndBookSelector((user, book) => Globals.EventStore.EventLog.Append(book.Id, new BookReservationPlaced(user.Id))));
                break;
            case MainMenuCommand.CancelReservation:
                Application.Run(new Dialogs.ReservedBooks(book => Globals.EventStore.EventLog.Append(book.Id, new BookReservationCancelled())));
                break;
            case MainMenuCommand.ShowBooks:
                Application.Run(new Dialogs.Books());
                break;
            case MainMenuCommand.ShowBooksBorrowed:
                Application.Run(new Dialogs.BorrowedBooks());
                break;
            case MainMenuCommand.ShowBooksOverdue:
                Application.Run(new Dialogs.OverdueBooks());
                break;
            case MainMenuCommand.ShowReservations:
                Application.Run(new Dialogs.ReservedBooks());
                break;
        }
    }
}
