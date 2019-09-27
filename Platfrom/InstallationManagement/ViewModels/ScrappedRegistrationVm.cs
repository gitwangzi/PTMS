using Gsafety.PTMS.ServiceReference.MaitenanceRecordService;
using Gsafety.PTMS.Share;
using Jounce.Core.View;
using Jounce.Core.ViewModel;
using Jounce.Framework.Command;
using Jounce.Framework.ViewModel;
/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 1deb19cb-8679-48f3-8a96-c8ae52c54fb3      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: LIN-20130409ZRS
/////                 Author: TEST(zhujf)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Installation.ViewModels
/////    Project Description:    
/////             Class Name: ScrappedRegistrationVm
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/9/3 17:52:28
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/9/3 17:52:28
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
    [ExportAsViewModel(InstallationName.ScrappedRegistrationVm)]
    public class ScrappedRegistrationVm : BaseEntityViewModel
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
        public ScrappedRegistrationVm()
        {
            try
            {
                maintenaceClient = ServiceClientFactory.Create<MaintenanceRecordServiceClient>();
                ReturnCommand = new ActionCommand<object>(obj => Return());
                SubmitCommand = new ActionCommand<object>(obj => Submit());
                maintenaceClient.ScrappedMaintenanceCompleted+=maintenaceClient_ScrappedMaintenanceCompleted;
            }
            catch
            {

            }
        }

        private void Return()
        {
            EventAggregator.Publish(new ViewNavigationArgs(InstallationName.SuiteMaintainingV));
        }

        private void Submit()
        {
            CurrentMaintainingRecord.Note = Note;
            CurrentMaintainingRecord.Status = RepairStatusType.Complete;
            CurrentMaintainingRecord.RepairType = RepairType.Scrap;
            CurrentMaintainingRecord.Station_Id = ApplicationContext.Instance.AuthenticationInfo.OrgCode;
            maintenaceClient.ScrappedMaintenanceAsync(CurrentMaintainingRecord, VehicleId, 40);
     
            EventAggregator.Publish(new ViewNavigationArgs(InstallationName.SuiteMaintainingV));
        }

        void maintenaceClient_ScrappedMaintenanceCompleted(object sender, ScrappedMaintenanceCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                return;
            }
            if (e.Result.Result)
            {
                MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("ID_INSTALL__ScrapSucess"));
            }
            else
            {
                MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("ID_INSTALL__ScrapFailed"));
            }
            Return();
        }

    }
}
