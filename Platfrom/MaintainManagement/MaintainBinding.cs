/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 5b8ae9dd-778d-42eb-8485-f8a5c3f8e726      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-DENGZL
/////                 Author: TEST(dengzl)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Maintain
/////    Project Description:    
/////             Class Name: MaintainBinding
/////          Class Version: v1.0.0.0
/////            Create Time: 7/24/2013 5:25:26 PM
/////      Class Description:  
/////======================================================================
/////          Modified Time: 7/24/2013 5:25:26 PM
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



namespace Gsafety.PTMS.Maintain
{
    public class MaintainBinding
    {
        [Export]
        public ViewModelRoute Binding
        {
            get { return ViewModelRoute.Create(MaintainName.GuaranteePeriodVm, MaintainName.GuaranteePeriodV); }
        }

        [Export]
        public ViewModelRoute BindingSuiteQuery
        {
            get { return ViewModelRoute.Create(MaintainName.SecuritySuiteVm, MaintainName.SecuritySuiteV); }
        }

        [Export]
        public ViewModelRoute BindingSuiteQueryDisplay
        {
            get { return ViewModelRoute.Create(MaintainName.SecuritySuiteDisplayVm, MaintainName.SecuritySuiteDisplayV); }
        }

        [Export]
        public ViewModelRoute BindingAbnormalSuiteMaintainHandle
        {
            get { return ViewModelRoute.Create(MaintainName.MaintenanceHandleDetailVm, MaintainName.MaintenanceHandleDetailV); }
        }

        [Export]
        public ViewModelRoute BindingAbnormalSuiteQuery
        {
            get { return ViewModelRoute.Create(MaintainName.MaintenanceHandleVm, MaintainName.MaintenanceHandleV); }
        }

        [Export]
        public ViewModelRoute BindingSuiteUpgrade
        {
            get { return ViewModelRoute.Create(MaintainName.SuiteUpgradeVm, MaintainName.SuiteUpgradeV); }
        }
        [Export]
        public ViewModelRoute BindingSuiteHistoryRecordDisplay
        {
            get { return ViewModelRoute.Create(MaintainName.SuiteHistoryRecordVm, MaintainName.SuiteHistoryRecordV); }
        }

        [Export]
        public ViewModelRoute BindingSuiteAlertInfoDisplay
        {
            get { return ViewModelRoute.Create(MaintainName.SuiteAlertInfoVm, MaintainName.SuiteAlertInfoV); }
        }

        [Export]
        public ViewModelRoute BindingSuiteUpgradeRecord
        {
            get { return ViewModelRoute.Create(MaintainName.UpgradeRecordVm, MaintainName.UpgradeRecordV); }
        }

        [Export]
        public ViewModelRoute BindingSuiteUpgradeRecordDisplay
        {
            get { return ViewModelRoute.Create(MaintainName.UpgradeRecordDisplayVm, MaintainName.UpgradeRecordDisplayV); }
        }

        [Export]
        public ViewModelRoute BindingUpgradeVersionDetail
        {
            get { return ViewModelRoute.Create(MaintainName.VersionMappingVm, MaintainName.VersionMappingV); }
        }

        [Export]
        public ViewModelRoute BindingUpgradeVersionEdit
        {
            get { return ViewModelRoute.Create(MaintainName.VersionMappingEditVm, MaintainName.VersionMappingEditV); }
        }

        [Export]
        public ViewModelRoute BindingUpgradeVersionAdd
        {
            get { return ViewModelRoute.Create(MaintainName.VersionMappingAddVm, MaintainName.VersionMappingAddV); }
        }

        [Export]
        public ViewModelRoute BindingUpgradeStatus
        {
            get { return ViewModelRoute.Create(MaintainName.UpgradeStatusVm, MaintainName.UpgradeStatusV); }
        }

        [Export]
        public ViewModelRoute BindingUpgradeOvertime
        {
            get { return ViewModelRoute.Create(MaintainName.UpgradeOvertimeVm, MaintainName.UpgradeOvertimeV); }
        }
        [Export]
        public ViewModelRoute BindingSuiteInspect
        {
            get { return ViewModelRoute.Create(MaintainName.SuiteInspectVm, MaintainName.SuiteInspectV); }
        }

        [Export]
        public ViewModelRoute BindingSuiteInspectDisplay
        {
            get { return ViewModelRoute.Create(MaintainName.SuiteInspectDisplayVm, MaintainName.SuiteInspectDisplayV); }
        }

        [Export]
        public ViewModelRoute BindingSuiteRunning
        {
            get { return ViewModelRoute.Create(MaintainName.SuiteRunningVm, MaintainName.SuiteRunningV); }
        }

        [Export]
        public ViewModelRoute BindingSuiteRunningDisplay
        {
            get { return ViewModelRoute.Create(MaintainName.SuiteRunningDisplayVm, MaintainName.SuiteRunningDisplayV); }
        }

        [Export]
        public ViewModelRoute BindingSuiteInstalling
        {
            get { return ViewModelRoute.Create(MaintainName.SuiteInstallingVm, MaintainName.SuiteInstallingV); }
        }

        [Export]
        public ViewModelRoute BindingMaintainRecord
        {
            get
            {
                return ViewModelRoute.Create(MaintainName.MaintainRecordVm, MaintainName.MaintainRecordV);
            }
        }

        [Export]
        public ViewModelRoute BindingMaintainList
        {
            get
            {
                return ViewModelRoute.Create(MaintainName.MaintenanceListViewModel, MaintainName.MaintenanceListView);
            }
        }

        [Export]
        public ViewModelRoute BindingMaintenanceDetail
        {
            get
            {
                return ViewModelRoute.Create(MaintainName.MaintenanceDetailViewModel, MaintainName.MaintenanceDetailView);
            }
        }

        [Export]
        public ViewModelRoute BindingMaintenanceSimple
        {
            get
            {
                return ViewModelRoute.Create(MaintainName.MaintenanceSimpleVm, MaintainName.MaintenanceSimpleV);
            }
        }

        [Export]
        public ViewModelRoute BindingMaintenanceScrap
        {
            get
            {
                return ViewModelRoute.Create(MaintainName.MaintenanceScrapVm, MaintainName.MaintenanceScrapV);
            }
        }

        [Export]
        public ViewModelRoute BindingMaintenanceRecordReport
        {
            get
            {
                return ViewModelRoute.Create(MaintainName.MaintainRecordReportVM, MaintainName.MaintainRecordReport);
            }
        }

        [Export]
        public ViewModelRoute BindingServiceLife
        {
            get
            {
                return ViewModelRoute.Create(MaintainName.SuiteLifeVm, MaintainName.SuiteLifeV);
            }
        }

        [Export]
        public ViewModelRoute BindingServiceLifeDetail
        {
            get
            {
                return ViewModelRoute.Create(MaintainName.SuiteLifeDetailVm, MaintainName.SuiteLifeDetailV);
            }
        }

        [Export]
        public ViewModelRoute BindingHandleRecord
        {
            get
            {
                return ViewModelRoute.Create(MaintainName.HandleRecordVm, MaintainName.HandleRecordV);
            }
        }

        [Export]
        public ViewModelRoute BindingHandleRecordDetail
        {
            get
            {
                return ViewModelRoute.Create(MaintainName.HandleRecordDetailVm, MaintainName.HandleRecordDetailV);
            }
        }
    }
}
