using Gsafety.PTMS.DBEntity;
using Gsafety.PTMS.PublicService.Contract;
using System;
using System.Collections.Generic;
namespace GSafety.PTMS.PublicService.Repository
{
    public class RunAppMessageUtility
    {
        public static RUN_APP_MESSAGE UpdateEntity(RUN_APP_MESSAGE entity, RunAppMessage model, bool isAdd)
        {
            if (isAdd)
            {
                entity.ID = model.ID;
            }
            entity.CLIENT_ID = model.ClientId;
            entity.MESSAGE = model.Message;
            entity.MESSAGE_TYPE = (short)model.MessageType;
            entity.CREATOR = model.Creator;
            entity.CREATE_TIME = model.CreateTime;
            entity.MESSAGE_TITLE = model.MessageTitle;
            return entity;
        }

        public static RunAppMessage GetModel(RUN_APP_MESSAGE entity)
        {
            RunAppMessage model = new RunAppMessage();
            model.ID = entity.ID;
            model.ClientId = entity.CLIENT_ID;
            model.Message = entity.MESSAGE;
            model.MessageType = entity.MESSAGE_TYPE;
            model.Creator = entity.CREATOR;
            model.CreateTime = entity.CREATE_TIME;
            model.MessageTitle = entity.MESSAGE_TITLE;
            return model;
        }

    }
}

