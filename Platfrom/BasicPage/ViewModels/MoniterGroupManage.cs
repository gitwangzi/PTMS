/////Copyright (C) Gsafety 2012 .All Rights Reserved.
/////======================================================================
/////                   Guid: 8c804e97-1d05-490e-8b9f-1932487fe09e      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-DENGZL
/////                 Author: TEST(dengzl)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Monitor.Models
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
using Gsafety.PTMS.ServiceReference.MonitorGroupService;
using System.Collections.Generic;
using Gsafety.PTMS.ServiceReference.MonitorGroupVehicleService;
using Jounce.Core.Model;
using Jounce.Core.Event;
using Gsafety.PTMS.Bases.Models;
using Gsafety.PTMS.Bases.Librarys;
using Gsafety.PTMS.Share;
using System.ComponentModel.Composition;

namespace Gsafety.PTMS.BasicPage.Monitor.ViewModels
{
    public delegate void RefreshData();
    public delegate void SubscriptionAntGps(Vehicle vehicle);
    /// <summary>
    /// MoniterGroupManage
    /// </summary>
    public class MoniterGroupManage : 
        BaseNotify, 
        IPartImportsSatisfiedNotification, 
        IEventSink<MonitorEntityMessage>, 
        IEventSink<UserDefineGroupDataRefreshMessage>,
        IEventSink<UserDefineGroupVehicleDataRefreshMessage>
        
    {
        #region Fields
        [Import]
        public IEventAggregator EventAggregator { get; set; }
        private ObservableCollection<MoniterGroup> _MoniterGroups = new ObservableCollection<MoniterGroup>();
        private TreeViewModel<IMonitorEntity> _MonitorTreeViews;


        #endregion

        #region Attributes

        public TreeViewModel<IMonitorEntity> MonitorGroupTreeViews
        {
            get
            {
                if (_MonitorTreeViews == null || _MonitorTreeViews.Nodes.Count == 0)
                    _MonitorTreeViews = new TreeViewModel<IMonitorEntity>(new ObservableCollection<IMonitorEntity>(_MoniterGroups), (IMonitorEntity e) => e.GetChilds());
                return _MonitorTreeViews;
            }
        }

        public ObservableCollection<MoniterGroup> ToGroupList()
        {
            return _MoniterGroups;
        }

        #endregion

        MonitorGroupServiceClient monitorGroupServiceClient = ServiceClientFactory.Create<MonitorGroupServiceClient>();

        MonitorGroupVehicleServiceClient monitorGroupVehicleServiceClient = ServiceClientFactory.Create<MonitorGroupVehicleServiceClient>();
        private static MoniterGroupManage _Instance = null;

        public static MoniterGroupManage Instance
        {
            get
            {
                if (_Instance == null)
                {
                    _Instance = new MoniterGroupManage();
                }
                return _Instance;
            }

        }

        public MoniterGroupManage()
        {
            CompositionInitializer.SatisfyImports(this);
            monitorGroupServiceClient.GetMonitorGroupsCompleted += monitorGroupServiceClient_GetMonitorGroupsCompleted;
            RefreshMoniterGroupsData();
        }


        public void RefreshMoniterGroupsData()
        {
            monitorGroupServiceClient.GetMonitorGroupsAsync(ApplicationContext.Instance.AuthenticationInfo.UserName);
        }

        void monitorGroupServiceClient_GetMonitorGroupsCompleted(object sender, GetMonitorGroupsCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                return;
            }
            ApplicationContext.Instance.Logger.LogInforMession(GetType().FullName, "begin load group");
            if (null == _MoniterGroups) { _MoniterGroups = new ObservableCollection<MoniterGroup>(); }
            _MoniterGroups.Clear();
            foreach (MonitorGroup mg in e.Result.Result)
            {
                MoniterGroup item = new MoniterGroup() { GroupName = mg.GroupName, Note = mg.Note, CreateUser = mg.CreateUser, GroupIndex = mg.GroupIndex, ID = mg.ID };
                item.Vehicles = new ObservableCollection<VehicleEx>();
                _MoniterGroups.Add(item);
            }
            MonitorGroupVehicleServiceClient monitorGroupVehicleServiceClient = ServiceClientFactory.Create<MonitorGroupVehicleServiceClient>();
            monitorGroupVehicleServiceClient.GetAllMonitorGroupsVehicleCompleted += monitorGroupVehicleServiceClient_GetAllMonitorGroupsVehicleCompleted;
            monitorGroupVehicleServiceClient.GetAllMonitorGroupsVehicleAsync();
        }
        
        void monitorGroupVehicleServiceClient_GetAllMonitorGroupsVehicleCompleted(object sender, GetAllMonitorGroupsVehicleCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                return;
            }
            if (null != e.Result)
            {
                ApplicationContext.Instance.Logger.LogInforMession(GetType().FullName, "begin load group cars");
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
                                    item.InitMonitor(mgv.Traced_Flag == 1);
                                    if (mgv.Traced_Flag == 1)
                                    {
                                        SubscriptionGpsEvent(item);
                                    }
                                    item.GroupID = mgv.Group_ID;
                                    VehicleEx vehicleEx = new VehicleEx(item);
                                    vehicleEx.CheckVisibility = Visibility.Collapsed;
                                    mg.Vehicles.Add(vehicleEx);
                                }
                            }
                        }
                    }
                }
                _MonitorTreeViews = new TreeViewModel<IMonitorEntity>(new ObservableCollection<IMonitorEntity>(_MoniterGroups), (IMonitorEntity w) => w.GetChilds());
                ApplicationContext.Instance.Logger.LogInforMession(GetType().FullName, "load group cars success");
            }
            //if (null != RefreshDataEvent)
            //{
            //    RefreshDataEvent();
            //}
            EventAggregator.Publish<UserDefineGroupPageRefreshMessage>(new UserDefineGroupPageRefreshMessage());
        }

        //public event RefreshData RefreshDataEvent;
        public event SubscriptionAntGps SubscriptionGpsEvent;

        private void AddVehicle(Vehicle vehicle, string groupID)
        {
            if (vehicle == null || string.IsNullOrEmpty(vehicle.VehicleId))
                return;
            MoniterGroup moniterGroup = null;// MoniterGroups.Where(item => groupID.Equals(item.ID)).FirstOrDefault();
            foreach (NodeViewModel<IMonitorEntity> me in _MonitorTreeViews.Nodes)
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
                VehicleEx vehicleEx = new VehicleEx(vehicle);
                vehicleEx.CheckVisibility = Visibility.Collapsed;
                moniterGroup.Vehicles.Add(vehicleEx);
                
                vehicle.GroupID = groupID;
                NodeViewModel<IMonitorEntity> searchGroup = _MonitorTreeViews.Nodes.Where(w => w.Model.Name == moniterGroup.Name).FirstOrDefault();
                if (searchGroup == null) return;
                NodeViewModel<IMonitorEntity> searchVehicle = _MonitorTreeViews.Nodes.Where(w => w.Model.Name == vehicle.Name && w.ParentNodeViewModel == searchGroup).FirstOrDefault();
                NodeViewModel<IMonitorEntity> vehiclemodel = new NodeViewModel<IMonitorEntity>(_MonitorTreeViews, searchGroup, vehicleEx, 1, (IMonitorEntity e) => e.GetChilds());
                if (searchGroup != null && searchGroup.IsExpanded)
                {
                    int index = _MonitorTreeViews.Nodes.IndexOf(searchGroup);
                    _MonitorTreeViews.Nodes.Insert(++index, vehiclemodel);
                }
                if (searchGroup != null && searchGroup.ChildrenCount == 1)
                {
                    searchGroup.ExpanderVisibility = Visibility.Visible;
                }
                //if (null != RefreshDataEvent)
                //{
                //    RefreshDataEvent();
                //}
                EventAggregator.Publish<UserDefineGroupPageRefreshMessage>(new UserDefineGroupPageRefreshMessage());
            }
        }

        private void DeleteVehicle(Vehicle vehicle)
        {
            if (vehicle == null)
                return;
            foreach (NodeViewModel<IMonitorEntity> me in _MonitorTreeViews.Nodes)
            {
                if (me.Indentation != 0) continue;
                MoniterGroup group = me.Model as MoniterGroup;

                foreach (var ve in group.Vehicles)
                {
                    if (ve.VehicleId.Equals(vehicle.VehicleId))
                    {
                        group.Vehicles.Remove(ve);
                        vehicle.GroupID = null;
                        group.Flag = Guid.NewGuid().ToString();
                        NodeViewModel<IMonitorEntity> searchGroup = _MonitorTreeViews.Nodes.Where(w => w.Model.Name == group.Name).FirstOrDefault();
                        if (searchGroup == null) return;
                        NodeViewModel<IMonitorEntity> searchVehicle = _MonitorTreeViews.Nodes.Where(w => w.Model.Name == vehicle.Name && w.ParentNodeViewModel == searchGroup).FirstOrDefault();

                        if (searchGroup != null && searchGroup.IsExpanded)
                        {
                            _MonitorTreeViews.Nodes.Remove(searchVehicle);
                        }
                        if (searchGroup.ChildrenCount == 0)
                        {
                            searchGroup.ExpanderVisibility = Visibility.Collapsed;
                        }
                        return;
                    }
                }
            }
        }

        public void OnImportsSatisfied()
        {
            EventAggregator.SubscribeOnDispatcher<MonitorEntityMessage>(this);
            EventAggregator.SubscribeOnDispatcher<UserDefineGroupDataRefreshMessage>(this);
            EventAggregator.SubscribeOnDispatcher<UserDefineGroupVehicleDataRefreshMessage>(this);
        }

        public void HandleEvent(MonitorEntityMessage publishedEvent)
        {
            if (null != publishedEvent)
            {
                switch (publishedEvent.Operator)
                {
                    case MonitorOperatorEnum.Add:
                        {
                            Gsafety.PTMS.Bases.Models.Vehicle vehicle = publishedEvent.Entity as Gsafety.PTMS.Bases.Models.Vehicle;
                            if (null == vehicle) return;
                            MoniterGroup group = _MoniterGroups.Where(w => w.ID == vehicle.GroupID).FirstOrDefault();
                            if (null == group) return;
                            VehicleEx vehicleex = group.AddVehicle(vehicle);
                            //group.Vehicles.Add(vehicle);
                            NodeViewModel<IMonitorEntity> searchGroup = _MonitorTreeViews.Nodes.Where(w => w.Model.Name == group.Name).FirstOrDefault();
                            if (searchGroup == null || !searchGroup.IsExpanded) return;
                            NodeViewModel<IMonitorEntity> searchVehicle = _MonitorTreeViews.Nodes.Where(w => w.Model.Name == vehicle.Name && w.ParentNodeViewModel == searchGroup).FirstOrDefault();
                            if (searchGroup.IsExpanded)
                            {
                                NodeViewModel<IMonitorEntity> vehiclemodel = new NodeViewModel<IMonitorEntity>(_MonitorTreeViews, searchGroup, vehicleex, 1, (IMonitorEntity e) => e.GetChilds());
                                int index = _MonitorTreeViews.Nodes.IndexOf(searchGroup);
                                _MonitorTreeViews.Nodes.Insert(++index, vehiclemodel);
                            }
                            //if (null != RefreshDataEvent)
                            //{
                            //    RefreshDataEvent();
                            //}
                            EventAggregator.Publish<UserDefineGroupPageRefreshMessage>(new UserDefineGroupPageRefreshMessage());
                        }
                        break;
                    case MonitorOperatorEnum.Remove:
                        {
                            Gsafety.PTMS.Bases.Models.Vehicle vehicle = publishedEvent.Entity as Gsafety.PTMS.Bases.Models.Vehicle;
                            if (null == vehicle) return;
                            MoniterGroup group = _MoniterGroups.Where(w => w.ID == vehicle.GroupID).FirstOrDefault();
                            if (null == group) return;
                            group.Vehicles.Remove(group.Vehicles.Where(w => w.VehicleId == vehicle.VehicleId).FirstOrDefault());
                            NodeViewModel<IMonitorEntity> searchGroup = _MonitorTreeViews.Nodes.Where(w => w.Model.Name == group.Name).FirstOrDefault();
                            if (searchGroup == null || !searchGroup.IsExpanded) return;
                            NodeViewModel<IMonitorEntity> searchVehicle = _MonitorTreeViews.Nodes.Where(w => w.Model.Name == vehicle.Name && w.ParentNodeViewModel == searchGroup).FirstOrDefault();
                            if (searchGroup.IsExpanded && null != searchVehicle)
                            {
                                _MonitorTreeViews.Nodes.Remove(searchVehicle);
                            }
                            //if (null != RefreshDataEvent)
                            //{
                            //    RefreshDataEvent();
                            //}
                            EventAggregator.Publish<UserDefineGroupPageRefreshMessage>(new UserDefineGroupPageRefreshMessage());
                        }
                        break;
                    case MonitorOperatorEnum.OnOffLine:
                        {
                            Gsafety.PTMS.Bases.Models.Vehicle vehicle = publishedEvent.Entity as Gsafety.PTMS.Bases.Models.Vehicle;
                            if (null == vehicle) return;
                            MoniterGroup group = _MoniterGroups.Where(w => w.ID == vehicle.GroupID).FirstOrDefault();
                            if (null == group) return;
                            VehicleEx vehicleex = group.Vehicles.Where(item => item.VehicleId == vehicle.VehicleId).FirstOrDefault();
                            vehicleex.IsOnLine = vehicle.IsOnLine;
                        }
                        break;
                }
            }
        }

        public void HandleEvent(UserDefineGroupDataRefreshMessage publishedEvent)
        {
            this.RefreshMoniterGroupsData();
        }

        public void HandleEvent(UserDefineGroupVehicleDataRefreshMessage publishedEvent)
        {
            if (publishedEvent != null)
            {
                switch (publishedEvent.doOperator)
                { 
                    case UserDefineGroupOperator.Add:
                        {
                            this.AddVehicle(publishedEvent.vehicle,publishedEvent.vehicle.GroupID);
                        }
                        break;
                    case UserDefineGroupOperator.Delete:
                        {
                            this.DeleteVehicle(publishedEvent.vehicle);
                        }
                        break;
                    case UserDefineGroupOperator.Move:
                        {
                            string newGroupId = publishedEvent.vehicle.GroupID;
                            publishedEvent.vehicle.GroupID = publishedEvent.oldGroup;
                            this.DeleteVehicle(publishedEvent.vehicle);
                            publishedEvent.vehicle.GroupID = null;
                            this.AddVehicle(publishedEvent.vehicle, newGroupId);
                        }
                        break;
                
                }
            }
        }
    }
}
