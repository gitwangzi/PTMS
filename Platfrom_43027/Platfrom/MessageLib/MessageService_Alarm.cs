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
        private void OnAlarm(string key, byte[] bytes)
        {
            try
            {
                LoggerManager.Logger.Info("OnAlarm");
                AlarmInfoEx model = ConvertHelper.BytesToObject(bytes) as AlarmInfoEx;

                foreach (string item in model.Organizations)
                {
                    if (DataManager.Organization.ContainsKey(item))
                    {
                        Dictionary<string, IMessageCallBackExt> dictionary = DataManager.Organization[item];

                        foreach (var user in dictionary.Keys)
                        {
                            try
                            {
                                var callback = dictionary[user];
                                callback.MessageCallBack(model);
                                LoggerManager.Logger.Info("Send Alarm Info To :" + user);
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


        public void CompleteAlarm(CompleteAlarm alarm)
        {

            try
            {
                LoggerManager.Logger.Info("CompleteAlarm");
                byte[] msg = ConvertHelper.ObjectToBytes(alarm);

                SendMessage(Constdefine.APPEXCHANGE, AlarmRoute.CompleteAlarm + alarm.VehicleID, msg);
            }
            catch (Exception ex)
            {
                LoggerManager.Logger.Error(ex);
            }

        }

        private void OnCompleteAlarmNotice(string key, byte[] bytes)
        {
            try
            {
                LoggerManager.Logger.Info("OnCompleteAlarmNotice");
                CompleteAlarm complateAlarm = ConvertHelper.BytesToObject(bytes) as CompleteAlarm;


                foreach (string item in complateAlarm.Organizations)
                {
                    if (DataManager.Organization.ContainsKey(item))
                    {
                        Dictionary<string, IMessageCallBackExt> dictionary = DataManager.Organization[item];

                        foreach (var callback in dictionary.Values)
                        {
                            try
                            {
                                callback.MessageCallBack(complateAlarm);
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

        public void TransferAlarm(AlarmInfoEx alarm)
        {
            try
            {
                LoggerManager.Logger.Info("TransferAlarm");
                byte[] msg = ConvertHelper.ObjectToBytes(alarm);

                SendMessage(Constdefine.APPEXCHANGE, AlarmRoute.JudgeTransferKey + alarm.VehicleId, msg);
            }
            catch (Exception ex)
            {
                LoggerManager.Logger.Error(ex);
            }
        }
    }
}
