using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace quiz_resolver
{
    /// <summary>
    /// Interaction logic for SecondWindow.xaml
    /// </summary>
    public partial class SecondWindow : Window
    {
        public SecondWindow()
        {
            InitializeComponent();
            //SecondWindow secondWindow = new SecondWindow(); this is not working, from stack
            //secondWindow.Owner = this;
            //secondWindow.Show();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Hide();
        }

        private void Button_Answer_A_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Answer_B_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Answer_C_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Answer_D_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Next_Question_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
