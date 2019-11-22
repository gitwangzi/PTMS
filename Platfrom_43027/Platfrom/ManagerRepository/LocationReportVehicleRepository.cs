using Gsafety.PTMS.Base.Contract.Data;
using Gsafety.PTMS.BaseInfo;
using Gsafety.PTMS.Common.Data;
using Gsafety.PTMS.Common.Data.Enum;
using Gsafety.PTMS.DBEntity;
using System.Collections.Generic;
using System.Linq;
namespace Gsafety.PTMS.Manager.Repository
{
    ///<summary>
    ///汇报策略车辆
    ///</summary>
    public class LocationReportVehicleRepository
    {

        static string FailedToSave = "Failed to Save to DB";
        /// <summary>
        /// 添加汇报策略车辆
        /// </summary>
        /// <param name="model">汇报策略车辆</param>
        public static SingleMessage<bool> InsertLocationReportVehicle(PTMSEntities context, List<LocationReportVehicle> models)
        {
            List<string> locationruleids = models.Select(n => n.LocationReportID).Distinct().ToList();
            List<string> temp = context.TRF_COMMAND_VEHICLE.Where(n => locationruleids.Contains(n.COMMAND_PARAM_ID)).Select(n => n.VEHICLE_ID).ToList();
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

                        LocationReportVehicleUtility.UpdateEntity(entity, item, true);

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
        /// 修改汇报策略车辆
        /// </summary>
        public static SingleMessage<bool> UpdateLocationReportVehicle(PTMSEntities context, LocationReportVehicle model)
        {
            var entity = context.TRF_COMMAND_VEHICLE.FirstOrDefault(t => t.ID == model.ID);
            if (null == entity)
            {
                return new SingleMessage<bool>(false, "");
            }

            LocationReportVehicleUtility.UpdateEntity(entity, model, false);

            return context.Save();
        }

        /// <summary>
        /// 删除汇报策略车辆
        /// </summary>
        public static SingleMessage<bool> DeleteLocationReportVehicleByID(PTMSEntities context, string ID)
        {
            //TRF_COMMAND_VEHICLE entity = context.TRF_COMMAND_VEHICLE.FirstOrDefault(t => t.ID == ID);
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
            TRF_COMMAND_VEHICLE entity = context.TRF_COMMAND_VEHICLE.FirstOrDefault(t => t.ID == ID);
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
        /// 获取汇报策略车辆
        /// </summary>
        public static SingleMessage<LocationReportVehicle> GetLocationReportVehicle(PTMSEntities context, string ID)
        {
            TRF_COMMAND_VEHICLE entity = context.TRF_COMMAND_VEHICLE.SingleOrDefault(n => n.ID == ID);
            if (entity != null)
            {
                LocationReportVehicle model = LocationReportVehicleUtility.GetModel(entity);
                return new SingleMessage<LocationReportVehicle>(model);
            }
            return null;
        }

        /// <summary>
        /// 获取汇报策略车辆
        /// </summary>
        public static MultiMessage<LocationReportVehicle> GetLocationReportVehicleList(PTMSEntities context, int pageIndex = 1, int pageSize = 10)
        {
            int totalCount;
            var list = context.TRF_COMMAND_VEHICLE.Where(n => n.TYPE == (short)CommandParaEnum.ReportStrategy).Page(out totalCount, pageIndex, pageSize, true).ToList();

            var items = list.Select(t => LocationReportVehicleUtility.GetModel(t)).ToList();

            return new MultiMessage<LocationReportVehicle>(items, totalCount);
        }


        public static MultiMessage<LocationReportVehicle> GetLocationReportVehicleListByLocationReportID(PTMSEntities context, string clientID, string locationReportID, int pageIndex, int pageSize)
        {
            int totalCount;
            var result = from lv in context.TRF_COMMAND_VEHICLE
                         join v in context.BSC_VEHICLE on
                         lv.VEHICLE_ID equals v.VEHICLE_ID
                         join vt in context.BSC_VEHICLE_TYPE on v.VEHICLE_TYPE equals vt.ID
                         join o in context.USR_ORGANIZATION on v.ORGNIZATION_ID equals o.ID
                         where lv.COMMAND_PARAM_ID == locationReportID && lv.TYPE == (short)CommandParaEnum.ReportStrategy

                         orderby lv.CREATE_TIME descending
                         select new LocationReportVehicle
                         {
                             CreateTime = lv.CREATE_TIME.Value,
                             ID = lv.ID,
                             LocationReportID = lv.COMMAND_PARAM_ID,
                             MdvrCoreSn = lv.MDVR_CORE_SN,
                             SendTime = lv.SEND_TIME,
                             Status = lv.STATUS.Value,
                             PacketSeq = (int)lv.PACKET_SEQ,
                             Creator = lv.CREATOR,
                             VehicleID = lv.VEHICLE_ID,
                             VehicleType = vt.NAME,
                             Organization = o.NAME
                         };
            List<LocationReportVehicle> list = result.Page(out totalCount, pageIndex, pageSize, true).ToList(); ;

            return new MultiMessage<LocationReportVehicle>(list, totalCount);
        }

        public static MultiMessage<LocationReportVehicle> GetAllLocationReportVehicleListByLocationReportID(PTMSEntities context, string clientID, string locationReportID)
        {
            var result = from lv in context.TRF_COMMAND_VEHICLE
                         where lv.COMMAND_PARAM_ID == locationReportID && lv.TYPE == (short)CommandParaEnum.ReportStrategy
                         select new LocationReportVehicle
                         {
                             ID = lv.ID,
                             LocationReportID = lv.COMMAND_PARAM_ID,
                             Status = lv.STATUS.Value,
                             VehicleID = lv.VEHICLE_ID,

                         };
            List<LocationReportVehicle> list = result.ToList();

            return new MultiMessage<LocationReportVehicle>(list, list.Count);
        }

        public static SingleMessage<bool> DeliverLocationReportRuleToVehicle(PTMSEntities context, List<LocationReportVehicle> vehicles)
        {
            List<string> ids = vehicles.Select(n => n.ID).ToList();
            var temp = from hv in context.TRF_COMMAND_VEHICLE
                       where ids.Contains(hv.ID) && hv.TYPE == (short)CommandParaEnum.ReportStrategy && hv.STATUS == (short)CommandStateEnum.UnDelivered
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

