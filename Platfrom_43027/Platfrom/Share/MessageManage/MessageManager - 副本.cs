/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 7cfc15f3-bda2-4914-a261-05d90413082f      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-LIW
/////                 Author: TEST(liw)
/////======================================================================
/////           Project Name: Gsafety.Ant.Share
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
using Gsafety.Ant.ServiceReference.MessageService;
using Jounce.Core.Event;
using System.ComponentModel.Composition;
using System.Collections.Generic;
using System.Reflection;
using System.ServiceModel.Channels;
using System.Xml.Linq;
using System.Linq;
using System.Threading;
using System.Windows.Threading;
using Gsafety.Ant.Bases.Librarys;
using System.Threading.Tasks;

namespace Gsafety.Ant.Share
{
    public class MessageManager
    {
        #region Fields

        /// <summary>
        /// 是否初始化完成
        /// </summary>
        private bool _initComplete = false;

        /// <summary>
        /// 消息接口
        /// </summary>
        private MessageServiceClient _messageClinet;

        /// <summary>
        /// 服务实例
        /// </summary>
        private InstanceContext _instanceContext;


        /// <summary>
        /// 队列
        /// </summary>
        private string _queue;

        /// <summary>
        /// 服务心跳消息
        /// </summary>
        private HeartbeatMonitorInfo _heartbeatMonitorInfo;

        private readonly int MonitorHeartbeatTimespan = 20000;

        #endregion

        /// <summary>
        /// 初始化构造函数
        /// </summary>
        /// <param name="eventAggregator"></param>
        public MessageManager()
        {
            try
            {
                _heartbeatMonitorInfo = new HeartbeatMonitorInfo();
                MQMessageCallBack callBack = new MQMessageCallBack();
                callBack.HeartbeatMonitor = _heartbeatMonitorInfo;
                _instanceContext = new InstanceContext(callBack);                  ////回调类
                _queue = Guid.NewGuid().ToString();                                ////队列号，客户端唯一


                Task.Factory.StartNew(() => HeartBeatTimer_Tick());

                //////初始化
                //InitMessage();
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogError(null, ex);
            }
        }

        /// <summary>
        /// 心跳检测
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HeartBeatTimer_Tick()
        {
            while (true)
            {
                if (_heartbeatMonitorInfo.ServiceStatus == ConnectStatus.Connected)
                {
                    if (_heartbeatMonitorInfo.IsDisconnection)
                    {
                        _heartbeatMonitorInfo.ServiceStatus = ConnectStatus.DisConnected;
                        ApplicationContext.Instance.Logger.LogWaring(GetType().FullName, "Message Service Disconnected");
                    }
                }

                if (_heartbeatMonitorInfo.ServiceStatus == ConnectStatus.DisConnected)
                {
                    ApplicationContext.Instance.Logger.LogWaring(GetType().FullName, "ReConnection Message Service");
                    InitMessage();
                }
                Thread.Sleep(MonitorHeartbeatTimespan);
            }
        }

        /// <summary>
        /// 初始化
        /// </summary>
        private void InitMessage()
        {
            string url = GetMessageServiceUrl(typeof(MessageServiceClient).Name);
            if (!string.IsNullOrWhiteSpace(url))
            {
                ////自定义customBinding绑定
                ////TODO:是否需要Try Catch，重连
                EndpointAddress address = new EndpointAddress(url);
                BindingElement element = new TcpTransportBindingElement();
                CustomBinding customBinding = new CustomBinding(element);

                customBinding.SendTimeout = System.TimeSpan.FromSeconds(10);      ////发送消息时超时时间 5秒
                customBinding.ReceiveTimeout = System.TimeSpan.MaxValue;         ////禁用连接超时时间 最大                

                _messageClinet = new MessageServiceClient(_instanceContext, customBinding, address);
                _messageClinet.InitCompleted += _messageClinet_InitCompleted;    ////初始化回调
                _messageClinet.InnerChannel.Faulted += InnerChannel_Faulted;     ////第一次发生错误回调
                _heartbeatMonitorInfo.ServiceStatus = ConnectStatus.RequestConnect;

                _messageClinet.InitAsync(_queue, ApplicationContext.Instance.AuthenticationInfo.GetRuleInfo());
            }
            else
            {
                throw new Exception("Can not found MessageServiceClient Config!");
            }
        }

        /// <summary>
        /// 初始完成
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void _messageClinet_InitCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                ////初始化成功，执行方法
                _initComplete = true;
                MessageActionCache.InvokeAction();
                ApplicationContext.Instance.Logger.LogInforMession(null, string.Format("Client {0} Init Completed", _queue));
                _heartbeatMonitorInfo.ServiceStatus = ConnectStatus.Connected;

            }
            else
            {
                _heartbeatMonitorInfo.ServiceStatus = ConnectStatus.DisConnected;
                ////初始化发送错误
                ApplicationContext.Instance.Logger.LogInforMession(null, e.Error.ToString());
                ApplicationContext.Instance.Logger.LogInforMession(null, "An error occurs in init client, recconnect ....");
            }
            _heartbeatMonitorInfo.LastHeartBeatTime = DateTime.Now;
        }



        /// <summary>
        /// 查找消息服务Url
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        private string GetMessageServiceUrl(string serviceName)
        {
            var doc = XElement.Parse(ApplicationContext.Instance.ServerConfig.ServiceUrlConfig);
            return doc.Descendants("add").FirstOrDefault(x => x.Attribute("key").Value.Equals(serviceName)).Attribute("value").Value;
        }

        /// <summary>
        /// 第一次发送错误
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void InnerChannel_Faulted(object sender, EventArgs e)
        {
            try
            {
                InitMessage();
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogError(null, ex);
            }

            //(sender as ICommunicationObject).Abort();
            //if (sender is IMessageService)
            //{
            //    sender = _messageClinet.ChannelFactory.CreateChannel();
            //}

            //_heartbeatMonitorInfo.ServiceStatus = ConnectStatus.DisConnected;
            //_heartbeatMonitorInfo.LastHeartBeatTime = DateTime.Now;
            //try
            //{
            //    ApplicationContext.Instance.Logger.LogInforMession(null, "An error occurs,recconnect in message service.......");
            //    ////断开连接
            //    _messageClinet.InnerChannel.Close();
            //    _initComplete = false;
            //}
            //catch (Exception ex)
            //{
            //    ApplicationContext.Instance.Logger.LogError(null, ex);
            //}
            //finally
            //{
            //    _heartbeatMonitorInfo.ServiceStatus = ConnectStatus.DisConnected;
            //}
        }

        #region 公布出来的方法

        #region 业务类

        /// <summary>
        /// 获取一键报警的GPS数据
        /// </summary>
        /// <param name="mdvrId"></param>
        public void GetAlarmGpsMessage(string mdvrId)
        {
            ////断线重连使用,记住执行的方法            ;
            if (MessageActionCache.Add(GetAlarmGpsMessage, mdvrId) && _initComplete)
                _messageClinet.GetAlarmGpsMessageAsync(mdvrId);
        }

        /// <summary>
        /// 取消一键报警的GPS数据
        /// </summary>
        /// <param name="mdvrId"></param>
        public void CancelAlarmGpsMessage(string mdvrId)
        {
            MessageActionCache.Remove(GetAlarmGpsMessage, mdvrId);
            if (_initComplete)
                _messageClinet.CancelAlarmGpsMessageAsync(mdvrId);
        }

        /// <summary>
        /// 获取一键报警消息
        /// </summary>
        public void GetAlarmMessage()
        {
            if (MessageActionCache.Add(GetAlarmMessage) && _initComplete)
                _messageClinet.GetAlarmMessageAsync();
        }

        /// <summary>
        /// 取消一键报警消息
        /// </summary>
        public void CancelAlarmMessage()
        {
            MessageActionCache.Remove(GetAlarmMessage);
            if (_initComplete)
                _messageClinet.CancelAlarmMessageAsync();
        }

        /// <summary>
        /// 获取解除一键报警消息
        /// </summary>
        public void GetCancelAlarmMessage()
        {
            if (MessageActionCache.Add(CancelAlarmMessage) && _initComplete)
                _messageClinet.GetCancelAlarmMessageAsync();
        }

        /// <summary>
        /// 获取日常监控Gps消息
        /// </summary>
        /// <param name="mdvrId"></param>
        public void GetMonitorGpsMessage(string mdvrId)
        {
            if (MessageActionCache.Add(GetMonitorGpsMessage, mdvrId) && _initComplete)
                _messageClinet.GetMonitorGpsMessageAsync(mdvrId);
        }

        /// <summary>
        /// 取消日常监控Gps消息
        /// </summary>
        /// <param name="mdvrId"></param>
        public void CancelMonitorGpsMessage(string mdvrId)
        {
            MessageActionCache.Remove(GetMonitorGpsMessage, mdvrId);
            if (_initComplete)
                _messageClinet.CancelMonitorGpsMessageAsync(mdvrId);
        }

        /// <summary>
        /// 获取摄像头无信号告警消息
        /// </summary>
        public void GetCameraNoSignalAlertMessage()
        {
            if (MessageActionCache.Add(GetCameraNoSignalAlertMessage) && _initComplete)
                _messageClinet.GetCameraNoSignalAlertMessageAsync();
        }

        /// <summary>
        /// 取消摄像头无信号告警消息
        /// </summary>
        public void CancelCameraNoSignalAlertMessage()
        {
            MessageActionCache.Remove(GetCameraNoSignalAlertMessage);
            if (_initComplete)
                _messageClinet.CancelCameraNoSignalAlertMessageAsync();
        }

        /// <summary>
        /// 获取摄像头遮挡告警消息
        /// </summary>
        public void GetCameraOcclusionAlertMessage()
        {
            if (MessageActionCache.Add(GetCameraOcclusionAlertMessage) && _initComplete)
                _messageClinet.GetCameraOcclusionAlertMessageAsync();
        }

        /// <summary>
        /// 取消摄像头遮挡告警消息
        /// </summary>
        public void CancelCameraOcclusionAlertMessage()
        {
            MessageActionCache.Remove(CancelCameraOcclusionAlertMessage);
            if (_initComplete)
                _messageClinet.CancelCameraOcclusionAlertMessageAsync();
        }

        /// <summary>
        /// 获取点火报警实体类消息
        /// </summary>
        public void GetFireAlertMessage()
        {
            if (MessageActionCache.Add(GetFireAlertMessage) && _initComplete)
                _messageClinet.GetFireAlertMessageAsync();
        }

        /// <summary>
        /// 取消点火报警实体类消息
        /// </summary>
        public void CancelFireAlertMessage()
        {
            MessageActionCache.Remove(GetFireAlertMessage);
            if (_initComplete)
                _messageClinet.CancelFireAlertMessageAsync();
        }

        /// <summary>
        /// 获取GPS接收机故障报警实体类消息
        /// </summary>
        public void GetGpsReceiverFaultAlertMessage()
        {
            if (MessageActionCache.Add(GetGpsReceiverFaultAlertMessage) && _initComplete)
                _messageClinet.GetGpsReceiverFaultAlertMessageAsync();
        }

        /// <summary>
        /// 取消GPS接收机故障报警实体类消息
        /// </summary>
        public void CancelGpsReceiverFaultAlertMessage()
        {
            MessageActionCache.Remove(CancelGpsReceiverFaultAlertMessage);
            if (_initComplete)
                _messageClinet.CancelGpsReceiverFaultAlertMessageAsync();
        }

        /// <summary>
        /// 获取MDVR存储卡错误报警实体类消息
        /// </summary>
        public void GetMdvrMemoryCardErrorAlertMessage()
        {
            if (MessageActionCache.Add(GetMdvrMemoryCardErrorAlertMessage) && _initComplete)
                _messageClinet.GetMdvrMemoryCardErrorAlertMessageAsync();
        }

        /// <summary>
        /// 取消MDVR存储卡错误报警实体类消息
        /// </summary>
        public void CancelMdvrMemoryCardErrorAlertMessage()
        {
            MessageActionCache.Remove(GetMdvrMemoryCardErrorAlertMessage);
            if (_initComplete)
                _messageClinet.CancelMdvrMemoryCardErrorAlertMessageAsync();
        }

        /// <summary>
        /// 获取异常开关门报警实体类消息
        /// </summary>
        public void GetOpenOrCloseDoorAbnormalAlertMessage()
        {
            if (MessageActionCache.Add(GetOpenOrCloseDoorAbnormalAlertMessage) && _initComplete)
                _messageClinet.GetOpenOrCloseDoorAbnormalAlertMessageAsync();
        }

        /// <summary>
        /// 取消异常开关门报警实体类消息
        /// </summary>
        public void CancelOpenOrCloseDoorAbnormalAlertMessage()
        {
            MessageActionCache.Remove(GetOpenOrCloseDoorAbnormalAlertMessage);
            if (_initComplete)
                _messageClinet.CancelOpenOrCloseDoorAbnormalAlertMessageAsync();
        }

        /// <summary>
        /// 获取超速报警实体类消息
        /// </summary>
        public void GetOverSpeedAlertMessage()
        {
            if (MessageActionCache.Add(GetOverSpeedAlertMessage) && _initComplete)
                _messageClinet.GetOverSpeedAlertMessageAsync();
        }

        /// <summary>
        /// 取消超速报警实体类消息
        /// </summary>
        public void CancelOverSpeedAlertMessage()
        {
            MessageActionCache.Remove(GetOverSpeedAlertMessage);
            if (_initComplete)
                _messageClinet.CancelOverSpeedAlertMessageAsync();
        }

        /// <summary>
        /// 获取区域报警实体类消息
        /// </summary>
        public void GetRegionAlertMessage()
        {
            if (MessageActionCache.Add(GetRegionAlertMessage) && _initComplete)
                _messageClinet.GetRegionAlertMessageAsync();
        }

        /// <summary>
        /// 取消区域报警实体类消息
        /// </summary>
        public void CancelRegionAlertMessage()
        {
            MessageActionCache.Remove(CancelRegionAlertMessage);
            if (_initComplete)
                _messageClinet.CancelRegionAlertMessageAsync();
        }

        /// <summary>
        /// 获取安全套件警情解除通知实体类消息
        /// </summary>
        public void GetRemoveDeviceSuiteAlertNotifyMessage()
        {
            if (MessageActionCache.Add(GetRemoveDeviceSuiteAlertNotifyMessage) && _initComplete)
                _messageClinet.GetRemoveDeviceSuiteAlertNotifyMessageAsync();
        }

        /// <summary>
        /// 取消安全套件警情解除通知实体类消息
        /// </summary>
        public void CancelRemoveDeviceSuiteAlertNotifyMessage()
        {
            MessageActionCache.Remove(GetRemoveDeviceSuiteAlertNotifyMessage);
            if (_initComplete)
                _messageClinet.CancelRemoveDeviceSuiteAlertNotifyMessageAsync();
        }

        /// <summary>
        /// 获取温度报警实体类消息
        /// </summary>
        public void GetTemperatureAlertMessage()
        {
            if (MessageActionCache.Add(GetTemperatureAlertMessage) && _initComplete)
                _messageClinet.GetTemperatureAlertMessageAsync();
        }

        /// <summary>
        /// 取消温度报警实体类消息
        /// </summary>
        public void CancelTemperatureAlertMessage()
        {
            MessageActionCache.Remove(GetTemperatureAlertMessage);
            if (_initComplete)
                _messageClinet.CancelTemperatureAlertMessageAsync();
        }

        /// <summary>
        /// 获取电压异常报警实体类消息
        /// </summary>
        public void GetVoltageAbnormalAlertMessage()
        {
            if (MessageActionCache.Add(GetVoltageAbnormalAlertMessage) && _initComplete)
                _messageClinet.GetVoltageAbnormalAlertMessageAsync();
        }

        /// <summary>
        /// 取消电压异常报警实体类消息
        /// </summary>
        public void CancelVoltageAbnormalAlertMessage()
        {
            MessageActionCache.Remove(GetVoltageAbnormalAlertMessage);
            if (_initComplete)
                _messageClinet.CancelVoltageAbnormalAlertMessageAsync();
        }

        /// <summary>
        /// 获取自检消息
        /// </summary>
        public void GetInspectMessage()
        {
            if (MessageActionCache.Add(GetInspectMessage) && _initComplete)
                _messageClinet.GetInspectMessageAsync();
        }

        /// <summary>
        /// 取消自检消息
        /// </summary>
        public void CancelInspectMessage()
        {
            MessageActionCache.Remove(GetInspectMessage);
            if (_initComplete)
                _messageClinet.CancelInspectMessageAsync();
        }

        /// <summary>
        /// 获取上线消息
        /// </summary>
        public void GetOnOfflineMessage()
        {
            if (MessageActionCache.Add(GetOnOfflineMessage) && _initComplete)
                _messageClinet.GetOnOfflineMessageAsync();
        }

        /// <summary>
        /// 取消上线消息
        /// </summary>
        public void CancelOnOfflineMessage()
        {
            MessageActionCache.Remove(GetOnOfflineMessage);
            if (_initComplete)
                _messageClinet.CancelOnOfflineMessageAsync();
        }

        /// <summary>
        /// 获取运维信息
        /// </summary>
        public void GetSuiteRunintStatusMessage()
        {
            MessageActionCache.Remove(GetSuiteRunintStatusMessage);
            if (_initComplete)
                _messageClinet.GetSuiteRunintStatusMessageAsync();
        }

        #endregion

        #region 用户类消息

        /// <summary>
        /// 正在处理一键报警
        /// </summary>
        /// <param name="mode"></param>
        public void SendHandingAlarmMessage(HandingAlarm mode)
        {
            _messageClinet.SendHandingAlarmMessageAsync(mode);
        }

        /// <summary>
        /// 订阅正在处理一键报警
        /// </summary>
        public void GetHandingAlarmMessage()
        {
            if (MessageActionCache.Add(GetHandingAlarmMessage) && _initComplete)
                _messageClinet.GetHandingAlarmMessageAsync();
        }

        /// <summary>
        /// 一键报警处理完成
        /// </summary>
        /// <param name="mode"></param>
        public void SendCompleteAlarmMessage(CompleteAlarm mode)
        {
            _messageClinet.SendCompleteAlarmMessageAsync(mode);
        }

        /// <summary>
        /// 订阅一键报警处理完成
        /// </summary>
        public void GetCompleteAlarmMessage()
        {
            if (MessageActionCache.Add(GetCompleteAlarmMessage) && _initComplete)
                _messageClinet.GetCompleteAlarmMessageAsync();
        }

        /// <summary>
        /// 设备安装完成信息
        /// </summary>
        /// <param name="mode"></param>
        public void SendDeviceInstallMessage(DeviceInstall mode)
        {
            _messageClinet.SendDeviceInstallMessageAsync(mode);
        }

        /// <summary>
        /// 订阅设备安装完成信息
        /// </summary>
        public void GetDeviceInstallMessage()
        {
            if (MessageActionCache.Add(GetDeviceInstallMessage) && _initComplete)
                _messageClinet.GetDeviceInstallMessageAsync();
        }

        /// <summary>
        /// 设备切换至维修
        /// </summary>
        /// <param name="mode"></param>
        public void SendDeviceMaintainMessage(DeviceMaintain model)
        {
            _messageClinet.SendDeviceMaintainMessageAsync(model);
        }

        /// <summary>
        /// 订阅设备切换至维修
        /// </summary>
        public void GetDeviceMaintainMessage()
        {
            if (MessageActionCache.Add(GetDeviceMaintainMessage) && _initComplete)
                _messageClinet.GetDeviceMaintainMessageAsync();
        }

        /// <summary>
        /// 告警处理
        /// </summary>
        /// <param name="mode"></param>
        public void SendHandingAlertMessage(HandingAlert model)
        {
            _messageClinet.SendHandingAlertMessageAsync(model);
        }

        /// <summary>
        /// 订阅告警处理
        /// </summary>
        public void GetHandingAlertMessage()
        {
            if (MessageActionCache.Add(GetHandingAlertMessage) && _initComplete)
                _messageClinet.GetHandingAlertMessageAsync();
        }

        /// <summary>
        /// 告警处理完成
        /// </summary>
        /// <param name="mode"></param>
        public void SendCompleteAlertMessage(CompleteAlert model)
        {
            _messageClinet.SendCompleteAlertMessageAsync(model);
        }

        /// <summary>
        /// 告警处理完成
        /// </summary>
        public void GetCompleteAlertMessage()
        {
            if (MessageActionCache.Add(GetCompleteAlertMessage) && _initComplete)
                _messageClinet.GetCompleteAlertMessageAsync();
        }

        /// <summary>
        /// 开始安装
        /// </summary>
        /// <param name="model"></param>
        public void SendStartInstallMessage(StartInstall model)
        {
            _messageClinet.SendStartInstallMessageAsync(model);
        }

        /// <summary>
        /// 订阅开始安装的消息
        /// </summary>
        public void GetStartInstallMessage()
        {
            if (MessageActionCache.Add(GetStartInstallMessage) && _initComplete)
                _messageClinet.GetStartInstallMessageAsync();
        }

        /// <summary>
        /// 发送流程中的安装的消息
        /// </summary>
        /// <param name="model"></param>
        public void SendDeleteInstallMessage(DeleteInstall model)
        {
            _messageClinet.SendDeleteInstallMessageAsync(model);
        }

        /// <summary>
        /// 获取删除流程中的安装
        /// </summary>
        public void GetDeleteInstallMessage()
        {
            if (MessageActionCache.Add(GetDeleteInstallMessage) && _initComplete)
                _messageClinet.GetDeleteInstallMessageAsync();
        }

        /// <summary>
        /// 获取被删除用户消息
        /// </summary>
        public void GetDeleteUserMessage()
        {
            if (MessageActionCache.Add(GetDeleteUserMessage) && _initComplete)
                _messageClinet.GetDeleteUserMessageAsync();
        }

        /// <summary>
        /// 获取被修改用户信息
        /// </summary>
        public void GetChangeUserMessage()
        {
            if (MessageActionCache.Add(GetChangeUserMessage) && _initComplete)
                _messageClinet.GetChangeUserMessageAsync();
        }

        /// <summary>
        /// 发送被删除用户消息
        /// </summary>
        public void SendDeleteUserMessage(DeleteUser model)
        {
            _messageClinet.SendDeleteUserMessageAsync(model);
        }

        /// <summary>
        /// 发送被修改用户信息
        /// </summary>
        public void SendChangeUserMessage(ChangeUser model)
        {
            _messageClinet.SendChangeUserMessageAsync(model);
        }

        /// <summary>
        /// 发送路线信息
        /// </summary>
        public void SendRouteMessage(RouteCMD model)
        {
            _messageClinet.SendRouteMessageAsync(model);
        }

        /// <summary>
        /// 发送升级通知
        /// </summary>
        public void SendUpgradeNotifyMessage(UpgradeNotify model)
        {
            _messageClinet.SendUpgradeNotifyMessageAsync(model);
        }
        #endregion

        #region 向下命令

        /// <summary>
        /// 电子围栏命令
        /// </summary>
        public void SendElectricFenceCMD(ElectricFenceCMD item)
        {
            _messageClinet.SendElectricFenceCMDAsync(item);
        }

        /// <summary>
        /// 软件升级命令
        /// </summary>
        public void SendUpgradeCMD(UpgradeCMD item)
        {
            _messageClinet.SendUpgradeCMDAsync(item);
        }

        /// <summary>
        /// 获取升级状态
        /// </summary>
        public void SendGetUpgradeStatusCMD(UpgradeStatusCMD item)
        {
            _messageClinet.SendGetUpgradeStatusCMDAsync(item);
        }

        /// <summary>
        /// 获取安全套件状态信息
        /// </summary>
        public void SendGetSuiteRunintStatusCMD(SuiteRunintStatusCMD item)
        {
            _messageClinet.SendGetSuiteRunintStatusCMDAsync(item);
        }

        /// <summary>
        /// 设置超速指令
        /// </summary>
        public void SendSettingOverSpeedCMD(SettingOverSpeedCMD item)
        {
            _messageClinet.SendSettingOverSpeedCMDAsync(item);
        }

        #endregion

        #endregion
    }


}
