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
            MessageBox.Show(query);
            try
            {
                using (MySqlCommand mySqlCommand = new MySqlCommand(query, DBConnection.Instance.Connection))
                {
                    return mySqlCommand.ExecuteReader().HasRows;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, this.ToString() + " Perform() Exception", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }
    }
}
