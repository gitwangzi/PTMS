using Gsafety.PTMS.Common.Data;
using Gsafety.PTMS.DBEntity;
using System;
using System.Collections.Generic;
namespace Gsafety.PTMS.Alert.Repository
{
    public class BusinessAlertHandleUtility
    {
        public static ALT_BUSINESS_ALERT_HANDLE UpdateEntity(ALT_BUSINESS_ALERT_HANDLE entity, BusinessAlertHandle model, bool isAdd)
        {
            if (isAdd)
            {
                entity.ID = model.ID;
            }
            entity.BUSINESS_ALERT_ID = model.BusinessAlertID;
            entity.HANDLE_USER = model.HandleUser;
            entity.CONTENT = model.Content;
            entity.HANDLE_TIME = model.HandleTime;
            return entity;
        }

        public static BusinessAlertHandle GetModel(ALT_BUSINESS_ALERT_HANDLE entity)
        {
            BusinessAlertHandle model = new BusinessAlertHandle();
            model.ID = entity.ID;
            model.BusinessAlertID = entity.BUSINESS_ALERT_ID;
            model.HandleUser = entity.HANDLE_USER;
            model.Content = entity.CONTENT;
            model.HandleTime = entity.HANDLE_TIME.Value;
            return model;
        }

    }
}

