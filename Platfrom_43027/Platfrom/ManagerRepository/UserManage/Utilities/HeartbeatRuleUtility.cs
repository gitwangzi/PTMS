using Gsafety.PTMS.Common.Data;
using Gsafety.PTMS.Common.Data.Enum;
using Gsafety.PTMS.DBEntity;
using System;
using System.Collections.Generic;
namespace Gsafety.PTMS.Manager.Repository
{
    public class HeartbeatRuleUtility
    {

        public static TRF_COMMAND_PARAM UpdateEntity(TRF_COMMAND_PARAM entity, HeartbeatRule model, bool isAdd)
        {
            if (isAdd)
            {
                entity.ID = model.ID;
                entity.CLIENT_ID = model.ClientID;
                //entity.IsVisible = model.IsVisible;
            }

            entity.CLIENT_ID = model.ClientID;
            entity.NAME = model.Name;
            entity.INTERVAL = (short)model.Interval;
            entity.CREATOR = model.Creator;
            entity.CREATE_TIME = model.CreateTime;
            entity.TYPE = (short)CommandParaEnum.HeartBeat;

            if (model.Valid)
                entity.VALID = 1;
            else
                entity.VALID = 0;
            return entity;
        }

        public static HeartbeatRule GetModel(TRF_COMMAND_PARAM entity)
        {
            HeartbeatRule model = new HeartbeatRule();
            model.ID = entity.ID;
            model.ClientID = entity.CLIENT_ID;
            model.Name = entity.NAME;
            if (entity.INTERVAL.HasValue)
                model.Interval = entity.INTERVAL.Value;
            model.Creator = entity.CREATOR;
            if (entity.CREATE_TIME.HasValue)
                model.CreateTime = entity.CREATE_TIME.Value;
            entity.TYPE = (short)CommandParaEnum.HeartBeat;
            if (entity.VALID == 0)
            {
                model.Valid = false;
            }
            else
            {
                model.Valid = true;
            }
            return model;
        }

    }
}

