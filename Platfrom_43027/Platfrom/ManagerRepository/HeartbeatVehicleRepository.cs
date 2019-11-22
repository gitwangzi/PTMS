using System;
using System.Collections.Generic;
using System.Linq;
using Gsafety.PTMS.BaseInfo;
using Gsafety.PTMS.Base.Contract.Data;
using Gsafety.PTMS.DBEntity;
using Gsafety.PTMS.Common.Data;
using Gsafety.PTMS.Common.Data.Enum;

namespace Gsafety.PTMS.Manager.Repository
{
    ///<summary>
    ///心跳规则车辆
    ///</summary>
    public class HeartbeatVehicleRepository
    {

        static string FailedToSave = "Failed to Save to DB";
        /// <summary>
        /// 添加心跳规则车辆
        /// </summary>
        /// <param name="model">心跳规则车辆</param>
        public static SingleMessage<bool> InsertHeartbeatVehicle(PTMSEntities context, List<HeartbeatVehicle> models)
        {
            List<string> heartbeatids = models.Select(n => n.HeartbeatID).Distinct().ToList();
            List<string> temp = context.TRF_COMMAND_VEHICLE.Where(n => heartbeatids.Contains(n.COMMAND_PARAM_ID) && n.TYPE == (short)CommandParaEnum.HeartBeat).Select(n => n.VEHICLE_ID).ToList();
            List<string> vehicles = models.Select(n => n.VehicleID).Distinct().ToList();
            List<RUN_SUITE_WORKING> workingsuites = context.RUN_SUITE_WORKING.Where(n => vehicles.Contains(n.VEHICLE_ID)).ToList();

            bool shouldsave = false;
            foreach (var item in models)
            {
                if (!temp.Contains(item.VehicleID))
                {
                    RUN_SUITE_WORKING workingsuite = workingsuites.FirstOrDefault(n => n.VEHICLE_ID == item.VehicleID);
                    if (workingsuite != null)
                    {
                        item.MdvrCoreSn = workingsuite.MDVR_CORE_SN;
                        var entity = new TRF_COMMAND_VEHICLE();
                        HeartbeatVehicleUtility.UpdateEntity(entity, item, true);

                        context.TRF_COMMAND_VEHICLE.Add(entity);
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
        /// 删除心跳规则车辆
        /// </summary>
        public static SingleMessage<bool> DeleteHeartbeatVehicleByID(PTMSEntities context, string ID)
        {
            //bool referenced = context.TRF_COMMAND_VEHICLE.Any(n => n.COMMAND_PARAM_ID == ID);
            //if (referenced)
            //{
            //    return new SingleMessage<bool>(false, "Referenced");
            //}
            TRF_COMMAND_VEHICLE entity = context.TRF_COMMAND_VEHICLE.FirstOrDefault(t => t.ID == ID);
            //if (entity != null && entity.STATUS == (short)CommandStateEnum.UnDelivered || entity.STATUS == (short)CommandStateEnum.WaitForDeliver || entity.STATUS == (short)CommandStateEnum.Failed || entity.STATUS == (short)CommandStateEnum.Succeed)
            //{
            //    context.TRF_COMMAND_VEHICLE.Attach(entity);
            //    context.TRF_COMMAND_VEHICLE.Remove(entity);
            //    return context.Save();
            //}
            //else
            //{
            //    return new SingleMessage<bool>(false, "");
            //}

            if (entity != null)
            {
                if (entity.STATUS == (short)CommandStateEnum.UnDelivered || entity.STATUS == (short)CommandStateEnum.Failed || entity.STATUS == (short)CommandStateEnum.Succeed || entity.STATUS == (short)CommandStateEnum.WaitForDeliver)
                {
                    context.TRF_COMMAND_VEHICLE.Attach(entity);
                    context.TRF_COMMAND_VEHICLE.Remove(entity);
                    return context.Save();
                }
                else
                {
                    return new SingleMessage<bool>(false, "OperatorServiceError");
                }
            }
            else
            {
                return new SingleMessage<bool>(false, "OperatorServiceError");
            }
        }

        /// <summary>
        /// 获取心跳规则车辆
        /// </summary>
        public static MultiMessage<HeartbeatVehicle> GetHeartbeatVehicleList(PTMSEntities context, string clientID, string heartBeatRuleID, int pageIndex = 1, int pageSize = 10)
        {
            int totalCount;
            var result = from hv in context.TRF_COMMAND_VEHICLE
                         join v in context.BSC_VEHICLE on
                         hv.VEHICLE_ID equals v.VEHICLE_ID
                         join vt in context.BSC_VEHICLE_TYPE on v.VEHICLE_TYPE equals vt.ID
                         join o in context.USR_ORGANIZATION on v.ORGNIZATION_ID equals o.ID
                         where hv.COMMAND_PARAM_ID == heartBeatRuleID
                         orderby hv.CREATE_TIME descending
                         select new HeartbeatVehicle
                         {
                             CreateTime = hv.CREATE_TIME.Value,
                             ID = hv.ID,
                             HeartbeatID = hv.COMMAND_PARAM_ID,
                             MdvrCoreSn = hv.MDVR_CORE_SN,
                             SendTime = hv.SEND_TIME,
                             Status = hv.STATUS.Value,
                             PacketSeq = (int)hv.PACKET_SEQ,
                             Creator = hv.CREATOR,
                             VehicleID = hv.VEHICLE_ID,
                             VehicleType = vt.NAME,
                             Organization = o.NAME
                         };
            List<HeartbeatVehicle> list = result.Page(out totalCount, pageIndex, pageSize, true, n => n.CreateTime, false).ToList(); ;

            return new MultiMessage<HeartbeatVehicle>(list, totalCount);
        }


        public static SingleMessage<bool> DeliverHeartBeatRuleToVehicle(PTMSEntities context, List<HeartbeatVehicle> vehicles)
        {
            List<string> ids = vehicles.Select(n => n.ID).ToList();
            var temp = from hv in context.TRF_COMMAND_VEHICLE
                       where ids.Contains(hv.ID) && hv.TYPE == (short)CommandParaEnum.HeartBeat && hv.STATUS == (short)CommandStateEnum.UnDelivered
                       select hv;
            List<TRF_COMMAND_VEHICLE> lists = temp.ToList();

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

        public static MultiMessage<HeartbeatVehicle> GetAllHeartbeatVehicleListByHeartBeatID(PTMSEntities context, string clientID, string heartBeatRuleID)
        {
            var result = from hv in context.TRF_COMMAND_VEHICLE
                         where hv.COMMAND_PARAM_ID == heartBeatRuleID && hv.TYPE == (short)CommandParaEnum.HeartBeat
                         select new HeartbeatVehicle
                         {
                             ID = hv.ID,
                             HeartbeatID = hv.COMMAND_PARAM_ID,
                             Status = hv.STATUS.Value,
                             VehicleID = hv.VEHICLE_ID,

                         };
            List<HeartbeatVehicle> list = result.ToList();

            return new MultiMessage<HeartbeatVehicle>(list, list.Count);
        }
    }
}

