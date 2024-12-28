using System.Collections.ObjectModel;
using System.ComponentModel;
using Terminal.Gui;

namespace Quickstart;

public partial class ReturnBook
{
    readonly IEnumerable<BorrowedBook> _allBooks;

    public ReturnBook()
    {
        InitializeComponent();

        var okButton = new Button
        {
            Text = "OK",
            IsDefault = true
        };
        okButton.Accept += Return;
        AddButton(okButton);

        _allBooks = Globals.BorrowedBooks.GetAll().ToList();
        _books.Source = new ListWrapper<BorrowedBook>(new ObservableCollection<BorrowedBook>(_allBooks));
    }

    void Return(object? sender, HandledEventArgs e)
    {
        var borrowedBook = _allBooks.ElementAt(_books.SelectedItem);
        Globals.EventStore.EventLog.Append(borrowedBook.Id, new BookReturned());
        Application.RequestStop();
    }
}
