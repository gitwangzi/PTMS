using BaseLib.ViewModels;
using GisManagement.Models;
using Gsafety.Ant.Monitor.Models;
using Gsafety.Common.CommMessage;
using Gsafety.Common.CommMessage.Controls;
using Gsafety.Common.Controls;
using Gsafety.PTMS.Bases.Enums;
using Gsafety.PTMS.Bases.Librarys;
using Gsafety.PTMS.Bases.Models;
using Gsafety.PTMS.BasicPage.Views;
using Gsafety.PTMS.Monitor;
using Gsafety.PTMS.Monitor.ViewModels;
using Gsafety.PTMS.Monitor.Views;
using Gsafety.PTMS.ServiceReference.MessageService;
using Gsafety.PTMS.ServiceReference.MessageServiceExt;
using Gsafety.PTMS.ServiceReference.VehicleAlarmService;
using Gsafety.PTMS.ServiceReference.VehicleAlertService;
using Gsafety.PTMS.ServiceReference.VehicleMonitorService;
using Gsafety.PTMS.Share;
using Gsafety.PTMS.Share.Model;
using Jounce.Core.Event;
using Jounce.Core.View;
using Jounce.Core.ViewModel;
using Jounce.Framework;
using Jounce.Framework.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Linq;
//using Gsafety.PTMS.ServiceReference.TrafficManageService;

namespace Gsafety.Ant.Monitor.ViewModels
{
    [ExportAsViewModel(MonitorName.AntProductMonitorMainPageVm)]
    public partial class AntProductMonitorMainPageViewModel : PTMSBaseViewModel,
        IEventSink<UpdateOjectStatusNoMarkArgs>,
        IEventSink<EndLocationMonitor>,
        IPartImportsSatisfiedNotification,
        IEventSink<AlarmGisArgs>,
        IEventSink<AlarmHandleResult>,
        IEventSink<AlarmCountChange>,
        IEventSink<AlertHandleResult>,
        IEventSink<GisFixChangeRoute>,
        IEventSink<AlertGisArgs>
    {
        #region Public Attributes
        public VehicleTreeFactory VehicleTreeFactory { get; set; }

        private MonityEntityBase _monitorTreeSelectItem = new MonityEntityBase();
        /// <summary>
        /// 日常监控中选中项
        /// </summary>
        public MonityEntityBase MonitorTreeSelectItem
        {
            get
            {
                return _monitorTreeSelectItem;
            }
            set
            {
                if (_monitorTreeSelectItem != value)
                {
                    _monitorTreeSelectItem = value;

                    if (_monitorTreeSelectItem is VehicleEx)
                    {
                        MonitorTreeSelectVehicle = (_monitorTreeSelectItem as VehicleEx);
                    }
                    else
                    {
                        MonitorTreeSelectVehicle = null;
                    }

                    RaisePropertyChanged(() => MonitorTreeSelectItem);
                }
            }
        }

        private VehicleEx _monitorTreeSelectVehicle;
        /// <summary>
        /// 日常监控中选中的车
        /// </summary>
        public VehicleEx MonitorTreeSelectVehicle
        {
            get { return _monitorTreeSelectVehicle; }
            set
            {
                if (_monitorTreeSelectVehicle != value)
                {
                    var old = _monitorTreeSelectVehicle;
                    _monitorTreeSelectVehicle = value;
                    RaisePropertyChanged(() => MonitorTreeSelectVehicle);

                    if (value == null)
                    {
                        return;
                    }

                    LocateCar(value.VehicleId);
                    OnSelectedVehicleChanged(old, _monitorTreeSelectVehicle);
                    EventAggregator.Publish<Gsafety.PTMS.Bases.Models.Vehicle>(_monitorTreeSelectVehicle.VehicleInfo);
                    EventAggregator.Publish<ManualAlarmHandleDisplayArgs>(new ManualAlarmHandleDisplayArgs() { Show = null, VehicleID = _monitorTreeSelectVehicle.VehicleInfo.VehicleId });
                }
            }
        }

        private string _fiterText = string.Empty;
        /// <summary>
        /// 日常监控中的搜索条件
        /// </summary>
        public string FilterText
        {
            get
            {
                return _fiterText;
            }
            set
            {
                _fiterText = value.Trim();
                RaisePropertyChanged(() => FilterText);
            }
        }

        #endregion

        #region Command
        /// <summary>
        /// 历史视频
        /// </summary>
        public ICommand HistoricalVideoCommand { get; private set; }

        /// <summary>
        /// 搜索
        /// </summary>
        public ICommand VehicleSearchCommand { get; private set; }

        /// <summary>
        /// 监控组
        /// </summary>
        public ICommand LocationMonitorCommand { get; private set; }

        /// <summary>
        /// 电子围栏
        /// </summary>
        public ICommand ElectricFenceCheckedCommand { get; private set; }


        /// <summary>
        /// 选择电子围栏
        /// </summary>
        public ICommand SelectElectricFenceCommand { get; private set; }

        /// <summary>
        /// 发送消息
        /// </summary>
        public ICommand SendMessageCommond { get; private set; }

        /// <summary>
        /// 手动报警
        /// </summary>
        public ICommand AlarmByMonitorCommond { get; private set; }

        /// <summary>
        /// 行驶路线
        /// </summary>
        public ActionCommand<object> PlanRouteCommand { get; private set; }
        #endregion

        private void OnSelectedVehicleChanged(VehicleEx oldValue, VehicleEx newValue)
        {
            try
            {
                if ((oldValue != null) && (newValue != null))
                {
                    if (oldValue.VehicleId == newValue.VehicleId)
                    {
                        LocateCar(newValue.VehicleId);
                        return;
                    }
                }
                if (oldValue != null)
                {
                    //如果不是监控列表中则取消订阅
                    //string department = (oldValue.Parent as OrganizationEx).Organization.Name;
                    //if (CanUnbindGPS(oldValue.VehicleId)) MonitorGPS(oldValue.VehicleId, department, false, false, false);
                }

                if (newValue != null)
                {
                    //订阅
                    string department = (newValue.Parent as OrganizationEx).Organization.Name;
                    bool hasAlarm = ApplicationContext.Instance.BufferManager.AlarmManager.HasAlarm(newValue.VehicleId);
                    bool hasAlert = ApplicationContext.Instance.BufferManager.VehicleAlertManager.HasAlert(newValue.VehicleId);
                    MonitorGPS(newValue.VehicleId, department, true, hasAlarm, hasAlert);
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }

        private void LocateCar(string CarNo)
        {
            EventAggregator.Publish<DisplayCurrentPositionArgs>(new DisplayCurrentPositionArgs()
            {
                Prov = "",
                CarNo = CarNo,
                VE = (int)ElementLayerDefine.miVERealLocation
            });
        }

        public AntProductMonitorMainPageViewModel()
        {
            try
            {
                VehicleTreeFactory = new VehicleTreeFactory();

                HistoricalVideoCommand = new ActionCommand<object>((obj) => HistoricalVideo_Event(obj));
                VehicleSearchCommand = new ActionCommand<object>((treeview) => VehicleSearch(treeview));
                LocationMonitorCommand = new ActionCommand<object>((obj) => LocationMonitor_Event(obj));
                ElectricFenceCheckedCommand = new ActionCommand<object>((obj) => ElectricFenceCheckedClick_Event(obj));
                SelectElectricFenceCommand = new ActionCommand<object>((obj) => SelectElectricFenceClick_Event());
                PlanRouteCommand = new ActionCommand<object>((obj) => PlanRoute_Event(obj));
                SendMessageCommond = new ActionCommand<object>((obj) => SendMessage_Evnt(obj));
                AlarmByMonitorCommond = new ActionCommand<object>((obj) => AlarmByMonitor_Evnt(obj));
                FilterMonitorGroupCommand = new ActionCommand<object>((obj) => FilterMonitorGroup_Event(obj));
                DeleteMonitorGroupVehicleCommand = new ActionCommand<object>((obj) => DeleteMonitorGroupVehicle_Event(obj));
                this.IsEnableButtonEnable = false;

                InitalAlarm();
                InitalAlert();
                InitalDeviceAlert();
                InitMonitorList();
                InitMonitorGroupDropDownList();
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }

        private void PlanRoute_Event(object obj)
        {
            try
            {
                if (null != obj)
                {
                    string vehicleId = obj.ToString();
                    Gsafety.PTMS.ServiceReference.TrafficManageService.TrafficManageServiceClient trafficServiceClient = ServiceClientFactory.Create<Gsafety.PTMS.ServiceReference.TrafficManageService.TrafficManageServiceClient>();
                    trafficServiceClient.GetDeliveredTrafficRouteListByVehicleIDCompleted += ((sender, e) =>
                    {
                        if (e.Error != null || e.Result.IsSuccess == false)
                        {
                            ApplicationContext.Instance.Logger.LogException("GetDeliveredTrafficRouteListByVehicleIDCompleted", e.Error);
                            return;
                        }
                        if (e.Result.Result.Count == 0)
                        {
                            MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString(LProxy.Caption), ApplicationContext.Instance.StringResourceReader.GetString("Traffic_VehicleNoFindRoute"), MessageDialogButton.Ok);
                            return;
                        }

                        SymbolParams parm = new SymbolParams();
                        SymbolStyleSet symbolSelect = new SymbolStyleSet();
                        symbolSelect.ControlTabItemVisbility(0, 1);
                        symbolSelect.ControlTabItemVisbility(2, 1);
                        symbolSelect.Closed += ((o, args) =>
                        {
                            if ((bool)symbolSelect.DialogResult)
                            {
                                parm.LineColorParm = symbolSelect.LineColorParm;
                                parm.LineWidthParm = symbolSelect.LineWidthParm;
                                parm.TransparentParm = symbolSelect.TansparentParm;
                                parm.MarkColorParm = symbolSelect.MarkColorParm;
                                parm.MarkSizeParm = symbolSelect.SymbolSize;
                            }
                            else
                            {
                                return;
                            }

                            foreach (Gsafety.PTMS.ServiceReference.TrafficManageService.TrafficRoute route in e.Result.Result)
                            {
                                EventAggregator.Publish<MarkTrafficGraphic>(new MarkTrafficGraphic() { nType = TrafficFeature.Traffic_Route, parentId = vehicleId, childId = route.ID, TrafficRoute = route, bShow = true, MarkSymbolParm = parm });
                            }

                            if (e.Result.Result.Count != 0)
                            {
                                EventAggregator.Publish<ZoomGisView>(new ZoomGisView());
                            }

                        });
                        symbolSelect.Show();
                    });
                    trafficServiceClient.GetDeliveredTrafficRouteListByVehicleIDAsync(vehicleId, ApplicationContext.Instance.AuthenticationInfo.ClientID);
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }

        private void AlarmByMonitor_Evnt(object obj)
        {
            EventAggregator.Publish<ManualAlarmHandleDisplayArgs>(new ManualAlarmHandleDisplayArgs() { Show = true, VehicleID = obj.ToString() });
        }

        private void SendMessage_Evnt(object obj)
        {
            SendPhoneMsgWindow window = new SendPhoneMsgWindow(obj as string);
            window.Show();
        }

        #region 实时视频
        private ICommand _playVideoCommand;
        public ICommand PlayVideoCommand
        {
            get
            {
                if (_playVideoCommand == null)
                {
                    _playVideoCommand = new ActionCommand<object>(a => Play(a));
                }
                return _playVideoCommand;
            }
        }

        private void Play(object mdvrCoreID)
        {
            var cameraWindow = new CameraSelectWindow(mdvrCoreID as string, 4);
            cameraWindow.Closed += cameraWindow_Closed;
            cameraWindow.Show();
        }

        void cameraWindow_Closed(object sender, EventArgs e)
        {
            var winodw = sender as CameraSelectWindow;
            if (winodw.DialogResult == true && winodw.SelectResult.Count > 0)
            {
                var info = new MediaInfo()
                {
                    MediaInfoItems = winodw.SelectResult,
                    IsHideProgressControl = true,
                    ShowHistoryLine = false,
                    AutoPlay = true,
                    Orientation = Orientation.Horizontal,
                };

                ApplicationContext.Instance.EventAggregator.Publish<MediaInfo>(info);
            }
        }

        public ICommand _capturePhotoConmand;
        public ICommand CapturePhotoConmand
        {
            get
            {
                if (_capturePhotoConmand == null)
                {
                    _capturePhotoConmand = new ActionCommand<object>(a => Capture(a));
                }
                return _capturePhotoConmand;
            }
        }

        private void Capture(object mdvrCoreID)
        {
            var capturePhotoWindow = new CapturePhotoWindow(mdvrCoreID.ToString());
            capturePhotoWindow.Closed += capturePhotoWindow_Closed;
            capturePhotoWindow.Show();
        }

        void capturePhotoWindow_Closed(object sender, EventArgs e)
        {
        }

        #endregion

        /// <summary>
        ///get Finally  GPS coordinates
        ///查询车辆位置相关信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void vehicleMonitorServiceClient_GetLastMonitorGPSCompleted(object sender, GetLastMonitorGPSCompletedEventArgs e)
        {
            try
            {
                Gsafety.PTMS.Bases.Models.Vehicle dargs = e.UserState as Gsafety.PTMS.Bases.Models.Vehicle;
                if ((e.Result != null) && (e.Result.Result != null))
                {
                    Gsafety.PTMS.ServiceReference.VehicleMonitorService.GPS antgps = e.Result.Result;
                    if (antgps != null)
                    {
                        if (GPSState.Valid(antgps.Valid))
                        {
                            LocateVechile(dargs);
                            return;
                        }
                    }
                }
                MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString("GIS_LocationFailed"), ApplicationContext.Instance.StringResourceReader.GetString("GIS_Notice"), MessageDialogButton.Ok);

            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }

        /// <summary>
        /// Vehicles fence 
        /// </summary>
        /// <param name="obj"></param>
        private void ElectricFenceCheckedClick_Event(object obj)
        {
            if (null != obj)
            {
                string vehicleId = obj.ToString();
                Gsafety.PTMS.ServiceReference.TrafficManageService.TrafficManageServiceClient trafficServiceClient = ServiceClientFactory.Create<Gsafety.PTMS.ServiceReference.TrafficManageService.TrafficManageServiceClient>();
                trafficServiceClient.GetTrafficFenceListOnVehicleByVehicleIDCompleted += ((sender, e) =>
                    {
                        if (e.Error != null || e.Result.IsSuccess == false)
                        {
                            ApplicationContext.Instance.Logger.LogException("trafficServiceClient_GetTrafficFenceListOnVehicleByVehicleIDCompleted", e.Error);
                            return;
                        }
                        if (e.Result.Result.Count == 0)
                        {
                            MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString(LProxy.Caption), ApplicationContext.Instance.StringResourceReader.GetString("Traffic_VehicleNoFindFence"), MessageDialogButton.Ok);
                            return;
                        }
                        SymbolParams parm = new SymbolParams();
                        SymbolStyleSet symbolSelect = new SymbolStyleSet();
                        symbolSelect.ControlTabItemVisbility(1, 0);
                        symbolSelect.ControlTabItemVisbility(2, 0);
                        symbolSelect.Closed += ((o, args) =>
                        {
                            if ((bool)symbolSelect.DialogResult)
                            {
                                parm.FillColorParm = symbolSelect.FillColorParm;
                                parm.TransparentParm = symbolSelect.TansparentParm;
                                parm.MarkColorParm = symbolSelect.MarkColorParm;
                                parm.MarkSizeParm = symbolSelect.SymbolSize;
                            }
                            else
                            {
                                return;
                            }

                            foreach (Gsafety.PTMS.ServiceReference.TrafficManageService.TrafficFence fence in e.Result.Result)
                            {
                                EventAggregator.Publish<MarkTrafficGraphic>(new MarkTrafficGraphic() { nType = TrafficFeature.Traffic_PolygonFence, parentId = vehicleId, childId = fence.ID, TrafficFence = fence, bShow = true, MarkSymbolParm = parm });
                            }

                            if (e.Result.Result.Count != 0)
                            {
                                EventAggregator.Publish<ZoomGisView>(new ZoomGisView());
                            }

                        });
                        symbolSelect.Show();
                    });
                trafficServiceClient.GetTrafficFenceListOnVehicleByVehicleIDAsync(vehicleId, ApplicationContext.Instance.AuthenticationInfo.ClientID);
            }
        }


        /// <summary>
        /// Vehicles fence 
        /// </summary>
        /// <param name="obj"></param>
        private void SelectElectricFenceClick_Event()
        {
            EventAggregator.Publish<SelectFenceDispayArgs>(new SelectFenceDispayArgs() { Show = true });
        }
        /// <summary>
        /// Activated form event
        /// </summary>
        /// <param name="viewName"></param>
        /// <param name="viewParameters"></param>
        protected override void ActivateView(string viewName, System.Collections.Generic.IDictionary<string, object> viewParameters)
        {
            try
            {
                EventAggregator.Publish(MonitorName.VehicleInfoView.AsViewNavigationArgs());

                EventAggregator.Publish(GisManagement.GisName.MonitorGisView.AsViewNavigationArgs());
                EventAggregator.Publish(MonitorName.MonitorManualAlarmHandleView.AsViewNavigationArgs());

                ApplicationContext.Instance.CurrentGISName = GisManagement.GisName.MonitorGisView;

                InitGISDisplayFromMonitorList();

                ActivateAlarmView(viewName, viewParameters);
                ActivateAlertView(viewName, viewParameters);
                ActivateFenceView(viewName, viewParameters);

                RefreshGIS();
                RefreshDetail();
                EventAggregator.Publish<ManualAlarmHandleDisplayArgs>(new ManualAlarmHandleDisplayArgs() { Show = false });
                EventAggregator.Publish<SelectFenceDispayArgs>(new SelectFenceDispayArgs() { Show = false });

                object mview = ApplicationContext.Instance.MenuManager.Router.ViewQuery(MonitorName.AntProductMonitorMainPageV);
                Frame frame = (mview as UserControl).FindName("ContentFrame") as Frame;

                if (frame.CurrentSource == null)
                    return;
                frame.Refresh();
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }

        private void ActivateFenceView(string viewName, IDictionary<string, object> viewParameters)
        {
            try
            {

                EventAggregator.Publish(MonitorName.MonitorSelectFenceView.AsViewNavigationArgs());

               
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }

        private void RefreshDetail()
        {
            try
            {
                if (selectedheader == ApplicationContext.Instance.StringResourceReader.GetString("MAINPAGE_Monitor"))
                {
                    AlarmInfoVisibility = Visibility.Collapsed;
                    AlertInfoVisibility = Visibility.Collapsed;
                    BaseInfoVisibility = Visibility.Visible;

                    EventAggregator.Publish<AlarmHandlerDispayArgs>(new AlarmHandlerDispayArgs() { Show = false });
                    EventAggregator.Publish<AlertHandleDisplayArgs>(new AlertHandleDisplayArgs() { Show = false });
                }
                else if (selectedheader == ApplicationContext.Instance.StringResourceReader.GetString("VehicleAlarm"))
                {
                    AlarmInfoVisibility = Visibility.Visible;
                    AlertInfoVisibility = Visibility.Collapsed;
                    BaseInfoVisibility = Visibility.Collapsed;

                    EventAggregator.Publish<AlertHandleDisplayArgs>(new AlertHandleDisplayArgs() { Show = false });
                    EventAggregator.Publish<ManualAlarmHandleDisplayArgs>(new ManualAlarmHandleDisplayArgs() { Show = false });
                }
                else if (selectedheader == ApplicationContext.Instance.StringResourceReader.GetString("ALERT_VehicleAlert"))
                {
                    AlarmInfoVisibility = Visibility.Collapsed;
                    AlertInfoVisibility = Visibility.Visible;
                    BaseInfoVisibility = Visibility.Collapsed;

                    EventAggregator.Publish<ManualAlarmHandleDisplayArgs>(new ManualAlarmHandleDisplayArgs() { Show = false });
                    EventAggregator.Publish<AlarmHandlerDispayArgs>(new AlarmHandlerDispayArgs() { Show = false });
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }

        private void RefreshGIS()
        {
            if (selectedheader == ApplicationContext.Instance.StringResourceReader.GetString("MAINPAGE_Monitor"))
            {
                EventAggregator.Publish<GisDisplayControlEvent>(
               new GisDisplayControlEvent()
               {
                   Display = GisDisplayControlType.miMonitor_RealTime
               });
            }
            else if (selectedheader == ApplicationContext.Instance.StringResourceReader.GetString("VehicleAlarm"))
            {
                EventAggregator.Publish<GisDisplayControlEvent>(
               new GisDisplayControlEvent()
               {
                   Display = GisDisplayControlType.miMonitor_Alarm
               });
            }
            else if (selectedheader == ApplicationContext.Instance.StringResourceReader.GetString("ALERT_VehicleAlert"))
            {
                EventAggregator.Publish<GisDisplayControlEvent>(
               new GisDisplayControlEvent()
               {
                   Display = GisDisplayControlType.miMonitor_Alert
               });
            }
        }

        private void LocateVechile(Gsafety.PTMS.Bases.Models.Vehicle obj)
        {
            Gsafety.PTMS.Bases.Models.Vehicle vehicle = obj;

            if (null != vehicle)
            {
                EventAggregator.Publish<DisplayCurrentPositionArgs>(new DisplayCurrentPositionArgs()
                {
                    Prov = vehicle.ProvinceName,
                    CarNo = vehicle.VehicleId,
                    VE = (int)ElementLayerDefine.miVERealLocation
                });
            }
        }

        #region 关注车辆 监控组
        /// <summary>
        /// Position monitoring
        /// Messaging service to send a location request command, while giving Gis view monitor messages sent starting position
        /// </summary>
        /// <param name="vechileId"></param>
        private void LocationMonitor_Event(object vechileId)
        {
            if (vechileId != null)
            {
                Views.MonitorGroupManager addGroup = new Views.MonitorGroupManager();
                addGroup.AddVechileToGroup(vechileId.ToString());

                addGroup.Closed += (a, b) =>
                {
                    if (addGroup.DialogResult == true)
                    {
                        string department = GetOrganizationName(vechileId.ToString());
                        bool hasAlarm = ApplicationContext.Instance.BufferManager.AlarmManager.HasAlarm(vechileId.ToString());
                        bool hasAlert = ApplicationContext.Instance.BufferManager.VehicleAlertManager.HasAlert(vechileId.ToString());
                        MonitorGPS(vechileId.ToString(), department, true, hasAlarm, hasAlert);
                    }
                    RefreshMonitorGroup();
                };
            }
        }
        #endregion

        //判断是否可以取消绑定
        private bool CanUnbindGPS(string CarNo)
        {
            //如果在监控列表中不允许取消绑定
            foreach (TableDataElement te in _TableData)
            {
                if (te.VehicleId == CarNo) return false;
            }
            return true;
        }
        //绑定或者取消绑定GPS
        private void MonitorGPS(string VehicleId, string DepartmentName, bool bind, bool alarm, bool alert)
        {
            try
            {
                List<string> lst = new List<string>();
                lst.Add(VehicleId);
                if (bind)
                {
                    ApplicationContext.Instance.MessageClient.MonitorVehicle(lst);
                    RequestVehicleMonitorArgs locationArgs = new RequestVehicleMonitorArgs();
                    locationArgs.CarNo = VehicleId;
                    locationArgs.UniqueId = VehicleId;
                    locationArgs.IsAlarm = alarm;//读报警列表是否有未处理的
                    locationArgs.IsAlert = alert;//读告警列表是否有未处理的
                    locationArgs.Department = DepartmentName;//组织机构
                    locationArgs.Op = 1;
                    EventAggregator.Publish<RequestVehicleMonitorArgs>(locationArgs);
                }
                else
                {
                    ApplicationContext.Instance.MessageClient.UnMonitorVehicle(lst);

                    RequestVehicleMonitorArgs locationArgs = new RequestVehicleMonitorArgs();
                    locationArgs.CarNo = VehicleId;
                    locationArgs.UniqueId = VehicleId;
                    locationArgs.Op = 0;
                    EventAggregator.Publish<RequestVehicleMonitorArgs>(locationArgs);
                }

            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }

        #region 历史视频
        private void HistoricalVideo_Event(object vechileId)
        {
            var cvm = new HistoryVideoManageWindow(vechileId.ToString());
            cvm.Closed += cvm_Closed;
            cvm.Show();
        }

        void cvm_Closed(object sender, EventArgs e)
        {
            var window = sender as HistoryVideoManageWindow;
            if (window.DialogResult != true)
            {
                return;
            }

            if (window.SelectVideoInfoItems.Count > 0)
            {
                var mediaPlayerInfo = new MediaInfo();
                mediaPlayerInfo.IsHideProgressControl = false;
                mediaPlayerInfo.Orientation = Orientation.Vertical;
                mediaPlayerInfo.AutoPlay = false;
                mediaPlayerInfo.ShowHistoryLine = true;

                foreach (var item in window.SelectVideoInfoItems)
                {
                    var info = new MediaInfo.MediaInfoItem()
                    {
                        StartTime = item.Model.StartTime,
                        EndTime = item.Model.EndTime,
                        Url = item.Model.FileID,
                        Channel = (int)item.CameraInstallLocation,
                        IsRealVideo = false,
                        IsShowControlBar = false,
                        IsShowProcessBar = false,
                        ShowRemoveBtn = false,
                    };
                    mediaPlayerInfo.MediaInfoItems.Add(info);
                }

                mediaPlayerInfo.VehicleId = window.HistoryVideoManageContentViewModel.CarNo;
                EventAggregator.Publish<MediaInfo>(mediaPlayerInfo);
            }
        }
        #endregion

        #region 搜索

        public void VehicleSearch(object treeview)
        {
            VehicleTreeFactory.SearchVehicleTree(FilterText);
        }
        #endregion

        public void OnImportsSatisfied()
        {
            EventAggregator.SubscribeOnDispatcher<UpdateOjectStatusNoMarkArgs>(this);
            EventAggregator.SubscribeOnDispatcher<EndLocationMonitor>(this);
            EventAggregator.SubscribeOnDispatcher<AlarmGisArgs>(this);
            EventAggregator.SubscribeOnDispatcher<AlarmHandleResult>(this);
            EventAggregator.SubscribeOnDispatcher<AlarmCountChange>(this);
            EventAggregator.SubscribeOnDispatcher<AlertHandleResult>(this);
            EventAggregator.SubscribeOnDispatcher<AlertGisArgs>(this);
            EventAggregator.SubscribeOnDispatcher<GisFixChangeRoute>(this);
        }

        public void HandleEvent(UpdateOjectStatusNoMarkArgs publishedEvent)
        {
            try
            {
                if (publishedEvent != null && publishedEvent.UpdateObject != null)
                {
                    Gsafety.PTMS.Bases.Models.Vehicle v = ApplicationContext.Instance.BufferManager.VehicleOrganizationManage.GetVehicle(publishedEvent.UpdateObject.ToString());
                    if (v != null)
                    {
                        if (publishedEvent.markType == Gsafety.PTMS.Bases.Enums.TrafficFeature.Traffic_PolygonFence)
                        {
                            v.ElectricFence = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }

        public void HandleEvent(GisFixChangeRoute publishedEvent)
        {
            try
            {
                if (publishedEvent != null && publishedEvent.VechileId!= null)
                {
                    
                    VehicleEx SelectVehicle = VehicleTreeFactory.VehicleList.FirstOrDefault(t => t.VehicleId == publishedEvent.VechileId);
                    if (SelectVehicle!=null)
                    {
                        MonitorTreeSelectVehicle = SelectVehicle;
                    }
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }

        /// <summary>
        /// the message when gps request is faild from mdvr
        /// </summary>
        /// <param name="publishedEvent"></param>
        public void HandleEvent(EndLocationMonitor publishedEvent)
        {
            try
            {
                if (publishedEvent != null)
                {
                    Gsafety.PTMS.Bases.Models.Vehicle vehicle = ApplicationContext.Instance.BufferManager.VehicleOrganizationManage.GetVehicle(publishedEvent.VechileID);
                    if (vehicle != null)
                    {
                        if (publishedEvent.EndType == LocationMonitorEndType.RequestFails)
                        {
                            vehicle.IsMonitor = false;
                            MessageBoxHelper.ShowDialog(publishedEvent.VechileID + "--" + ApplicationContext.Instance.StringResourceReader.GetString("Monitor_MDVRGPS_RequestFails"), ApplicationContext.Instance.StringResourceReader.GetString("GIS_Notice"), MessageDialogButton.Ok);
                        }

                        if (publishedEvent.EndType == LocationMonitorEndType.MonitorEnd)
                        {
                            vehicle.IsMonitor = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }

        protected override void DeactivateView(string viewName)
        {
            base.DeactivateView(viewName);
            ApplicationContext.Instance.CurrentView = -1;
        }

        private bool _FirstInitGISDisplayFromMonitorList = false;
        public void InitGISDisplayFromMonitorList()
        {
            try
            {
                if (_FirstInitGISDisplayFromMonitorList == true) return;

                foreach (var item in _TableData)
                {
                    MonitorGPS(item.VehicleId, item.Orgnization, true, item.HasAlarm, item.HasAlert);
                }
                if (this._MonitorPageCollectionView != null && this._MonitorPageCollectionView.Count > 0)
                {
                    this.IsEnableButtonEnable = true;
                    RaisePropertyChanged(() => this.IsEnableButtonEnable);
                }
                else
                {
                    this.IsEnableButtonEnable = false;
                    RaisePropertyChanged(() => this.IsEnableButtonEnable);
                }
                JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => MonitorPageCollectionView));
                _FirstInitGISDisplayFromMonitorList = true;
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }

        string selectedheader = ApplicationContext.Instance.StringResourceReader.GetString("MAINPAGE_Monitor");
        /// <summary>
        /// Switching event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Accordion_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                Accordion accordion = sender as Accordion;
                object obj = accordion.SelectedItem;
                AccordionItem item = obj as AccordionItem;
                selectedheader = item.Header.ToString();

                if (selectedheader == ApplicationContext.Instance.StringResourceReader.GetString("VehicleAlarm"))
                {
                    if (CurrentUnHandedAlarmInfo != null)
                    {
                        MonitorAlarmInfoDisplay info = new MonitorAlarmInfoDisplay();
                        info.DisPlayInfo = CurrentUnHandedAlarmInfo;
                        EventAggregator.Publish<MonitorAlarmInfoDisplay>(info);
                    }
                    else
                    {
                        EventAggregator.Publish<MonitorAlarmInfoDisplay>(new MonitorAlarmInfoDisplay());
                    }
                }
                else if (selectedheader == ApplicationContext.Instance.StringResourceReader.GetString("ALERT_VehicleAlert"))
                {
                    if (SelectedVehicleAlertModel != null)
                    {
                        MonitorAlertInfoDisplay info = new MonitorAlertInfoDisplay();
                        info.DisPlayInfo = SelectedVehicleAlertModel;
                        EventAggregator.Publish<MonitorAlertInfoDisplay>(info);
                    }
                }
                else if (selectedheader == ApplicationContext.Instance.StringResourceReader.GetString("SUITE_DeviceAlarmQuery"))
                {
                    if (SelectedVehicleDeviceAlertModel != null)
                    {
                        MonitorDeviceAlertInfoDisplay info = new MonitorDeviceAlertInfoDisplay();
                        info.DisPlayInfo = SelectedVehicleDeviceAlertModel;
                        EventAggregator.Publish<MonitorDeviceAlertInfoDisplay>(info);
                    }
                }

                RefreshGIS();
                RefreshDetail();
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }

        private Visibility _BaseInfoVisibility;
        public Visibility BaseInfoVisibility
        {
            get
            {
                return _BaseInfoVisibility;
            }
            set
            {
                _BaseInfoVisibility = value;
                RaisePropertyChanged("BaseInfoVisibility");
            }
        }

        private Visibility _AlarmInfoVisibility;
        public Visibility AlarmInfoVisibility
        {
            get
            {
                return _AlarmInfoVisibility;
            }
            set
            {
                _AlarmInfoVisibility = value;
                RaisePropertyChanged("AlarmInfoVisibility");
            }
        }

        private Visibility _AlertInfoVisibility;
        public Visibility AlertInfoVisibility
        {
            get
            {
                return _AlertInfoVisibility;
            }
            set
            {
                _AlertInfoVisibility = value;
                RaisePropertyChanged("AlertInfoVisibility");
            }
        }

    }
}
