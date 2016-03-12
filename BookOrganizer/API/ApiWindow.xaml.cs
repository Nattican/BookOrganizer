﻿using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using Newtonsoft.Json;
using BookOrganizer.Views;
using BookOrganizer.ViewModels;

namespace BookOrganizer.API
{
    /// <summary>
    /// Логика взаимодействия для ApiWindow.xaml
    /// </summary>
    public partial class ApiWindow : Window
    {
        public event Action<Book> Result1;
        List<Book> bb;

        public ApiWindow()
        {
            InitializeComponent();
            Result1 += OnAdd;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try// на случай пустой строки
            {
                if (dataGrid2.SelectedItem != null)
                {
                    var selectedBook = (Book)dataGrid2.SelectedItem;
                    var v = new AddBookView();
                    v.DataContext = new AddBookViewModel(selectedBook.Author, selectedBook.Name, selectedBook.Year,int.Parse(selectedBook.Pages));
                    v.Show();
                }
            }
            catch { }
        }

        private void searchButton_Click(object sender, RoutedEventArgs e)
        {
            var s = searchBox.Text.Trim();
            if (s == "") return;
            searchBox.Text = "";

            listBox1.Items.Clear();
            try
            {
                bb = new List<Book>();
                var webClient = new WebClient();
                string result = webClient.DownloadString(MakeQuery1(s));
                API_Data data = JsonConvert.DeserializeObject<API_Data>(result);
                foreach (Books b in data.Books)
                {
                    if (b.Book.Author == null) b.Book.Author = "Uknown author";
                    if (b.Book.Year == "0") b.Book.Year = "Uknown";
                    if (b.Book.Name.Length > 50) b.Book.Name = b.Book.Name.Substring(0, 50) + "...";
                    Result1(b.Book);
                }
                dataGrid2.ItemsSource = bb;
            }
            catch (Exception ex) { MessageBox.Show("An error occured: " + ex.Message); }
        }

        static string MakeQuery1(string title)
        {
            return string.Format("http://api.knigafund.ru/api/books/search.json?query={0}", title);
        }

        static string MakeQuery2(string subject)
        {
            return string.Format("http://api.knigafund.ru/api/products/{0}/books.json?per_page=100", subject);
        }

        private async void comboBox1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string s = "";
            switch (comboBox1.SelectedIndex)
            {
                case 0: s = "2"; break;
                case 1: s = "160"; break;
                case 2: s = "161"; break;
                case 3: s = "165"; break;
                case 4: s = "170"; break;
                case 5: s = "177"; break;
                case 6: s = "162"; break;
                case 7: s = "178"; break;
                case 8: s = "167"; break;
                case 9: s = "173"; break;
                case 10: s = "195"; break;
                case 11: s = "171"; break;
                case 12: s = "172"; break;
                case 13: s = "176"; break;
                case 14: s = "190"; break;
                case 15: s = "185"; break;
                case 16: s = "174"; break;
                case 17: s = "194"; break;
                case 18: s = "175"; break;
                case 19: s = "192"; break;
                case 20: s = "163"; break;
                case 21: s = "180"; break;
                case 22: s = "164"; break;
                default: s = "2"; break;
            }
            await AddItems(s);
            if (listBox1.Items.Count != 0) listBox1.Items.Clear();
            dataGrid2.ItemsSource = bb;
        }

        private async Task AddItems(string s)
        {
            bb = new List<Book>();
            var task = Task.Factory.StartNew(() =>
            {
                try
                {
                    var webClient = new WebClient();
                    string result = webClient.DownloadString(MakeQuery2(s));
                    API_Data data = JsonConvert.DeserializeObject<API_Data>(result);
                    foreach (Books b in data.Books)
                    {
                        if (b.Book.Author == null) b.Book.Author = "Uknown author";
                        if (b.Book.Year == "0") b.Book.Year = "Uknown";
                        b.Book.Name = b.Book.Name.Trim();
                        if (b.Book.Name.Length > 50) b.Book.Name = b.Book.Name.Substring(0, 50) + "...";
                        if (b.Book.Author.Length > 30) b.Book.Author = b.Book.Author.Substring(0, 30) + "...";
                        if (b.Book.Annotation.Length > 100) b.Book.Annotation = b.Book.Annotation.Substring(0, 100) + "...";
                        Result1(b.Book);
                    }
                }
                catch (Exception ex) { MessageBox.Show("An error occured: " + ex.Message); }
            });
            await task;
        }

        public void OnAdd(Book obj)
        {
            bb.Add(obj);
            dataGrid2.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(() => dataGrid2.Items.Refresh()));
        }
    }
}