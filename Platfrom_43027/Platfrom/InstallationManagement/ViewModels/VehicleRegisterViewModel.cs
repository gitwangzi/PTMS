/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: e6d5fe09-1708-48e9-953d-3812d32a5c39      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-LINGL
/////                 Author: TEST(zhangzl)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Installation.ViewModels
/////    Project Description:    
/////             Class Name: VehicleRegisterViewModel
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/9/3 15:46:13
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/9/3 15:46:13
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using Gsafety.PTMS.ServiceReference.InstallStaffService;
using Gsafety.PTMS.Share;
using Jounce.Core.Command;
using Jounce.Core.View;
using Jounce.Core.ViewModel;
using Jounce.Framework.Command;
using Jounce.Framework.ViewModel;
using Gsafety.PTMS.ServiceReference.MessageService;
using System.Reflection;

namespace Gsafety.PTMS.Installation.ViewModels
{
    [ExportAsViewModel(InstallationName.VehicleRegisterVm)]
    public class VehicleRegisterViewModel : BaseEntityViewModel
    {
        InstallStaffServiceClient InstallStaffClient = null;
        public string VehicleId { get; set; }
        public DateTime ArrivalTime { get; set; }
        private string DeviceAlertHandleId { get; set; }
        private string MdvrCoreId;

        public IActionCommand ReturnCommand { get; private set; }

        protected override void ActivateView(string viewName, System.Collections.Generic.IDictionary<string, object> viewParameters)
        {
            base.ActivateView(viewName, viewParameters);
            if (viewParameters.Count != 0)
            {
                DeviceAlertHandleId = viewParameters["Id"].ToString();
                MdvrCoreId = viewParameters["mdvrCoreId"].ToString();

                VehicleId = viewParameters["vehicleId"].ToString();
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => VehicleId));
                ArrivalTime = DateTime.Now;
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => ArrivalTime));
            }
            InstallStaffClient.GetInstallStaffByTypeAsync(MaintenanceStaffType.MaintenancePersonnel);

        }

        public VehicleRegisterViewModel()
        {
            try
            {
                InstallStaffClient = ServiceClientFactory.Create<InstallStaffServiceClient>();
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogError(MethodBase.GetCurrentMethod().ToString(), ex);
            }
            InstallStaffClient.GetInstallStaffByTypeCompleted += installStaffServiceClient_GetInstallStaffByTypeCompleted;
   
            ReturnCommand = new ActionCommand<object>(obj => Return());
        }


        private void Return()
        {
            EventAggregator.Publish(new ViewNavigationArgs(InstallationName.WaitMaintainV));
        }

        protected override void OnCommitted()
        {
            //SuiteMaintenance item = new SuiteMaintenance()
            //{
            //    Maintainer = CurrentInstallStaff.Name,
            //    Station_Id = ApplicationContext.Instance.AuthenticationInfo.OrgCode,
            //    ArrivalTime = this.ArrivalTime,
            //    RecordStaff = ApplicationContext.Instance.AuthenticationInfo.UserName,
            //    VehicleID = VehicleId,
            //    RepairType = RepairType.Unknown,
            //    Status = RepairStatusType.Repairing,
            //};
            //MaintenanceRecordClient.RegisterMaintenanceAsync(item, DeviceAlertHandleId);
        }

        void installStaffServiceClient_GetInstallStaffByTypeCompleted(object sender, GetInstallStaffByTypeCompletedEventArgs e)
        {
            InstallStaffList = new List<InstallStaffBasicInfo>(e.Result.Result);
            InstallStaffBasicInfo item= new InstallStaffBasicInfo { Name = ApplicationContext.Instance.StringResourceReader.GetString("BASEINFO_PleaseSelect"), ID = string.Empty };
            InstallStaffList.Insert(0, item);
            Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => InstallStaffList));
            CurrentInstallStaff = InstallStaffList[0];
            Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => CurrentInstallStaff));
        }

        public List<InstallStaffBasicInfo> InstallStaffList { get; set; }

        private InstallStaffBasicInfo currentInstallStaff;
        public InstallStaffBasicInfo CurrentInstallStaff
        {
            get { return currentInstallStaff; }
            set
            {
                currentInstallStaff = value;
                if (currentInstallStaff != null)
                {
                    ValidateInstallStaff(ExtractPropertyName(() => CurrentInstallStaff), value);
                }
            }
        }

        private void ValidateInstallStaff(string prop, object value)
        {
            ClearErrors(prop);

            if (value == null || !(value is InstallStaffBasicInfo) || ((InstallStaffBasicInfo)value).ID == string.Empty)
            {
                SetError(prop, ApplicationContext.Instance.StringResourceReader.GetString("BASEINFO_MustFilled"));
            }
        }
    }
}

