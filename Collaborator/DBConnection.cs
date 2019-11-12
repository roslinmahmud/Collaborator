using System;
using MySql.Data.MySqlClient;
using System.Windows;
using System.Net;
using System.Threading;
using System.Net.NetworkInformation;

namespace Collaborator
{
    //Singleton design pattern used
    class DBConnection
    {
        private MainWindow mainWindow = null;
        private static DBConnection instance = null;
        private MySqlConnection connection = null;
        private Ping ping = null;
        private string connectionString = "Server=remotemysql.com;Port=3306;Database=5oj1fjtwef;Uid=5oj1fjtwef;Pwd=MJ8T2gcsFB;";

        private DBConnection()
        {
            ping = new Ping();
        }
        public MainWindow MainWindow
        {
            set
            {
                mainWindow = value;
            }
        }
        public static DBConnection Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new DBConnection();
                    return instance;
                }
                else
                {
                    return instance;
                }
            }
        }

        public MySqlConnection Connection
        {
            get
            {
                if (connection == null)
                {
                    try
                    {
                        connection = new MySqlConnection(connectionString);
                        connection.Open();
                    }
                    catch (Exception e)
                    {
                        connection = null;
                        MessageBox.Show(e.Message, this.ToString() + " Connection Exception", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                return connection;
            }
            set
            {
                connection = value;
            }
        }

        public void Connect()
        {
            if (connection == null)
            {
                try
                {
                    connection = new MySqlConnection(connectionString);
                    connection.Open();
                }
                catch (Exception e)
                {
                    connection = null;
                    MessageBox.Show(e.Message, this.ToString() + " Connect() Exception", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            return;
        }

        public bool IsConnect()
        {
            return !(connection == null);
        }
        
        public void CheckInternetConnection()
        {
            while (true)
            {
                try
                {
                    if(ping.Send("www.google.com.mx").Status == IPStatus.Success)
                    {
                        if (mainWindow.ErrorMessage.IsVisible)
                        {
                            mainWindow.Dispatcher.Invoke(() =>
                            {
                                mainWindow.ErrorMessage.Visibility = Visibility.Collapsed;
                            });

                        }
                        if(!IsConnect())
                        {
                            Connect();
                        }
                    }
                    
                }
                catch(Exception e)
                {
                    mainWindow.Dispatcher.Invoke(() =>
                    {
                        mainWindow.ErrorMessage.Visibility = Visibility.Visible;
                    });
                    DBConnection.Instance.Connection = null;
                }
                finally
                {
                    Thread.Sleep(3000);
                    
                }
            }
        }

    }
}
