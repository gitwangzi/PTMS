using Gsafety.Common.Logging;
using Gsafety.Common.Util;
using Gsafety.PTMS.Analysis.Helper;
using Gsafety.PTMS.Common.Data;
using Gsafety.PTMS.Message.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using LogisticsWebApi.MQHelper;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace LogisticsWebApi.MQHelper
{
    public class InstallMessage
    {


        public static void CompleteInstallSuite(string mdvrcoresn, string organization, string vehicle)
        {
            try
            {
                LoggerManager.Logger.Info("CompleteInstallSuite:" + mdvrcoresn);
                byte[] sendmsg = ConvertHelper.ObjectToBytes(mdvrcoresn);

                SendMessage("APP_EXCHANGE", UserMessageRoute.CompleteSuiteInstallKey, sendmsg);
            }
            catch (Exception ex)
            {
                LoggerManager.Logger.Error(ex);
            }
        }

        public static void SendMessage(string exchange, string route, byte[] msg)
        {

            try
            {

                using (var conn = new MQFactory().CreateConnection())
                {
                    using (IModel ch = conn.CreateModel())
                    {

                        IBasicProperties bp = ch.CreateBasicProperties();
                        bp.Type = "1";
                        bp.Priority = 1;
                        ch.BasicPublish(exchange, route, bp, msg);
                        LoggerManager.Logger.Info("send message:" + System.Text.ASCIIEncoding.ASCII.GetString(msg));
                    }
                }
            }
            catch (Exception ex)
            {
                LoggerManager.Logger.Error("SendMessage", ex);
            }
        }


        public static void RemoveInstallSuite(string mdvrcoresn)
        {
            try
            {
                LoggerManager.Logger.Info("RemoveInstallSuite:" + mdvrcoresn);
                byte[] sendmsg = ConvertHelper.ObjectToBytes(mdvrcoresn);

                SendMessage("APP_EXCHANGE", UserMessageRoute.DeleteSuiteInstallKey, sendmsg);
            }
            catch (Exception ex)
            {
                LoggerManager.Logger.Error(ex);
            }
        }

        public static void SetAlarmPara(string mdvrcoresn)
        {
            try
            {
                LoggerManager.Logger.Info("SetAlarmParaCommand");              
                var param = new SetAlarmParaJson()
                {
                    UID = mdvrcoresn,
                    ChannelList = new List<int>() { 0, 1, 2, 3 },
                    AlarmBeforeTime = 10,
                    AlarmEndTime = 10,
                    RelatedData = 2                  
                };

                var json = ConvertHelper.ConvertModelToJson(param);
                var sendbytes = System.Text.UnicodeEncoding.UTF8.GetBytes(json);

                SendMessage("MDVR_EXCHANGE", "MDVR.SetAlarmPara." + mdvrcoresn, sendbytes);
            }
            catch (Exception ex)
            {
                LoggerManager.Logger.Error(ex);
            }
        }

    }
}
