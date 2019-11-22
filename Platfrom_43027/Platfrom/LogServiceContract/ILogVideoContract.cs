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
    public interface ILogVideo
    {
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="model"></param>
        [OperationContract]
        SingleMessage<bool> InsertVideoDownloadLog(List<LogVideo> models);

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="model"></param>
        [OperationContract]
        SingleMessage<bool> InsertVideoPlayLog(List<LogVideo> models);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="model"></param>
        [OperationContract]
        SingleMessage<bool> ClearVideoDownloadLog(string ClientID, LogOperate log);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="model"></param>
        [OperationContract]
        SingleMessage<bool> ClearVideoPlayLog(string ClientID, LogOperate log);

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <returns>获取</returns>
        [OperationContract]
        MultiMessage<LogVideo> GetVideoPlayLogList(string clientID, string vehicleID, string userName, string mdvrCoreSN, DateTime? startTime, DateTime? endTime, int pageIndex, int pageSize);

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <returns>获取</returns>
        [OperationContract]
        MultiMessage<LogVideo> GetVideoDownloadLogList(string clientID, string vehicleID, string userName, string mdvrCoreSN, DateTime? startTime, DateTime? endTime, int pageIndex, int pageSize);

    }
}

