using System.Collections.ObjectModel;
using MongoDB.Driver;
using Terminal.Gui;

namespace Quickstart.Dialogs;

public partial class Books
{
    public Books(Action<Book>? returnAction = default)
    {
        InitializeComponent();

        var button = new Button
        {
            Text = "OK",
            IsDefault = true
        };
        AddButton(button);

        var books = Globals.Books.GetAll();
        _books.Source = new ListWrapper<string>(new ObservableCollection<string>(books.Select(_ => _.Title)));

        button.Accept += (s, e) =>
        {
            returnAction?.Invoke(books.ElementAt(_books.SelectedItem));
            Application.RequestStop();
        };
    }
}
