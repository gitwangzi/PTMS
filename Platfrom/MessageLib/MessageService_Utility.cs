using Gsafety.Common.Logging;
using Gsafety.Common.Util;
using Gsafety.MQ;
using Gsafety.PTMS.Analysis.Helper;
using Gsafety.PTMS.Common.Data;
using Gsafety.PTMS.Message.Contract;
using Gsafety.PTMS.Message.Contract.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Gsafety.PTMS.MessageLib
{
    public partial class MessageService
    {
        private readonly int HeartbeatSpan = 30000;
        private void HeartbeatSending(string queue)
        {
            while (true)
            {
                try
                {
                    KeyValuePair<string, IMessageCallBackExt>[] ClientsConnected;
                    lock (DataManager.SocketClients)
                    {
                        ClientsConnected = DataManager.SocketClients.ToArray();
                    }

                    if (_stop)
                    {
                        return;
                    }

                    HeartbeatInfo heartbeatInfo = new HeartbeatInfo();

                    heartbeatInfo.CurrentTime = DateTime.Now;

                    List<UserModel> users = new List<UserModel>();
                    foreach (var item in ClientsConnected)
                    {
                        try
                        {
                            item.Value.MessageCallBack(heartbeatInfo);
                        }
                        catch (Exception)
                        {
                            users.Add(DataManager.Models[item.Key]);
                            LoggerManager.Logger.Info(DataManager.Models[item.Key].UserToken + " add to remove list");
                        }
                    }
                    if (users.Count != 0)
                    {
                        lock (DataManager.SocketClients)
                        {
                            foreach (var item in users)
                            {
                                DataManager.SocketClients.Remove(item.UserToken);
                            }

                        }

                        lock (DataManager.Organization)
                        {
                            foreach (string organization in DataManager.Organization.Keys)
                            {
                                foreach (var item in users)
                                {
                                    if (DataManager.Organization[organization].ContainsKey(item.UserToken))
                                        DataManager.Organization[organization].Remove(item.UserToken);
                                }
                            }

                        }


                        lock (DataManager.Vehicles)
                        {
                            foreach (var item in users)
                            {
                                foreach (var vehicle in item.Vehicles)
                                {
                                    DataManager.Vehicles[vehicle].Remove(item.UserToken);
                                }
                            }
                        }

                        lock (DataManager.Users)
                        {
                            foreach (var item in users)
                            {
                                DataManager.Users.Remove(item.UserToken);
                            }
                        }

                        lock (DataManager.Models)
                        {
                            foreach (var item in users)
                            {
                                DataManager.Models.Remove(item.UserToken);
                            }
                        }

                        byte[] msg = ConvertHelper.ObjectToBytes(users);
                        string routeKey = UserRoute.UserLogout;

                        SendMessage(Constdefine.APPEXCHANGE, routeKey, msg);
                    }

                }
                catch (Exception ex)
                {
                    LoggerManager.Logger.Error(ex);
                    ClearConn();
                    return;
                }
                Thread.Sleep(HeartbeatSpan);
            }
        }

        private void UserOnlineHeartBeatSending()
        {
            while (true)
            {
                try
                {
                    UserOnlineHeartBeat heartbeat = new UserOnlineHeartBeat();
                    lock (DataManager.Clients)
                    {
                        heartbeat.SessionIDs.AddRange(DataManager.SocketClients.Keys.ToList());
                    }

                    if (_stop)
                    {
                        return;
                    }

                    byte[] msg = ConvertHelper.ObjectToBytes(heartbeat);
                    string routeKey = UserRoute.UserOnlineHeartBeat;

                    SendMessage(Constdefine.APPEXCHANGE, routeKey, msg);

                    Thread.Sleep(1000 * 100);
                }
                catch
                {

                }
            }
        }
    }
}
