using Gsafety.PTMS.Base.Contract.Data;
using Gsafety.PTMS.BaseInfo;
using Gsafety.PTMS.BaseInformation.Contract;
using Gsafety.PTMS.DBEntity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Gsafety.PTMS.BaseInformation.Repository
{
    ///<summary>
    ///组织机构表
    ///</summary>
    public class OrganizationRepository
    {

        /// <summary>
        /// 添加组织机构表
        /// </summary>
        /// <param name="model">组织机构表</param>
        public static bool InsertOrganization(PTMSEntities context, Organization model, string userID)
        {
            try
            {
                USR_ORGANIZATION entity = new USR_ORGANIZATION();
                context.USR_ORGANIZATION.Add(OrganizationUtility.GetEntity(entity, model));

                USR_GUSER u = context.USR_GUSER.FirstOrDefault(n => n.ID == userID);
                if (u.IS_CLIENT_CREATE != 1)
                {
                    USR_ORGANIZATION_USER ou = new USR_ORGANIZATION_USER();
                    ou.ID = Guid.NewGuid().ToString();
                    ou.CREATE_TIME = DateTime.Now.ToUniversalTime();
                    ou.ORGANIZATION_ID = model.ID;
                    ou.USER_ID = userID;
                    context.USR_ORGANIZATION_USER.Add(ou);
                }
                if (context.SaveChanges() > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// 修改组织机构表
        /// </summary>
        public static bool UpdateOrganization(PTMSEntities context, Organization model)
        {
            try
            {
                USR_ORGANIZATION entity = context.USR_ORGANIZATION.FirstOrDefault(n => n.ID == model.ID && n.VALID == (short)ValidEnum.Valid);
                if (entity != null)
                {
                    entity = OrganizationUtility.GetEntity(entity, model);
                    context.USR_ORGANIZATION.Attach(entity);
                    context.Entry(entity).State = EntityState.Modified;
                    if (context.SaveChanges() > 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// 删除组织机构表
        /// </summary>
        public static SingleMessage<bool> DeleteOrganization(PTMSEntities context, string Id, string userid)
        {
            bool result = context.USR_ORGANIZATION.Any(r => r.PARENT_ID == Id && r.VALID == 1);
            if (result)
            {
                return new SingleMessage<bool>(false, "HasChild");
            }

            result = context.BSC_VEHICLE.Any(v => v.ORGNIZATION_ID == Id && v.VALID == 1);
            if (result)
            {
                return new SingleMessage<bool>(false, "ReferenceByVehicle");
            }

            result = context.USR_ORGANIZATION_USER.Any(v => v.ORGANIZATION_ID == Id && v.USER_ID != userid);
            if (result)
            {
                return new SingleMessage<bool>(false, "ReferenceByPerson");
            }

            USR_ORGANIZATION_USER userorganization = context.USR_ORGANIZATION_USER.FirstOrDefault(v => v.ORGANIZATION_ID == Id && v.USER_ID != userid);

            if (userorganization != null)
            {
                context.USR_ORGANIZATION_USER.Remove(userorganization);
            }

            USR_ORGANIZATION entity = context.USR_ORGANIZATION.FirstOrDefault(x => x.ID == Id && x.VALID == (short)ValidEnum.Valid);

            if (entity != null)
            {
                entity.VALID = 0;
                if (context.SaveChanges() > 0)
                {
                    return new SingleMessage<bool>(true);
                }
                else
                {
                    return new SingleMessage<bool>(false, "FailedToSave");
                }
            }
            else
            {
                return new SingleMessage<bool>(false, "NotFound");
            }
        }

        public static List<Organization> GetOrganizationByUser(PTMSEntities context, string userid)
        {
            try
            {
                List<Organization> list = new List<Organization>();
                USR_GUSER user = context.USR_GUSER.FirstOrDefault(n => n.ID == userid);
                if (user.IS_CLIENT_CREATE == 1)
                {
                    var items = from o in context.USR_ORGANIZATION
                                where o.CLIENT_ID == user.CLIENT_ID && o.VALID == 1
                                select o;
                    foreach (var item in items)
                    {
                        list.Add(OrganizationUtility.GetModel(item));
                    }
                }
                else
                {
                    var items = from ou in context.USR_ORGANIZATION_USER
                                join o in context.USR_ORGANIZATION on ou.ORGANIZATION_ID equals o.ID
                                where ou.USER_ID == userid && o.VALID == 1
                                select o;
                    foreach (var item in items)
                    {
                        list.Add(OrganizationUtility.GetModel(item));
                    }
                }

                return list;
            }
            catch
            {
                throw;
            }
        }

        public static List<Organization> GetAllOrganization(PTMSEntities context, string clientID)
        {
            try
            {
                List<Organization> list = new List<Organization>();
                var items = from o in context.USR_ORGANIZATION
                            where o.CLIENT_ID == clientID && o.VALID == 1
                            select o;
                List<string> ids = new List<string>();
                foreach (var item in items)
                {
                    list.Add(OrganizationUtility.GetModel(item));

                    ids.Add(item.ID);
                }

                while (ids.Count != 0)
                {
                    var parents = (from o in context.USR_ORGANIZATION
                                   where ids.Any(n => n == o.PARENT_ID) && o.VALID == 1
                                   select o).ToList();
                    ids.Clear();
                    foreach (var item in parents)
                    {
                        if (!list.Any(n => n.ID == item.ID))
                        {
                            list.Add(OrganizationUtility.GetModel(item));
                            ids.Add(item.ID);
                        }
                    }
                }


                return list;
            }
            catch
            {
                throw;
            }
        }
    }
}

