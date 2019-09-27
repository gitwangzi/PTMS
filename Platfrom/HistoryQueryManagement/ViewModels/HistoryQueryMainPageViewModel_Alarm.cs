using GisManagement.Models;
using Gsafety.Common.CommMessage;
using Gsafety.Common.Controls;
using Gsafety.PTMS.ServiceReference.VehicleAlarmService;
using Gsafety.PTMS.Share;
using Gsafety.PTMS.Spreadsheet;
using Jounce.Framework.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace HistoryQueryManagement.ViewModels
{
    public partial class HistoryQueryMainPageViewModel
    {
        ICommand _AlarmSearchCommand;

        public ICommand AlarmSearchCommand
        {
            get { return _AlarmSearchCommand; }
            set { _AlarmSearchCommand = value; }
        }

        ICommand _AlarmExportCommand;

        public ICommand AlarmExportCommand
        {
            get { return _AlarmExportCommand; }
            set { _AlarmExportCommand = value; }
        }

        private Nullable<DateTime> _AlarmStartTime;

        private Nullable<DateTime> _AlarmEndTime;

        public Nullable<DateTime> AlarmStartTime
        {
            get { return _AlarmStartTime; }
            set
            {
                _AlarmStartTime = value;
            }
        }
        /// <summary>
        /// End Time
        /// </summary>
        public Nullable<DateTime> AlarmEndTime
        {
            get { return _AlarmEndTime; }
            set
            {
                if (value != null)
                {
                    _AlarmEndTime = new DateTime(value.Value.Year, value.Value.Month, value.Value.Day, 23, 59, 59);
                }
                else
                {
                    _AlarmEndTime = null;
                }
            }
        }

        public int AlarmsPageIndex
        {
            get;
            set;
        }
        public int AlarmsItemCount
        {
            get;
            set;
        }
        private PagedCollectionView _AlarmsPageSizeValueDatePager;
        public PagedCollectionView AlarmsDataPager
        {
            get
            {
                return this._AlarmsPageSizeValueDatePager;
            }
            set
            {
                this._AlarmsPageSizeValueDatePager = value;
                RaisePropertyChanged(() => this.AlarmsDataPager);
            }
        }
        private int _AlarmsPageSizeValue = 30;
        public int AlarmsPageSizeValue
        {
            get
            {
                return this._AlarmsPageSizeValue;
            }
            set
            {
                this._AlarmsPageSizeValue = value;
                RaisePropertyChanged(() => this.AlarmsPageSizeValue);
            }
        }

        private ObservableCollection<AlarmInfoEx> _allAlarms;

        public ObservableCollection<AlarmInfoEx> AllAlarms
        {
            get { return this._allAlarms; }
            set
            {
                _allAlarms = value;
                RaisePropertyChanged(() => this.AllAlarms);
            }
        }

        private string _AlarmCarNumber;
        /// <summary>
        /// Processed license plate number
        /// </summary>
        public string AlarmCarNumber
        {
            get { return _AlarmCarNumber; }
            set
            {
                _AlarmCarNumber = value.Trim();
            }
        }

        private AlarmInfoEx _selectedAlarmInfo;

        public AlarmInfoEx SelectedAlarmInfo
        {
            get
            {

                return _selectedAlarmInfo;
            }
            set
            {

                if (_selectedAlarmInfo != null)
                {
                    if (_selectedAlarmInfo.IsMarkGraphic == false) //如果选择其他对象后，上一个对象没有被标识，则从地图删除
                    {
                        EventAggregator.Publish<AlarmLocationAddRemoveArgs>(new AlarmLocationAddRemoveArgs() { MdvrCoreId = _selectedAlarmInfo.VehicleId, Direction = _selectedAlarmInfo.Direction, AlarmTime = _selectedAlarmInfo.AlarmTime, GpsTime = _selectedAlarmInfo.GpsTime, Speed = _selectedAlarmInfo.Speed, GpsValid = _selectedAlarmInfo.GpsValid, Latitude = _selectedAlarmInfo.Latitude, Longitude = _selectedAlarmInfo.Longitude, Op = 0, VehicleId = _selectedAlarmInfo.VehicleId });
                    }
                }

                if (_selectedAlarmInfo != value)
                {
                    _selectedAlarmInfo = value;

                    if (_selectedAlarmInfo != null)
                    {
                        OnAlarmSelectionChange(_selectedAlarmInfo);
                    }
                }

                if (_selectedAlarmInfo != null)
                {
                    if (GPSState.Valid(_selectedAlarmInfo.GpsValid))
                    {
                        EventAggregator.Publish<AlarmLocationAddRemoveArgs>(new AlarmLocationAddRemoveArgs() { MdvrCoreId = _selectedAlarmInfo.VehicleId, Direction = _selectedAlarmInfo.Direction, AlarmTime = _selectedAlarmInfo.AlarmTime, GpsTime = _selectedAlarmInfo.GpsTime, Speed = _selectedAlarmInfo.Speed, GpsValid = _selectedAlarmInfo.GpsValid, Latitude = _selectedAlarmInfo.Latitude, Longitude = _selectedAlarmInfo.Longitude, Op = 1, VehicleId = _selectedAlarmInfo.VehicleId });
                    }
                    else
                    {
                        if (selectedheader == ApplicationContext.Instance.StringResourceReader.GetString("AlarmHistory"))
                        {
                            MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString("GIS_UNValidXY"));
                        }
                    }
                }

                RaisePropertyChanged("SelectedAlarmInfo");
            }
        }

        Visibility _alarmVisiblity = Visibility.Collapsed;

        public Visibility AlarmVisiblity
        {
            get { return _alarmVisiblity; }
            set
            {
                _alarmVisiblity = value;
                RaisePropertyChanged(() => this.AlarmVisiblity);
            }
        }


        private void AlarmHistoriyTrace_Event(object obj)
        {
            if (obj == null) return;
            AlarmInfoEx ainfo = obj as AlarmInfoEx;
            EventAggregator.Publish<DisplayHistoricalRoute>(new DisplayHistoricalRoute()
            {
                VechileId = ainfo.VehicleId,
                StartTime = ainfo.GpsTime == null ? DateTime.Now.AddHours(-1) : ainfo.GpsTime.Value.AddHours(-1),
                EndTime = ainfo.GpsTime == null ? DateTime.Now : ainfo.GpsTime.Value.AddHours(1),
            });

        }

        public ICommand AlarmHistroyTraceCommand { get; private set; }


        private void InitalAlarm()
        {
            try
            {
                AlarmSearchCommand = new ActionCommand<object>(obj => AlarmSearch_Event(obj));
                AlarmExportCommand = new ActionCommand<object>(obj => AlarmExport_Event());

                AllAlarms = new ObservableCollection<AlarmInfoEx>();
                AlarmsPageIndex = 1;
                ////Processed an alarm paging key
                AlarmsDataPager = new PagedCollectionView(AllAlarms);
                this.AlarmsDataPager.PageChanged += AlarmsDataPager_PageChanged;
                this.AlarmsDataPager.PageSize = AlarmsPageSizeValue;

                AlarmHistroyTraceCommand = new ActionCommand<object>((obj) => AlarmHistoriyTrace_Event(obj));
                MarkAlarmHappenGraphicCommand = new ActionCommand<object>((obj) => MarkAlarmHappenGraphic_Event_Event(obj));
                VehicleAlarmServiceClient vehicleAlarmServiceClient = InitialAlarmClient();
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }

        private void MarkAlarmHappenGraphic_Event_Event(object obj)
        {
            if (obj == null) return;
            AlarmInfoEx ainfo = obj as AlarmInfoEx;
            if ((ainfo.GpsValid == "N") || (ainfo.GpsValid == null))
            {
                MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString(LProxy.Caption), ApplicationContext.Instance.StringResourceReader.GetString("GIS_GpsInValid"));
                return;
            }
            ainfo.IsMarkGraphic = !ainfo.IsMarkGraphic;

        }

        public ICommand MarkAlarmHappenGraphicCommand { get; set; }
        private VehicleAlarmServiceClient InitialAlarmClient()
        {
            VehicleAlarmServiceClient vehicleAlarmServiceClient = ServiceClientFactory.Create<VehicleAlarmServiceClient>();
            vehicleAlarmServiceClient.GetAllAlarmsCompleted += vehicleAlarmServiceClient_GetAllAlarmsCompleted;
            vehicleAlarmServiceClient.GetTransferDisposeByAlarmIDCompleted += vehicleAlarmServiceClient_GetTransferDisposeByAlarmIDCompleted;
            vehicleAlarmServiceClient.GetApealDisposeByAlarmIDCompleted += vehicleAlarmServiceClient_GetApealDisposeByAlarmIDCompleted;

            return vehicleAlarmServiceClient;
        }

        public int AlarmCount { get; set; }

        void vehicleAlarmServiceClient_GetAllAlarmsCompleted(object sender, GetAllAlarmsCompletedEventArgs e)
        {
            try
            {
                if (e.Error == null)
                {
                    AllAlarms = e.Result.Result;
                    AlarmCount = e.Result.TotalRecord;
                    RaisePropertyChanged(() => AlarmCount);
                    foreach (var item in AllAlarms)
                    {
                        item.AlarmTime = item.AlarmTime.Value.ToLocalTime();
                        item.VehicleInfo = ApplicationContext.Instance.BufferManager.VehicleOrganizationManage.VehicleList.FirstOrDefault(t => t.VehicleId == item.VehicleId);
                    }
                    if (AllAlarms != null && AllAlarms.Count > 0)
                        SelectedAlarmInfo = AllAlarms[0];
                    if (AlarmsPageIndex == 1)
                    {
                        List<int> pageList = new List<int>(e.Result.TotalRecord);
                        for (int i = 0; i < e.Result.TotalRecord; i++)
                        {
                            pageList.Add(i);
                        }
                        AlarmsDataPager = new PagedCollectionView(pageList);
                        AlarmsDataPager.PageChanged += AlarmsDataPager_PageChanged;
                    }
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("vehicleAlarmServiceClient_GetAllAlarmsCompleted", ex);
            }
            finally
            {
                VehicleAlarmServiceClient client = sender as VehicleAlarmServiceClient;
                CloseAlarmClient(client);
            }
        }

        private void CloseAlarmClient(VehicleAlarmServiceClient client)
        {
            client.CloseAsync();
            client = null;
        }

        private void AlarmsDataPager_PageChanged(object sender, EventArgs e)
        {
            AlarmsPageIndex = ((PagedCollectionView)sender).PageIndex + 1;
            GetAlarmByPage(AlarmsPageIndex);
        }

        private void AlarmSearch_Event(object obj)
        {
            GetAlarmByPage(1);
        }

        private void AlarmExport_Event()
        {
            try
            {
                SaveFileDialog dlg = new SaveFileDialog();
                dlg.Filter = "Excel Document (.xlsx)|*.xlsx";
                dlg.DefaultExt = ".xlsx";
                bool? dialogResult = dlg.ShowDialog();
                if (dialogResult == true)
                {
                    VehicleAlarmServiceClient client = ServiceClientFactory.Create<VehicleAlarmServiceClient>();

                    if (!string.IsNullOrEmpty(AlarmCarNumber))
                    {
                        AlarmCarNumber = AlarmCarNumber.Trim();
                    }
                    DateTime? begintime = AlarmStartTime;
                    DateTime? endtime = AlarmEndTime;
                    if (AlarmStartTime.HasValue && AlarmEndTime.HasValue)
                    {

                        begintime = AlarmStartTime.Value.ToUniversalTime();
                        endtime = AlarmEndTime.Value.ToUniversalTime();
                    }
                    ObservableCollection<string> organizations = new ObservableCollection<string>();
                    foreach (var item in ApplicationContext.Instance.AuthenticationInfo.Organizations)
                    {
                        organizations.Add(item.ID);
                    }

                    client.GetAllAlarmsCompleted += (s, e) =>
                    {
                        try
                        {
                            if (e.Result != null && e.Result.TotalRecord > 0)
                            {
                                foreach (var item in e.Result.Result)
                                {
                                    if(item.AlarmTime.HasValue)
                                    {
                                        item.AlarmTime = item.AlarmTime.Value.ToLocalTime();
                                    }
                                   
                                }
                                List<string> Codes = new List<string>();
                                Codes.Add("VehicleId");
                                Codes.Add("MdvrCoreId");
                               // Codes.Add("VehicleType");
                                Codes.Add("AlarmTime");
                                //Codes.Add("TransferStatus");
                                Codes.Add("Longitude");
                                Codes.Add("Latitude");
                                Codes.Add("Speed");
                                Codes.Add("Direction");


                                List<string> Names = new List<string>();
                                Names.Add(ApplicationContext.Instance.StringResourceReader.GetString("BASEINFO_VehicleNumber"));
                                Names.Add(ApplicationContext.Instance.StringResourceReader.GetString("BASEINFO_SuiteID"));
                               // Names.Add(ApplicationContext.Instance.StringResourceReader.GetString("BASEINFO_VehicleType"));
                                Names.Add(ApplicationContext.Instance.StringResourceReader.GetString("MAINPAGE_AlarmTime"));
                                //Names.Add(ApplicationContext.Instance.StringResourceReader.GetString("TransferStatus"));
                                Names.Add(ApplicationContext.Instance.StringResourceReader.GetString("ALARM_Longitude"));
                                Names.Add(ApplicationContext.Instance.StringResourceReader.GetString("Latitude"));
                                Names.Add(ApplicationContext.Instance.StringResourceReader.GetString("Speed"));
                                Names.Add(ApplicationContext.Instance.StringResourceReader.GetString("ALARM_DIR"));

                                XLSXExporter xlsx = new XLSXExporter();
                                xlsx.Export(e.Result.Result.ToList(), dlg.OpenFile(), Codes, Names);
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


                    if (AllAlarms.Count > 10000)
                    {                       
                        client.GetAllAlarmsAsync(AlarmCarNumber, begintime, endtime, new PagingInfo { PageIndex = 1, PageSize = 10000 }, ApplicationContext.Instance.AuthenticationInfo.ClientID, organizations);
                    }
                    else
                    {                       
                        client.GetAllAlarmsAsync(AlarmCarNumber, begintime, endtime, new PagingInfo { PageIndex = AlarmsDataPager.PageIndex + 1, PageSize = AlarmsPageSizeValue }, ApplicationContext.Instance.AuthenticationInfo.ClientID, organizations);
                    }
                }
            }
            catch (Exception ex)
            {
                
            }
        }


        private void GetAlarmByPage(int pageIndex)
        {
            try
            {
                if (!string.IsNullOrEmpty(AlarmCarNumber))
                {
                    AlarmCarNumber = AlarmCarNumber.Trim();
                }
                DateTime? begintime = AlarmStartTime;
                DateTime? endtime = AlarmEndTime;
                if (AlarmStartTime.HasValue && AlarmEndTime.HasValue)
                {
                    if (AlarmStartTime.Value < AlarmEndTime.Value)
                    {
                        if (AlarmEndTime.Value > DateTime.Now.Date.AddDays(1))
                        {
                            MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString(LProxy.Caption), ApplicationContext.Instance.StringResourceReader.GetString("Rpt_EndTimeError"));
                            return;
                        }
                        else
                        {
                            begintime = AlarmStartTime.Value.ToUniversalTime();
                            endtime = AlarmEndTime.Value.ToUniversalTime();
                        }
                    }
                    else
                    {
                        MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString(LProxy.Caption), ApplicationContext.Instance.StringResourceReader.GetString("VDM_TimeSettingError"));
                        return;
                    }
                }

                AlarmsPageIndex = pageIndex;
                ObservableCollection<string> organizations = new ObservableCollection<string>();
                foreach (var item in ApplicationContext.Instance.AuthenticationInfo.Organizations)
                {
                    organizations.Add(item.ID);
                }
                VehicleAlarmServiceClient vehicleAlarmServiceClient = InitialAlarmClient();
                vehicleAlarmServiceClient.GetAllAlarmsAsync(AlarmCarNumber, begintime, endtime, new PagingInfo { PageIndex = pageIndex, PageSize = AlarmsPageSizeValue }, ApplicationContext.Instance.AuthenticationInfo.ClientID, organizations);

            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }
    }
}
