using Gsafety.PTMS.Common.Data;
using Gsafety.PTMS.Common.Data.Enum;
using Gsafety.PTMS.DBEntity;
using System;
using System.Collections.Generic;

namespace Gsafety.PTMS.Manager.Repository
{
    public class HeartbeatVehicleUtility
    {
        public static TRF_COMMAND_VEHICLE UpdateEntity(TRF_COMMAND_VEHICLE entity, HeartbeatVehicle model, bool isAdd)
        {
            if (isAdd)
            {
                entity.ID = model.ID;
            }
            entity.COMMAND_PARAM_ID = model.HeartbeatID;
            entity.MDVR_CORE_SN = model.MdvrCoreSn;
            entity.SEND_TIME = model.SendTime;
            entity.STATUS = (short)model.Status;
            entity.PACKET_SEQ = (int)model.PacketSeq;
            entity.CREATOR = model.Creator;
            entity.VEHICLE_ID = model.VehicleID;
            entity.CREATE_TIME = model.CreateTime;
            entity.TYPE = (short)CommandParaEnum.HeartBeat;
            return entity;
        }

        public static HeartbeatVehicle GetModel(TRF_COMMAND_VEHICLE entity)
        {
            HeartbeatVehicle model = new HeartbeatVehicle();
            model.ID = entity.ID;
            model.HeartbeatID = entity.COMMAND_PARAM_ID;
            model.MdvrCoreSn = entity.MDVR_CORE_SN;
            if (entity.SEND_TIME.HasValue)
                model.SendTime = entity.SEND_TIME.Value;
            model.Status = (short)entity.STATUS;
            model.PacketSeq = (int)entity.PACKET_SEQ;
            model.Creator = entity.CREATOR;
            model.VehicleID = entity.VEHICLE_ID;
            if (entity.CREATE_TIME.HasValue)
                model.CreateTime = entity.CREATE_TIME.Value;

            return model;
        }

    }
}

