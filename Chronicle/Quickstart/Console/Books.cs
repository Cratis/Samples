using System.Collections.ObjectModel;
using MongoDB.Driver;
using Terminal.Gui;

namespace Quickstart;

public partial class Books
{
    public Books()
    {
        InitializeComponent();

        var button = new Button
        {
            Text = "OK",
            IsDefault = true
        };
        button.Accept += (s, e) => Application.RequestStop();
        AddButton(button);

        var books = Globals.Books.GetAll();
        _books.Source = new ListWrapper<string>(new ObservableCollection<string>(books.Select(_ => _.Title)));
    }
}
