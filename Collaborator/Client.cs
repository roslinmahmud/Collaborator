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
                        if (contacts[i].Client == null)
                        {
                            contacts[i].Alive = true;
                            contacts[i].Client = GetClient(contacts[i]);
                        }
                    }
                    else
                    {
                        if (contacts[i].Client != null)
                        {
                            contacts[i].Alive = false;
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
                user.Alive = false;
                return null;
            }
        }

    }
}
