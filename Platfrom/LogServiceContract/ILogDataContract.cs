using Gsafety.PTMS.Base.Contract.Data;
using LogServiceContract.Data;
using System;
using System.ServiceModel;

namespace LogServiceContract
{
    ///<summary>
    ///视频日志
    ///</summary>
    [ServiceContract]
    public interface ILogData
    {

        /// <summary>
        /// 添加视频日志
        /// </summary>
        /// <param name="model">视频日志</param>
        [OperationContract]
        SingleMessage<bool> InsertLogData(LogData model);

        /// <summary>
        /// 修改视频日志
        /// </summary>
        /// <param name="model">视频日志</param>
        [OperationContract]
        SingleMessage<bool> UpdateLogData(LogData model);

        /// <summary>
        /// 删除视频日志
        /// </summary>
        /// <param name="model">视频日志</param>
        [OperationContract]
        SingleMessage<bool> DeleteLogDataByID(string ID);

        /// <summary>
        /// 获取视频日志
        /// </summary>
        /// <returns>获取视频日志</returns>
        [OperationContract]
        MultiMessage<LogData> GetLogDataList(PagingInfo page, string clientID);
        /// <summary>
        /// 获取视频日志列表
        /// </summary>
        /// <returns>获取视频日志</returns>
        [OperationContract]
        MultiMessage<LogData> GetLogDataListByCondition(PagingInfo page, string clientID, string logName, DateTime? beginTime, DateTime? endTime);

    }
}

