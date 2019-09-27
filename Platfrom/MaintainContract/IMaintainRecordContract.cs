using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using Gsafety.PTMS.Base.Contract.Data;
using Gsafety.PTMS.Common.Data;
namespace Gsafety.PTMS.Maintain.Contract
{
    ///<summary>
    ///
    ///</summary>
    [ServiceContract]
    public interface IMaintainRecord
    {
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="model"></param>
        [OperationContract]
        SingleMessage<bool> InsertMaintainRecord(MaintainRecord model);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="model"></param>
        [OperationContract]
        SingleMessage<bool> UpdateMaintainRecord(MaintainRecord model);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="model"></param>
        [OperationContract]
        SingleMessage<bool> DeleteMaintainRecordByID(string ID);

        /// <summary>
        /// 获取
        /// </summary>
        /// <returns>获取</returns>
        [OperationContract]
        SingleMessage<MaintainRecord> GetMaintainRecord(string ID);
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <returns>获取</returns>
        [OperationContract]
        MultiMessage<MaintainRecord> GetMaintainRecordList(string clientID, string worker, DateTime? beginTime, DateTime? endTime, int pageIndex, int pageSize);

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <returns>获取</returns>
        [OperationContract]
        MultiMessage<MaintainRecordUnfinished> GetMaintainRecordUnfinishedList(string clientID, string vehcileID, string contact, string worker, int pageIndex, int pageSize);

    }
}

