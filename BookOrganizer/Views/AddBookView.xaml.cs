using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace BookOrganizer.Views
{
    /// <summary>
    /// Interaction logic for MainView.xaml
    /// </summary>
    public partial class AddBookView : Window
    {
        public AddBookView()
        {
            InitializeComponent();

        }

        private void OnlyDigitsOnlyFour(object sender, KeyEventArgs e)
        {
            string[] alow = new string[] { "D0", "D1", "D2", "D3", "D4", "D5", "D6", "D7", "D8", "D9" };
            foreach (var item in alow)
            {
                if (item == e.Key.ToString())
                {
                    switch ((((TextBox)sender).Text.Trim()).Count())
                    {
                        case 3:
                            int y = DateTime.Now.Year;
                            int pressed = int.Parse(e.Key.ToString().Substring(1));
                            if (int.Parse(((TextBox)sender).Text.Trim() + pressed.ToString()) > y) { ((TextBox)sender).Text = y.ToString(); } else { return; }
                            break;
                        case 4: e.Handled = true; break;
                        default: return;
                    }
                }
            }
            e.Handled = true;
        }

        private void FinishTimeBox_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (StartTimeBox.SelectedDate == null) { FinishTimeBox.SelectedDate = null; return; }
            var dS = Convert.ToDateTime(StartTimeBox.SelectedDate);
            var dF = Convert.ToDateTime(FinishTimeBox.SelectedDate);
            if (dF.CompareTo(dS) == -1) { FinishTimeBox.SelectedDate = StartTimeBox.SelectedDate; }
        }

        private void PagesBox_KeyDown(object sender, KeyEventArgs e)
        {
            string[] alow = new string[] { "D0", "D1", "D2", "D3", "D4", "D5", "D6", "D7", "D8", "D9" };
            foreach (var item in alow)
            {
                if (item == e.Key.ToString())
                {
                    return;
                }
            }
            e.Handled = true;
        }
    }
}
