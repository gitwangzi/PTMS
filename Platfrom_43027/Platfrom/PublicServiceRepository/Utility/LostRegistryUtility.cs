using Gsafety.PTMS.DBEntity;
using Gsafety.PTMS.PublicService.Contract;
using System;
using System.Collections.Generic;
namespace GSafety.PTMS.PublicService.Repository
{
    public class LostRegistryUtility
    {

        public static RUN_LOST_REGISTRY UpdateEntity(RUN_LOST_REGISTRY entity, LostRegistry model, bool isAdd)
        {
            if (isAdd)
            {

                entity.ID = model.ID;
                entity.CLIENT_ID = model.ClientID;

            }
            entity.CLIENT_ID = model.ClientID;
            entity.LOST_NAME = model.LostName;
            entity.CONTENT = model.Content;
            entity.KEYWORD = model.Keyword;
            entity.LOST_IDCARD = model.LostIdcard;
            entity.LOST_PHONE = model.LostPhone;
            entity.ADDRESS = model.Address;
            entity.LOST_TIME = model.LostTime;
            entity.CREATE_TIME = model.CreateTime;
            return entity;
        }

        public static LostRegistry GetModel(RUN_LOST_REGISTRY entity)
        {
            LostRegistry model = new LostRegistry();
            model.ID = entity.ID;
            model.ClientID = entity.CLIENT_ID;
            model.LostName = entity.LOST_NAME;
            model.Content = entity.CONTENT;
            model.Keyword = entity.KEYWORD;
            model.LostIdcard = entity.LOST_IDCARD;
            model.LostPhone = entity.LOST_PHONE;
            model.Address = entity.ADDRESS;
            model.LostTime = entity.LOST_TIME;
            model.CreateTime = entity.CREATE_TIME;

            return model;
        }

    }
}

