using Gsafety.PTMS.Common.Data;
using Gsafety.PTMS.Common.Data.Enum;
using Gsafety.PTMS.DBEntity;
using System;
using System.Collections.Generic;
namespace Gsafety.PTMS.Manager.Repository
{
    public class VideoRuleVehicleUtility
    {
        public static TRF_COMMAND_VEHICLE UpdateEntity(TRF_COMMAND_VEHICLE entity, VideoRuleVehicle model, bool isAdd)
        {
            if (isAdd)
            {
                entity.ID = model.ID;
                entity.CREATE_TIME = model.CreateTime;
            }
            entity.COMMAND_PARAM_ID = model.VideoRuleID;
            entity.MDVR_CORE_SN = model.MdvrCoreSn;
            entity.SEND_TIME = model.SendTime;
            entity.STATUS = (short)model.Status;
            entity.PACKET_SEQ = (int)model.PacketSeq;
            entity.CREATOR = model.Creator;
            entity.VEHICLE_ID = model.VehicleID;
            entity.TYPE = (short)CommandParaEnum.LED;
            return entity;
        }

        public static VideoRuleVehicle GetModel(TRF_COMMAND_VEHICLE entity)
        {
            VideoRuleVehicle model = new VideoRuleVehicle();
            model.ID = entity.ID;
            model.MdvrCoreSn = entity.MDVR_CORE_SN;
            if (entity.SEND_TIME.HasValue)
                model.SendTime = entity.SEND_TIME.Value;
            model.Creator = entity.CREATOR;
            model.VehicleID = entity.VEHICLE_ID;
            model.ID = entity.ID;

            if (entity.CREATE_TIME.HasValue)
                model.SendTime = entity.CREATE_TIME.Value;
            if (entity.PACKET_SEQ.HasValue)
                model.PacketSeq = entity.PACKET_SEQ.Value;
            if (entity.STATUS.HasValue)
                model.Status = entity.STATUS.Value;


            return model;
        }

    }
}

