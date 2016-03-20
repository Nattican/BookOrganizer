using System;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;

namespace BookOrganizer
{
    public class Context : DbContext
    {
        public DbSet<Book> Books { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Author> Authors { get; set; }

        //public Context() : base("BookOrganizer_SHOB") { }
        //public Context() : base("Data Source=SHMIKEL-NB\\SQLExpress;Initial Catalog=BookOrganiser_H;Integrated Security=True;") { }

    }

    public class Book : ICloneable
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        public Author Author { get; set; }
        public int Year { get; set; }
        public Genre Genre { get; set; }
        public int Pages { get; set; }
        public string Annotation { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? FinishTime { get; set; }
        public string Comment { get; set; }
        public int Mark { get; set; }

        public object Clone()
        {
            var other = new Book();
            other.Id = Id;
            other.Title = Title;
            other.Author = Author;
            other.Year = Year;
            other.Genre = Genre;
            other.Pages = Pages;
            other.Annotation = Annotation;
            other.StartTime = StartTime;
            other.FinishTime = FinishTime;
            other.Comment = Comment;
            other.Mark = Mark;
            return other;
        }
        public void PullChanges(Book other)
        {
            if (other.Title != Title) Title = other.Title;
            if (other.Author != Author) Author = other.Author;
            if (other.Year != Year) Year = other.Year;
            if (other.Genre != Genre) Genre = other.Genre;
            if (other.Pages != Pages) Pages = other.Pages;
            if (other.Annotation != Annotation) Annotation = other.Annotation;
            if (other.StartTime != StartTime) StartTime = other.StartTime;
            if (other.FinishTime != FinishTime) FinishTime = other.FinishTime;
            if (other.Comment != Comment) Comment = other.Comment;
            if (other.Mark != Mark) Mark = other.Mark;
        }
        public override bool Equals(object obj)
        {
            try
            {
                var o = (Book)obj;
                bool res =
                    Title == o.Title &&
                    (Author == null && o.Author == null ? true : Author.Equals(o.Author)) &&
                    Year == o.Year &&
                    (Genre == null && o.Genre == null ? true : Genre.Equals(o.Genre)) &&
                    Pages == o.Pages &&
                    Annotation == o.Annotation &&
                    StartTime == o.StartTime &&
                    FinishTime == o.FinishTime &&
                    Comment == o.Comment &&
                    Mark == o.Mark;
                return res;
            }
            catch { return base.Equals(obj); }
        }
    }
    public class Genre : Object
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public override bool Equals(object obj)
        {
            if (this == null && obj == null) { return true; }
            else { return this.GetHashCode() == ((Genre)obj).GetHashCode(); }
        }
        public override int GetHashCode()
        {
            return Id + Name.GetHashCode();
        }
    }

    public class Author : Object
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public override bool Equals(object obj)
        {
            if (this == null && obj == null) { return true; }
            else { return this.GetHashCode() == ((Author)obj).GetHashCode(); }
        }
        public override int GetHashCode()
        {
            return Id + Name.GetHashCode();
        }
    }
    public static class DB
    {
        public static string WhereIsMyDB()
        {
            using (var context = new Context())
            {
                return context.Database.Connection.ConnectionString;
            }
        }
    }
}