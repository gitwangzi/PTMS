/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 41ef3923-a7da-4deb-bc4e-90098dd78402      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-LIW
/////                 Author: TEST(liw)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Alarm.Contract
/////    Project Description:    
/////             Class Name: IVehicleAlarmService
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/8/8 11:03:41
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/8/23 11:03:41
/////            Modified by: BilongLiu
/////   Modified Description: 
/////======================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using Gsafety.PTMS.Alarm.Contract.Data;
using Gsafety.PTMS.Base.Contract.Data;
using Gsafety.PTMS.Common.Data;

namespace Gsafety.PTMS.Alarm.Contract
{
    [ServiceContract]
    public interface IVehicleAlarmService
    {

        #region Extension Methods

        /// <summary>
        /// 获取一键报警，已处理或未处理(用户名统一验证，放在Htttp头)
        /// </summary>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <param name="isHanded">是否已处理</param>
        /// <param name="pagingInfo">分页信息</param>
        /// <returns>一键报警列表</returns>
        //[OperationContract]
        //MultiMessage<AlarmInfo> GetAlarms(DateTime startTime, DateTime endTime, bool isHanded, PagingInfo pagingInfo);
        /// <summary>
        /// 获取一键报警真警，虚警(用户名统一验证，放在Htttp头)
        /// </summary>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <param name="isTrueAlarm">是否为真警情</param>
        /// <param name="pagingInfo">页数大小</param>
        /// <returns>一键报警列表</returns>
        //[OperationContract]
        //MultiMessage<AlarmInfo> GetAlarmsEx(DateTime startTime, DateTime endTime, bool isTrueAlarm, PagingInfo pagingInfo);
        /// <summary>
        /// 获取车辆相关的一键报警，(用户名统一验证，放在Htttp头)
        /// </summary>
        /// <param name="vehicleId">车牌号</param>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <returns>一键报警列表</returns>
        //[OperationContract]
        //MultiMessage<AlarmInfo> GetAlarmsByVechile(string vehicleId, DateTime startTime, DateTime endTime);


        ///// <summary>
        ///// 根据车牌号获取所有车辆的所有报警信息
        ///// </summary>
        ///// <param name="carNumber"></param>
        ///// <returns></returns>
        //[OperationContract]
        //MultiMessage<AlarmInfo> GetUnhandledAlarms(string vehicleId, PagingInfo pagingInfo);

        ///// <summary>
        ///// 根据告警ID获取告警详情
        ///// </summary>
        ///// <param name="vehicleAlarmId"></param>
        ///// <returns></returns>
        //[OperationContract]
        //SingleMessage<AlarmInfo> GetAlarmDetail(string vehicleAlarmId);

        ///// <summary>
        ///// 增加一键报处理信息（处理人缺省wcf头中）
        ///// </summary>
        ///// <param name="alarmId">报警ID</param>
        ///// <param name="operationTime">处理时间</param>
        ///// <param name="content">处理内容</param>
        //[OperationContract]
        //SingleMessage<Boolean> AddAlarmTreatment(string alarmId, DateTime operationTime, string content, string Disposestaff, short? istruealarm);

        ///// <summary>
        ///// 结束一键报警
        ///// </summary>
        ///// <param name="alarmID">报警ID</param>
        ///// <param name="operationTime">处理时间</param>
        ///// <param name="content">说明</param>
        //[OperationContract]
        //SingleMessage<Boolean> FinishAlarm(string alarmID, DateTime operationTime, string content,string Disposestaff,short? istruealarm);jiangjRemove

        #endregion
        /// <summary>
        /// find all alarm about all vehicle
        /// </summary>
        /// <param name="carNumber"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <param name="isTrueAlarm"></param>
        /// <param name="pagingInfo"></param>
        /// <returns></returns>
        [OperationContract]
        MultiMessage<Gsafety.PTMS.Common.Data.AlarmInfoEx> GetHandledAlarms(string carNumber, DateTime? startTime, DateTime? endTime, short? isTrueAlarm, PagingInfo pagingInfo, string clientid);

        [OperationContract]
        MultiMessage<Gsafety.PTMS.Common.Data.AlarmInfoEx> GetAllAlarms(string carNumber, DateTime? startTime, DateTime? endTime, PagingInfo pagingInfo, string clientid, List<string> organizations);


        /// <summary>
        /// find unhandle alarm，page first load use this function。dzl add 2014-02-19
        /// </summary>
        /// <param name="carNumber"></param>
        /// <returns></returns>
        [OperationContract]
        MultiMessage<Gsafety.PTMS.Common.Data.AlarmInfoEx> GetUnHandledAllAlarms(PagingInfo pagingInfo, string clientid);

        [OperationContract]
        MultiMessage<Gsafety.PTMS.Common.Data.AlarmTypeInfo> GetAllAlarmType();


        /// <summary>
        /// the alarm whether handle(whether In Alarm_Dispose)
        /// </summary>
        /// <param name="vehicleAlarmId"></param>
        /// <returns></returns>
        [OperationContract]
        SingleMessage<bool> IfAlarmDetail(string vehicleAlarmId);

        [OperationContract]
        MultiMessage<AlarmNoteInfo> GetAllAlarmNote(string clientid);

        [OperationContract]
        SingleMessage<bool> AddAlarmNote(string ID, string clientid, string note);

        [OperationContract]
        SingleMessage<bool> DeleteAlarmNote(string ID);

        [OperationContract]
        SingleMessage<bool> UpdateAlarmNote(string ID, string Note);

        [OperationContract]

        MultiMessage<AlarmEmailInfo> GetAllAlarmEmail(string clientid);

        [OperationContract]

        SingleMessage<bool> AddAlarmEmail(AlarmEmailInfo eamil);

        [OperationContract]

        SingleMessage<bool> UpdateAlarmEmail(AlarmEmailInfo eamil);

        [OperationContract]

        SingleMessage<bool> DeleteAlarmEmail(string ID);


        /// <summary>
        /// GET ALARM GPS
        /// </summary>
        /// <param name="alarmId">CarNO</param>
        /// <returns></returns>
        [OperationContract]
        MultiMessage<GPS> GetAlarmGPSTrack(string vehicleId, DateTime startTime, DateTime endTime);

        /// <summary>
        /// Get Alarm(Alarm_Dispose)
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        MultiMessage<AlarmTreatment> GetAlarmTreatments(string alarmID);

        [OperationContract]
        MultiMessage<Gsafety.PTMS.Common.Data.AlarmInfoEx> GetUnHandledAlarms(string clientid, List<string> Organizations);

        [OperationContract]
        SingleMessage<AlarmHandleResult> HandleAlarm(string alarmid, string user, bool alarmresult, string note, DateTime time, bool istransfer, int transfermode, int incidentlevel, string incidentaddress, string incidenttype);


        /// <summary>
        /// 获取警情核查
        /// </summary>
        /// <returns>获取警情核查表</returns>
        [OperationContract]
        SingleMessage<ApealDispose> GetApealDisposeByAlarmID(string alarmID);

        /// <summary>
        /// 获取警情处理表
        /// </summary>
        /// <returns>获取警情处理表</returns>
        [OperationContract]
        SingleMessage<TransferDispose> GetTransferDisposeByAlarmID(string AlarmID);

        /// <summary>
        /// 获取警情处理状态
        /// </summary>
        /// <returns>获取警情处理状态</returns>
        [OperationContract]
        SingleMessage<int> GetTransferDisposeByAlarmID_CAD(string AlarmID);

        /// <summary>
        /// 安装流程中检查报警
        /// </summary>
        /// <param name="installationDetailID"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        [OperationContract]
        SingleMessage<bool> GetAlarmCheck(string installationDetailID, DateTime date);

        [OperationContract]
        SingleMessage<AlarmInfoEx> InsertManualAlarm(ManualAlarmInfo alarminfo);
    }

}

