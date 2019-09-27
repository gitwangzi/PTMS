using Gsafety.PTMS.Common.Data;
using Gsafety.PTMS.DBEntity;
using System;
using System.Collections.Generic;
namespace GSafety.PTMS.PublicService.Repository
{
    public class AppMessageVehicleUtility
    {

        public static RUN_APPMESSAGE_VEHICLE UpdateEntity(RUN_APPMESSAGE_VEHICLE entity, AppMessageVehicle model, bool isAdd)
        {
            if (isAdd)
            {
                entity.ID = model.ID;
                entity.CLIENT_ID = model.ClientID;
            }
            entity.CHAUFFEUR_ID = model.ChauffeurID;
            entity.CHAUFFEUR_NAME = model.ChauffeurName;
            entity.VEHICLE_ID = model.VehicleID;
            entity.CLIENT_ID = model.ClientID;
            entity.MESSAGE_ID = model.MessageID;
            entity.MESSAGE_TITLE = model.MessageTitle;
            entity.MESSAGE = model.Message;
            entity.MESSAGE_TYPE = model.MessageType;
            entity.SEND_TIME = model.SendTime;
            entity.STATUS = model.Status;
            return entity;
        }

        public static AppMessageVehicle GetModel(RUN_APPMESSAGE_VEHICLE entity)
        {
            AppMessageVehicle model = new AppMessageVehicle();
            model.ID = entity.ID;
            model.ChauffeurID = entity.CHAUFFEUR_ID;
            model.ChauffeurName = entity.CHAUFFEUR_NAME;
            model.VehicleID = entity.VEHICLE_ID;
            model.ClientID = entity.CLIENT_ID;
            model.MessageID = entity.MESSAGE_ID;
            model.MessageTitle = entity.MESSAGE_TITLE;
            model.Message = entity.MESSAGE;
            model.MessageType = entity.MESSAGE_TYPE.Value;
            if (entity.SEND_TIME.HasValue)
                model.SendTime = entity.SEND_TIME.Value;
            model.Status = entity.STATUS.Value;
            return model;
        }

    }
}

