using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;

namespace BookOrganizer
{
    public class Context : DbContext
    {
        public DbSet<Book> Books { get; set; }
        public DbSet<Genre> Genres { get; set; }

        public Context() : base("BookOrganiser") { }
    }

    public class Book
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Author { get; set; }
        public int Year { get; set; }
        public Genre Genre { get; set; }
        public int Pages { get; set; }
        public string Annotation { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? FinishTime { get; set; }
        public string Comment { get; set; }
        public int Mark { get; set; }
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