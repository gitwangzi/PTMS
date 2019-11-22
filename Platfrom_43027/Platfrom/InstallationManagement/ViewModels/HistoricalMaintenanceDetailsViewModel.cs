/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 4317bab7-16ab-4a32-8950-36b8e66aba40      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: GAOZITIAN-PC
/////                 Author: TEST(gaozt)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Installation.ViewModels
/////    Project Description:    
/////             Class Name: HistoricalMaintenanceDetailsViewModel
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/9/6 14:20:25
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/9/6 14:20:25
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
using Gsafety.PTMS.Installation.Models;
using Gsafety.PTMS.ServiceReference.MaitenanceRecordService;
using Gsafety.PTMS.ServiceReference.SecuritySuiteService;
using Gsafety.PTMS.Share;
using Jounce.Core.Command;
using Jounce.Core.View;
using Jounce.Core.ViewModel;
using Jounce.Framework.Command;

namespace Gsafety.PTMS.Installation.ViewModels
{
    [ExportAsViewModel(InstallationName.HistoricalMaintenanceDetailsVm)]
    public class HistoricalMaintenanceDetailsViewModel : BaseViewModel
    {
        private HistorySuiteMaintenance CurrentInstallRecord { get; set; }

        private MaintainInfo _maintaininfo = new MaintainInfo();
        public MaintainInfo Maintaininfo
        {
            get
            {
                return _maintaininfo;
            }
            set
            {
                _maintaininfo = value;
            }
        }


        MaintenanceRecordServiceClient maintenanceRecordServiceClient = ServiceClientFactory.Create<MaintenanceRecordServiceClient>();
        SecuritySuiteServiceClient securitySuiteServiceClient = ServiceClientFactory.Create<SecuritySuiteServiceClient>();
        protected override void ActivateView(string viewName, System.Collections.Generic.IDictionary<string, object> viewParameters)
        {
            Maintaininfo = new MaintainInfo();
            CurrentInstallRecord = (HistorySuiteMaintenance)viewParameters["HistorySuiteMaintenance"];
            Maintaininfo.CarNumber = CurrentInstallRecord.VehicleID;
            Maintaininfo.SuiteId = CurrentInstallRecord.SuiteId;
            Maintaininfo.Maintainer = CurrentInstallRecord.MaintenanceStafft;
            Maintaininfo.Note = CurrentInstallRecord.Note;
            Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => Maintaininfo));
            securitySuiteServiceClient.GetRepairSuiteBySuiteidAsync(CurrentInstallRecord.SuiteId);
        }

        public IActionCommand backcommand { get; private set; }
        public HistoricalMaintenanceDetailsViewModel()
        {
            backcommand = new ActionCommand<object>(obj => back());
            securitySuiteServiceClient.GetRepairSuiteBySuiteidCompleted += securitySuiteServiceClient_GetRepairSuiteBySuiteidCompleted;
        }

        private void back()
        {
            EventAggregator.Publish(new ViewNavigationArgs(InstallationName.HistoryInstallationInfoV));
        }

        void securitySuiteServiceClient_GetRepairSuiteBySuiteidCompleted(object sender, GetRepairSuiteBySuiteidCompletedEventArgs e)
        {
            try
            {
                if (e.Result.Result.NewDeviceSuite != null)
                {
                    Maintaininfo.NewCamera1 = e.Result.Result.NewDeviceSuite.Camera1Id;
                    Maintaininfo.NewCamera2 = e.Result.Result.NewDeviceSuite.Camera2Id;
                    Maintaininfo.NewAlarm1 = e.Result.Result.NewDeviceSuite.AlarmButton1Id;
                    Maintaininfo.NewAlarm2 = e.Result.Result.NewDeviceSuite.AlarmButton2Id;
                    Maintaininfo.NewAlarm3 = e.Result.Result.NewDeviceSuite.AlarmButton3Id;
                    Maintaininfo.NewUps = e.Result.Result.NewDeviceSuite.UpsId;
                    Maintaininfo.NewSdCard = e.Result.Result.NewDeviceSuite.SdCardId;
                    Maintaininfo.NewOpenDoor = e.Result.Result.NewDeviceSuite.DoorSensorId;
                    Maintaininfo.NewMdvrSn = e.Result.Result.NewDeviceSuite.MdvrId;
                    Maintaininfo.NewMdvrCoreSn = e.Result.Result.NewDeviceSuite.MdvrCoreId;
                }
                if (e.Result.Result.OldDeviceSuite != null)
                {
                    Maintaininfo.OldCamera1 = e.Result.Result.OldDeviceSuite.Camera1Id;
                    Maintaininfo.OldCamera2 = e.Result.Result.OldDeviceSuite.Camera2Id;
                    Maintaininfo.OldAlarm1 = e.Result.Result.OldDeviceSuite.AlarmButton1Id;
                    Maintaininfo.OldAlarm2 = e.Result.Result.OldDeviceSuite.AlarmButton2Id;
                    Maintaininfo.OldAlarm3 = e.Result.Result.OldDeviceSuite.AlarmButton3Id;
                    Maintaininfo.OldUps = e.Result.Result.OldDeviceSuite.UpsId;
                    Maintaininfo.OldSdCard = e.Result.Result.OldDeviceSuite.SdCardId;
                    Maintaininfo.OldOpenDoor = e.Result.Result.OldDeviceSuite.DoorSensorId;
                    Maintaininfo.OldMdvrSn = e.Result.Result.OldDeviceSuite.MdvrId;
                    Maintaininfo.OldMdvrCoreSn = e.Result.Result.OldDeviceSuite.MdvrCoreId;
                }
                if (e.Result.Result.NewDeviceSuite != null && e.Result.Result.OldDeviceSuite != null)
                {
                    Maintaininfo.isrepairinit();
                }
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => Maintaininfo));
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogError("HistoricalMaintenanceDetailsViewModel", ex);
            }
        }
    }
}
