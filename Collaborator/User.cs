using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Collaborator
{
    class User
    {
        List<User> list;
        public int Id { set; get; }
        public string Username { set; get; }
        public string Name { set; get; }
        public string Photo_Path { set; get; }
        public string Ip { set; get; }
        public User()
        {
            list = new List<User>();
        }
        public User(int id, string username, string name, string photo_path, string ip)
        {
            Id = id;
            Username = username;
            Name = name;
            Photo_Path = photo_path;
            Ip = ip;
        }
        public List<User> List
        {
            get
            {
                if(!list.Any())
                {
                    string query = "SELECT id, username, name, photo_path, ip FROM USER WHERE USERNAME!='" + Properties.Settings.Default.UserName + "';";
                    try
                    {
                        using (MySqlCommand mySqlCommand = new MySqlCommand(query, DBConnection.Instance.Connection))
                        {
                            MySqlDataReader dataReader = mySqlCommand.ExecuteReader();
                            while (dataReader.Read()){
                                list.Add(new User(dataReader.GetInt32(0), dataReader.GetString(1),
                                dataReader.GetString(2),dataReader.GetString(3), dataReader.GetString(4)));
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show(e.Message, this.ToString() + " Perform() Exception", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                return list;
            }
        }
    }
    
}
