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
        private List<Author> authors;

        public List<Author> Authors
        {
            get { return authors; }
            set { authors = value; OnPropertyChanged("Authors"); }
        }
        public List<Genre> Genres
        {
            get { return genres; }
            set { genres = value; OnPropertyChanged("Genres"); }
        }

        public Author Author
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

        public int[] AvailableMarks
        {
            get
            { return new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 }; }
        }
        #endregion
        public Action<Book> BookOut;
        #region Constructor

        public AddBookViewModel(Book b)
        {
            book = b;
            using (var c = new Context())
            {
                Genres = c.Genres.ToList();
                Authors = c.Authors.ToList();

                if (b.Genre != null)
                {
                    var g = Genres.FirstOrDefault(p => p.Name == book.Genre.Name);
                    if (g != null)
                    {
                        book.Genre = Genres.FirstOrDefault(p => p.Name == book.Genre.Name);
                    }
                    else { c.Genres.Add(book.Genre); c.SaveChanges(); book.Genre = Genres.FirstOrDefault(p => p.Name == book.Genre.Name); Genres = c.Genres.ToList(); }

                }

                if (b.Author != null)
                {
                    var a = Authors.FirstOrDefault(p => p.Name == book.Author.Name);
                    if (a != null)
                    {
                        book.Author = Authors.FirstOrDefault(p => p.Name == book.Author.Name);
                    }
                    else { c.Authors.Add(book.Author); c.SaveChanges(); book.Author = Authors.FirstOrDefault(p => p.Name == book.Author.Name); Authors = c.Authors.ToList(); }

                }
            }
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
        #region NewAuthorCommand
        private DelegateCommand newAuthorCommand;
        public ICommand NewAuthorCommand
        {
            get
            {
                if (newAuthorCommand == null)
                {
                    newAuthorCommand = new DelegateCommand(NewAuthor);
                }
                return newAuthorCommand;
            }
        }

        private void NewAuthor()
        {
            MessageBox.Show("");
        }
        #endregion
    }
}
