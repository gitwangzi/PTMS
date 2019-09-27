using Gsafety.PTMS.Base.Contract.Data;
using Gsafety.PTMS.Message.Contract.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Gsafety.PTMS.Message.Contract.Data.Video;

namespace Gsafety.PTMS.Message.Contract
{
    /// <summary>
    /// Call Wcf
    /// </summary>
    //Appoint CallBack Function
    [ServiceContract(CallbackContract = typeof(IMessageCallBackContract))]
    [ServiceKnownType(typeof(AlarmInfo))]
    [ServiceKnownType(typeof(HandingAlert))]
    [ServiceKnownType(typeof(CompleteAlert))]
    [ServiceKnownType(typeof(HandingAlarm))]
    [ServiceKnownType(typeof(DeviceMaintain))]
    [ServiceKnownType(typeof(DeviceInstall))]
    [ServiceKnownType(typeof(CompleteAlarm))]
    [ServiceKnownType(typeof(CameraNoSignalAlert))]
    [ServiceKnownType(typeof(CameraOcclusionAlert))]
    [ServiceKnownType(typeof(FireAlert))]
    [ServiceKnownType(typeof(GpsReceiverFaultAlert))]
    [ServiceKnownType(typeof(MdvrMemoryCardErrorAlert))]
    [ServiceKnownType(typeof(OpenOrCloseDoorAbnormalAlert))]
    [ServiceKnownType(typeof(OverSpeedAlert))]
    [ServiceKnownType(typeof(RegionAlert))]
    [ServiceKnownType(typeof(RemoveDeviceSuiteAlertNotify))]
    [ServiceKnownType(typeof(TemperatureAlert))]
    [ServiceKnownType(typeof(VoltageAbnormalAlert))]
    [ServiceKnownType(typeof(InspectInfo))]
    [ServiceKnownType(typeof(UpgradeCMD))]
    [ServiceKnownType(typeof(GPS))]
    [ServiceKnownType(typeof(PTMSGPS))]
    [ServiceKnownType(typeof(OnOffline))]
    [ServiceKnownType(typeof(ElectircFenceSendSettingModel))]
    [ServiceKnownType(typeof(ElectricFenceCMD))]
    [ServiceKnownType(typeof(HeartbeatInfo))]
    [ServiceKnownType(typeof(ElectricFenceReply))]
    [ServiceKnownType(typeof(SettingOverSpeedReply))]
    [ServiceKnownType(typeof(TravelPlanCMD))]
    [ServiceKnownType(typeof(AreaType))]
    [ServiceKnownType(typeof(RealAlarm))]
    [ServiceKnownType(typeof(LocationMonitorCMD))]
    [ServiceKnownType(typeof(CancelLocationMonitorCMD))]
    [ServiceKnownType(typeof(EndLocationMonitor))]
    [ServiceKnownType(typeof(LocationMonitorEndType))]
    [ServiceKnownType(typeof(SettingGpsSendUpCMD))]
    [ServiceKnownType(typeof(GpsSendUpModel))]
    [ServiceKnownType(typeof(RuleOperationType))]
    public interface IMessageService
    {
        #region User Message

        /// <summary>
        /// Send Handing Alarm Message
        /// </summary>
        /// <param name="model"></param>
        //[OperationContract]
        //void SendHandingAlarmMessage(HandingAlarm model);

        ///// <summary>
        ///// Get Handing Alarm Message
        ///// </summary>
        //[OperationContract]
        //void GetHandingAlarmMessage();

        /// <summary>
        /// Get Complete Alarm Message
        /// </summary>
        [OperationContract]
        void SendCompleteAlarmMessage(CompleteAlarm model);
        [OperationContract]
        void GetCompleteAlarmMessage();

        [OperationContract]
        void SendChangeGroupVechleMessage(ChangeGroupVehicle model);

        [OperationContract]
        void GetChangeGroupVechleMessage();

        [OperationContract]
        void SendChangeGroupMessage(ChangeGroup model);

        [OperationContract]
        void GetChangeGroupMessage();
        /// <summary>
        /// Send Device Install Message
        /// </summary>
        /// <param name="model"></param>
        [OperationContract]
        void SendDeviceInstallMessage(DeviceInstall model);

        /// <summary>
        /// Get Device Install Message
        /// </summary>
        [OperationContract]
        void GetDeviceInstallMessage();

        /// <summary>
        /// Send Device Maintain Message
        /// </summary>
        /// <param name="model"></param>
        [OperationContract]
        void SendDeviceMaintainMessage(DeviceMaintain model);

        /// <summary>
        /// Get Device Maintain Message
        /// </summary>
        [OperationContract]
        void GetDeviceMaintainMessage();

        /// <summary>
        /// Send Handing Alert Message
        /// </summary>
        /// <param name="model"></param>
        [OperationContract]
        void SendHandingAlertMessage(HandingAlert model);

        /// <summary>
        /// Get Handing Alert Message
        /// </summary>
        [OperationContract]
        void GetHandingAlertMessage();

        /// <summary>
        /// Send Complete Alert Message
        /// </summary>
        [OperationContract]
        void SendCompleteAlertMessage(CompleteAlert modell);

        /// <summary>
        /// Get Complete Alert Message
        /// </summary>
        [OperationContract]
        void GetCompleteAlertMessage();

        /// <summary>
        /// Send Start Install Message 
        /// </summary>
        [OperationContract]
        void SendStartInstallMessage(StartInstall modell);

        /// <summary>
        /// Get Start Install Message
        /// </summary>
        [OperationContract]
        void GetStartInstallMessage();

        /// <summary>
        /// Send Delete Install Message
        /// </summary>
        [OperationContract]
        void SendDeleteInstallMessage(DeleteInstall model);

        /// <summary>
        /// Get Delete Install Message
        /// </summary>
        [OperationContract]
        void GetDeleteInstallMessage();

        /// <summary>
        /// Get the Delete User Message
        /// </summary>
        [OperationContract]
        void GetDeleteUserMessage();

        /// <summary>
        /// Get the Upgrade User Message
        /// </summary>
        [OperationContract]
        void GetChangeUserMessage();

        /// <summary>
        /// Send the Message of Deleting User 
        /// </summary>
        [OperationContract]
        void SendDeleteUserMessage(DeleteUser model);

        /// <summary>
        /// Send Change User Message
        /// </summary>
        [OperationContract]
        void SendChangeUserMessage(ChangeUser model);

        /// <summary>
        /// Send Route Message
        /// </summary>
        [OperationContract]
        void SendRouteMessage(RouteCMD model);

        /// <summary>
        /// Send Batch Route Message
        /// </summary>
        [OperationContract]
        void SendRouteMessageSet(List<RouteCMD> routes);

        /// <summary>
        /// Send Travel Plan Message 
        /// </summary>
        [OperationContract]
        void SendTravelPlanMessage(TravelPlanCMD model);

        /// <summary>
        /// Send Batch Travel Plan Message
        /// </summary>
        [OperationContract]
        void SendTravelPlanMessageSet(List<TravelPlanCMD> travelPlans);

        /// <summary>
        /// Send Upgrade Notify Message
        /// </summary>
        [OperationContract]
        void SendUpgradeNotifyMessage(UpgradeNotify model);


        #endregion

        #region MQ消息

        /// <summary>
        /// Get Alarm Message
        /// </summary>
        [OperationContract]
        void GetAlarmMessage();

        /// <summary>
        /// Cancel Alarm Message
        /// </summary>
        [OperationContract]
        void CancelAlarmMessage();

        /// <summary>
        /// Get Cancel Alarm Message
        /// </summary>
        [OperationContract]
        void GetCancelAlarmMessage();

        /// <summary>
        /// Get Alarm GPS Message
        /// </summary>
        [OperationContract]
        void GetAlarmGpsMessage(string mdvrCoreId);

        /// <summary>
        /// Cancel Alarm GPS Message
        /// </summary>
        [OperationContract]
        void CancelAlarmGpsMessage(string mdvrCoreId);

        /// <summary>
        /// Get Monitor GPS Message
        /// </summary>
        [OperationContract]
        void GetMonitorGpsMessage(LocationMonitorCMD locationMonitorCmd);

        /// <summary>
        /// Cancel Monitor GPS Message
        /// </summary>
        [OperationContract]
        void CancelMonitorGpsMessage(CancelLocationMonitorCMD cancelLocationMonitorCmd);

        [OperationContract]
        void GetLocationMonitorEndMessage(string SessionId);

        /// <summary>
        /// Get Camera Nosignal Alert Message
        /// </summary>
        [OperationContract]
        void GetCameraNoSignalAlertMessage();

        /// <summary>
        /// Cancel Camera NoSignal Alert Message 
        /// </summary>
        [OperationContract]
        void CancelCameraNoSignalAlertMessage();

        /// <summary>
        /// Get Camera Occlusion Alert Message
        /// </summary>
        [OperationContract]
        void GetCameraOcclusionAlertMessage();

        /// <summary>
        /// Cancel Camera Occlusion Alert Message
        /// </summary>
        [OperationContract]
        void CancelCameraOcclusionAlertMessage();

        /// <summary>
        /// Get Fire Alert Message
        /// </summary>
        [OperationContract]
        void GetFireAlertMessage();

        /// <summary>
        /// Cancel Fire Alert Message
        /// </summary>
        [OperationContract]
        void CancelFireAlertMessage();

        /// <summary>
        /// Get GPS Reciever Error Alert Message
        /// </summary>
        [OperationContract]
        void GetGpsReceiverFaultAlertMessage();

        /// <summary>
        /// Cancel GPS Receiver Fault Alert Message
        /// </summary>
        [OperationContract]
        void CancelGpsReceiverFaultAlertMessage();

        /// <summary>
        /// Get MDVR Card Error Alert Message
        /// </summary>
        [OperationContract]
        void GetMdvrMemoryCardErrorAlertMessage();

        /// <summary>
        /// Cancel MDVR Card Error Alert Message
        /// </summary>
        [OperationContract]
        void CancelMdvrMemoryCardErrorAlertMessage();

        /// <summary>
        /// Get Door Abnormal Alert Message
        /// </summary>
        [OperationContract]
        void GetOpenOrCloseDoorAbnormalAlertMessage();

        /// <summary>
        /// Cancel Door Abnormal Alert Message
        /// </summary>
        [OperationContract]
        void CancelOpenOrCloseDoorAbnormalAlertMessage();

        /// <summary>
        /// Get OverSpeed Alert Message
        /// </summary>
        [OperationContract]
        void GetOverSpeedAlertMessage();

        /// <summary>
        /// Cancel Overspeed Alert Message
        /// </summary>
        [OperationContract]
        void CancelOverSpeedAlertMessage();

        /// <summary>
        /// Get Region Alert Meesage
        /// </summary>
        [OperationContract]
        void GetRegionAlertMessage();

        /// <summary>
        /// Cancel Region Alert Message
        /// </summary>
        [OperationContract]
        void CancelRegionAlertMessage();

        /// <summary>
        /// Get the Remove Message of The Device Suite Alert Notification
        /// </summary>
        [OperationContract]
        void GetRemoveDeviceSuiteAlertNotifyMessage();

        /// <summary>
        /// Cancel Remove Device Suite Alert Notify Message
        /// </summary>
        [OperationContract]
        void CancelRemoveDeviceSuiteAlertNotifyMessage();

        /// <summary>
        /// Get Temperature Alert Message
        /// </summary>
        [OperationContract]
        void GetTemperatureAlertMessage();

        /// <summary>
        /// Cancel Temperature Alert Message
        /// </summary>
        [OperationContract]
        void CancelTemperatureAlertMessage();

        /// <summary>
        /// Get Voltage Abnormal Alert Message
        /// </summary>
        [OperationContract]
        void GetVoltageAbnormalAlertMessage();

        /// <summary>
        /// Cancel Voltage Abnormal Alert Message
        /// </summary>
        [OperationContract]
        void CancelVoltageAbnormalAlertMessage();

        /// <summary>
        /// Get Inspect Message
        /// </summary>
        [OperationContract]
        void GetInspectMessage();

        /// <summary>
        /// Cancel Inspect Message
        /// </summary>
        [OperationContract]
        void CancelInspectMessage();

        /// <summary>
        /// Get Online Message
        /// </summary>
        [OperationContract]
        void GetOnOfflineMessage();

        /// <summary>
        /// Cancel Online Message
        /// </summary>
        [OperationContract]
        void CancelOnOfflineMessage();

        /// <summary>
        /// Get Safe Suite Runing-Status Message
        /// </summary>
        [OperationContract]
        void GetSuiteRunintStatusMessage();

        /// <summary>
        /// Get Electric Fence Reply Message
        /// </summary>
        [OperationContract]
        void GetFenceReplyMessage();

        /// <summary>
        /// Get OverSpeed-Setting Reply
        /// </summary>
        [OperationContract]
        void GetSettingOverSpeedReplyMessage();
        #endregion

        #region Send Command

        /// <summary>
        /// Send Upgrade Command
        /// </summary>
        [OperationContract]
        void SendUpgradeCMD(UpgradeCMD modell);

        /// <summary>
        /// Send Electric Fence Information
        /// </summary>
        [OperationContract]
        void SendElectricFenceCMD(ElectircFenceSendSettingModel modell);

        /// <summary>
        /// Batch Send Electric Fence Information
        /// </summary>
        /// <param name="fenceSet"></param>
        [OperationContract]
        void SendElectricFenceCMDSet(List<ElectircFenceSendSettingModel> fenceSet);

        /// <summary>
        /// Get Upgrade State Command
        /// </summary>
        [OperationContract]
        void SendGetUpgradeStatusCMD(UpgradeStatusCMD model);

        /// <summary>
        /// Get the State Information of Safety Suite Command
        /// </summary>
   

        /// <summary>
        /// Set Overspeed Command
        /// </summary>
        [OperationContract]
        void SendSettingOverSpeedCMD(SettingOverSpeedCMD model);

        /// <summary>
        /// Set Overspeed Command By Batch
        /// </summary>
        [OperationContract]
        void SendSettingOverSpeedCMDSet(List<SettingOverSpeedCMD> overSpeed);



        #region New Add 2014-11
        [OperationContract]
        void SendElectircFenceSetting(ElectircFenceSendSettingModel model);

        [OperationContract]
        void SendOverSpeedSetting(OverSpeedSendSettingModel model);
        /// <summary>
        /// Set GPS Upload By Batch
        /// </summary>
        /// <param name="model">Include </param>
        /// <param name="gpsUpLoad"></param>
        [OperationContract]
        void SendSettingGpsUploadCMDSet(GpsSendUpModel model);

        [OperationContract]
        void SendSettingAbnormalDoorUploadCMDSet(AbnormalDoorSendUpModel model);

        [OperationContract]
        void SendSettingTemperatureUploadCMDSet(TemperatureSendUpModel model);

        [OperationContract]
        void SendSettingOneKeyAlarmUploadCMDSet(OneKeyAlarmSendUpModel model);

        [OperationContract]
        void SendInfomationCommand(SendInfomationModel model);
        #endregion

        [OperationContract]
        void SendGetVideoListCMD(QueryMdvrFileList model);

        [OperationContract]
        void GetSettingVideoListReplyMessage();

        [OperationContract]
        void GetV23ReplyMessage();
        [OperationContract]
        void SendDownloadMdvrFile(DownloadFile model);


        #endregion


        /// <summary>
        /// Init Queue
        /// </summary>
        /// <param name="QUEUE"></param>
        [OperationContract]
        void Init(string QUEUE, RuleInfo ruleInfo);
    }

    /// <summary>
    /// CallBack Interface
    /// </summary>
    [ServiceKnownType(typeof(AlarmInfo))]
    [ServiceKnownType(typeof(HandingAlert))]
    [ServiceKnownType(typeof(CompleteAlert))]
    [ServiceKnownType(typeof(HandingAlarm))]
    [ServiceKnownType(typeof(DeviceMaintain))]
    [ServiceKnownType(typeof(DeviceInstall))]
    [ServiceKnownType(typeof(CompleteAlarm))]
    [ServiceKnownType(typeof(CameraNoSignalAlert))]
    [ServiceKnownType(typeof(CameraOcclusionAlert))]
    [ServiceKnownType(typeof(FireAlert))]
    [ServiceKnownType(typeof(GpsReceiverFaultAlert))]
    [ServiceKnownType(typeof(MdvrMemoryCardErrorAlert))]
    [ServiceKnownType(typeof(OpenOrCloseDoorAbnormalAlert))]
    [ServiceKnownType(typeof(OverSpeedAlert))]
    [ServiceKnownType(typeof(RegionAlert))]
    [ServiceKnownType(typeof(RemoveDeviceSuiteAlertNotify))]
    [ServiceKnownType(typeof(TemperatureAlert))]
    [ServiceKnownType(typeof(VoltageAbnormalAlert))]
    [ServiceKnownType(typeof(InspectInfo))]
    [ServiceKnownType(typeof(UpgradeCMD))]
    [ServiceKnownType(typeof(GPS))]
    [ServiceKnownType(typeof(OnOffline))]
    [ServiceKnownType(typeof(ElectricFenceCMD))]
    [ServiceKnownType(typeof(StartInstall))]
    [ServiceKnownType(typeof(DeleteInstall))]
    [ServiceKnownType(typeof(DeleteUser))]
    [ServiceKnownType(typeof(ChangeUser))]
    [ServiceKnownType(typeof(BasicInfo))]
    [ServiceKnownType(typeof(Enviroment))]
    [ServiceKnownType(typeof(Hardware))]
    [ServiceKnownType(typeof(HeartbeatInfo))]
    [ServiceKnownType(typeof(ElectricFenceReply))]
    [ServiceKnownType(typeof(SettingOverSpeedReply))]
    [ServiceKnownType(typeof(TravelPlanCMD))]
    [ServiceKnownType(typeof(AreaType))]
    [ServiceKnownType(typeof(RealAlarm))]
    [ServiceKnownType(typeof(LocationMonitorCMD))]
    [ServiceKnownType(typeof(CancelLocationMonitorCMD))]
    [ServiceKnownType(typeof(EndLocationMonitor))]
    [ServiceKnownType(typeof(LocationMonitorEndType))]
    [ServiceKnownType(typeof(SettingGpsSendUpCMD))]
    [ServiceKnownType(typeof(GpsSendUpModel))]
    [ServiceKnownType(typeof(VideoListResult))]
    [ServiceKnownType(typeof(ChangeGroup))]
    [ServiceKnownType(typeof(ChangeGroupVehicle))]
    public interface IMessageCallBackContract
    {
        [OperationContract(IsOneWay = true)]
        void MessageCallBack(object message);
    }
}
