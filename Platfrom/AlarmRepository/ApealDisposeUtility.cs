using Gsafety.PTMS.Common.Data;
using Gsafety.PTMS.DBEntity;
using System;
using System.Collections.Generic;
namespace Gsafety.PTMS.Alarm.Repository
{
    public class ApealDisposeUtility
    {

        public static ALM_ALARM_DISPOSE UpdateEntity(ALM_ALARM_DISPOSE entity, ApealDispose model, bool isAdd)
        {
            if (isAdd)
            {
                entity.ID = model.ID;
            }

            entity.ALARM_ID = model.AlarmID;
            entity.ALARM_FLAG = (short)model.AlarmFlag;
            entity.CONTENT = model.Content;
            entity.DISPOSE_STAFF = model.DisposeStaff;
            entity.DISPOSE_TIME = model.DisposeTime;
            return entity;
        }

        public static ApealDispose GetModel(ALM_ALARM_DISPOSE entity)
        {
            ApealDispose model = new ApealDispose();
            model.ID = entity.ID;
            model.AlarmID = entity.ALARM_ID;
            model.AlarmFlag = entity.ALARM_FLAG.Value;
            model.Content = entity.CONTENT;
            model.DisposeStaff = entity.DISPOSE_STAFF;
            model.DisposeTime = entity.DISPOSE_TIME.Value;
            return model;
        }

    }
}

