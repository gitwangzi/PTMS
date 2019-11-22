/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 31ad0ead-e878-4e58-b73f-036473b9bac9      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-DENGZL
/////                 Author: TEST(dengzl)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Traffic
/////    Project Description:    
/////             Class Name: TrafficBinding
/////          Class Version: v1.0.0.0
/////            Create Time: 7/24/2013 5:19:01 PM
/////      Class Description:  
/////======================================================================
/////          Modified Time: 7/24/2013 5:19:01 PM
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System;
using System.ComponentModel.Composition;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Jounce.Core.ViewModel;

namespace Gsafety.PTMS.Traffic
{
    public class TrafficBinding
    {
        [Export]
        public ViewModelRoute TrafficBingdingMenu
        {
            get { return ViewModelRoute.Create(TrafficName.TrafficMenuVm, TrafficName.TrafficMenuV); }
        }

        [Export]
        public ViewModelRoute SpeedRuleParameterSettingBinding
        {
            get { return ViewModelRoute.Create(TrafficName.SpeedRuleParameterSettingVm, TrafficName.SpeedRuleParameterSettingV); }
        }

        [Export]
        public ViewModelRoute TrafficBingdingMainPage
        {
            get { return ViewModelRoute.Create(TrafficName.TrafficMainPageViewModel, TrafficName.TrafficMainPage); }
        }

        //[Export]
        //public ViewModelRoute BindingTrafficDetailInfo
        //{
        //    get { return ViewModelRoute.Create(TrafficName.TrafficManagerDetailInfoViewModel, TrafficName.TrafficManagerDetailInfoView); }
        //} 
        /// <summary>
        /// 
        /// </summary>
        //[Export]
        //public ViewModelRoute TrafficGISViewVm
        //{
        //    get { return ViewModelRoute.Create(GisManagement.GisName.TrfficGisVM, GisManagement.GisName.TrafficGisView); }
        //}
        /// <summary>
        /// 
        /// </summary>
        //[Export]
        //public ViewModelRoute TrafficGISViewVm2
        //{
        //    get { return ViewModelRoute.Create(GisManagement.GisName.GisViewModel, GisManagement.GisName.TrafficGisView); }
        //}
        /// <summary>
        /// 
        /// </summary>
        [Export]
        public ViewModelRoute TrafficBingdingDetailInfo
        {
            get { return ViewModelRoute.Create(TrafficName.TrafficManagerDetailInfoViewModel, TrafficName.TrafficManagerDetailInfoView); }
        }

        /// <summary>
        /// 
        /// </summary>
        [Export]
        public ViewModelRoute CarHisBindingTafficFence
        {
            get { return ViewModelRoute.Create(GisManagement.GisName.GpsCarHisDataViewModel, (GisManagement.GisName.GpsCarHisDataViewTrafficFence)); }
        }

        /// <summary>
        /// 
        /// </summary>
        [Export]
        public ViewModelRoute CarHisBindingTrafficRoute
        {
            get { return ViewModelRoute.Create(GisManagement.GisName.GpsCarHisDataViewModel, (GisManagement.GisName.GpsCarHisDataViewTrafficRoute)); }
        }

        /// <summary>
        /// 
        /// </summary>
        [Export]
        public ViewModelRoute CarHisBindingTrafficManager
        {
            get { return ViewModelRoute.Create(GisManagement.GisName.GpsCarHisDataViewModel, (GisManagement.GisName.GpsCarHisDataViewTrafficManager)); }
        }

        #region

        [Export]
        public ViewModelRoute SettingToVehicle
        {
            get
            {
                return ViewModelRoute.Create(TrafficName.SettingToVehicleVM, TrafficName.SettingToVehicleView);
            }
        }
        [Export]
        public ViewModelRoute RuleToVehicle
        {
            get
            {
                return ViewModelRoute.Create(TrafficName.RuleToVehicleViewModel, TrafficName.RuleToVehicleView);
            }
        }

        [Export]
        public ViewModelRoute RuleCommandVehicle
        {
            get
            {
                return ViewModelRoute.Create(TrafficName.RuleCommandStateViewModel, TrafficName.RuleCommandStateView);
            }
        }

        [Export]
        public ViewModelRoute SpeedRuleParameterSetting
        {
            get
            {
                return ViewModelRoute.Create(TrafficName.SpeedRuleParameterSettingVm, TrafficName.SpeedRuleParameterSettingV);
            }
        }

        [Export]
        public ViewModelRoute RuleToVehicleDetail
        {
            get
            {
                return ViewModelRoute.Create(TrafficName.RuleToVehicleDetailViewModel, TrafficName.RuleToVehicleDetailView);
            }
        }

        [Export]
        public ViewModelRoute SendVechileDetail
        {
            get
            {
                return ViewModelRoute.Create(TrafficName.SendVehicleDetailVm, TrafficName.SendVehicleDetailV);
            }
        }
        #endregion
    }
}
