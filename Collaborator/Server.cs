using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.Windows;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Threading;

namespace Collaborator
{
    class Server
    {
        TcpListener server = null;
        TcpClient client = null;
        IPAddress iPAddress = null;
        StringBuilder message = null;
        BackgroundWorker worker = null;
        public Server()
        {
            iPAddress = IPAddress.Parse(Connection.HostIP);
            server = new TcpListener(iPAddress, 11998);

            message = new StringBuilder();
            worker = new BackgroundWorker();
        }
        public async void StartServer()
        {
            server.Start();
            
            worker.DoWork += Worker_DoWork;
            worker.ProgressChanged += Worker_ProgressChanged;
            worker.WorkerReportsProgress = true;
            worker.RunWorkerAsync();

            while (true)
            {
                client = await server.AcceptTcpClientAsync();
                AssignClient(client.Client.RemoteEndPoint.ToString().Split(':')[0]);
            }
        }

        private void Worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            User.Instance.ContactList[e.ProgressPercentage].messages.Add(new Message() { Text = message.ToString() });
            message.Clear();
        }

        private void Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            List<User> users = User.Instance.ContactList;
            while (true)
            {
                for (int i = 0; i < users.Count; i++)
                {
                    if (users[i].Client == null)
                        continue;
                    byte[] bytes = new byte[1024];
                    NetworkStream stream = users[i].Client.GetStream();
                    int sz;
                    if (users[i].Client.Available != 0)
                    {
                        sz = stream.Read(bytes, 0, bytes.Length);
                        message.Append(Encoding.ASCII.GetString(bytes, 0, sz));
                        worker.ReportProgress(i);
                    }

                }
                Thread.Sleep(1000);
            }
        }

        public void AssignClient(string IP)
        {
            List<User> users = new List<User>(User.Instance.ContactList);
            for(int i = 0; i < users.Count; i++)
            {
                if (users[i].Ip.Equals(IP))
                {
                    users[i].Client = this.client;
                    MessageBox.Show("Assigned to "+ users[i].Name);
                }
            }
        }
    }
}
