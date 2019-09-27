using Jounce.Core.ViewModel;
/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 77b33b04-b853-4438-a8ec-6f2c2828ae06      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-DENGZL
/////                 Author: TEST(dengzl)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Monitor
/////    Project Description:    
/////             Class Name: MonitorBinding
/////          Class Version: v1.0.0.0
/////            Create Time: 7/24/2013 5:22:58 PM
/////      Class Description:  
/////======================================================================
/////          Modified Time: 7/24/2013 5:22:58 PM
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System.ComponentModel.Composition;

namespace Gsafety.PTMS.Monitor
{
    public class MonitorBinding
    {
        [Export]
        public ViewModelRoute BindingMenu
        {
            get
            {
                return ViewModelRoute.Create(MonitorName.MonitorMenuVm, MonitorName.MonitorMenuV);
            }
        }

        [Export]
        public ViewModelRoute Binding
        {
            get { return ViewModelRoute.Create(MonitorName.MonitorMainPageVm, MonitorName.MonitorMainPageV); }
        }

        [Export]
        public ViewModelRoute BindingVehicleDetail
        {
            get { return ViewModelRoute.Create(MonitorName.VehicleDetailViewModel, MonitorName.VehicleDetailView); }
        }


        [Export]
        public ViewModelRoute BindingMonitorGroupManager
        {
            get { return ViewModelRoute.Create(MonitorName.MonitorGroupManagerVm, MonitorName.MonitorGroupManager); }
        }


        [Export]
        public ViewModelRoute BindingHisList
        {
            get { return ViewModelRoute.Create(GisManagement.GisName.GpsCarHisDataViewModel, GisManagement.GisName.GpsCarHisDataViewMonitor); }
        }

        [Export]
        public ViewModelRoute BindingVehicleInfo
        {
            get { return ViewModelRoute.Create(MonitorName.VehicleInfoViewModle, MonitorName.VehicleInfoView); }
        }

        /// <summary>
        /// 实现公共交通产品主界面View和ViewModel的绑定
        /// </summary>
        [Export]
        public ViewModelRoute BindingMonitorMainPage
        {
            get
            {
                return ViewModelRoute.Create(MonitorName.AntProductMonitorMainPageVm, MonitorName.AntProductMonitorMainPageV);
            }
        }

        [Export]
        public ViewModelRoute BindingMonitorAlarmInfoView
        {
            get
            {
                return ViewModelRoute.Create(MonitorName.MonitorAlarmInfoViewModel, MonitorName.MonitorAlarmInfoView);
            }
        }

        [Export]
        public ViewModelRoute BindingMonitorAlertInfoView
        {
            get
            {
                return ViewModelRoute.Create(MonitorName.MonitorAlertInfoViewModel, MonitorName.MonitorAlertInfoView);
            }
        }

        [Export]
        public ViewModelRoute BindingAlarmHandle
        {
            get
            {
                return ViewModelRoute.Create(MonitorName.MonitorAlarmHandleViewModel, MonitorName.MonitorAlarmHandleView);
            }
        }

        [Export]
        public ViewModelRoute BindingAlertHandle
        {
            get
            {
                return ViewModelRoute.Create(MonitorName.MonitorAlertHandleViewModel, MonitorName.MonitorAlertHandleView);
            }
        }

        [Export]
        public ViewModelRoute BindingManualAlarmHandle
        {
            get
            {
                return ViewModelRoute.Create(MonitorName.MonitorManualAlarmHandleViewModel, MonitorName.MonitorManualAlarmHandleView);
            }
        }



        [Export]
        public ViewModelRoute BindingSelectFence
        {
            get
            {
                return ViewModelRoute.Create(MonitorName.MonitorSelectFenceViewModel, MonitorName.MonitorSelectFenceView);
            }
        }
    }
}
