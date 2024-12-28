using System.Collections.ObjectModel;
using Terminal.Gui;

namespace Quickstart;

public partial class Library
{
    public Library()
    {
        InitializeComponent();
        _mainMenu.Source = new ListWrapper<MainMenuItem>(
        [
            new(MainMenuCommand.InitializeWithBooks, "Initialize with books"),
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
                Application.Run(new BorrowBook());
                break;
            case MainMenuCommand.ReturnBook:
                Application.Run(new ReturnBook());
                break;
            case MainMenuCommand.SetBookAsOverdue:
                break;
            case MainMenuCommand.ReserveBook:
                break;
            case MainMenuCommand.CancelReservation:
                break;
            case MainMenuCommand.ShowBooks:
                Application.Run(new Books());
                break;
            case MainMenuCommand.ShowBooksBorrowed:
                Application.Run(new BorrowedBooks());
                break;
        }
    }
}
