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
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Jounce.Core.ViewModel;
using Jounce.Core.Command;
using Jounce.Framework;
using Jounce.Framework.Command;
using Jounce.Core.View;
using Gsafety.PTMS.Share;
using Gsafety.PTMS.MainPage.Views;
using Gsafety.PTMS.ServiceReference.MessageService;
using GisManagement.Models;
using Gsafety.PTMS.ServiceReference.DeviceInstallService;
using Jounce.Core.Event;
using Gsafety.Common.CommMessage;
using System.ComponentModel.Composition;
using System.Windows.Controls.Primitives;
using System.Windows.Media.Imaging;
using System.IO;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Browser;
using Gsafety.PTMS.BasicPage.VideoDisplay;
using ESRI.ArcGIS.Client.Geometry;
using Gsafety.Common.Controls;

namespace Gsafety.PTMS.MainPage.ViewModels
{
    [ExportAsViewModel(MainPageName.CentralPlatformVm)]
    public class CentralPlatformVm : BaseViewModel
        , IEventSink<MessageNotifitionActiveteParamter>
        , IPartImportsSatisfiedNotification
        , IEventSink<Gsafety.PTMS.ServiceReference.MessageService.AlarmInfo>
        , IEventSink<Gsafety.PTMS.ServiceReference.MessageService.AlertBaseModel>
        , IEventSink<MessageServiceStatus>
    {


        private const string _playVedioResource = "Gsafety.PTMS.MainPage.Sound.alarm.mp3";
        MediaElement _VedioPlay = new MediaElement();
        Stream _VedioStream;
        private bool _autoPlanyMusic;
        private MessageServiceStatus _MessageServiceCurrentStatus;

        #region Attributes

        public string UserName
        {
            get { return ApplicationContext.Instance.AuthenticationInfo.UserName; }
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

        #endregion

        #region ActionCommand

        public IActionCommand MenuCommand { get; private set; }
        public IActionCommand ChangePasswordCommand { get; private set; }
        public IActionCommand UserInformationCommmand { get; private set; }
        public IActionCommand MonitorCommand { get; private set; }
        public IActionCommand OneKeyCommand { get; private set; }
        public IActionCommand AlertCommand { get; private set; }
        public IActionCommand TrafficCommand { get; private set; }
        public IActionCommand CloseMessage { get; private set; }
        public IActionCommand ExitCommand { get; private set; }
        public IActionCommand RefreshCommand { get; private set; }


        public ICommand MapToolsToLeft { get; set; }
        public ICommand MapToolsToRight { get; set; }//
        public ICommand MapToolsReturnEarth { get; set; }//回到原图
        public ICommand MapToolsRange { get; set; }//测距离
        public ICommand MapToolsErea { get; set; }//测面积
        public ICommand MapToolsAnnotation { get; set; }//标绘
        public ICommand MapToolsClearLayers { get; set; }//清理
        public ICommand MapToolsPrintMapLayers { get; set; }//打印
        public ICommand MapToolsPolygon { get; set; }//选择多变性
        public ICommand MapToolsRectangle { get; set; }//选择矩形
        public ICommand MapToolsCircle { get; set; }//选择圆形
        public ICommand MapToolsCoordsSearch { get; set; }//坐标查询
        public ICommand MapToolsHistoryTrack { get; set; }//历史轨迹
        public ICommand MapToolsAreaPosition { get; set; }//地区定位

        #endregion

        public CentralPlatformVm()
        {
            ApplicationContext.Instance.DeActiveViewCallback += DeActiveCallback;
            MenuCommand = new ActionCommand<object>(obj => MenuEvnet(obj));
            ChangePasswordCommand = new ActionCommand<object>(obj => ChangePassword_Event());
            UserInformationCommmand = new ActionCommand<object>(obj => UserInformation_Evnet());
            MonitorCommand = new ActionCommand<object>(obj => _MonitorCommand());
            OneKeyCommand = new ActionCommand<object>(obj => _OneKeyCommand());
            AlertCommand = new ActionCommand<object>(obj => _AlertCommand());
            TrafficCommand = new ActionCommand<object>(obj => _TrafficCommand());
            ExitCommand = new ActionCommand<object>(ojb => Exit_Command());
            RefreshCommand = new ActionCommand<object>(obj => Refresh_Command());


            MapToolsToLeft = new ActionCommand<object>(obj => ToLeft());
            MapToolsToRight = new ActionCommand<object>(obj => ToRight());
            MapToolsReturnEarth = new ActionCommand<object>(obj => ReturnEarth());
            MapToolsRange = new ActionCommand<object>(obj => Range());
            MapToolsErea = new ActionCommand<object>(obj => Erea());
            MapToolsAnnotation = new ActionCommand<object>(obj => Annotation());
            MapToolsClearLayers = new ActionCommand<object>(obj => ClearLayers());
            MapToolsPrintMapLayers = new ActionCommand<object>(obj => PrintMapLayers());

            MapToolsPolygon = new ActionCommand<object>(obj => Polygon());
            MapToolsRectangle = new ActionCommand<object>(obj => Rectangle());
            MapToolsCircle = new ActionCommand<object>(obj => Circle());
            MapToolsCoordsSearch = new ActionCommand<object>(obj => CoordsSearch());
            MapToolsHistoryTrack = new ActionCommand<object>(obj => HistoryTrack());
            MapToolsAreaPosition = new ActionCommand<object>(obj => AreaPosition());


            CloseMessage = new ActionCommand<object>(obj =>
            {
                object mview = Router.ViewQuery(MainPageName.CentralPlatformV);
                ToggleButton toggleButton = (mview as UserControl).FindName("MessageTip") as ToggleButton;
                toggleButton.IsChecked = false;
            }
            );
        }
        /// <summary>
        /// 地区定位
        /// </summary>
        /// <returns></returns>
        private void AreaPosition()
        {

        }
        /// <summary>
        /// 历史轨迹
        /// </summary>
        /// <returns></returns>
        private void HistoryTrack()
        {

        }
        /// <summary>
        /// 查询
        /// </summary>
        /// <returns></returns>
        private void CoordsSearch()
        {

        }
        /// <summary>
        /// 选择圆形
        /// </summary>
        /// <returns></returns>
        private void Circle()
        {

        }
        /// <summary>
        /// 选择矩形
        /// </summary>
        /// <returns></returns>
        private void Rectangle()
        {

        }
        /// <summary>
        /// 选择多变性
        /// </summary>
        /// <returns></returns>
        private void Polygon()
        {

        }

        /// <summary>
        /// 打印
        /// </summary>
        private void PrintMapLayers()
        {
            MessageBoxHelper.ShowDialog("PrintMapLayers");
        }

        /// <summary>
        /// 清理
        /// </summary>
        private void ClearLayers()
        {
            MessageBoxHelper.ShowDialog("ClearLayers");
        }
        /// <summary>
        /// 标绘
        /// </summary>
        private void Annotation()
        {
            MessageBoxHelper.ShowDialog("Annotation");
        }
        /// <summary>
        /// 测面积
        /// </summary>
        private void Erea()
        {
            MessageBoxHelper.ShowDialog("Erea");
        }
        /// <summary>
        /// 测距离
        /// </summary>
        private void Range()
        {
            MessageBoxHelper.ShowDialog("Range");
        }

        /// <summary>
        /// 回到全图
        /// </summary>
        /// <returns></returns>
        private void ReturnEarth()
        {
            MessageBoxHelper.ShowDialog("ReturnEarth");
        }

        /// <summary>
        /// /
        /// </summary>
        /// <returns></returns>
        private void ToRight()
        {
            MessageBoxHelper.ShowDialog("ToRight");
        }

        /// <summary>
        /// 返回操作
        /// </summary>
        /// <returns></returns>
        private void ToLeft()
        {
            MessageBoxHelper.ShowDialog("ToLeft");
        }





        private void _TrafficCommand()
        {
            EventAggregator.Publish<GisDisplayControlEvent>(new GisDisplayControlEvent() { Display = GisDisplayControlType.miTraffic });
        }

        private void _AlertCommand()
        {
            EventAggregator.Publish<GisDisplayControlEvent>(new GisDisplayControlEvent() { Display = GisDisplayControlType.miMonitor_Alert });
        }

        private void _OneKeyCommand()
        {
            EventAggregator.Publish<GisDisplayControlEvent>(new GisDisplayControlEvent() { Display = GisDisplayControlType.miMonitor_Alarm });
        }

        private void _MonitorCommand()
        {
            EventAggregator.Publish<GisDisplayControlEvent>(new GisDisplayControlEvent() { Display = GisDisplayControlType.miMonitor_RealTime });
        }

        private void Exit_Command()
        {
            //if (MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString("MAINPAGE_ExitAceptar"), 
            //    ApplicationContext.Instance.StringResourceReader.GetString("MAINPAGE_ExitAPrompt"),
            //    MessageDialogButton.OkAndCancel)==)
            //{
            //    HtmlPage.Window.Eval("window.location.reload();");
            //}
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
            try
            {
                EventAggregator.Publish(MainPageName.MessageNotifition.AsViewNavigationArgs());
                SubscriptionMessage();
                ApplicationContext.Instance.MenuManager.Router = Router;
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
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
            UserInformation userInfomation = new UserInformation();
            userInfomation.Show();

        }

        public void OnImportsSatisfied()
        {
            EventAggregator.SubscribeOnDispatcher<MessageNotifitionActiveteParamter>(this);
            EventAggregator.SubscribeOnDispatcher<Gsafety.PTMS.ServiceReference.MessageService.AlarmInfo>(this);
            EventAggregator.SubscribeOnDispatcher<Gsafety.PTMS.ServiceReference.MessageService.AlertBaseModel>(this);
            EventAggregator.SubscribeOnDispatcher<MessageServiceStatus>(this);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="isActivete"></param>
        public void HandleEvent(MessageNotifitionActiveteParamter isActivete)
        {
            try
            {
                if (isActivete.IsActivete)
                    GoToVisualState("showFloatPanel", true);
                else
                {
                    object mview = Router.ViewQuery(MainPageName.CentralPlatformV);
                    ToggleButton toggleButton = (mview as UserControl).FindName("MessageTip") as ToggleButton;
                    toggleButton.IsChecked = false;
                    GoToVisualState("hiddenFloatPanel", true);
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
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

        #region TestCode
        private void TestMoniting()
        {
            System.Threading.Thread thread = new System.Threading.Thread(SendMessage);
            thread.Start();

        }

        private void SendMessage()
        {
            int i = 0;
            while (true)
            {
                if (i++ % 2 == 0)
                {
                    AlarmInfo aa = new AlarmInfo();
                    aa.CityName = Guid.NewGuid().ToString();
                    aa.ProvinceName = Guid.NewGuid().ToString();
                    aa.VehicleId = Guid.NewGuid().ToString();
                    aa.SuiteStatus = (int)Gsafety.PTMS.ServiceReference.SecuritySuiteService.DeviceSuiteStatus.Running;
                    aa.AlarmTime = DateTime.Now;
                    EventAggregator.Publish<AlarmInfo>(aa);
                }
                else
                {
                    OverSpeedAlert aaa = new OverSpeedAlert();
                    aaa.CityName = Guid.NewGuid().ToString();
                    aaa.ProvinceName = Guid.NewGuid().ToString();
                    aaa.VehicleID = "BJ10000";
                    aaa.SuiteStatus = (int)Gsafety.PTMS.ServiceReference.SecuritySuiteService.DeviceSuiteStatus.Running;
                    aaa.AlertTime = DateTime.Now;
                    aaa.Speed = "13";
                    aaa.AlertType = 13;
                    EventAggregator.Publish<OverSpeedAlert>(aaa);
                }

                System.Threading.Thread.Sleep(15000);
            }

        }
        #endregion

        private void DeActiveCallback(int view)
        {
            try
            {
                if (view == 1)
                {
                    object mview = Router.ViewQuery(MainPageName.CentralPlatformV);
                    var link = (mview as UserControl).FindName("OneKeyLink") as HyperlinkButton;
                    var spanel = link.Content as StackPanel;
                    var img = spanel.FindName("alarm_img") as Image;
                    var url = "/ExternalResource;component/Images/MainPage_alarm.png";
                    img.Source = new BitmapImage(new Uri(url, UriKind.Relative));

                }
                else if (view == 2)
                {
                    object mview = Router.ViewQuery(MainPageName.CentralPlatformV);
                    var link = (mview as UserControl).FindName("AlertLink") as HyperlinkButton;
                    var spanel = link.Content as StackPanel;
                    var img = spanel.FindName("alert_img") as Image;
                    var url = "/ExternalResource;component/Images/MainPage_vehiclealarm.png";
                    img.Source = new BitmapImage(new Uri(url, UriKind.Relative));
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
                    object mview = Router.ViewQuery(MainPageName.CentralPlatformV);
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
                    object mview = Router.ViewQuery(MainPageName.CentralPlatformV);
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

    }
}
