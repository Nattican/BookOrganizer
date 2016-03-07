using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BookOrganizer.Views
{
    /// <summary>
    /// Interaction logic for MainView.xaml
    /// </summary>
    public partial class MainView : Window
    {
        public MainView()
        {
            InitializeComponent();
            using (var c = new Context()) 
            {
                //c.Authors.Add(new Author() { FirstName = "Александр", SecondName = "Пушкин", Born = 1111, Dead = 1148 });
                //c.SaveChanges();
            }
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            ApiWindow api = new ApiWindow();
            api.Show();
        }

    }
}
