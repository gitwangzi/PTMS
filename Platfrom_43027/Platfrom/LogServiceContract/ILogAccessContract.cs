using Gsafety.PTMS.Base.Contract.Data;
using Gsafety.PTMS.Common.Data;
using LogServiceContract.Data;
using System;
using System.ServiceModel;

namespace LogServiceContract
{
    ///<summary>
    ///访问日志
    ///</summary>
    [ServiceContract]
    public interface ILogAccess
    {

        /// <summary>
        /// 添加访问日志
        /// </summary>
        /// <param name="model">访问日志</param>
        [OperationContract]
        SingleMessage<bool> InsertLogAccess(LogAccess model);

        /// <summary>
        /// 修改访问日志
        /// </summary>
        /// <param name="model">访问日志</param>
        [OperationContract]
        SingleMessage<bool> UpdateLogAccess(LogAccess model);

        /// <summary>
        /// 删除访问日志
        /// </summary>
        /// <param name="ID">要删除的日志编号</param>
        [OperationContract]
        SingleMessage<bool> DeleteLogAccessByID(string ID);

        /// <summary>
        /// 获取访问日志
        /// </summary>
        /// <returns>获取访问日志</returns>
        [OperationContract]
        MultiMessage<LogAccess> GetLogAccessList(PagingInfo page, string clientID);
        /// <summary>
        /// 获取访问日志列表
        /// </summary>
        /// <returns>获取访问日志</returns>
        [OperationContract]
        MultiMessage<LogAccess> GetLogAccessListByCondition(PagingInfo page, string clientID,string logName,DateTime? beginTime,DateTime? endTime);

    }
}
