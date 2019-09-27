using Gsafety.PTMS.Base.Contract.Data;
using Gsafety.PTMS.Common.Data;
using LogServiceContract.Data;
using System;
using System.ServiceModel;

namespace LogServiceContract
{
    ///<summary>
    ///外部访问日志
    ///</summary>
    [ServiceContract]
    public interface ILogManager
    {
        /// <summary>
        /// 获取外部访问日志
        /// </summary>
        /// <returns>获取外部访问日志</returns>
        [OperationContract]
        MultiMessage<LogManager> GetLogManagerList(PagingInfo page);
        /// <summary>
        /// 获取外部访问日志列表
        /// </summary>
        /// <returns>获取外部访问日志</returns>
        [OperationContract]
        MultiMessage<LogManager> GetLogManagerListByCondition(PagingInfo page, string logName, DateTime? beginTime, DateTime? endTime);

    }
}

