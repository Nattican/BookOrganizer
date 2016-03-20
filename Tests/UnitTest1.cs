using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BookOrganizer.Views;
using BookOrganizer.ViewModels;
using BookOrganizer;

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
        //Метод проверяет равенство объектов реального и того что собирает AddBookViewModel и показывает когда онидолжны быть различными а когда нет 
        [TestMethod]
        public void TestMethod2()
        {
            var t1 = DateTime.Now;
            var t2 = DateTime.Now.AddDays(-1);
            var emptyBook = new BookOrganizer.Book();
            var model = new AddBookViewModel(emptyBook);
            Book resBookManual = new Book()
            {
                Annotation = "Аннотация",
                Comment = "Комментарий",
                FinishTime = t1,
                StartTime = t2,
                Title = "Название",
                Pages = 100,
                Mark = 10,
                Year = 2016
            };

            model.Annotation = "Аннотация";
            model.Comment = "Комментарий";
            model.FinishTime = t1;
            model.StartTime = t2;
            model.Title = "Название";
            model.Pages = 100;
            model.Mark = 10;
            model.Year = 2016;

            var privateModel = new PrivateObject(model);

            var t = (privateModel.GetFieldOrProperty("book")) as Book;
            Assert.AreEqual(resBookManual, t);


            model.Author = "Пушкин А.С.";
            model.Genre = "Классика";
            resBookManual.Author = new Author() { Name = "Пушкин А.С." };
            resBookManual.Genre = new Genre() { Name = "Классика" };

            model.BookOut += (a) => { Assert.AreNotEqual(a, resBookManual); Assert.AreNotEqual(((Book)a).Author.Id, resBookManual.Author.Id); Assert.AreNotEqual(((Book)a).Genre.Id, resBookManual.Genre.Id); };
            model.SubmitCommand.Execute(null);

            var bookAutoCreatedButWithManualAuthorAndGenre = (privateModel.GetFieldOrProperty("book")) as Book;
            bookAutoCreatedButWithManualAuthorAndGenre.Author = new Author() { Name = "Пушкин А.С.", Id = 0 };
            bookAutoCreatedButWithManualAuthorAndGenre.Genre = new Genre() { Name = "Классика", Id = 0 };

            privateModel.SetFieldOrProperty("book", bookAutoCreatedButWithManualAuthorAndGenre);

            t = privateModel.GetFieldOrProperty("book") as Book;
            Assert.AreEqual(resBookManual, t);

        }

    }
}
