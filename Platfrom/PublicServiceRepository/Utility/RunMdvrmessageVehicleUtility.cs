using Gsafety.PTMS.Common.Data;
using Gsafety.PTMS.DBEntity;
using Gsafety.PTMS.PublicService.Contract;
using System;
using System.Collections.Generic;
namespace GSafety.PTMS.PublicService.Repository
{
    public class RunMdvrmessageVehicleUtility
    {

        public static RUN_MDVRMESSAGE_VEHICLE UpdateEntity(RUN_MDVRMESSAGE_VEHICLE entity, MdvrMessageVehicle model, bool isAdd)
        {
            if (isAdd)
            {
                entity.ID = Guid.NewGuid().ToString();
            }
            entity.MESSAGE_ID = model.MessageId;
            entity.SEND_TIME = model.SendTime;
            entity.VEHICLE_ID = model.VehicleId;
            entity.STATUS = (short)model.Status;
            entity.CREATE_TIME = model.CreateTime;
            entity.CONTENT = model.Content;
            entity.MESSAGE_TYPE = (short)model.MessageType;
            entity.MDVR_CORE_SN = model.MdvrCoreSn;

            return entity;
        }

        public static MdvrMessageVehicle GetModel(RUN_MDVRMESSAGE_VEHICLE entity)
        {
            MdvrMessageVehicle model = new MdvrMessageVehicle();
            model.ID = entity.ID;
            model.MessageId = entity.MESSAGE_ID;
            model.SendTime = entity.SEND_TIME;
            model.VehicleId = entity.VEHICLE_ID;
            model.Status = entity.STATUS;
            model.CreateTime = entity.CREATE_TIME;

            model.Content = entity.CONTENT;
            if (entity.MESSAGE_TYPE.HasValue)
                model.MessageType = entity.MESSAGE_TYPE.Value;
            model.MdvrCoreSn = entity.MDVR_CORE_SN;
            return model;
        }
    }
}

