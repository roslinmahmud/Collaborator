using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Threading;
using System.Windows;

namespace Collaborator
{
    class Connection
    {
        private static Connection connection = null;
        private MainWindow mainWindow = null;
        private Ping ping = null;
        private Connection()
        {
            ping = new Ping();
        }
        public static Connection Instance
        {
            get
            {
                if(connection == null)
                {
                    connection = new Connection();
                }
                return connection;
            }
        }
        public MainWindow MainWindow
        {
            set
            {
                mainWindow = value;
            }
        }
        public void CheckInternetConnection()
        {
            while (true)
            {
                try
                {
                    if (ping.Send("www.google.com.mx").Status == IPStatus.Success)
                    {
                        if (mainWindow.ErrorMessage.IsVisible)
                        {
                            Application.Current.Dispatcher.Invoke(() =>
                            {
                                mainWindow.ErrorMessage.Visibility = Visibility.Collapsed;
                            });

                        }
                        if (!DBConnection.Instance.IsConnect())
                        {
                            DBConnection.Instance.Connect();
                        }
                    }

                }
                catch (Exception e)
                {
                    Application.Current.Dispatcher.Invoke(() =>
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
        public static String HostIP
        {
            get
            {
                foreach (NetworkInterface ni in NetworkInterface.GetAllNetworkInterfaces())
                {
                    if (ni.GetIPProperties().GatewayAddresses.FirstOrDefault() != null)
                    {
                        if (ni.NetworkInterfaceType == NetworkInterfaceType.Wireless80211 || ni.NetworkInterfaceType == NetworkInterfaceType.Ethernet)
                        {
                            foreach (UnicastIPAddressInformation ip in ni.GetIPProperties().UnicastAddresses)
                            {
                                if (ip.Address.AddressFamily == AddressFamily.InterNetwork)
                                {
                                    return (ip.Address.ToString());
                                }
                            }
                        }
                    }
                }
                return "127.0.0.1";
            }
        }
    }
}
