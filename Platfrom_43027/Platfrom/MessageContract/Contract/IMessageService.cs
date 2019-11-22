using Gsafety.PTMS.Base.Contract.Data;
using Gsafety.PTMS.Message.Contract.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Gsafety.PTMS.Message.Contract.Data.Video;
using Gsafety.PTMS.Common.Enum;

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
    //[ServiceKnownType(typeof(CompleteAlarm))]
    //[ServiceKnownType(typeof(GPS))]
    //[ServiceKnownType(typeof(PTMSGPS))]
    [ServiceKnownType(typeof(OnOffline))]
    [ServiceKnownType(typeof(HeartbeatInfo))]
    [ServiceKnownType(typeof(AreaType))]
    [ServiceKnownType(typeof(RealAlarm))]
    [ServiceKnownType(typeof(EndLocationMonitor))]
    [ServiceKnownType(typeof(LocationMonitorEndType))]
    [ServiceKnownType(typeof(RuleOperationType))]
    public interface IMessageService
    {
        #region User Message

        /// <summary>
        /// Get Complete Alarm Message
        /// </summary>
        //[OperationContract]
        //void SendCompleteAlarmMessage(CompleteAlarm model);

        /// <summary>
        /// Send Device Install Message
        /// </summary>
        /// <param name="model"></param>
        [OperationContract]
        void SendDeviceInstallMessage(DeviceInstall model);

        /// <summary>
        /// Send Device Maintain Message
        /// </summary>
        /// <param name="model"></param>
        [OperationContract]
        void SendDeviceMaintainMessage(DeviceMaintain model);

        /// <summary>
        /// Send Handing Alert Message
        /// </summary>
        /// <param name="model"></param>
        [OperationContract]
        void SendHandingAlertMessage(HandingAlert model);


        /// <summary>
        /// Send Complete Alert Message
        /// </summary>
        [OperationContract]
        void SendCompleteAlertMessage(CompleteAlert modell);

        /// <summary>
        /// Send Start Install Message 
        /// </summary>
        [OperationContract]
        void SendStartInstallMessage(StartInstall modell);

        /// <summary>
        /// Send Delete Install Message
        /// </summary>
        [OperationContract]
        void SendDeleteInstallMessage(DeleteInstall model);

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

        #region New Add 2014-11
        /// <summary>
        /// Set GPS Upload By Batch
        /// </summary>
        /// <param name="model">Include </param>
        /// <param name="gpsUpLoad"></param>
        [OperationContract]
        void SendSettingGpsUploadCMDSet(GpsSendUpModel model);

        [OperationContract]
        void SendSettingOneKeyAlarmUploadCMDSet(OneKeyAlarmSendUpModel model);

        [OperationContract]
        void SendInfomationCommand(SendInfomationModel model);
        #endregion

        [OperationContract]
        void SendGetVideoListCMD(QueryMdvrFileList model);


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
    //[ServiceKnownType(typeof(CompleteAlarm))]
    [ServiceKnownType(typeof(UpgradeCMD))]
    //[ServiceKnownType(typeof(GPS))]
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
