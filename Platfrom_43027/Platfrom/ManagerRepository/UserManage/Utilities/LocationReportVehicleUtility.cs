using Gsafety.PTMS.Common.Data;
using Gsafety.PTMS.Common.Data.Enum;
using Gsafety.PTMS.DBEntity;
using System;
using System.Collections.Generic;
namespace Gsafety.PTMS.Manager.Repository
{
    public class LocationReportVehicleUtility
    {
        public static TRF_COMMAND_VEHICLE UpdateEntity(TRF_COMMAND_VEHICLE entity, LocationReportVehicle model, bool isAdd)
        {
            if (isAdd)
            {
                entity.ID = model.ID;
                entity.CREATE_TIME = model.CreateTime;
            }
            entity.COMMAND_PARAM_ID = model.LocationReportID;
            entity.VEHICLE_ID = model.VehicleID;
            entity.MDVR_CORE_SN = model.MdvrCoreSn;
            if (model.SendTime.HasValue)
                entity.SEND_TIME = model.SendTime.Value;
            entity.STATUS = (short)model.Status;
            entity.PACKET_SEQ = (int)model.PacketSeq;
            entity.CREATOR = model.Creator;
            entity.TYPE = (short)CommandParaEnum.ReportStrategy;

            return entity;
        }

        public static LocationReportVehicle GetModel(TRF_COMMAND_VEHICLE entity)
        {
            LocationReportVehicle model = new LocationReportVehicle();
            model.ID = entity.ID;
            model.LocationReportID = entity.COMMAND_PARAM_ID;
            model.VehicleID = entity.VEHICLE_ID;
            model.MdvrCoreSn = entity.MDVR_CORE_SN;
            model.SendTime = entity.SEND_TIME;
            model.Status = entity.STATUS.Value;
            if (entity.PACKET_SEQ.HasValue)
                model.PacketSeq = entity.PACKET_SEQ.Value;
            model.Creator = entity.CREATOR;
            if (entity.CREATE_TIME.HasValue)
                model.CreateTime = entity.CREATE_TIME.Value;

            return model;
        }

    }
}

