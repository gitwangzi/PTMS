using Gsafety.PTMS.ServiceReference.MaitenanceRecordService;
using Gsafety.PTMS.Share;
using Jounce.Core.View;
using Jounce.Core.ViewModel;
using Jounce.Framework.Command;
using Jounce.Framework.ViewModel;
/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 17749712-fa1e-4591-9449-8c6bfd8144f1      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: LIN-20130409ZRS
/////                 Author: TEST(zhujf)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Installation.ViewModels
/////    Project Description:    
/////             Class Name: SimpleMaintenanceVm
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/9/3 17:52:49
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/9/3 17:52:49
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

namespace Gsafety.PTMS.Installation.ViewModels
{
    [ExportAsViewModel(InstallationName.SimpleMaintenanceVm)]
    public class SimpleMaintenanceVm : BaseEntityViewModel
    {
        private MaintenanceRecordServiceClient maintenaceClient;
        public ICommand ReturnCommand { get; private set; }
        public ICommand SubmitCommand { get; private set; }

        public SuiteMaintenance CurrentMaintainingRecord { get; private set; }

        private string _VehicleId;

        public string VehicleId
        {
            get { return _VehicleId; }
            set
            {
                _VehicleId = value;
            }
        }

        private string _SuiteId;

        public string SuiteId
        {
            get { return _SuiteId; }
            set
            {
                _SuiteId = value;
            }
        }

        private string _Maintainer;

        public string Maintainer
        {
            get { return _Maintainer; }
            set
            {
                _Maintainer = value;
            }
        }

        private string _Note;
        public string Note
        {
            get { return _Note; }
            set
            {
                _Note = value;
            }
        }

        protected override void ActivateView(string viewName, System.Collections.Generic.IDictionary<string, object> viewParameters)
        {
            base.ActivateView(viewName, viewParameters);
            CurrentMaintainingRecord = viewParameters["key"] as Gsafety.PTMS.ServiceReference.MaitenanceRecordService.SuiteMaintenance;
            Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => CurrentMaintainingRecord));
            VehicleId = CurrentMaintainingRecord.VehicleID;
            Maintainer = CurrentMaintainingRecord.Maintainer;
            SuiteId = CurrentMaintainingRecord.SuiteId;
            Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => VehicleId));
            Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => Maintainer));
            Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => SuiteId));
            Note = null;
            Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => Note));
        }

        public SimpleMaintenanceVm()
        {
            try
            {
                maintenaceClient = ServiceClientFactory.Create<MaintenanceRecordServiceClient>();
                ReturnCommand = new ActionCommand<object>(obj => Return());
                SubmitCommand = new ActionCommand<object>(obj => Submit());
                maintenaceClient.SimpleMaintenanceCompleted += maintenaceClient_SimpleMaintenanceCompleted;
            }
            catch
            {

            }
        }

        void maintenaceClient_SimpleMaintenanceCompleted(object sender, SimpleMaintenanceCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                return;
            }
            if (e.Result.Result)
            {
                MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("ID_INSTALL__SimpleSucess"));
            }
            else
            {
                MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("ID_INSTALL__SimpleFailed"));
            }
            Return();
        }

        private void Return()
        {
            EventAggregator.Publish(new ViewNavigationArgs(InstallationName.SuiteMaintainingV));
        }

        private void Submit()
        {
            CurrentMaintainingRecord.Note = Note;
            CurrentMaintainingRecord.Status = RepairStatusType.Complete;
            CurrentMaintainingRecord.RepairType = RepairType.Simple;
            CurrentMaintainingRecord.Station_Id = ApplicationContext.Instance.AuthenticationInfo.OrgCode;
    
            maintenaceClient.SimpleMaintenanceAsync(CurrentMaintainingRecord, VehicleId, 20);
            EventAggregator.Publish(new ViewNavigationArgs(InstallationName.SuiteMaintainingV));
        }

    }
}
