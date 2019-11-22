using Gsafety.PTMS.Common.Data;
using System;
using System.Collections.Generic;
using Gsafety.PTMS.DBEntity;
namespace LogServiceRepository
{
    public class LogVideoUtility
    {

        public static LOG_VIDEO UpdateEntity(LOG_VIDEO entity, LogVideo model, bool isAdd)
        {
            if (isAdd)
            {

                entity.ID = model.ID;
                entity.CLIENT_ID = model.ClientID;

            }
            entity.OPERATE_TIME = model.OperateTime;
            entity.LOG_TYPE = model.LogType;
            entity.OPERATOR_ID = model.OperatorID;
            entity.OPERATOR_NAME = model.OperatorName;
            entity.CHANNEL = model.Channel;
            entity.VEHICLE_ID = model.VehicleID;
            entity.MDVR_CORE_SN = model.MdvrCoreSn;
            entity.SUITE_SN = model.SuiteSn;
            entity.CONTENT = model.Content;
            return entity;
        }

        public static LogVideo GetModel(LOG_VIDEO entity)
        {
            LogVideo model = new LogVideo();
            model.ID = entity.ID;
            model.OperateTime = entity.OPERATE_TIME;
            model.LogType = entity.LOG_TYPE.Value;
            model.OperatorID = entity.OPERATOR_ID;
            model.OperatorName = entity.OPERATOR_NAME;
            model.Channel = entity.CHANNEL;
            model.VehicleID = entity.VEHICLE_ID;
            model.ClientID = entity.CLIENT_ID;
            model.MdvrCoreSn = entity.MDVR_CORE_SN;
            model.SuiteSn = entity.SUITE_SN;
            model.Content = entity.CONTENT;
            return model;
        }

    }
}

