using Gsafety.PTMS.Base.Contract.Data;
using Gsafety.PTMS.Common.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;

namespace Gsafety.PTMS.Manager.Contract
{
    ///<summary>
    ///心跳规则
    ///</summary>
    [ServiceContract]
    public interface IHeartbeatRule
    {

        /// <summary>
        /// 添加心跳规则
        /// </summary>
        /// <param name="model">心跳规则</param>
        [OperationContract]
        SingleMessage<bool> InsertHeartbeatRule(HeartbeatRule model);

        /// <summary>
        /// 修改心跳规则
        /// </summary>
        /// <param name="model">心跳规则</param>
        [OperationContract]
        SingleMessage<bool> UpdateHeartbeatRule(HeartbeatRule model);

        /// <summary>
        /// 删除心跳规则
        /// </summary>
        /// <param name="model">心跳规则</param>
        [OperationContract]
        SingleMessage<bool> DeleteHeartbeatRuleByID(string ID);

        /// <summary>
        /// 获取心跳规则列表
        /// </summary>
        /// <returns>获取心跳规则</returns>
        [OperationContract]
        MultiMessage<HeartbeatRule> GetHeartbeatRuleList(int pageIndex, int pageSize, string clientid, string name);

    }
}

