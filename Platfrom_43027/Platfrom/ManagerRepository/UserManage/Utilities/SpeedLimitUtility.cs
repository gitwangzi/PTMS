using Gsafety.PTMS.Common.Data;
using Gsafety.PTMS.Common.Data.Enum;
using Gsafety.PTMS.DBEntity;
using System;
using System.Collections.Generic;
namespace Gsafety.PTMS.Manager.Repository
{
    public class SpeedLimitUtility
    {
        public static TRF_COMMAND_PARAM UpdateEntity(TRF_COMMAND_PARAM entity, SpeedLimit model, bool isAdd)
        {
            if (isAdd)
            {
                entity.ID = Guid.NewGuid().ToString();
                entity.CLIENT_ID = model.ClientID;
            }
            entity.NAME = model.Name;
            entity.MAX_SPEED = (int)model.MaxSpeed;
            entity.DURATION = (int)model.Duration;
            entity.CREATOR = model.Creator;
            entity.CREATE_TIME = model.CreateTime;
            entity.TYPE = (short)CommandParaEnum.Speed;
            if (model.Valid)
                entity.VALID = 1;
            else
                entity.VALID = 0;
            return entity;
        }

        public static SpeedLimit GetModel(TRF_COMMAND_PARAM entity)
        {
            SpeedLimit model = new SpeedLimit();
            model.ID = entity.ID;
            model.ClientID = entity.CLIENT_ID;
            model.Name = entity.NAME;
            if (entity.MAX_SPEED.HasValue)
                model.MaxSpeed = entity.MAX_SPEED.Value;
            if (entity.DURATION.HasValue)
                model.Duration = entity.DURATION.Value;
            model.Creator = entity.CREATOR;
            if (entity.CREATE_TIME.HasValue)
                model.CreateTime = entity.CREATE_TIME.Value;
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

