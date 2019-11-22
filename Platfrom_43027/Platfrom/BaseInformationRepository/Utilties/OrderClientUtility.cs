using Gsafety.PTMS.BaseInformation.Contract.Data;
using Gsafety.PTMS.Common.Data;
using Gsafety.PTMS.DBEntity;
using System;
using System.Collections.Generic;
namespace Gsafety.PTMS.BaseInformation.Repository
{
    public class OrderClientUtility
    {
        public static void UpdateEntity(BSC_ORDER_CLIENT entity, OrderClient model, bool isAdd)
        {
            if (isAdd)
            {
                entity.ID = model.ID;
            }

            entity.NAME = model.Name;
            entity.BEGIN_TIME = model.BeginTime;
            entity.END_TIME = model.EndTime;
            entity.ADDRESS = model.Address;
            entity.PHONE = model.Phone;
            entity.MOBILE = model.Mobile;
            entity.EMAIL = model.Email;
            entity.CONTACT = model.Contact;
            entity.TANSFER_MODE = (short)model.TansferMode;
            entity.USER_COUNT = model.UserCount;
            entity.DEVICE_COUNT = model.DeviceCount;
            entity.STATUS = (short)model.Status;
            entity.VERSION = model.PlatformVersion;
            entity.CREATE_TIME = DateTime.UtcNow;
        }

        public static OrderClient GetModel(BSC_ORDER_CLIENT entity)
        {
            OrderClient model = new OrderClient();
            model.ID = entity.ID;
            model.Name = entity.NAME;
            model.BeginTime = entity.BEGIN_TIME;
            model.EndTime = entity.END_TIME;
            model.Address = entity.ADDRESS;
            model.Phone = entity.PHONE;
            model.Mobile = entity.MOBILE;
            model.Email = entity.EMAIL;
            model.Contact = entity.CONTACT;
            model.TansferMode = (TansferModeEnum)entity.TANSFER_MODE;
            model.UserCount = (int)entity.USER_COUNT;
            model.DeviceCount = (int)entity.DEVICE_COUNT;
            model.Status = (StatusEnum)entity.STATUS;
            model.PlatformVersion = entity.VERSION;
            return model;
        }
    }
}

