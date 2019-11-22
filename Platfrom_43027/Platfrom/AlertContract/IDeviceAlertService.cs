using Gsafety.PTMS.Base.Contract.Data;
/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 7c0be976-c51a-4328-87f8-f4e34a1fcb16      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: KENNY-PC
/////                 Author: TEST(jiaoyx)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Alert.Contract
/////    Project Description:    
/////             Class Name: IVehicleAlertService
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/8/5 11:03:45
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/8/26 11:03:45
/////            Modified by: BilongLiu
/////   Modified Description: 
/////======================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using Gsafety.PTMS.Alert.Contract.Data;
using Gsafety.PTMS.BaseInformation.Contract.Data;
using System.Collections.ObjectModel;
using Gsafety.PTMS.Common.Data;

namespace Gsafety.PTMS.Alert.Contract
{
    [ServiceContract]
    public interface IDeviceAlertService
    {
        #region

        ///// <summary>
        ///// 获取特定设备某时间段特定类型的告警信息
        ///// </summary>
        ///// <param name="vechileId">车牌号</param>
        ///// <param name="sutieId">套件号</param>
        ///// <param name="alertType">告警类型</param>
        ///// <param name="ishandled">是否已处理</param>
        ///// <param name="pagingInfo">分页信息</param>
        ///// <returns>设备告警信息</returns>
        //[OperationContract]
        //MultiMessage<DeviceAlert> GetDeviceAlertForVechileSer(string vechileId, string sutieId, List<decimal?> alertType, decimal? selecthandle, DateTime startTime, DateTime endTime, PagingInfo pagingInfo);

        ///// <summary>
        ///// 增加设备告警核实
        ///// </summary>
        ///// <param name="alertCheck">设备告警核实信息</param>
        ///// <returns>操作结果</returns>
        //[OperationContract]
        //SingleMessage<Boolean> AddDeviceAlertCheck(DeviceAlertCheck deviceAlertCheck, bool handleFlag);


        ///// <summary>
        ///// 根据车牌号获取告警ID Add By penggl
        ///// </summary>
        ///// <param name="vehicleId"></param>
        ///// <returns></returns>
        //[OperationContract]
        //SingleMessage<string> GetAlertIdByVehicleId(string vehicleId);   

        ///// <summary>
        ///// 添加设备告警处理
        ///// </summary>
        ///// <param name="alertTreatment">设备告警处理信息</param>
        ///// <returns>操作结果</returns>
        //[OperationContract]
        //SingleMessage<Boolean> AddDeviceAlertHandle(DeviceAlertHandle deviceAlertHandle);
        #endregion
        /// <summary>
        /// AddDeviceAlert
        /// </summary>
        [OperationContract]
        SingleMessage<Gsafety.PTMS.Alert.Contract.Data.DeviceAlert> AddDeviceAlert(Gsafety.PTMS.Alert.Contract.Data.DeviceAlert alert);

        /// <summary>
        /// get the device alert ，which suite in the some time 
        /// </summary>
        /// <param name="CarNumber"></param>
        /// <param name="sutieId"></param>
        /// <param name="alertType"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <param name="pagingInfo"></param>
        /// <returns></returns>
        [OperationContract]
        MultiMessage<DeviceAlertEx> GetDeviceAlertEx1(string CarNumber, string sutieId, List<decimal?> alertType, DateTime? startTime, DateTime? endTime, PagingInfo pagingInfo);

        /// <summary>
        /// update the suitestatus (on mdvrCoreSN)
        /// </summary>
        /// <param name="mdvrCoreSN"></param>
        /// <param name="status"></param>
        /// <param name="alertType"></param>
        /// <returns></returns>
        [OperationContract]
        SingleMessage<Boolean> ModifySecuritySuiteStatus(string mdvrCoreSN, DeviceSuiteStatus status, Gsafety.PTMS.Base.Contract.Data.DeviceAlertType alertType);

        [OperationContract]
        SingleMessage<bool> UpdateDeviceAlert(Gsafety.PTMS.Alert.Contract.Data.DeviceAlert model);

        [OperationContract]
        SingleMessage<bool> DeleteDeviceAlertByID(string ID);

        [OperationContract]
        MultiMessage<Gsafety.PTMS.Alert.Contract.Data.DeviceAlert> GetDeviceAlertList(string clientID, string vehicleID, int? alertType, DateTime? StartTime, DateTime? EndTime, List<string> stationids, int pageIndex, int pageSize);


    }
}
