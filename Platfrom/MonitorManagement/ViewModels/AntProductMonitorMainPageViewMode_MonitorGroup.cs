using Gsafety.Ant.Monitor.Models;
using Gsafety.Common.Controls;
using Gsafety.PTMS.Bases.Models;
using Gsafety.PTMS.ServiceReference.MessageServiceExt;
using Gsafety.PTMS.ServiceReference.RunMonitorGroupService;
using Gsafety.PTMS.Share;
using Jounce.Framework;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Threading;
namespace Gsafety.Ant.Monitor.ViewModels
{
    /// <summary>
    /// 该类声明列表数据结构 和 列表数据
    /// </summary>
    /// 
    public partial class AntProductMonitorMainPageViewModel
    {
        /// <summary>
        /// 过滤监控组
        /// </summary>
        public ICommand FilterMonitorGroupCommand { get; private set; }
        public ICommand DeleteMonitorGroupVehicleCommand { get; private set; }

        private string AllGroup = ApplicationContext.Instance.StringResourceReader.GetString("AllGroup");

        /// <summary>
        /// 从监控组中删除车辆
        /// </summary>
        /// <param name="obj"></param>
        private void DeleteMonitorGroupVehicle_Event(object obj)
        {

            var result = (SelfMessageBox)MessageBoxHelper.ShowDialog(LProxy.Caption, ApplicationContext.Instance.StringResourceReader.GetString("MONITOR_GroupVehicleDeleConditionQuesitionMsg"), MessageDialogButton.OkAndCancel);
            result.Closed += (s, e) =>
            {
                if (result.DialogResult == true)
                {
                    try
                    {
                        RunMonitorGroupServiceClient client = ServiceClientFactory.Create<RunMonitorGroupServiceClient>();
                        client.DeleteRunMonitorGroupVehicleByIDCompleted += client_DeleteRunMonitorGroupVehicleByIDCompleted;
                        client.DeleteRunMonitorGroupVehicleByIDAsync(obj.ToString(), ApplicationContext.Instance.AuthenticationInfo.UserID);
                    }
                    catch (Exception ex)
                    {
                        ApplicationContext.Instance.Logger.LogException("DeleteMonitorGroup", ex);
                    }
                }
            };
        }

        /// <summary>
        /// 移除车辆从监控组
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void client_DeleteRunMonitorGroupVehicleByIDCompleted(object sender, DeleteRunMonitorGroupVehicleByIDCompletedEventArgs e)
        {
            try
            {
                if (e.Error != null)
                {
                    ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), e.Error);
                    MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString(LProxy.ServerError));
                }
                else
                {
                    if (e.Result.IsSuccess == false)
                    {
                        if (!string.IsNullOrEmpty(e.Result.ErrorMsg))
                        {
                            ApplicationContext.Instance.Logger.LogError(MethodBase.GetCurrentMethod().ToString(),
                                e.Result.ErrorMsg);
                            MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString(e.Result.ErrorMsg));
                        }
                        else
                        {
                            MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString(LProxy.OperatorServiceError));
                            ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(),
                                e.Result.ExceptionMessage);
                        }
                    }
                    else
                    {
                        //从内存中删除
                        foreach (var item in ApplicationContext.Instance.BufferManager.MonitorGroupManager.MoniterGroupManagerOC)
                        {
                            if (item.GroupVehicle != null && item.GroupName == CurSelectedTableData.Group)
                            {
                                foreach (var v in item.GroupVehicle)
                                {
                                    if (v.VehicleId == CurSelectedTableData.VehicleId)
                                    {
                                        item.GroupVehicle.Remove(v);
                                        break;
                                    }
                                }
                            }
                        }
                        MonitorGPS(CurSelectedTableData.VehicleId, "", false, false, false);
                        _TableData.Remove(CurSelectedTableData);
                    }
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
            finally
            {
                CloseMonitorGroupServiceClient(sender);
            }
        }

        private static void CloseMonitorGroupServiceClient(object sender)
        {
            RunMonitorGroupServiceClient client = sender as RunMonitorGroupServiceClient;
            if (client != null)
            {
                client.CloseAsync();
                client = null;
            }
        }

        private List<RunMonitorGroup> _monitorGroupList;
        public List<RunMonitorGroup> MonitorGroupList
        {
            get { return _monitorGroupList; }
            set
            {
                _monitorGroupList = value;
                RaisePropertyChanged(() => MonitorGroupList);
            }
        }

        private RunMonitorGroup LastSelectedGroupName;
        private RunMonitorGroup groupName;
        public RunMonitorGroup GroupName
        {
            get { return groupName; }
            set
            {
                if (value == null || groupName == value) return;
                groupName = value;

                LastSelectedGroupName = new RunMonitorGroup();
                LastSelectedGroupName.GroupIndex = value.GroupIndex;
                LastSelectedGroupName.GroupName = value.GroupName;


                RaisePropertyChanged(() => GroupName);
                try
                {
                    if (GroupName.GroupIndex > -1)
                    {
                        _MonitorPageCollectionView.Filter = new Predicate<object>(FilterGroup);

                        foreach (var item in ApplicationContext.Instance.BufferManager.MonitorGroupManager.MoniterGroupManagerOC)
                        {
                            if (item.GroupVehicle != null && item.GroupName != value.GroupName)
                            {
                                foreach (var car in item.GroupVehicle)
                                {
                                    MonitorGPS(car.VehicleId, "", false, false, false);
                                }
                               
                            }
                        }
                        
                    }
                    else
                    {
                        _MonitorPageCollectionView.Filter = null;
                        AddallDataElement();
                    }

                    MonitorPageCollectionView.Refresh();
                }
                catch (Exception ex)
                {
                    ApplicationContext.Instance.Logger.LogException("GroupName", ex);
                }
            }
        }

        private bool FilterGroup(object obj)
        {
            TableDataElement info = obj as TableDataElement;
            if (info.Group == groupName.GroupName)
            {
                return true;
            }
            return false;
        }

        private bool _isOperatorButtonEnable;
        /// <summary>
        /// 操作按钮是否可用
        /// </summary>
        public bool IsEnableButtonEnable
        {
            get
            {
                return this._isOperatorButtonEnable;
            }
            set
            {
                this._isOperatorButtonEnable = value;
                RaisePropertyChanged(() => this.IsEnableButtonEnable);
            }
        }


        /// <summary>
        /// 监控列表中绑定的数据
        /// </summary>
        private ObservableCollection<TableDataElement> _TableData = new ObservableCollection<TableDataElement>();
        private PagedCollectionView _MonitorPageCollectionView;
        public PagedCollectionView MonitorPageCollectionView
        {
            get
            {
                return _MonitorPageCollectionView;
            }
            set
            {
                _MonitorPageCollectionView = value;
                RaisePropertyChanged(() => MonitorPageCollectionView);
            }
        }

        /// <summary>
        /// 当前选中的车辆
        /// </summary>
        private TableDataElement curSelectedTableData;
        public TableDataElement CurSelectedTableData
        {
            get
            {
                return curSelectedTableData;
            }
            set
            {
                if (curSelectedTableData != value)
                {
                    curSelectedTableData = value;
                    if (value == null)
                    {
                        return;
                    }
                    LocateCar(value.VehicleId);
                    Gsafety.PTMS.Bases.Models.Vehicle ve = ApplicationContext.Instance.BufferManager.VehicleOrganizationManage.GetVehicle(value.VehicleId);
                    EventAggregator.Publish<Gsafety.PTMS.Bases.Models.Vehicle>(ve);
                    EventAggregator.Publish<ManualAlarmHandleDisplayArgs>(new ManualAlarmHandleDisplayArgs() { Show = null, VehicleID = value.VehicleId });
                    RaisePropertyChanged(() => CurSelectedTableData);
                }
            }
        }

        /// <summary>
        /// 过滤监控组
        /// </summary>
        /// <param name="obj"></param>
        private void FilterMonitorGroup_Event(object obj)
        {
            IsDropDownOpen = true;

        }

        /// <summary>
        /// 过滤下拉框出现与否
        /// </summary>
        private bool isDropDownOpen;
        public bool IsDropDownOpen
        {
            get { return isDropDownOpen; }
            set
            {
                isDropDownOpen = value;
                RaisePropertyChanged(() => IsDropDownOpen);
            }
        }


        /// <summary>
        /// 初始化监控组下拉列表
        /// </summary>
        private void InitMonitorGroupDropDownList()
        {
            try
            {
                MonitorGroupList = new List<RunMonitorGroup>();
                RunMonitorGroup mg = new RunMonitorGroup();
                mg.GroupIndex = -1;
                mg.GroupName = AllGroup;
                MonitorGroupList.Add(mg);
                MonitorGroupList.AddRange(ApplicationContext.Instance.BufferManager.MonitorGroupManager.MoniterGroupManagerOC);
                if (LastSelectedGroupName != null)
                    GroupName = LastSelectedGroupName;
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }

        private DispatcherTimer _queryTimer;

        /// <summary>
        /// 初始化监控列表中的数据
        /// </summary>
        /// <param name="publishedEvent"></param>
        public void InitMonitorList()
        {
            try
            {
                _TableData = new ObservableCollection<TableDataElement>();
                _MonitorPageCollectionView = new PagedCollectionView(_TableData);
                _MonitorPageCollectionView.SortDescriptions.Add(new SortDescription() { Direction = ListSortDirection.Ascending, PropertyName = "GroupIndex" });



                //AddallDataElement();

                //按分组初始化
                foreach (var item in ApplicationContext.Instance.BufferManager.MonitorGroupManager.MoniterGroupManagerOC)
                {
                    if (item.GroupVehicle != null)
                    {
                        foreach (var car in item.GroupVehicle)
                        {
                            AddDataElement(_TableData, car.VehicleId, item.GroupName);
                        }
                    }
                }

           
                if (this._MonitorPageCollectionView.Count > 0)
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

            while (EventAggregator != null)
            {
                _queryTimer.Stop();
                AddallDataElement();
                break;
            }
        }

        private bool FindInMoniterGroupManager(string vehicleId)
        {
            foreach (var item in ApplicationContext.Instance.BufferManager.MonitorGroupManager.MoniterGroupManagerOC)
            {
                if (item.GroupVehicle != null)
                {
                    foreach (var car in item.GroupVehicle)
                    {
                        if (car.VehicleId == vehicleId) return true;
                    }
                }
            }
            return false;
        }



        private void AddallDataElement()
        {
            try
            {
                foreach (var ve in ApplicationContext.Instance.BufferManager.VehicleOrganizationManage.VehicleList)
                {

                    LocateCar(ve.VehicleId);


                    if (ve != null)
                    {
                        TableDataElement td = new TableDataElement();
                        td.VehicleId = ve.VehicleId;
                        td.UniqueId = ve.UniqueId;

                        td.Orgnization = GetOrganizationName(td.VehicleId);
                        td.VehicleInfo = ve;

                        td.HasAlarm = ApplicationContext.Instance.BufferManager.AlarmManager.HasAlarm(td.VehicleId);
                        td.HasAlert = ApplicationContext.Instance.BufferManager.VehicleAlertManager.HasAlert(td.VehicleId);

                        if (_FirstInitGISDisplayFromMonitorList)
                        {
                            MonitorGPS(td.VehicleId, td.Orgnization, true, td.HasAlarm, td.HasAlert);
                        }                       

                    }
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }

        private void AddDataElement(ObservableCollection<TableDataElement> TeList, string vehicleId, string groupname)
        {
            try
            {
                Gsafety.PTMS.Bases.Models.Vehicle ve = ApplicationContext.Instance.BufferManager.VehicleOrganizationManage.GetVehicle(vehicleId);
                if (ve != null)
                {
                    TableDataElement td = new TableDataElement();
                    td.VehicleId = ve.VehicleId;
                    td.UniqueId = ve.UniqueId;

                    td.Orgnization = GetOrganizationName(td.VehicleId);
                    td.VehicleInfo = ve;
                    td.Group = groupname;
                    td.HasAlarm = ApplicationContext.Instance.BufferManager.AlarmManager.HasAlarm(td.VehicleId);
                    td.HasAlert = ApplicationContext.Instance.BufferManager.VehicleAlertManager.HasAlert(td.VehicleId);
                    TeList.Add(td);
                    if (_FirstInitGISDisplayFromMonitorList)
                    {
                        MonitorGPS(td.VehicleId, td.Orgnization, true, td.HasAlarm, td.HasAlert);
                    }
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }

        private void UpdateTableData(string vehicleId, string groupname, List<TableDataElement> table)
        {
            try
            {
                TableDataElement te = table.FirstOrDefault(x => x.VehicleId == vehicleId);
                if (te == null)
                {
                    AddDataElement(_TableData, vehicleId, groupname);
                }
                else
                {
                    te.Group = groupname;
                    _TableData.Add(te);
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }

        public void RefreshMonitorGroup()
        {
            try
            {
                //InitMonitorList();
                InitMonitorGroupDropDownList();
                //依据内存中的数据更新TableData;
                //哎，打个补丁吧。

                for (int i = _TableData.Count - 1; i >= 0; i--)
                {
                    if (!FindInMoniterGroupManager(_TableData[i].VehicleId))
                    {
                        _TableData.RemoveAt(i);
                    }
                }

                List<TableDataElement> temp = _TableData.ToList();
                _TableData.Clear();

                foreach (var item in ApplicationContext.Instance.BufferManager.MonitorGroupManager.MoniterGroupManagerOC)
                {
                    if (item.GroupVehicle != null)
                    {
                        foreach (var car in item.GroupVehicle)
                        {
                            UpdateTableData(car.VehicleId, item.GroupName, temp);
                        }
                    }
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
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }

        public void HandleEvent(AlarmGisArgs publishedEvent)
        {
            foreach (TableDataElement te in _TableData)
            {
                if (te.VehicleId == publishedEvent.VehicleID)
                {
                    te.HasAlarm = (publishedEvent.Alarm > 0);
                    break;
                }
            }
        }


        public void HandleEvent(AlertGisArgs publishedEvent)
        {
            foreach (TableDataElement te in _TableData)
            {
                if (te.VehicleId == publishedEvent.VehicleID)
                {
                    te.HasAlert = (publishedEvent.Alert > 0);
                    break;
                }
            }
        }

        public class TableDataElement : INotifyPropertyChanged
        {
            private string _Group;
            public string Group
            {
                get
                {
                    return _Group;
                }
                set
                {
                    _Group = value;
                    RaisePropertyChanged("Group");
                }
            }

            private bool _hasAlarm;
            public bool HasAlarm
            {
                get
                {
                    return _hasAlarm;
                }
                set
                {
                    _hasAlarm = value;
                    RaisePropertyChanged("HasAlarm");
                }
            }

            public bool HasAlert { get; set; }

            public string Orgnization { get; set; }

            public string VehicleId { get; set; }

            public Gsafety.PTMS.Bases.Models.Vehicle VehicleInfo { get; set; }

            public string UniqueId { get; set; }

            public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

            protected void RaisePropertyChanged(string propertyName)
            {
                System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
                if ((propertyChanged != null))
                {
                    propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
                }
            }
        }
    }

}
