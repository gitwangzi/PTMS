using Gs.PTMS.Common.Data.Enum;
using Gsafety.PTMS.ServiceReference.MessageServiceExt;
using Jounce.Core.Event;
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

namespace Gsafety.PTMS.Share.MessageManage
{
    public class MQCallBackExt : IMessageServiceExtCallback
    {
        [Import]
        public IEventAggregator _EventAggregator { get; set; }

        public HeartbeatMonitorInfo HeartbeatMonitor { get; set; }

        public MQCallBackExt()
        {
            CompositionInitializer.SatisfyImports(this);
        }

        public void MessageCallBack(object message)
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
            else if (message is UserModel)
            {
                UserModel usermodel = message as UserModel;
                ApplicationContext.Instance.MessageClient.Stop = true;
                if (usermodel.MessageType == (int)MessageTypeEnum.ForceLogout)
                {
                    _EventAggregator.Publish<ForceLogoutArg>(new ForceLogoutArg());
                }
            }
            else if (message is GPS)
            {
                GPS gps = message as GPS;
                if (gps != null)
                {
                    gps.GpsTime = gps.GpsTime.Value.ToLocalTime();
                    _EventAggregator.Publish<GPS>(gps);
                }
                return;
            }
            else if (message is OnOfflineEx)
            {
                _EventAggregator.Publish<OnOfflineEx>(message as OnOfflineEx);
                return;
            }
            else if (message is AlarmInfoEx)
            {
                _EventAggregator.Publish<AlarmInfoEx>(message as AlarmInfoEx);
            }
            else if (message is BusinessAlertEx)
            {
                _EventAggregator.Publish<BusinessAlertEx>(message as BusinessAlertEx);
            }
            else if (message is DeviceAlertEx)
            {
                _EventAggregator.Publish<DeviceAlertEx>(message as DeviceAlertEx);
            }
            else if (message is CompleteAlarm)
            {
                _EventAggregator.Publish<CompleteAlarm>(message as CompleteAlarm);
            }
            else if (message is CompleteAlert)
            {
                _EventAggregator.Publish<CompleteAlert>(message as CompleteAlert);
            }
            else if (message is QueryServerFileListMessageResponse)
            {
                _EventAggregator.Publish<QueryServerFileListMessageResponse>(message as QueryServerFileListMessageResponse);
            }
            else if (message is TakePictureMessageResponse)
            {
                _EventAggregator.Publish<TakePictureMessageResponse>(message as TakePictureMessageResponse);
            }
            else if (message is Gsafety.PTMS.ServiceReference.MessageServiceExt.AuthenticationInfo)
            {
                Gsafety.PTMS.ServiceReference.MessageServiceExt.AuthenticationInfo info = message as Gsafety.PTMS.ServiceReference.MessageServiceExt.AuthenticationInfo;
                ApplicationContext.Instance.ServerConfig.Authenticate = info.Code;
            }
            else if (message is Gsafety.PTMS.ServiceReference.MessageServiceExt.Vehicle)
            {
                _EventAggregator.Publish<Gsafety.PTMS.ServiceReference.MessageServiceExt.Vehicle>(message as Gsafety.PTMS.ServiceReference.MessageServiceExt.Vehicle);
            }
        }
    }
}
