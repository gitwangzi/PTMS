using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using Gsafety.PTMS.Base.Contract.Data;
using Gsafety.PTMS.BaseInformation.Contract.Data;
using Gsafety.PTMS.Common.Data;
using Gsafety.PTMS.Common.Enum;

namespace Gsafety.PTMS.BaseInformation.Contract
{
    ///<summary>
    ///安全套件
    ///</summary>
    [ServiceContract]
    public interface IBscDevSuiteService
    {

        /// <summary>
        /// 添加安全套件
        /// </summary>
        /// <param name="model">安全套件</param>
        [OperationContract]
        SingleMessage<bool> InsertBscDevSuite(DevSuite model);

        /// <summary>
        /// 修改安全套件
        /// </summary>
        /// <param name="model">安全套件</param>
        [OperationContract]
        SingleMessage<bool> UpdateBscDevSuite(DevSuite model);

        /// <summary>
        /// 删除安全套件
        /// </summary>
        /// <param name="model">安全套件</param>
        [OperationContract]
        SingleMessage<bool> DeleteBscDevSuiteByID(string SuiteInfoID);

        /// <summary>
        /// 获取安全套件
        /// </summary>
        /// <returns>获取安全套件</returns>
        [OperationContract]
        SingleMessage<DevSuite> GetBscDevSuite(string SuiteInfoID);
        /// <summary>
        /// 获取安全套件
        /// </summary>
        /// <returns>获取安全套件</returns>
        [OperationContract]
        SingleMessage<DevSuite> GetDevSuiteBySuiteID(string suiteID);
        /// <summary>
        /// 获取安全套件列表
        /// </summary>
        /// <returns>获取安全套件</returns>
        [OperationContract]
        MultiMessage<DevSuite> GetBscDevSuiteList(string clientID, InstallStatusType? installStatus, string vehicleSn, string suitID, string mdvrCoreSn, string mdvrSn, string mdvrSim, int pageIndex, int pageSize);

        /// <summary>
        /// Batch Add Device Suite
        /// </summary>
        /// <param name="suiteList"></param>
        /// <returns></returns>
        [OperationContract]
        SingleMessage<Boolean> BatchAdd(List<DevSuite> suiteList);

        /// <summary>
        /// Check DeviceSuite exist 
        /// </summary>
        /// <param name="deviceSuiteList"></param>
        /// <returns></returns>
        [OperationContract]
        MultiMessage<DevSuite> CheckSecuritySuiteExist(List<DevSuite> deviceSuiteList);

        /// <summary>
        /// 获取安全套件导出列表
        /// </summary>
        /// <returns>获取安全套件</returns>
        [OperationContract]
        MultiMessage<DevSuite> GetBscDevSuiteExportList(string clientID, InstallStatusType? installStatus, string vehicleSn, string suitID, string mdvrCoreSn, string mdvrSn, string mdvrSim, int pageIndex, int pageSize);

        [OperationContract]
        SingleMessage<string> GetBscDevSuiteIDByVehicleSN(string vehicleSN);
    }
}

