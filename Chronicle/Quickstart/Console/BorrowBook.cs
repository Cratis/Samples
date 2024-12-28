using System.Collections.ObjectModel;
using System.ComponentModel;
using MongoDB.Driver;
using Terminal.Gui;

namespace Quickstart;

public partial class BorrowBook
{
    readonly IEnumerable<User> _allUsers;
    readonly IEnumerable<Book> _allBooks;

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

        _allUsers = Globals.Users.GetAll().ToList();
        _allBooks = Globals.Books.GetAll().ToList();
        _borrowers.Source = new ListWrapper<User>(new ObservableCollection<User>(_allUsers));
        _books.Source = new ListWrapper<Book>(new ObservableCollection<Book>(_allBooks));
    }

    void Save(object? sender, HandledEventArgs e)
    {
        var user = _allUsers.ElementAt(_borrowers.SelectedItem);
        var book = _allBooks.ElementAt(_books.SelectedItem);

        Globals.EventStore.EventLog.Append(book.Id, new BookBorrowed(user.Id));
        Application.RequestStop();
    }
}
