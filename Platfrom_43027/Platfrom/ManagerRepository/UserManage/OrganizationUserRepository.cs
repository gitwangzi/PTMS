using System;
using System.Collections.Generic;
using System.Linq;
using Gsafety.PTMS.BaseInfo;

using Gsafety.PTMS.Base.Contract.Data;
using Gsafety.PTMS.Message.Contract;
using Gsafety.PTMS.DBEntity;
using System.Collections.ObjectModel;
namespace Gsafety.PTMS.Manager.Repository.UserManage
{
    ///<summary>
    ///组织机构用户关系表
    ///</summary>
    public class OrganizationUserRepository
    {

        static string FailedToSave = "Failed to Save to DB";
        /// <summary>
        /// 添加组织机构用户关系表
        /// </summary>
        /// <param name="model">组织机构用户关系表</param>
        public static SingleMessage<bool> InsertOrganizationUser(List<string> orgIds, string userID)
        {
            using (var context = new PTMSEntities())
            {
                var exists = context.USR_ORGANIZATION_USER.Where(t => t.USER_ID == userID).ToList();
                var needDelete = exists.Where(t => orgIds.Contains(t.ORGANIZATION_ID) == false).ToList();

                foreach (var item in needDelete)
                {
                    context.USR_ORGANIZATION_USER.Remove(item);
                }

                var needAdd = orgIds.Where(t => exists.Any(r => r.ORGANIZATION_ID == t) == false).ToList();
                foreach (var item in needAdd)
                {
                    var entity = new USR_ORGANIZATION_USER()
                    {
                        ID = Guid.NewGuid().ToString(),
                        ORGANIZATION_ID = item,
                        USER_ID = userID,
                        CREATE_TIME = DateTime.UtcNow,
                    };

                    context.USR_ORGANIZATION_USER.Add(entity);
                }

                if (needAdd.Count == 0 && needDelete.Count == 0)
                {
                    return new SingleMessage<bool>(true);
                }

                return context.Save();
            }
        }

        /// <summary>
        /// 修改组织机构用户关系表
        /// </summary>
        public static SingleMessage<bool> UpdateOrganizationUser(PTMSEntities context, OrganizationUser model)
        {
            var entity = context.USR_ORGANIZATION_USER.FirstOrDefault(t => t.ID == model.ID);
            if (null == entity)
            {
                return new SingleMessage<bool>(false, "");
            }

            OrganizationUserUtility.UpdateEntity(entity, model, false);
            context.Entry(entity).State = System.Data.EntityState.Modified;
            return context.Save();
        }

        /// <summary>
        /// 删除组织机构用户关系表
        /// </summary>
        public static SingleMessage<bool> DeleteOrganizationUserByID(PTMSEntities context, string ID)
        {
            USR_ORGANIZATION_USER entity = context.USR_ORGANIZATION_USER.FirstOrDefault(t => t.ID == ID);
            if (entity != null)
            {
                context.USR_ORGANIZATION_USER.Attach(entity);
                context.USR_ORGANIZATION_USER.Remove(entity);
                return context.Save();
            }
            else
            {
                return new SingleMessage<bool>(false, "");
            }
        }

        ///// <summary>
        ///// 获取组织机构用户关系表
        ///// </summary>
        //public static MultiMessage<Organization> GetOrganizationUser(PTMSEntities context, string ID)
        //{
        //    USR_ORGANIZATION entity = context.USR_ORGANIZATION.SingleOrDefault(n => n.ID == ID);
        //    if (entity != null)
        //    {
        //        OrganizationUser model = OrganizationUserUtility.GetModel(entity);
        //        return new MultiMessage<OrganizationUser>(model);
        //    }
        //    return null;
        //}

        /// <summary>
        /// 获取组织机构用户关系表
        /// </summary>
        public static MultiMessage<OrganizationUser> GetOrganizationUserList(PTMSEntities context, string ID)
        {
            int totalCount;

            var items = from u in context.USR_GUSER
                        join ou in context.USR_ORGANIZATION_USER on u.ID equals ou.USER_ID
                        join o in context.USR_ORGANIZATION on ou.ORGANIZATION_ID equals o.ID
                        where u.ID == ID
                        orderby ou.CREATE_TIME descending
                        select new OrganizationUser
                        {
                            ID = ou.ID,
                            UserId = ou.USER_ID,
                            OrganizationId = ou.ORGANIZATION_ID,
                            CreateTime = ou.CREATE_TIME
                        };

            var list = items.Page(out totalCount, 1, 20, true).ToList();

            return new MultiMessage<OrganizationUser>(list, totalCount);
        }

    }
}

