using Gsafety.Common.Logging;
using Gsafety.Common.Util;
using Gsafety.MQ;
using Gsafety.PTMS.Analysis.Helper;
using Gsafety.PTMS.Common.Data;
using Gsafety.PTMS.Common.Enum;
using Gsafety.PTMS.Message.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Gsafety.PTMS.MessageLib
{
    public partial class MessageService
    {
        public void Register(UserModel usermodel)
        {
            try
            {
                LoggerManager.Logger.Info("Register session" + usermodel.UserToken);
                IMessageCallBackExt callback = OperationContext.Current.GetCallbackChannel<IMessageCallBackExt>();
                OperationContext.Current.Channel.Faulted += delegate(object sender, EventArgs e)
                {
                    LoggerManager.Logger.Info("Channel.Faulted");
                    try
                    {
                        lock (DataManager.SocketClients)
                        {
                            DataManager.SocketClients.Remove(usermodel.UserToken);
                            LoggerManager.Logger.Info("Remove Client:" + usermodel.UserToken);
                        }
                    }
                    catch (Exception ex)
                    {
                        LoggerManager.Logger.Error(ex);
                    }

                    try
                    {
                        if (usermodel.Organization != null)
                        {
                            lock (DataManager.Organization)
                            {
                                foreach (string item in usermodel.Organization)
                                {
                                    Dictionary<string, IMessageCallBackExt> organizations = DataManager.Organization[item];

                                    organizations.Remove(usermodel.UserToken);
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        LoggerManager.Logger.Error(ex);
                    }

                    try
                    {
                        lock (DataManager.Clients)
                        {
                            Dictionary<string, IMessageCallBackExt> clients = DataManager.Clients[usermodel.ClientID];

                            if (clients.ContainsKey(usermodel.UserToken))
                                clients.Remove(usermodel.UserToken);

                        }
                    }
                    catch (Exception ex)
                    {
                        LoggerManager.Logger.Error(ex);
                    }

                    try
                    {
                        if (usermodel.Vehicles != null)
                        {
                            lock (DataManager.Vehicles)
                            {
                                foreach (string item in DataManager.Vehicles.Keys)
                                {
                                    Dictionary<string, IMessageCallBackExt> dicvehicles = DataManager.Vehicles[item];
                                    if (dicvehicles.ContainsKey(usermodel.UserToken))
                                        dicvehicles.Remove(usermodel.UserToken);
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        LoggerManager.Logger.Error(ex);
                    }

                    try
                    {
                        lock (DataManager.Users)
                        {
                            Dictionary<string, IMessageCallBackExt> users = DataManager.Users[usermodel.UserID];

                            users.Remove(usermodel.UserToken);
                        }
                    }
                    catch (Exception ex)
                    {
                        LoggerManager.Logger.Error(ex);
                    }

                    try
                    {
                        lock (DataManager.Models)
                        {
                            DataManager.Models.Remove(usermodel.UserToken);
                        }
                    }
                    catch (Exception ex)
                    {
                        LoggerManager.Logger.Error(ex);
                    }

                    try
                    {
                        lock (DataManager.SocketClients)
                        {
                            if (DataManager.SocketClients.ContainsKey(usermodel.UserToken))
                            {
                                DataManager.SocketClients.Remove(usermodel.UserToken);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        LoggerManager.Logger.Error(ex);
                    }

                    byte[] sendmsg = ConvertHelper.ObjectToBytes(usermodel);

                    SendMessage(Constdefine.APPEXCHANGE, UserRoute.UserLogout, sendmsg);
                };
                //重联
                try
                {
                    lock (DataManager.SocketClients)
                    {
                        if (DataManager.SocketClients.ContainsKey(usermodel.UserToken))
                        {
                            DataManager.SocketClients[usermodel.UserToken] = callback;
                        }
                        else
                        {
                            DataManager.SocketClients.Add(usermodel.UserToken, callback);
                        }
                    }
                }
                catch (Exception ex)
                {
                    LoggerManager.Logger.Error(ex);
                }

                try
                {
                    if (usermodel.Vehicles != null)
                    {
                        lock (DataManager.Vehicles)
                        {
                            foreach (string item in usermodel.Vehicles)
                            {
                                if (!DataManager.Vehicles.ContainsKey(item))
                                {
                                    DataManager.Vehicles.Add(item, new Dictionary<string, IMessageCallBackExt>());
                                }

                                Dictionary<string, IMessageCallBackExt> dicvehicles = DataManager.Vehicles[item];

                                if (dicvehicles.ContainsKey(usermodel.UserToken))
                                {
                                    dicvehicles[usermodel.UserToken] = callback;
                                }
                                else
                                {
                                    dicvehicles.Add(usermodel.UserToken, callback);
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    LoggerManager.Logger.Error(ex);
                }

                try
                {

                    if (usermodel.Organization != null)
                    {
                        lock (DataManager.Organization)
                        {
                            foreach (string item in usermodel.Organization)
                            {
                                if (!DataManager.Organization.ContainsKey(item))
                                {
                                    DataManager.Organization.Add(item, new Dictionary<string, IMessageCallBackExt>());
                                }

                                Dictionary<string, IMessageCallBackExt> organizations = DataManager.Organization[item];

                                if (!organizations.ContainsKey(usermodel.UserToken))
                                {
                                    organizations.Add(usermodel.UserToken, callback);
                                }
                                else
                                {
                                    organizations[usermodel.UserToken] = callback;
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    LoggerManager.Logger.Error(ex);
                }
                try
                {

                    lock (DataManager.Clients)
                    {
                        if (!DataManager.Clients.ContainsKey(usermodel.ClientID))
                        {
                            DataManager.Clients.Add(usermodel.ClientID, new Dictionary<string, IMessageCallBackExt>());
                        }

                        Dictionary<string, IMessageCallBackExt> clients = DataManager.Clients[usermodel.ClientID];

                        if (!clients.ContainsKey(usermodel.UserToken))
                        {
                            clients.Add(usermodel.UserToken, callback);
                        }
                        else
                        {
                            clients[usermodel.UserToken] = callback;
                        }
                    }
                }
                catch (Exception ex)
                {
                    LoggerManager.Logger.Error(ex);
                }


                try
                {
                    lock (DataManager.Users)
                    {
                        if (!DataManager.Users.ContainsKey(usermodel.UserID))
                        {
                            DataManager.Users.Add(usermodel.UserID, new Dictionary<string, IMessageCallBackExt>());
                        }

                        Dictionary<string, IMessageCallBackExt> users = DataManager.Users[usermodel.UserID];

                        if (!users.ContainsKey(usermodel.UserToken))
                        {
                            users.Add(usermodel.UserToken, callback);
                        }
                        else
                        {
                            users[usermodel.UserToken] = callback;
                        }
                    }
                }
                catch (Exception ex)
                {
                    LoggerManager.Logger.Error(ex);
                }

                try
                {
                    lock (DataManager.Models)
                    {
                        if (DataManager.Models.ContainsKey(usermodel.UserToken))
                        {
                            DataManager.Models[usermodel.UserToken] = usermodel;
                        }
                        else
                        {
                            DataManager.Models.Add(usermodel.UserToken, usermodel);
                        }
                    }
                }
                catch (Exception ex)
                {
                    LoggerManager.Logger.Error(ex);
                }

                byte[] msg = ConvertHelper.ObjectToBytes(usermodel);

                SendMessage(Constdefine.APPEXCHANGE, UserRoute.UserLogin, msg);

                AuthenticationInfo info = new AuthenticationInfo();
                info.UserToken = usermodel.UserToken;

                string model = JsonHelper.ToJsonString(info);

                msg = System.Text.UTF8Encoding.UTF8.GetBytes(model);

                SendMessage(Constdefine.APPEXCHANGE, UserMessageRoute.AuthenticationRequest, msg);
            }
            catch (Exception ex)
            {
                LoggerManager.Logger.Error(ex);
            }
        }

        private void OnForceLogout(byte[] bytes)
        {
            try
            {
                LoggerManager.Logger.Info("OnForceLogout");
                UserModel model = ConvertHelper.BytesToObject(bytes) as UserModel;

                if (DataManager.Users.ContainsKey(model.UserID))
                {
                    Dictionary<string, IMessageCallBackExt> dictionary = DataManager.Users[model.UserID];

                    foreach (var item in dictionary.Keys)
                    {
                        if (item != model.UserToken)
                        {
                            try
                            {
                                dictionary[item].MessageCallBack(model);
                            }
                            catch (Exception ex)
                            {
                                LoggerManager.Logger.Error(ex);
                            }

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LoggerManager.Logger.Error(ex);
            }
        }

        private void OnForceMultiplyLogout(byte[] bytes)
        {
            LoggerManager.Logger.Info("OnForceMultiplyLogout");
            ClientUserList model = ConvertHelper.BytesToObject(bytes) as ClientUserList;

            UserModel usrmodel = new UserModel();
            usrmodel.MessageType = (short)MessageTypeEnum.ForceLogout;
            foreach (var item in model.UserList)
            {
                if (DataManager.Users.ContainsKey(item))
                {
                    IMessageCallBackExt[] usrs = DataManager.Users[item].Values.ToArray();

                    foreach (var usercallback in usrs)
                    {
                        try
                        {
                            usercallback.MessageCallBack(usrmodel);
                        }
                        catch (Exception ex)
                        {
                            LoggerManager.Logger.Error(ex);
                        }
                    }
                }
            }
        }

        private void OnAuthenticationReponse(byte[] bytes)
        {
            LoggerManager.Logger.Info("OnAuthenticationReponse");
            string str = System.Text.UnicodeEncoding.UTF8.GetString(bytes);
            AuthenticationInfo model = JsonHelper.FromJsonString<Gsafety.PTMS.Common.Data.AuthenticationInfo>(str);

            if (model.Code == true)
            {
                if (DataManager.SocketClients.ContainsKey(model.UserToken))
                {
                    DataManager.SocketClients[model.UserToken].MessageCallBack(model);
                }
            }
            else
            {
                foreach (var item in DataManager.SocketClients.Values)
                {
                    item.MessageCallBack(model);
                }
            }
        }
    }
}
