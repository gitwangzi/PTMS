using Gsafety.Common.Logging;
using Gsafety.Common.Util;
using Gsafety.MQ;
using Gsafety.PTMS.Common.Data.Data.DataAccess;
using Gsafety.PTMS.Common.Data.Enum;
using Gsafety.PTMS.DBEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gsafety.PTMS.AnalysisLib.Command
{
    public class AppMessageCommand : CommandBase
    {
        public override void OnSend(DBEntity.PTMSEntities context)
        {
            var hvs = from mv in context.RUN_APPMESSAGE_VEHICLE
                      join w in context.RUN_MOBILE_WORKING on mv.VEHICLE_ID equals w.VEHICLE_ID
                      where mv.STATUS == (short)CommandStateEnum.WaitForDeliver && w.ONLINE_FLAG == 1
                      group mv by mv.VEHICLE_ID into grp
                      let createtime = grp.Min(n => n.CREATE_TIME)
                      from row in grp
                      where row.CREATE_TIME == createtime
                      select row;


            List<RUN_APPMESSAGE_VEHICLE> tosendlist = hvs.ToList();

            foreach (var item in tosendlist)
            {
                try
                {
                    AppMessage message = new AppMessage();
                    message.UID = item.MOBILE_UID;
                    message.MessageTitle = item.MESSAGE_TITLE;
                    message.MessgeType = item.MESSAGE_TYPE.ToString();
                    string json = ConvertHelper.ConvertModelToJson(message);
                    byte[] sendbytes = System.Text.UnicodeEncoding.UTF8.GetBytes(json);
                    TransforMessage.PublishMessage(Constdefine.MDVREXCHANGE, "MDVR.SendMessage." + message.UID, sendbytes);

                    item.SEND_TIME = DateTime.Now.ToUniversalTime();
                    item.STATUS = (short)CommandStateEnum.Succeed;

                    context.SaveChanges();

                }
                catch (Exception ex)
                {
                    LoggerManager.Logger.Error(ex);
                }
            }

            context.SaveChanges();
        }
    }
}
