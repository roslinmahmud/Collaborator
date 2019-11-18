using MySql.Data.MySqlClient;
using System;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Windows;

namespace Collaborator
{
    class Register
    {
        private string username;
        private string fullname;
        private string password;
        private string photo_path;
        private string ip;

        public Register()
        {

        }
        public Register(string username, string fullname, string password)
        {
            this.username = username;
            this.fullname = fullname;
            this.password = password;
            this.photo_path = "Avatar/user.png";
            this.ip = Connection.HostIP;
        }  

        public bool Perform()
        {
            DBConnection.Instance.Connect();
            string query = "INSERT INTO USER (USERNAME, NAME, PASSWORD, PHOTO_PATH, IP) VALUES ('"+username+"','"+fullname+"','"+password+"','"+photo_path+"','"+ip+"');";
            try
            {
                using (MySqlCommand mySqlCommand = new MySqlCommand(query, DBConnection.Instance.Connection))
                {
                    if (mySqlCommand.ExecuteNonQuery() == 1)
                    {
                        Properties.Settings.Default.UserName = username;
                        Properties.Settings.Default.Password = password;
                        Properties.Settings.Default.Save();
                        return true;
                    }
                    return false;
                }
            }
            catch(Exception e)
            {
                MessageBox.Show(e.Message, this.ToString() + " Perform() Exception", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }
    }
}
