using Gsafety.PTMS.Common.Data;
using Gsafety.PTMS.DBEntity;
using System;
using System.Collections.Generic;

namespace Gsafety.PTMS.Manager.Repository
{
    public class UserOnlineUtility
    {
        public static RUN_USER_ONLINE UpdateEntity(RUN_USER_ONLINE entity, UserOnline model, bool isAdd)
        {
            if (isAdd)
            {
                entity.CLIENT_ID = model.ClientID;
                entity.USER_ID = model.UserID;

            }
            entity.CLIENT_ID = model.ClientID;
            entity.ONLINE_TIME = model.OnlineTime;
            return entity;
        }

        public static UserOnline GetModel(RUN_USER_ONLINE entity)
        {
            UserOnline model = new UserOnline();
            model.ClientID = entity.CLIENT_ID;
            model.OnlineTime = entity.ONLINE_TIME;
            model.UserID = entity.USER_ID;
            return model;
        }

    }
}

