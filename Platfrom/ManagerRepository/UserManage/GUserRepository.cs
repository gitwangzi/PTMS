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
    public class GUserRepository
    {

        /// <summary>
        ///根据名获取用户
        /// </summary>
        public static SingleMessage<GUser> GetGUser(PTMSEntities context, string account)
        {
            if (string.IsNullOrWhiteSpace(account))
            {
                throw new ArgumentNullException("account");
            }

            var user = context.USR_GUSER.SingleOrDefault(n => n.ACCOUNT == account.Trim() && n.VALID == (short)ValidEnum.Valid);

            if (user == null)
            {
                return new SingleMessage<GUser>(false, CommonErrorMessage.AccountNoExist);
            }

            var model = GUserUtility.GetModel(user);
            return new SingleMessage<GUser>(model);
        }

        /// <summary>
        /// 插入用户
        /// 方法内没有调用context.SaveChanges(）
        /// </summary>
        /// <param name="context"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public static SingleMessage<bool> Insert(PTMSEntities context, GUser model)
        {
            if (string.IsNullOrWhiteSpace(model.ID))
            {
                throw new ArgumentNullException("model.ID");
            }

            if (string.IsNullOrWhiteSpace(model.Account) || context.USR_GUSER.Any(t => t.ACCOUNT == model.Account && t.VALID == 1))
            {
                return new SingleMessage<bool>(false, CommonErrorMessage.AccountExist);
            }

            if (string.IsNullOrWhiteSpace(model.RoleID) || string.IsNullOrWhiteSpace(model.ClientID))
            {
                throw new ArgumentNullException("model.RoleID or model.ClientID");
            }

            if (false == context.USR_ROLE.Any(r => r.ID == model.RoleID))
            {
                return new SingleMessage<bool>(false, CommonErrorMessage.RoleNotExist);
            }

            if (true == context.BSC_ORDER_CLIENT.Any(t => t.ID == model.ClientID) && true == context.BSC_ORDER_CLIENT.Local.Any(t => t.ID == model.ClientID))
            {
                return new SingleMessage<bool>(false, CommonErrorMessage.OrderClientNotExist);
            }

            model.CreateTime = ConvertHelper.DateTimeNow();
            var entity = new USR_GUSER();
            GUserUtility.UpdateEntity(entity, model, true);

            context.USR_GUSER.Add(entity);

            return new SingleMessage<bool>(true);
        }

        /// <summary>
        /// 更新用户
        /// 方法内没有调用context.SaveChanges(）
        /// </summary>
        /// <param name="context"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public static SingleMessage<bool> Update(PTMSEntities context, GUser model)
        {
            if (string.IsNullOrWhiteSpace(model.ID) || string.IsNullOrWhiteSpace(model.RoleID))
            {
                throw new ArgumentNullException("model.ID or model.RoleID");
            }

            if (false == context.USR_ROLE.Any(r => r.ID == model.RoleID && r.VALID == (short)ValidEnum.Valid))
            {
                return new SingleMessage<bool>(false, CommonErrorMessage.RoleNotExist);
            }

            var entity = context.USR_GUSER.FirstOrDefault(t => t.ID == model.ID && t.VALID == (short)ValidEnum.Valid);
            if (null == entity)
            {
                return new SingleMessage<bool>(false, CommonErrorMessage.AccountNoExist);
            }

            if (entity.ROLE_ID != model.RoleID)
            {
                USR_ROLE role = context.USR_ROLE.FirstOrDefault(n => n.ID == entity.ROLE_ID);
                USR_ROLE modelrole = context.USR_ROLE.FirstOrDefault(n => n.ID == model.RoleID);

                if (role.ROLE_CATEGORY == (short)RoleCategory.MaintainAdmin || role.ROLE_CATEGORY == (short)RoleCategory.MaintainMonitor)
                {
                    if (modelrole.ROLE_CATEGORY == (short)RoleCategory.SecurityAdmin || modelrole.ROLE_CATEGORY == (short)RoleCategory.SecurityMonitor)
                    {
                        List<BSC_SETUPSTATION_USER> setupstations = context.BSC_SETUPSTATION_USER.Where(n => n.USER_ID == model.ID).ToList();
                        foreach (var item in setupstations)
                        {
                            context.BSC_SETUPSTATION_USER.Remove(item);
                        }
                    }
                }
                else if (modelrole.ROLE_CATEGORY == (short)RoleCategory.SecurityAdmin || modelrole.ROLE_CATEGORY == (short)RoleCategory.SecurityMonitor)
                {
                    if (modelrole.ROLE_CATEGORY == (short)RoleCategory.MaintainAdmin || modelrole.ROLE_CATEGORY == (short)RoleCategory.MaintainMonitor)
                    {
                        List<USR_ORGANIZATION_USER> organizationusrs = context.USR_ORGANIZATION_USER.Where(n => n.USER_ID == model.ID).ToList();
                        foreach (var item in organizationusrs)
                        {
                            context.USR_ORGANIZATION_USER.Remove(item);
                        }

                        List<RUN_MONITOR_GROUP_VEHICLE> groupvehicles = (from gv in context.RUN_MONITOR_GROUP_VEHICLE
                                                                         join g in context.RUN_MONITOR_GROUP on gv.GROUP_ID equals g.ID
                                                                         where g.AD_USER == model.ID
                                                                         select gv).ToList();
                        foreach (var item in groupvehicles)
                        {
                            context.RUN_MONITOR_GROUP_VEHICLE.Remove(item);
                        }

                        List<RUN_MONITOR_GROUP> groups = (from g in context.RUN_MONITOR_GROUP
                                                          where g.AD_USER == model.ID
                                                          select g).ToList();

                        foreach (var item in groups)
                        {
                            context.RUN_MONITOR_GROUP.Remove(item);
                        }
                    }
                }
            }

            GUserUtility.UpdateEntity(entity, model, false);

            context.USR_GUSER.Attach(entity);
            context.Entry(entity).State = EntityState.Modified;

            return context.Save();
        }

        public static SingleMessage<bool> UpdateDepartment(PTMSEntities context, string userID, string departmentID)
        {
            if (string.IsNullOrWhiteSpace(userID) || string.IsNullOrWhiteSpace(departmentID))
            {
                return new SingleMessage<bool>(false, CommonErrorMessage.ParameterInvalid);
            }

            var entity = context.USR_GUSER.FirstOrDefault(t => t.ID == userID);
            if (null == entity)
            {
                return new SingleMessage<bool>(false, CommonErrorMessage.AccountNoExist);
            }

            if (context.USR_DEPARTMENT.Any(t => t.ID == departmentID) == false)
            {
                return new SingleMessage<bool>(false, CommonErrorMessage.UerDepartmentNoExist);
            }

            entity.DEPARTMENT = departmentID;
            context.Entry(entity).State = EntityState.Modified;

            return context.Save();
        }

        public static SingleMessage<bool> DeleteByUserID(PTMSEntities context, string userID)
        {
            if (string.IsNullOrWhiteSpace(userID))
            {
                throw new ArgumentNullException("userID");
            }

            userID = userID.Trim();
            List<USR_ORGANIZATION_USER> usrs = context.USR_ORGANIZATION_USER.Where(n => n.USER_ID == userID).ToList();
            foreach (var item in usrs)
            {
                context.USR_ORGANIZATION_USER.Remove(item);
            }
            List<BSC_SETUPSTATION_USER> setupusers = context.BSC_SETUPSTATION_USER.Where(n => n.USER_ID == userID).ToList();
            foreach (var item in setupusers)
            {
                context.BSC_SETUPSTATION_USER.Remove(item);
            }

            List<RUN_MONITOR_GROUP_VEHICLE> groupvehicles = (from gv in context.RUN_MONITOR_GROUP_VEHICLE
                                                             join g in context.RUN_MONITOR_GROUP on gv.GROUP_ID equals g.ID
                                                             where g.AD_USER == userID
                                                             select gv).ToList();
            foreach (var item in groupvehicles)
            {
                context.RUN_MONITOR_GROUP_VEHICLE.Remove(item);
            }

            List<RUN_MONITOR_GROUP> groups = (from g in context.RUN_MONITOR_GROUP
                                              where g.AD_USER == userID
                                              select g).ToList();

            foreach (var item in groups)
            {
                context.RUN_MONITOR_GROUP.Remove(item);
            }

            var entity = context.USR_GUSER.FirstOrDefault(t => t.ID == userID);

            //Set To UnValid
            entity.VALID = (short)ValidEnum.UnValid;
            context.Entry(entity).State = EntityState.Modified;
            return context.Save();
        }

        public static MultiMessage<GUser> GetGUserList(PTMSEntities context, string clientID, string userDepartmentID, string qureyUserName, int pageIndex = 1, int pageSize = 10, string orderBy = "")
        {
            if (string.IsNullOrWhiteSpace(clientID) || string.IsNullOrWhiteSpace(userDepartmentID))
            {
                return new MultiMessage<GUser>(new List<GUser>(), 0);
            }

            Expression<Func<USR_GUSER, bool>> filter = n => n.CLIENT_ID == clientID
                && n.VALID == (short)ValidEnum.Valid
                && n.DEPARTMENT == userDepartmentID;
            if (string.IsNullOrWhiteSpace(qureyUserName) == false)
            {
                qureyUserName = qureyUserName.Trim().ToUpper();
                filter = filter.And(t => t.USER_NAME.ToUpper().Contains(qureyUserName));
            }

            int totalCount;
            var list = context.USR_GUSER
                .Where(filter)
                .Page(out totalCount, pageIndex, pageSize, true, t => t.CREATE_TIME, false)
                .Select(entity => new GUser()
                {
                    ID = entity.ID,
                    Account = entity.ACCOUNT,
                    UserName = entity.USER_NAME,
                    CreateTime = entity.CREATE_TIME,
                    Phone = entity.PHONE,
                    Mobile = entity.MOBILE,
                    Email = entity.EMAIL,
                    Address = entity.ADDRESS,
                    Description = entity.DESCRIPTION,
                    RoleID = entity.ROLE_ID,
                    Creator = context.USR_GUSER.FirstOrDefault(t => t.ID == entity.CREATOR).ACCOUNT,
                    Department = entity.DEPARTMENT,
                    ClientID = entity.CLIENT_ID,
                    RoleName = context.USR_ROLE.FirstOrDefault(t => t.ID == entity.ROLE_ID).NAME,
                    RoleCategory = context.USR_ROLE.FirstOrDefault(t => t.ID == entity.ROLE_ID).ROLE_CATEGORY,
                })
                .ToList();

            return new MultiMessage<GUser>(list, totalCount);
        }

        public static SingleMessage<bool> ModifyPassword(PTMSEntities context, string userID, string password)
        {
            if (string.IsNullOrWhiteSpace(userID))
            {
                throw new ArgumentNullException("userID");
            }
            if (string.IsNullOrWhiteSpace(password))
            {
                throw new ArgumentNullException("password");
            }

            var user = context.USR_GUSER.FirstOrDefault(t => t.ID == userID && t.VALID == (short)ValidEnum.Valid);
            if (user == null)
            {
                return new SingleMessage<bool>(false, CommonErrorMessage.AccountNoExist);
            }

            user.PASSWORD = password;
            context.Entry(user).State = EntityState.Modified;

            return context.Save();
        }

        /// <summary>
        /// 检查用户名是否存在
        /// </summary>
        /// <param name="context"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        public static SingleMessage<bool> IsUserNameExist(PTMSEntities context, string userName, string userID)
        {
            if (string.IsNullOrWhiteSpace(userName))
            {
                throw new ArgumentNullException("userName");
            }

            bool result = false;
            if (string.IsNullOrEmpty(userID))
            {
                result = context.USR_GUSER.Any(g => g.ACCOUNT == userName && g.VALID == 1);
            }
            else
            {
                result = context.USR_GUSER.Any(g => g.ACCOUNT == userName && g.ID != userID && g.VALID == 1);
            }

            return new SingleMessage<bool>(result);
        }

        internal static bool IsRoleContainUser(PTMSEntities context, string roleID)
        {
            return context.USR_GUSER.Any(t => t.VALID == (short)1 && t.ROLE_ID == roleID);
        }

        public static SingleMessage<GUser> GetOrderClientGUserByClientID(PTMSEntities context, string ID)
        {
            if (string.IsNullOrWhiteSpace(ID))
            {
                throw new ArgumentNullException("ID");
            }

            var user = context.USR_GUSER.SingleOrDefault(n => n.CLIENT_ID == ID.Trim() && n.IS_CLIENT_CREATE == 1 && n.VALID == 1);

            if (user == null)
            {
                return new SingleMessage<GUser>(false, CommonErrorMessage.OrderClientAdminNoExist);
            }

            var model = GUserUtility.GetModel(user);
            return new SingleMessage<GUser>(model);
        }


        public static MultiMessage<GUser> GetInstallStationUser(PTMSEntities context, string clientID)
        {
            MultiMessage<GUser> result = new MultiMessage<GUser>();
            if (string.IsNullOrWhiteSpace(clientID))
            {
                return new MultiMessage<GUser>(new List<GUser>(), 0);
            }

            var temp = from u in context.USR_GUSER
                       join r in context.USR_ROLE on u.ROLE_ID equals r.ID
                       where (r.ROLE_CATEGORY == (short)RoleCategory.MaintainAdmin || r.ROLE_CATEGORY == (short)RoleCategory.MaintainMonitor) && u.VALID == 1 && u.CLIENT_ID == clientID
                       select u;

            foreach (var item in temp)
            {
                result.Result.Add(GUserUtility.GetModel(item));
            }

            return result;
        }


        public static MultiMessage<GUser> GetInstallStationUserByPage(PTMSEntities context, string clientID, int pageSize, int pageIndex)
        {
            MultiMessage<GUser> result = new MultiMessage<GUser>();
            if (string.IsNullOrWhiteSpace(clientID))
            {
                return new MultiMessage<GUser>(new List<GUser>(), 0);
            }
            int totalCount = 0;
            var temp = from u in context.USR_GUSER
                       join r in context.USR_ROLE on u.ROLE_ID equals r.ID
                       where (r.ROLE_CATEGORY == (short)RoleCategory.MaintainAdmin || r.ROLE_CATEGORY == (short)RoleCategory.MaintainMonitor) && u.VALID == 1 && u.CLIENT_ID == clientID
                       select u;
            var list = temp
                .Page(out totalCount, pageIndex, pageSize, true, t => t.CREATE_TIME, false);
            foreach (var item in list)
            {
                result.Result.Add(GUserUtility.GetModel(item));
            }
            result.TotalRecord = totalCount;
            return result;
        }
    }
}
