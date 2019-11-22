using Gsafety.PTMS.Common.Data;
using Gsafety.PTMS.Common.Data.Enum;
using Gsafety.PTMS.DBEntity;
using System;
using System.Collections.Generic;
namespace Gsafety.PTMS.Manager.Repository
{
    public class VehicleSpeedUtility
    {
        public static TRF_COMMAND_VEHICLE UpdateEntity(TRF_COMMAND_VEHICLE entity, VehicleSpeed model, bool isAdd)
        {
            if (isAdd)
            {
                entity.ID = model.ID;
            }
            entity.COMMAND_PARAM_ID = model.SpeedID;
            entity.MDVR_CORE_SN = model.MdvrCoreSn;
            entity.STATUS = (short)model.Status;
            entity.PACKET_SEQ = (int)model.PacketSeq;
            entity.CREATOR = model.Creator;
            entity.VEHICLE_ID = model.VehicleID;
            entity.CREATE_TIME = model.CreateTime;
            entity.TYPE = (short)CommandParaEnum.Speed;
            return entity;
        }

        public static VehicleSpeed GetModel(TRF_COMMAND_VEHICLE entity)
        {
            VehicleSpeed model = new VehicleSpeed();
            model.ID = entity.ID;
            model.SpeedID = entity.COMMAND_PARAM_ID;
            model.MdvrCoreSn = entity.MDVR_CORE_SN;
            if (entity.STATUS.HasValue)
                model.Status = entity.STATUS.Value;
            if (entity.PACKET_SEQ.HasValue)
                model.PacketSeq = entity.PACKET_SEQ.Value;
            model.Creator = entity.CREATOR;
            model.VehicleID = entity.VEHICLE_ID;
            model.CreateTime = entity.CREATE_TIME.Value;
            return model;
        }

    }
}

