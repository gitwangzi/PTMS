/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: a5bf27c2-313b-49f5-b9df-8bb6c0ef5e99      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-DENGZL
/////                 Author: TEST(dengzl)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Share.MessageManage
/////    Project Description:    
/////             Class Name: MQMessageCallBack
/////          Class Version: v1.0.0.0
/////            Create Time: 11/26/2013 9:10:30 AM
/////      Class Description:  
/////======================================================================
/////          Modified Time: 11/26/2013 9:10:30 AM
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System;
using System.ComponentModel.Composition;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Gsafety.PTMS.Bases.Librarys;
using Gsafety.PTMS.ServiceReference.MessageService;
using Jounce.Core.Event;

namespace Gsafety.PTMS.Share
{
    public class MQMessageCallBack : IMessageServiceCallback
    {

        [Import]
        public IEventAggregator _EventAggregator { get; set; }

        public HeartbeatMonitorInfo HeartbeatMonitor { get; set; }

        public MQMessageCallBack()
        {
            CompositionInitializer.SatisfyImports(this);
        }

        public void MessageCallBack(object message)
        {
            try
            {
                if (HeartbeatMonitor != null)
                {
                    if (HeartbeatMonitor.ServiceStatus != MessageServiceStatus.Connected)
                        HeartbeatMonitor.ServiceStatus = MessageServiceStatus.Connected;
                    HeartbeatMonitor.LastHeartBeatTime = DateTime.Now;
                }

                if (message is HeartbeatInfo)
                {
                    return;
                }

                ////GPS V30
                if (message is GPS)
                {
                    _EventAggregator.Publish<GPS>(message as GPS);
                    return;
                }

                if (message is OnOffline)
                {
                    _EventAggregator.Publish<OnOffline>(message as OnOffline);
                    return;
                }

                if (message is AlarmInfo)
                {
                    _EventAggregator.Publish<AlarmInfo>(message as AlarmInfo);
                    return;
                }

                if (message is CameraNoSignalAlert)
                {
                    _EventAggregator.Publish<CameraNoSignalAlert>(message as CameraNoSignalAlert);
                    return;
                }

                if (message is CameraOcclusionAlert)
                {
                    _EventAggregator.Publish<CameraOcclusionAlert>(message as CameraOcclusionAlert);
                    return;
                }

                if (message is FireAlert)
                {
                    _EventAggregator.Publish<FireAlert>(message as FireAlert);
                    return;
                }

                if (message is GpsReceiverFaultAlert)
                {
                    _EventAggregator.Publish<GpsReceiverFaultAlert>(message as GpsReceiverFaultAlert);
                    return;
                }

                if (message is MdvrMemoryCardErrorAlert)
                {
                    _EventAggregator.Publish<MdvrMemoryCardErrorAlert>(message as MdvrMemoryCardErrorAlert);
                    return;
                }

                if (message is OverSpeedAlert)
                {
                    _EventAggregator.Publish<OverSpeedAlert>(message as OverSpeedAlert);
                    return;
                }

                if (message is RegionAlert)
                {
                    _EventAggregator.Publish<RegionAlert>(message as RegionAlert);
                    return;
                }

                if (message is RemoveDeviceSuiteAlertNotify)
                {
                    _EventAggregator.Publish<RemoveDeviceSuiteAlertNotify>(message as RemoveDeviceSuiteAlertNotify);
                    return;
                }

                if (message is TemperatureAlert)
                {
                    _EventAggregator.Publish<TemperatureAlert>(message as TemperatureAlert);
                    return;
                }

                if (message is VoltageAbnormalAlert)
                {
                    _EventAggregator.Publish<VoltageAbnormalAlert>(message as VoltageAbnormalAlert);
                    return;
                }

                if (message is InspectInfo)
                {
                    _EventAggregator.Publish<InspectInfo>(message as InspectInfo);
                    return;
                }

                if (message is UpgradeCMD)
                {
                    _EventAggregator.Publish<UpgradeCMD>(message as UpgradeCMD);
                    return;
                }

                if (message is UpgradeCMD)
                {
                    _EventAggregator.Publish<UpgradeCMD>(message as UpgradeCMD);
                    return;
                }

                if (message is ChangeGroup)
                {
                    _EventAggregator.Publish<ChangeGroup>(message as ChangeGroup);
                    return;
                }

                if (message is ChangeGroupVehicle)
                {
                    _EventAggregator.Publish<ChangeGroupVehicle>(message as ChangeGroupVehicle);
                    return;
                }

                if (message is HandingAlarm)
                {
                    _EventAggregator.Publish<HandingAlarm>(message as HandingAlarm);
                    return;
                }

                if (message is CompleteAlert)
                {
                    _EventAggregator.Publish<CompleteAlert>(message as CompleteAlert);
                    return;
                }

                if (message is DeviceInstall)
                {
                    _EventAggregator.Publish<DeviceInstall>(message as DeviceInstall);
                    return;
                }

                if (message is DeviceMaintain)
                {
                    _EventAggregator.Publish<DeviceMaintain>(message as DeviceMaintain);
                    return;
                }

                if (message is StartInstall)
                {
                    _EventAggregator.Publish<StartInstall>(message as StartInstall);
                    return;
                }

                if (message is DeleteInstall)
                {
                    _EventAggregator.Publish<DeleteInstall>(message as DeleteInstall);
                    return;
                }

                if (message is HandingAlert)
                {
                    _EventAggregator.Publish<HandingAlert>(message as HandingAlert);
                    return;
                }

                if (message is DeleteUser)
                {
                    _EventAggregator.Publish<DeleteUser>(message as DeleteUser);
                    return;
                }

                if (message is ChangeUser)
                {
                    _EventAggregator.Publish<ChangeUser>(message as ChangeUser);
                    return;
                }

                if (message is BasicInfo)
                {
                    _EventAggregator.Publish<BasicInfo>(message as BasicInfo);
                    return;
                }

                if (message is Enviroment)
                {
                    _EventAggregator.Publish<Enviroment>(message as Enviroment);
                    return;
                }

                if (message is Hardware)
                {
                    _EventAggregator.Publish<Hardware>(message as Hardware);
                    return;
                }

                if (message is ElectricFenceReply)
                {
                    _EventAggregator.Publish<ElectricFenceReply>(message as ElectricFenceReply);
                    return;
                }

                if (message is SettingOverSpeedReply)
                {
                    _EventAggregator.Publish<SettingOverSpeedReply>(message as SettingOverSpeedReply);
                    return;
                }

                if (message is EndLocationMonitor)
                {
                    _EventAggregator.Publish<EndLocationMonitor>(message as EndLocationMonitor);
                    return;
                }
                if (message is VideoListResult)
                {
                    _EventAggregator.Publish<VideoListResult>(message as VideoListResult);
                    return;
                }
                if (message is CompleteAlarm)
                {
                    ApplicationContext.Instance.Logger.Log(Jounce.Core.Application.LogSeverity.Information, "MessageManager", "CompleteAlarm Notify is Received!");
                    _EventAggregator.Publish<CompleteAlarm>(message as CompleteAlarm);
                    return;
                }
            }
            catch (Exception ex)
            {
            }
        }
    }
}
