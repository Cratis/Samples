using System.ComponentModel;
using Terminal.Gui;

namespace Quickstart.Dialogs;

public partial class AddBook
{
    public AddBook()
    {
        InitializeComponent();

        _save.Accept += Save;
        _cancel.Accept += Cancel;
    }

    void Save(object? sender, HandledEventArgs e)
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
