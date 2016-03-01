using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;

namespace BookOrganizer.Views
{
    public class Context : DbContext
    {
        public DbSet<Book> Books { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Start> Started { get; set; }
        public DbSet<Finish> Finished { get; set; }
        public DbSet<FutureBook> FutureList { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public Context():base() { }
        // : base("BooksBase") { }
    }

    public class Book
    {
        public string Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public Author Author { get; set; }
        [Required]
        public int Year { get; set; }
        public Genre Genre { get; set; }
    }
    public class Author
    {
        public int Id { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string SecondName { get; set; }
        public int Born { get; set; }
        public int Dead { get; set; }
    }
    public class Start
    {
        public int Id { get; set; }
        [Required]
        public DateTime Time { get; set; }
        [Required]
        public Book Book { get; set; }
    }
    public class Finish
    {
        public int Id { get; set; }
        [Required]
        public DateTime Time { get; set; }
        [Required]
        public Book Book { get; set; }
        public string Comment { get; set; }
        [Required]
        public int Mark { get; set; }
    }
    public class FutureBook
    {
        public int Id { get; set; }
        [Required]
        public Book Book { get; set; }
        [Required]
        public DateTime Time { get; set; }
    }
    public class Genre
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
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