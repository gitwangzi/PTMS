using Gsafety.PTMS.Base.Contract.Data;
using System.ServiceModel;
namespace Gsafety.PTMS.BaseInformation.Contract
{
    ///<summary>
    ///组织机构表
    ///</summary>
    [ServiceContract]
    public interface IOrganization
    {

        /// <summary>
        /// 添加组织机构表
        /// </summary>
        /// <param name="model">组织机构表</param>
        [OperationContract]
        SingleMessage<bool> InsertOrganization(Organization model, string userID);

        /// <summary>
        /// 修改组织机构表
        /// </summary>
        /// <param name="model">组织机构表</param>
        [OperationContract]
        SingleMessage<bool> UpdateOrganization(Organization model);

        /// <summary>
        /// 删除组织机构表
        /// </summary>
        /// <param name="model">组织机构表</param>
        [OperationContract]
        SingleMessage<bool> DeleteOrganization(string Id, string userid);

        /// <summary>
        /// 获取组织机构表
        /// </summary>
        /// <returns>获取组织机构表列表</returns>
        [OperationContract]
        MultiMessage<Organization> GetOrganizationByUser(string userid);

        [OperationContract]
        MultiMessage<Organization> GetAllOrganization(string clientID);
    }
}

