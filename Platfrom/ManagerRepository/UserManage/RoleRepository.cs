using Gsafety.Common.Util;
using Gsafety.PTMS.Base.Contract.Data;
using Gsafety.PTMS.BaseInfo;
using Gsafety.PTMS.DBEntity;
using Gsafety.PTMS.Manager.Contract;
using Gsafety.PTMS.Manager.Contract.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;

namespace Gsafety.PTMS.Manager.Repository
{
    public class RoleRepository
    {
        private static List<int> _exclusiveRoleCategory = new List<int>()
        {
            (int)RoleCategory.SuperPower,
            (int)RoleCategory.ClientAdmin,
        };

        /// <summary>
        /// 获取角色
        /// 包含角色所具有的功能项
        /// </summary>
        public static SingleMessage<Role> GetRoleByID(PTMSEntities context, string roleID)
        {
            if (string.IsNullOrWhiteSpace(roleID))
            {
                throw new ArgumentNullException("roleID");
            }

            var role = context.USR_ROLE.FirstOrDefault(n => n.ID == roleID && n.VALID == (short)ValidEnum.Valid);

            if (role == null)
            {
                return new SingleMessage<Role>(false, CommonErrorMessage.RoleNotExist);
            }
            var model = RoleUtility.GetModel(role);

            model.FuncItems = GetFuncItems(context, roleID);

            return new SingleMessage<Role>(model);
        }

        /// <summary>
        /// 针对角色分配功能
        /// </summary>
        /// <param name="context"></param>
        /// <param name="roleID"></param>
        /// <returns></returns>
        public static MultiMessage<string> GetFunItemsByRoleID(PTMSEntities context, string roleID)
        {
            if (string.IsNullOrWhiteSpace(roleID))
            {
                throw new ArgumentNullException("roleID");
            }

            if (context.USR_ROLE.Any(n => n.ID == roleID && n.VALID == (short)ValidEnum.Valid) == false)
            {
                return new MultiMessage<string>(false, CommonErrorMessage.RoleNotExist);
            }

            var list = GetFuncItems(context, roleID);

            return new MultiMessage<string>(list, list.Count);
        }

        private static List<string> GetFuncItems(PTMSEntities context, string roleID)
        {
            var funItems = context.USR_ROLE_FUNCS.Where(t => t.ROLE_ID == roleID).Select(t => t.FUNC_ID).ToList();

            return funItems;
        }

        /// <summary>
        /// 添加角色表
        /// </summary>
        /// <param name="model">角色表</param>
        public static SingleMessage<bool> InsertRole(PTMSEntities context, Role model)
        {
            if (string.IsNullOrWhiteSpace(model.ID))
            {
                throw new ArgumentNullException("model.ID");
            }

            if (string.IsNullOrWhiteSpace(model.Name))
            {
                throw new ArgumentNullException("model.Name");
            }

            if (model.RoleCategory == (short)RoleCategory.SuperPower)
            {
                throw new ArgumentNullException("model.RoleCategory");
            }

            var entity = new USR_ROLE();
            var result = context.USR_ROLE.FirstOrDefault(t => t.NAME == model.Name && t.CLIENT_ID == model.ClientID && t.VALID == 1);
            if (result != null)
            {
                return new SingleMessage<bool>(false, "SameNameExist");
            }
            model.CreateTime = ConvertHelper.DateTimeNow();
            RoleUtility.UpdateEntity(entity, model, true);

            context.USR_ROLE.Add(entity);

            return context.Save();
        }

        /// <summary>
        /// 修改角色表
        /// </summary>
        public static SingleMessage<bool> UpdateRole(PTMSEntities context, Role model,bool isUpdateRole)
        {
            if (string.IsNullOrWhiteSpace(model.ID))
            {
                throw new ArgumentNullException("model.ID");
            }

            if (string.IsNullOrWhiteSpace(model.Name))
            {
                throw new ArgumentNullException("model.Name");
            }

            var result = context.USR_ROLE.FirstOrDefault(t => t.NAME == model.Name && t.ID != model.ID && t.VALID == 1);
            if (result != null)
            {
                return new SingleMessage<bool>(false, "SameNameExist");
            }

            var entity = context.USR_ROLE.FirstOrDefault(t => t.ID == model.ID && t.VALID == (short)ValidEnum.Valid);
            if (null == entity)
            {
                return new SingleMessage<bool>(false, CommonErrorMessage.RoleNotExist);
            }

            if(isUpdateRole)
            {
                bool reference = context.USR_GUSER.Any(n => n.ROLE_ID == model.ID && n.VALID == (short)ValidEnum.Valid);
                if (reference)
                {
                    return new SingleMessage<bool>(false, "UserReference");
                }

                List<USR_ROLE_FUNCS> funcs = context.USR_ROLE_FUNCS.Where(n => n.ROLE_ID == model.ID).ToList();
                foreach (var item in funcs)
                {
                    context.USR_ROLE_FUNCS.Remove(item);
                }
            }

            RoleUtility.UpdateEntity(entity, model, false);
            context.Entry(entity).State = EntityState.Modified;

            return context.Save();
        }

        /// <summary>
        /// 删除角色表
        /// </summary>
        public static SingleMessage<bool> DeleteRoleByID(PTMSEntities context, string ID)
        {
            if (string.IsNullOrWhiteSpace(ID))
            {
                throw new ArgumentNullException("ID");
            }

            bool reference = context.USR_GUSER.Any(n => n.ROLE_ID == ID && n.VALID == (short)ValidEnum.Valid);
            if (reference)
            {
                return new SingleMessage<bool>(false, "UserReference");
            }

            List<USR_ROLE_FUNCS> funcs = context.USR_ROLE_FUNCS.Where(n => n.ROLE_ID == ID).ToList();
            foreach (var item in funcs)
            {
                context.USR_ROLE_FUNCS.Remove(item);
            }

            var entity = context.USR_ROLE.FirstOrDefault(t => t.ID == ID && t.VALID == (short)ValidEnum.Valid && t.IS_PREDEFINED == 0);

            if (entity == null)
            {
                return new SingleMessage<bool>(false, CommonErrorMessage.RoleNotExist);
            }
            entity.VALID = (short)0;

            return context.Save();
        }

        /// <summary>
        /// 获取角色表
        /// </summary>
        public static MultiMessage<Role> GetRoleList(PTMSEntities context, string clientID, string roleName, int pageIndex = 1, int pageSize = 10)
        {
            Expression<Func<USR_ROLE, bool>> filter = t => t.VALID == (short)ValidEnum.Valid
                && ((t.CLIENT_ID == clientID && t.IS_PREDEFINED == 0) || t.IS_PREDEFINED == 1)
                && _exclusiveRoleCategory.Contains(t.ROLE_CATEGORY) == false;

            if (string.IsNullOrWhiteSpace(roleName) == false)
            {
                var name = roleName.Trim().ToUpper();
                filter = filter.And(t => t.NAME.ToUpper().Contains(name));
            }

            int totalCount;

            var items = context.USR_ROLE
                .Where(filter)
                .Page(out totalCount, pageIndex, pageSize, true, t => t.CREATE_TIME, false)
                .Select(role => new Role()
                {
                    ID = role.ID,
                    Name = role.NAME,
                    ClientID = role.CLIENT_ID,
                    CreateTime = role.CREATE_TIME,
                    Description = role.DESCRIPTION,
                    IsPredefined = role.IS_PREDEFINED == (short)1,
                    RoleCategory = role.ROLE_CATEGORY,
                    Creator = context.USR_GUSER.FirstOrDefault(t => t.ID == role.CREATOR).USER_NAME
                })
                .ToList();

            return new MultiMessage<Role>(items, totalCount);
        }

        public static SingleMessage<bool> UpdateRoleFunItems(PTMSEntities context, string roleID, List<string> funItemIDs)
        {
            if (string.IsNullOrWhiteSpace(roleID))
            {
                throw new ArgumentNullException("roleID");
            }

            if (null == funItemIDs)
            {
                funItemIDs = new List<string>();
            }

            if (false == context.USR_ROLE.Any(t => t.ID == roleID && t.VALID == (short)ValidEnum.Valid))
            {
                return new SingleMessage<bool>(false, CommonErrorMessage.RoleNotExist);
            }

            var previewFunItems = context.USR_ROLE_FUNCS.Where(t => t.ROLE_ID == roleID).ToList();
            var previewFunItemIDs = previewFunItems.Select(t => t.FUNC_ID).ToList();

            var needDelItems = previewFunItems.Where(t => funItemIDs.Contains(t.FUNC_ID) == false).ToList();
            foreach (var item in needDelItems)
            {
                context.USR_ROLE_FUNCS.Remove(item);
            }

            var date = ConvertHelper.DateTimeNow();
            var needAddItems = funItemIDs.Where(t => previewFunItemIDs.Contains(t) == false)
                .Select(t => new USR_ROLE_FUNCS()
                {
                    ID = Guid.NewGuid().ToString(),
                    FUNC_ID = t,
                    ROLE_ID = roleID,
                    CREATE_TIME = date
                }).ToList();

            foreach (var item in needAddItems)
            {
                context.USR_ROLE_FUNCS.Add(item);
            }

            if (needDelItems.Count == 0 && needAddItems.Count == 0)
            {
                return new SingleMessage<bool>(true);
            }

            return context.Save();
        }
    }
}
