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
    ///视频车辆关联表
    ///</summary>
    public class VideoRuleVehicleRepository
    {

        static string FailedToSave = "Failed to Save to DB";
        /// <summary>
        /// 添加视频车辆关联表
        /// </summary>
        /// <param name="model">视频车辆关联表</param>
        public static SingleMessage<bool> InsertVideoRuleVehicle(PTMSEntities context, List<VideoRuleVehicle> models)
        {
            List<string> videoruleids = models.Select(n => n.VideoRuleID).Distinct().ToList();
            List<string> temp = context.TRF_COMMAND_VEHICLE.Where(n => videoruleids.Contains(n.COMMAND_PARAM_ID) && n.TYPE == (short)CommandParaEnum.LED).Select(n => n.VEHICLE_ID).ToList();
            List<string> vehicles = models.Select(n => n.VehicleID).Distinct().ToList();
            List<RUN_SUITE_WORKING> workingsuites = context.RUN_SUITE_WORKING.Where(n => vehicles.Contains(n.VEHICLE_ID)).ToList();


            foreach (var item in models)
            {
                if (!temp.Contains(item.VehicleID))
                {
                    RUN_SUITE_WORKING workingsuite = workingsuites.FirstOrDefault(n => n.VEHICLE_ID == item.VehicleID);
                    if (workingsuite != null)
                    {
                        item.MdvrCoreSn = workingsuite.MDVR_CORE_SN;
                        var entity = new TRF_COMMAND_VEHICLE();
                        VideoRuleVehicleUtility.UpdateEntity(entity, item, true);

                        context.TRF_COMMAND_VEHICLE.Add(entity);
                    }
                }
            }

            return context.Save();
        }

        /// <summary>
        /// 修改视频车辆关联表
        /// </summary>
        public static SingleMessage<bool> UpdateVideoRuleVehicle(PTMSEntities context, VideoRuleVehicle model)
        {
            var entity = context.TRF_COMMAND_VEHICLE.FirstOrDefault(t => t.ID == model.ID);
            if (null == entity)
            {
                return new SingleMessage<bool>(false, "");
            }

            VideoRuleVehicleUtility.UpdateEntity(entity, model, false);

            return context.Save();
        }

        /// <summary>
        /// 删除视频车辆关联表
        /// </summary>
        public static SingleMessage<bool> DeleteVideoRuleVehicleByID(PTMSEntities context, string ID)
        {
            TRF_COMMAND_VEHICLE entity = context.TRF_COMMAND_VEHICLE.FirstOrDefault(t => t.ID == ID);
            if (entity != null)
            {
                context.TRF_COMMAND_VEHICLE.Attach(entity);
                context.TRF_COMMAND_VEHICLE.Remove(entity);
                return context.Save();
            }
            else
            {
                return new SingleMessage<bool>(false, "");
            }
        }

        /// <summary>
        /// 获取视频车辆关联表
        /// </summary>
        public static SingleMessage<VideoRuleVehicle> GetVideoRuleVehicle(PTMSEntities context, string ID)
        {
            TRF_COMMAND_VEHICLE entity = context.TRF_COMMAND_VEHICLE.SingleOrDefault(n => n.ID == ID);
            if (entity != null)
            {
                VideoRuleVehicle model = VideoRuleVehicleUtility.GetModel(entity);
                return new SingleMessage<VideoRuleVehicle>(model);
            }
            return null;
        }

        /// <summary>
        /// 获取视频车辆关联表
        /// </summary>
        public static MultiMessage<VideoRuleVehicle> GetVideoRuleVehicleList(PTMSEntities context, int pageIndex = 1, int pageSize = 10)
        {
            int totalCount;
            var list = context.TRF_COMMAND_VEHICLE.Where(n => n.TYPE == (short)CommandParaEnum.LED).Page(out totalCount, pageIndex, pageSize, true).ToList();

            var items = list.Select(t => VideoRuleVehicleUtility.GetModel(t)).ToList();

            return new MultiMessage<VideoRuleVehicle>(items, totalCount);
        }


        public static MultiMessage<VideoRuleVehicle> GetVideoRuleVehicleListByVideoRuleID(PTMSEntities context, string clientID, string videoRuleID, int pageIndex, int pageSize)
        {
            int totalCount;
            var result = from vv in context.TRF_COMMAND_VEHICLE
                         join v in context.BSC_VEHICLE on
                         vv.VEHICLE_ID equals v.VEHICLE_ID
                         join vt in context.BSC_VEHICLE_TYPE on v.VEHICLE_TYPE equals vt.ID
                         join o in context.USR_ORGANIZATION on v.ORGNIZATION_ID equals o.ID
                         where vv.COMMAND_PARAM_ID == videoRuleID && vv.TYPE == (short)CommandParaEnum.LED
                         orderby vv.CREATE_TIME descending
                         select new VideoRuleVehicle
                         {
                             CreateTime = vv.CREATE_TIME.Value,
                             ID = vv.ID,
                             VideoRuleID = vv.COMMAND_PARAM_ID,
                             MdvrCoreSn = vv.MDVR_CORE_SN,
                             SendTime = vv.SEND_TIME.Value,
                             Status = vv.STATUS.Value,
                             PacketSeq = (int)vv.PACKET_SEQ,
                             Creator = vv.CREATOR,
                             VehicleID = vv.VEHICLE_ID,
                             VehicleType = vt.NAME,
                             Organization = o.NAME
                         };
            List<VideoRuleVehicle> list = result.Page(out totalCount, pageIndex, pageSize, true).ToList(); ;

            return new MultiMessage<VideoRuleVehicle>(list, totalCount);
        }

        public static MultiMessage<VideoRuleVehicle> GetAllVideoRuleVehicleListByVideoRuleID(PTMSEntities context, string clientID, string videoRuleID)
        {
            var result = from vv in context.TRF_COMMAND_VEHICLE
                         where vv.COMMAND_PARAM_ID == videoRuleID
                         select new VideoRuleVehicle
                         {
                             ID = vv.ID,
                             VideoRuleID = vv.COMMAND_PARAM_ID,
                             Status = vv.STATUS.Value,
                             VehicleID = vv.VEHICLE_ID,

                         };
            List<VideoRuleVehicle> list = result.ToList();

            return new MultiMessage<VideoRuleVehicle>(list, list.Count);
        }

        public static SingleMessage<bool> DeliverVideoRuleToVehicle(PTMSEntities context, List<VideoRuleVehicle> vehicles)
        {
            List<string> ids = vehicles.Select(n => n.ID).ToList();
            var temp = from hv in context.TRF_COMMAND_VEHICLE
                       where ids.Contains(hv.ID) && hv.TYPE == (short)CommandParaEnum.LED
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

