using Gsafety.Common.Logging;
using Gsafety.Common.Util;
using Gsafety.MQ;
using Gsafety.PTMS.Common.Data;
using Gsafety.PTMS.Common.Data.Enum;
using Gsafety.PTMS.DBEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gsafety.PTMS.AnalysisLib.Command
{
    public class MDVRTextCommand : CommandBase
    {
        public override void Send(DBEntity.PTMSEntities context)
        {
            var sending = from mv in context.RUN_MDVRMESSAGE_VEHICLE
                          where mv.STATUS == (short)CommandStateEnum.Delivering
                          select mv.ID;
            List<string> sendinglist = sending.ToList();

            var hvs = from mv in context.RUN_MDVRMESSAGE_VEHICLE
                      join w in context.RUN_SUITE_WORKING on mv.VEHICLE_ID equals w.VEHICLE_ID
                      where (mv.STATUS == (short)CommandStateEnum.WaitForDeliver || mv.STATUS == (short)CommandStateEnum.Failed) && w.ONLINE_FLAG == 1
                      group mv by mv.VEHICLE_ID into grp
                      let createtime = grp.Min(n => n.CREATE_TIME)
                      from row in grp
                      where row.CREATE_TIME == createtime
                      select row;


            List<RUN_MDVRMESSAGE_VEHICLE> tosendlist = hvs.ToList();

            foreach (var item in tosendlist)
            {
                if (CommandBase.ShouldRun())
                {
                    try
                    {
                        MDVRMessage message = new MDVRMessage();
                        message.UID = item.MDVR_CORE_SN;
                        message.TextContent = item.CONTENT;
                        message.TextFlag = new List<int>();
                        message.TextFlag.Add(4);
                        message.TextFlag.Add(3);
                        string json = ConvertHelper.ConvertModelToJson(message);
                        byte[] sendbytes = System.Text.UnicodeEncoding.UTF8.GetBytes(json);
                        TransforMessage.PublishMessage(Constdefine.MDVREXCHANGE, "MDVR.SendText." + message.UID, sendbytes);

                        item.PACKET_SEQ = message.SerialNo;
                        item.SEND_TIME = DateTime.Now.ToUniversalTime();
                        item.STATUS = (short)CommandStateEnum.Delivering;

                        context.SaveChanges();

                    }
                    catch (Exception ex)
                    {
                        LoggerManager.Logger.Error(ex);
                    }
                }
            }

            var templist = from fq in context.RUN_MDVRMESSAGE_VEHICLE
                           where fq.STATUS == (short)CommandStateEnum.Delivering && sendinglist.Contains(fq.ID)
                           select fq;

            foreach (var item in templist.ToList())
            {
                item.STATUS = (short)CommandStateEnum.Failed;
            }

            context.SaveChanges();
        }

        public override void OnReply(DBEntity.PTMSEntities context, byte[] bytes, string key)
        {
            string str = System.Text.UnicodeEncoding.UTF8.GetString(bytes);
            GenenalResponseEx response = JsonHelper.FromJsonString<GenenalResponseEx>(str);
            RUN_MDVRMESSAGE_VEHICLE record = context.RUN_MDVRMESSAGE_VEHICLE.Where(n => n.PACKET_SEQ == response.ResponseSerialNo && n.MDVR_CORE_SN == response.UID).OrderBy(n => n.CREATE_TIME).FirstOrDefault();
            if (record != null)
            {
                if (response.Result == 0)
                {
                    //update the record status
                    record.STATUS = (short)CommandStateEnum.Succeed;
                }
                else
                {
                    //update the record status
                    record.STATUS = (short)CommandStateEnum.Failed;
                }

                context.SaveChanges();
            }
        }
    }
}
