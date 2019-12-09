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
        TcpClient client = null;
        Ping ping = null;
        BackgroundWorker worker = null;
        public Client()
        {
            ping = new Ping();
            client = User.Instance.Client;
        }
        public void Connect(string IP, bool status)
        {
            try
            {
                if (status)
                {
                    if (User.Instance.Client != null)
                    {
                        if (User.Instance.Client.Client.RemoteEndPoint.ToString().Split(':')[0] != IP)
                        {
                            User.Instance.Client.Close();
                            User.Instance.Client = new TcpClient(IP, 11998);
                            MessageBox.Show(User.Instance.Client.Client.RemoteEndPoint.ToString().Split(':')[0]);
                        }
                    }
                    else
                    {
                        User.Instance.Client = new TcpClient(IP, 11998);
                        MessageBox.Show(User.Instance.Client.Client.RemoteEndPoint.ToString().Split(':')[0]);
                    }
                }
            }
            catch(Exception e)
            {
                MessageBox.Show(e.Message);
            }
            
        }
        
        public void SendMessage(string message)
        {
            Byte[] data = System.Text.Encoding.ASCII.GetBytes(message);

            NetworkStream stream = User.Instance.Client.GetStream();

            stream.Write(data, 0, data.Length);
        }

        public void StopClient()
        {
            worker.CancelAsync();
        }
        public void CheckStatus()
        {
            worker = new BackgroundWorker();
            worker.DoWork += Worker_DoWork;
            worker.WorkerSupportsCancellation = true;
            worker.RunWorkerAsync();

            
        }

        private void Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            List<User> contacts = User.Instance.ContactList;
            while (true)
            {
                if (worker.CancellationPending)
                {
                    contacts.Clear();
                    break;
                }
                for (int i = 0; i < contacts.Count; i++)
                {
                    IPAddress iP = IPAddress.Parse(contacts[i].Ip);
                    if (ping.Send(iP).Status == IPStatus.Success)
                    {
                        contacts[i].Alive = true;
                    }
                    else
                    {
                        contacts[i].Alive = false;
                    }
                }
                Thread.Sleep(100);
            }
            
        }
    }
}
