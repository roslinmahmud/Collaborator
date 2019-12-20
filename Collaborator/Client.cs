using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace Collaborator
{
    class Client
    {
        Ping ping = null;
        BackgroundWorker worker = null;
        public Client()
        {
        }
        
        public void SendMessage(string message, TcpClient client)
        {
            try
            {
                Byte[] data = System.Text.Encoding.ASCII.GetBytes(message);

                NetworkStream stream = client.GetStream();

                stream.Write(data, 0, data.Length);
            }
            catch(Exception e)
            {
                MessageBox.Show(e.Message, this.ToString() + " SendMessage() Exception", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        public async void StoreMessage(string message, string receiverID)
        {
            string date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string query = "INSERT INTO MESSAGE (SENDER_ID, RECEIVER_ID, MESSAGE_DATA, DATE_TIME) VALUES ('" + User.Instance.Id+"','"+receiverID+"','"+message+"','"+date+"');";
            try
            {
                using (MySqlCommand mySqlCommand = new MySqlCommand(query, DBConnection.Instance.Connection))
                {
                    await mySqlCommand.ExecuteNonQueryAsync();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, this.ToString() + " StoreMessage Exception", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        public void StartClient()
        {
            worker = new BackgroundWorker();
            worker.DoWork += Worker_DoWork;
            worker.WorkerSupportsCancellation = true;
            worker.RunWorkerAsync();
        }
        public void StopClient()
        {
            worker.CancelAsync();
        }

        private void Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            List<User> contacts = User.Instance.ContactList;
            ping = new Ping();
            IPAddress iP;
            while (true)
            {
                if (worker.CancellationPending)
                {
                    contacts.Clear();
                    break;
                }
                for (int i = 0; i < contacts.Count; i++)
                {
                    iP = IPAddress.Parse(contacts[i].Ip);
                    if (ping.Send(iP).Status == IPStatus.Success)
                    {
                        contacts[i].Alive = true;
                        if (contacts[i].Client == null)
                        {
                            contacts[i].Client = GetClient(contacts[i]);
                        }
                    }
                    else
                    {
                        contacts[i].Alive = false;
                        if (contacts[i].Client != null)
                        {
                            contacts[i].Client = null;
                        }
                    }
                }
                Thread.Sleep(10000);
            }
            
        }

        private TcpClient GetClient(User user)
        {
            try
            {
                return new TcpClient(user.Ip, 11998);
                //MessageBox.Show(user.Name + " Connected");
            }
            catch (Exception e)
            {
                //MessageBox.Show(e.Message, this.ToString() + " GetClient() Exception", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }
        }

    }
}
