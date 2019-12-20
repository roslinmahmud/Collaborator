using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Common;
using System.Linq;
using System.Net.Sockets;
using System.Windows;

namespace Collaborator
{
    class User
    {
        private static User user = null;
        private List<User> contactlist = new List<User>();
        private TcpClient client = null;
        internal ObservableCollection<Message> messages = new ObservableCollection<Message>();
        private User()
        {
        }
        private User(int id, string username, string name, string photo_path, string ip)
        {
            Id = id;
            UserName = username;
            Name = name;
            Photo_Path = photo_path;
            Ip = ip;
        }
        public int Id { set; get; }
        public string UserName { set; get; }
        public string Name { set; get; }
        public string Photo_Path { set; get; }
        public string Ip { set; get; }
        public bool Alive { set; get; }
        public TcpClient Client
        {
            get{ return client; }
            set{ client = value; }
        }
        public static User Instance
        {
            get
            {
                if (user == null)
                {
                    user = new User();
                }
                return user;
            }
            set
            {
                user = value;
            }
        }
        
        public List<User> ContactList
        {
            get
            {
                if(!contactlist.Any())
                {
                    string query = "SELECT id, username, name, photo_path, ip FROM USER WHERE USERNAME!='" + Properties.Settings.Default.UserName + "';";
                    try
                    {
                        using (MySqlCommand mySqlCommand = new MySqlCommand(query, DBConnection.Instance.Connection))
                        {
                            MySqlDataReader dataReader = mySqlCommand.ExecuteReader();
                            while (dataReader.Read()){
                                User user = new User();
                                user.Init(dataReader);
                                contactlist.Add(user);
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show(e.Message, this.ToString() + " ContactList Exception", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                return contactlist;
            }
            set { contactlist = value; }
        }
        public void SyncMessage()
        {
            string query = "SELECT MESSAGE_DATA, DATE_TIME, SENDER_ID FROM MESSAGE WHERE (RECEIVER_ID='"+User.Instance.Id+"' AND SENDER_ID='"+Id+ "') OR (RECEIVER_ID='" + Id + "' AND SENDER_ID='" + User.Instance.Id + "');";
            try
            {
                using (MySqlCommand mySqlCommand = new MySqlCommand(query, DBConnection.Instance.Connection))
                {
                    DbDataReader dataReader = mySqlCommand.ExecuteReader();
                    while (dataReader.Read())
                    {
                        string dateTime = DateTime.Parse(dataReader.GetString(1)).ToString("hh:mm tt ddd");
                        if (dataReader.GetString(2) == Id.ToString())
                        {
                            messages.Add(new Message() { Text = dataReader.GetString(0), Align="Left", DateTime = dateTime, Color = "LightGray" });
                        }
                        else
                        {
                            messages.Add(new Message() { Text = dataReader.GetString(0), Align = "Right", DateTime = dateTime, Color = "LightBlue" });
                        }
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, this.ToString() + " ContactList Exception", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        public void Init(MySqlDataReader dataReader)
        {
            //MessageBox.Show(dataReader.GetString(1));
            Id = dataReader.GetInt32(0);
            UserName = dataReader.GetString(1);
            Name = dataReader.GetString(2);
            Photo_Path = dataReader.GetString(3);
            Ip = dataReader.GetString(4);
        }
        public void Init()
        {
            // MessageBox.Show("main user init " + Properties.Settings.Default.UserName);
            string query = "SELECT id, username, name, photo_path, ip FROM USER WHERE USERNAME='" + Properties.Settings.Default.UserName + "';";
            try
            {
                using (MySqlCommand mySqlCommand = new MySqlCommand(query, DBConnection.Instance.Connection))
                {
                    MySqlDataReader dataReader = mySqlCommand.ExecuteReader();
                    if (dataReader.Read())
                    {
                        Init(dataReader);
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, this.ToString() + " Init Exception", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            
        }
        public void Reset()
        {
            UnSave();
            user = null;
        }
        public void Save()
        {
            Properties.Settings.Default.UserName = UserName;
            Properties.Settings.Default.Save();
        }
        private void UnSave()
        {
            Properties.Settings.Default.UserName = string.Empty;
            Properties.Settings.Default.Save();
        }
        public bool Saved()
        {
            return Properties.Settings.Default.UserName != string.Empty;
        }
        public bool CheckIP()
        {
            return Ip == Connection.HostIP;
        }
        public void UpdateIP()
        {
            DBConnection.Instance.Connect();
            string query = "UPDATE USER SET IP='"+Connection.HostIP+"' WHERE USERNAME='" + Properties.Settings.Default.UserName + "';";
            try
            {
                using (MySqlCommand mySqlCommand = new MySqlCommand(query, DBConnection.Instance.Connection))
                {
                    if(mySqlCommand.ExecuteNonQuery() == 1)
                    {
                        Ip = Connection.HostIP;
                    }
                }
                
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, this.ToString() + " UpdateIP Exception", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
    
}
