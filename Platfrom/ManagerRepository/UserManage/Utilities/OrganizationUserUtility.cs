using Gsafety.PTMS.DBEntity;
using Gsafety.PTMS.Message.Contract;
using System;
using System.Collections.Generic;
namespace Gsafety.PTMS.Manager.Repository
{
	public class OrganizationUserUtility
	{

        public static USR_ORGANIZATION_USER UpdateEntity(USR_ORGANIZATION_USER entity, OrganizationUser model, bool isAdd)
		{
			if (isAdd)
			{

				entity.ID = model.ID;

			}
			entity.USER_ID = model.UserId;
			entity.ORGANIZATION_ID = model.OrganizationId;
			entity.CREATE_TIME = model.CreateTime;
			return entity;
		}

        public static OrganizationUser GetModel(USR_ORGANIZATION_USER entity)
		{
			OrganizationUser model = new OrganizationUser();
			model.ID = entity.ID;
            model.UserId = entity.USER_ID;
            model.OrganizationId = entity.ORGANIZATION_ID;
            model.CreateTime = entity.CREATE_TIME;
			return model;
		}

	}
}

