using Gsafety.PTMS.Base.Contract.Data;
using Gsafety.PTMS.BaseInfo;
using Gsafety.PTMS.BaseInformation.Repository;
using Gsafety.PTMS.Common.Data;
using Gsafety.PTMS.Common.Data.Enum;
using Gsafety.PTMS.DBEntity;
using Gsafety.PTMS.Manager.Contract;
using Gsafety.PTMS.Manager.Contract.Data;
using Gsafety.PTMS.Manager.Repository;
using Gsafety.PTMS.Manager.Repository.UserManage;
using Gsafety.PTMS.Message.Contract;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Transactions;

namespace Gs.PTMS.Service
{
    // 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码、svc 和配置文件中的类名“AccountService”。
    // 注意: 为了启动 WCF 测试客户端以测试此服务，请在解决方案资源管理器中选择 AccountService.svc 或 AccountService.svc.cs，然后开始调试。
    public class AccountService : BaseService, IUserService, IRoleService, IUsrDepartment, IUserOnline
    {
        #region User
        string clientadminrole = "E5713BCC-A8A7-4CB3-A8AE-03292B67D52D";
        public SingleMessage<bool> InsertGUser(GUser model, LogOperate logOperate)
        {
            try
            {
                Info("InsertGUser");
                Info("model:" + Convert.ToString(model));
                model.CreateTime = DateTime.Now.ToUniversalTime();
                SingleMessage<bool> result = null;
                using (PTMSEntities context = new PTMSEntities())
                {
                    TransactionOptions optons = new TransactionOptions();
                    optons.IsolationLevel = IsolationLevel.ReadCommitted;
                    var scope = new TransactionScope(TransactionScopeOption.Required, optons);
                    try
                    {
                        result = GUserRepository.Insert(context, model);
                        if (result.IsSuccess)
                        {
                            logOperate.OperateType = (short)OperateTypeEnum.UserManage;
                            AddOperateLog(logOperate, context);

                            result = context.Save();
                            if (result.IsSuccess)
                            {
                                scope.Complete();
                            }
                        }
                    }
                    finally
                    {
                        scope.Dispose();
                    }
                }

                Log<bool>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new SingleMessage<bool>(false, ex);
            }
        }

        private static void AddOperateLog(LogOperate logOperate, PTMSEntities context)
        {
            LOG_OPERATE entity = new LOG_OPERATE();
            entity.ID = Guid.NewGuid().ToString();
            entity.CLIENT_ID = logOperate.ClientID;
            if (logOperate.OperateContent.Length < 2000)
            {
                entity.OPERATE_CONTENT = logOperate.OperateContent;
            }
            else
            {
                entity.OPERATE_CONTENT = logOperate.OperateContent.Substring(0, 1999);
            }
            entity.OPERATE_TIME = DateTime.Now.ToUniversalTime();
            entity.OPERATOR_ID = logOperate.OperatorID;
            entity.OPERATOR_NAME = logOperate.OperatorName;
            entity.OPERATE_TYPE = (short)logOperate.OperateType;
            context.LOG_OPERATE.Add(entity);
        }

        public SingleMessage<bool> UpdateGUser(GUser model, LogOperate logOperate)
        {
            try
            {
                Info("UpdateGUser");
                Info("model:" + Convert.ToString(model));
                SingleMessage<bool> result = null;
                using (PTMSEntities context = new PTMSEntities())
                {
                    TransactionOptions optons = new TransactionOptions();
                    optons.IsolationLevel = IsolationLevel.ReadCommitted;
                    var scope = new TransactionScope(TransactionScopeOption.Required, optons);
                    try
                    {
                        result = GUserRepository.Update(context, model);
                        if (result.IsSuccess)
                        {
                            if (logOperate != null)
                            {
                                logOperate.OperateType = (short)OperateTypeEnum.UserManage;
                                AddOperateLog(logOperate, context);
                            }

                            if (result.IsSuccess)
                            {
                                scope.Complete();
                            }

                        }
                    }
                    finally
                    {
                        scope.Dispose();
                    }
                }

                Log<bool>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new SingleMessage<bool>(false, ex);
            }
        }

        public SingleMessage<bool> UpdateDepartment(string userID, string departmentID)
        {
            try
            {
                Info("UpdateDepartment");
                Info("userID:" + userID);
                Info("departmentID:" + departmentID);
                SingleMessage<bool> result = null;
                using (PTMSEntities context = new PTMSEntities())
                {
                    result = GUserRepository.UpdateDepartment(context, userID, departmentID);
                }

                Log<bool>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new SingleMessage<bool>(false, ex);
            }
        }


        public SingleMessage<bool> DeleteGUser(string userID, LogOperate logOperate)
        {
            try
            {
                Info("DeleteGUser");
                Info("userID:" + userID);
                SingleMessage<bool> result = null;
                using (PTMSEntities context = new PTMSEntities())
                {
                    TransactionOptions optons = new TransactionOptions();
                    optons.IsolationLevel = IsolationLevel.ReadCommitted;
                    var scope = new TransactionScope(TransactionScopeOption.Required, optons);
                    try
                    {
                        result = GUserRepository.DeleteByUserID(context, userID);
                        if (result.IsSuccess)
                        {
                            logOperate.OperateType = (short)OperateTypeEnum.UserManage;
                            AddOperateLog(logOperate, context);
                            result = context.Save();
                            if (result.IsSuccess)
                            {
                                scope.Complete();
                            }
                        }
                    }
                    finally
                    {
                        scope.Dispose();
                    }
                }

                Log<bool>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new SingleMessage<bool>(false, ex);
            }
        }

        public MultiMessage<GUser> GetGUserList(string clientID, string userDepartmentID, string qureyUserName, int pageIndex = 1, int pageSize = 10)
        {
            try
            {
                Info("GetGUser");
                MultiMessage<GUser> result = null;
                using (PTMSEntities context = new PTMSEntities())
                {
                    result = GUserRepository.GetGUserList(context, clientID, userDepartmentID, qureyUserName, pageIndex, pageSize);
                }

                Log<GUser>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new MultiMessage<GUser>(false, ex);
            }
        }

        public SingleMessage<AccountInfo> GetAccountInfo(string account, string password)
        {
            try
            {
                Info("GetAccountInfo");
                Info("model:" + Convert.ToString(account));
                Info("password:" + Convert.ToString(password));
                SingleMessage<AccountInfo> result = new SingleMessage<AccountInfo>();
                result.Result = new AccountInfo();
                using (PTMSEntities context = new PTMSEntities())
                {
                    var userResult = GUserRepository.GetGUser(context, account);
                    if (userResult.IsSuccess == false)
                    {
                        return new SingleMessage<AccountInfo>(false, userResult.ErrorMsg);
                    }

                    var user = userResult.Result;
                    if (user.Password == password)
                    {
                        result.Result.User = user;
                        result.Result.Role = RoleRepository.GetRoleByID(context, user.RoleID).Result;
                        if (result.Result.Role.RoleCategory == (int)RoleCategory.SuperPower || result.Result.Role.RoleCategory == (int)RoleCategory.ClientAdmin)
                        {
                            result.Result.Allowed = true;
                        }
                        else
                        {
                            SingleMessage<OrderClient> orderclient = OrderClientRepository.GetOrderClient(context, userResult.Result.ClientID);
                            if (orderclient.Result.Status == StatusEnum.Stop)
                            {
                                result.Result.Allowed = false;
                                result.ErrorMsg = "Suspended";

                                return result;
                            }
                            else if (orderclient.Result.EndTime < DateTime.Now.ToUniversalTime())
                            {
                                result.Result.Allowed = false;
                                result.ErrorMsg = "Expired";
                                return result;
                            }
                            else
                            {
                                int count = context.RUN_USER_ONLINE.Count(n => n.CLIENT_ID == userResult.Result.ClientID);
                                if (count >= orderclient.Result.UserCount)
                                {
                                    bool found = context.RUN_USER_ONLINE.Any(n => n.USER_ID == userResult.Result.ID);
                                    if (!found)
                                    {
                                        result.Result.Allowed = false;
                                        result.ErrorMsg = "MaxUser";
                                        return result;
                                    }
                                    else
                                    {
                                        result.Result.Allowed = true;
                                    }
                                }
                                else
                                {
                                    result.Result.Allowed = true;
                                    result.Result.TransferMode = (short)orderclient.Result.TansferMode;
                                }
                            }
                        }
                    }
                    else
                    {
                        result.Result.Allowed = false;
                        result.ErrorMsg = "PasswordError";
                        result.IsSuccess = false;
                        return result;
                    }
                }

                Log<AccountInfo>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new SingleMessage<AccountInfo>(false, ex);
            }
        }

        public SingleMessage<GUser> GetUserByAccoutName(string account)
        {
            try
            {
                Info("GetUserByAccoutName");
                SingleMessage<GUser> result = null;
                using (PTMSEntities context = new PTMSEntities())
                {
                    result = GUserRepository.GetGUser(context, account);
                }

                Log<GUser>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new SingleMessage<GUser>(false, ex);
            }
        }

        public SingleMessage<bool> IsUserNameExist(string userName, string userID)
        {
            Info("IsUserNameExist");
            Info(userName);
            try
            {
                SingleMessage<bool> result;
                using (var context = new PTMSEntities())
                {
                    result = GUserRepository.IsUserNameExist(context, userName, userID);
                }
                Log<bool>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new SingleMessage<bool>(false, ex);
            }
        }

        public SingleMessage<GUser> GetOrderClientGUserByClientID(string ID)
        {
            Info("GetOrderClientGUserByClientID");
            Info(ID.ToString());
            try
            {
                SingleMessage<GUser> result = null;
                using (var context = new PTMSEntities())
                {
                    result = GUserRepository.GetOrderClientGUserByClientID(context, ID);
                }
                Log<GUser>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new SingleMessage<GUser>(false, ex);
            }
        }

        public SingleMessage<bool> InsertOrderClientSystemAdmin(GUser model)
        {
            try
            {
                Info("InsertOrderClientSystemAdmin");
                Info("model:" + Convert.ToString(model));
                model.CreateTime = DateTime.Now.ToUniversalTime();
                SingleMessage<bool> result = null;
                using (PTMSEntities context = new PTMSEntities())
                {
                    model.RoleID = clientadminrole;
                    result = GUserRepository.Insert(context, model);
                    if (result.IsSuccess && result.Result)
                    {
                        return context.Save();
                    }
                }

                Log<bool>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new SingleMessage<bool>(false, ex);
            }
        }

        public SingleMessage<bool> ModifyPassword(string userID, string password, LogOperate logOperate)
        {
            try
            {
                Info("ModifyPassword");
                Info("userID:" + Convert.ToString(userID));
                Info("password:" + password);
                SingleMessage<bool> result = null;
                using (PTMSEntities context = new PTMSEntities())
                {
                    TransactionOptions optons = new TransactionOptions();
                    optons.IsolationLevel = IsolationLevel.ReadCommitted;
                    var scope = new TransactionScope(TransactionScopeOption.Required, optons);
                    try
                    {
                        result = GUserRepository.ModifyPassword(context, userID, password);
                        if (result.IsSuccess)
                        {
                            logOperate.OperateType = (short)OperateTypeEnum.UserManage;
                            AddOperateLog(logOperate, context);
                            result = context.Save();
                            if (result.IsSuccess)
                            {
                                scope.Complete();
                            }
                        }
                    }
                    finally
                    {
                        scope.Dispose();
                    }
                }

                Log<bool>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new SingleMessage<bool>(false, ex);
            }
        }

        public MultiMessage<GUser> GetInstallStationUser(string clientID)
        {
            Info("GetInstallStationUser");
            Info("clientID:" + clientID);
            try
            {
                MultiMessage<GUser> result = null;
                using (var context = new PTMSEntities())
                {
                    result = GUserRepository.GetInstallStationUser(context, clientID);
                    result.IsSuccess = true;
                }
                Log<GUser>(result);

                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new MultiMessage<GUser>(false, ex);
            }
        }


        public MultiMessage<GUser> GetInstallStationUserByPage(string clientID, int pageSize, int pageIndex)
        {
            Info("GetInstallStationUserByPage");
            Info("clientID:" + clientID);
            try
            {
                MultiMessage<GUser> result = null;
                using (var context = new PTMSEntities())
                {
                    result = GUserRepository.GetInstallStationUserByPage(context, clientID, pageSize, pageIndex);
                    result.IsSuccess = true;
                }
                Log<GUser>(result);

                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new MultiMessage<GUser>(false, ex);
            }
        }
        #endregion

        #region Role

        public SingleMessage<bool> InsertRole(Role model, LogOperate logOperate)
        {
            try
            {
                Info("InsertRole");
                Info("model:" + Convert.ToString(model));
                SingleMessage<bool> result = null;
                using (PTMSEntities context = new PTMSEntities())
                {
                    TransactionOptions optons = new TransactionOptions();
                    optons.IsolationLevel = IsolationLevel.ReadCommitted;
                    var scope = new TransactionScope(TransactionScopeOption.Required, optons);
                    try
                    {
                        model.CreateTime = DateTime.Now.ToUniversalTime();
                        result = RoleRepository.InsertRole(context, model);
                        if (result.IsSuccess)
                        {
                            logOperate.OperateType = (short)OperateTypeEnum.RoleManage;
                            AddOperateLog(logOperate, context);
                            result = context.Save();
                            if (result.IsSuccess)
                            {
                                scope.Complete();
                            }
                        }
                    }
                    finally
                    {
                        scope.Dispose();
                    }
                }

                Log<bool>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new SingleMessage<bool>(false, ex);
            }
        }

        public SingleMessage<bool> UpdateRole(Role model, LogOperate logOperate)
        {
            try
            {
                Info("UpdateRole");
                Info("model:" + Convert.ToString(model));
                SingleMessage<bool> result = null;
                using (PTMSEntities context = new PTMSEntities())
                {
                    TransactionOptions optons = new TransactionOptions();
                    optons.IsolationLevel = IsolationLevel.ReadCommitted;
                    var scope = new TransactionScope(TransactionScopeOption.Required, optons);
                    try
                    {
                        result = RoleRepository.UpdateRole(context, model);
                        if (result.IsSuccess)
                        {
                            logOperate.OperateType = (short)OperateTypeEnum.RoleManage;
                            AddOperateLog(logOperate, context);
                            result = context.Save();
                            if (result.IsSuccess)
                            {
                                scope.Complete();
                            }
                        }
                    }
                    finally
                    {
                        scope.Dispose();
                    }
                }

                Log<bool>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new SingleMessage<bool>(false, ex);
            }
        }

        public SingleMessage<bool> DeleteRoleByID(string ID, LogOperate logOperate)
        {
            try
            {
                Info("DeleteRoleByID");
                Info("ID:" + ID);
                SingleMessage<bool> result = null;
                using (PTMSEntities context = new PTMSEntities())
                {
                    TransactionOptions optons = new TransactionOptions();
                    optons.IsolationLevel = IsolationLevel.ReadCommitted;
                    var scope = new TransactionScope(TransactionScopeOption.Required, optons);
                    try
                    {
                        result = RoleRepository.DeleteRoleByID(context, ID);
                        if (result.IsSuccess)
                        {
                            logOperate.OperateType = (short)OperateTypeEnum.RoleManage;
                            AddOperateLog(logOperate, context);
                            result = context.Save();
                            if (result.IsSuccess)
                            {
                                scope.Complete();
                            }
                        }
                    }
                    finally
                    {
                        scope.Dispose();
                    }
                }

                Log<bool>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new SingleMessage<bool>(false, ex);
            }
        }

        public MultiMessage<Role> GetRoleList(string clientID, string roleName, int pageIndex, int pageSize)
        {
            try
            {
                Info("GetRoleList");
                Info("pageIndex:" + pageIndex);
                Info("pageSize:" + pageSize);
                MultiMessage<Role> result = null;
                using (PTMSEntities context = new PTMSEntities())
                {
                    result = RoleRepository.GetRoleList(context, clientID, roleName, pageIndex, pageSize);
                }

                Log<Role>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new MultiMessage<Role>(false, ex);
            }
        }

        public SingleMessage<bool> UpdateRoleFunItems(string roleID, List<string> funItemIDs, LogOperate logOperate)
        {
            try
            {
                Info("UpdateRoleFunItems");
                Info("roleID:" + roleID);
                Info("funItemIDs", funItemIDs);
                SingleMessage<bool> result = null;
                using (PTMSEntities context = new PTMSEntities())
                {
                    TransactionOptions optons = new TransactionOptions();
                    optons.IsolationLevel = IsolationLevel.ReadCommitted;
                    var scope = new TransactionScope(TransactionScopeOption.Required, optons);
                    try
                    {
                        result = RoleRepository.UpdateRoleFunItems(context, roleID, funItemIDs);
                        if (result.IsSuccess)
                        {
                            logOperate.OperateType = (short)OperateTypeEnum.RoleManage;
                            AddOperateLog(logOperate, context);
                            result = context.Save();
                            if (result.IsSuccess)
                            {
                                scope.Complete();
                            }
                        }
                    }
                    finally
                    {
                        scope.Dispose();
                    }
                }

                Log<bool>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new SingleMessage<bool>(false, ex);
            }
        }

        public MultiMessage<string> GetFunItemsByRoleID(string roleID)
        {
            try
            {
                Info("GetFunItemsByRoleID");
                Info("roleID:" + roleID);
                MultiMessage<string> result = null;
                using (PTMSEntities context = new PTMSEntities())
                {
                    result = RoleRepository.GetFunItemsByRoleID(context, roleID);
                }

                Log<string>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new MultiMessage<string>(false, ex);
            }
        }

        public MultiMessage<FuncItem> GetAllFunItems()
        {
            try
            {
                Info("GetAllFunItems");
                MultiMessage<FuncItem> result = null;
                using (PTMSEntities context = new PTMSEntities())
                {
                    result = FuncItemRepository.GetAllItems(context);
                }

                Log<FuncItem>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new MultiMessage<FuncItem>(false, ex);
            }
        }

        #endregion

        #region UsrDepartment
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="model"></param>
        public SingleMessage<bool> InsertUsrDepartment(UsrDepartment model)
        {
            Info("InsertUsrDepartment");
            Info(model.ToString());
            model.CreateTime = DateTime.Now.ToUniversalTime();
            try
            {
                SingleMessage<bool> result = null;

                using (var context = new PTMSEntities())
                {
                    result = UsrDepartmentRepository.InsertUsrDepartment(context, model);
                }
                Log<bool>(result);
                return result;
            }
            catch (DbEntityValidationException ex)
            {
                Error(ex);
                return new SingleMessage<bool>(false, ex);
            }

            catch (Exception ex)
            {
                Error(ex);
                return new SingleMessage<bool>(false, ex);
            }
        }

        /// <summary>
        /// 修改
        /// </summary>
        public SingleMessage<bool> UpdateUsrDepartment(UsrDepartment model)
        {
            Info("UpdateUsrDepartment");
            Info(model.ToString());
            try
            {
                SingleMessage<bool> result = null;
                using (var context = new PTMSEntities())
                {
                    result = UsrDepartmentRepository.UpdateUsrDepartment(context, model);
                }
                Log<bool>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new SingleMessage<bool>(false, ex);
            }
        }

        /// <summary>
        /// 删除
        /// </summary>
        public SingleMessage<bool> DeleteUsrDepartmentByID(string ID)
        {
            Info("DeleteUsrDepartmentByID");
            Info(ID.ToString());
            try
            {
                SingleMessage<bool> result = null;
                using (var context = new PTMSEntities())
                {
                    result = UsrDepartmentRepository.DeleteUsrDepartmentByID(context, ID);
                }
                Log<bool>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new SingleMessage<bool>(false, ex);
            }
        }

        /// <summary>
        /// 是否可以删除的标志
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public SingleMessage<bool> IsCanDeleteUsrDepartment(string ID)
        {
            Info("IsCanDeleteUsrDepartment");
            Info(ID.ToString());
            try
            {
                SingleMessage<bool> result = null;
                using (var context = new PTMSEntities())
                {
                    result = UsrDepartmentRepository.IsCanDeleteUserDepartmentById(context, ID);
                }
                Log<bool>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new SingleMessage<bool>(false, ex);
            }
        }


        /// <summary>
        /// 获取
        /// </summary>
        public SingleMessage<UsrDepartment> GetUsrDepartment(string ID)
        {
            Info("GetUsrDepartment");
            Info(ID.ToString());
            try
            {
                SingleMessage<UsrDepartment> result = null;
                using (var context = new PTMSEntities())
                {
                    result = UsrDepartmentRepository.GetUsrDepartment(context, ID);
                }
                Log<UsrDepartment>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new SingleMessage<UsrDepartment>(false, ex);
            }
        }
        /// <summary>
        /// 获取
        /// </summary>
        public MultiMessage<UsrDepartment> GetUsrDepartmentList(int pageIndex, int pageSize)
        {
            Info("GetUsrDepartmentList");
            Info(pageIndex.ToString());
            Info(pageSize.ToString());
            try
            {
                MultiMessage<UsrDepartment> result = null;
                using (var context = new PTMSEntities())
                {
                    result = UsrDepartmentRepository.GetUsrDepartmentList(context, "", "", pageIndex, pageSize);
                }
                Log<UsrDepartment>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new MultiMessage<UsrDepartment>(false, ex);
            }
        }

        /// <summary>
        /// 获取用户组织机构列表
        /// </summary>
        /// <returns></returns>
        public MultiMessage<UsrDepartment> GetUserDepartmentList(string name, string clientId)
        {
            Info("GetUsrDepartmentList");
            try
            {
                MultiMessage<UsrDepartment> result = null;
                using (var context = new PTMSEntities())
                {
                    result = UsrDepartmentRepository.GetUsrDepartmentList(context, name, clientId, 1, -1);
                }
                Log<UsrDepartment>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new MultiMessage<UsrDepartment>(false, ex);
            }
        }


        #endregion

        public MultiMessage<Gsafety.PTMS.Common.Data.UserOnline> GetUserOnlineList(string clientID, string userName, int pageIndex, int pageSize)
        {
            Info("GetUserOnlineList");
            Info(clientID.ToString());
            Info(pageIndex.ToString());
            Info(pageSize.ToString());
            try
            {
                MultiMessage<UserOnline> result = null;
                using (var context = new PTMSEntities())
                {
                    result = UserOnlineRepository.GetUserOnlineList(context, clientID, userName, pageIndex, pageSize);
                }
                Log<UserOnline>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new MultiMessage<UserOnline>(false, ex);
            }
        }

        public MultiMessage<Gsafety.PTMS.Message.Contract.OrganizationUser> GetUserVehicleOrg(string userID)
        {
            Info("GetUserVehicleOrg");
            Info(userID.ToString());

            try
            {
                MultiMessage<OrganizationUser> result = null;
                using (var context = new PTMSEntities())
                {
                    result = OrganizationUserRepository.GetOrganizationUserList(context, userID);
                }
                Log<OrganizationUser>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new MultiMessage<OrganizationUser>(false, ex);
            }
        }

        public SingleMessage<bool> InsertUserVehicleOrg(List<string> orgIds, string userID)
        {
            Info("InsertUserVehicleOrg");
            Info(userID);
            foreach (var item in orgIds)
            {
                Info(item);
            }

            try
            {
                SingleMessage<bool> result = null;

                result = OrganizationUserRepository.InsertOrganizationUser(orgIds, userID);

                Log<bool>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new SingleMessage<bool>(false, ex);
            }
        }
    }
}
