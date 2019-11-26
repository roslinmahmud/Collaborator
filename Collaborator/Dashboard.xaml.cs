using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
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
        List<User> users;
        BackgroundWorker backgroundWorker = new BackgroundWorker();
        public Dashboard()
        {
            InitializeComponent();

            backgroundWorker.DoWork += BackgroundWorker_DoWork;
            backgroundWorker.RunWorkerCompleted += BackgroundWorker_RunWorkerCompleted;
            backgroundWorker.RunWorkerAsync();
        }

        private void BackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            ContactList.ItemsSource = users;
        }


        private void BackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            User.Instance.Init();
            users = User.Instance.ContactList;
        }
        private void TopPanel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        public string FullName { get; set; }
        public MainWindow MainWindowInstance
        {
            set
            {
                mainWindow = value;
            }
        }
      
        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            User.Instance.UnSave();
            mainWindow.Show();
            this.Close();
        }

        private void ContactList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            User user =(User) ContactList.SelectedItem;
            FullName = user.Name;
            HeaderTextBlock.Text = FullName;
        }

    }
}
