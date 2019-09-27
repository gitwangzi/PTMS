using Gsafety.PTMS.Base.Contract.Data;
using Gsafety.PTMS.Common.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
namespace LogServiceContract
{
    ///<summary>
    ///
    ///</summary>
    [ServiceContract]
    public interface ILogOperate
    {

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <returns>获取</returns>
        [OperationContract]
        MultiMessage<LogOperate> GetOperationLogList(string userName,string clientID, DateTime? startTime, DateTime? endTime, int pageIndex, int pageSize);

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <returns>获取</returns>
        [OperationContract]
        SingleMessage<bool> ClearOperateLog(string clientID,LogOperate log);

    }
}

