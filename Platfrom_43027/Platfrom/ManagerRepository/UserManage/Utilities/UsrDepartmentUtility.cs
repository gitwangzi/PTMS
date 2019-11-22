using Gsafety.PTMS.DBEntity;
using Gsafety.PTMS.Manager.Contract.Data;
using System;
using System.Collections.Generic;
using Gsafety.PTMS.BaseInfo;

namespace Gsafety.PTMS.Manager.Repository
{
    public class UsrDepartmentUtility
    {

        public static void UpdateEntity(USR_DEPARTMENT entity, UsrDepartment model, bool isAdd)
        {
            if (isAdd)
            {
                entity.ID = model.ID;
                entity.CLIENT_ID = model.ClientID;
                entity.CREATE_TIME = model.CreateTime;
                entity.CREATOR = model.Creator;
                entity.VALID = (short)ValidEnum.Valid;
            }

            entity.PARENT_ID = model.ParentID;
            entity.NAME = model.Name;
            entity.CONTACT = model.Contact;
            entity.PHONE = model.Phone;
            entity.EMAIL = model.Email;
        }

        public static UsrDepartment GetModel(USR_DEPARTMENT entity)
        {
            UsrDepartment model = new UsrDepartment();
            model.ID = entity.ID;
            model.ClientID = entity.CLIENT_ID;
            model.ParentID = entity.PARENT_ID;
            model.Name = entity.NAME;
            model.Contact = entity.CONTACT;
            model.Phone = entity.PHONE;
            model.Email = entity.EMAIL;
            model.Creator = entity.CREATOR;
            model.CreateTime = entity.CREATE_TIME;

            return model;
        }

    }
}

