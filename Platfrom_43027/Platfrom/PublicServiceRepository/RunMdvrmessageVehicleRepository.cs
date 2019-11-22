using System;
using System.Collections.Generic;
using System.Linq;
using Gsafety.PTMS.BaseInfo;
using System.ServiceModel;
using Gsafety.PTMS.DBEntity;
using Gsafety.PTMS.PublicService.Contract;
using Gsafety.PTMS.Base.Contract.Data;
using Gsafety.PTMS.Common.Data.Enum;
using Gsafety.PTMS.Common.Data;
namespace GSafety.PTMS.PublicService.Repository
{
    ///<summary>
    ///消息车辆
    ///</summary>
    public class RunMdvrmessageVehicleRepository
    {

        static string FailedToSave = "Failed to Save to DB";
        /// <summary>
        /// 添加消息车辆
        /// </summary>
        /// <param name="models">消息车辆</param>
        public static SingleMessage<bool> InsertRunMdvrmessageVehicle(PTMSEntities context, List<MdvrMessageVehicle> models)
        {
            List<string> speedids = models.Select(n => n.MessageId).Distinct().ToList();
            List<string> temp = context.RUN_MDVRMESSAGE_VEHICLE.Where(n => speedids.Contains(n.MESSAGE_ID)).Select(n => n.VEHICLE_ID).ToList();
            List<string> vehicles = models.Select(n => n.VehicleId).Distinct().ToList();
            List<RUN_SUITE_WORKING> workingsuites = context.RUN_SUITE_WORKING.Where(n => vehicles.Contains(n.VEHICLE_ID)).ToList();

            bool shouldsave = false;
            foreach (var item in models)
            {
                if (!temp.Contains(item.VehicleId))
                {
                    RUN_SUITE_WORKING workingsuite = workingsuites.FirstOrDefault(n => n.VEHICLE_ID == item.VehicleId);
                    if (workingsuite != null)
                    {
                        item.MdvrCoreSn = workingsuite.MDVR_CORE_SN;
                        var entity = new RUN_MDVRMESSAGE_VEHICLE();
                        RunMdvrmessageVehicleUtility.UpdateEntity(entity, item, true);

                        context.RUN_MDVRMESSAGE_VEHICLE.Add(entity);

                        shouldsave = true;
                    }
                }
            }

            if (!shouldsave)
            {
                return new SingleMessage<bool>(true);
            }

            return context.Save();
        }

        /// <summary>
        /// 修改消息车辆
        /// </summary>
        public static SingleMessage<bool> UpdateRunMdvrmessageVehicle(PTMSEntities context, MdvrMessageVehicle model)
        {
            var entity = context.RUN_MDVRMESSAGE_VEHICLE.FirstOrDefault(t => t.ID == model.ID);
            if (null == entity)
            {
                return new SingleMessage<bool>(false, "");
            }

            RunMdvrmessageVehicleUtility.UpdateEntity(entity, model, false);

            return context.Save();
        }

        /// <summary>
        /// 删除消息车辆
        /// </summary>
        public static SingleMessage<bool> DeleteRunMdvrmessageVehicleByID(PTMSEntities context, string ID)
        {
            RUN_MDVRMESSAGE_VEHICLE entity = context.RUN_MDVRMESSAGE_VEHICLE.FirstOrDefault(t => t.ID == ID);
            if (entity != null)
            {
                context.RUN_MDVRMESSAGE_VEHICLE.Attach(entity);
                context.RUN_MDVRMESSAGE_VEHICLE.Remove(entity);
                return context.Save();
            }
            else
            {
                return new SingleMessage<bool>(false, "");
            }
        }


        /// <summary>
        /// 获取消息车辆
        /// </summary>
        public static MultiMessage<MdvrMessageVehicle> GetRunMdvrmessageVehicleList(PTMSEntities context, string messageID, int pageIndex = 1, int pageSize = 10)
        {
            int totalCount;

            var result = from mv in context.RUN_MDVRMESSAGE_VEHICLE
                         join v in context.BSC_VEHICLE on
                         mv.VEHICLE_ID equals v.VEHICLE_ID
                         join vt in context.BSC_VEHICLE_TYPE on v.VEHICLE_TYPE equals vt.ID
                         join o in context.USR_ORGANIZATION on v.ORGNIZATION_ID equals o.ID
                         where mv.MESSAGE_ID == messageID
                         orderby mv.CREATE_TIME descending
                         select new MdvrMessageVehicle
                         {

                             ID = mv.ID,
                             MessageId = mv.MESSAGE_ID,
                             SendTime = mv.SEND_TIME,
                             Organization = o.NAME,
                             VehicleId = mv.VEHICLE_ID,
                             Status = mv.STATUS,
                             CreateTime = mv.CREATE_TIME,

                         };

            var list = result.Page(out totalCount, pageIndex, pageSize, true).ToList();

            return new MultiMessage<MdvrMessageVehicle>(list, totalCount);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="vehicles"></param>
        /// <returns></returns>
        public static SingleMessage<bool> DeliverRunMdvrmessageToVehicle(PTMSEntities context, List<MdvrMessageVehicle> vehicles)
        {
            List<string> ids = vehicles.Select(n => n.ID).ToList();
            var temp = from hv in context.RUN_MDVRMESSAGE_VEHICLE
                       where ids.Contains(hv.ID) && hv.STATUS == (short)CommandStateEnum.UnDelivered
                       select hv;
            List<RUN_MDVRMESSAGE_VEHICLE> lists = temp.ToList();
            if (lists.Count == 0)
            {
                return new SingleMessage<bool>(true);
            }
            foreach (var item in lists)
            {
                item.STATUS = (short)CommandStateEnum.WaitForDeliver;
            }

            return context.Save();
        }



        public static MultiMessage<MdvrMessageVehicle> GetAllRunMdvrmessageVehicleListBySpeedID(PTMSEntities context, string clietnID, string messageId)
        {
            var result = from vs in context.RUN_MDVRMESSAGE_VEHICLE
                         where vs.MESSAGE_ID == messageId
                         select new MdvrMessageVehicle
                         {
                             ID = vs.ID,
                             MessageId = vs.MESSAGE_ID,
                             Status = vs.STATUS,
                             VehicleId = vs.VEHICLE_ID,

                         };
            List<MdvrMessageVehicle> list = result.ToList();

            return new MultiMessage<MdvrMessageVehicle>(list, list.Count);
        }
    }
}

