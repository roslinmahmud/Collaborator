using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        internal List<User> users;
        private BackgroundWorker backgroundWorker = new BackgroundWorker();
        Server server = null;
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
            server = new Server(ChatMessageScroll);
            server.StartServer();
        }

        private void BackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            User.Instance.Init();
            users = User.Instance.ContactList;
            if (!User.Instance.CheckIP())
            {
                User.Instance.UpdateIP();
            }
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

        private void ContactList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            User user = ContactList.SelectedItem as User;
            HeaderTextBlock.Text = user.Name;
            ChatMessage.ItemsSource = user.messages;
        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            User.Instance.UnSave();
            mainWindow.Show();
            server.StopServer();
            this.Close();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }

        private void CloseIcon_MouseEnter(object sender, MouseEventArgs e)
        {
            CloseIcon.Foreground = Brushes.Red;
        }

        private void CloseIcon_MouseLeave(object sender, MouseEventArgs e)
        {
            CloseIcon.Foreground = Brushes.White;
        }

        private void MessageTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox MessageTextBox = sender as TextBox;
            if(MessageTextBox.Text.Length > 0 && !SendButton.IsEnabled)
            {
                SendButton.IsEnabled = true;
            }
            else if (MessageTextBox.Text.Length == 0 && SendButton.IsEnabled)
            {
                SendButton.IsEnabled = false;
            }
        }
    }
}
