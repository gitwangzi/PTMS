using Gsafety.PTMS.Alert.Contract.Data;
using Gsafety.PTMS.Base.Contract.Data;
using Gsafety.PTMS.Common.Data;
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
using System.ServiceModel;

namespace Gsafety.PTMS.Alert.Contract
{
    [ServiceContract]
    public interface IVehicleAlertService
    {
        #region Extension Method

        /// <summary>
        /// 按车公司和告警类别获取所有未处理的车辆告警信息
        /// </summary>
        /// <param name="companyCode">行政区划代码</param>
        /// <param name="alertType">告警类别</param>
        /// <param name="pagingInfo">分页信息</param>
        /// <returns>车辆告警信息</returns>
        //[OperationContract]
        //MultiMessage<VehicleAlert> GetVehicleUnhandleAlertEx1(string companyCode, int alertType, PagingInfo pagingInfo);
        /// <summary>
        /// 按行政区划获取某时间段已处理的车辆告警信息
        /// </summary>
        /// <param name="disctrictCode">行政区划代码</param>
        /// <param name="startTime">起始日期</param>
        /// <param name="endTime">结束日期</param>
        /// <param name="pagingInfo">分页信息</param>
        /// <returns>车辆告警信息</returns>
        //[OperationContract]
        //MultiMessage<VehicleAlert> GetVehicleHandledAlert(string disctrictCode, DateTime startTime, DateTime endTime, PagingInfo pagingInfo);
        /// <summary>
        /// 按行政区划和告警类别获取某时间段已处理的车辆告警信息
        /// </summary>
        /// <param name="disctrictCode">行政区划代码</param>
        /// <param name="alertType">告警类别</param>
        /// <param name="startTime">起始日期</param>
        /// <param name="endTime">结束日期</param>
        /// <param name="pagingInfo">分页信息</param>
        /// <returns>车辆告警信息</returns>
        //[OperationContract]
        //MultiMessage<VehicleAlert> GetVehicleHandledAlertEx(string disctrictCode, int alertType, DateTime startTime, DateTime endTime, PagingInfo pagingInfo);
        /// <summary>
        /// 获取特定车辆某时间段的告警信息
        /// </summary>
        /// <param name="vehicleId">车牌号</param>
        /// <param name="startTime">起始日期</param>
        /// <param name="endTime">结束日期</param>
        /// <param name="pagingInfo">分页信息</param>
        /// <returns>车辆告警信息</returns>
        //[OperationContract]
        //MultiMessage<VehicleAlert> GetVehicleAlert(string vehicleId, DateTime startTime, DateTime endTime, PagingInfo pagingInfo);
        /// <summary>
        /// 获取特定车辆某时间段特定类型的告警信息
        /// </summary>
        /// <param name="vehicleId">车牌号</param>
        /// <param name="alertType">告警类型</param>
        /// <param name="startTime">起始日期</param>
        /// <param name="endTime">结束日期</param>
        /// <param name="pagingInfo">分页信息</param>
        /// <returns>车辆告警信息</returns>
        //[OperationContract]
        //MultiMessage<VehicleAlert> GetVehicleAlertEx(string vehicleId, int alertType, DateTime startTime, DateTime endTime, PagingInfo pagingInfo); 

        //[OperationContract]
        //MultiMessage<VehicleAlert> GetVehicleUnhandleAlert(string proviceCode, string cityCode, string companyCode, string vehicleId,
        //    DateTime? startTime, DateTime? endTime, int alertType, PagingInfo pagingInfo);
        #endregion

        /// <summary>
        /// GET ALL HANDLE VEHICLE ALERT
        /// </summary>
        /// <param name="proviceCode"></param>
        /// <param name="cityCode"></param>
        /// <param name="companyCode"></param>
        /// <param name="vehicleId"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <param name="alertType"></param>
        /// <param name="pagingInfo"></param>
        /// <returns></returns>
        //[OperationContract]
        //MultiMessage<VehicleAlertEx> GetVehicleHandledAlert(string proviceCode, string cityCode, string companyCode, string vehicleId,
        //    DateTime? startTime, DateTime? endTime, int alertType, PagingInfo pagingInfo);


        /// <summary>
        /// ADD VEHICLE ALERT MSG
        /// </summary>
        [OperationContract]
        MultiMessage<VehicleAlertEx> GetVehicleHandledAlert(string proviceCode, string cityCode, string companyCode, string vehicleId,
            DateTime? startTime, DateTime? endTime, int alertType, PagingInfo pagingInfo);


        [OperationContract]
        MultiMessage<BusinessAlertEx> GetAllBusinessAlert(string vehicleId,
            DateTime? startTime, DateTime? endTime, PagingInfo pagingInfo, List<string> orgnizations, int alerttype);


        /// <summary>
        /// ADD VEHICLE ALERT MSG
        /// </summary>
        [OperationContract]
        SingleMessage<Boolean> AddVechileAlertTreatment(VehicleAlertTreatment alertTreatment);

        /// <summary>
        /// GET THE VEHICLE ALERT MSG BY THE VEHICLEALERTID
        /// </summary>
        /// <param name="vehicleAlertId"></param>
        /// <returns></returns>
        [OperationContract]
        SingleMessage<VehicleAlertDetail> GetVehicleAlertDetail(string vehicleAlertId);

        /// <summary>
        /// IN/OUT FENCE ALERT
        /// </summary>
        /// <param name="vehicleID"></param>
        /// <param name="fenceID"></param>
        /// <param name="fenceType"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        [OperationContract]
        MultiMessage<VehicleFenceAlert> GetVehicleFenceAlert(string vehicleID, string fenceID, short alertType, DateTime? startTime, DateTime? endTime);

        [OperationContract]
        MultiMessage<Gsafety.PTMS.Common.Data.BusinessAlertEx> GetUnHandleAlertByClient(string clientID, List<string> orgnizations);

        /// <summary>
        /// 添加已处理业务类告警
        /// </summary>
        /// <param name="model">已处理业务类告警</param>
        [OperationContract]
        SingleMessage<AlertHandleResult> InsertBusinessAlertHandle(BusinessAlertHandle model);

        /// <summary>
        /// 获取已处理业务类告警
        /// </summary>
        /// <returns>获取已处理业务类告警</returns>
        [OperationContract]
        SingleMessage<BusinessAlertHandle> GetBusinessAlertHandleByAlertID(string alertID);

        /// <summary>
        /// 获取车辆告警处理信息
        /// </summary>
        /// <param name="id">警情编号</param>
        /// <param name="vehicleId">车辆编号</param>
        /// <param name="clientId">客户端编号</param>
        /// <returns></returns>
        [OperationContract]
        SingleMessage<BusinessAlertEx> GetVehicleAlertDisposeInfo(string id, string vehicleId, string clientId);
    }
}
