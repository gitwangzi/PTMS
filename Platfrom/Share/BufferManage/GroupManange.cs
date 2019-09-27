using Gsafety.PTMS.Bases.Models;
using Gsafety.PTMS.ServiceReference.MonitorGroupService;
using Gsafety.PTMS.ServiceReference.MonitorGroupVehicleService;
using Gsafety.PTMS.Share;
using Jounce.Core.Event;
using Jounce.Core.Model;
/////Copyright (C) Gsafety 2014 .All Rights Reserved.
/////======================================================================
/////                   Guid: 204cf0be-3a46-43c0-aa1d-e3b8a579b463      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-ZHOUDD
/////                 Author: GJSY(zhoudd)
/////======================================================================
/////           Project Name: Gsafety.Ant.Share.BufferManage
/////    Project Description:    
/////             Class Name: GroupManange
/////          Class Version: v1.0.0.0
/////            Create Time: 2014/11/7 11:36:48
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2014/11/7 11:36:48
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Net;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Gsafety.PTMS.Share
{
    public class GroupManange : BaseNotify
    {
        [Import]
        public IEventAggregator EventAggregator { get; set; }
        private ObservableCollection<MoniterGroupEx> _MoniterGroups = new ObservableCollection<MoniterGroupEx>();
        MonitorGroupServiceClient monitorGroupServiceClient = ServiceClientFactory.Create<MonitorGroupServiceClient>();
        public event RefreshData RefreshDataEvent;
        public bool _GetDataResult = true;
        public delegate void RefreshData();


        MonitorGroupVehicleServiceClient monitorGroupVehicleServiceClient = ServiceClientFactory.Create<MonitorGroupVehicleServiceClient>();

        private AsyncOperation asyncOper;

        public GroupManange()
        {
            asyncOper = AsyncOperationManager.CreateOperation(null);
            _MoniterGroups = new ObservableCollection<MoniterGroupEx>();
            monitorGroupServiceClient.GetMonitorGroupsCompleted += monitorGroupServiceClient_GetMonitorGroupsCompleted;

        }
        public void DataLoading()
        {
            RefreshMoniterGroupsData();
            //TaskFactory taskFactory = new TaskFactory();
            //taskFactory.StartNew(UiRefresh);
        }
        public void RefreshMoniterGroupsData()
        {
            monitorGroupServiceClient.GetMonitorGroupsAsync(ApplicationContext.Instance.AuthenticationInfo.UserName);
        }
        public ObservableCollection<MoniterGroupEx> MoniterGroups
        {
            get
            {
                if (!_GetDataResult)
                    RefreshMoniterGroupsData();
                return _MoniterGroups;
            }
        }
        void monitorGroupServiceClient_GetMonitorGroupsCompleted(object sender, GetMonitorGroupsCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                _GetDataResult = false;
                return;
            }
            ApplicationContext.Instance.Logger.LogInforMession(GetType().FullName, "begin load group");
            if (null == _MoniterGroups) { _MoniterGroups = new ObservableCollection<MoniterGroupEx>(); }
            _MoniterGroups.Clear();
            foreach (MonitorGroup mg in e.Result.Result)
            {
                MoniterGroupEx item = new MoniterGroupEx() { GroupName = mg.GroupName, Note = mg.Note, CreateUser = mg.CreateUser, GroupIndex = mg.GroupIndex, ID = mg.ID };
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
                _GetDataResult = false;
                return;
            }
            if (null != e.Result)
            {
                ApplicationContext.Instance.Logger.LogInforMession(GetType().FullName, "begin load group cars");
                foreach (MoniterGroupEx mg in _MoniterGroups)
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
                                        //SubscriptionGpsEvent(item);
                                    }
                                    item.GroupID = mgv.Group_ID;
                                    //item.IsVisibleToChoose = Visibility.Collapsed;
                                    mg.Vehicles.Add(new VehicleEx(null, item));
                                }
                            }
                        }
                    }
                }
                //_MonitorTreeViews = new TreeViewModel<MonitorEntity>(new ObservableCollection<MonitorEntity>(_MoniterGroups), (MonitorEntity w) => w.GetChilds());

                ApplicationContext.Instance.Logger.LogInforMession(GetType().FullName, "load group cars success");

            }

            if (null != RefreshDataEvent)
            {
                RefreshDataEvent();
            }
        }

    }
}
