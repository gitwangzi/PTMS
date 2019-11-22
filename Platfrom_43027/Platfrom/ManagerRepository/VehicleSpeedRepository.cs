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
    ///超速车辆关联表
    ///</summary>
    public class VehicleSpeedRepository
    {

        static string FailedToSave = "Failed to Save to DB";

        /// <summary>
        /// 修改超速车辆关联表
        /// </summary>
        public static SingleMessage<bool> UpdateVehicleSpeed(PTMSEntities context, VehicleSpeed model)
        {
            var entity = context.TRF_COMMAND_VEHICLE.FirstOrDefault(t => t.ID == model.ID);
            if (null == entity)
            {
                return new SingleMessage<bool>(false, "");
            }

            VehicleSpeedUtility.UpdateEntity(entity, model, false);

            return context.Save();
        }

        /// <summary>
        /// 删除超速车辆关联表
        /// </summary>
        public static SingleMessage<bool> DeleteVehicleSpeedByID(PTMSEntities context, string ID)
        {
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
        /// 获取超速车辆关联表
        /// </summary>
        public static SingleMessage<VehicleSpeed> GetVehicleSpeed(PTMSEntities context, string ID)
        {
            TRF_COMMAND_VEHICLE entity = context.TRF_COMMAND_VEHICLE.SingleOrDefault(n => n.ID == ID);
            if (entity != null)
            {
                VehicleSpeed model = VehicleSpeedUtility.GetModel(entity);
                return new SingleMessage<VehicleSpeed>(model);
            }
            return null;
        }

        /// <summary>
        /// 获取超速车辆关联表
        /// </summary>
        public static MultiMessage<VehicleSpeed> GetVehicleSpeedList(PTMSEntities context, int pageIndex = 1, int pageSize = 10)
        {
            int totalCount;
            var list = context.TRF_COMMAND_VEHICLE.Where(n => n.TYPE == (short)CommandParaEnum.Speed).Page(out totalCount, pageIndex, pageSize, true).ToList();

            var items = list.Select(t => VehicleSpeedUtility.GetModel(t)).ToList();

            return new MultiMessage<VehicleSpeed>(items, totalCount);
        }


        public static SingleMessage<bool> InsertVehicleSpeed(PTMSEntities context, List<VehicleSpeed> models)
        {
            List<string> speedids = models.Select(n => n.SpeedID).Distinct().ToList();
            List<string> temp = context.TRF_COMMAND_VEHICLE.Where(n => speedids.Contains(n.COMMAND_PARAM_ID) && n.TYPE == (short)CommandParaEnum.Speed).Select(n => n.VEHICLE_ID).ToList();
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
                        VehicleSpeedUtility.UpdateEntity(entity, item, true);

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

        public static MultiMessage<VehicleSpeed> GetAllVehicleSpeedListBySpeedID(PTMSEntities context, string clietnID, string speedID)
        {
            var result = from vs in context.TRF_COMMAND_VEHICLE
                         where vs.COMMAND_PARAM_ID == speedID && vs.TYPE == (short)CommandParaEnum.Speed
                         select new VehicleSpeed
                         {
                             ID = vs.ID,
                             SpeedID = vs.COMMAND_PARAM_ID,
                             Status = vs.STATUS.Value,
                             VehicleID = vs.VEHICLE_ID,

                         };
            List<VehicleSpeed> list = result.ToList();

            return new MultiMessage<VehicleSpeed>(list, list.Count);
        }

        public static MultiMessage<VehicleSpeed> GetVehicleSpeedListBySpeedID(PTMSEntities context, string clientID, string speedID, string vehicleName, int pageIndex, int pageSize)
        {
            int totalCount;
            List<VehicleSpeed> list = new List<VehicleSpeed>();
            if (string.IsNullOrEmpty(vehicleName))
            {
                var result = from vs in context.TRF_COMMAND_VEHICLE
                             join v in context.BSC_VEHICLE on
                             vs.VEHICLE_ID equals v.VEHICLE_ID
                             join vt in context.BSC_VEHICLE_TYPE on v.VEHICLE_TYPE equals vt.ID
                             join o in context.USR_ORGANIZATION on v.ORGNIZATION_ID equals o.ID
                             where vs.COMMAND_PARAM_ID == speedID && vs.TYPE == (short)CommandParaEnum.Speed
                             orderby vs.CREATE_TIME descending
                             select new VehicleSpeed
                             {
                                 CreateTime = vs.CREATE_TIME,
                                 ID = vs.ID,
                                 SpeedID = vs.COMMAND_PARAM_ID,
                                 MdvrCoreSn = vs.MDVR_CORE_SN,
                                 SendTime = vs.SEND_TIME,
                                 Status = vs.STATUS.Value,
                                 PacketSeq = (int)vs.PACKET_SEQ,
                                 Creator = vs.CREATOR,
                                 VehicleID = vs.VEHICLE_ID,
                                 VehicleType = vt.NAME,
                                 Organization = o.NAME
                             };
                list = result.Page(out totalCount, pageIndex, pageSize, true).ToList();
            }
            else
            {
                string vehicle = vehicleName.ToUpper();
                var result = from vs in context.TRF_COMMAND_VEHICLE
                             join v in context.BSC_VEHICLE on
                             vs.VEHICLE_ID equals v.VEHICLE_ID
                             join vt in context.BSC_VEHICLE_TYPE on v.VEHICLE_TYPE equals vt.ID
                             join o in context.USR_ORGANIZATION on v.ORGNIZATION_ID equals o.ID
                             where vs.COMMAND_PARAM_ID == speedID && vs.TYPE == (short)CommandParaEnum.Speed
                             && vs.VEHICLE_ID.ToUpper().Contains(vehicle)
                             orderby vs.CREATE_TIME descending
                             select new VehicleSpeed
                             {
                                 CreateTime = vs.CREATE_TIME,
                                 ID = vs.ID,
                                 SpeedID = vs.COMMAND_PARAM_ID,
                                 MdvrCoreSn = vs.MDVR_CORE_SN,
                                 SendTime = vs.SEND_TIME,
                                 Status = vs.STATUS.Value,
                                 PacketSeq = (int)vs.PACKET_SEQ,
                                 Creator = vs.CREATOR,
                                 VehicleID = vs.VEHICLE_ID,
                                 VehicleType = vt.NAME,
                                 Organization = o.NAME
                             };
                list = result.Page(out totalCount, pageIndex, pageSize, true).ToList();
            }

            return new MultiMessage<VehicleSpeed>(list, totalCount);
        }

        public static SingleMessage<bool> DeliverSpeedLimitToVehicle(PTMSEntities context, List<VehicleSpeed> vehicles)
        {
            List<string> ids = vehicles.Select(n => n.ID).ToList();
            var temp = from hv in context.TRF_COMMAND_VEHICLE
                       where ids.Contains(hv.ID) && hv.STATUS == (short)CommandStateEnum.UnDelivered
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
    }
}

