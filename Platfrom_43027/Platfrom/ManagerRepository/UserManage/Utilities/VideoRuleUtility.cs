using Gsafety.PTMS.Common.Data;
using Gsafety.PTMS.Common.Data.Enum;
using Gsafety.PTMS.DBEntity;
using Gsafety.PTMS.Manager.Contract.Data.CommandManage;
using System;
using System.Collections.Generic;
namespace Gsafety.PTMS.Manager.Repository
{
    public class VideoRuleUtility
    {

        public static TRF_COMMAND_PARAM UpdateEntity(TRF_COMMAND_PARAM entity, VideoRule model, bool isAdd)
        {
            if (isAdd)
            {
                entity.ID = model.ID;
                entity.CLIENT_ID = model.ClientID;
            }
            entity.CLIENT_ID = model.ClientID;
            entity.BRIGHTNESS = (short)model.Brightness;
            entity.CONTRAST = (short)model.Contrast;
            entity.CREATOR = model.Creator;
            entity.CREATE_TIME = model.CreateTime;
            entity.NAME = model.Name;
            entity.TYPE = (short)CommandParaEnum.LED;
            if (model.Valid == 1)
                entity.VALID = 1;
            else
                entity.VALID = 0;
            return entity;
        }

        public static VideoRule GetModel(TRF_COMMAND_PARAM entity)
        {
            VideoRule model = new VideoRule();
            model.ID = entity.ID;
            model.ClientID = entity.CLIENT_ID;
            model.Creator = entity.CREATOR;
            model.Name = entity.NAME;
            if (entity.BRIGHTNESS.HasValue)
                model.Brightness = entity.BRIGHTNESS.Value;
            if (entity.CONTRAST.HasValue)
                model.Contrast = entity.CONTRAST.Value;
            if (entity.CREATE_TIME.HasValue)
                model.CreateTime = entity.CREATE_TIME.Value;

            if (entity.VALID == 0)
            {
                model.Valid = 0;
            }
            else
            {
                model.Valid = 1;
            }
            return model;
        }

    }
}

