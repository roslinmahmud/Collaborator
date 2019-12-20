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
using System.Windows.Controls;

namespace Collaborator
{
    class Server
    {
        TcpListener server = null;
        TcpClient client = null;
        IPAddress iPAddress = null;
        StringBuilder message = null;
        BackgroundWorker worker = null;
        ScrollViewer scrollViewer = null;
        List<User> users = null;
        Dictionary<string, int> index = null;
        public Server()
        {    
        }
        public Server(ScrollViewer scrollViewer)
        {
            iPAddress = IPAddress.Parse(Connection.HostIP);
            server = new TcpListener(iPAddress, 11998);

            message = new StringBuilder();
            worker = new BackgroundWorker();

            this.scrollViewer = scrollViewer;
        }
        public void StartServer()
        {
            server.Start();

            PairUserIP();

            AcceptClient();

            RetrieveMessage();
   
            ReceiveMessages();
        }
        public void StopServer()
        {
            server.Stop();
        }
        private void PairUserIP()
        {
            users = User.Instance.ContactList;
            index = new Dictionary<string, int>();

            for (int i = 0; i < users.Count; i++)
            {
                //Creating <userIP, index> pairs
                index[users[i].Ip] = i;
            }
        }
        private async void AcceptClient()
        {
            while (true)
            {
                try
                {
                    client = await server.AcceptTcpClientAsync();
                    AssignClient(client.Client.RemoteEndPoint.ToString().Split(':')[0]);
                }
                catch (Exception e)
                {
                    break;
                }
            }
        }
        private void RetrieveMessage()
        {
            for (int i=0;i<users.Count;i++)
            {
                //Syncing users previous messages
                users[i].SyncMessage();
            }
        }
        private void ReceiveMessages()
        {
            worker.DoWork += Worker_DoWork;
            worker.ProgressChanged += Worker_ProgressChanged;
            worker.WorkerReportsProgress = true;
            worker.RunWorkerAsync();
        }

        private void Worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            User.Instance.ContactList[e.ProgressPercentage].messages.Add(new Message() { Text = message.ToString(), Align="Left", DateTime = DateTime.Now.ToString("hh:mm tt ddd"), Color = "LightGray" });
            scrollViewer.ScrollToBottom();
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
                    while (users[i].Client.Available != 0)
                    {
                        sz = stream.Read(bytes, 0, bytes.Length);
                        message.Append(Encoding.ASCII.GetString(bytes, 0, sz));
                    }
                    if(message.Length > 0) 
                    {
                        worker.ReportProgress(i);
                    }

                }
                Thread.Sleep(1000);
            }
        }

        private void AssignClient(string IP)
        {
            users[index[IP]].Client = client;
            users[index[IP]].Alive = true;
            //MessageBox.Show("Assigned to "+ users[i].Name);
        }
    }
}
