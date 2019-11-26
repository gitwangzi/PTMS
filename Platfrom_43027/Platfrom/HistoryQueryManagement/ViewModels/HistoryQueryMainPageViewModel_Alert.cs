using GisManagement.Models;
using Gsafety.Common.CommMessage;
using Gsafety.PTMS.ServiceReference.VehicleAlertService;
using Gsafety.PTMS.ServiceReference.VehicleService;
using Gsafety.PTMS.ServiceReference.RunMonitorGroupService;
using Gsafety.PTMS.Share;
using Gsafety.PTMS.Monitor;
using Gsafety.PTMS.Monitor.Models;
using Gsafety.PTMS.Bases.Enums;
using Jounce.Framework.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Linq;
using Gsafety.Common.Controls;
using System.Reflection;
using Jounce.Core.ViewModel;
using Jounce.Framework;
using Gsafety.PTMS.Spreadsheet;
using Gsafety.Ant.HistoryQueryManagement.Model;

namespace HistoryQueryManagement.ViewModels
{
    public partial class HistoryQueryMainPageViewModel
    {
        ICommand _AlertSearchCommand;


        //alert type list
        public ObservableCollection<VehicleAlertType> VehicleAlertTypeList { get; set; }


        private VehicleAlertType selectedAlertType;
        /// <summary>
        /// 
        /// </summary>
        public VehicleAlertType SelectedAlertType
        {
            get
            {
                return selectedAlertType;
            }
            set
            {
                this.selectedAlertType = value;
                RaisePropertyChanged(() => this.SelectedAlertType);
            }
        }

   
        public List<string> VehicleIds = new List<string>();

    

        private void InitalAlert()
        {
            try
            {

                //Groups = new List<RunMonitorGroup>();
                //RunMonitorGroup mg = new RunMonitorGroup();
                //mg.GroupIndex = -1;
                //mg.GroupName = ApplicationContext.Instance.StringResourceReader.GetString("All");
                //Groups.Add(mg);
                //Groups.AddRange(ApplicationContext.Instance.BufferManager.MonitorGroupManager.MoniterGroupManagerOC);
                //SearchByGroup = Groups[0];

                var temp = new EnumAdapter<BusinessAlertType>().GetEnumInfos();

                VehicleAlertTypeList = new ObservableCollection<VehicleAlertType>();

                foreach (var item in temp)
                {
                    if (item.Value < 4)
                    {
                        VehicleAlertTypeList.Add(new VehicleAlertType
                        {
                            Code = (short)item.Value,
                            Name = item.LocalizedString
                        });
                    }
                }
                //VehicleAlertTypeList.Insert(0, new VehicleAlertType { Code = -1, Name = ApplicationContext.Instance.StringResourceReader.GetString("BASEINFO_PleaseSelect") });
                VehicleAlertTypeList.Insert(0, new VehicleAlertType { Code = -1, Name = ApplicationContext.Instance.StringResourceReader.GetString("All") });
                SelectedAlertType = VehicleAlertTypeList[0];
                _AlertSearchCommand = new ActionCommand<object>(obj => AlertSearch_Event(obj));
                _AlertExportCommand = new ActionCommand<object>(obj => AlertExport_Event());

                AllAlerts = new ObservableCollection<BusinessAlertEx>();
                AlertPageIndex = 1;
                ////Processed an alarm paging key
                AlertsDataPager = new PagedCollectionView(AllAlerts);
                this.AlertsDataPager.PageChanged += AlertsDataPager_PageChanged;
                this.AlertsDataPager.PageSize = AlertsPageSizeValue;

                AlertHistroyTraceCommand = new ActionCommand<object>((obj) => AlertHistoriyTrace_Event(obj));
                MarkAlertHappenGraphicCommand = new ActionCommand<object>((obj) => MarkAlertHappenGraphic_Event(obj));
                vehicleAlertClient = InitialAlertClient();
                this.ChauffeurList = new ObservableCollection<Gsafety.PTMS.ServiceReference.ChauffeurService.Chauffeur>();
                //VehicleTypeClient client = ServiceClientFactory.Create<VehicleTypeClient>();
                //client.GetVehicleTypeListCompleted += client_GetVehicleTypeListCompleted;
                //client.GetVehicleTypeListAsync(ApplicationContext.Instance.AuthenticationInfo.ClientID);
                //分布类中的初始化获取驾驶员客户端的方法
                this.InilitClient();
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }

        private void AlertExport_Event()
        {
            try
            {
                SaveFileDialog dlg = new SaveFileDialog();
                dlg.Filter = "Excel Document (.xlsx)|*.xlsx";
                dlg.DefaultExt = ".xlsx";
                bool? dialogResult = dlg.ShowDialog();
                if (dialogResult == true)
                {
                    VehicleAlertServiceClient client = ServiceClientFactory.Create<VehicleAlertServiceClient>();
                    client.GetAllBusinessAlertCompleted += (s, e) =>
                    {
                        try
                        {
                            if (e.Result != null && e.Result.TotalRecord > 0)
                            {
                                foreach (var item in e.Result.Result)
                                {
                                    item.AlertTime = item.AlertTime.Value.ToLocalTime();
                                }
                                List<string> Codes = new List<string>();
                                Codes.Add("VehicleId");
                                Codes.Add("MdvrCoreId");
                                Codes.Add("AlertTime");
                                Codes.Add("AlertType");
                                Codes.Add("Longitude");
                                Codes.Add("Latitude");
                                Codes.Add("Speed");
                                Codes.Add("Direction");


                                List<string> Names = new List<string>();
                                Names.Add(ApplicationContext.Instance.StringResourceReader.GetString("BASEINFO_VehicleNumber"));
                                Names.Add(ApplicationContext.Instance.StringResourceReader.GetString("BASEINFO_SuiteID"));
                                Names.Add(ApplicationContext.Instance.StringResourceReader.GetString("ALERT_ALERTTime"));
                                Names.Add(ApplicationContext.Instance.StringResourceReader.GetString("ALERT_AlertType"));
                                Names.Add(ApplicationContext.Instance.StringResourceReader.GetString("ALARM_Longitude"));
                                Names.Add(ApplicationContext.Instance.StringResourceReader.GetString("Latitude"));
                                Names.Add(ApplicationContext.Instance.StringResourceReader.GetString("Speed"));
                                Names.Add(ApplicationContext.Instance.StringResourceReader.GetString("ALARM_DIR"));

                                List<AlertExportModel> list = new List<AlertExportModel>();
                                foreach (var item in e.Result.Result)
                                {
                                    try
                                    {
                                        AlertExportModel vem = new AlertExportModel();
                                        vem.VehicleId = item.VehicleId;
                                        vem.SuiteID = item.MdvrCoreId;
                                        vem.AlertTime = item.AlertTime;
                                        int state = (int)item.AlertType;
                                        switch (state)
                                        {
                                            case 0:
                                                vem.AlertType = ApplicationContext.Instance.StringResourceReader.GetString("BusinessAlertType_OverSpeed");
                                                break;
                                            case 1:
                                                vem.AlertType = ApplicationContext.Instance.StringResourceReader.GetString("BusinessAlertType_InOutAera");
                                                break;
                                            case 2:
                                                vem.AlertType = ApplicationContext.Instance.StringResourceReader.GetString("BusinessAlertType_InOutRoute");
                                                break;
                                            case 3:
                                                vem.AlertType = ApplicationContext.Instance.StringResourceReader.GetString("BusinessAlertType_RouteOffset");
                                                break;
                                        }
                                        vem.Latitude = item.Latitude;
                                        vem.Longitude = item.Longitude;
                                        vem.Speed = item.Speed;
                                        vem.Direction = item.Direction;
                                        list.Add(vem);
                                    }
                                    catch (Exception)
                                    {

                                    }

                                }
                                XLSXExporter xlsx = new XLSXExporter();
                                xlsx.Export(list, dlg.OpenFile(), Codes, Names);
                                MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString("TRAFFIC_TIP"), ApplicationContext.Instance.StringResourceReader.GetString("BaseInfo_ExportSucceed"), MessageDialogButton.Ok);
                            }
                            else
                            {
                                MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString("TRAFFIC_TIP"), ApplicationContext.Instance.StringResourceReader.GetString("BaseInfo_ExportFaild"), MessageDialogButton.Ok);
                            }

                        }
                        catch (Exception)
                        {

                        }
                    };


                    if (AllAlerts.Count > 10000)
                    {
                        if (!string.IsNullOrEmpty(AlertCarNumber))
                        {
                            AlertCarNumber = AlertCarNumber.Trim();
                        }
                        DateTime? begintime = AlertStartTime;
                        DateTime? endtime = AlertEndTime;
                        if (AlertStartTime.HasValue && AlertEndTime.HasValue)
                        {

                            begintime = AlertStartTime.Value.ToUniversalTime();
                            endtime = AlertEndTime.Value.ToUniversalTime();
                        }
                        ObservableCollection<string> organizations = new ObservableCollection<string>();
                        foreach (var item in ApplicationContext.Instance.AuthenticationInfo.Organizations)
                        {
                            organizations.Add(item.ID);
                        }
                        client.GetAllBusinessAlertAsync(AlertCarNumber, begintime, endtime, new Gsafety.PTMS.ServiceReference.VehicleAlertService.PagingInfo { PageIndex = 1, PageSize = 10000 }, organizations, selectedAlertType.Code);
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(AlertCarNumber))
                        {
                            AlertCarNumber = AlertCarNumber.Trim();
                        }
                        DateTime? begintime = AlertStartTime;
                        DateTime? endtime = AlertEndTime;
                        if (AlertStartTime.HasValue && AlertEndTime.HasValue)
                        {

                            begintime = AlertStartTime.Value.ToUniversalTime();
                            endtime = AlertEndTime.Value.ToUniversalTime();
                        }
                        ObservableCollection<string> organizations = new ObservableCollection<string>();
                        foreach (var item in ApplicationContext.Instance.AuthenticationInfo.Organizations)
                        {
                            organizations.Add(item.ID);
                        }
                        client.GetAllBusinessAlertAsync(AlertCarNumber, begintime, endtime, new Gsafety.PTMS.ServiceReference.VehicleAlertService.PagingInfo { PageIndex = AlertsDataPager.PageIndex + 1, PageSize = AlertsPageSizeValue }, organizations,selectedAlertType.Code);
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        //void client_GetVehicleTypeListCompleted(object sender, GetVehicleTypeListCompletedEventArgs e)
        //{
        //    if (e.Error == null)
        //    {
        //        VehicleTypes = new List<Gsafety.PTMS.ServiceReference.VehicleService.VehicleType>();
        //        foreach (var item in e.Result.Result)
        //        {
        //            VehicleTypes.Add(item);
        //        }

        //        VehicleTypes.Insert(0, new Gsafety.PTMS.ServiceReference.VehicleService.VehicleType() { ID = string.Empty, Name = ApplicationContext.Instance.StringResourceReader.GetString("All") });

        //        JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => VehicleTypes));

        //        SearchByVehicleType = VehicleTypes[0];

        //        JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => SearchByVehicleType));
        //    }
        //}
      
        private void MarkAlertHappenGraphic_Event(object obj)
        {
            if (obj == null) return;
            BusinessAlertEx ainfo = obj as BusinessAlertEx;
            if ((ainfo.GpsValid == "N") || (ainfo.GpsValid == null))
            {
                MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString(LProxy.Caption), ApplicationContext.Instance.StringResourceReader.GetString("GIS_GpsInValid"));
                return;
            }
            ainfo.IsMarkGraphic = !ainfo.IsMarkGraphic;

        }
        private VehicleAlertServiceClient InitialAlertClient()
        {
            VehicleAlertServiceClient vehicleAlertClient = ServiceClientFactory.Create<VehicleAlertServiceClient>();
            vehicleAlertClient.GetAllBusinessAlertCompleted += vehicleAlertClient_GetAllBusinessAlertCompleted;
            vehicleAlertClient.GetBusinessAlertHandleByAlertIDCompleted += vehicleAlertClient_GetBusinessAlertHandleByAlertIDCompleted;

            return vehicleAlertClient;
        }

        private void AlertHistoriyTrace_Event(object obj)
        {
            if (obj == null) return;
            BusinessAlertEx ainfo = obj as BusinessAlertEx;
            EventAggregator.Publish<DisplayHistoricalRoute>(new DisplayHistoricalRoute()
            {
                VechileId = ainfo.VehicleId,
                StartTime = ainfo.GpsTime == null ? DateTime.Now.AddHours(-1) : ainfo.GpsTime.Value.AddHours(-1),
                EndTime = ainfo.GpsTime == null ? DateTime.Now : ainfo.GpsTime.Value.AddHours(1),
            });

        }

        public ICommand AlertHistroyTraceCommand { get; private set; }

        public int AlertCount { get; set; }

        void vehicleAlertClient_GetAllBusinessAlertCompleted(object sender, GetAllBusinessAlertCompletedEventArgs e)
        {
            try
            {
                if (e.Error == null)
                {
                    AllAlerts = e.Result.Result;
                    AlertCount = e.Result.TotalRecord;
                    RaisePropertyChanged(() => AlertCount);
                    if (AllAlerts != null && AllAlerts.Count > 0)
                        SelectedAlert = AllAlerts[0];

                    foreach (var item in AllAlerts)
                    {
                        if (item.AlertTime.HasValue)
                        {
                            item.AlertTime = item.AlertTime.Value.ToLocalTime();
                        }
                        item.VehicleInfo = ApplicationContext.Instance.BufferManager.VehicleOrganizationManage.VehicleList.FirstOrDefault(t => t.VehicleId == item.VehicleId);
                    }
                    if (AlertPageIndex == 1)
                    {
                        List<int> pageList = new List<int>(e.Result.TotalRecord);
                        for (int i = 0; i < e.Result.TotalRecord; i++)
                        {
                            pageList.Add(i);
                        }
                        AlertsDataPager = new PagedCollectionView(pageList);
                        AlertsDataPager.PageChanged += AlertsDataPager_PageChanged;
                    }
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("vehicleAlertClient_GetAllBusinessAlertCompleted", ex);
            }
            finally
            {
                VehicleAlertServiceClient client = sender as VehicleAlertServiceClient;
                CloseAlertClient(client);
            }

        }
        public ICommand MarkAlertHappenGraphicCommand { get; private set; }

        private void CloseAlertClient(VehicleAlertServiceClient client)
        {
            client.CloseAsync();
            client = null;
        }

        void AlertsDataPager_PageChanged(object sender, EventArgs e)
        {
            AlertPageIndex = ((PagedCollectionView)sender).PageIndex + 1;
            GetAlertByPage(AlertPageIndex);
        }

        private void AlertSearch_Event(object obj)
        {
            GetAlertByPage(1);
        }

        public ICommand AlertSearchCommand
        {
            get { return _AlertSearchCommand; }
            set { _AlertSearchCommand = value; }
        }

        private ICommand _AlertExportCommand;
        public ICommand AlertExportCommand
        {
            get { return _AlertExportCommand; }
            set { _AlertExportCommand = value; }
        }

        private Nullable<DateTime> _AlertStartTime;

        private Nullable<DateTime> _AlertEndTime;

        public Nullable<DateTime> AlertStartTime
        {
            get { return _AlertStartTime; }
            set
            {
                _AlertStartTime = value;
                RaisePropertyChanged(() => this.AlertStartTime);
            }
        }
        /// <summary>
        /// End Time
        /// </summary>
        public Nullable<DateTime> AlertEndTime
        {
            get { return _AlertEndTime; }
            set
            {
                if (value != null)
                {
                    _AlertEndTime = value;
                    RaisePropertyChanged(() => this.AlertEndTime);
                }
                else
                {
                    _AlertEndTime = null;
                }
            }
        }

        public int AlertPageIndex
        {
            get;
            set;
        }
        public int AlertItemCount
        {
            get;
            set;
        }
        private PagedCollectionView _AlertsPageSizeValueDatePager;
        public PagedCollectionView AlertsDataPager
        {
            get
            {
                return this._AlertsPageSizeValueDatePager;
            }
            set
            {
                this._AlertsPageSizeValueDatePager = value;
                RaisePropertyChanged(() => this.AlertsDataPager);
            }
        }
        private int _AlertsPageSizeValue = 30;
        public int AlertsPageSizeValue
        {
            get
            {
                return this._AlertsPageSizeValue;
            }
            set
            {
                this._AlertsPageSizeValue = value;
                RaisePropertyChanged(() => this.AlertsPageSizeValue);
            }
        }

        private ObservableCollection<BusinessAlertEx> _allAlerts;

        public ObservableCollection<BusinessAlertEx> AllAlerts
        {
            get { return this._allAlerts; }
            set
            {
                _allAlerts = value;
                RaisePropertyChanged(() => this.AllAlerts);
            }
        }

        private string _AlertCarNumber;
        /// <summary>
        /// Processed license plate number
        /// </summary>
        public string AlertCarNumber
        {
            get { return _AlertCarNumber; }
            set
            {
                _AlertCarNumber = value.Trim();
            }
        }

        private BusinessAlertEx _selectedAlert;

        public BusinessAlertEx SelectedAlert
        {
            get
            {
                return _selectedAlert;
            }
            set
            {
                if (_selectedAlert != null)
                {
                    if (_selectedAlert.IsMarkGraphic == false) //如果选择其他对象后，上一个对象没有被标识，则从地图删除
                    {
                        EventAggregator.Publish<AlertAddRemoveCurrentPosition>(new AlertAddRemoveCurrentPosition() { Direction = _selectedAlert.Direction, AlertTime = _selectedAlert.AlertTime, GpsTime = _selectedAlert.GpsTime, Speed = _selectedAlert.Speed, Valid = _selectedAlert.GpsValid, Latitude = _selectedAlert.Latitude, Longitude = _selectedAlert.Longitude, Op = 0, VehicleId = _selectedAlert.VehicleId });
                    }
                }
                if (_selectedAlert != value)
                {
                    _selectedAlert = value;
                    OnAlertSelectChange(_selectedAlert);
                }


                if (_selectedAlert != null)
                {
                    if (GPSState.Valid(_selectedAlert.GpsValid))
                    {
                        EventAggregator.Publish<AlertAddRemoveCurrentPosition>(new AlertAddRemoveCurrentPosition() { Direction = _selectedAlert.Direction, AlertTime = _selectedAlert.AlertTime, GpsTime = _selectedAlert.GpsTime, Speed = _selectedAlert.Speed, Valid = _selectedAlert.GpsValid, Latitude = _selectedAlert.Latitude, Longitude = _selectedAlert.Longitude, Op = 1, VehicleId = _selectedAlert.VehicleId });
                    }
                    else
                    {
                        if (selectedheader == ApplicationContext.Instance.StringResourceReader.GetString("AlertHistory"))
                        {
                            MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString("GIS_UNValidXY"));
                        }
                     
                    }
                }
            }
        }

        Visibility _alertVisiblity = Visibility.Collapsed;

        public Visibility AlertVisiblity
        {
            get { return _alertVisiblity; }
            set
            {
                _alertVisiblity = value;
                RaisePropertyChanged(() => this.AlertVisiblity);
            }
        }

        private void GetAlertByPage(int pageIndex)
        {
            try
            {
                if (!string.IsNullOrEmpty(AlertCarNumber))
                {
                    AlertCarNumber = AlertCarNumber.Trim();
                }
                DateTime? begintime = AlertStartTime;
                DateTime? endtime = AlertEndTime;
                if (AlertStartTime != null && AlertEndTime != null)
                {
                    if (AlertStartTime < AlertEndTime)
                    {
                        if (AlertEndTime > DateTime.Now.Date.AddDays(1))
                        {
                            //MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("Rpt_EndTimeError"));
                            MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString("Tip"),
                                ApplicationContext.Instance.StringResourceReader.GetString("Rpt_EndTimeError"));
                            return;
                        }
                        else
                        {
                            begintime = AlertStartTime.Value.ToUniversalTime();
                            endtime = AlertEndTime.Value.ToUniversalTime();
                        }
                    }
                    else
                    {
                        //MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("VDM_TimeSettingError"));
                        MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString("Tip"),
                               ApplicationContext.Instance.StringResourceReader.GetString("VDM_TimeSettingError"));
                        return;
                    }
                }

                AlertPageIndex = pageIndex;
                ObservableCollection<string> organizations = new ObservableCollection<string>();
                foreach (var item in ApplicationContext.Instance.AuthenticationInfo.Organizations)
                {
                    organizations.Add(item.ID);
                }
                VehicleAlertServiceClient vehicleAlertClient = InitialAlertClient();
                vehicleAlertClient.GetAllBusinessAlertAsync(AlertCarNumber, begintime, endtime, new Gsafety.PTMS.ServiceReference.VehicleAlertService.PagingInfo { PageIndex = pageIndex, PageSize = AlertsPageSizeValue }, organizations, selectedAlertType.Code);
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }
    }
}
