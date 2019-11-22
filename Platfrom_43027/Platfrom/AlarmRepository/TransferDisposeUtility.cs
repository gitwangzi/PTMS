using Gsafety.PTMS.Common.Data;
using Gsafety.PTMS.DBEntity;
using System;
using System.Collections.Generic;
namespace Gsafety.PTMS.Alarm.Repository
{
    public class TransferDisposeUtility
    {

        public static ALM_911_DISPOSE UpdateEntity(ALM_911_DISPOSE entity, TransferDispose model, bool isAdd)
        {
            if (isAdd)
            {
                entity.ID = model.AlarmID;
                entity.ALARM_ID = model.AlarmID;

            }
            entity.FORWARDED_FLAG = (short)model.ForwardedFlag;
            entity.ALARM_FLAG = (short)model.AlarmFlag;
            entity.DISPOSE_TIME = model.DisposeTime;
            entity.DISPOSE_STAFF = model.DisposeStaff;
            entity.TRANSFER_CENTER = model.TransferCenter;
            entity.INCIDENT_ID = model.IncidentID;
            entity.FORWARD_DEST = model.ForwardDest;
            entity.CONTENT = model.Content;
            entity.FORWARD_TIME = model.ForwardTime;
            entity.ALARM_ADDRESS = model.AlarmAddress;
            entity.INCIDENT_TYPE = model.IncidentType;
            entity.APPREAL_STAFF = model.ApprealStaff;
            entity.DISPOSE_CONTENT = model.DisposeContent;
            entity.CREATE_TIME = model.CreateTime;
            if (model.IsTransfer)
            {
                entity.IS_TRANSFER = 1;
            }
            else
            {
                entity.IS_TRANSFER = 0;
            }

            entity.TRANSFER_MODE = (short)model.TransferMode;
            return entity;
        }

        public static TransferDispose GetModel(ALM_911_DISPOSE entity)
        {
            TransferDispose model = new TransferDispose();
            model.ID = entity.ID;
            model.AlarmID = entity.ALARM_ID;
            if (entity.FORWARDED_FLAG.HasValue)
                model.ForwardedFlag = entity.FORWARDED_FLAG.Value;
            if (entity.ALARM_FLAG.HasValue)
                model.AlarmFlag = entity.ALARM_FLAG.Value;
            if (entity.DISPOSE_TIME.HasValue)
                model.DisposeTime = entity.DISPOSE_TIME.Value;
            model.DisposeStaff = entity.DISPOSE_STAFF;
            model.TransferCenter = entity.TRANSFER_CENTER;
            model.IncidentID = entity.INCIDENT_ID;
            model.ForwardDest = entity.FORWARD_DEST;
            model.Content = entity.CONTENT;
            if (entity.FORWARD_TIME.HasValue)
                model.ForwardTime = entity.FORWARD_TIME.Value;
            model.AlarmAddress = entity.ALARM_ADDRESS;
            model.IncidentType = entity.INCIDENT_TYPE;
            model.ApprealStaff = entity.APPREAL_STAFF;
            model.DisposeContent = entity.DISPOSE_CONTENT;
            model.CreateTime = entity.CREATE_TIME;
            if (entity.IS_TRANSFER.HasValue)
            {
                model.IsTransfer = entity.IS_TRANSFER == 1;
            }
            else
            {
                model.IsTransfer = false;
            }

            if (entity.TRANSFER_MODE.HasValue)
            {
                model.TransferMode = entity.TRANSFER_MODE.Value;
            }

            return model;
        }

    }
}

