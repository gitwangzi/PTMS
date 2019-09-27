using Gsafety.PTMS.Base.Contract.Data;
using Gsafety.PTMS.Common.Data;
using Gsafety.PTMS.Manager.Contract.Data;
using Gsafety.PTMS.Message.Contract;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Gsafety.PTMS.Manager.Contract
{
    [ServiceContract]
    public interface IUserService
    {
        /// <summary>
        /// 添加用户表
        /// </summary>
        /// <param name="model">用户表Model</param>
        [OperationContract]
        SingleMessage<bool> InsertGUser(GUser model, LogOperate logOperate);

        /// <summary>
        /// 添加用户表
        /// </summary>
        /// <param name="model">用户表Model</param>
        [OperationContract]
        SingleMessage<bool> InsertOrderClientSystemAdmin(GUser model);

        /// <summary>
        /// 修改用户表
        /// </summary>
        /// <param name="model">用户表Model</param>
        [OperationContract]
        SingleMessage<bool> UpdateGUser(GUser model, LogOperate logOperate);

        /// <summary>
        /// 删除用户表
        /// </summary>
        [OperationContract]
        SingleMessage<bool> DeleteGUser(string userID, LogOperate logOperate);

        [OperationContract]
        SingleMessage<GUser> GetUserByAccoutName(string account);

        /// <summary>
        /// 获取用户表
        /// </summary>
        /// <returns>获取用户表列表</returns>
        [OperationContract]
        MultiMessage<GUser> GetGUserList(string clientID, string userDepartmentID, string qureyUserName, int pageIndex = 1, int pageSize = 10);

        [OperationContract]
        SingleMessage<AccountInfo> GetAccountInfo(string account, string password);

        [OperationContract]
        SingleMessage<bool> IsUserNameExist(string userName, string userID);

        /// <summary>
        /// 获取用户表
        /// </summary>
        /// <returns>获取用户表</returns>
        [OperationContract]
        SingleMessage<GUser> GetOrderClientGUserByClientID(string clientid);

        /// <summary>
        /// 重置密码
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        [OperationContract]
        SingleMessage<bool> ModifyPassword(string userID, string password, LogOperate logOperate);

        /// <summary>
        /// 修改用户部门
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="departmentID"></param>
        /// <returns></returns>
        [OperationContract]
        SingleMessage<bool> UpdateDepartment(string userID, string departmentID);

        /// <summary>
        /// Check Install Station Exist
        /// </summary>
        /// <param name="installStationList"></param>
        /// <returns></returns>
        [OperationContract]
        MultiMessage<GUser> GetInstallStationUser(string clientID);



        [OperationContract]
        MultiMessage<OrganizationUser> GetUserVehicleOrg(string userID);

        [OperationContract]
        SingleMessage<bool> InsertUserVehicleOrg(List<string> orgIds, string userID);

        [OperationContract]
        MultiMessage<GUser> GetInstallStationUserByPage(string clientID, int pageSize, int pageIndex);
    }
}
