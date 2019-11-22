using Gsafety.Ant.Installation.ViewModels;
using Gsafety.Common.Controls;
using Gsafety.PTMS.ServiceReference.AppConfigService;
using Gsafety.PTMS.ServiceReference.DeviceInstallService;
using Gsafety.PTMS.ServiceReference.VedioService;
using Gsafety.PTMS.ServiceReference.VehicleAlarmService;
using Gsafety.PTMS.Share;
using Jounce.Core.View;
using Jounce.Core.ViewModel;
/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: a6e58a11-abbe-4794-82ee-222bb6a11a76      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: GAOZITIAN-PC
/////                 Author: TEST(gaozt)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Installation.ViewModels
/////    Project Description:    
/////             Class Name: CheckDeviceFunctionViewModel
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/8/17 15:26:35
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/8/17 15:26:35
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Threading;
namespace Gsafety.PTMS.Installation.ViewModels
{
    [ExportAsViewModel(InstallationName.InstallSuiteFunctionCheckVm)]
    public class InstallSuiteFunctionCheckViewModel : InstallSuiteViewModelBase
    {
        private int _alarmCheck;
        public int AlarmCheck
        {
            get { return _alarmCheck; }
            set
            {
                _alarmCheck = value;
                RaisePropertyChanged(() => AlarmCheck);
                CheckNextButton();
            }
        }

        private int _gpsCheck;
        public int GpsCheck
        {
            get { return _gpsCheck; }
            set
            {
                _gpsCheck = value;
                RaisePropertyChanged(() => GpsCheck);
                CheckNextButton();
            }
        }

        private int _videoCheck;
        public int VideoCheck
        {
            get { return _videoCheck; }
            set
            {
                _videoCheck = value;
                RaisePropertyChanged(() => VideoCheck);
                CheckNextButton();
            }
        }

        private Dictionary<int, bool> _channelDictionary = new Dictionary<int, bool>();
        public Dictionary<int, bool> ChannelDictionary
        {
            get { return _channelDictionary; }
            set
            {
                _channelDictionary = value;
                RaisePropertyChanged(() => ChannelDictionary);
            }
        }

        private int validtime = 0;
        private DispatcherTimer _queryTimer;

        VedioServiceClient vedioServiceClient = null;
        AppConfigManagerClient appConfigManagerClient = null;
        VehicleAlarmServiceClient vehicleAlarmServiceClient;

        public InstallSuiteFunctionCheckViewModel()
        {
            try
            {
                _queryTimer = new DispatcherTimer();
                _queryTimer.Interval = TimeSpan.FromSeconds(3);
                _queryTimer.Tick += QueryTimer_Tick;

                vedioServiceClient = ServiceClientFactory.Create<VedioServiceClient>();
                appConfigManagerClient = ServiceClientFactory.Create<AppConfigManagerClient>();
                vehicleAlarmServiceClient = ServiceClientFactory.Create<VehicleAlarmServiceClient>();

                ImageSource = "Step05.png";
                step = 5;

                vedioServiceClient.CheckAlarmVideoCompleted += vedioServiceClient_CheckAlarmVideoCompleted;
                appConfigManagerClient.GetConfigInfoBySectionNameCompleted += appConfigManagerClient_GetConfigInfoBySectionNameCompleted;
                vehicleAlarmServiceClient.GetAlarmCheckCompleted += vehicleAlarmServiceClient_GetAlarmCheckCompleted;
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("CheckDeviceFunctionViewModel()", ex);
            }
        }

        void QueryTimer_Tick(object sender, EventArgs e)
        {
            Get();
        }

        protected override void ActivateView(string viewName, System.Collections.Generic.IDictionary<string, object> viewParameters)
        {
            try
            {
                base.ActivateView(viewName, viewParameters);

                appConfigManagerClient.GetConfigInfoBySectionNameAsync("AlarmVideoTime");
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }

        protected override void DeactivateView(string viewName)
        {
            try
            {
                base.DeactivateView(viewName);

                _queryTimer.Stop();
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }

        protected override void Get()
        {
            try
            {
                CheckAlarmVideoArgs arg = new CheckAlarmVideoArgs();
                arg.Alarm_Id = _InstallID;
                arg.Date = DateTime.UtcNow.AddHours(-validtime);

                if (VideoCheck != 1)
                {
                    vedioServiceClient.CheckAlarmVideoAsync(arg);
                }

                if (AlarmCheck != 1)
                {
                    vehicleAlarmServiceClient.GetAlarmCheckAsync(_InstallID, arg.Date);
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }

        void appConfigManagerClient_GetConfigInfoBySectionNameCompleted(object sender, GetConfigInfoBySectionNameCompletedEventArgs e)
        {
            try
            {
                if (e.Error != null)
                {
                    return;
                }
                if (e.Result.Result != null)
                {
                    validtime = int.Parse(e.Result.Result.SECTION_VALUE);
                }
                else
                {
                    validtime = 1;
                }

                _queryTimer.Start();
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("appConfigManagerClient_GetConfigInfoBySectionNameCompleted", ex);
                validtime = 1;
            }

        }

        void vehicleAlarmServiceClient_GetAlarmCheckCompleted(object sender, GetAlarmCheckCompletedEventArgs e)
        {
            try
            {
                if (e.Result.Result)
                {
                    AlarmCheck = 1;
                    CheckToStopTimer();
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("vedioServiceClient_CheckAlarmVideoCompleted", ex);
            }
        }

        void vedioServiceClient_CheckAlarmVideoCompleted(object sender, CheckAlarmVideoCompletedEventArgs e)
        {
            try
            {
                if (e.Result.IsSuccess)
                {
                    ChannelDictionary = e.Result.Result;

                    VideoCheck = ChannelDictionary.Values.Any(t => t == false) ? 0 : 1;
                    CheckToStopTimer();
                }
                else
                {
                    MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString("Tip"), ApplicationContext.Instance.StringResourceReader.GetString("ID_INSTALL_ServiceError"), MessageDialogButton.Ok);
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("vedioServiceClient_CheckAlarmVideoCompleted", ex);
            }
        }

        private void CheckToStopTimer()
        {
            if (AlarmCheck == 1 && VideoCheck == 1 && GpsCheck == 1)
            {
                _queryTimer.Stop();
            }
        }

        protected override void NextPage()
        {
            //TODO
            InstallationInfo installDetailModel = new InstallationInfo()
            {
                /// ID
                Id = _InstallID,
                /// The current installation steps
                CheckStep = 5
            };
            deviceInstallServiceClient.UpdateInstallationAsync(installDetailModel);
        }

        protected override void GoNextPage()
        {
            EventAggregator.Publish(new ViewNavigationArgs(InstallationName.InstallConfirmV, new Dictionary<string, object>() { { "ID", _InstallID } }));
        }

        protected override void ResetData()
        {
            try
            {
                ChannelDictionary = new Dictionary<int, bool>();
                AlarmCheck = 2;
                GpsCheck = 1;
                VideoCheck = 2;
                IsGetMessage = true;
                IsFinished = false;
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("CheckDeviceFunctionViewModel ResetData", ex);
            }
        }

        void CheckNextButton()
        {
            if (GpsCheck == 1 && VideoCheck == 1 && AlarmCheck == 1)
            {
                IsFinished = true;
            }
            else
            {
                IsFinished = false;
            }
        }
    }
}
