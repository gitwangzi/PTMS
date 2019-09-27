using Gsafety.PTMS.ServiceReference.DeviceAlertService;
using Gsafety.PTMS.ServiceReference.InstallStationService;
using Gsafety.PTMS.ServiceReference.VehicleService;
using Gsafety.PTMS.ServiceReference.WorkingSuiteService;
using Gsafety.PTMS.Share;
using Jounce.Core.View;
using Jounce.Core.ViewModel;
using Jounce.Framework.Command;
using Jounce.Framework.ViewModel;
/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 56704d0a-30df-4b9b-94eb-6570efb09ace      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-PENGGL
/////                 Author: TEST(penggl)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Maintain.ViewModels
/////    Project Description:    
/////             Class Name: AbnormalSuiteMaintainHandleVm
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/9/12 11:28:32
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/9/12 11:28:32
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System;
using System.Collections.Generic;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Gsafety.PTMS.Maintain.ViewModels
{
    [ExportAsViewModel(MaintainName.MaintenanceHandleDetailVm)]
    public class MaintenanceHandleDetailVm : BaseEntityViewModel
    {
        private InstallStationServiceClient InstallStationClient = ServiceClientFactory.Create<InstallStationServiceClient>();
        private DeviceAlertServiceClient DeviceAlertClient = ServiceClientFactory.Create<DeviceAlertServiceClient>();
        private VehicleServiceClient VehicleClient = ServiceClientFactory.Create<VehicleServiceClient>();
        public string Title { get; private set; }
        public Gsafety.PTMS.ServiceReference.VehicleService.Vehicle CurrentVehicle { get; private set; }
        public DateTime CurrentTime { get; private set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public ICommand ResetCommand { get; private set; }
        public ICommand ReturnCommand { get; private set; }
        private DeviceAlertHandle InitialDeviceAlertHandle { get; set; }
        public DeviceAlertHandle CurrentDeviceAlertHandle { get; set; }

        private InstallStation currentInstallStation;
        public InstallStation CurrentInstallStation
        {
            get { return currentInstallStation; }
            set
            {
                currentInstallStation = value;
                if (IsChecked)
                {
                    ValidateInstallStation(ExtractPropertyName(() => CurrentInstallStation), currentInstallStation);
                }
            }
        }

        private void ValidateInstallStation(string prop, InstallStation value)
        {
            ClearErrors(prop);

            if (value == null ||value.Id == string.Empty)
            {
                SetError(prop, ApplicationContext.Instance.StringResourceReader.GetString("BASEINFO_MustFilled"));//必填字段
            }
        }

        public List<InstallStation> InstallStationList { get; set; }
        public WorkingSuite CurrentWorkingSuite { get; set; }
        public WorkingSuite InitialWorkingSuite { get; set; }
        public string IsView { get; private set; }
        public string CheckedContent { get; private set; }
        public int RowNum { get; private set; }
        private bool isChecked;

        public bool IsChecked
        {
            get { return isChecked; }
            set 
            { 
                isChecked = value;
                if (isChecked)
                {
                    IsView = "Visible";
                    Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => IsView));
                    RowNum = 4;
                    Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => RowNum));

                    ValidateInstallStation(ExtractPropertyName(() => CurrentInstallStation), currentInstallStation);
                }
                else
                {
                    IsView = "Collapsed";
                    Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => IsView));
                    RowNum = 3;
                    Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => RowNum));

                    ClearErrors(ExtractPropertyName(() => CurrentInstallStation));
                }
            }
        }


        
        private string action;
        protected override void ActivateView(string viewName, System.Collections.Generic.IDictionary<string, object> viewParameters)
        {
            base.ActivateView(viewName, viewParameters);
            action = viewParameters["action"].ToString();
            if (action == "arrange")
            {
                Title = ApplicationContext.Instance.StringResourceReader.GetString("MAINTAIN_AbnormalSuiteMaintainArrange");
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => Title));
                InitialWorkingSuite = viewParameters["arrange"] as WorkingSuite;
                VehicleClient.GetVehicleByIDAsync(InitialWorkingSuite.VehicleId);
                CurrentWorkingSuite = MaintainCommon.Clone(InitialWorkingSuite);
                IsChecked = false;
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => IsChecked));
                
            }
            InstallStationClient.GetInstallStationsAsync();
        }

        public MaintenanceHandleDetailVm()
        {
            try
            {
                VehicleClient.GetVehicleByIDCompleted += VehicleClient_GetVehicleByIDCompleted;
                InstallStationClient.GetInstallStationsCompleted += InstallStationClient_GetInstallStationsCompleted;
                DeviceAlertClient.AddDeviceAlertHandleCompleted += DeviceAlertClient_AddDeviceAlertHandleCompleted;
                DeviceAlertClient.AddDeviceAlertCheckCompleted += DeviceAlertClient_AddDeviceAlertCheckCompleted;
                ReturnCommand = new ActionCommand<object>(obj => Return());
                ResetCommand = new ActionCommand<object>(obj => Reset());
                CurrentTime = DateTime.Now;
            }
            catch
            {

            }
        }

        void VehicleClient_GetVehicleByIDCompleted(object sender, GetVehicleByIDCompletedEventArgs e)
        {
            if (e.Result != null)
            {
                CurrentVehicle = e.Result.Result;
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => CurrentVehicle));
            }
        }

        void InstallStationClient_GetInstallStationsCompleted(object sender, GetInstallStationsCompletedEventArgs e)
        {
            InstallStationList = new List<InstallStation>();
            InstallStationList.Add(new InstallStation { Name = ApplicationContext.Instance.StringResourceReader.GetString("BASEINFO_PleaseSelect"), Id = string.Empty });
            InstallStationList.AddRange(e.Result.Result);
            Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => InstallStationList));
            Reset();
        }

        void DeviceAlertClient_AddDeviceAlertCheckCompleted(object sender, AddDeviceAlertCheckCompletedEventArgs e)
        {
            if (IsChecked)
            {
                CurrentDeviceAlertHandle.VehicleId = CurrentWorkingSuite.VehicleId;
                CurrentDeviceAlertHandle.StartTime = StartTime;
                CurrentDeviceAlertHandle.EndTime = EndTime;
                CurrentDeviceAlertHandle.StationId = CurrentInstallStation.Id;
                CurrentDeviceAlertHandle.Status = 1;  //处置
                CurrentDeviceAlertHandle.HandleUser = ApplicationContext.Instance.AuthenticationInfo.UserName;
                DeviceAlertClient.AddDeviceAlertHandleAsync(CurrentDeviceAlertHandle);
            }
            else
            {
                MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("MAINTAIN_HandleSuccess"));
                Return();
            }
        }

        void DeviceAlertClient_AddDeviceAlertHandleCompleted(object sender, AddDeviceAlertHandleCompletedEventArgs e)
        {
            if (e.Result == null || !e.Result.IsSuccess)
            {
                MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("BASEINFO_Operate_Failed"));
            }
            Return();
        }

        protected override void OnCommitted()
        {
            DeviceAlertCheck temp = new DeviceAlertCheck
            {
                Content = CheckedContent,
                DisposeStaff = ApplicationContext.Instance.AuthenticationInfo.UserName,
                VehicleId = CurrentWorkingSuite.VehicleId,
            };
            DeviceAlertClient.AddDeviceAlertCheckAsync(temp, IsChecked);
        }

        private void Reset()
        {
            StartTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
            Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => StartTime));
            EndTime = new DateTime(DateTime.Now.AddDays(1).Year, DateTime.Now.AddDays(1).Month, DateTime.Now.AddDays(1).Day, 23, 59, 59);
            Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => EndTime));
            CurrentTime = DateTime.Now;
            Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => CurrentTime));
            CurrentDeviceAlertHandle = new DeviceAlertHandle();
            Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => CurrentDeviceAlertHandle));
            CurrentWorkingSuite = MaintainCommon.Clone(InitialWorkingSuite);
            Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => CurrentWorkingSuite));
            CurrentInstallStation = InstallStationList[0];
            Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => CurrentInstallStation));
            CheckedContent = string.Empty;
            Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => CheckedContent));
        }

        private void Return()
        {
            EventAggregator.Publish(new ViewNavigationArgs(MaintainName.MaintenanceHandleV, new Dictionary<string, object>() { { "action", "return" } }));
        }

    }
}
