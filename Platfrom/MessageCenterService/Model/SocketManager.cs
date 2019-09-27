using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//added
using System.Net.Sockets;
using System.Threading;
using System.Net;
using System.Threading.Tasks;
using MessageCenterService.Share;
using System.IO;
using System.Runtime.Serialization;
using Gsafety.Common.Util;

namespace Gs.PTMS.MessageCenterService
{
    public class SocketManager
    {
        private Socket Listener;
        private bool isRunning;
        private byte[] _heartbeat = null;
        private readonly int HeartbeatSpan = 30000;
        public void Start()
        {
            Listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            Listener.SetSocketOption(SocketOptionLevel.Tcp, (SocketOptionName)SocketOptionName.NoDelay, 0);
            // The allowed port range in Silverlight is 4502 to 4534.
            Listener.Bind(new IPEndPoint(IPAddress.Any, 4530));
            Listener.Listen(20);
            Listener.BeginAccept(new AsyncCallback(OnConnection), null);
            isRunning = true;

            Task.Factory.StartNew(() => { SendHeartBeat(); });
        }

        private void SendHeartBeat()
        {
            Message message = new Message();
            message.MessageType = MessageType.HeartBeat;

            _heartbeat = TypeConverter.ObjectToByte<Message>(message);

            while (isRunning)
            {
                SocketConnection[] clients = null;
                List<SocketConnection> invalidclients = new List<SocketConnection>();
                lock (DataManager.SocketClients)
                {
                    clients = DataManager.SocketClients.Values.ToArray();
                }


                foreach (var item in clients)
                {
                    try
                    {
                        item.SendMessage(_heartbeat, _heartbeat.Length);
                    }
                    catch (Exception)
                    {
                        invalidclients.Add(item);
                    }
                }

                if (invalidclients.Count > 0)
                {
                    RemoveInvalidClient(invalidclients);
                }

                Thread.Sleep(HeartbeatSpan);
            }
        }

        public static void RemoveInvalidClient(List<SocketConnection> invalidclients)
        {
            lock (DataManager.SocketClients)
            {
                foreach (var item in invalidclients)
                {
                    if (!string.IsNullOrEmpty(item.UserToken))
                    {
                        DataManager.SocketClients.Remove(item.UserToken);
                    }
                }
            }

            lock (DataManager.Clients)
            {
                foreach (var item in invalidclients)
                {
                    if (!string.IsNullOrEmpty(item.UserToken))
                    {
                        if (DataManager.Clients.ContainsKey(item.ClientID))
                        {
                            if (DataManager.Clients[item.ClientID].ContainsKey(item.UserToken))
                            {
                                DataManager.Clients[item.ClientID].Remove(item.UserToken);
                                break;
                            }
                        }
                    }
                }
            }

            lock (DataManager.Organization)
            {
                foreach (var item in invalidclients)
                {
                    if (!string.IsNullOrEmpty(item.UserToken))
                    {
                        foreach (string organization in item.Organization)
                        {
                            if (DataManager.Organization.ContainsKey(organization))
                            {
                                if (DataManager.Clients[organization].ContainsKey(item.UserToken))
                                {
                                    DataManager.Clients[organization].Remove(item.UserToken);
                                }
                            }
                        }
                    }
                }
            }

            lock (DataManager.Users)
            {
                foreach (var item in invalidclients)
                {
                    if (!string.IsNullOrEmpty(item.UserToken))
                    {
                        if (DataManager.Users.ContainsKey(item.UserID))
                        {
                            if (DataManager.Users[item.UserID].ContainsKey(item.UserToken))
                            {
                                DataManager.Users[item.UserID].Remove(item.UserToken);
                                break;
                            }
                        }
                    }
                }
            }
        }

        private void OnConnection(IAsyncResult ar)
        {
            if (isRunning == false)
                return;

            // Then look for other connections
            Listener.BeginAccept(new AsyncCallback(OnConnection), null);
            Socket Client = Listener.EndAccept(ar);

            // Handle the current connection.            
            SocketConnection NewClient = new SocketConnection(Client, this);
            NewClient.Start();
        }

        public void Close()
        {
            isRunning = false;
            if (Listener != null)
            {
                try
                {
                    Listener.Close();
                }
                catch (Exception err)
                {
                    Console.WriteLine(err.Message);
                }
            }
            //close connection for each connected clients
            foreach (SocketConnection client in DataManager.SocketClients.Values)
            {
                client.Close();
            }
        }

        public void OnMessage(byte[] buffer, int bytesRead)
        {

        }

        public void DeliverMessage(byte[] message, int bytesRead)
        {
            Console.WriteLine("Delivering the message...");
            // Duplication of connection to prevent cross threading issue
            SocketConnection[] ClientsConnected;
            lock (DataManager.SocketClients)
            {
                ClientsConnected = DataManager.SocketClients.Values.ToArray();
            }

            foreach (SocketConnection cnt in ClientsConnected)
            {
                try
                {
                    cnt.SendMessage(message, bytesRead);
                }
                catch
                {
                    // Remove the disconnected client
                    lock (DataManager.SocketClients)
                    {
                        DataManager.SocketClients.Remove(cnt.UserToken);
                    }

                    cnt.Close();
                }
            }

        }

        public static void OnRabbitMqMessage(string routekey, byte[] data)
        {
            switch (routekey)
            {
                default:
                    break;
            }
        }
    }
}
