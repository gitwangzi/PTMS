/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: d65d6572-ca96-4a1b-a61e-efd7b2875c7a      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: KENNY-PC
/////                 Author: TEST(jiaoyx)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Alert.ViewModels
/////    Project Description:    
/////             Class Name: VehicleAlertViewModel
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/8/5 16:41:50
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/8/5 16:41:50
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
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Shapes;
using System.Linq;
using Jounce.Core.ViewModel;
using Jounce.Framework.Command;
using Gsafety.PTMS.ServiceReference.VehicleAlertService;
using Gsafety.PTMS.Share;
using Jounce.Core.Model;
using Jounce.Core.Event;
using System.Windows.Data;
using Jounce.Framework;
using Gsafety.PTMS.Bases.Enums;
using Gsafety.PTMS.ServiceReference.MessageService;
using GisManagement.Models;
using Gsafety.Common.CommMessage;
using Gsafety.PTMS.ServiceReference.VehicleMonitorService;
using GisManagement.ViewModels;
using System.Text;
using Gsafety.PTMS.Bases.Models;
using Gsafety.PTMS.Alert.Models;
using Gsafety.PTMS.Alert.Views;
using Gsafety.PTMS.BasicPage.Monitor.Views;
using System.ComponentModel.Composition;

namespace Gsafety.PTMS.Alert.ViewModels
{
    [ExportAsViewModel(AlertName.VehicleAlertViewModel)]
    public class VehicleAlertViewModel : BaseViewModel,
        IEventSink<ColorConfigChange>,
        IPartImportsSatisfiedNotification
    {
        VehicleAlertServiceClient vehicleAlertServiceClient = null;
        private VehicleMonitorServiceClient vehicleMonitorServiceClient = null;

        #region UnHandle Property
        public VehicleAlertType AlertType { get; set; }

        /// <summary>
        /// Unhandled Page
        /// </summary>
        PagedCollectionView _VehicleAlertUnHandledPagedCV;
        public PagedCollectionView VehicleAlertUnHandledPagedCV
        {
            get { return _VehicleAlertUnHandledPagedCV; }
        }

        //alert type list
        public ObservableCollection<VehicleAlertType> VehicleAlertTypeList { get; set; }

        /// <summary>
        /// Untreated Selected Row
        /// </summary>
        private Gsafety.PTMS.ServiceReference.VehicleAlertService.VehicleAlert _selectedVehicleAlertModel;
        public Gsafety.PTMS.ServiceReference.VehicleAlertService.VehicleAlert SelectedVehicleAlertModel
        {
            get
            {
                return this._selectedVehicleAlertModel;
            }
            set
            {
                this._selectedVehicleAlertModel = value;

                if (_selectedVehicleAlertModel != null && CurrentAccordionSelectIndex == 0)
                {
                    string vehcileid = _selectedVehicleAlertModel.VehicleId;
                    //  EventAggregator.Publish<int>(0);
                    GetDeatilAlertModel(_selectedVehicleAlertModel);
                }
                RaisePropertyChanged(() => this.SelectedVehicleAlertModel);
            }
        }

        private string _carNumber;
        public string CarNumber
        {
            get
            {
                return this._carNumber;
            }
            set
            {
                this._carNumber = value.Trim();
                RaisePropertyChanged(() => this.CarNumber);
            }
        }


        private bool _IsUnhandledBusy;
        public bool IsUnhandledBusy
        {
            get
            {
                return this._IsUnhandledBusy;
            }
            set
            {
                this._IsUnhandledBusy = value;
                RaisePropertyChanged(() => this.IsUnhandledBusy);
            }
        }


        #endregion

        #region Handle Property
        //had already handle alert list page index
        public int HandleAlertPageIndex { get; set; }

        public VehicleAlertType AlertTypeHandled { get; set; }

        /// <summary>
        /// handled alarm at current
        /// </summary>
        private VehicleAlertEx _selectedVehicleAlertHandledModel;
        public VehicleAlertEx SelectedVehicleAlertHandledModel
        {
            get
            {
                return this._selectedVehicleAlertHandledModel;
            }
            set
            {
                this._selectedVehicleAlertHandledModel = value;
                if (_selectedVehicleAlertHandledModel != null && CurrentAccordionSelectIndex == 1)
                {
                    GetDeatilAlertModel(_selectedVehicleAlertHandledModel);
                    //  EventAggregator.Publish<VehicleAlert>(_selectedVehicleAlertHandledModel);
                }
                RaisePropertyChanged(() => this.SelectedVehicleAlertHandledModel);
            }
        }

        /// <summary>
        /// Page processed DataCollection
        /// </summary>
        private ObservableCollection<VehicleAlertEx> _VehicleAlertHandledModels;
        public ObservableCollection<VehicleAlertEx> VehicleAlertHandledModels
        {
            get
            {
                return this._VehicleAlertHandledModels;
            }
            set
            {
                this._VehicleAlertHandledModels = value;
                RaisePropertyChanged(() => this.VehicleAlertHandledModels);
            }
        }

        /// <summary>
        /// Handled vehicle alarm collection
        /// </summary>
        private PagedCollectionView _vehicleAlertPageView;
        public PagedCollectionView VehicleAlertPageView
        {
            get
            {
                return this._vehicleAlertPageView;
            }
            set
            {
                this._vehicleAlertPageView = value;
                RaisePropertyChanged(() => this.VehicleAlertPageView);
            }
        }

        private string _carNumberHandled;
        public string CarNumberHandled
        {
            get
            {
                return this._carNumberHandled;
            }
            set
            {
                this._carNumberHandled = value.Trim();
                RaisePropertyChanged(() => this.CarNumberHandled);
            }
        }

        private bool _isHandledBusy;
        public bool IsHandledBusy
        {
            get
            {
                return this._isHandledBusy;
            }
            set
            {
                this._isHandledBusy = value;
                RaisePropertyChanged(() => this.IsHandledBusy);
            }
        }

        private int _HandleAlertPageSizeValue = 20;
        public int HandleAlertPageSizeValue
        {
            get { return _HandleAlertPageSizeValue; }
            set
            {
                _HandleAlertPageSizeValue = value;
                RaisePropertyChanged(() => this.HandleAlertPageSizeValue);
            }
        }

        private DateTime? _EndTime;
        public DateTime? EndTime
        {
            get { return _EndTime; }
            set
            {
                _EndTime = value;
                if (value != null)
                {
                    _EndTime = new DateTime(value.Value.Year, value.Value.Month, value.Value.Day, 23, 59, 59);
                }
            }
        }

        private DateTime? _StartTime;
        public DateTime? StartTime
        {
            get { return _StartTime; }
            set { _StartTime = value; }
        }
        #endregion

        #region prop
        private VehicleAlertDetail currentalertdetail;
        Gsafety.PTMS.ServiceReference.VehicleAlertService.VehicleAlert currentalert = new Gsafety.PTMS.ServiceReference.VehicleAlertService.VehicleAlert();
        private int CurrentAccordionSelectIndex = 0;

        public string BusyContent { get { return ApplicationContext.Instance.StringResourceReader.GetString("ALARM_PleaseWait"); } }
        #endregion

        #region Command

        public ICommand SelectHandledMonitorGroup { get; private set; }
        public ICommand SelectMonitorGroup { get; private set; }
        /// <summary>
        /// unhandle
        /// </summary>
        public ICommand GetVehicleAlertCommand { get; private set; }
        /// <summary>
        /// already handle
        /// </summary>
        public ICommand GetVehicleAlertExCommand { get; private set; }
        /// <summary>
        /// locate
        /// </summary>
        public ICommand LocatePositionCommand { get; private set; }

        public ICommand HandleCommand { get; private set; }

        public ICommand LocateCommand { get; private set; }

        public ICommand LocateHandledCommand { get; private set; }

        public ICommand OpenDetailViewCommand { get; private set; }
        #endregion

        #region  Init
        public VehicleAlertViewModel()
        {
            try
            {
                //initialize client
                vehicleAlertServiceClient = ServiceClientFactory.Create<VehicleAlertServiceClient>();
                vehicleMonitorServiceClient = ServiceClientFactory.Create<VehicleMonitorServiceClient>();

                vehicleAlertServiceClient.GetVehicleHandledAlertCompleted += vehicleAlertServiceClient_GetVehicleHandledAlertCompleted;
                vehicleAlertServiceClient.GetVehicleAlertDetailCompleted += vehicleAlertServiceClient_GetVehicleAlertDetailCompleted;
                vehicleMonitorServiceClient.GetLastMonitorGPSCompleted += vehicleMonitorServiceClient_GetLastMonitorGPSCompleted;
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("VehicleAlertViewModel", ex);
            }

            _VehicleAlertUnHandledPagedCV = new PagedCollectionView(ApplicationContext.Instance.BufferManager.VehicleAlertManager.VehicleAlert);

            //initialize command
            GetVehicleAlertCommand = new ActionCommand<string>(obj => GetVehicleAlertAction(1));
            LocatePositionCommand = new ActionCommand<object>(obj => locatepositionAction(obj));
            LocateCommand = new ActionCommand<object>(obj => LocateAction(obj));
            HandleCommand = new ActionCommand<string>(obj => HandleAction(obj));
            SelectMonitorGroup = new ActionCommand<object>(obj => UnHandledMonitorGroup(obj));


            SelectHandledMonitorGroup = new ActionCommand<object>(obj => HandldedMonitorGroup(obj));
            GetVehicleAlertExCommand = new ActionCommand<object>(obj => GetHandleAlertAction(1));
            LocateHandledCommand = new ActionCommand<object>(obj => LocateHandledAction(obj));
            OpenDetailViewCommand = new ActionCommand<object>((obj) => OpenDetailViewClick_Event(obj));


            var targetlist = new List<Gsafety.PTMS.Bases.Enums.EnumInfos>();
            targetlist = new EnumAdapter<BusinessAlertType>().GetEnumInfos().Where(f => f.EnumAttributeInfo.Flag == "0").ToList();

            if (ApplicationContext.Instance.ServerConfig.AlertType == "1")
            {
                targetlist.AddRange(new EnumAdapter<BusinessAlertType>().GetEnumInfos().Where(f => f.EnumAttributeInfo.Flag == "1").ToList());
            }

            VehicleAlertTypeList = new ObservableCollection<VehicleAlertType>();

            foreach (var item in targetlist)
            {
                VehicleAlertTypeList.Add(new VehicleAlertType
                {
                    Code = (short)item.Value,
                    Name = ApplicationContext.Instance.StringResourceReader.GetString(item.Name)
                });
            }
            VehicleAlertTypeList.Insert(0, new VehicleAlertType { Code = 0, Name = ApplicationContext.Instance.StringResourceReader.GetString("BASEINFO_PleaseSelect") });

            this.AlertType = VehicleAlertTypeList.FirstOrDefault();
            this.AlertTypeHandled = VehicleAlertTypeList.FirstOrDefault();

            VehicleAlertHandledModels = new ObservableCollection<VehicleAlertEx>();
            VehicleAlertPageView = new PagedCollectionView(VehicleAlertHandledModels);

            this.VehicleAlertPageView.PageChanged += VehicleAlertPageView_PageChanged;
            this.VehicleAlertPageView.PageSize = HandleAlertPageSizeValue;
            HandleAlertPageIndex = 1;
            Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => HandleAlertPageIndex));
        }

        private void UnHandledMonitorGroup(object obj)
        {
            if (null != SelectedVehicleAlertModel)
            {
                Vehicle vehicle = ApplicationContext.Instance.BufferManager.VehicleOrganizationManage.VehicleList.Where(p => p.VehicleId == SelectedVehicleAlertModel.VehicleId).FirstOrDefault();
                if (null != vehicle)
                {
                    Gsafety.PTMS.BasicPage.Monitor.Views.GroupSelectWindow window = new GroupSelectWindow(vehicle);
                    window.Closed += groupSelectWindow_Closed;
                    window.Show();
                }
            }
        }

        private void HandldedMonitorGroup(object obj)
        {
            if (null != SelectedVehicleAlertHandledModel)
            {
                Vehicle vehicle = ApplicationContext.Instance.BufferManager.VehicleOrganizationManage.VehicleList.Where(p => p.VehicleId == SelectedVehicleAlertHandledModel.VehicleId).FirstOrDefault();
                if (null != vehicle)
                {
                    Gsafety.PTMS.BasicPage.Monitor.Views.GroupSelectWindow window = new GroupSelectWindow(vehicle);
                    window.Closed += groupSelectWindow_Closed;
                    window.Show();
                }
            }
        }

        void groupSelectWindow_Closed(object sender, EventArgs e)
        {
            //GroupSelectWindow win = sender as GroupSelectWindow;
            //if (null != win && win.DialogResult == true)
            //{
            //    if ((string.IsNullOrEmpty(win.OldGroup) && string.IsNullOrEmpty(win.model.GroupID)) || (win.OldGroup == win.model.GroupID))
            //    {
            //        return;
            //    }
            //    else if (string.IsNullOrEmpty(win.OldGroup) && !string.IsNullOrEmpty(win.model.GroupID))
            //    {
            //        Gsafety.PTMS.BasicPage.Monitor.ViewModels.MoniterGroupManage.Instance.AddVehicle(win.model, win.model.GroupID);
            //    }
            //    else if (!string.IsNullOrEmpty(win.OldGroup) && string.IsNullOrEmpty(win.model.GroupID))
            //    {
            //        Gsafety.PTMS.BasicPage.Monitor.ViewModels.MoniterGroupManage.Instance.DeleteVehicle(win.model);
            //    }
            //    else
            //    {
            //        Gsafety.PTMS.BasicPage.Monitor.ViewModels.MoniterGroupManage.Instance.MoveVehicle(win.model, win.OldGroup, win.model.GroupID);
            //    }

            //}
            GroupSelectWindow win = sender as GroupSelectWindow;
            if (null != win && win.DialogResult == true)
            {
                if ((string.IsNullOrEmpty(win.OldGroup) && string.IsNullOrEmpty(win.model.GroupID)) || (win.OldGroup == win.model.GroupID))
                {
                    return;
                }
                else if (string.IsNullOrEmpty(win.OldGroup) && !string.IsNullOrEmpty(win.model.GroupID))
                {
                    //Gsafety.PTMS.BasicPage.Monitor.ViewModels.MoniterGroupManage.Instance.AddVehicle(win.model, win.model.GroupID);
                    EventAggregator.Publish<UserDefineGroupVehicleDataRefreshMessage>(new UserDefineGroupVehicleDataRefreshMessage() { vehicle = win.model, doOperator = UserDefineGroupOperator.Add });
                }
                else if (!string.IsNullOrEmpty(win.OldGroup) && string.IsNullOrEmpty(win.model.GroupID))
                {
                    //Gsafety.PTMS.BasicPage.Monitor.ViewModels.MoniterGroupManage.Instance.DeleteVehicle(win.model);
                    EventAggregator.Publish<UserDefineGroupVehicleDataRefreshMessage>(new UserDefineGroupVehicleDataRefreshMessage() { vehicle = win.model, doOperator = UserDefineGroupOperator.Delete });
                }
                else
                {
                    //Gsafety.PTMS.BasicPage.Monitor.ViewModels.MoniterGroupManage.Instance.MoveVehicle(win.model, win.OldGroup, win.model.GroupID);
                    EventAggregator.Publish<UserDefineGroupVehicleDataRefreshMessage>(new UserDefineGroupVehicleDataRefreshMessage() { vehicle = win.model, doOperator = UserDefineGroupOperator.Move, oldGroup = win.OldGroup });
                }

            }
        }

        protected override void ActivateView(string viewName, System.Collections.Generic.IDictionary<string, object> viewParameters)
        {
            base.ActivateView(viewName, viewParameters);
            ApplicationContext.Instance.CurrentView = 2;
            EventAggregator.Publish(AlertName.AlertDetailPage.AsViewNavigationArgs());
            EventAggregator.Publish(GisManagement.GisName.GpsCarHisDataViewMonitor.AsViewNavigationArgs());

            ApplicationContext.Instance.CurrentGISName = GisManagement.GisName.AlertGisView;
            object mview = ApplicationContext.Instance.MenuManager.Router.ViewQuery(AlertName.VehicleAlertView);
            Frame frame = (mview as UserControl).FindName("ContentFrame") as Frame;
            if (frame.CurrentSource == null)
                return;
            frame.Refresh();
        }

        protected override void DeactivateView(string viewName)
        {
            base.DeactivateView(viewName);
            ApplicationContext.Instance.CurrentView = -1;
        }
        #endregion

        #region Handled Function
        private void locatepositionAction(object obj)
        {
            try
            {
                currentalert = obj as Gsafety.PTMS.ServiceReference.VehicleAlertService.VehicleAlert;
                vehicleMonitorServiceClient.GetLastMonitorGPSAsync(currentalert.VehicleId);
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("VehicleAlertViewModel", ex);
            }
        }

        void VehicleAlertPageView_PageChanged(object sender, EventArgs e)
        {
            HandleAlertPageIndex = ((PagedCollectionView)sender).PageIndex + 1;
            GetHandleAlertAction(HandleAlertPageIndex);
        }

        private void GetHandleAlertAction(int HandleAlertPageIndex)
        {
            IsHandledBusy = true;
            this.HandleAlertPageIndex = HandleAlertPageIndex;
            ServiceClientFactory.CreateMessageHeader(vehicleAlertServiceClient.InnerChannel);
            if (!string.IsNullOrEmpty(CarNumberHandled))
            {
                CarNumberHandled = CarNumberHandled.Trim();
            }
            if (StartTime != null && EndTime != null)
            {
                if (StartTime < EndTime)
                {
                    if (EndTime < DateTime.Now.Date.AddDays(1))
                    {
                        vehicleAlertServiceClient.GetVehicleHandledAlertAsync(null, null, null, CarNumberHandled, null, null, this.AlertTypeHandled == null ? 0 : this.AlertTypeHandled.Code, new PagingInfo { PageIndex = HandleAlertPageIndex, PageSize = HandleAlertPageSizeValue });
                        return;
                    }
                    else
                    {
                        MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("Rpt_EndTimeError"));
                    }
                }
                else
                {
                    MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("VDM_TimeSettingError"));
                }
            }
            vehicleAlertServiceClient.GetVehicleHandledAlertAsync(null, null, null, CarNumberHandled, StartTime, EndTime, this.AlertTypeHandled == null ? 0 : this.AlertTypeHandled.Code, new PagingInfo { PageIndex = HandleAlertPageIndex, PageSize = HandleAlertPageSizeValue });
        }

        private void LocateHandledAction(object obj)
        {
            try
            {
                Gsafety.PTMS.ServiceReference.VehicleAlertService.VehicleAlert vealert = obj as Gsafety.PTMS.ServiceReference.VehicleAlertService.VehicleAlert;
                #region
                if (vealert.GpsValid != "A")
                {
                    if (vealert.GpsValid == "N")
                    {
                        MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("ALERT_NoGPSModel"));

                    }
                    else if (vealert.GpsValid == "V")
                    {
                        MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("ALERT_NotEnable"));
                    }
                    else
                    {
                        MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("ALERT_GPSUnValid"));
                    }
                    return;
                }
                #endregion
                AlertAddRemoveArgs args = new AlertAddRemoveArgs();
                if (!vealert.IsMonitor.HasValue)
                {
                    args.Op = 1;
                    vealert.IsMonitor = true;
                }
                else
                {
                    if (vealert.IsMonitor == true)
                    {
                        args.Op = 0;
                        vealert.IsMonitor = false;
                    }
                    else if (vealert.IsMonitor == false)
                    {
                        args.Op = 1;
                        vealert.IsMonitor = true;
                    }
                }
                args.Latitude = vealert.Latitude;

                args.Longitude = vealert.Longitude;
                args.MdvrCoreSN = vealert.MdvrCoreId;
                args.Speed = vealert.Speed;
                args.GpsTime = vealert.GpsTime;
                args.GpsValid = vealert.GpsValid;
                args.Direction = vealert.Direction;
                args.AlertType = vealert.AlertType.Value;
                args.AlertTime = vealert.AlertTime.Value;
                args.VehicleID = vealert.VehicleId;

                if (((args.Op == 1) && (args.GpsValid == "A")) || (args.Op == 0))
                {
                    EventAggregator.Publish<AlertAddRemoveArgs>(args);
                }
                else
                {
                    if (args.GpsValid == "N")
                    {
                        MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("ALERT_NoGPSModel"));

                    }
                    else if (args.GpsValid == "V")
                    {
                        MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("ALERT_NotEnable"));
                    }
                    else
                    {
                        MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("ALERT_GPSUnValid"));
                    }
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("VehicleAlertViewModel", ex);
            }

        }
        #endregion

        #region Event
        /// <summary>
        /// GPS Information Recently
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void vehicleMonitorServiceClient_GetLastMonitorGPSCompleted(object sender, GetLastMonitorGPSCompletedEventArgs e)
        {
            //2016.8.1
            //if (e.Error != null)
            //{
            //    return;
            //}
            //if (e.Result.Result != null)
            //{
            //    Gsafety.PTMS.ServiceReference.VehicleMonitorService.PTMSGPS gpsinfo = e.Result.Result;

            //    AlertAddRemoveCurrentPosition args = new AlertAddRemoveCurrentPosition();
            //    if (!currentalert.IsLocation.HasValue)
            //    {
            //        args.Op = 1;
            //        currentalert.IsLocation = true;
            //    }
            //    else
            //    {
            //        if (currentalert.IsLocation == true)
            //        {
            //            args.Op = 0;
            //            currentalert.IsLocation = false;
            //        }
            //        else if (currentalert.IsLocation == false)
            //        {
            //            args.Op = 1;
            //            currentalert.IsLocation = true;
            //        }
            //    }
            //    args.Latitude = gpsinfo.Latitude;
            //    args.Longitude = gpsinfo.Longitude;
            //    args.MdvrCoreId = gpsinfo.MdvrCoreId;
            //    args.GPSTime = gpsinfo.GPSTime;
            //    args.GPSValid = gpsinfo.GPSValid;
            //    args.Direction = gpsinfo.Direction;
            //    args.Speed = gpsinfo.Speed;
            //    args.VehicleId = gpsinfo.VehicleId;
            //    args.AlertTime = currentalert.AlertTime.Value;
            //    args.VehicleType = (Gsafety.PTMS.Bases.Enums.VehicleType)currentalert.VehicleType;
            //    args.CityName = currentalert.City;
            //    args.ProvinceName = currentalert.Province;
            //    args.SuiteInfoId = currentalert.SuiteInfoId;

            //    if (((args.Op == 1) && (GPSState.Valid(args.GPSValid))) || (args.Op == 0))
            //    {
            //        EventAggregator.Publish<AlertAddRemoveCurrentPosition>(args);
            //    }
            //    else
            //    {
            //        if (!GPSState.Valid(args.GPSValid))
            //        {
            //            MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("ALERT_GPSUnValid"));
            //        }
            //    }
            //}
            //else
            //{
            //    MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("ALERT_GPSUnValid"));
            //}
        }

        private void vehicleAlertServiceClient_GetVehicleAlertDetailCompleted(object sender, GetVehicleAlertDetailCompletedEventArgs e)
        {
            currentalertdetail = new VehicleAlertDetail();//Add By penggl
            var Alertdeatil = new VehicleAlertDetail();
            if (e.Error == null || e.Result != null)
            {
                if (e.Result.Result != null)
                {
                    currentalertdetail = e.Result.Result;//Add By penggl
                    Alertdeatil = e.Result.Result;
                    EventAggregator.Publish<VehicleAlertDetail>(Alertdeatil);
                    if (CurrentAccordionSelectIndex == 1)
                    {
                        EventAggregator.Publish<VehicleAlertEx>(SelectedVehicleAlertHandledModel);
                    }
                }
            }
        }

        void vehicleAlertServiceClient_GetVehicleHandledAlertCompleted(object sender, GetVehicleHandledAlertCompletedEventArgs e)
        {
            try
            {
                if (e.Error != null)
                {
                    IsHandledBusy = false;
                }
                else
                {
                    IsHandledBusy = false;
                    VehicleAlertHandledModels = e.Result.Result;
                    foreach (var item in VehicleAlertHandledModels)
                    {
                        if (MonitorList.AlertHappenLocationElements.Exists(item.VehicleId + "@" + item.AlertTime))
                        {
                            item.IsMonitor = true;
                        }
                    }
                    if (VehicleAlertHandledModels != null && VehicleAlertHandledModels.Count > 0)
                        SelectedVehicleAlertHandledModel = VehicleAlertHandledModels[0];
                    if (HandleAlertPageIndex == 1)
                    {
                        List<int> pageList = new List<int>(e.Result.TotalRecord);
                        for (int i = 0; i < e.Result.TotalRecord; i++)
                        {
                            pageList.Add(i);
                        }
                        this.VehicleAlertPageView = new PagedCollectionView(pageList);
                        this.VehicleAlertPageView.PageChanged += VehicleAlertPageView_PageChanged;

                    }
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("VehicleAlertViewModel", ex);
            }
        }
        #endregion

        #region Unhandled Method

        private void GetDeatilAlertModel(Gsafety.PTMS.ServiceReference.VehicleAlertService.VehicleAlert _selectedVehicleAlertModel)
        {
            vehicleAlertServiceClient.GetVehicleAlertDetailAsync(_selectedVehicleAlertModel.Id);
        }

        private void GetDeatilAlertModel(VehicleAlertEx _selectedVehicleAlertHandledModel)
        {
            vehicleAlertServiceClient.GetVehicleAlertDetailAsync(_selectedVehicleAlertHandledModel.Id1);
        }

        private void GetVehicleAlertAction(int pageIndex)
        {
            if (this.AlertType.Code == 0)
            {
                if (!string.IsNullOrEmpty(CarNumber))
                {
                    VehicleAlertUnHandledPagedCV.Filter = null;
                    VehicleAlertUnHandledPagedCV.Filter = new Predicate<object>(FileterCarNum);
                }
                else
                {
                    VehicleAlertUnHandledPagedCV.Filter = null;
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(CarNumber))
                {
                    VehicleAlertUnHandledPagedCV.Filter = null;
                    VehicleAlertUnHandledPagedCV.Filter = new Predicate<object>(FileterCarNumAndAlerttype);
                }
                else
                {
                    VehicleAlertUnHandledPagedCV.Filter = null;
                    VehicleAlertUnHandledPagedCV.Filter = new Predicate<object>(FileterAlerttype);
                }
            }
            Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => VehicleAlertUnHandledPagedCV));
        }

        #region FilterActions
        private bool FileterCarNumAndAlerttype(object obj)
        {
            Gsafety.PTMS.ServiceReference.VehicleAlertService.VehicleAlert alertinfo = obj as Gsafety.PTMS.ServiceReference.VehicleAlertService.VehicleAlert;

            return alertinfo.VehicleId.Contains(CarNumber.Trim()) && (alertinfo.AlertType.Equals(this.AlertType.Code));
        }

        private bool FileterAlerttype(object obj)
        {
            Gsafety.PTMS.ServiceReference.VehicleAlertService.VehicleAlert alertinfo = obj as Gsafety.PTMS.ServiceReference.VehicleAlertService.VehicleAlert;
            return alertinfo.AlertType.Equals(this.AlertType.Code);
        }

        private bool FileterCarNum(object obj)
        {
            Gsafety.PTMS.ServiceReference.VehicleAlertService.VehicleAlert alertinfo = obj as Gsafety.PTMS.ServiceReference.VehicleAlertService.VehicleAlert;
            return alertinfo.VehicleId.ToLower().Contains(CarNumber.Trim().ToLower());
        }
        #endregion

        private void HandleAction(string obj)
        {
            try
            {
                if ("0".Equals(obj))
                {
                    HandleAlertWindow HandleAlertWindow = new Views.HandleAlertWindow();
                    HandleAlertWindow.ResultAction = R => HandleAlertWindowAction(R);
                    HandleAlertWindow.Show();
                    HandleAlertWindow.GetVehicleDetail(SelectedVehicleAlertModel.Id, SelectedVehicleAlertModel.MdvrCoreId, SelectedVehicleAlertModel.AlertTime.Value);
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("VehicleAlertViewModel", ex);
            }
        }

        private void HandleAlertWindowAction(bool msg)
        {
            try
            {
                if (msg)
                {
                    GetVehicleAlertAction(1);
                    GetHandleAlertAction(HandleAlertPageIndex);
                }
                else
                {
                    MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("ALERT_TrealFail"));
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("VehicleAlertViewModel", ex);
            }

        }

        private void TrealUnhandleAction()
        {
            try
            {
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => VehicleAlertUnHandledPagedCV));
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("VehicleAlertViewModel", ex);
            }

        }

        private void LocateAction(object obj)
        {
            try
            {
                Gsafety.PTMS.ServiceReference.VehicleAlertService.VehicleAlert vealert = obj as Gsafety.PTMS.ServiceReference.VehicleAlertService.VehicleAlert;
                #region
                if (vealert.GpsValid != "A")
                {
                    if (vealert.GpsValid == "N")
                    {
                        MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("ALERT_NoGPSModel"));
                    }
                    else if (vealert.GpsValid == "V")
                    {
                        MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("ALERT_NotEnable"));
                    }
                    else
                    {
                        MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("ALERT_GPSUnValid"));
                    }
                    return;
                }
                #endregion
                AlertAddRemoveArgs args = new AlertAddRemoveArgs();
                if (!vealert.IsMonitor.HasValue)
                {
                    args.Op = 1;
                    vealert.IsMonitor = true;
                }
                else
                {
                    if (vealert.IsMonitor == true)
                    {
                        args.Op = 0;
                        vealert.IsMonitor = false;
                    }
                    else if (vealert.IsMonitor == false)
                    {
                        args.Op = 1;
                        vealert.IsMonitor = true;
                    }
                }
                args.Latitude = vealert.Latitude;
                args.Longitude = vealert.Longitude;
                args.MdvrCoreSN = vealert.MdvrCoreId;
                args.Speed = vealert.Speed;
                args.GpsTime = vealert.GpsTime;
                args.GpsValid = vealert.GpsValid;
                args.AlertTime = vealert.AlertTime.Value;
                args.Direction = vealert.Direction;
                args.AlertType = vealert.AlertType.Value;
                args.VehicleID = vealert.VehicleId;


                if (((args.Op == 1) && (args.GpsValid == "A")) || (args.Op == 0))
                {
                    EventAggregator.Publish<AlertAddRemoveArgs>(args);
                }
                else
                {
                    if (args.GpsValid == "N")
                    {
                        MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("ALERT_NoGPSModel"));
                    }
                    else if (args.GpsValid == "V")
                    {
                        MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("ALERT_NotEnable"));
                    }
                    else
                    {
                        MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("ALERT_GPSUnValid"));
                    }
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("VehicleAlertViewModel", ex);
            }

        }
        #endregion

        /// <summary>
        /// Detail on-off state
        /// </summary>
        /// <param name="obj"></param>
        internal void OpenDetailViewClick_Event(object obj)
        {
            EventAggregator.Publish<OpenState>(new OpenState() { State = true });
        }

        /// <summary>
        /// Switch between unhandle and handle
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <remarks>
        /// Modify by wzs
        /// </remarks>
        public void Accordion_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                Accordion accordion = sender as Accordion;
                CurrentAccordionSelectIndex = accordion.SelectedIndex;
                if (CurrentAccordionSelectIndex == 0)
                {
                    SelectedVehicleAlertModel = SelectedVehicleAlertModel;
                    EventAggregator.Publish<int>(2);
                }
                else if (CurrentAccordionSelectIndex == 1)
                {
                    SelectedVehicleAlertHandledModel = SelectedVehicleAlertHandledModel;
                    EventAggregator.Publish<int>(3);
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("VehicleAlertViewModel", ex);
            }
        }

        public void HandleEvent(ColorConfigChange publishedEvent)
        {
            _VehicleAlertUnHandledPagedCV = new PagedCollectionView(ApplicationContext.Instance.BufferManager.VehicleAlertManager.VehicleAlert);
            GetHandleAlertAction(HandleAlertPageIndex);
            RaisePropertyChanged(() => VehicleAlertUnHandledPagedCV);
            RaisePropertyChanged(() => VehicleAlertHandledModels);
        }

        public void OnImportsSatisfied()
        {
            EventAggregator.SubscribeOnDispatcher<ColorConfigChange>(this);
        }
    }
}
