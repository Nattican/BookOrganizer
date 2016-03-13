using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;

using BookOrganizer.Commands;
using System.Linq;

namespace BookOrganizer.ViewModels
{
    public class AddBookViewModel : ViewModelBase
    {
        #region Book object with public methods
        private Book book;
        private List<Genre> genres;

        public List<Genre> Genres
        {
            get { return genres; }
            set { genres = value; OnPropertyChanged("Genres"); }
        }
        public string Author
        {
            get { return book.Author; }
            set
            {
                book.Author = value;
                OnPropertyChanged("Author");
            }
        }
        public string Title
        {
            get { return book.Title; }
            set
            {
                book.Title = value;
                OnPropertyChanged("Title");
            }
        }
        public int Year
        {
            get { return book.Year; }
            set
            {
                book.Year = value;
                OnPropertyChanged("Year");
            }
        }
        public string Annotation
        {
            get { return book.Annotation; }
            set
            {
                book.Annotation = value;
                OnPropertyChanged("Annotation");
            }
        }
        public string Comment
        {
            get { return book.Comment; }
            set
            {
                book.Comment = value;
                OnPropertyChanged("Comment");
            }
        }
        public DateTime? FinishTime
        {
            get { return book.FinishTime; }
            set
            {
                book.FinishTime = value;
                OnPropertyChanged("FinishTime");
            }
        }
        public DateTime? StartTime
        {
            get { return book.StartTime; }
            set
            {
                book.StartTime = value;
                OnPropertyChanged("StartTime");
            }
        }
        public Genre Genre
        {
            get { return book.Genre; }
            set
            {
                book.Genre = value;
                OnPropertyChanged("Genre");
            }
        }
        public int Id
        {
            get { return book.Id; }
            set
            {
                book.Id = value;
                OnPropertyChanged("Id");
            }
        }
        public int Mark
        {
            get { return book.Mark; }
            set
            {
                book.Mark = value;
                OnPropertyChanged("Mark");
            }
        }
        public int Pages
        {
            get { return book.Pages; }
            set
            {
                book.Pages = value;
                OnPropertyChanged("Pages");
            }
        }
        #endregion
        #region visual properties
        private int selectedMode = 0;
        public int SelectedMode
        {
            get { return selectedMode; }
            set
            {
                selectedMode = value;
                switch (value)
                {
                    case 0:
                        AfterRead = true; break;
                    case 1:
                        AfterRead = false; break;
                    case 2:
                        AfterRead = false; break;
                }
                OnPropertyChanged("SelectedMode");
            }
        }
        public int[] AvailableMarks
        {
            get
            { return new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 }; }
        }
        private bool afterRead = true;
        public bool AfterRead
        {
            get { return afterRead; }
            set
            {
                afterRead = value; OnPropertyChanged("AfterRead");
            }
        }
        #endregion
        public Action<Book> BookOut;
        #region Constructor

        public AddBookViewModel(Book book)
        {
            this.book = book;
            //hello, logic of changing existing item
        }
        public AddBookViewModel(string author = "", string title = "", string year = "0", int pages = 0)
        {
            using (var c = new Context())
            {
                Genres = c.Genres.ToList();
            }
            book = new Book() { Title = title, Author = author, Year = int.Parse(year), Pages = pages };
        }

        #endregion

        #region SubmitCommand
        private DelegateCommand submitCommand;
        public ICommand SubmitCommand
        {
            get
            {
                if (submitCommand == null)
                {
                    submitCommand = new DelegateCommand(Submit);
                }
                return submitCommand;
            }
        }

        private void Submit()
        {
            if (BookOut != null) { BookOut(book); }
        }

        #endregion
    }
}
