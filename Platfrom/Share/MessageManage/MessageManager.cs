/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 7cfc15f3-bda2-4914-a261-05d90413082f      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-LIW
/////                 Author: TEST(liw)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Share
/////    Project Description:    
/////             Class Name: MessageManager
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/8/31 14:48:50
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/8/31 14:48:50
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.ServiceModel;
using Gsafety.PTMS.ServiceReference.MessageService;
using Gsafety.PTMS.ServiceReference.VedioService;
using Jounce.Core.Event;
using System.ComponentModel.Composition;
using System.Collections.Generic;
using System.Reflection;
using System.ServiceModel.Channels;
using System.Xml.Linq;
using System.Linq;
using System.Threading;
using System.Windows.Threading;
using Gsafety.PTMS.Bases.Librarys;
using System.Threading.Tasks;
using System.Xml;
using Jounce.Core.Application;

namespace Gsafety.PTMS.Share
{
    public class MessageManager
    {
        #region Fields

        private bool _initComplete = false;

        private MessageServiceClient _messageClinet;

        private InstanceContext _instanceContext;

        private string _queue;

        private HeartbeatMonitorInfo _heartbeatMonitorInfo;

        private readonly int MonitorHeartbeatTimespan = 10000;

        private readonly int CmdSendTimeout = 2000;
        private readonly int CmdSetSendTimeout = 10000;

        #endregion

        public MessageManager()
        {
            try
            {
                _heartbeatMonitorInfo = new HeartbeatMonitorInfo();
                MQMessageCallBack callBack = new MQMessageCallBack();
                callBack.HeartbeatMonitor = _heartbeatMonitorInfo;
                _instanceContext = new InstanceContext(callBack);
                _queue = Guid.NewGuid().ToString();


                Task.Factory.StartNew(() => HeartBeatTimer_Tick());


                //InitMessage();
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(null, ex);
            }
        }

        private void HeartBeatTimer_Tick()
        {
            while (true)
            {
                if (_heartbeatMonitorInfo.ServiceStatus == MessageServiceStatus.Connected)
                {
                    if (_heartbeatMonitorInfo.IsDisconnection)
                    {
                        _heartbeatMonitorInfo.ServiceStatus = MessageServiceStatus.DisConnected;
                        ApplicationContext.Instance.Logger.LogWaring(GetType().FullName, "Message Service Disconnected");
                    }
                }

                if (_heartbeatMonitorInfo.ServiceStatus == MessageServiceStatus.DisConnected)
                {
                    ApplicationContext.Instance.Logger.LogWaring(GetType().FullName, "ReConnection Message Service");
                    InitMessage();
                }
                Thread.Sleep(MonitorHeartbeatTimespan);
            }
        }

        private void InitMessage()
        {
            string url = GetMessageServiceUrl(typeof(MessageServiceClient).Name);
            if (!string.IsNullOrWhiteSpace(url))
            {
                EndpointAddress address = new EndpointAddress(url);
                BindingElementCollection elements = new BindingElementCollection();
                elements.Add(new BinaryMessageEncodingBindingElement());
                elements.Add(new TcpTransportBindingElement());
                //TcpTransportBindingElement element = new TcpTransportBindingElement();
                CustomBinding customBinding = new CustomBinding(elements);

                //BinaryMessageEncodingBindingElement be = new BinaryMessageEncodingBindingElement();
                //be.MaxReadPoolSize = 16;
                //be.MaxSessionSize = 2048;
                //be.MaxWritePoolSize = 16;
                //be.MessageVersion = MessageVersion.Default;
                //XmlDictionaryReaderQuotas quotas = be.ReaderQuotas;
                customBinding.SendTimeout = System.TimeSpan.FromSeconds(50);
                customBinding.ReceiveTimeout = System.TimeSpan.MaxValue;

                //customBinding.Elements.Find<TransportBindingElement>().MaxReceivedMessageSize = int.MaxValue;

                customBinding.Elements.Find<TcpTransportBindingElement>().ConnectionBufferSize = 500000;
                //customBinding.Elements.BinaryMessageEncoding
                customBinding.Elements.Find<TcpTransportBindingElement>().MaxBufferSize = int.MaxValue;
                customBinding.Elements.Find<TcpTransportBindingElement>().MaxReceivedMessageSize = int.MaxValue;

                _messageClinet = new MessageServiceClient(_instanceContext, customBinding, address);
                _messageClinet.InitCompleted += _messageClinet_InitCompleted;
                _messageClinet.InnerChannel.Faulted += InnerChannel_Faulted;
                _messageClinet.SendGetVideoListCMDCompleted += _messageClinet_SendGetVideoListCMDCompleted;
                _heartbeatMonitorInfo.ServiceStatus = MessageServiceStatus.RequestConnect;
                //_messageClinet.InitAsync(_queue, ApplicationContext.Instance.AuthenticationInfo.GetRuleInfo());
            }
            else
            {
                Exception exception = new Exception("Can not found MessageServiceClient Config!");
                ApplicationContext.Instance.Logger.LogException(GetType().FullName, exception);
            }
        }

        void _messageClinet_SendGetVideoListCMDCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                ApplicationContext.Instance.Logger.LogInforMession(GetType().FullName, e.Error.ToString());
            }
            if (e.Cancelled)
            {
                ApplicationContext.Instance.Logger.LogInforMession(GetType().FullName, e.Cancelled.ToString());
            }
        }


        void _messageClinet_InitCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            try
            {
                if (e.Error == null)
                {
                    _initComplete = true;
                    MessageActionCache.InvokeAction();
                    ApplicationContext.Instance.Logger.LogInforMession(null, string.Format("Client {0} Init Completed", _queue));
                    _heartbeatMonitorInfo.ServiceStatus = MessageServiceStatus.Connected;
                    Register();
                }
                else
                {
                    _initComplete = false;
                    _heartbeatMonitorInfo.ServiceStatus = MessageServiceStatus.DisConnected;
                    ApplicationContext.Instance.Logger.LogInforMession(GetType().FullName, e.Error.ToString());
                }
                _heartbeatMonitorInfo.LastHeartBeatTime = DateTime.Now;
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(this.GetType().FullName, ex);
            }
        }

        public void Register()
        {
            GetOnOfflineMessage();
            GetDeviceInstallMessage();
            GetDeviceMaintainMessage();
            GetRemoveDeviceSuiteAlertNotifyMessage();
            GetDeleteUserMessage();
            GetChangeUserMessage();

            if (ApplicationContext.Instance.AuthenticationInfo.MonitorFunction)
            {
                ApplicationContext.Instance.MessageManager.GetLocationMonitorEndMessage();
            }

            if (ApplicationContext.Instance.AuthenticationInfo.AlarmFunction)
            {
                ApplicationContext.Instance.MessageManager.GetAlarmMessage();
                ApplicationContext.Instance.MessageManager.GetCompleteAlarmMessage();
            }

            if (ApplicationContext.Instance.AuthenticationInfo.AlertFunction)
            {
                ApplicationContext.Instance.MessageManager.GetOpenOrCloseDoorAbnormalAlertMessage();
                ApplicationContext.Instance.MessageManager.GetOverSpeedAlertMessage();
                ApplicationContext.Instance.MessageManager.GetRegionAlertMessage();
                ApplicationContext.Instance.MessageManager.GetCompleteAlertMessage();
            }

            if (ApplicationContext.Instance.AuthenticationInfo.TrafficFunction)
            {
                ApplicationContext.Instance.MessageManager.GetFenceReplyMessage();
                ApplicationContext.Instance.MessageManager.GetSettingOverSpeedReplyMessage();
            }
            ApplicationContext.Instance.MessageManager.GetChangeGroupMessage();
            ApplicationContext.Instance.MessageManager.GetChangeGroupVehicleMessage();

            ApplicationContext.Instance.Logger.Log(LogSeverity.Information, "MessageManager", "Register to Rabbit MQ Successfully!");
        }

        void InnerChannel_Faulted(object sender, EventArgs e)
        {
            _initComplete = false;
            _heartbeatMonitorInfo.ServiceStatus = MessageServiceStatus.DisConnected;
        }

        private string GetMessageServiceUrl(string serviceName)
        {
            var doc = XElement.Parse(ApplicationContext.Instance.ServerConfig.ServiceUrlConfig);
            return doc.Descendants("add").FirstOrDefault(x => x.Attribute("key").Value.Equals(serviceName)).Attribute("value").Value;

        }

        #region

        #region

        public void GetAlarmGpsMessage(string mdvrId)
        {
            var result = MessageActionCache.Add(GetAlarmGpsMessage, mdvrId);
            if (_initComplete && result)
                _messageClinet.GetAlarmGpsMessageAsync(mdvrId);
        }

        public void CancelAlarmGpsMessage(string mdvrId)
        {
            try
            {
                var result = MessageActionCache.Remove(GetAlarmGpsMessage, mdvrId);
                if (_initComplete && result)
                    _messageClinet.CancelAlarmGpsMessageAsync(mdvrId);
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(this.GetType().FullName, ex);

            }
        }

        public void GetAlarmMessage()
        {
            try
            {
                var result = MessageActionCache.Add(GetAlarmMessage);
                if (_initComplete && result)
                    _messageClinet.GetAlarmMessageAsync();
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(this.GetType().FullName, ex);

            }
        }

        public void CancelAlarmMessage()
        {
            try
            {
                var result = MessageActionCache.Remove(GetAlarmMessage);
                if (_initComplete && result)
                    _messageClinet.CancelAlarmMessageAsync();
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(this.GetType().FullName, ex);
            }
        }

        public void GetCancelAlarmMessage()
        {
            try
            {
                var result = MessageActionCache.Add(CancelAlarmMessage);
                if (_initComplete && result)
                    _messageClinet.GetCancelAlarmMessageAsync();
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(this.GetType().FullName, ex);

            }
        }



        public bool GetMonitorGpsMessage(LocationMonitorCMD locationMonitorCmd)
        {
            try
            {
                locationMonitorCmd.SessionID = _queue;
                //Task task = _messageClinet.RequestMonitorGpsMessage(locationMonitorCmd);
                //task.Wait(CmdSendTimeout);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public void CancelMonitorGpsMessage(CancelLocationMonitorCMD cancelLocationMonitorCmd)
        {
            try
            {
                cancelLocationMonitorCmd.SessionID = _queue;
                _messageClinet.CancelMonitorGpsMessageAsync(cancelLocationMonitorCmd);
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(this.GetType().FullName, ex);

            }
        }

        public void GetLocationMonitorEndMessage()
        {
            try
            {
                var result = MessageActionCache.Add(GetLocationMonitorEndMessage);
                if (_initComplete && result)
                    _messageClinet.GetLocationMonitorEndMessageAsync(_queue);
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(this.GetType().FullName, ex);

            }
        }

        public void GetCameraNoSignalAlertMessage()
        {
            try
            {
                var result = MessageActionCache.Add(GetCameraNoSignalAlertMessage);
                if (_initComplete && result)
                    _messageClinet.GetCameraNoSignalAlertMessageAsync();
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(this.GetType().FullName, ex);

            }
        }

        public void CancelCameraNoSignalAlertMessage()
        {
            try
            {
                var result = MessageActionCache.Remove(GetCameraNoSignalAlertMessage);
                if (_initComplete && result)
                    _messageClinet.CancelCameraNoSignalAlertMessageAsync();
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(this.GetType().FullName, ex);

            }
        }

        public void GetCameraOcclusionAlertMessage()
        {
            try
            {
                var result = MessageActionCache.Add(GetCameraOcclusionAlertMessage);
                if (_initComplete && result)
                    _messageClinet.GetCameraOcclusionAlertMessageAsync();

            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(this.GetType().FullName, ex);

            }
        }

        public void CancelCameraOcclusionAlertMessage()
        {
            try
            {
                var result = MessageActionCache.Remove(CancelCameraOcclusionAlertMessage);
                if (_initComplete && result)
                    _messageClinet.CancelCameraOcclusionAlertMessageAsync();
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(this.GetType().FullName, ex);

            }

        }

        public void GetFireAlertMessage()
        {
            try
            {
                var result = MessageActionCache.Add(GetFireAlertMessage);
                if (_initComplete && result)
                    _messageClinet.GetFireAlertMessageAsync();
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(this.GetType().FullName, ex);
            }
        }

        public void CancelFireAlertMessage()
        {
            try
            {
                var result = MessageActionCache.Remove(GetFireAlertMessage);
                if (_initComplete && result)
                    _messageClinet.CancelFireAlertMessageAsync();
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(this.GetType().FullName, ex);
            }
        }

        public void GetGpsReceiverFaultAlertMessage()
        {
            try
            {
                var result = MessageActionCache.Add(GetGpsReceiverFaultAlertMessage);
                if (_initComplete && result)
                    _messageClinet.GetGpsReceiverFaultAlertMessageAsync();
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(this.GetType().FullName, ex);
            }
        }

        public void CancelGpsReceiverFaultAlertMessage()
        {
            try
            {
                var result = MessageActionCache.Remove(CancelGpsReceiverFaultAlertMessage);
                if (_initComplete && result)
                    _messageClinet.CancelGpsReceiverFaultAlertMessageAsync();
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(this.GetType().FullName, ex);
            }

        }

        public void GetMdvrMemoryCardErrorAlertMessage()
        {
            try
            {
                var result = MessageActionCache.Add(GetMdvrMemoryCardErrorAlertMessage);
                if (_initComplete && result)
                    _messageClinet.GetMdvrMemoryCardErrorAlertMessageAsync();
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(this.GetType().FullName, ex);
            }
        }

        public void CancelMdvrMemoryCardErrorAlertMessage()
        {
            try
            {
                var result = MessageActionCache.Remove(GetMdvrMemoryCardErrorAlertMessage);
                if (_initComplete && result)
                    _messageClinet.CancelMdvrMemoryCardErrorAlertMessageAsync();
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(this.GetType().FullName, ex);
            }
        }

        public void GetOpenOrCloseDoorAbnormalAlertMessage()
        {
            try
            {
                var result = MessageActionCache.Add(GetOpenOrCloseDoorAbnormalAlertMessage);
                if (_initComplete && result)
                    _messageClinet.GetOpenOrCloseDoorAbnormalAlertMessageAsync();
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(this.GetType().FullName, ex);
            }
        }

        public void CancelOpenOrCloseDoorAbnormalAlertMessage()
        {
            try
            {
                var result = MessageActionCache.Remove(GetOpenOrCloseDoorAbnormalAlertMessage);
                if (_initComplete && result)
                    _messageClinet.CancelOpenOrCloseDoorAbnormalAlertMessageAsync();
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(this.GetType().FullName, ex);
            }
        }

        public void GetOverSpeedAlertMessage()
        {
            try
            {
                var result = MessageActionCache.Add(GetOverSpeedAlertMessage);
                if (_initComplete && result)
                    _messageClinet.GetOverSpeedAlertMessageAsync();
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(this.GetType().FullName, ex);
            }
        }

        public void CancelOverSpeedAlertMessage()
        {
            try
            {
                var result = MessageActionCache.Remove(GetOverSpeedAlertMessage);
                if (_initComplete && result)
                    _messageClinet.CancelOverSpeedAlertMessageAsync();
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(this.GetType().FullName, ex);
            }
        }

        public void GetRegionAlertMessage()
        {
            try
            {
                var result = MessageActionCache.Add(GetRegionAlertMessage);
                if (_initComplete && result)
                    _messageClinet.GetRegionAlertMessageAsync();
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(this.GetType().FullName, ex);
            }
        }

        public void CancelRegionAlertMessage()
        {
            try
            {
                var result = MessageActionCache.Remove(CancelRegionAlertMessage);
                if (_initComplete && result)
                    _messageClinet.CancelRegionAlertMessageAsync();
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(this.GetType().FullName, ex);
            }
        }

        public void GetRemoveDeviceSuiteAlertNotifyMessage()
        {
            try
            {
                var result = MessageActionCache.Add(GetRemoveDeviceSuiteAlertNotifyMessage);
                if (_initComplete && result)
                    _messageClinet.GetRemoveDeviceSuiteAlertNotifyMessageAsync();
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(this.GetType().FullName, ex);
            }
        }

        public void CancelRemoveDeviceSuiteAlertNotifyMessage()
        {
            try
            {
                var result = MessageActionCache.Remove(GetRemoveDeviceSuiteAlertNotifyMessage);
                if (_initComplete && result)
                    _messageClinet.CancelRemoveDeviceSuiteAlertNotifyMessageAsync();
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(this.GetType().FullName, ex);
            }
        }

        public void GetTemperatureAlertMessage()
        {
            try
            {
                var result = MessageActionCache.Add(GetTemperatureAlertMessage);
                if (_initComplete && result)
                    _messageClinet.GetTemperatureAlertMessageAsync();
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(this.GetType().FullName, ex);
            }
        }

        public void CancelTemperatureAlertMessage()
        {
            try
            {
                var result = MessageActionCache.Remove(GetTemperatureAlertMessage);
                if (_initComplete && result)
                    _messageClinet.CancelTemperatureAlertMessageAsync();
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(this.GetType().FullName, ex);
            }
        }

        public void GetVoltageAbnormalAlertMessage()
        {
            try
            {
                var result = MessageActionCache.Add(GetVoltageAbnormalAlertMessage);
                if (_initComplete && result)
                    _messageClinet.GetVoltageAbnormalAlertMessageAsync();
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(this.GetType().FullName, ex);
            }
        }

        public void CancelVoltageAbnormalAlertMessage()
        {
            try
            {
                var result = MessageActionCache.Remove(GetVoltageAbnormalAlertMessage);
                if (_initComplete && result)
                    _messageClinet.CancelVoltageAbnormalAlertMessageAsync();
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(this.GetType().FullName, ex);
            }
        }

        public void GetInspectMessage()
        {
            try
            {
                var result = MessageActionCache.Add(GetInspectMessage);
                if (_initComplete && result)
                    _messageClinet.GetInspectMessageAsync();
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(this.GetType().FullName, ex);
            }
        }

        public void CancelInspectMessage()
        {
            try
            {
                var result = MessageActionCache.Remove(GetInspectMessage);
                if (_initComplete && result)
                    _messageClinet.CancelInspectMessageAsync();
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(this.GetType().FullName, ex);
            }
        }

        public void GetOnOfflineMessage()
        {
            try
            {
                var result = MessageActionCache.Add(GetOnOfflineMessage);
                if (_initComplete && result)
                    _messageClinet.GetOnOfflineMessageAsync();
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(this.GetType().FullName, ex);
            }
        }

        public void CancelOnOfflineMessage()
        {
            try
            {
                var result = MessageActionCache.Remove(GetOnOfflineMessage);
                if (_initComplete && result)
                    _messageClinet.CancelOnOfflineMessageAsync();
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(this.GetType().FullName, ex);
            }
        }

        public void GetSuiteRunintStatusMessage()
        {
            try
            {
                var result = MessageActionCache.Remove(GetSuiteRunintStatusMessage);
                if (_initComplete && result)
                    _messageClinet.GetSuiteRunintStatusMessageAsync();
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(this.GetType().FullName, ex);
            }
        }

        public void GetFenceReplyMessage()
        {
            try
            {
                var result = MessageActionCache.Add(GetFenceReplyMessage);
                if (_initComplete && result)
                    _messageClinet.GetFenceReplyMessageAsync();
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(this.GetType().FullName, ex);
            }
        }

        public void GetSettingOverSpeedReplyMessage()
        {
            try
            {
                var result = MessageActionCache.Add(GetSettingOverSpeedReplyMessage);
                if (_initComplete && result)
                    _messageClinet.GetSettingOverSpeedReplyMessageAsync();
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(this.GetType().FullName, ex);
            }
        }

        #endregion

        #region

        //public void SendHandingAlarmMessage(HandingAlarm mode)
        //{
        //    _messageClinet.SendHandingAlarmMessageAsync(mode);
        //}

        //public void GetHandingAlarmMessage()
        //{
        //    MessageActionCache.Add(GetHandingAlarmMessage);
        //    if (_initComplete)
        //        _messageClinet.GetHandingAlarmMessageAsync();
        //}

        public void GetCompleteAlarmMessage()
        {
            try
            {
                MessageActionCache.Add(GetCompleteAlarmMessage);
                if (_initComplete)
                    _messageClinet.GetCompleteAlarmMessageAsync();
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(this.GetType().FullName, ex);
            }
        }

        public void SendDeviceInstallMessage(DeviceInstall model)
        {
            try
            {
                _messageClinet.SendDeviceInstallMessageAsync(model);
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(this.GetType().FullName, ex);
            }
        }

        public void SendChangeGroupMessage(ChangeGroup model)
        {
            try
            {
                _messageClinet.SendChangeGroupMessageAsync(model);
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(this.GetType().FullName, ex);
            }
        }

        public void SendChangeGroupVehicleMessage(ChangeGroupVehicle model)
        {
            try
            {
                _messageClinet.SendChangeGroupVechleMessageAsync(model);
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(this.GetType().FullName, ex);
            }
        }


        public void GetDeviceInstallMessage()
        {
            try
            {
                MessageActionCache.Add(GetDeviceInstallMessage);
                if (_initComplete)
                    _messageClinet.GetDeviceInstallMessageAsync();
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(this.GetType().FullName, ex);
            }
        }

        public void SendDeviceMaintainMessage(DeviceMaintain model)
        {
            try
            {
                _messageClinet.SendDeviceMaintainMessageAsync(model);
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(this.GetType().FullName, ex);
            }
        }

        public void GetDeviceMaintainMessage()
        {
            try
            {
                MessageActionCache.Add(GetDeviceMaintainMessage);
                if (_initComplete)
                    _messageClinet.GetDeviceMaintainMessageAsync();
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(this.GetType().FullName, ex);
            }
        }

        public void SendHandingAlertMessage(HandingAlert model)
        {
            try
            {
                _messageClinet.SendHandingAlertMessageAsync(model);
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(this.GetType().FullName, ex);
            }
        }

        public void GetHandingAlertMessage()
        {
            try
            {
                MessageActionCache.Add(GetHandingAlertMessage);
                if (_initComplete)
                    _messageClinet.GetHandingAlertMessageAsync();
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(this.GetType().FullName, ex);
            }
        }

        public void SendCompleteAlertMessage(CompleteAlert model)
        {
            try
            {
                _messageClinet.SendCompleteAlertMessageAsync(model);
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(this.GetType().FullName, ex);
            }
        }

        public void GetCompleteAlertMessage()
        {
            try
            {
                MessageActionCache.Add(GetCompleteAlertMessage);
                if (_initComplete)
                    _messageClinet.GetCompleteAlertMessageAsync();
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(this.GetType().FullName, ex);
            }
        }

        public void SendStartInstallMessage(StartInstall model)
        {
            try
            {
                _messageClinet.SendStartInstallMessageAsync(model);
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(this.GetType().FullName, ex);
            }
        }

        public void GetStartInstallMessage()
        {
            try
            {
                MessageActionCache.Add(GetStartInstallMessage);
                if (_initComplete)
                    _messageClinet.GetStartInstallMessageAsync();
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(this.GetType().FullName, ex);
            }
        }

        public void SendDeleteInstallMessage(DeleteInstall model)
        {
            try
            {
                _messageClinet.SendDeleteInstallMessageAsync(model);
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(this.GetType().FullName, ex);
            }
        }

        public void GetDeleteInstallMessage()
        {
            try
            {
                MessageActionCache.Add(GetDeleteInstallMessage);
                if (_initComplete)
                    _messageClinet.GetDeleteInstallMessageAsync();
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(this.GetType().FullName, ex);
            }
        }

        public void GetDeleteUserMessage()
        {
            try
            {
                MessageActionCache.Add(GetDeleteUserMessage);
                if (_initComplete)
                    _messageClinet.GetDeleteUserMessageAsync();
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(this.GetType().FullName, ex);
            }
        }

        public void GetChangeUserMessage()
        {
            try
            {
                MessageActionCache.Add(GetChangeUserMessage);
                if (_initComplete)
                    _messageClinet.GetChangeUserMessageAsync();
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(this.GetType().FullName, ex);
            }
        }

        public void SendDeleteUserMessage(DeleteUser model)
        {
            try
            {
                _messageClinet.SendDeleteUserMessageAsync(model);
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(this.GetType().FullName, ex);
            }
        }

        public void SendChangeUserMessage(ChangeUser model)
        {
            try
            {
                _messageClinet.SendChangeUserMessageAsync(model);
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(this.GetType().FullName, ex);
            }
        }

        public bool SendRouteMessage(RouteCMD model)
        {

            try
            {
                if (_heartbeatMonitorInfo.ServiceStatus != MessageServiceStatus.Connected || model == null)
                    return false;
                //Task task = _messageClinet.SendRouteMessageEx(model);
                //return task.Wait(CmdSendTimeout);
                return false;
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(this.GetType().FullName, ex);
                return false;
            }
        }

        public bool BatchSendRouteMessage(List<RouteCMD> routes)
        {
            try
            {
                if (_heartbeatMonitorInfo.ServiceStatus != MessageServiceStatus.Connected || routes == null || routes.Count == 0)
                    return false;
                //Task task = _messageClinet.SendRouteMessageSetEx(new System.Collections.ObjectModel.ObservableCollection<RouteCMD>(routes));
                //return task.Wait(CmdSetSendTimeout);
                return false;
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(this.GetType().FullName, ex);
                return false;
            }
        }

        public bool SendTravelPlanCMD(TravelPlanCMD travelPlan)
        {
            try
            {
                if (_heartbeatMonitorInfo.ServiceStatus != MessageServiceStatus.Connected || travelPlan == null)
                    return false;
                ////Task task = _messageClinet.SendTravelPlanMessageEx(travelPlan);
                //return task.Wait(CmdSendTimeout);
                return false;
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(this.GetType().FullName, ex);
                return false;
            }
        }

        public bool BatchSendTravePlanCMD(List<TravelPlanCMD> traveplans)
        {

            try
            {
                if (_heartbeatMonitorInfo.ServiceStatus != MessageServiceStatus.Connected || traveplans == null || traveplans.Count == 0)
                    return false;
                //Task task = _messageClinet.SendTravelPlanMessageSetEx(new System.Collections.ObjectModel.ObservableCollection<TravelPlanCMD>(traveplans));
                //return task.Wait(CmdSetSendTimeout);
                return false;
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(this.GetType().FullName, ex);
                return false;
            }
        }

        public void SendUpgradeNotifyMessage(UpgradeNotify model)
        {
            _messageClinet.SendUpgradeNotifyMessageAsync(model);
        }
        #endregion

        #region

        public bool SendElectricFenceCMD(ElectircFenceSendSettingModel item)
        {
            try
            {
                if (_heartbeatMonitorInfo.ServiceStatus != MessageServiceStatus.Connected || item == null)
                    return false;
                ApplicationContext.Instance.Logger.Log(LogSeverity.Information, string.Empty, item.ToString());
                //Task task = _messageClinet.SendElectricFenceCMDEx(item);
                //return task.Wait(CmdSendTimeout);
                return false;
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(this.GetType().FullName, ex);
                return false;
            }
        }

        public bool BatchSendElectricFenceCMD(List<ElectircFenceSendSettingModel> electricFences)
        {
            try
            {
                if (_heartbeatMonitorInfo.ServiceStatus != MessageServiceStatus.Connected || electricFences == null || electricFences.Count == 0)
                    return false;
                foreach (var item in electricFences)
                {
                    ApplicationContext.Instance.Logger.Log(LogSeverity.Information, string.Empty, item.ToString());
                }
                //Task task = _messageClinet.SendElectricFenceCMDSetEx(new System.Collections.ObjectModel.ObservableCollection<ElectircFenceSendSettingModel>(electricFences));

                //return task.Wait(CmdSetSendTimeout);
                return false;

            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(this.GetType().FullName, ex);
                return false;
            }

        }

        public void SendUpgradeCMD(UpgradeCMD item)
        {
            try
            {
                _messageClinet.SendUpgradeCMDAsync(item);
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(this.GetType().FullName, ex);
            }
        }

        public void SendGetUpgradeStatusCMD(UpgradeStatusCMD item)
        {
            try
            {
                _messageClinet.SendGetUpgradeStatusCMDAsync(item);
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(this.GetType().FullName, ex);

            }
        }



        public bool SendSettingOverSpeedCMD(SettingOverSpeedCMD item)
        {
            try
            {
                if (_heartbeatMonitorInfo.ServiceStatus != MessageServiceStatus.Connected || item == null)
                    return false;
                //Task task = _messageClinet.SendSettingOverSpeedEx(item);
                //return task.Wait(CmdSendTimeout);
                return false;
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(this.GetType().FullName, ex);
                return false;
            }
        }

        public bool BatchSendSettingOverSpeedCMD(List<SettingOverSpeedCMD> overSpeeds)
        {
            try
            {
                if (_heartbeatMonitorInfo.ServiceStatus != MessageServiceStatus.Connected || overSpeeds == null || overSpeeds.Count == 0)
                    return false;

                //Task task = _messageClinet.SendSettingOverSpeedSetEx(new System.Collections.ObjectModel.ObservableCollection<SettingOverSpeedCMD>(overSpeeds));
                //return task.Wait(CmdSetSendTimeout);
                return false;
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(this.GetType().FullName, ex);
                return false;
            }
        }
        public void SendSettingGpsCMD(Gsafety.PTMS.ServiceReference.MessageService.GpsSendUpModel cmd)
        {
            try
            {
                _messageClinet.SendSettingGpsUploadCMDSetAsync(cmd);
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(this.GetType().FullName, ex);
            }

        }
        public void SendAbnormalDoorSettingCMD(Gsafety.PTMS.ServiceReference.MessageService.AbnormalDoorSendUpModel cmd)
        {
            try
            {
                _messageClinet.SendSettingAbnormalDoorUploadCMDSetAsync(cmd);
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(this.GetType().FullName, ex);
            }

        }
        public void SendTemperatureSettingCMD(Gsafety.PTMS.ServiceReference.MessageService.TemperatureSendUpModel cmd)
        {
            try
            {
                _messageClinet.SendSettingTemperatureUploadCMDSetAsync(cmd);
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(this.GetType().FullName, ex);
            }
        }

        public void SendSettingOneKeyAlarmUploadCMD(Gsafety.PTMS.ServiceReference.MessageService.OneKeyAlarmSendUpModel cmd)
        {
            try
            {
                _messageClinet.SendSettingOneKeyAlarmUploadCMDSetAsync(cmd);
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(this.GetType().FullName, ex);
            }

        }
        public void SendInfomationCMD(Gsafety.PTMS.ServiceReference.MessageService.SendInfomationModel cmd)
        {
            try
            {
                //_messageClinet.SendInfomationCMDEx(cmd);
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(this.GetType().FullName, ex);
            }

        }
        public void SendSettingOverSpeedUploadCMD(Gsafety.PTMS.ServiceReference.MessageService.OverSpeedSendSettingModel cmd)
        {
            try
            {
                _messageClinet.SendOverSpeedSettingAsync(cmd);
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(this.GetType().FullName, ex);
            }

        }

        public bool SendSettingElectircFenceUploadCMD(Gsafety.PTMS.ServiceReference.MessageService.ElectircFenceSendSettingModel cmd)
        {
            try
            {
                if (_heartbeatMonitorInfo.ServiceStatus != MessageServiceStatus.Connected || cmd == null)
                    return false;
                //Task task = _messageClinet.SendSettingElectircFenceUploadCMDEx(cmd);
                //return task.Wait(CmdSetSendTimeout);
                return false;
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(this.GetType().FullName, ex);
                return false;
            }
        }
        #endregion

        #endregion

        public void SendGetVideoListCMD(QueryMdvrFileList model)
        {
            try
            {
                _messageClinet.SendGetVideoListCMDAsync(model);

            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(this.GetType().FullName, ex);
            }
        }

        public void SendDownloadMdvrFile(DownloadFile model)
        {
            try
            {
                _messageClinet.SendDownloadMdvrFileAsync(model);
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(this.GetType().FullName, ex);
            }
        }

        public void GetSettingVideoListReplyMessage()
        {
            try
            {
                var result = MessageActionCache.Add(GetSettingVideoListReplyMessage);
                if (_initComplete && result)
                    _messageClinet.GetSettingVideoListReplyMessageAsync();
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(this.GetType().FullName, ex);
            }
        }

        public void GetChangeGroupMessage()
        {
            try
            {
                var result = MessageActionCache.Add(GetChangeGroupMessage);
                if (_initComplete && result)
                    _messageClinet.GetChangeGroupMessageAsync();
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(this.GetType().FullName, ex);
            }
        }

        public void GetChangeGroupVehicleMessage()
        {
            try
            {
                var result = MessageActionCache.Add(GetChangeGroupVehicleMessage);
                if (_initComplete && result)
                    _messageClinet.GetChangeGroupVechleMessageAsync();
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(this.GetType().FullName, ex);
            }
        }
    }


}
