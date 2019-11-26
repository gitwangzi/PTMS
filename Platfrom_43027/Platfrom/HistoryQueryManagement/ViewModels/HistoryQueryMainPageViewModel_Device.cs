using GisManagement.Models;
using Gsafety.Common.CommMessage;
using Gsafety.PTMS.ServiceReference.DeviceAlertService;
using Gsafety.PTMS.Share;
using Gsafety.PTMS.Monitor;
using Gsafety.PTMS.Monitor.Models;
using Jounce.Framework.Command;
using Gsafety.PTMS.Bases.Enums;
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
using BaseLib.Model;
using BaseLib.ViewModels;
using Gsafety.Ant.HistoryQueryManagement.Model;
using Gsafety.PTMS.Spreadsheet;

namespace HistoryQueryManagement.ViewModels
{
    public partial class HistoryQueryMainPageViewModel
    {
       

        ICommand _DeviceAlertSearchCommand;

        ICommand _DeviceAlertExportCommand;

        private ObservableCollection<VehicleAlertType> _zAlertTypes = new ObservableCollection<VehicleAlertType>();
        /// <summary>
        /// 车辆情况
        /// </summary>
        public ObservableCollection<VehicleAlertType> ZAlertTypes
        {
            get { return _zAlertTypes; }
            set
            {
                _zAlertTypes = value;
                RaisePropertyChanged(() => ZAlertTypes);
            }
        }

        private VehicleAlertType searchByAlertType;
        /// <summary>
        /// 
        /// </summary>
        public VehicleAlertType SearchByAlertType
        {
            get
            {
                return searchByAlertType;
            }
            set
            {
                this.searchByAlertType = value;
                RaisePropertyChanged(() => this.SearchByAlertType);
            }
        }

        private void InitalDeviceAlert()
        {
            try
            {               

                ZAlertTypes.Add(new VehicleAlertType()
                {
                    Code = (int)DeviceAlertTypes.MAINTAIN_Full_N,
                    Name = ApplicationContext.Instance.StringResourceReader.GetString("All")
                });
                ZAlertTypes.Add(new VehicleAlertType()
                {
                    Code = (int)DeviceAlertTypes.GNSSModelError,
                    Name = ApplicationContext.Instance.StringResourceReader.GetString("GNSSModelError")
                });
                ZAlertTypes.Add(new VehicleAlertType()
                {
                    Code = (int)DeviceAlertTypes.GNSSNoAntenna,
                    Name = ApplicationContext.Instance.StringResourceReader.GetString("GNSSNoAntenna")
                });
                ZAlertTypes.Add(new VehicleAlertType()
                {
                    Code = (int)DeviceAlertTypes.GNSSCircuit,
                    Name = ApplicationContext.Instance.StringResourceReader.GetString("GNSSCircuit")
                });
                ZAlertTypes.Add(new VehicleAlertType()
                {
                    Code = (int)DeviceAlertTypes.PowerSourceNoVoltage,
                    Name = ApplicationContext.Instance.StringResourceReader.GetString("PowerSourceNoVoltage")
                });
                ZAlertTypes.Add(new VehicleAlertType()
                {
                    Code = (int)DeviceAlertTypes.PowerSourceNoPower,
                    Name = ApplicationContext.Instance.StringResourceReader.GetString("PowerSourceNoPower")
                });
                ZAlertTypes.Add(new VehicleAlertType()
                {
                    Code = (int)DeviceAlertTypes.LEDError,
                    Name = ApplicationContext.Instance.StringResourceReader.GetString("LEDError")
                });
                ZAlertTypes.Add(new VehicleAlertType()
                {
                    Code = (int)DeviceAlertTypes.TTSError,
                    Name = ApplicationContext.Instance.StringResourceReader.GetString("TTSError")
                });
                ZAlertTypes.Add(new VehicleAlertType()
                {
                    Code = (int)DeviceAlertTypes.VidiconError,
                    Name = ApplicationContext.Instance.StringResourceReader.GetString("VidiconError")
                });

                SearchByAlertType = ZAlertTypes[0];


                DeviceAlertSearchCommand = new ActionCommand<object>(obj => DeviceAlertSearch_Event(obj));
                DeviceAlertLocateCommand = new ActionCommand<object>(obj => MarkAllDeviceAlertHappenGraphic_Event(obj));
                DeviceAlertExportCommand = new ActionCommand<object>(obj => DeviceAlertExport_Event());
                AllDeviceAlerts = new ObservableCollection<DeviceAlertEx>();
                DeviceAlertPageIndex = 1;
                ////Processed an alarm paging key
                DeviceAlertsDataPager = new PagedCollectionView(AllDeviceAlerts);
                this.DeviceAlertsDataPager.PageChanged += DeviceAlertsDataPager_PageChanged;
                this.DeviceAlertsDataPager.PageSize = DeviceAlertsPageSizeValue;

                DeviceAlertHistroyTraceCommand = new ActionCommand<object>((obj) => DeviceAlertHistoriyTrace_Event(obj));
                MarkDeviceAlertHappenGraphicCommand = new ActionCommand<object>((obj) => MarkDeviceAlertHappenGraphic_Event(obj));
                deviceAlertClient = InitialDeviceAlertClient();
                //this.ChauffeurList = new ObservableCollection<Gsafety.PTMS.ServiceReference.ChauffeurService.Chauffeur>();
                //分布类中的初始化获取驾驶员客户端的方法
                //this.InilitDeviceClient();
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }

        private void DeviceAlertExport_Event()
        {
            try
            {
                SaveFileDialog dlg = new SaveFileDialog();
                dlg.Filter = "Excel Document (.xlsx)|*.xlsx";
                dlg.DefaultExt = ".xlsx";
                bool? dialogResult = dlg.ShowDialog();
                if (dialogResult == true)
                {
                    DeviceAlertServiceClient client = ServiceClientFactory.Create<DeviceAlertServiceClient>();

                    if (!string.IsNullOrEmpty(DeviceAlertCarNumber))
                    {
                        DeviceAlertCarNumber = DeviceAlertCarNumber.Trim();
                    }
                    DateTime? begintime = DeviceAlertStartTime;
                    DateTime? endtime = DeviceAlertEndTime;
                    if (DeviceAlertStartTime.HasValue && DeviceAlertEndTime.HasValue)
                    {

                        begintime = DeviceAlertStartTime.Value.ToUniversalTime();
                        endtime = DeviceAlertEndTime.Value.ToUniversalTime();
                    }
                    ObservableCollection<decimal?> type = new ObservableCollection<decimal?>();
                    if (searchByAlertType.Code != -1)
                    {
                        type.Add(searchByAlertType.Code);
                    }

                    client.GetDeviceAlertEx1Completed += (s, e) =>
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
                                        var alerttype = ZAlertTypes.Where(t => t.Code == item.AlertType).FirstOrDefault();
                                        if (alerttype != null)
                                            vem.AlertType = alerttype.Name;
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


                    if (AllDeviceAlerts.Count > 10000)
                    {                        
                        client.GetDeviceAlertEx1Async(DeviceAlertCarNumber,"",type, begintime, endtime, new PagingInfo { PageIndex = 1, PageSize = 10000 });
                    }
                    else
                    {                       
                        client.GetDeviceAlertEx1Async(DeviceAlertCarNumber, "", type, begintime, endtime, new PagingInfo { PageIndex = 1, PageSize = DeviceAlertsPageSizeValue });
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void MarkDeviceAlertHappenGraphic_Event(object obj)
        {
            if (obj == null) return;
            DeviceAlertEx ainfo = obj as DeviceAlertEx;
            if ((ainfo.GpsValid == "N") || (ainfo.GpsValid == null))
            {
                MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString(LProxy.Caption), ApplicationContext.Instance.StringResourceReader.GetString("GIS_GpsInValid"));
                return;
            }
            ainfo.IsMarkGraphic = !ainfo.IsMarkGraphic;
        }

        private void MarkAllDeviceAlertHappenGraphic_Event(object obj)
        {
            foreach (DeviceAlertEx ainfo in AllDeviceAlerts)
            {

                if ((ainfo.GpsValid == "N") || (ainfo.GpsValid == null))
                {
                    continue;
                }
                if (ainfo.IsMarkGraphic == false) //如果选择其他对象后，上一个对象没有被标识，则从地图删除
                {
                    EventAggregator.Publish<AlertAddRemoveCurrentPosition>(new AlertAddRemoveCurrentPosition() { Direction = ainfo.Direction, AlertTime = ainfo.AlertTime, GpsTime = ainfo.GpsTime, Speed = ainfo.Speed, Valid = ainfo.GpsValid, Latitude = ainfo.Latitude, Longitude = ainfo.Longitude, Op = 1, VehicleId = ainfo.VehicleId });
                    ainfo.IsMarkGraphic = !ainfo.IsMarkGraphic;

                }
                EventAggregator.Publish<FullMapArgs>(new FullMapArgs() { Op = 1 });

            }
           

        }
        private DeviceAlertServiceClient InitialDeviceAlertClient()
        {
            DeviceAlertServiceClient vehicleAlertClient = ServiceClientFactory.Create<DeviceAlertServiceClient>();
            vehicleAlertClient.GetDeviceAlertEx1Completed += vehicleAlertClient_GetDeviceAlertEx1Completed;
           

            return vehicleAlertClient;
        }

        private void DeviceAlertHistoriyTrace_Event(object obj)
        {
            if (obj == null) return;
            DeviceAlertEx ainfo = obj as DeviceAlertEx;
            EventAggregator.Publish<DisplayHistoricalRoute>(new DisplayHistoricalRoute()
            {
                VechileId = ainfo.VehicleId,
                StartTime = ainfo.GpsTime == null ? DateTime.Now.AddHours(-1) : ainfo.GpsTime.Value.AddHours(-1),
                EndTime = ainfo.GpsTime == null ? DateTime.Now : ainfo.GpsTime.Value.AddHours(1),
            });

        }

        public ICommand DeviceAlertHistroyTraceCommand { get; private set; }

        public int DeviceAlertCount { get; set; }

        void vehicleAlertClient_GetDeviceAlertEx1Completed(object sender, GetDeviceAlertEx1CompletedEventArgs e)
        {
            try
            {
                if (e.Error == null)
                {
                    AllDeviceAlerts = e.Result.Result;
                    DeviceAlertCount = e.Result.TotalRecord;
                    RaisePropertyChanged(() => DeviceAlertCount);
                   

                    foreach (var item in AllDeviceAlerts)
                    {
                        if (item.AlertTime.HasValue)
                        {
                            item.AlertTime = item.AlertTime.Value.ToLocalTime();
                        }
                        item.VehicleInfo = ApplicationContext.Instance.BufferManager.VehicleOrganizationManage.VehicleList.FirstOrDefault(t => t.VehicleId == item.VehicleId);                        
                        item.OrganizationName = item.VehicleInfo.OrganizationName;
                        item.OrganizationId = item.VehicleInfo.OrganizationID;
                    }
                    if (AllDeviceAlerts != null && AllDeviceAlerts.Count > 0)
                        SelectedDeviceAlert = AllDeviceAlerts[0];

                    if (DeviceAlertPageIndex == 1)
                    {
                        List<int> pageList = new List<int>(e.Result.TotalRecord);
                        for (int i = 0; i < e.Result.TotalRecord; i++)
                        {
                            pageList.Add(i);
                        }
                        DeviceAlertsDataPager = new PagedCollectionView(pageList);
                        DeviceAlertsDataPager.PageChanged += DeviceAlertsDataPager_PageChanged;
                    }
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("vehicleAlertClient_GetAllBusinessAlertCompleted", ex);
            }
            finally
            {
                DeviceAlertServiceClient client = sender as DeviceAlertServiceClient;
                CloseAlertClient(client);
            }

        }
        public ICommand MarkDeviceAlertHappenGraphicCommand { get; private set; }

        private void CloseAlertClient(DeviceAlertServiceClient client)
        {
            client.CloseAsync();
            client = null;
        }

        void DeviceAlertsDataPager_PageChanged(object sender, EventArgs e)
        {
            DeviceAlertPageIndex = ((PagedCollectionView)sender).PageIndex + 1;
            GetDeviceAlertByPage(DeviceAlertPageIndex);
        }

        private void DeviceAlertSearch_Event(object obj)
        {
            GetDeviceAlertByPage(1);
        }

        public ICommand DeviceAlertSearchCommand
        {
            get { return _DeviceAlertSearchCommand; }
            set { _DeviceAlertSearchCommand = value; }
        }

        public ICommand DeviceAlertExportCommand
        {
            get { return _DeviceAlertExportCommand; }
            set { _DeviceAlertExportCommand = value; }
        }


        ICommand _DeviceAlertLocateCommand;

        public ICommand DeviceAlertLocateCommand
        {
            get { return _DeviceAlertLocateCommand; }
            set { _DeviceAlertLocateCommand = value; }
        }


        //alert type list
       // public ObservableCollection<VehicleAlertType> VehicleAlertTypeList { get; set; }

       // public VehicleAlertType SelectedDeviceAlertType { get; set; }

        private Nullable<DateTime> _DeviceAlertStartTime ;

        private Nullable<DateTime> _DeviceAlertEndTime ;

        public Nullable<DateTime> DeviceAlertStartTime
        {
            get { return _DeviceAlertStartTime; }
            set
            {
                _DeviceAlertStartTime = value;
                RaisePropertyChanged(() => this.DeviceAlertStartTime);
            }
        }
        /// <summary>
        /// End Time
        /// </summary>
        public Nullable<DateTime> DeviceAlertEndTime
        {
            get { return _DeviceAlertEndTime; }
            set
            {
                if (value != null)
                {
                    _DeviceAlertEndTime = value;
                    RaisePropertyChanged(() => this.DeviceAlertEndTime);
                }
                else
                {
                    _DeviceAlertEndTime = null;
                }
            }
        }

        public int DeviceAlertPageIndex
        {
            get;
            set;
        }
        public int DeviceAlertItemCount
        {
            get;
            set;
        }
        private PagedCollectionView _DeviceAlertsPageSizeValueDatePager;
        public PagedCollectionView DeviceAlertsDataPager
        {
            get
            {
                return this._DeviceAlertsPageSizeValueDatePager;
            }
            set
            {
                this._DeviceAlertsPageSizeValueDatePager = value;
                RaisePropertyChanged(() => this.DeviceAlertsDataPager);
            }
        }
        private int _DeviceAlertsPageSizeValue = 12;
        public int DeviceAlertsPageSizeValue
        {
            get
            {
                return this._DeviceAlertsPageSizeValue;
            }
            set
            {
                this._DeviceAlertsPageSizeValue = value;
                RaisePropertyChanged(() => this.DeviceAlertsPageSizeValue);
            }
        }



        private ObservableCollection<DeviceAlertEx> _allDeviceAlerts;

        public ObservableCollection<DeviceAlertEx> AllDeviceAlerts
        {
            get { return this._allDeviceAlerts; }
            set
            {
                _allDeviceAlerts = value;
                RaisePropertyChanged(() => this.AllDeviceAlerts);
            }
        }

        private string _DeviceAlertCarNumber;
        /// <summary>
        /// Processed license plate number
        /// </summary>
        public string DeviceAlertCarNumber
        {
            get { return _DeviceAlertCarNumber; }
            set
            {
                _DeviceAlertCarNumber = value.Trim();
            }
        }

        private DeviceAlertEx _selectedDeviceAlert;

        public DeviceAlertEx SelectedDeviceAlert
        {
            get
            {
                return _selectedDeviceAlert;
            }
            set
            {
                if (_selectedDeviceAlert != null)
                {
                    if (_selectedDeviceAlert.IsMarkGraphic == false) //如果选择其他对象后，上一个对象没有被标识，则从地图删除
                    {
                        EventAggregator.Publish<AlertAddRemoveCurrentPosition>(new AlertAddRemoveCurrentPosition() { Direction = _selectedDeviceAlert.Direction, AlertTime = _selectedDeviceAlert.AlertTime, GpsTime = _selectedDeviceAlert.GpsTime, Speed = _selectedDeviceAlert.Speed, Valid = _selectedDeviceAlert.GpsValid, Latitude = _selectedDeviceAlert.Latitude, Longitude = _selectedDeviceAlert.Longitude, Op = 0, VehicleId = _selectedDeviceAlert.VehicleId });
                    }
                }
                if (_selectedDeviceAlert != value)
                {
                    _selectedDeviceAlert = value;
                    OnDeviceAlertSelectChange(_selectedDeviceAlert);
                }


                if (_selectedDeviceAlert != null)
                {
                    if (GPSState.Valid(_selectedDeviceAlert.GpsValid))
                    {
                        EventAggregator.Publish<AlertAddRemoveCurrentPosition>(new AlertAddRemoveCurrentPosition() { Direction = _selectedDeviceAlert.Direction, AlertTime = _selectedDeviceAlert.AlertTime, GpsTime = _selectedDeviceAlert.GpsTime, Speed = _selectedDeviceAlert.Speed, Valid = _selectedDeviceAlert.GpsValid, Latitude = _selectedDeviceAlert.Latitude, Longitude = _selectedDeviceAlert.Longitude, Op = 1, VehicleId = _selectedDeviceAlert.VehicleId });
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

        Visibility _DevicealertVisiblity = Visibility.Collapsed;

        public Visibility DeviceAlertVisiblity
        {
            get { return _DevicealertVisiblity; }
            set
            {
                _DevicealertVisiblity = value;
                RaisePropertyChanged(() => this.DeviceAlertVisiblity);
            }
        }

        private void GetDeviceAlertByPage(int pageIndex)
        {
            try
            {
                if (!string.IsNullOrEmpty(DeviceAlertCarNumber))
                {
                    DeviceAlertCarNumber = DeviceAlertCarNumber.Trim();
                }
                DateTime? begintime = DeviceAlertStartTime;
                DateTime? endtime = DeviceAlertEndTime;
                if (DeviceAlertStartTime != null && DeviceAlertEndTime != null)
                {
                    if (DeviceAlertStartTime < DeviceAlertEndTime)
                    {
                        if (DeviceAlertEndTime > DateTime.Now.Date.AddDays(1))
                        {
                            //MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("Rpt_EndTimeError"));
                            MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString("Tip"),
                                ApplicationContext.Instance.StringResourceReader.GetString("Rpt_EndTimeError"));
                            return;
                        }
                        else
                        {
                            //begintime = DeviceAlertStartTime.Value.ToUniversalTime().Date;
                            //endtime = DeviceAlertEndTime.Value.ToUniversalTime().Date.AddDays(1);
                            begintime = DeviceAlertStartTime.Value.ToUniversalTime().AddSeconds(1);
                            endtime = DeviceAlertEndTime.Value.ToUniversalTime().AddSeconds(1);
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

                DeviceAlertPageIndex = pageIndex;
               
                DeviceAlertServiceClient vehicleAlertClient = InitialDeviceAlertClient();
                ObservableCollection<decimal?> type = new ObservableCollection<decimal?>();
                if (searchByAlertType.Code != -1)
                {
                    type.Add(searchByAlertType.Code);
                }

                vehicleAlertClient.GetDeviceAlertEx1Async(DeviceAlertCarNumber, "", type, begintime, endtime, new PagingInfo { PageIndex = pageIndex, PageSize = DeviceAlertsPageSizeValue });
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }
    }
    public enum DeviceAlertTypes
    {
        MAINTAIN_Full_N = -1,
        GNSSModelError = 0,
        GNSSNoAntenna = 1,
        GNSSCircuit = 2,
        PowerSourceNoVoltage = 3,
        PowerSourceNoPower = 4,
        LEDError = 5,
        TTSError = 6,
        VidiconError = 7,

    }
}
