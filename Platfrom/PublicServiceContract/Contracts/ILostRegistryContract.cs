using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ComponentModel.DataAnnotations;
using Gsafety.PTMS.Base.Contract.Data;

namespace Gsafety.PTMS.PublicService.Contract
{
    ///<summary>
    ///丢失登记
    ///</summary>
    [ServiceContract]
    public interface ILostRegistry
    {

        /// <summary>
        /// 添加丢失登记
        /// </summary>
        /// <param name="model">丢失登记</param>
        [OperationContract]
        SingleMessage<bool> InsertLostRegistry(LostRegistry model);

        /// <summary>
        /// 修改丢失登记
        /// </summary>
        /// <param name="model">丢失登记</param>
        [OperationContract]
        SingleMessage<bool> UpdateLostRegistry(LostRegistry model);

        /// <summary>
        /// 删除丢失登记
        /// </summary>
        /// <param name="model">丢失登记</param>
        [OperationContract]
        SingleMessage<bool> DeleteLostRegistryByID(string ID);

        /// <summary>
        /// 获取丢失登记
        /// </summary>
        /// <returns>获取丢失登记</returns>
        [OperationContract]
        SingleMessage<LostRegistry> GetLostRegistry(string ID);
        /// <summary>
        /// 获取丢失登记列表
        /// </summary>
        /// <returns>获取丢失登记</returns>
        [OperationContract]
        MultiMessage<LostRegistry> GetLostRegistryList(PagingInfo page, string clientID);

        [OperationContract]
        MultiMessage<LostRegistry> GetLostRegistryByConditionList(PagingInfo page, string clientID, string LostIDCard, string Keyword, string LostName);

    }
}

