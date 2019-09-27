using Gsafety.PTMS.Base.Contract.Data;
using Gsafety.PTMS.Common.Data;
using Gsafety.PTMS.Manager.Contract.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.ServiceModel;

namespace Gsafety.PTMS.Manager.Contract
{
    ///<summary>
    ///角色表
    ///</summary>
    [ServiceContract]
    public interface IRoleService
    {
        /// <summary>
        /// 添加角色表
        /// </summary>
        /// <param name="model">角色表</param>
        [OperationContract]
        SingleMessage<bool> InsertRole(Role model, LogOperate logOperate);

        /// <summary>
        /// 修改角色表
        /// </summary>
        /// <param name="model">角色表</param>
        [OperationContract]
        SingleMessage<bool> UpdateRole(Role model, LogOperate logOperate);

        /// <summary>
        /// 删除角色表
        /// </summary>
        /// <param name="model">角色表</param>
        [OperationContract]
        SingleMessage<bool> DeleteRoleByID(string ID, LogOperate logOperate);

        /// <summary>
        /// 获取角色表列表
        /// </summary>
        /// <returns>获取角色表</returns>
        [OperationContract]
        MultiMessage<Role> GetRoleList(string clientID, string roleName, int pageIndex, int pageSize);

        [OperationContract]
        SingleMessage<bool> UpdateRoleFunItems(string roleID, List<string> funItemIDs, LogOperate logOperate);

        [OperationContract]
        MultiMessage<string> GetFunItemsByRoleID(string roleID);

        [OperationContract]
        MultiMessage<FuncItem> GetAllFunItems();
    }
}

