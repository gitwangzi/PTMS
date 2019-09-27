/////Copyright (C) Gsafety 2012 .All Rights Reserved.
/////======================================================================
/////                   Guid: 8c804e97-1d05-490e-8b9f-1932487fe09e      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-DENGZL
/////                 Author: TEST(dengzl)
/////======================================================================
/////           Project Name: Gsafety.Ant.Monitor.Models
/////    Project Description:    
/////             Class Name: MoniterGroupManage
/////          Class Version: v1.0.0.0
/////            Create Time: 7/3/2012 10:06:06 AM
/////      Class Description:  
/////======================================================================
/////          Modified Time: 7/3/2012 10:06:06 AM
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System;
using System.Collections.ObjectModel;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Linq;
using Gsafety.Ant.Share;
using Gsafety.Ant.ServiceReference.MonitorGroupService;
using System.Collections.Generic;
using Gsafety.Ant.ServiceReference.MonitorGroupVehicleService;
using Gsafety.Ant.Bases.Librarys;
using Jounce.Core.Model;
using Jounce.Core.Event;
using Gsafety.Ant.Share.Model;
using System.ComponentModel.Composition;

namespace Gsafety.Ant.Monitor.Models
{
    public delegate void RefreshData();
    public delegate void SaveAllAfter();
    /// <summary>
    /// 分组管理
    /// </summary>
    public class MoniterGroupManage : BaseNotify, IPartImportsSatisfiedNotification, IEventSink<MonitorEntityMessage>
    {
        #region Fields
        [Import]
        public IEventAggregator EventAggregator { get; set; }
        private ObservableCollection<MoniterGroup> _MoniterGroups = new ObservableCollection<MoniterGroup>();
        private TreeViewModel<MonitorEntity> _MonitorTreeViews;
        public event SaveAllAfter SaveAllAfterEvent;

        #endregion

        #region Attributes

        public TreeViewModel<MonitorEntity> MonitorGroupTreeViews
        {

            get
            {
                if (_MonitorTreeViews == null || _MonitorTreeViews.Nodes.Count == 0)
                    _MonitorTreeViews = new TreeViewModel<MonitorEntity>(new ObservableCollection<MonitorEntity>(_MoniterGroups), (MonitorEntity e) => e.GetChilds());
                return _MonitorTreeViews;
            }
            //set
            //{

            //    _MoniterGroups = value;

            //    Jounce.Framework.JounceHelper.ExecuteOnUI(() => { RaisePropertyChanged(() => MoniterGroups); });

            //}
        }

        /// <summary>
        /// 转换一下，为了和以前兼容一下
        /// </summary>
        /// <returns></returns>
        public ObservableCollection<MoniterGroup> ToGroupList()
        {

            //ObservableCollection<MoniterGroup> ans = new ObservableCollection<MoniterGroup>();

            //foreach (NodeViewModel<MonitorEntity> me in _MonitorTreeViews.Nodes)
            //{
            //    if (me.Indentation == 0)
            //    {
            //        ans.Add((MoniterGroup)me.Model);
            //    }
            //}

            //return ans;

            return _MoniterGroups;
        }

        #endregion
        /// <summary>
        /// 分组服务引用
        /// </summary>
        MonitorGroupServiceClient monitorGroupServiceClient = ServiceClientFactory.Create<MonitorGroupServiceClient>();

        /// <summary>
        /// 分组与车辆关联服务引用
        /// </summary>
        MonitorGroupVehicleServiceClient monitorGroupVehicleServiceClient = ServiceClientFactory.Create<MonitorGroupVehicleServiceClient>();

        public MoniterGroupManage()
        {
            //TestData testData = new TestData();
            //MoniterGroups = testData.MoniterGroups;
            //monitorGroupServiceClient.GetMonitorGroupsCompleted += monitorGroupServiceClient_GetMonitorGroupsCompleted;
            CompositionInitializer.SatisfyImports(this);
            monitorGroupServiceClient.BatchAddMonitorGroupCompleted += monitorGroupServiceClient_BatchAddMonitorGroupCompleted;
            monitorGroupServiceClient.GetMonitorGroupsCompleted += monitorGroupServiceClient_GetMonitorGroupsCompleted;
            RefreshMoniterGroupsData();

            // monitorGroupVehicleServiceClient.GetAllMonitorGroupsVehicleCompleted += monitorGroupVehicleServiceClient_GetAllMonitorGroupsVehicleCompleted;
            //LoadMoniterGroupsData();
        }

        /// <summary>
        /// 加载或刷新分组数据
        /// </summary>
        public void RefreshMoniterGroupsData()
        {

            monitorGroupServiceClient.GetMonitorGroupsAsync(ApplicationContext.Instance.AuthenticationInfo.UserName);
        }

        /// <summary>
        /// 加载分组信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void monitorGroupServiceClient_GetMonitorGroupsCompleted(object sender, GetMonitorGroupsCompletedEventArgs e)
        {
            if (e.Error != null)
            {

                return;
            }
            if (null == e.Result)
            {

            }

            ApplicationContext.Instance.Logger.LogInforMession(GetType().FullName, "开始加载车辆的分组");
            if (null == _MoniterGroups) { _MoniterGroups = new ObservableCollection<MoniterGroup>(); }
            _MoniterGroups.Clear();
            //MoniterGroups = new ObservableCollection<MoniterGroup>();
            foreach (MonitorGroup mg in e.Result.Result)
            {
                MoniterGroup item = new MoniterGroup() { GroupName = mg.GroupName, Note = mg.Note, CreateUser = mg.CreateUser, GroupIndex = mg.GroupIndex, ID = mg.ID };
                item.Vehicles = new ObservableCollection<Vehicle>();
                _MoniterGroups.Add(item);

            }

            //RaisePropertyChanged(() => MonitorGroupTreeViews);
            ApplicationContext.Instance.Logger.LogInforMession(GetType().FullName, "加载车辆的分组完成");
            MonitorGroupVehicleServiceClient monitorGroupVehicleServiceClient = ServiceClientFactory.Create<MonitorGroupVehicleServiceClient>();
            monitorGroupVehicleServiceClient.GetAllMonitorGroupsVehicleCompleted += monitorGroupVehicleServiceClient_GetAllMonitorGroupsVehicleCompleted;
            monitorGroupVehicleServiceClient.GetAllMonitorGroupsVehicleAsync();


        }

        /// <summary>
        /// 对分组进行车辆的填充
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void monitorGroupVehicleServiceClient_GetAllMonitorGroupsVehicleCompleted(object sender, GetAllMonitorGroupsVehicleCompletedEventArgs e)
        {
            if (e.Error != null)
            {

                return;
            }
            if (null != e.Result)
            {

                // foreach (Gsafety.Ant.Share.Vehicle vc in ApplicationContext.Instance.BufferManager.DistrictManager.VehicleList)
                // {
                ApplicationContext.Instance.Logger.LogInforMession(GetType().FullName, "开始加载分组中关联的车辆");
                foreach (MoniterGroup mg in _MoniterGroups)
                {
                    foreach (MonitorGroupVehicle mgv in e.Result.Result)
                    {
                        if (mg.ID == mgv.Group_ID)
                        {
                            foreach (Vehicle item in ApplicationContext.Instance.BufferManager.DistrictManager.VehicleList)
                            {
                                if (item.VehicleId.Trim() == mgv.Vehicle_ID.Trim())
                                {
                                    item.InitMonitor(mgv.Traced_Flag == 1 ? true : false);
                                    item.GroupID = mgv.Group_ID;
                                    mg.Vehicles.Add(item);
                                    //if (item.IsMonitor==true)// && null != moniterVehicle)
                                    //{
                                    //    InitMonitorVehicle(item);
                                    //}
                                    //if (item.IsMonitor == true && Loaded_Event != null)
                                    //{
                                    //    Loaded_Event(item);
                                    //}
                                }
                                //Vehicle v = new Vehicle();
                                //v.GroupID = mgv.Group_ID;
                                //v.VehicleId = mgv.Vehicle_ID;
                                //v.ANTGPSID = mgv.ANTGPSID;
                                //v.MDVRID = mgv.MDVRID;
                                //v.VehicleType = mgv.Type == ServiceReference.MonitorGroupVehicleService.VehicleType.Bus ? Gsafety.Ant.Share.VehicleType.Bus : mgv.Type == ServiceReference.MonitorGroupVehicleService.VehicleType.Flota ? Gsafety.Ant.Share.VehicleType.Flota : Gsafety.Ant.Share.VehicleType.Taxi;
                                //v.IsOnLine = mgv.IsOnLine == null ? false : mgv.IsOnLine == 1 ? true : false;
                                //v.IsMonitor = mgv.Traced_Flag == null ? false : mgv.Traced_Flag == 1 ? true : false;
                                ////v.ANTGPSID = mgv.
                                //mg.Vehicles.Add(v);
                                //vc.GroupID = mg.ID;AEB0 0072
                            }
                        }
                    }
                    // }
                }
                _MonitorTreeViews = new TreeViewModel<MonitorEntity>(new ObservableCollection<MonitorEntity>(_MoniterGroups), (MonitorEntity w) => w.GetChilds());

                ApplicationContext.Instance.Logger.LogInforMession(GetType().FullName, "加载分组中关联的车辆完成");

            }

            if (null != RefreshDataEvent)
            {
                RefreshDataEvent();
            }
        }

        public Vehicle findMacth(string mdvrID)
        {

            foreach (NodeViewModel<MonitorEntity> mgz in MonitorGroupTreeViews.Nodes)
            {
                MoniterGroup mg = mgz.Model as MoniterGroup;
                foreach (Vehicle car in mg.Vehicles)
                {
                    if (car.MDVRID == mdvrID)
                    {
                        return car;
                    }
                }
            }

            return null;

        }

        void monitorGroupServiceClient_BatchAddMonitorGroupCompleted(object sender, BatchAddMonitorGroupCompletedEventArgs e)
        {
            if (null != SaveAllAfterEvent)
            {
                SaveAllAfterEvent();
            }
        }
        /// <summary>
        /// 加载车辆结构数据
        /// </summary>
        private void LoadVehicleData()
        {
        }

        public event RefreshData RefreshDataEvent;



        /// <summary>
        /// 加载分组数据
        /// </summary>
        //public void LoadMoniterGroupsData()
        //{
        //    monitorGroupServiceClient.GetMonitorGroupsAsync(ApplicationContext.Instance.AuthenticationInfo.UserName);
        //}

        public void AddVehicle(Vehicle vehicle)
        {

            MoniterGroup moniterGroup = this.ToGroupList().Where(item => item.IsDefaultGroup).FirstOrDefault();
            if (moniterGroup != null)
                AddVehicle(vehicle, moniterGroup.GroupName);
            else
                AddVehicle(vehicle, this.ToGroupList()[0].GroupName);
        }

        public void AddGroup(string groupName)
        {
            int exist = this.ToGroupList().Select(item => groupName.Equals(item.GroupName)).Count();
            if (exist > 0)
                return;
            MoniterGroup moniterGroup = new MoniterGroup();
            moniterGroup.GroupName = groupName;//.Add(new NodeViewModel<T>(this, root, 0, getChildFunction))
            _MonitorTreeViews.Nodes.Add(new NodeViewModel<MonitorEntity>(_MonitorTreeViews, moniterGroup, 0, (MonitorEntity e) => e.GetChilds()));
        }

        public void DeleteGroup(string groupName)
        {
            //MoniterGroup moniterGroup = this.ToGroupList().Where(item => groupName.Equals(item.GroupName)).FirstOrDefault();
            foreach (NodeViewModel<MonitorEntity> me in _MonitorTreeViews.Nodes)
            {
                if (((MoniterGroup)me.Model).GroupName == groupName)
                {
                    _MonitorTreeViews.Nodes.Remove(me);
                    return;
                }
            }

        }

        public void GroupReName(string oldName, string newName)
        {
        }

        public void SetDefultGroup(string groupName)
        {

        }

        public void SaveAll()
        {
            ObservableCollection<ServiceReference.MonitorGroupService.MonitorGroup> list = new ObservableCollection<ServiceReference.MonitorGroupService.MonitorGroup>();
            int i = 0;

            //foreach (MoniterGroup mg in MoniterGroups)
            foreach (MoniterGroup mg in _MoniterGroups)
            {
                //MoniterGroup mg = (MoniterGroup)me.Model;
                list.Add(new ServiceReference.MonitorGroupService.MonitorGroup() { ID = mg.ID, GroupIndex = (short)i, CreateUser = mg.CreateUser, Note = mg.Note, GroupName = mg.GroupName });
                i++;
            }
            monitorGroupServiceClient.BatchAddMonitorGroupAsync(list, ApplicationContext.Instance.AuthenticationInfo.UserName);

        }

        public void AddVehicle(Vehicle vehicle, string groupID)
        {
            if (vehicle == null || string.IsNullOrEmpty(vehicle.VehicleId))
                return;
            //foreach (var group in MoniterGroups)
            //{
            //    foreach (var ve in group.Vehicles)
            //    {
            //        if (ve.VehicleId.Equals(vehicle.VehicleId))
            //            return;
            //    }
            //}
            MoniterGroup moniterGroup = null;// MoniterGroups.Where(item => groupID.Equals(item.ID)).FirstOrDefault();
            foreach (NodeViewModel<MonitorEntity> me in _MonitorTreeViews.Nodes)
            {
                if (me.Indentation != 0) continue;
                if (((MoniterGroup)me.Model).ID == groupID)
                {
                    moniterGroup = (MoniterGroup)me.Model;
                    break;
                }

            }
            if (moniterGroup != null)
            {
                moniterGroup.Vehicles.Add(vehicle);
                monitorGroupVehicleServiceClient.AddMonitorGroupsVehicleAsync(moniterGroup.ID, vehicle.VehicleId, vehicle.IsMonitor == true ? (short)1 : (short)0, moniterGroup.Vehicles.Count);
                vehicle.GroupID = groupID;



                NodeViewModel<MonitorEntity> searchGroup = _MonitorTreeViews.Nodes.Where(w => w.Model.Name == moniterGroup.Name).FirstOrDefault();
                if (searchGroup == null) return;
                NodeViewModel<MonitorEntity> searchVehicle = _MonitorTreeViews.Nodes.Where(w => w.Model.Name == vehicle.Name && w.ParentNodeViewModel == searchGroup).FirstOrDefault();

                NodeViewModel<MonitorEntity> vehiclemodel = new NodeViewModel<MonitorEntity>(_MonitorTreeViews, searchGroup, vehicle, 1, (MonitorEntity e) => e.GetChilds());


                if (searchGroup != null && searchGroup.IsExpanded)
                {
                    int index = _MonitorTreeViews.Nodes.IndexOf(searchGroup);
                    _MonitorTreeViews.Nodes.Insert(++index, vehiclemodel);

                }

                if (searchGroup != null && searchGroup.ChildrenCount == 1)
                {
                    searchGroup.ExpanderVisibility = Visibility.Visible;
                }

                //if (!searchGroup.IsExpanded && searchGroup.ChildrenCount == 1)
                //{
                //    searchGroup.IsExpanded = true;
                //}
                //var aa = searchGroup.ExpanderVisibility;
                //Jounce.Framework.JounceHelper.ExecuteOnUI(()=>RaisePropertyChanged(()=>searchGroup.ExpanderVisibility));
                ////searchGroup.ExpanderVisibility

                if (null != RefreshDataEvent)
                {
                    RefreshDataEvent();
                }
            }

        }



        public void DeleteVehicle(Vehicle vehicle)
        {
            if (vehicle == null)
                return;
            foreach (NodeViewModel<MonitorEntity> me in _MonitorTreeViews.Nodes)
            {
                if (me.Indentation != 0) continue;
                MoniterGroup group = me.Model as MoniterGroup;

                foreach (var ve in group.Vehicles)
                {
                    if (ve.VehicleId.Equals(vehicle.VehicleId))
                    {
                        group.Vehicles.Remove(ve);
                        monitorGroupVehicleServiceClient.RemoveMonitorGroupsVehicleAsync(group.ID, vehicle.VehicleId);
                        vehicle.GroupID = null;
                        group.Flag = Guid.NewGuid().ToString();

                        NodeViewModel<MonitorEntity> searchGroup = _MonitorTreeViews.Nodes.Where(w => w.Model.Name == group.Name).FirstOrDefault();
                        if (searchGroup == null) return;
                        NodeViewModel<MonitorEntity> searchVehicle = _MonitorTreeViews.Nodes.Where(w => w.Model.Name == vehicle.Name && w.ParentNodeViewModel == searchGroup).FirstOrDefault();

                        //NodeViewModel<MonitorEntity> vehiclemodel = _MonitorTreeViews.Nodes.Where(w => w.Model.Name == vehicle.Name && w.ParentNodeViewModel == searchGroup).FirstOrDefault();


                        if (searchGroup != null && searchGroup.IsExpanded)
                        {
                            _MonitorTreeViews.Nodes.Remove(searchVehicle);

                        }

                        if (searchGroup.IsExpanded && searchGroup.ChildrenCount == 0)
                        {
                            searchGroup.ExpanderVisibility = Visibility.Collapsed;
                        }

                        return;
                    }
                }
            }
        }

        public void DeleteVehicle(Vehicle vehicle, string groupID)
        {
            if (vehicle == null || string.IsNullOrEmpty(vehicle.VehicleId))
                return;
            //foreach (var group in MoniterGroups)
            //{
            //    foreach (var ve in group.Vehicles)
            //    {
            //        if (ve.VehicleId.Equals(vehicle.VehicleId))
            //            return;
            //    }
            //}
            MoniterGroup moniterGroup = null;// MoniterGroups.Where(item => groupID.Equals(item.ID)).FirstOrDefault();
            foreach (NodeViewModel<MonitorEntity> me in _MonitorTreeViews.Nodes)
            {
                if (((MoniterGroup)me.Model).ID == groupID)
                {
                    moniterGroup = (MoniterGroup)me.Model;
                    break;
                }
            }

            moniterGroup.Vehicles.Remove(vehicle);
            monitorGroupVehicleServiceClient.RemoveMonitorGroupsVehicleAsync(moniterGroup.ID, vehicle.VehicleId);
        }
        public void ChangeMonitorStatu(string vehicleId, bool monitorStatus)
        {
        }

        public MoniterGroup FindGroupByVehicle(string VehicleId)
        {
            foreach (NodeViewModel<MonitorEntity> me in _MonitorTreeViews.Nodes)
            {
                MoniterGroup group = me.Model as MoniterGroup;
                foreach (Vehicle ve in group.Vehicles)
                {
                    if (ve.VehicleId.Trim().Equals(VehicleId))
                    {
                        return group;
                    }

                }
            }

            return null;

        }

        public void MoveVehicle(Vehicle vehicle, string oldGroupid, string newGroupID)
        {
            if (vehicle == null || string.IsNullOrEmpty(vehicle.VehicleId))
                return;
            vehicle.GroupID = oldGroupid;
            DeleteVehicle(vehicle);
            vehicle.GroupID = null;
            AddVehicle(vehicle, newGroupID);
            //foreach (var group in MoniterGroups)
            //{
            //    foreach (var ve in group.Vehicles)
            //    {
            //        if (ve.VehicleId.Equals(vehicle.VehicleId))
            //            return;
            //    }
            //}

            //MoniterGroup moniterGroup = MoniterGroups.Where(item => newGroupID.Equals(item.ID)).FirstOrDefault();
            //MoniterGroup moniterGroup = null;// MoniterGroups.Where(item => groupID.Equals(item.ID)).FirstOrDefault();
            //foreach (NodeViewModel<MonitorEntity> me in _MonitorTreeViews.Nodes)
            //{
            //    if (((MoniterGroup)me.Model).ID == newGroupID)
            //    {
            //        moniterGroup = (MoniterGroup)me.Model;
            //        break;
            //    }
            //}

            //Vehicle vehicleflag = moniterGroup.Vehicles.Where(item => vehicle.VehicleId.Trim().Equals(item.VehicleId.Trim())).FirstOrDefault();
            //if (null == vehicleflag)
            //{
            //    moniterGroup.Vehicles.Add(vehicle);
            //}

            //if (moniterGroup != null)
            //{

            //    monitorGroupVehicleServiceClient.MoveAsync(vehicle.VehicleId, vehicle.IsMonitor == true ? (short)1 : (short)0, moniterGroup.Vehicles.Count, vehicle.GroupID, newGroupID);
            //    vehicle.GroupID = newGroupID;
            //}
        }

        public void OnImportsSatisfied()
        {
            EventAggregator.SubscribeOnDispatcher<MonitorEntityMessage>(this);
        }

        public void HandleEvent(MonitorEntityMessage publishedEvent)
        {
            if (null != publishedEvent)
            {

                switch (publishedEvent.Operator)
                {

                    case MonitorOperatorEnum.Add:
                        {
                            Gsafety.Ant.Share.Vehicle vehicle = publishedEvent.Entity as Gsafety.Ant.Share.Vehicle;
                            if (null == vehicle) return;
                            MoniterGroup group = _MoniterGroups.Where(w => w.ID == vehicle.GroupID).FirstOrDefault();
                            if (null == group) return;
                            group.Vehicles.Add(vehicle);
                            NodeViewModel<MonitorEntity> searchGroup = _MonitorTreeViews.Nodes.Where(w => w.Model.Name == group.Name).FirstOrDefault();
                            if (searchGroup == null || !searchGroup.IsExpanded) return;
                            NodeViewModel<MonitorEntity> searchVehicle = _MonitorTreeViews.Nodes.Where(w => w.Model.Name == vehicle.Name && w.ParentNodeViewModel == searchGroup).FirstOrDefault();
                            if (searchGroup.IsExpanded)
                            {
                                NodeViewModel<MonitorEntity> vehiclemodel = new NodeViewModel<MonitorEntity>(_MonitorTreeViews, searchGroup, vehicle, 1, (MonitorEntity e) => e.GetChilds());
                                int index = _MonitorTreeViews.Nodes.IndexOf(searchGroup);
                                _MonitorTreeViews.Nodes.Insert(++index, vehiclemodel);

                            }

                            if (null != RefreshDataEvent)
                            {
                                RefreshDataEvent();
                            }
                        }
                        break;
                    case MonitorOperatorEnum.Remove:
                        {
                            Gsafety.Ant.Share.Vehicle vehicle = publishedEvent.Entity as Gsafety.Ant.Share.Vehicle;
                            if (null == vehicle) return;
                            MoniterGroup group = _MoniterGroups.Where(w => w.ID == vehicle.GroupID).FirstOrDefault();
                            if (null == group) return;
                            group.Vehicles.Remove(group.Vehicles.Where(w => w.VehicleId == vehicle.VehicleId).FirstOrDefault());
                            NodeViewModel<MonitorEntity> searchGroup = _MonitorTreeViews.Nodes.Where(w => w.Model.Name == group.Name).FirstOrDefault();
                            if (searchGroup == null || !searchGroup.IsExpanded) return;
                            NodeViewModel<MonitorEntity> searchVehicle = _MonitorTreeViews.Nodes.Where(w => w.Model.Name == vehicle.Name && w.ParentNodeViewModel == searchGroup).FirstOrDefault();
                            if (searchGroup.IsExpanded && null != searchVehicle)
                            {
                                _MonitorTreeViews.Nodes.Remove(searchVehicle);

                            }

                            if (null != RefreshDataEvent)
                            {
                                RefreshDataEvent();
                            }
                        }
                        break;

                }


            }
        }
    }
}
