/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 41ef3923-a7da-4deb-bc4e-90098dd78402      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-LIW
/////                 Author: TEST(BilongLiu)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Alarm.Contract
/////    Project Description:    
/////             Class Name: IVehicleAlarmService
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/8/26 11:03:41
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/8/26 11:03:41
/////            Modified by: 
/////   Modified Description: 
/////======================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using Gsafety.PTMS.Base.Contract.Data;
using Gsafety.PTMS.SecuritySuite.Contract.Data;

namespace Gsafety.PTMS.SecuritySuite.Contract
{
    /// <summary>
    /// The service of securitysuite and vehiclestatus
    /// </summary>
    [ServiceContract]
    public interface IVehicleStatusService
    {
        /// <summary>
        ///  根据行政区划获取车辆状态
        /// </summary>
        /// <param name="districtCode">行政区划编码</param>
        /// <returns>车辆的状态列表</returns>
        [OperationContract]
        MultiMessage<VehicleStatus> GetVehicleStatusByDistrict(string districtCode);
        /// <summary>
        ///  根据行政区划获取车辆状态
        /// </summary>
        /// <param name="districtCode">行政区划编码</param>
        /// <param name="vehicleType">车辆类型</param>
        /// <returns>车辆的状态列表</returns>
        [OperationContract]
        MultiMessage<VehicleStatus> GetVehicleStatusByDistrictEx(string districtCode, int vehicleType);
        /// <summary>
        ///  根据是否在线获取车辆状态
        /// </summary>
        /// <param name="districtCode">行政区划编码</param>
        /// <param name="isOnline">是否在线</param>
        /// <returns>车辆的状态列表</returns>
        [OperationContract]
        MultiMessage<VehicleStatus> GetVehicleStatusByStatus(string districtCode, Boolean isOnline);
        /// <summary>
        /// 根据监控分组获取车辆状态 
        /// </summary>
        /// <returns>车辆的状态列表</returns>
        [OperationContract]
        MultiMessage<VehicleStatus> GetVehicleStatusByGroup(string groupId);
        /// <summary>
        /// 根据车牌号获取当前车辆状态信息
        /// </summary>
        /// <param name="vehicleId">车牌号</param>
        /// <returns>车辆的状态</returns>
        [OperationContract]
        SingleMessage<VehicleStatus> GetVehicleStatusByVehicleNumber(string vehicleId);
        /// <summary>
        /// 根据车辆在线时长获取当前车辆状态信息
        /// </summary>
        /// <param name="vehicleId"></param>
        /// <returns></returns>
        [OperationContract]
        MultiMessage<VehicleStatus> GetVehicleStatusByTimeSpan(int timespan);
        /// <summary>
        /// 模糊查询安全套件状态，分页查询
        /// </summary>
        /// <param name="vehicleId">车牌号</param>
        /// <param name="suiteId">安全套件编号</param>
        /// <param name="pageInfo">分页信息</param>
        /// <param name="onlineStatus">在线状态</param>
        /// <returns>符合条件的安全套件列表</returns>
        [OperationContract]
        MultiMessage<SuiteStatus> GetSuiteStatusFuzzy(string vehicleId, string suiteId, int onlineStatus, int timespan, PagingInfo pageInfo);
        /// <summary>
        /// 根据在线、离线时间查询车辆
        /// </summary>
        /// <param name="vehicleId">车牌号</param>
        /// <param name="suiteId">安全套件编号</param>
        /// <param name="pageInfo">分页信息</param>
        /// <param name="onlineStatus">在线状态</param>
        /// <returns>符合条件的安全套件列表</returns>
        [OperationContract]
        MultiMessage<SuiteStatus> GetVehicleTimeSpanFuzzy(string vehicleId, string suiteId, int onlineStatus, string timespan, PagingInfo pageInfo);
        /// <summary>
        /// 套件状态管理
        /// </summary>
        /// <param name="vehicleId">车牌号</param>
        /// <param name="suiteId">安全套件编号</param>
        /// <param name="pageInfo">分页信息</param>
        /// <param name="onlineStatus">在线状态</param>
        /// <returns>符合条件的安全套件列表</returns>
        [OperationContract]
        MultiMessage<InitialSuiteMangement> GetSuiteStatusManagement(string suiteId, int currentStatus, PagingInfo pageInfo);
        /// <summary>
        /// 套件状态切换入库
        /// </summary>
        /// <param name="vehicleId">车牌号</param>
        /// <param name="suiteId">安全套件编号</param>
        /// <param name="pageInfo">分页信息</param>
        /// <param name="onlineStatus">在线状态</param>
        /// <returns>符合条件的安全套件列表</returns>
        [OperationContract]
        bool RunningToAbnoraml(SuiteMangementInfo newSuiteStatus);

        [OperationContract]
        bool AbnoramlToRepair(SuiteMangementInfo newSuiteStatus);

        [OperationContract]
        bool AbnoramlToRunning(SuiteMangementInfo newSuiteStatus);

        [OperationContract]
        bool RepairToInitial(SuiteMangementInfo newSuiteStatus);
          }
}
