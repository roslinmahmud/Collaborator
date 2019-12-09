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
        
        public void SendMessage(string message, TcpClient client)
        {
            Byte[] data = System.Text.Encoding.ASCII.GetBytes(message);

            NetworkStream stream = client.GetStream();

            stream.Write(data, 0, data.Length);
        }

        public void StopClient()
        {
            worker.CancelAsync();
        }
        public void StartClient()
        {
            worker = new BackgroundWorker();
            worker.DoWork += Worker_DoWork;
            worker.WorkerSupportsCancellation = true;
            worker.RunWorkerAsync();

            
        }

        private void Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            List<User> contacts = User.Instance.ContactList;
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
                            Connect(i);
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
                Thread.Sleep(100);
            }
            
        }

        private void Connect(int ind)
        {
            try
            {
                User.Instance.ContactList[ind].Client = new TcpClient(User.Instance.ContactList[ind].Ip, 11998);

                MessageBox.Show(User.Instance.ContactList[ind].Client.Client.RemoteEndPoint.ToString().Split(':')[0]);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }

        }
    }
}
