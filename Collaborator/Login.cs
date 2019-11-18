using MySql.Data.MySqlClient;
using System;
using System.Windows;

namespace Collaborator
{
    class Login
    {
        private string username;
        private string password;
        public Login()
        {

        }
        public Login(string username, string password)
        {
            this.username = username;
            this.password = password;
        }

        public bool Perform()
        {
            DBConnection.Instance.Connect();
            string query = "SELECT * FROM USER WHERE USERNAME='" + username + "' AND PASSWORD='" + password + "';";
            try
            {
                using (MySqlCommand mySqlCommand = new MySqlCommand(query, DBConnection.Instance.Connection))
                {
                    if(mySqlCommand.ExecuteReader().HasRows)
                    {
                        Properties.Settings.Default.UserName = username;
                        Properties.Settings.Default.Password = password;
                        Properties.Settings.Default.Save();
                        return true;
                    }
                    return false;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, this.ToString() + " Perform() Exception", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }
        public bool Saved()
        {
            return Properties.Settings.Default.UserName != string.Empty && Properties.Settings.Default.Password != string.Empty;
        }
    }
}
