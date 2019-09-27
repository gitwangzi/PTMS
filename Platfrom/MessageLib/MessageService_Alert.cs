using Gsafety.Common.Logging;
using Gsafety.Common.Util;
using Gsafety.MQ;
using Gsafety.PTMS.Analysis.Helper;
using Gsafety.PTMS.Common.Data;
using Gsafety.PTMS.Message.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gsafety.PTMS.MessageLib
{
    public partial class MessageService
    {
        private void OnDeviceAlert(string key, byte[] bytes)
        {
            try
            {
                LoggerManager.Logger.Info("OnDeviceAlert");
                DeviceAlertEx model = ConvertHelper.BytesToObject(bytes) as DeviceAlertEx;

                foreach (string item in model.Organizations)
                {
                    if (DataManager.Organization.ContainsKey(item))
                    {
                        Dictionary<string, IMessageCallBackExt> dictionary = DataManager.Organization[item];

                        foreach (var usr in dictionary.Keys)
                        {
                            try
                            {
                                var callback = dictionary[usr];
                                callback.MessageCallBack(model);
                                LoggerManager.Logger.Info("Send DeviceAlert Info To :" + usr);
                            }
                            catch (Exception)
                            {
                                LoggerManager.Logger.Info("Failed to Send DeviceAlert Info To :" + usr);
                            }
                        }
                    }
                }

            }
            catch (Exception)
            {

            }
        }

        private void OnBusinessAlert(string key, byte[] bytes)
        {
            try
            {
                LoggerManager.Logger.Info("OnBusinessAlert");
                BusinessAlertEx model = ConvertHelper.BytesToObject(bytes) as BusinessAlertEx;

                foreach (string item in model.Organizations)
                {
                    if (DataManager.Organization.ContainsKey(item))
                    {
                        Dictionary<string, IMessageCallBackExt> dictionary = DataManager.Organization[item];

                        foreach (var usr in dictionary.Keys)
                        {
                            try
                            {
                                var callback = dictionary[usr];
                                callback.MessageCallBack(model);
                                LoggerManager.Logger.Info("Send BusinessAlert Info To :" + usr);
                            }
                            catch (Exception)
                            {
                                LoggerManager.Logger.Info("Failed to Send BusinessAlert Info To :" + usr);
                            }
                        }
                    }
                }

            }
            catch (Exception)
            {

            }
        }

        public void CompleteAlert(Common.Data.CompleteAlert alert)
        {
            try
            {
                LoggerManager.Logger.Info("CompleteAlert");
                byte[] msg = ConvertHelper.ObjectToBytes(alert);

                SendMessage(Constdefine.APPEXCHANGE, AlertRoute.CompleteAlert + alert.VehicleID, msg);
            }
            catch (Exception ex)
            {
                LoggerManager.Logger.Error(ex);
            }
        }

        private void OnCompleteAlertNotice(string key, byte[] bytes)
        {
            try
            {
                LoggerManager.Logger.Info("OnCompleteAlertNotice");
                CompleteAlert complateAlert = ConvertHelper.BytesToObject(bytes) as CompleteAlert;

                foreach (string item in complateAlert.Organizations)
                {
                    if (DataManager.Organization.ContainsKey(item))
                    {
                        Dictionary<string, IMessageCallBackExt> dictionary = DataManager.Organization[item];

                        foreach (var callback in dictionary.Values)
                        {
                            try
                            {
                                callback.MessageCallBack(complateAlert);
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
    }
}
