using Gsafety.PTMS.BaseInfo;
using Gsafety.PTMS.DBEntity;
using Gsafety.PTMS.Manager.Contract.Data;
using System;
using System.Collections.Generic;

namespace Gsafety.PTMS.Manager.Repository
{
    public class RoleUtility
    {
        public static void UpdateEntity(USR_ROLE entity, Role model, bool isAdd)
        {
            if (isAdd)
            {
                entity.ID = model.ID;
                entity.CLIENT_ID = model.ClientID;
                entity.CREATE_TIME = model.CreateTime;
                entity.IS_PREDEFINED = (short)(0);
                entity.CREATOR = model.Creator;
                entity.VALID = (short)ValidEnum.Valid;
            }
            entity.NAME = model.Name;
            entity.DESCRIPTION = model.Description;
            //默认设置为0
            entity.ROLE_CATEGORY = model.RoleCategory;
        }

        public static Role GetModel(USR_ROLE entity)
        {
            Role model = new Role();
            model.ID = entity.ID;
            model.Name = entity.NAME;
            model.ClientID = entity.CLIENT_ID;
            model.CreateTime = entity.CREATE_TIME;
            model.Description = entity.DESCRIPTION;
            model.IsPredefined = entity.IS_PREDEFINED == (short)1 ? true : false;
            model.RoleCategory = entity.ROLE_CATEGORY;
            model.Creator = entity.CREATOR;

            return model;
        }
    }
}

