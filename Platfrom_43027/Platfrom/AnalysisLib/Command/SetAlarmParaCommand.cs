using Gsafety.PTMS.Common.Data.Enum;
using Gsafety.PTMS.DBEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gsafety.PTMS.Common.Data;
using Gsafety.Common.Util;
using Gsafety.MQ;
using Gsafety.Common.Logging;
using Gsafety.PTMS.Analysis.Helper;
using System.Reflection;

namespace Gsafety.PTMS.AnalysisLib.Command
{
    public class SetAlarmParaCommand : CommandBase
    {
        public static void Send(PTMSEntities context, byte[] bytes, string key)
        {
            try
            {
                var model = ConvertHelper.BytesToObject(bytes) as SetAlarmPara;

                var param = new SetAlarmParaJson()
                {
                    UID = model.MDVRID,
                    ChannelList = model.ChannelList,
                    AlarmBeforeTime = model.AlarmBeforeTime,
                    AlarmEndTime = model.AlarmEndTime,
                    RelatedData = model.RelatedData
                    //ChannelList = new List<int>() { 0, 1, 2, 3 },
                    //AlarmBeforeTime = 10,
                    //AlarmEndTime = 5,
                    //RelatedData = 1
                };

                var json = ConvertHelper.ConvertModelToJson(param);
                var sendbytes = System.Text.UnicodeEncoding.UTF8.GetBytes(json);
                TransforMessage.PublishMessage(Constdefine.MDVREXCHANGE, InstallRoute.SetAlarmParaMDVRKey + param.UID, sendbytes);

                var old = context.MTN_VIDEO_UPLOAD_COMMAND.FirstOrDefault(t => t.INSTALLATION_DETAIL_ID == model.InstallationDetailID);
                var time = ConvertHelper.DateTimeNow();

                if (old == null)
                {
                    var command = new MTN_VIDEO_UPLOAD_COMMAND()
                    {
                        CREATE_TIME = time,
                        ID = model.CommandID,
                        MDVR_CORE_SN = model.MDVRID,
                        STATUS = (short)CommandStateEnum.Delivering,
                        SEND_TIME = time,
                        PACKAGE_SEQ = param.SerialNo,
                        INSTALLATION_DETAIL_ID = model.InstallationDetailID
                    };

                    context.MTN_VIDEO_UPLOAD_COMMAND.Add(command);
                }
                else
                {
                    old.CREATE_TIME = time;
                    old.SEND_TIME = time;
                    old.PACKAGE_SEQ = param.SerialNo;
                    old.STATUS = (short)CommandStateEnum.Delivering;
                }

                context.SaveChanges();
            }
            catch (System.Exception ex)
            {
                LoggerManager.Logger.Error(ex);
            }
        }

        public static void OnReply(PTMSEntities context, byte[] bytes, string key)
        {
            var json = UnicodeEncoding.UTF8.GetString(bytes);
            var responseModel = ConvertHelper.ConvertJsonToModel<SetAlarmParaResponseJson>(json);
            
            var targetQuery = context.MTN_VIDEO_UPLOAD_COMMAND.Where(t => t.MDVR_CORE_SN == responseModel.UID).OrderByDescending(t => t.SEND_TIME).FirstOrDefault();
            if (targetQuery == null)
            {
                LoggerManager.Logger.Error(MethodBase.GetCurrentMethod().ToString() + " : Get MTN_VIDEO_UPLOAD_COMMAND Error");
                return;
            }

            targetQuery.STATUS = (short)CommandStateEnum.Succeed;

            //var currentTime = ConvertHelper.DateTimeNow().AddDays(-1);
            //var list = context.MTN_VIDEO_UPLOAD_COMMAND.Where(t => t.SEND_TIME.HasValue && t.SEND_TIME < currentTime).ToList();
            //foreach (var item in list)
            //{
            //    context.MTN_VIDEO_UPLOAD_COMMAND.Remove(item);
            //}

            context.SaveChanges();
        }
    }
}
