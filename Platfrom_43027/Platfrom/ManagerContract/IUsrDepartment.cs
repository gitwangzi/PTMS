using Gsafety.PTMS.Base.Contract.Data;
using Gsafety.PTMS.Manager.Contract.Data;
using System.ServiceModel;

namespace Gsafety.PTMS.Manager.Contract
{
    ///<summary>
    ///用户部门
    ///</summary>
    [ServiceContract]
    public interface IUsrDepartment
    {

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="model"></param>
        [OperationContract]
        SingleMessage<bool> InsertUsrDepartment(UsrDepartment model);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="model"></param>
        [OperationContract]
        SingleMessage<bool> UpdateUsrDepartment(UsrDepartment model);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="model"></param>
        [OperationContract]
        SingleMessage<bool> DeleteUsrDepartmentByID(string ID);

        /// <summary>
        /// 获取
        /// </summary>
        /// <returns>获取</returns>
        [OperationContract]
        SingleMessage<UsrDepartment> GetUsrDepartment(string ID);

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <returns>获取</returns>
        [OperationContract]
        MultiMessage<UsrDepartment> GetUsrDepartmentList(int pageIndex, int pageSize);

        /// <summary>
        /// 获取部门的树形结构
        /// </summary>
        /// <param name="name">查询条件名称</param>
        /// <param name="clientId">客户端编号</param>
        /// <returns></returns>
        [OperationContract]
        MultiMessage<UsrDepartment> GetUserDepartmentList(string name, string clientId);

        /// <summary>
        /// 是否能删除此部门
        /// 存在子集不能删除
        /// 存在直属用户不能删除
        /// </summary>
        /// <param name="ID">要删除的部门的编号</param>
        /// <returns></returns>
        [OperationContract]
        SingleMessage<bool> IsCanDeleteUsrDepartment(string ID);

    }
}
