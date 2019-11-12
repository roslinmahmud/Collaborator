using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MySql.Data.MySqlClient;

namespace Collaborator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DBConnection.Instance.MainWindow = this;
            Thread DBConnect = new Thread(new ThreadStart(DBConnection.Instance.CheckInternetConnection));
            DBConnect.Start();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if(e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void PackIconMaterial_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Environment.Exit(0);
        }

        private void PackIconMaterial_MouseEnter(object sender, MouseEventArgs e)
        {
            closeIcon.Foreground = Brushes.Red;
        }

        private void closeIcon_MouseLeave(object sender, MouseEventArgs e)
        {
            closeIcon.Foreground = Brushes.White;
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string sql = "SELECT USERNAME, PASSWORD FROM USER";
            try
            {
                DBConnection.Instance.Connect();
                using(MySqlCommand mySqlCommand = new MySqlCommand(sql, DBConnection.Instance.Connection)){
                    MySqlDataReader mySqlDataReader = mySqlCommand.ExecuteReader();
                    MessageBox.Show("connected");
                    while (mySqlDataReader.Read())
                    {
                        MessageBox.Show(mySqlDataReader.GetString(1));
                    }
                }

            }
            catch (Exception ec)
            {
                DBConnection.Instance.Connection = null;
                MessageBox.Show(this.ToString() + " Exception: " + ec.Message);
            }
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void RegisterBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            TopLoginPanel.Visibility = Visibility.Collapsed;
            BottomLoginPanel.Visibility = Visibility.Collapsed;
            TopRegisterPanel.Visibility = Visibility.Visible;
            FullNamePanel.Visibility = Visibility.Visible;
            BottomRegisterPanel.Visibility = Visibility.Visible;
        }

        private void LoginBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            TopRegisterPanel.Visibility = Visibility.Collapsed;
            FullNamePanel.Visibility = Visibility.Collapsed;
            BottomRegisterPanel.Visibility = Visibility.Collapsed;
            TopLoginPanel.Visibility = Visibility.Visible;
            BottomLoginPanel.Visibility = Visibility.Visible;
        }
        public void ShowInternetConnectionError()
        {

        }
    }
}
