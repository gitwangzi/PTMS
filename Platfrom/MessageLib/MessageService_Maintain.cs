using Gsafety.Common.Logging;
using Gsafety.Common.Util;
using Gsafety.MQ;
using Gsafety.PTMS.Analysis.Helper;
using Gsafety.PTMS.Common.Data;
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
        public void BeginInstallSuite(string mdvrcoresn)
        {
            try
            {
                LoggerManager.Logger.Info("BeginInstallSuite:" + mdvrcoresn);
                byte[] sendmsg = ConvertHelper.ObjectToBytes(mdvrcoresn);

                SendMessage(Constdefine.APPEXCHANGE, UserMessageRoute.StartSuiteInstallKey, sendmsg);
            }
            catch (Exception ex)
            {
                LoggerManager.Logger.Error(ex);
            }
        }

        public void CompleteInstallSuite(string mdvrcoresn, string organization, string vehicle)
        {
            try
            {
                LoggerManager.Logger.Info("CompleteInstallSuite:" + mdvrcoresn);
                byte[] sendmsg = ConvertHelper.ObjectToBytes(mdvrcoresn);

                SendMessage(Constdefine.APPEXCHANGE, UserMessageRoute.CompleteSuiteInstallKey, sendmsg);
            }
            catch (Exception ex)
            {
                LoggerManager.Logger.Error(ex);
            }
        }


        public void RemoveInstallSuite(string mdvrcoresn)
        {
            try
            {
                LoggerManager.Logger.Info("RemoveInstallSuite:" + mdvrcoresn);
                byte[] sendmsg = ConvertHelper.ObjectToBytes(mdvrcoresn);

                SendMessage(Constdefine.APPEXCHANGE, UserMessageRoute.DeleteSuiteInstallKey, sendmsg);
            }
            catch (Exception ex)
            {
                LoggerManager.Logger.Error(ex);
            }
        }

        public void BeginInstallGPS(string mdvrcoresn)
        {
            try
            {
                LoggerManager.Logger.Info("BeginInstallGPS:" + mdvrcoresn);
                byte[] sendmsg = ConvertHelper.ObjectToBytes(mdvrcoresn);

                SendMessage(Constdefine.APPEXCHANGE, UserMessageRoute.StartGPSInstallKey, sendmsg);
            }
            catch (Exception ex)
            {
                LoggerManager.Logger.Error(ex);
            }
        }

        public void CompleteInstallGPS(string mdvrcoresn, string organization, string vehicle)
        {
            try
            {
                LoggerManager.Logger.Info("CompleteInstallGPS:" + mdvrcoresn);
                byte[] sendmsg = ConvertHelper.ObjectToBytes(mdvrcoresn);

                SendMessage(Constdefine.APPEXCHANGE, UserMessageRoute.CompleteGPSInstallKey, sendmsg);
            }
            catch (Exception ex)
            {
                LoggerManager.Logger.Error(ex);
            }
        }

        public void RemoveInstallGPS(string mdvrcoresn)
        {
            try
            {
                LoggerManager.Logger.Info("RemoveInstallGPS:" + mdvrcoresn);
                byte[] sendmsg = ConvertHelper.ObjectToBytes(mdvrcoresn);

                SendMessage(Constdefine.APPEXCHANGE, UserMessageRoute.DeleteGPSInstallKey, sendmsg);
            }
            catch (Exception ex)
            {
                LoggerManager.Logger.Error(ex);
            }
        }

        public void SetAlarmParaCommand(SetAlarmPara commandInfo)
        {
            try
            {
                LoggerManager.Logger.Info("SetAlarmParaCommand");
                byte[] sendmsg = ConvertHelper.ObjectToBytes(commandInfo);

                SendMessage(Constdefine.APPEXCHANGE, InstallRoute.SetAlarmParaAppKey, sendmsg);
            }
            catch (Exception ex)
            {
                LoggerManager.Logger.Error(ex);
            }
        }

        public void OnInstallCompleteNotification(string key, byte[] bytes)
        {
            try
            {
                LoggerManager.Logger.Info("OnInstallCompleteNotification");
                Vehicle vehicle = ConvertHelper.BytesToObject(bytes) as Vehicle;

                if (DataManager.Organization.ContainsKey(vehicle.OrgnizationId))
                {
                    Dictionary<string, IMessageCallBackExt> dictionary = DataManager.Organization[vehicle.OrgnizationId];

                    foreach (var callback in dictionary.Values)
                    {
                        try
                        {
                            callback.MessageCallBack(vehicle);
                        }
                        catch (Exception ex)
                        {
                            LoggerManager.Logger.Error(ex);
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
