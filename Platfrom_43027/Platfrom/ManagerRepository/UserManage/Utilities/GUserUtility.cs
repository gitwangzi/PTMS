using Gsafety.PTMS.DBEntity;
using Gsafety.PTMS.Manager.Contract.Data;
using System;
using System.Collections.Generic;
using Gsafety.Common.Util;
using Gsafety.PTMS.BaseInfo;
namespace Gsafety.PTMS.Manager.Repository
{
    public class GUserUtility
    {
        public static void UpdateEntity(USR_GUSER entity, GUser model, bool isAdd)
        {
            if (isAdd)
            {
                entity.ID = model.ID;
                entity.PASSWORD = model.Password;
                entity.ACCOUNT = model.Account;
                entity.CREATE_TIME = ConvertHelper.DateTimeToUTC(model.CreateTime);
                entity.CLIENT_ID = model.ClientID;
                entity.VALID = (short)ValidEnum.Valid;
                entity.DEPARTMENT = model.Department;
                if (model.IsClientCreate)
                    entity.IS_CLIENT_CREATE = 1;
                else
                    entity.IS_CLIENT_CREATE = 0;
                entity.CREATOR = model.Creator;
            }

            entity.USER_NAME = model.UserName;
            entity.PHONE = model.Phone;
            entity.MOBILE = model.Mobile;
            entity.EMAIL = model.Email;
            entity.ADDRESS = model.Address;
            entity.DESCRIPTION = model.Description;
            entity.ROLE_ID = model.RoleID;
            entity.DEPARTMENT = model.Department;
        }

        public static GUser GetModel(USR_GUSER entity)
        {
            GUser model = new GUser();
            model.ID = entity.ID;
            model.Account = entity.ACCOUNT;
            model.UserName = entity.USER_NAME;
            model.CreateTime = entity.CREATE_TIME;
            model.Password = entity.PASSWORD;
            model.Phone = entity.PHONE;
            model.Mobile = entity.MOBILE;
            model.Email = entity.EMAIL;
            model.Address = entity.ADDRESS;
            model.Description = entity.DESCRIPTION;
            model.RoleID = entity.ROLE_ID;
            model.Creator = entity.CREATOR;
            if (entity.IS_CLIENT_CREATE == 0)
            {
                model.IsClientCreate = false;
            }
            else
            {
                model.IsClientCreate = true;
            }
            model.Department = entity.DEPARTMENT;
            model.ClientID = entity.CLIENT_ID;

            return model;
        }

    }
}

