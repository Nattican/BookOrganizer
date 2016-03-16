using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;

using BookOrganizer.Commands;
using System.Collections.ObjectModel;
using BookOrganizer.Views;

namespace BookOrganizer.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private ObservableCollection<Book> currentList;
        public ObservableCollection<Book> CurrentList
        {
            get { return currentList; }
            set
            {
                currentList = value;
                OnPropertyChanged("CurrentList");
            }
        }
        private int selectedMode = 0;
        public int SelectedMode
        {
            get { return selectedMode; }
            set
            {
                selectedMode = value;
                OnPropertyChanged("SelectedMode");
                OpenList(value);
            }
        }

        #region Constructor

        public MainViewModel()
        {
            OpenList(0);
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
        private DelegateCommand<int> openListCommand;
        public ICommand OpenListCommand
        {
            get
            {
                if (openListCommand == null)
                {
                    openListCommand = new DelegateCommand<int>(OpenList);
                }
                return openListCommand;
            }
        }

        private void OpenList(int param = 0)
        {
            using (var c = new Context())
            {
                List<Book> temp;
                switch (param)
                {
                    case 2:
                        temp = c.Books.Where(z => ((z.StartTime == null) || (((DateTime)(z.StartTime)).CompareTo(DateTime.Now) == 1))).ToList();
                        break;
                    case 1:
                        temp = c.Books.Where(z => (z.StartTime != null) && (((DateTime)(z.StartTime)).CompareTo(DateTime.Now) != 1) && z.FinishTime == null).ToList();
                        break;
                    case 0:
                        temp = c.Books.Where(z => z.FinishTime != null).ToList();
                        break;
                    default: temp = new List<Book>(); break;
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
            v.DataContext = new AddBookViewModel();
            ((AddBookViewModel)v.DataContext).BookOut += () =>
             {
                 OpenList(selectedMode);
                 v.Close();
             };
            v.Show();
        }



        #endregion
        #region DeleteBookCommand
        private DelegateCommand<int> deleteBookCommand;
        public ICommand DeleteBookCommand
        {
            get
            {
                if (deleteBookCommand == null)
                {
                    deleteBookCommand = new DelegateCommand<int>(DeleteBook);
                }
                return deleteBookCommand;
            }
        }

        private void DeleteBook(int id)
        {
            //try

            {
                using (var c = new Context())
                {
                    c.Books.Remove(c.Books.First(p => p.Id == id));
                    c.SaveChanges();
                }

                OpenList(selectedMode);

            }
            //catch { }
        }
        #endregion
        #region EditBookCommand
        private DelegateCommand<int> editBookCommand;
        public ICommand EditBookCommand
        {
            get
            {
                if (editBookCommand == null)
                {
                    editBookCommand = new DelegateCommand<int>(EditBook);
                }
                return editBookCommand;
            }
        }

        private void EditBook(int id)
        {
            // try
            {
                using (var c = new Context())
                {
                    var bookForEdit = c.Books.First(p => p.Id == id);
                    var v = new AddBookView();
                    v.DataContext = new AddBookViewModel(bookForEdit);
                    c.Books.Remove(c.Books.First(p => p.Id == id));
                    c.SaveChanges();

                    ((AddBookViewModel)v.DataContext).BookOut += () =>
                    {
                        OpenList(selectedMode);
                        v.Close();
                    };
                    v.Show();
                }
            }
            // catch { }
        }



        #endregion

    }
    public class UndoRedoAction
    {
        
    }
}
