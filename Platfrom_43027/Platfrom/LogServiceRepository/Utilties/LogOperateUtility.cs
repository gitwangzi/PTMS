using Gsafety.PTMS.Common.Data;
using Gsafety.PTMS.DBEntity;
using System;
using System.Collections.Generic;
namespace LogServiceRepository
{
    public class LogOperateUtility
    {

        public static LOG_OPERATE UpdateEntity(LOG_OPERATE entity, LogOperate model, bool isAdd)
        {
            if (isAdd)
            {

                entity.ID = model.ID;
                entity.CLIENT_ID = model.ClientID;

            }
            entity.CLIENT_ID = model.ClientID;
            entity.OPERATE_CONTENT = model.OperateContent;
            entity.OPERATE_TIME = model.OperateTime;
            entity.OPERATE_TYPE = (short)model.OperateType;
            entity.OPERATOR_ID = model.OperatorID;
            entity.OPERATOR_NAME = model.OperatorName;
            return entity;
        }

        public static LogOperate GetModel(LOG_OPERATE entity)
        {
            LogOperate model = new LogOperate();
            model.ID = entity.ID;
            model.ClientID = entity.CLIENT_ID;
            model.OperateContent = entity.OPERATE_CONTENT;
            model.OperateTime = entity.OPERATE_TIME.Value;
            if (entity.OPERATE_TYPE.HasValue)
                model.OperateType = entity.OPERATE_TYPE.Value;
            model.OperatorID = entity.OPERATOR_ID;
            model.OperatorName = entity.OPERATOR_NAME;
            return model;
        }

    }
}

