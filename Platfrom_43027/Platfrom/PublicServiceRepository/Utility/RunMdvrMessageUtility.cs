
using Gsafety.PTMS.DBEntity;
using Gsafety.PTMS.PublicService.Contract;
using System;
using System.Collections.Generic;

namespace GSafety.PTMS.PublicService.Repository
{
    public class RunMdvrMessageUtility
    {

        public static RUN_MDVR_MESSAGE UpdateEntity(RUN_MDVR_MESSAGE entity, RunMdvrMessage model, bool isAdd)
        {
            if (isAdd)
            {
                entity.ID = Guid.NewGuid().ToString();
            }
            entity.CLIENT_ID = model.ClientId;
            entity.CONTENT = model.Content;
            entity.MESSAGE_TYPE = (short)model.MessageType;
            entity.CREATE_TIME = model.CreateTime;
            entity.CREATOR = model.Creator;
            entity.MESSAGE_TITLE = model.MessageTitle;

            return entity;
        }

        public static RunMdvrMessage GetModel(RUN_MDVR_MESSAGE entity)
        {
            RunMdvrMessage model = new RunMdvrMessage();
            model.ID = entity.ID;
            model.ClientId = entity.CLIENT_ID;
            model.Content = entity.CONTENT;
            model.MessageType = entity.MESSAGE_TYPE;
            model.CreateTime = entity.CREATE_TIME;
            model.MessageTitle = entity.MESSAGE_TITLE;
            model.Creator = entity.CREATOR;

            return model;
        }

    }
}

