using Gsafety.PTMS.Base.Contract.Data;
using Gsafety.PTMS.BaseInformation.Contract.Data;
using Gsafety.PTMS.Common.Data;
using Gsafety.PTMS.Common.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Gsafety.PTMS.BaseInformation.Contract
{
    ///<summary>
    ///GPS设备
    ///</summary>
    [ServiceContract]
    public interface IDevGpsService
    {

        /// <summary>
        /// 添加GPS设备
        /// </summary>
        /// <param name="model">GPS设备</param>
        [OperationContract]
        SingleMessage<bool> InsertDevGps(DevGps model);

        /// <summary>
        /// 修改GPS设备
        /// </summary>
        /// <param name="model">GPS设备</param>
        [OperationContract]
        SingleMessage<bool> UpdateDevGps(DevGps model);

        /// <summary>
        /// 删除GPS设备
        /// </summary>
        /// <param name="model">GPS设备</param>
        [OperationContract]
        SingleMessage<bool> DeleteDevGpsByID(string ID);

        /// <summary>
        /// 获取GPS设备
        /// </summary>
        /// <returns>获取GPS设备</returns>
        [OperationContract]
        SingleMessage<DevGps> GetDevGps(string ID);

        /// <summary>
        /// 获取GPS设备
        /// </summary>
        /// <returns>获取GPS设备</returns>
        [OperationContract]
        SingleMessage<DevGps> GetDevGpsBySN(string ID);
        /// <summary>
        /// 获取GPS设备列表
        /// </summary>
        /// <returns>获取GPS设备</returns>
        [OperationContract]
        MultiMessage<DevGps> GetDevGpsList(PagingInfo page, string clientID);

        [OperationContract]
        MultiMessage<DevGps> GetByNameDevGpsList(PagingInfo page, string clientID, string name,string vehicleID,InstallStatusType? installStatus, string mdvrSim);

        /// <summary>
        /// Add devGps in batch
        /// </summary>
        /// <param name="devGpsBatchList"></param>
        /// <returns></returns>
        [OperationContract]
        SingleMessage<Boolean> BatchAddDevGps(List<DevGps> devGpsBatchList);

        /// <summary>
        /// Check devGps Exist
        /// </summary>
        /// <param name="devGpsBatchList"></param>
        /// <returns></returns>
        [OperationContract]
        MultiMessage<DevGps> CheckDevGpsExist(List<DevGps> devGpsBatchList);
    }
}

