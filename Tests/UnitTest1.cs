using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BookOrganizer.Views;
using BookOrganizer.ViewModels;

namespace Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var book = new BookOrganizer.Book() { Title = "что-то", Author = new BookOrganizer.Author() { Name = "Пушкин" } };
            var b = new AddBookViewModel(book);

            PrivateObject obj = new PrivateObject(b);
            Assert.AreEqual(book, obj.GetFieldOrProperty("book") as BookOrganizer.Book);

        }
    }
}
