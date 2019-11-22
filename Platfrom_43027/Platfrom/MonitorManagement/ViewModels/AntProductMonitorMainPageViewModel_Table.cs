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
using System.Windows.Data;
using System.Windows.Input;
namespace Gsafety.Ant.Monitor.ViewModels
{
    /// <summary>
    /// 该类声明列表数据结构 和 列表数据
    /// </summary>
    /// 
    public class RefreshMonitorListData
    {
    }

    public partial class AntProductMonitorMainPageViewModel
    {

        private string AllGroup = ApplicationContext.Instance.StringResourceReader.GetString("AllGroup");

        /// <summary>
        /// 更新在线状态
        /// </summary>
        /// <param name="e"></param>
        public void HandleEvent(OnOfflineEx e)
        {
            try
            {

                TableDataElement item = _TableData.Where(x => x.VehicleId == e.VehicleId).FirstOrDefault();
                if((item != null) && (item.IsOnLine != (e.IsOnline > 0)))
                {
                    item.IsOnLine = (e.IsOnline > 0);
                    JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => MonitorPageCollectionView));
                }

            }
            catch(Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("HandleEvent", ex);
            }
        }


        /// <summary>
        /// 过滤监控组
        /// </summary>
        public ICommand FilterMonitorGroupCommand { get; private set; }
        public ICommand DeleteMonitorGroupCommand { get; private set; }


        /// <summary>
        /// 从监控组中删除车辆
        /// </summary>
        /// <param name="obj"></param>
        private void DeleteMonitorGroup_Event(object obj)
        {

            var result = (SelfMessageBox)MessageBoxHelper.ShowDialog(LProxy.Caption, ApplicationContext.Instance.StringResourceReader.GetString("MONITOR_GroupVehicleDeleConditionQuesitionMsg"), MessageDialogButton.OkAndCancel);
            result.Closed += (s, e) =>
            {
                if(result.DialogResult == true)
                {
                    try
                    {
                        client = ServiceClientFactory.Create<RunMonitorGroupServiceClient>();
                        client.DeleteRunMonitorGroupVehicleByIDCompleted += client_DeleteRunMonitorGroupVehicleByIDCompleted;
                        client.DeleteRunMonitorGroupVehicleByIDAsync(obj.ToString());
                    }
                    catch(Exception ex)
                    {
                        client = ServiceClientFactory.Create<RunMonitorGroupServiceClient>();
                        ApplicationContext.Instance.Logger.LogException("DeleteMonitorGroup", ex);
                    }
                    finally
                    {
                        client.CloseAsync();
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
            if(e.Error == null)
            {

                //从内存中删除
                foreach(var item in ApplicationContext.Instance.BufferManager.MonitorGroupManager._MoniterGroupManagerOC)
                {
                    if(item.GroupVehicle != null && item.GroupName == CurSelectedTableData.Group)
                    {
                        foreach(var v in item.GroupVehicle)
                        {
                            if(v.VehicleId == CurSelectedTableData.VehicleId)
                            {
                                item.GroupVehicle.Remove(v);
                                break;
                            }
                        }
                    }
                }

                _TableData.Remove(CurSelectedTableData);
                MonitorGPS(sender.ToString(), "", false, false, false);
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
                if(value == null) return;
                groupName = value;

                LastSelectedGroupName = new RunMonitorGroup();
                LastSelectedGroupName.GroupIndex = value.GroupIndex;
                LastSelectedGroupName.GroupName = value.GroupName;


                RaisePropertyChanged(() => GroupName);
                try
                {
                    if(GroupName.GroupIndex > -1)
                    {
                        _MonitorPageCollectionView.Filter = new Predicate<object>(FilterGroup);
                    }
                    else
                    {
                        _MonitorPageCollectionView.Filter = null;
                    }
                }
                catch(Exception ex)
                {
                    ApplicationContext.Instance.Logger.LogException("GroupName", ex);
                }
            }
        }

        private bool FilterGroup(object obj)
        {
            TableDataElement info = obj as TableDataElement;
            if(info.Group == groupName.GroupName)
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
                //if(this._MonitorPageCollectionView != null && this._MonitorPageCollectionView.Count > 0)
                //{
                //    this.IsEnableButtonEnable = true;
                //    RaisePropertyChanged(() => this.IsEnableButtonEnable);
                //}
                //else
                //{
                //    this.IsEnableButtonEnable = false;
                //    RaisePropertyChanged(() => this.IsEnableButtonEnable);
                //}
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
                if(value != null) ContextMenuSelectedVehicleId = value.VehicleId;//add 20116.9.7
                curSelectedTableData = value;
                if(value != null)
                {
                    LocateCar(value.VehicleId);
                    Vehicle ve = ApplicationContext.Instance.BufferManager.VehicleOrganizationManage.GetVehicle(value.VehicleId);
                    EventAggregator.Publish<Vehicle>(ve);
                }
                RaisePropertyChanged(() => CurSelectedTableData);
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
            MonitorGroupList = new List<RunMonitorGroup>();
            RunMonitorGroup mg = new RunMonitorGroup();
            mg.GroupIndex = -1;
            mg.GroupName = AllGroup;
            MonitorGroupList.Add(mg);
            MonitorGroupList.AddRange(ApplicationContext.Instance.BufferManager.MonitorGroupManager._MoniterGroupManagerOC.ToList());
            if(LastSelectedGroupName != null) GroupName = LastSelectedGroupName;
        }


        RunMonitorGroupServiceClient client = null;

        /// <summary>
        /// 初始化监控列表中的数据
        /// </summary>
        /// <param name="publishedEvent"></param>
        public void InitMonitorList()
        {
            try
            {
                client = ServiceClientFactory.Create<RunMonitorGroupServiceClient>();
                client.GetRunMonitorGroupVehicleListCompleted += client_GetRunMonitorGroupVehicleListCompleted;
                client.GetRunMonitorGroupVehicleListAsync(0, int.MaxValue);
            }
            catch(Exception)
            {
                client = ServiceClientFactory.Create<RunMonitorGroupServiceClient>();
            }
            finally
            {
                client.CloseAsync();
            }
        }

        private bool FindInMoniterGroupManager(string vehicleId)
        {
            foreach(var item in ApplicationContext.Instance.BufferManager.MonitorGroupManager._MoniterGroupManagerOC)
            {
                if(item.GroupVehicle != null)
                {
                    foreach(var car in item.GroupVehicle)
                    {
                        if(car.VehicleId == vehicleId) return true;
                    }
                }
            }
            return false;
        }
        private void AddDataElement(ObservableCollection<TableDataElement> TeList, string vehicleId, string groupname)
        {
            Vehicle ve = ApplicationContext.Instance.BufferManager.VehicleOrganizationManage.GetVehicle(vehicleId);
            if(ve != null)
            {
                TableDataElement td = new TableDataElement();
                td.VehicleId = ve.VehicleId;
                td.UniqueId = ve.UniqueId;

                td.Orgnization = GetOrganizationName(td.VehicleId);
                td.IsOnLine = ve.IsOnLine;
                td.Group = groupname;
                td.HasAlarm = ApplicationContext.Instance.BufferManager.AlarmManager.HasAlarm(td.VehicleId);
                TeList.Add(td);
                MonitorGPS(td.VehicleId, td.Orgnization, true, td.HasAlarm, false);
            }
        }
        private void UpdateTableData(string vehicleId, string groupname)
        {
            TableDataElement te = _TableData.FirstOrDefault(x => x.VehicleId == vehicleId);
            if(te == null)
            {
                AddDataElement(_TableData, vehicleId, groupname);
            }
            else
            {
                te.Group = groupname;
            }
        }
        public void HandleEvent(RefreshMonitorListData publishedEvent)
        {
            //InitMonitorList();
            InitMonitorGroupDropDownList();
            //依据内存中的数据更新TableData;
            //哎，打个补丁吧。

            foreach(var item in ApplicationContext.Instance.BufferManager.MonitorGroupManager._MoniterGroupManagerOC)
            {
                if(item.GroupVehicle != null)
                {
                    foreach(var car in item.GroupVehicle)
                    {
                        UpdateTableData(car.VehicleId, item.GroupName);
                    }
                }
            }
            for(int i = _TableData.Count - 1; i >= 0; i--)
            {
                if(!FindInMoniterGroupManager(_TableData[i].VehicleId))
                {
                    _TableData.RemoveAt(i);
                }
            }
            if(this._MonitorPageCollectionView != null && this._MonitorPageCollectionView.Count > 0)
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


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void client_GetRunMonitorGroupVehicleListCompleted(object sender, GetRunMonitorGroupVehicleListCompletedEventArgs e)
        {
            try
            {
                if(e.Result.Result.Count > 0)
                {
                    _TableData = new ObservableCollection<TableDataElement>();

                    foreach(var item in e.Result.Result)
                    {
                        string groupname = ApplicationContext.Instance.BufferManager.MonitorGroupManager._MoniterGroupManagerOC
                          .FirstOrDefault(x => x.ID == item.GroupId) == null ? "" :
                       ApplicationContext.Instance.BufferManager.MonitorGroupManager._MoniterGroupManagerOC
                      .FirstOrDefault(x => x.ID == item.GroupId).GroupName;

                        //AddDataElement(_TableData, item.VehicleId, groupname);
                    }
                    _MonitorPageCollectionView = new PagedCollectionView(_TableData);
                    if(this._MonitorPageCollectionView != null && this._MonitorPageCollectionView.Count > 0)
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
            }
            catch(Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("GetRunMonitorGroupVehicleList", ex);
            }
        }

        public void HandleEvent(AlarmGisArgs publishedEvent)
        {
            foreach(TableDataElement te in _TableData)
            {
                if(te.VehicleId == publishedEvent.VehicleID)
                {
                    te.HasAlarm = (publishedEvent.Alarm > 0);
                    break;
                }
            }
        }

        /// <summary>
        ///  
        /// </summary>
        public class TableDataElement : INotifyPropertyChanged
        {            /// <summary>
            /// 状态
            /// </summary>
            public bool IsOnLine { get; set; }
            /// <summary>
            /// 所属组
            /// </summary>
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

            /// <summary>
            /// 报警状态
            /// </summary>
            public bool HasAlarm { get; set; }

            /// <summary>
            /// 组织机构名称
            /// </summary>
            public string Orgnization { get; set; }

            /// <summary>
            /// 车牌号
            /// </summary>
            public string VehicleId { get; set; }

            public string UniqueId { get; set; }

            public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

            protected void RaisePropertyChanged(string propertyName)
            {
                System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
                if((propertyChanged != null))
                {
                    propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
                }
            }
        }
    }

}
