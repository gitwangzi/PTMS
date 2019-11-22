using Gsafety.PTMS.Common.Data;
using Gsafety.PTMS.DBEntity;
using Gsafety.PTMS.PublicService.Contract;
using System;
using System.Collections.Generic;
namespace GSafety.PTMS.PublicService.Repository
{
    public class FoundRegistryUtility
    {

        public static RUN_FOUND_REGISTRY UpdateEntity(RUN_FOUND_REGISTRY entity, FoundRegistry model, bool isAdd)
        {
            if (isAdd)
            {

                entity.ID = model.ID;
                entity.CLIENT_ID = model.ClientID;

            }
            entity.CLIENT_ID = model.ClientID;
            entity.FOUNDER = model.Founder;
            entity.FOUNDER_ID_CARD = model.FounderIDCard;
            entity.FOUND_PHONE = model.FoundPhone;
            entity.FOUND_TIME = model.FoundTime;
            entity.CONTENT = model.Content;
            entity.KEYWORD = model.Keyword;
            entity.LOST_NAME = model.LostName;
            entity.LOST_PHONE = model.LostPhone;
            entity.ADDRESS = model.Address;
            entity.STATUS = (short)model.Status;
            entity.CREATE_TIME = model.CreateTime;
            if (model.ClaimTime.HasValue)
                entity.CLAIM_TIME = model.ClaimTime;
            entity.VEHICLE_ID = model.VehicleID;
            return entity;
        }

        public static FoundRegistry GetModel(RUN_FOUND_REGISTRY entity)
        {
            FoundRegistry model = new FoundRegistry();
            model.ID = entity.ID;
            model.ClientID = entity.CLIENT_ID;
            model.Founder = entity.FOUNDER;
            model.FounderIDCard = entity.FOUNDER_ID_CARD;
            model.FoundPhone = entity.FOUND_PHONE;
            model.FoundTime = entity.FOUND_TIME;
            model.Content = entity.CONTENT;
            model.Keyword = entity.KEYWORD;
            model.LostName = entity.LOST_NAME;
            model.LostPhone = entity.LOST_PHONE;
            model.Address = entity.ADDRESS;
            model.Status = entity.STATUS;
            model.CreateTime = entity.CREATE_TIME.Value;
            if (entity.CLAIM_TIME.HasValue)
                model.ClaimTime = entity.CLAIM_TIME;
            model.VehicleID = entity.VEHICLE_ID;
            return model;
        }

    }
}

