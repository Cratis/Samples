
//------------------------------------------------------------------------------

//  <auto-generated>
//      This code was generated by:
//        TerminalGuiDesigner v2.0.0.0
//      Changes to this file may cause incorrect behavior and will be lost if
//      the code is regenerated.
//  </auto-generated>
// -----------------------------------------------------------------------------
namespace Quickstart.Dialogs {
    using System;
    using Terminal.Gui;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Drawing;
    
    
    public partial class UserAndBookSelector : Terminal.Gui.Dialog {
        
        private Terminal.Gui.Label label;
        
        private Terminal.Gui.ListView _borrowers;
        
        private Terminal.Gui.Label label2;
        
        private Terminal.Gui.ListView _books;
        
        private void InitializeComponent() {
            this._books = new Terminal.Gui.ListView();
            this.label2 = new Terminal.Gui.Label();
            this._borrowers = new Terminal.Gui.ListView();
            this.label = new Terminal.Gui.Label();
            this.Width = Dim.Percent(90);
            this.Height = Dim.Percent(90);
            this.X = Pos.Center();
            this.Y = Pos.Center();
            this.Visible = true;
            this.Arrangement = Terminal.Gui.ViewArrangement.Movable;
            this.Modal = true;
            this.TextAlignment = Terminal.Gui.Alignment.Start;
            this.Title = "";
            this.label.Width = Dim.Auto();
            this.label.Height = 1;
            this.label.X = 1;
            this.label.Y = 1;
            this.label.Visible = true;
            this.label.Arrangement = Terminal.Gui.ViewArrangement.Fixed;
            this.label.Data = "label";
            this.label.Text = "Borrower";
            this.label.TextAlignment = Terminal.Gui.Alignment.Start;
            this.Add(this.label);
            this._borrowers.Width = Dim.Fill(1);
            this._borrowers.Height = 10;
            this._borrowers.X = 1;
            this._borrowers.Y = 2;
            this._borrowers.Visible = true;
            this._borrowers.Arrangement = Terminal.Gui.ViewArrangement.Fixed;
            this._borrowers.Data = "_borrowers";
            this._borrowers.TextAlignment = Terminal.Gui.Alignment.Start;
            this._borrowers.Source = new Terminal.Gui.ListWrapper<string>(new System.Collections.ObjectModel.ObservableCollection<string>(new string[] {
                            "Item1",
                            "Item2",
                            "Item3"}));
            this._borrowers.AllowsMarking = false;
            this._borrowers.AllowsMultipleSelection = true;
            this.Add(this._borrowers);
            this.label2.Width = Dim.Auto();
            this.label2.Height = 1;
            this.label2.X = 1;
            this.label2.Y = 13;
            this.label2.Visible = true;
            this.label2.Arrangement = Terminal.Gui.ViewArrangement.Fixed;
            this.label2.Data = "label2";
            this.label2.Text = "Book";
            this.label2.TextAlignment = Terminal.Gui.Alignment.Start;
            this.Add(this.label2);
            this._books.Width = Dim.Fill(1);
            this._books.Height = 10;
            this._books.X = 1;
            this._books.Y = 14;
            this._books.Visible = true;
            this._books.Arrangement = Terminal.Gui.ViewArrangement.Fixed;
            this._books.Data = "_books";
            this._books.TextAlignment = Terminal.Gui.Alignment.Start;
            this._books.Source = new Terminal.Gui.ListWrapper<string>(new System.Collections.ObjectModel.ObservableCollection<string>(new string[] {
                            "Item1",
                            "Item2",
                            "Item3"}));
            this._books.AllowsMarking = false;
            this._books.AllowsMultipleSelection = true;
            this.Add(this._books);
        }
    }
}
