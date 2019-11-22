using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//added
using System.Net.Sockets;
using System.IO;
using MessageCenterService.Share;
using Gsafety.Common.Util;
using System.Threading.Tasks;
using Gsafety.MQ;
using Gsafety.PTMS.Analysis.Helper;
using MessageCenterService.MessageHandlers;
using System.Xml;

namespace Gs.PTMS.MessageCenterService
{
    public class SocketConnection
    {
        private Socket Client;
        private SocketManager MServer;
        private string _clientID;

        public string ClientID
        {
            get { return _clientID; }
            set { _clientID = value; }
        }
        private string _userToken;
        private string _userid = string.Empty;

        public string UserID
        {
            get { return _userid; }
            set { _userid = value; }
        }

        public string UserToken
        {
            get { return _userToken; }
            set { _userToken = value; }
        }

        List<string> _organization = new List<string>();

        public List<string> Organization
        {
            get { return _organization; }
            set { _organization = value; }
        }


        public SocketConnection(Socket Client, SocketManager server)
        {
            this.Client = Client;
            this.MServer = server;
        }

        private byte[] Message = new byte[1024 * 800];

        public void Start()
        {
            try
            {
                // Listen for messages.
                Client.BeginReceive(Message, 0, Message.Length, SocketFlags.None, new AsyncCallback(OnMsgReceived), null);
            }
            catch (SocketException se)
            {
                Console.WriteLine(se.Message);
            }
        }

        public void OnMsgReceived(IAsyncResult ar)
        {
            try
            {
                int bytesRead = Client.EndReceive(ar);
                if (bytesRead > 0)
                {
                    //Sending message to all the connected clients.
                    Task.Factory.StartNew(() => { OnMessage(Message, bytesRead); });

                    if (Client != null)
                    {
                        // Listen for next messages.
                        Client.BeginReceive(Message, 0, Message.Length, SocketFlags.None, new AsyncCallback(OnMsgReceived), null);
                    }
                }
            }
            catch (SocketException ex)
            {
                if (ex.ErrorCode == 10054)
                {
                    List<SocketConnection> connections = new List<SocketConnection>();
                    connections.Add(this);
                    SocketManager.RemoveInvalidClient(connections);
                    if (this.Client != null)
                    {
                        this.Client.Close();
                        this.Client = null;
                    }
                }
            }
            catch (Exception err)
            {

                Console.WriteLine(err.Message);
            }
        }

        private void OnMessage(byte[] buffer, int bytesRead)
        {
            try
            {
                Message message = TypeConverter.ByteToObject<Message>(buffer, bytesRead);
                switch (message.MessageType)
                {
                    case MessageType.HeartBeat:
                        break;
                    case MessageType.Register:
                        RegisterHandler handler = new RegisterHandler();
                        handler.OnMessage(message, this);
                        break;
                    case MessageType.RegisterReply:
                        break;
                    case MessageType.UserLogin:
                        break;
                    case MessageType.UnRegister:
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void Close()
        {
            try
            {
                Client.Close();
                Client = null;
            }
            catch (Exception err)
            {
                Console.WriteLine(err.Message);
            }
        }

        public void SendMessage(byte[] data, int bytesRead)
        {
            Client.Send(data, 0, bytesRead, SocketFlags.None);
        }

        public void SendRegisterReply()
        {
            Message message = new Message();
            message.MessageType = MessageType.RegisterReply;
            message.Body = TypeConverter.ObjectToByte<string>(UserToken);

            byte[] bytes = TypeConverter.ObjectToByte<Message>(message);
            SendMessage(bytes, bytes.Length);
        }
    }
}