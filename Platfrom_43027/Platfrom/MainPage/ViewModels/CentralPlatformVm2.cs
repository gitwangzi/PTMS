using ESRI.ArcGIS.Client.Geometry;
using GisManagement.Models;
using Gsafety.Common.CommMessage;
using Gsafety.Common.Controls;
using Gsafety.PTMS.BasicPage.VideoDisplay;
using Gsafety.PTMS.MainPage.Views;
using Gsafety.PTMS.ServiceReference.DeviceInstallService;
using Gsafety.PTMS.ServiceReference.MessageService;
using Gsafety.PTMS.Share;
using Jounce.Core.Command;
using Jounce.Core.Event;
using Jounce.Core.View;
using Jounce.Core.ViewModel;
using Jounce.Framework;
using Jounce.Framework.Command;
/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 921168d2-6b37-4253-9a89-eb017ea3b929      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-DENGZL
/////                 Author: TEST(dengzl)
/////======================================================================
/////           Project Name: Gsafety.PTMS.MainPage.ViewModels
/////    Project Description:    
/////             Class Name: CentralPlatformVm
/////          Class Version: v1.0.0.0
/////            Create Time: 8/5/2013 1:38:27 PM
/////      Class Description:  
/////======================================================================
/////          Modified Time: 8/5/2013 1:38:27 PM
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System;
using System.ComponentModel.Composition;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Browser;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using Gsafety.PTMS.Bases.Models;
using System.Collections.ObjectModel;
using System.Linq;
using Gsafety.Ant.MainPage.Views;
using System.Windows.Threading;

namespace Gsafety.PTMS.MainPage.ViewModels
{
    [ExportAsViewModel(MainPageName.CentralPlatformVm2)]
    public class CentralPlatformVm2 : BaseViewModel
        , IEventSink<MessageNotifitionActiveteParamter>
        , IPartImportsSatisfiedNotification
        , IEventSink<Gsafety.PTMS.ServiceReference.MessageService.AlarmInfo>
        , IEventSink<Gsafety.PTMS.ServiceReference.MessageService.AlertBaseModel>
        , IEventSink<MessageServiceStatus>
         , IEventSink<ForceLogoutArg>
    {
        public VehicleOrganizationManage VehicleOrganizationManage
        {
            get
            {
                return ApplicationContext.Instance.BufferManager.VehicleOrganizationManage;
            }
        }

        private const string _playVedioResource = "Gsafety.PTMS.MainPage.Sound.alarm.mp3";
        MediaElement _VedioPlay = new MediaElement();
        Stream _VedioStream;
        private bool _autoPlanyMusic;
        private MessageServiceStatus _MessageServiceCurrentStatus;

        #region Attributes

        public AuthenticationInfo AuthenticationInfo
        {
            get { return ApplicationContext.Instance.AuthenticationInfo; }
        }

        public BusyInfo BusyInfo
        {
            get
            {
                return ApplicationContext.Instance.BusyInfo;
            }
        }

        public bool AutoPlayMusic
        {
            get { return _autoPlanyMusic; }
            set
            {
                if (value != _autoPlanyMusic)
                {
                    _autoPlanyMusic = value;
                    RaisePropertyChanged(() => AutoPlayMusic);
                }
            }
        }

        public string ManagerVisibility { get; set; }

        public MessageServiceStatus MessageServiceCurrentStatus
        {
            get
            {
                return _MessageServiceCurrentStatus;
            }
            set
            {
                _MessageServiceCurrentStatus = value;
            }
        }

        public Uri MainPageUri
        {
            get
            {
                CentralNavigationerMainpage mainpageManager = new CentralNavigationerMainpage();
                return mainpageManager.GetMainpageUri();
            }
        }

        private bool _isAlertVisibility = true;

        public bool IsAlertVisibility
        {
            get { return _isAlertVisibility; }
            set
            {
                if (value != _isAlertVisibility)
                {
                    _isAlertVisibility = value;
                    RaisePropertyChanged(() => IsAlertVisibility);
                }
            }
        }

        private bool _isSilenceVisibility = false;

        public bool IsSilenceVisibility
        {
            get { return _isSilenceVisibility; }
            set
            {
                if (value != _isSilenceVisibility)
                {
                    _isSilenceVisibility = value;
                    RaisePropertyChanged(() => IsSilenceVisibility);
                }
            }
        }

        #endregion

        #region ActionCommand

        public IActionCommand MenuCommand { get; private set; }
        public IActionCommand ChangePasswordCommand { get; private set; }
        public IActionCommand UserInformationCommmand { get; private set; }
        public IActionCommand AlertCommand { get; private set; }
        public IActionCommand SilenceCommand { get; private set; }
        public IActionCommand TrafficCommand { get; private set; }
        public IActionCommand CloseMessage { get; private set; }
        public IActionCommand ExitCommand { get; private set; }
        public IActionCommand RefreshCommand { get; private set; }
        private DispatcherTimer _queryTimer;

        public IActionCommand RecycleCommmand { get; private set; }
        #endregion

        public CentralPlatformVm2()
        {
            try
            {
                this.IsAlertVisibility = false;
                this.IsSilenceVisibility = true;
                ApplicationContext.Instance.DeActiveViewCallback += DeActiveCallback;
                MenuCommand = new ActionCommand<object>(obj => MenuEvnet(obj));
                ChangePasswordCommand = new ActionCommand<object>(obj => ChangePassword_Event());
                UserInformationCommmand = new ActionCommand<object>(obj => UserInformation_Evnet());
                RecycleCommmand = new ActionCommand<object>(obj => Recycle_Event());
                AlertCommand = new ActionCommand<object>(obj => _AlertEvent());
                SilenceCommand = new ActionCommand<object>(obj => _SilenceEvent());
                ExitCommand = new ActionCommand<object>(ojb => Exit_Command());
                RefreshCommand = new ActionCommand<object>(obj => Refresh_Command());

                CloseMessage = new ActionCommand<object>(obj =>
                {
                    object mview = Router.ViewQuery(MainPageName.CentralPlatformV);
                    ToggleButton toggleButton = (mview as UserControl).FindName("MessageTip") as ToggleButton;
                    toggleButton.IsChecked = false;
                }
                );

                _queryTimer = new DispatcherTimer();
                _queryTimer.Interval = TimeSpan.FromSeconds(1);
                _queryTimer.Tick += _queryTimer_Tick;
                _queryTimer.Start();
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }

        }

        void _queryTimer_Tick(object sender, EventArgs e)
        {
            GC.Collect();
            //GC.WaitForPendingFinalizers();
            //HtmlPage.Window.Invoke("FlushMemory");
            //_queryTimer.Stop();
            //if (!ApplicationContext.Instance.ServerConfig.Authenticate)
            //{
            //    var result = MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString(LProxy.Caption),
            //    ApplicationContext.Instance.StringResourceReader.GetString("Expired"));
            //    result.Closed += result_Closed;
            //}
        }

        private void Exit_Command()
        {
            var window = MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString("MAINPAGE_ExitAPrompt"), ApplicationContext.Instance.StringResourceReader.GetString("MAINPAGE_ExitAceptar"), MessageDialogButton.OkAndCancel);
            window.Closed += closeWindow_Closed;
        }

        void closeWindow_Closed(object sender, EventArgs e)
        {
            var window = sender as ChildWindow;
            if (window.DialogResult == true)
            {
                HtmlPage.Window.Eval("window.location.reload();");
                HtmlPage.Window.Invoke("CloseShell");
            }
        }

        private void Refresh_Command()
        {
            //TravelPlanCMD travelPlan = new TravelPlanCMD();
            //travelPlan.BeginDate = DateTime.Now.AddHours(-1);
            //travelPlan.EndDate = DateTime.Now;
            //travelPlan.VechileID = "GSG9988";
            //travelPlan.WeekDay = "0,2,5";
            //travelPlan.OperType = 1;
            //ApplicationContext.Instance.MessageManager.SendTravelPlanCMD(travelPlan);
            //ApplicationContext.Instance.MessageManager.ForwardingAlarmMessageToEcu911(ApplicationContext.Instance.BufferManager.AlarmManager.AllAlarmInfo[0]);
            //ApplicationContext.Instance.MessageManager.ForwardingAlarmMessageToEcu911(ApplicationContext.Instance.BufferManager.AlarmManager.AllAlarmInfo[0]);
            //ApplicationContext.Instance.MessageManager.BatchSendElectricFenceCMD(null);

        }

        protected override void ActivateView(string viewName, System.Collections.Generic.IDictionary<string, object> viewParameters)
        {
            EventAggregator.Publish(MainPageName.MessageNotifition.AsViewNavigationArgs());
            SubscriptionMessage();
            ApplicationContext.Instance.MenuManager.Router = Router;
            DeviceInstallServiceClient deviceInstallServiceClient = ServiceClientFactory.Create<DeviceInstallServiceClient>();
            ////TestMoniting();
        }

        private void MenuEvnet(object paramter)
        {
            EventAggregator.Publish(new ViewNavigationArgs(paramter.ToString()));
        }

        private void ChangePassword_Event()
        {
            ChangePassword changePassword = new ChangePassword();
            changePassword.Show();
        }

        private void UserInformation_Evnet()
        {
            UserDetailInfoWindow userDetailInfo = new UserDetailInfoWindow();
            userDetailInfo.Show();
        }

        private void Recycle_Event()
        {
            SystemRubbishWindow systemRubbish = new SystemRubbishWindow();
            systemRubbish.Show();
        }

        private void _AlertEvent()
        {
            this.IsAlertVisibility = true;
            this.IsSilenceVisibility = false;
        }

        private void _SilenceEvent()
        {
            this.IsAlertVisibility = false;
            this.IsSilenceVisibility = true;
        }

        public void OnImportsSatisfied()
        {
            EventAggregator.SubscribeOnDispatcher<MessageNotifitionActiveteParamter>(this);
            EventAggregator.SubscribeOnDispatcher<Gsafety.PTMS.ServiceReference.MessageService.AlarmInfo>(this);
            EventAggregator.SubscribeOnDispatcher<Gsafety.PTMS.ServiceReference.MessageService.AlertBaseModel>(this);
            EventAggregator.SubscribeOnDispatcher<MessageServiceStatus>(this);
            EventAggregator.SubscribeOnDispatcher<ForceLogoutArg>(this);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="isActivete"></param>
        public void HandleEvent(MessageNotifitionActiveteParamter isActivete)
        {
            if (isActivete.IsActivete)
                GoToVisualState("showFloatPanel", true);
            else
            {
                object mview = Router.ViewQuery(MainPageName.CentralPlatformV2);
                ToggleButton toggleButton = (mview as UserControl).FindName("MessageTip") as ToggleButton;
                toggleButton.IsChecked = false;
                GoToVisualState("hiddenFloatPanel", true);
            }
        }

        private void SubscriptionMessage()
        {
            //System.Threading.Thread.Sleep(500);
            //ApplicationContext.Instance.MessageManager.GetOnOfflineMessage();
            //ApplicationContext.Instance.MessageManager.GetDeviceInstallMessage();
            //ApplicationContext.Instance.MessageManager.GetDeviceMaintainMessage();
            //ApplicationContext.Instance.MessageManager.GetRemoveDeviceSuiteAlertNotifyMessage();
            //ApplicationContext.Instance.MessageManager.GetDeleteUserMessage();
            //ApplicationContext.Instance.MessageManager.GetChangeUserMessage();

            //if (ApplicationContext.Instance.AuthenticationInfo.MonitorFunction)
            //{
            //    ApplicationContext.Instance.MessageManager.GetLocationMonitorEndMessage();
            //}

            //if (ApplicationContext.Instance.AuthenticationInfo.AlarmFunction)
            //{
            //    ApplicationContext.Instance.MessageManager.GetAlarmMessage();
            //    ApplicationContext.Instance.MessageManager.GetCompleteAlarmMessage();
            //}

            //if (ApplicationContext.Instance.AuthenticationInfo.AlertFunction)
            //{
            //    ApplicationContext.Instance.MessageManager.GetOpenOrCloseDoorAbnormalAlertMessage();
            //    ApplicationContext.Instance.MessageManager.GetOverSpeedAlertMessage();
            //    ApplicationContext.Instance.MessageManager.GetRegionAlertMessage();
            //    ApplicationContext.Instance.MessageManager.GetCompleteAlertMessage();
            //}

            //if (ApplicationContext.Instance.AuthenticationInfo.TrafficFunction)
            //{
            //    ApplicationContext.Instance.MessageManager.GetFenceReplyMessage();
            //    ApplicationContext.Instance.MessageManager.GetSettingOverSpeedReplyMessage();
            //}
            //ApplicationContext.Instance.MessageManager.GetChangeGroupMessage();
            //ApplicationContext.Instance.MessageManager.GetChangeGroupVehicleMessage();
            //if (ApplicationContext.Instance.AuthenticationInfo.TrafficFenceFunction)
            //{
            //    ApplicationContext.Instance.MessageManager.GetFenceReplyMessage();
            //}
            //if (ApplicationContext.Instance.AuthenticationInfo.TrafficSpeedFunction)
            //{
            //    ApplicationContext.Instance.MessageManager.GetSettingOverSpeedReplyMessage();
            //}
        }

        private void DeActiveCallback(int view)
        {
            try
            {
                if (view == 1)
                {
                    object mview = Router.ViewQuery(MainPageName.CentralPlatformV2);
                    var link = (mview as UserControl).FindName("OneKeyLink") as HyperlinkButton;
                    var spanel = link.Content as StackPanel;
                    var img = spanel.FindName("alarm_img") as Image;
                    var url = "/ExternalResource;component/Images/MainPage_alarm.png";
                    img.Source = new BitmapImage(new Uri(url, UriKind.Relative));

                }
                else if (view == 2)
                {
                    object mview = Router.ViewQuery(MainPageName.CentralPlatformV2);
                    var link = (mview as UserControl).FindName("AlertLink") as HyperlinkButton;
                    if (link != null)
                    {
                        var spanel = link.Content as StackPanel;
                        var img = spanel.FindName("alert_img") as Image;
                        var url = "/ExternalResource;component/Images/MainPage_vehiclealarm.png";
                        img.Source = new BitmapImage(new Uri(url, UriKind.Relative));
                    }
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }

        public void HandleEvent(AlarmInfo publishedEvent)
        {
            try
            {
                if (ApplicationContext.Instance.CurrentView != 1 && FilterAlarm(publishedEvent))
                {
                    object mview = Router.ViewQuery(MainPageName.CentralPlatformV2);
                    var link = (mview as UserControl).FindName("OneKeyLink") as HyperlinkButton;
                    var spanel = link.Content as StackPanel;
                    var img = spanel.FindName("alarm_img") as Image;
                    var url = "/ExternalResource;component/Images/MenuAlarm-new.png";
                    img.Source = new BitmapImage(new Uri(url, UriKind.Relative));
                    PlayMusic();
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }

        public void HandleEvent(AlertBaseModel publishedEvent)
        {
            try
            {
                if (ApplicationContext.Instance.CurrentView != 2 && FilterAlert(publishedEvent))
                {
                    object mview = Router.ViewQuery(MainPageName.CentralPlatformV2);
                    var link = (mview as UserControl).FindName("AlertLink") as HyperlinkButton;
                    var spanel = link.Content as StackPanel;
                    var img = spanel.FindName("alert_img") as Image;
                    var url = "/ExternalResource;component/Images/MenuVehicleAlarm-new.png";
                    img.Source = new BitmapImage(new Uri(url, UriKind.Relative));
                    PlayMusic();
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }

        public void HandleEvent(MessageServiceStatus status)
        {
            MessageServiceCurrentStatus = status;
            Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => MessageServiceCurrentStatus));
        }

        private bool FilterAlarm(AlarmInfo item)
        {
            var result = item.SuiteStatus == (int)Gsafety.PTMS.ServiceReference.SecuritySuiteService.DeviceSuiteStatus.Abnormal
                || item.SuiteStatus == (int)Gsafety.PTMS.ServiceReference.SecuritySuiteService.DeviceSuiteStatus.WaitingMaintenance
                || item.SuiteStatus == (int)Gsafety.PTMS.ServiceReference.SecuritySuiteService.DeviceSuiteStatus.Running;


            return result;


        }
        private bool FilterAlert(AlertBaseModel item)
        {
            var result = item.SuiteStatus == (int)Gsafety.PTMS.ServiceReference.SecuritySuiteService.DeviceSuiteStatus.Abnormal
                || item.SuiteStatus == (int)Gsafety.PTMS.ServiceReference.SecuritySuiteService.DeviceSuiteStatus.WaitingMaintenance
                || item.SuiteStatus == (int)Gsafety.PTMS.ServiceReference.SecuritySuiteService.DeviceSuiteStatus.Running;


            return result;
        }

        private void PlayMusic()
        {
            try
            {
                if (AutoPlayMusic)
                {
                    if (_VedioStream == null)
                    {
                        _VedioStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(_playVedioResource);
                    }
                    _VedioPlay.Stop();
                    _VedioPlay.Volume = 1.0;
                    _VedioPlay.SetSource(_VedioStream);
                    _VedioPlay.Position = TimeSpan.Zero;
                    _VedioPlay.Play();
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }


        public void HandleEvent(ForceLogoutArg publishedEvent)
        {
            //MessageBox.Show("用户登陆,当前系统退出!");
            var result = MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString(LProxy.Caption),
                ApplicationContext.Instance.StringResourceReader.GetString(LProxy.CurrentUserLoginedSystemExit));
            result.Closed += result_Closed;

        }

        void result_Closed(object sender, EventArgs e)
        {
            HtmlPage.Window.Eval("window.location.reload();");
            HtmlPage.Window.Invoke("CloseShell");
        }
    }
}
