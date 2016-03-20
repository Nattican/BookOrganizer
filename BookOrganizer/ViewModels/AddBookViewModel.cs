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
        private List<string> genres;
        private List<string> authors;

        public List<string> Authors
        {
            get { return authors; }
            set { authors = value; OnPropertyChanged("Authors"); }
        }
        public List<string> Genres
        {
            get { return genres; }
            set { genres = value; OnPropertyChanged("Genres"); }
        }

        private string stringAuthor;
        private string stringGenre;

        public string Author
        {
            get { return stringAuthor; }
            set
            {
                stringAuthor = value;
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
        public string Genre
        {
            get { return stringGenre; }
            set
            {
                stringGenre = value;
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
            stringAuthor = book.Author == null ? null : book.Author.Name;
            stringGenre = book.Genre == null ? null : book.Genre.Name;
            using (var c = new Context())
            {
                Genres = new List<string>(c.Genres.ToList().Select(p => p.Name));
                Authors = new List<string>(c.Authors.ToList().Select(p => p.Name));
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
            if (book.Title == null || book.Title.Trim() == "" || Author == null || Author.Trim() == "" || Genre == null || Genre.Trim() == "") { MessageBox.Show("Поля \"Название\",\"Автор\",\"Жанр\" обязательны для заполнения!\n\n  \"Мы вновь спасли этот грешный мир\" - Ваша команда разработчиков"); return; }
            using (var c = new Context())
            {
                var a = c.Authors.FirstOrDefault(p => p.Name == Author);
                if (a == null)
                {
                    c.Authors.Add(new Author() { Name = Author });
                    c.SaveChanges();
                    book.Author = c.Authors.First(p => p.Name == Author);
                }
                else { book.Author = a; }
                var g = c.Genres.FirstOrDefault(p => p.Name == Genre);
                if (g == null)
                {
                    c.Genres.Add(new Genre() { Name = Genre });
                    c.SaveChanges();
                    book.Genre = c.Genres.First(p => p.Name == Genre);
                }
                else { book.Genre = g; }
            }
            if (BookOut != null) { BookOut(book); }
        }
        #endregion
    }
}
