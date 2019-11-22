using Gs.PTMS.MessageCenterService;
using Gsafety.Common.Util;
using Gsafety.MQ;
using Gsafety.PTMS.Analysis.Helper;
using MessageCenterService.Share;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageCenterService.MessageHandlers
{
    public class RegisterHandler : MessageHandler
    {
        public override void OnMessage(Share.Message message, SocketConnection connection)
        {
            if (message.Body != null)
            {
                RegisterEntity entity = TypeConverter.ByteToObject<RegisterEntity>(message.Body, message.Body.Length);
                connection.ClientID = message.ClientID;
                connection.UserID = entity.UserID;
                connection.UserToken = entity.UserToken;

                if (!string.IsNullOrEmpty(entity.UserToken))
                {
                    if (DataManager.SocketClients.ContainsKey(entity.UserToken))
                    {
                        lock (DataManager.SocketClients)
                        {
                            DataManager.SocketClients[entity.UserToken].Close();
                            DataManager.SocketClients[entity.UserToken] = connection;
                        }
                    }
                    else
                    {
                        lock (DataManager.SocketClients)
                        {
                            DataManager.SocketClients[entity.UserToken] = connection;
                        }
                    }

                    lock (DataManager.Organization)
                    {
                        foreach (var item in entity.Organization)
                        {
                            connection.Organization.Add(item);
                            if (!DataManager.Organization.ContainsKey(item))
                            {
                                DataManager.Organization.Add(item, new Dictionary<string, SocketConnection>());
                            }

                            Dictionary<string, SocketConnection> organizations = DataManager.Organization[item];

                            if (!organizations.ContainsKey(connection.UserToken))
                            {
                                organizations.Add(connection.UserToken, connection);
                            }
                            else
                            {
                                organizations[connection.UserToken] = connection;
                            }
                        }
                    }

                    lock (DataManager.Clients)
                    {
                        if (!DataManager.Clients.ContainsKey(message.ClientID))
                        {
                            DataManager.Clients.Add(message.ClientID, new Dictionary<string, SocketConnection>());
                        }

                        Dictionary<string, SocketConnection> clients = DataManager.Clients[message.ClientID];

                        if (!clients.ContainsKey(entity.UserToken))
                        {
                            clients.Add(entity.UserToken, connection);
                        }
                        else
                        {
                            clients[entity.UserToken] = connection;
                        }
                    }

                    lock (DataManager.Users)
                    {
                        if (!DataManager.Users.ContainsKey(connection.UserID))
                        {
                            DataManager.Users.Add(connection.UserID, new Dictionary<string, SocketConnection>());
                        }

                        Dictionary<string, SocketConnection> users = DataManager.Users[connection.UserID];

                        if (!users.ContainsKey(connection.UserToken))
                        {
                            users.Add(connection.UserToken, connection);
                        }
                        else
                        {
                            users[connection.UserToken] = connection;
                        }
                    }
                }
                else
                {
                    connection.UserToken = Guid.NewGuid().ToString();
                    lock (DataManager.SocketClients)
                    {
                        DataManager.SocketClients[connection.UserToken] = connection;
                    }

                    lock (DataManager.Organization)
                    {
                        foreach (var item in entity.Organization)
                        {
                            if (!DataManager.Organization.ContainsKey(item))
                            {
                                DataManager.Organization.Add(item, new Dictionary<string, SocketConnection>());
                            }

                            Dictionary<string, SocketConnection> organizations = DataManager.Organization[item];

                            if (!organizations.ContainsKey(connection.UserToken))
                            {
                                organizations.Add(connection.UserToken, connection);
                            }
                        }
                    }

                    lock (DataManager.Clients)
                    {
                        if (!DataManager.Clients.ContainsKey(message.ClientID))
                        {
                            DataManager.Clients.Add(message.ClientID, new Dictionary<string, SocketConnection>());
                        }

                        Dictionary<string, SocketConnection> clients = DataManager.Clients[message.ClientID];

                        if (!clients.ContainsKey(connection.UserToken))
                        {
                            clients.Add(connection.UserToken, connection);
                        }

                    }

                    lock (DataManager.Users)
                    {
                        if (!DataManager.Users.ContainsKey(connection.UserID))
                        {
                            DataManager.Users.Add(connection.UserID, new Dictionary<string, SocketConnection>());
                        }

                        Dictionary<string, SocketConnection> users = DataManager.Users[connection.UserID];

                        if (!users.ContainsKey(connection.UserToken))
                        {
                            users.Add(connection.UserToken, connection);
                        }
                    }
                }

                Message replymessage = new Message();
                replymessage.MessageType = MessageType.RegisterReply;
                replymessage.Body = TypeConverter.ObjectToByte<string>(connection.UserToken);
                byte[] replybytes = TypeConverter.ObjectToByte<Message>(replymessage);

                connection.SendMessage(replybytes, replybytes.Length);

                //向rabbitmq发送消息
                Message rabbitmqmessage = new Message();
                rabbitmqmessage.MessageType = MessageType.UserLogin;
                replymessage.Body = TypeConverter.ObjectToByte<string>(connection.UserID);
                byte[] rabbitmqbytes = TypeConverter.ObjectToByte<Message>(rabbitmqmessage);

                RabbitMqManager.PublishMessage(Constdefine.APPEXCHANGE, UserMessageRoute.UserLogin, rabbitmqbytes);
            }
        }
    }
}
