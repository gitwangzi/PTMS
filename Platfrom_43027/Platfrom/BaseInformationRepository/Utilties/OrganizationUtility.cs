
using Gsafety.PTMS.BaseInfo;
using Gsafety.PTMS.BaseInformation.Contract;
using Gsafety.PTMS.BaseInformation.Contract.Data;
using Gsafety.PTMS.DBEntity;
using System;
using System.Collections.Generic;

namespace Gsafety.PTMS.BaseInformation.Repository
{
    public class OrganizationUtility
    {
        public static USR_ORGANIZATION GetEntity(USR_ORGANIZATION entity,Organization model)
        {
            entity.ID = model.ID;
            entity.NAME = model.Name;
            entity.PARENT_ID = model.ParentID.Trim();
            entity.CONTACT = model.Contact;
            entity.EMAIL = model.Email;
            entity.PHONE = model.Phone;
            entity.CREATE_TIME = model.CreateTime;
            entity.CREATOR = model.Creator;
            entity.CLIENT_ID = model.ClientID;
            if (model.Valid == 1)
                entity.VALID = 1;
            else
                entity.VALID = 0;
            return entity;
        }

        public static Organization GetModel(USR_ORGANIZATION entity)
        {
            Organization model = new Organization();
            model.ID = entity.ID;
            model.Name = entity.NAME;
            model.ParentID = entity.PARENT_ID.Trim();
            model.Contact = entity.CONTACT;
            model.Email = entity.EMAIL;
            model.Phone = entity.PHONE;
            model.CreateTime = entity.CREATE_TIME;
            model.Creator = entity.CREATOR;
            model.ClientID = entity.CLIENT_ID;
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

