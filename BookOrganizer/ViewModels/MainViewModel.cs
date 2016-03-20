using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;

using BookOrganizer.Commands;
using System.Collections.ObjectModel;
using BookOrganizer.Views;
using BookOrganizer.API;

namespace BookOrganizer.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private ObservableCollection<Book> currentList;
        private string selectedMode = "0";
        private Book selectedBook;
        private string[] radioButtons;
        private Stack<object[]> Undos = new Stack<object[]>();
        private Stack<object[]> Redos = new Stack<object[]>();

        public ObservableCollection<Book> CurrentList
        {
            get { return currentList; }
            set
            {
                currentList = value;
                OnPropertyChanged("CurrentList");
            }
        }
        public string SelectedMode
        {
            get { return selectedMode; }
            set
            {
                selectedMode = value;
                var t = new string[] { "Transparent", "Transparent", "Transparent" };
                t[int.Parse(selectedMode)] = "#FFBFBFBF";
                MyRadioButtons = t;
                OnPropertyChanged("SelectedMode");
            }
        }
        public Book SelectedBook
        {
            get { return selectedBook; }
            set
            {
                selectedBook = value;
                TimePeriod = selectedBook != null && SelectedBook.StartTime != null ? (SelectedBook.StartTime.Value.Date.ToShortDateString() + " - " + (SelectedBook.FinishTime != null ? SelectedBook.FinishTime.Value.Date.ToShortDateString() : "  ???")) : "";
                OnPropertyChanged("SelectedBook");
            }
        }
        public string[] MyRadioButtons
        {
            get { return radioButtons; }
            set { radioButtons = value; OnPropertyChanged("MyRadioButtons"); }
        }

        private string timePeriod;
        public string TimePeriod { get { return timePeriod; } set { timePeriod = value; OnPropertyChanged("TimePeriod"); } }


        #region Constructor

        public MainViewModel()
        {
            OpenList(SelectedMode);
        }

        #endregion

        #region ExitCommand
        private DelegateCommand exitCommand;
        public ICommand ExitCommand
        {
            get
            {
                if (exitCommand == null)
                {
                    exitCommand = new DelegateCommand(Exit);
                }
                return exitCommand;
            }
        }

        private void Exit()
        {
            Application.Current.Shutdown();
        }

        #endregion

        #region OpenListCommand
        private DelegateCommand<string> openListCommand;
        public ICommand OpenListCommand
        {
            get
            {
                if (openListCommand == null)
                {
                    openListCommand = new DelegateCommand<string>(OpenList);
                }
                return openListCommand;
            }
        }

        private void OpenList(string param = "0")
        {
            using (var c = new Context())
            {
                List<Book> temp = new List<Book>();
                switch (param)
                {
                    case "2":
                        temp = c.Books.Include("Author").Include("Genre").Where(z => (z.StartTime == null) || (((DateTime)(z.StartTime)).CompareTo(DateTime.Now) == 1)).ToList();
                        SelectedMode = "2";
                        break;
                    case "1":
                        temp = c.Books.Include("Author").Include("Genre").Where(z => (z.StartTime != null) && (((DateTime)(z.StartTime)).CompareTo(DateTime.Now) != 1) && z.FinishTime == null).ToList();
                        SelectedMode = "1";
                        break;
                    case "0":
                        temp = c.Books.Include("Author").Include("Genre").Where(z => (z.StartTime != null) && (((DateTime)(z.StartTime)).CompareTo(DateTime.Now) != 1) && z.FinishTime != null).ToList();
                        SelectedMode = "0";
                        break;
                    default:
                        break;
                }
                CurrentList = new ObservableCollection<Book>(temp);
            }
        }
        #endregion

        #region AddBookCommand
        private DelegateCommand addBookCommand;
        public ICommand AddBookCommand
        {
            get
            {
                if (addBookCommand == null)
                {
                    addBookCommand = new DelegateCommand(AddBook);
                }
                return addBookCommand;
            }
        }

        private void AddBook()
        {
            var v = new AddBookView();
            v.DataContext = new AddBookViewModel(new Book());
            ((AddBookViewModel)v.DataContext).BookOut += (b) =>
            {

                AddBookToDB(b);
                using (var c = new Context())
                {
                    Undos.Push(new object[] { "delete", c.Books.Include("Author").Include("Genre").First(p => p.Author.Name == b.Author.Name && p.Year == b.Year && p.Title == b.Title) });
                }
                OpenList(selectedMode);
                v.Close();
            };
            v.ShowDialog();
        }
        #endregion

        #region DeleteBookCommand
        private DelegateCommand deleteBookCommand;
        public ICommand DeleteBookCommand
        {
            get
            {
                if (deleteBookCommand == null)
                {
                    deleteBookCommand = new DelegateCommand(DeleteBook);
                }
                return deleteBookCommand;
            }
        }

        private void DeleteBook()
        {
            if (selectedBook != null)
            {
                var id = SelectedBook.Id;
                using (var c = new Context())
                {
                    Undos.Push(new object[] { "add", c.Books.Include("Author").Include("Genre").First(p => p.Id == id) });
                }
                RemoveBookFromDB(id);
                OpenList(selectedMode);
            }
        }

        #endregion

        #region OpenLibraryCommand
        private DelegateCommand openLibraryCommand;
        public ICommand OpenLibraryCommand
        {
            get
            {
                if (openLibraryCommand == null)
                {
                    openLibraryCommand = new DelegateCommand(OpenLibrary);
                }
                return openLibraryCommand;
            }
        }

        private void OpenLibrary()
        {
            ApiWindow api = new ApiWindow();
            api.ShowDialog();
        }

        #endregion

        #region EditBookCommand
        private DelegateCommand editBookCommand;
        public ICommand EditBookCommand
        {
            get
            {
                if (editBookCommand == null)
                {
                    editBookCommand = new DelegateCommand(EditBook);
                }
                return editBookCommand;
            }
        }

        private void EditBook()
        {
            if (selectedBook != null)
            {
                var id = selectedBook.Id;
                using (var c = new Context())
                {
                    var bookForEdit = c.Books.Include("Author").Include("Genre").First(p => p.Id == id);
                    object old = bookForEdit.Clone();
                    var v = new AddBookView();
                    v.DataContext = new AddBookViewModel(bookForEdit);

                    ((AddBookViewModel)v.DataContext).BookOut += (b) =>
                    {
                        using (var t = new Context())
                        {
                            var temp = t.Books.Include("Author").Include("Genre").First(p => p.Id == id);
                            temp.PullChanges(b);
                            temp.Author = t.Authors.FirstOrDefault(p => p.Id == temp.Author.Id);
                            temp.Genre = t.Genres.FirstOrDefault(p => p.Id == temp.Genre.Id);
                            t.SaveChanges();
                        }
                        Undos.Push(new object[] { "unedit", (Book)old });
                        OpenList(selectedMode);
                        v.Close();
                    };
                    v.ShowDialog();
                }
            }
        }



        #endregion

        #region UndoCommand
        private DelegateCommand undoCommand;
        public ICommand UndoCommand
        {
            get
            {
                if (undoCommand == null)
                {
                    undoCommand = new DelegateCommand(Undo);
                }
                return undoCommand;
            }
        }

        private void Undo()
        {
            if (Undos.Count == 0) return;
            var paramOBJ = Undos.Pop();
            Do((string)(paramOBJ[0]), (Book)(paramOBJ[1]));
        }
        #endregion

        #region RedoCommand
        private DelegateCommand redoCommand;
        public ICommand RedoCommand
        {
            get
            {
                if (redoCommand == null)
                {
                    redoCommand = new DelegateCommand(Redo);
                }
                return redoCommand;
            }
        }


        private void Redo()
        {
            if (Redos.Count == 0) return;
            var paramOBJ = Redos.Pop();
            Do((string)(paramOBJ[0]), (Book)(paramOBJ[1]), true);
        }
        #endregion

        private void AddBookToDB(Book b)
        {
            using (var c = new Context())
            {
                if (b.Author != null)
                {
                    var k = c.Authors.FirstOrDefault(p => p.Name == b.Author.Name);
                    if (k != null) { b.Author = k; }
                }
                if (b.Genre != null)
                {
                    var t = c.Genres.FirstOrDefault(p => p.Name == b.Genre.Name);
                    if (t != null) { b.Genre = t; }
                }
                c.Books.Add(b);
                c.SaveChanges();
            }
        }
        private void RemoveBookFromDB(int id)
        {
            using (var c = new Context())
            {
                Book temp = c.Books.First(p => p.Id == id);
                c.Books.Remove(temp);
                c.SaveChanges();
            }
        }
        public void Do(string param, Book b, bool reverse = false)
        {
            try
            {
                switch (param)
                {
                    case "delete":
                        using (var c = new Context())
                        {
                            var temp = c.Books.Include("Author").Include("Genre").First(p => p.Title == b.Title && p.Author.Name == b.Author.Name && p.Year == b.Year);
                            RemoveBookFromDB(temp.Id);
                            if (reverse)
                            {
                                Undos.Push(new object[] { "add", temp });
                            }
                            else
                                Redos.Push(new object[] { "add", temp });
                        }
                        break;
                    case "add":
                        using (var c = new Context())
                        {
                            AddBookToDB(b);
                            if (reverse)
                            {
                                Undos.Push(new object[] { "delete", b });
                            }
                            else
                            {
                                try
                                {
                                    if ((string)(Undos.Peek()[0]) == "unedit") { Undos.Pop(); }
                                }
                                catch { }
                                Redos.Push(new object[] { "delete", b });
                            }
                        }
                        break;
                    case "unedit":
                        using (var c = new Context())
                        {
                            var temp = c.Books.Include("Author").Include("Genre").First(p => p.Id == b.Id);
                            if (reverse)
                            {
                                Undos.Push(new object[] { "edit", temp.Clone() });
                            }
                            else
                            {
                                Redos.Push(new object[] { "edit", temp.Clone() });
                            }

                            temp.PullChanges(b);
                            c.SaveChanges();
                        }
                        break;
                    case "edit":
                        using (var c = new Context())
                        {

                            var temp = c.Books.Include("Author").Include("Genre").First(p => p.Id == b.Id);
                            if (reverse)
                            {
                                Undos.Push(new object[] { "unedit", temp.Clone() });
                            }
                            else
                                Redos.Push(new object[] { "unedit", temp.Clone() });

                            temp.PullChanges(b);
                            c.SaveChanges();
                        }
                        break;
                    default:
                        break;
                }
            }
            catch { MessageBox.Show("Последнняя вызванная вами команда могла привести к ошибке во временном пространстве.\n\n  \"Мы вновь спасли этот грешный мир\" - Ваша команда разработчиков"); }
            OpenList(SelectedMode);
        }
    }
}
