using Gsafety.PTMS.Base.Contract.Data;
using Gsafety.PTMS.BaseInfo;
using Gsafety.PTMS.DBEntity;
using Gsafety.PTMS.Manager.Contract.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Gsafety.PTMS.Manager.Repository
{
    ///<summary>
    ///用户部门
    ///</summary>
    public class UsrDepartmentRepository
    {
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="model"></param>
        public static SingleMessage<bool> InsertUsrDepartment(PTMSEntities context, UsrDepartment model)
        {
            if (string.IsNullOrWhiteSpace(model.ID))
            {
                throw new ArgumentNullException("model.ID");
            }

            if (string.IsNullOrWhiteSpace(model.Name))
            {
                throw new ArgumentNullException("model.Name");
            }

            var entity = new USR_DEPARTMENT();
            UsrDepartmentUtility.UpdateEntity(entity, model, true);

            context.USR_DEPARTMENT.Add(entity);

            return context.Save();
        }

        /// <summary>
        /// 修改
        /// </summary>
        public static SingleMessage<bool> UpdateUsrDepartment(PTMSEntities context, UsrDepartment model)
        {
            if (string.IsNullOrWhiteSpace(model.ID))
            {
                throw new ArgumentNullException("model.ID");
            }

            if (string.IsNullOrWhiteSpace(model.Name))
            {
                throw new ArgumentNullException("model.Name");
            }

            var entity = context.USR_DEPARTMENT.FirstOrDefault(t => t.ID == model.ID && t.VALID == (short)ValidEnum.Valid);
            if (null == entity)
            {
                return new SingleMessage<bool>(false, CommonErrorMessage.UerDepartmentNoExist);
            }

            UsrDepartmentUtility.UpdateEntity(entity, model, false);
            context.Entry(entity).State = EntityState.Modified;

            return context.Save();
        }

        /// <summary>
        /// 删除
        /// </summary>
        public static SingleMessage<bool> DeleteUsrDepartmentByID(PTMSEntities context, string ID)
        {
            if (string.IsNullOrWhiteSpace(ID))
            {
                throw new ArgumentNullException("ID");
            }

            var reference = context.USR_GUSER.Any(n => n.DEPARTMENT == ID && n.VALID == 1);

            if (reference == true)
            {
                return new SingleMessage<bool>(false, "ReferenceByUser");
            }

            var entity = context.USR_DEPARTMENT.FirstOrDefault(t => t.ID == ID && t.VALID == (short)ValidEnum.Valid);

            if (entity == null)
            {
                return new SingleMessage<bool>(false, CommonErrorMessage.UerDepartmentNoExist);
            }

            entity.VALID = (short)ValidEnum.UnValid;
            context.Entry(entity).State = EntityState.Modified;

            return context.Save();
        }

        public static SingleMessage<bool> IsCanDeleteUserDepartmentById(PTMSEntities context, string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new ArgumentNullException("id");
            }
            if (context.USR_DEPARTMENT.Count(c => c.PARENT_ID == id && c.VALID != 0) > 0)
            {
                return new SingleMessage<bool>(false);
            }
            if (context.USR_GUSER.Count(c => c.DEPARTMENT == id && c.VALID != 0) > 0)
            {
                return new SingleMessage<bool>(false);
            }
            return new SingleMessage<bool>(true);
        }

        /// <summary>
        /// 获取
        /// </summary>
        public static SingleMessage<UsrDepartment> GetUsrDepartment(PTMSEntities context, string ID)
        {
            if (string.IsNullOrWhiteSpace(ID))
            {
                throw new ArgumentNullException("ID");
            }

            var entity = context.USR_DEPARTMENT.SingleOrDefault(n => n.ID == ID && n.VALID == (short)ValidEnum.Valid);
            if (entity != null)
            {
                UsrDepartment model = UsrDepartmentUtility.GetModel(entity);
                return new SingleMessage<UsrDepartment>(model);
            }

            return new SingleMessage<UsrDepartment>(false, CommonErrorMessage.UerDepartmentNoExist);
        }

        /// <summary>
        /// 获取
        /// </summary>
        public static MultiMessage<UsrDepartment> GetUsrDepartmentList(PTMSEntities context, string name, string clientId, int pageIndex = 1, int pageSize = 10)
        {
            int totalCount;
            List<USR_DEPARTMENT> result = new List<USR_DEPARTMENT>();
            if (!string.IsNullOrEmpty(name))
            {
                result = context.USR_DEPARTMENT.Where(t => t.VALID == (short)ValidEnum.Valid && t.NAME.ToLower() == name.ToLower() && t.CLIENT_ID == clientId).Page(out totalCount, pageIndex, pageSize, true).ToList();

            }
            else
            {
                result =
                   context.USR_DEPARTMENT.Where(t => t.VALID == (short)ValidEnum.Valid && t.CLIENT_ID == clientId)
                       .Page(out totalCount, pageIndex, pageSize, true)
                       .ToList();
            }
            var items = result.Select(t => UsrDepartmentUtility.GetModel(t)).ToList();

            return new MultiMessage<UsrDepartment>(items, totalCount);
        }

        /// <summary>
        /// 获取用户组织机构列表
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static MultiMessage<UsrDepartment> GetUserDepartmentList(PTMSEntities context)
        {
            var list =
                context.USR_DEPARTMENT.Where(u => u.VALID == (short)ValidEnum.Valid)
                    .Select(t => UsrDepartmentUtility.GetModel(t))
                    .ToList();
            return new MultiMessage<UsrDepartment>(list, 0);
        }

    }
}
