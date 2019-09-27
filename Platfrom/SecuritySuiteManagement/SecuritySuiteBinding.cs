/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 71c7c783-c22d-4e50-88c8-a57ab3e515de      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-DENGZL
/////                 Author: TEST(dengzl)
/////======================================================================
/////           Project Name: Gsafety.PTMS.SecuritySuite
/////    Project Description:    
/////             Class Name: SecuritySuiteBinding
/////          Class Version: v1.0.0.0
/////            Create Time: 7/24/2013 5:24:02 PM
/////      Class Description:  
/////======================================================================
/////          Modified Time: 7/24/2013 5:24:02 PM
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.ComponentModel.Composition;
using Jounce.Core.ViewModel;

namespace Gsafety.PTMS.SecuritySuite
{
    public class SecuritySuiteBinding
    {
        
        [Export]
        public ViewModelRoute BindingMainPage
        {
            get { return ViewModelRoute.Create(SecuritySuiteName.SuiteMainPageVm, SecuritySuiteName.SuiteMainPageV); }
        }

        [Export]
        public ViewModelRoute BindingMenu
        {
            get
            {
                return ViewModelRoute.Create(SecuritySuiteName.SuiteMenuVm, SecuritySuiteName.SuiteMenuV);
            }
        }


        [Export]
        public ViewModelRoute BindingInstallHistory
        {
            get
            {
                return ViewModelRoute.Create(SecuritySuiteName.InstallFinishVm, SecuritySuiteName.InstallFinishV);
            }
        }

        [Export]
        public ViewModelRoute BindingInstallingRecord
        {
            get
            {
                return ViewModelRoute.Create(SecuritySuiteName.InstallingRecordVm, SecuritySuiteName.InstallingRecordV);
            }
        }

        [Export]
        public ViewModelRoute BindingOnLine
        {
            get
            {
                return ViewModelRoute.Create(SecuritySuiteName.OnLineVm, SecuritySuiteName.OnLineV);
            }
        }

        [Export]
        public ViewModelRoute BindingOffLine
        {
            get
            {
                return ViewModelRoute.Create(SecuritySuiteName.OffLineVm, SecuritySuiteName.OffLineV);
            }
        }

        [Export]
        public ViewModelRoute BindingVehicleElectronicFence
        {
            get
            {
                return ViewModelRoute.Create(SecuritySuiteName.VehicleElectronicFenceVM, SecuritySuiteName.VehicleElectronicFenceV);
            }
        }
        [Export]
        public ViewModelRoute BindingTravelPlanImplementation
        {
            get
            {
                return ViewModelRoute.Create(SecuritySuiteName.TravelPlanImplementationVM, SecuritySuiteName.TravelPlanImplementationV);
            }
        }
        [Export]
        public ViewModelRoute BindingTravelPlanDetail
        {
            get
            {
                return ViewModelRoute.Create(SecuritySuiteName.TravelPlanDetailVM, SecuritySuiteName.TravelPlanDetailV);
            }
        }
        [Export]
        public ViewModelRoute SwitchingStatus
        {
            get
            {
                return ViewModelRoute.Create(SecuritySuiteName.SwitchingStatusVm, SecuritySuiteName.SwitchingStatusV);
            }
        }
        [Export]
        public ViewModelRoute SwitchingStatusDisplay
        {
            get
            {
                return ViewModelRoute.Create(SecuritySuiteName.SwitchingStatusDisplayVm, SecuritySuiteName.SwitchingStatusDisplayV);
            }
        }
        [Export]
        public ViewModelRoute SwitchingAlerInfo
        {
            get
            {
                return ViewModelRoute.Create(SecuritySuiteName.SwitchingSuiteAlertInfoVm, SecuritySuiteName.SwitchingSuiteAlertInfoV);
            }
        }
        [Export]
        public ViewModelRoute SuiteStatusManagement
        {
            get
            {
                return ViewModelRoute.Create(SecuritySuiteName.SuiteStatusManagementVm, SecuritySuiteName.SuiteStatusManagementV);
            }
        }
        [Export]
        public ViewModelRoute InfoSuiteStatus
        {
            get
            {
                return ViewModelRoute.Create(SecuritySuiteName.SuiteStatusManagementVm, SecuritySuiteName.InfoSuiteStatusV);
            }
        }
    }
}
