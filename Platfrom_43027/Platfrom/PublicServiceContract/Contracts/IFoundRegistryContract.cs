using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ComponentModel.DataAnnotations;
using Gsafety.PTMS.PublicService.Contract;
using Gsafety.PTMS.Base.Contract.Data;
using Gsafety.PTMS.Common.Data;

namespace Gsafety.PTMS.PublicService.Contract
{
    ///<summary>
    ///拾到物
    ///</summary>
    [ServiceContract]
    public interface IFoundRegistry
    {

        /// <summary>
        /// 添加拾到物
        /// </summary>
        /// <param name="model">拾到物</param>
        [OperationContract]
        SingleMessage<bool> InsertFoundRegistry(FoundRegistry model);

        /// <summary>
        /// 修改拾到物
        /// </summary>
        /// <param name="model">拾到物</param>
        [OperationContract]
        SingleMessage<bool> UpdateFoundRegistry(FoundRegistry model);

        /// <summary>
        /// 删除拾到物
        /// </summary>
        /// <param name="model">拾到物</param>
        [OperationContract]
        SingleMessage<bool> DeleteFoundRegistryByID(string ID);

        /// <summary>
        /// 获取拾到物
        /// </summary>
        /// <returns>获取拾到物</returns>
        [OperationContract]
        SingleMessage<FoundRegistry> GetFoundRegistry(string ID);
        /// <summary>
        /// 获取拾到物列表
        /// </summary>
        /// <returns>获取拾到物</returns>
        [OperationContract]
        MultiMessage<FoundRegistry> GetFoundRegistryList(PagingInfo page, string clientID);
        
        /// <returns>获取拾到物</returns>
        [OperationContract]
        MultiMessage<FoundRegistry> GetFoundRegistryByConditionList(PagingInfo page, string clientID, string Founder, string Keyword, string LostName);



    }
}

