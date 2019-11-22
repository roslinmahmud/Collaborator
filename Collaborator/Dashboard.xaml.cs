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

namespace Collaborator
{
    /// <summary>
    /// Interaction logic for Dashboard.xaml
    /// </summary>
    public partial class Dashboard : Window
    {
        private MainWindow mainWindow = null;
        public Dashboard()
        {
            InitializeComponent();
            User user = new User();
            List<User> users = user.List;
            ContactList.ItemsSource = users;
        }
        public MainWindow MainWindowInstance
        {
            set
            {
                mainWindow = value;
            }
        }
      
        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.UserName = string.Empty;
            Properties.Settings.Default.Password = string.Empty;
            Properties.Settings.Default.Save();

            mainWindow.Show();
            this.Close();
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
