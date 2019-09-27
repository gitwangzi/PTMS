/////Copyright (C) Gsafety 2014 .All Rights Reserved.
/////======================================================================
/////                   Guid: cd8ba7cb-6cbb-468b-968d-69ec13d36275      
/////             clrversion: 4.0.30319.34003
/////Registered organization: 
/////           Machine Name: DENGZL
/////                 Author: DENGZL(dzl)
/////======================================================================
/////           Project Name: Gsafety.PTMS.ServiceReference.ServiceExtend
/////    Project Description:    
/////             Class Name: MessageServiceEx
/////          Class Version: v1.0.0.0
/////            Create Time: 2014/2/9 00:15:41
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2014/2/9 00:15:41
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System;
using System.Collections.ObjectModel;
using System.Net;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Gsafety.PTMS.ServiceReference.MessageService
{
    /// <summary>
    /// Message Service Extension Methods
    /// To realization service synchronization
    /// </summary>
    public partial class MessageServiceClient
    {
    
        /// <summary>uy
        ///  Send single electionic fence information in synchronous mode
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public Task SendElectricFenceCMDEx(ElectircFenceSendSettingModel args)
        {
            return Task.Factory.FromAsync(this.Channel.BeginSendElectricFenceCMD, this.Channel.EndSendElectricFenceCMD, args, null);
        }

        /// <summary>
        /// Send multiple electionic fence information in synchronous mode
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public Task SendElectricFenceCMDSetEx(ObservableCollection<ElectircFenceSendSettingModel> args)
        {
            return Task.Factory.FromAsync(this.Channel.BeginSendElectricFenceCMDSet, this.Channel.EndSendElectricFenceCMDSet, args, null);
        }

        /// <summary>
        /// Send single overspeed setting in synchronous mode
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public Task SendSettingOverSpeedEx(SettingOverSpeedCMD args)
        {
            return Task.Factory.FromAsync(this.Channel.BeginSendSettingOverSpeedCMD, this.Channel.EndSendSettingOverSpeedCMD, args, null);
        }

        /// <summary>
        /// Send multiple overspeed setting in synchronous mode
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public Task SendSettingOverSpeedSetEx(ObservableCollection<SettingOverSpeedCMD> args)
        {
            return Task.Factory.FromAsync(this.Channel.BeginSendSettingOverSpeedCMDSet, this.Channel.EndSendSettingOverSpeedCMDSet, args, null);
        }

        /// <summary>
        /// Send single travel information in synchronous mode 
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public Task SendTravelPlanMessageEx(TravelPlanCMD args)
        {
            return Task.Factory.FromAsync(this.Channel.BeginSendTravelPlanMessage, this.Channel.EndSendTravelPlanMessage, args, null);
        }

        /// <summary>
        /// Send multiple travel information in synchronous mode
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public Task SendTravelPlanMessageSetEx(ObservableCollection<TravelPlanCMD> args)
        {
            return Task.Factory.FromAsync(this.Channel.BeginSendTravelPlanMessageSet, this.Channel.EndSendTravelPlanMessageSet, args, null);
        }

        /// <summary>
        /// Send single line setting information in synchronous mode
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public Task SendRouteMessageEx(RouteCMD args)
        {
            return Task.Factory.FromAsync(this.Channel.BeginSendRouteMessage, this.Channel.EndSendRouteMessage, args, null);
        }

        /// <summary>
        /// Send multiple overspeed setting at same time
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public Task SendRouteMessageSetEx(ObservableCollection<RouteCMD> args)
        {
            return Task.Factory.FromAsync(this.Channel.BeginSendRouteMessageSet, this.Channel.EndSendRouteMessageSet, args, null);
        }


        /// <summary>
        /// Monitor vehicle location information request
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public Task RequestMonitorGpsMessage(LocationMonitorCMD args)
        {
            return Task.Factory.FromAsync(this.Channel.BeginGetMonitorGpsMessage, this.Channel.EndGetMonitorGpsMessage, args, null);
        }

        public Task SendSettingElectircFenceUploadCMDEx(ElectircFenceSendSettingModel args)
        {
            return Task.Factory.FromAsync(this.Channel.BeginSendElectircFenceSetting, this.Channel.EndSendElectircFenceSetting, args, null);
        }
        public Task SendInfomationCMDEx(SendInfomationModel args)
        {
            return Task.Factory.FromAsync(this.Channel.BeginSendInfomationCommand, this.Channel.EndSendInfomationCommand, args, null);
        }

    }
}
