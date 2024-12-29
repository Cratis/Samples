using System.Collections.ObjectModel;
using System.ComponentModel;
using Terminal.Gui;

namespace Quickstart.Dialogs;

public partial class BorrowedBooks
{
    readonly IEnumerable<BorrowedBook> _allBooks;
    readonly Action<BorrowedBook>? _returnAction;

    public BorrowedBooks(Action<BorrowedBook>? returnAction = default)
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
        _returnAction = returnAction;
    }

    void Return(object? sender, HandledEventArgs e)
    {
        _returnAction?.Invoke(_allBooks.ElementAt(_books.SelectedItem));
        Application.RequestStop();
    }
}
