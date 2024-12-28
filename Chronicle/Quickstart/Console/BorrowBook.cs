using System.Collections.ObjectModel;
using System.ComponentModel;
using MongoDB.Driver;
using Terminal.Gui;

namespace Quickstart;

public partial class BorrowBook
{
    public BorrowBook()
    {
        InitializeComponent();

        var okButton = new Button
        {
            Text = "OK",
            IsDefault = true
        };
        okButton.Accept += Save;
        AddButton(okButton);

        var books = Globals.Books.GetAll();
        _books.Source = new ListWrapper<string>(new ObservableCollection<string>(books.Select(_ => _.Title)));
    }

    void Save(object? sender, HandledEventArgs e)
    {
        Application.RequestStop();
    }
}
