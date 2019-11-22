using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using Gsafety.PTMS.Base.Contract.Data;
using Gsafety.PTMS.Manager.Contract.Data;
using Gsafety.PTMS.Common.Data;
namespace Gsafety.PTMS.Manager.Contract
{
    ///<summary>
    ///超速表
    ///</summary>
    [ServiceContract]
    public interface ISpeedLimit
    {

        /// <summary>
        /// 添加超速表
        /// </summary>
        /// <param name="model">超速表</param>
        [OperationContract]
        SingleMessage<bool> InsertSpeedLimit(SpeedLimit model);

        /// <summary>
        /// 修改超速表
        /// </summary>
        /// <param name="model">超速表</param>
        [OperationContract]
        SingleMessage<bool> UpdateSpeedLimit(SpeedLimit model);

        /// <summary>
        /// 删除超速表
        /// </summary>
        /// <param name="model">超速表</param>
        [OperationContract]
        SingleMessage<bool> DeleteSpeedLimitByID(string ID);

        /// <summary>
        /// 获取超速表列表
        /// </summary>
        /// <returns>获取超速表</returns>
        [OperationContract]
        MultiMessage<SpeedLimit> GetSpeedLimitListByName(string clientID, string name, int pageIndex, int pageSize);

    }
}

