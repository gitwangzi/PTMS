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
/////            Create Time: 2013/8/28 11:03:41
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/8/28 11:03:41
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
using Gsafety.PTMS.Integration.Contract.Data;
using System.ServiceModel.Web;
using Gsafety.PTMS.DBEntity;

namespace Gsafety.PTMS.Integration.Contract
{
    /// <summary>
    /// 为ECU-911提供的服务
    /// </summary>
    [ServiceContract]
    public interface IEcu911Service
    {
        /// <summary>
        /// 通过车辆的唯一标识（如车牌号）查询车辆的实时位置信息
        /// </summary>
        /// <param name="car_number">车牌号</param>
        /// <returns>车位置</returns>
        [OperationContract]
        [WebGet(UriTemplate = "GetLocation?car_number={car_number}", ResponseFormat = WebMessageFormat.Json)]
        Location GetLocation(PTMSEntities _context, string car_number);
        /// <summary>
        /// 通过车辆的唯一标识和时间范围查询车辆的历史位置信息
        /// </summary>
        /// <param name="car_number">车牌号</param>
        /// <param name="fromDate">查询的起始时间，精确到秒</param>
        /// <param name="toDate">查询的结束时间，精确到秒</param>
        /// <returns>位置对象集合，该集合应按时间排序</returns>
        [OperationContract]
        [WebGet(UriTemplate = "GetLocationHistory?car_number={car_number}&fromDate={fromDate}&toDate={toDate}", ResponseFormat = WebMessageFormat.Json)]
        IEnumerable<Location> GetLocationHistory(PTMSEntities _context, string car_number, DateTime fromDate, DateTime toDate);

        /// <summary>
        /// 根据车牌号查询特定车辆的信息
        /// </summary>
        /// <param name="car_number">车牌号</param>
        /// <returns>车辆信息</returns>
        [OperationContract]
        [WebGet(UriTemplate = "GetVehicleInfo?car_number={car_number}", ResponseFormat = WebMessageFormat.Json)]
        VehicleInfo GetVehicleInfo(PTMSEntities _context, string car_number);

        /// <summary>
        /// 通知公共交通整体安全系统警情处置已经完成
        /// </summary>
        /// <param name="args"></param>       
        /// <returns>1：成功，0：失败</returns>
        [OperationContract]
        [WebInvoke(UriTemplate = "EndAlarm", ResponseFormat = WebMessageFormat.Json, Method = "POST")]
        int EndAlarm(PTMSEntities _context, AlarmArgs args);
    }
}

