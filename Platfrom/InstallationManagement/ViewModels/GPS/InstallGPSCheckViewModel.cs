using Gsafety.Ant.Installation.ViewModels;
using Gsafety.Common.Controls;
using Gsafety.PTMS.ServiceReference.DevGpsService;
using Gsafety.PTMS.ServiceReference.DeviceInstallService;
using Gsafety.PTMS.ServiceReference.SecuritySuiteService;
using Gsafety.PTMS.ServiceReference.VehicleService;
using Gsafety.PTMS.Share;
using Jounce.Core.View;
using Jounce.Core.ViewModel;
/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 04849f7f-ad2b-44b6-9d22-f25d4731d197      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: GAOZITIAN-PC
/////                 Author: TEST(gaozt)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Installation.ViewModels
/////    Project Description:    
/////             Class Name: InputDeviceInfoViewModel
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/8/16 16:18:11
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/8/16 16:18:11
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System;
using System.Collections.Generic;
using System.Windows;
namespace Gsafety.PTMS.Installation.ViewModels
{
    [ExportAsViewModel(InstallationName.InstallGPSCheckVm)]
    public class InstallGPSCheckViewModel : InstallGPSViewModelBase
    {
        private DevGps _CurrentModel = new DevGps();
        public DevGps CurrentModel
        {
            get
            {
                return _CurrentModel;
            }
            set
            {
                _CurrentModel = value;
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => CurrentModel));
            }
        }

        string organizationid = string.Empty;

        DevGpsServiceClient devGPSServiceClient = null;
        VehicleServiceClient vehicleServiceClient = null;
        private string gpssn;
        public string GPSSn
        {
            get { return gpssn; }
            set
            {
                gpssn = value;
                if (string.IsNullOrEmpty(gpssn))
                {
                    IsGetMessage = false;
                }
                else
                {
                    IsGetMessage = true;
                }

                RaisePropertyChanged(() => GPSSn);
            }
        }
        public InstallGPSCheckViewModel()
        {
            try
            {
                step = 2;
                devGPSServiceClient = ServiceClientFactory.Create<DevGpsServiceClient>();
                vehicleServiceClient = ServiceClientFactory.Create<VehicleServiceClient>();
                ImageSource = "GpsSetp2.png";
                devGPSServiceClient.GetDevGpsBySNCompleted += devGPSServiceClient_GetDevGpsBySNCompleted;
                deviceInstallServiceClient.SubmitGPSForStep2Completed += deviceInstallServiceClient_SubmitGPSForStep2Completed;
                vehicleServiceClient.CheckInstallVehicleForGPSCompleted += vehicleServiceClient_CheckInstallVehicleForGPSCompleted;
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("InputDeviceInfoViewModel()", ex);
            }
        }

        void devGPSServiceClient_GetDevGpsBySNCompleted(object sender, GetDevGpsBySNCompletedEventArgs e)
        {
            try
            {
                if (e.Result.IsSuccess)
                {
                    if (e.Result.Result != null)
                    {
                        CurrentModel = e.Result.Result;
                        if (CurrentModel.Status == (short)DeviceSuiteStatus.Initial)
                        {
                            IsFinished = true;
                        }
                        else if (CurrentModel.Status == (short)DeviceSuiteStatus.Working)
                        {
                            IsFinished = false;

                            MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString("Tip"), ApplicationContext.Instance.StringResourceReader.GetString("ID_INSTALL_SuiteAlreadyInstalled"), MessageDialogButton.Ok);


                        }
                        else if (CurrentModel.Status == (short)DeviceSuiteStatus.Testing)
                        {
                            IsFinished = false;

                            MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString("Tip"), ApplicationContext.Instance.StringResourceReader.GetString("ID_INSTALL_SuiteTesting"), MessageDialogButton.Ok);
                        }
                        else if (CurrentModel.Status == (short)DeviceSuiteStatus.Running)
                        {
                            IsFinished = false;

                            MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString("Tip"), ApplicationContext.Instance.StringResourceReader.GetString("ID_INSTALL_SuiteAlreadyInstalled"), MessageDialogButton.Ok);
                        }
                        else if (CurrentModel.Status == (short)DeviceSuiteStatus.Abnormal)
                        {
                            IsFinished = false;

                            MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString("Tip"), ApplicationContext.Instance.StringResourceReader.GetString("ID_INSTALL_SuiteAbnormal"), MessageDialogButton.Ok);

                        }

                        else if (CurrentModel.Status == (short)DeviceSuiteStatus.WaitingMaintenance)
                        {
                            IsFinished = false;

                            MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString("Tip"), ApplicationContext.Instance.StringResourceReader.GetString("ID_INSTALL_SuiteWaitingMaintenance"), MessageDialogButton.Ok);
                        }
                        else if (CurrentModel.Status == (short)DeviceSuiteStatus.Maintenance)
                        {
                            IsFinished = false;

                            MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString("Tip"), ApplicationContext.Instance.StringResourceReader.GetString("ID_INSTALL_SuiteMaintenance"), MessageDialogButton.Ok);
                        }
                        else if (CurrentModel.Status == (short)DeviceSuiteStatus.Scrap)
                        {
                            IsFinished = false;

                            MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString("Tip"), ApplicationContext.Instance.StringResourceReader.GetString("ID_INSTALL_SuiteScrap"), MessageDialogButton.Ok);
                        }
                        else if (CurrentModel.Status == (short)DeviceSuiteStatus.History)
                        {
                            IsFinished = false;

                            MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString("Tip"), ApplicationContext.Instance.StringResourceReader.GetString("ID_INSTALL_SuiteHistory"), MessageDialogButton.Ok);
                        }
                        else
                        {
                            IsFinished = false;
                        }
                    }
                    else
                    {
                        //套件信息获取失败,此处要修改成定位设备信息获取失败
                        IsFinished = false;

                        MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString("Tip"), ApplicationContext.Instance.StringResourceReader.GetString("LocateDeviceGetInfoFaulted"), MessageDialogButton.Ok);
                    }
                }
                else if (string.IsNullOrEmpty(e.Result.ErrorMsg) == false)
                {
                    IsFinished = false;
                    MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString(e.Result.ErrorMsg), MessageDialogButton.Ok);
                }
                else
                {
                    IsFinished = false;

                    MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString("Tip"), ApplicationContext.Instance.StringResourceReader.GetString("ID_INSTALL_ServiceError"), MessageDialogButton.Ok);
                }
            }
            catch (Exception ex)
            {
                //套件信息获取失败,此处要修改成定位设备信息获取失败
                ApplicationContext.Instance.Logger.LogException("securitySuiteServiceClient_GetSecuritySuiteBySuiteIdCompleted", ex);

                MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString("Tip"), ApplicationContext.Instance.StringResourceReader.GetString("LocateDeviceGetInfoFaulted"), MessageDialogButton.Ok);

            }
        }

        protected override void Get()
        {
            vehicleServiceClient.CheckInstallVehicleForGPSAsync(InstallInfo.VehicleID, ApplicationContext.Instance.AuthenticationInfo.ClientID);
        }

        void vehicleServiceClient_CheckInstallVehicleForGPSCompleted(object sender, CheckInstallVehicleForGPSCompletedEventArgs e)
        {
            try
            {
                organizationid = e.Result.Result.OrganizationID;
                devGPSServiceClient.GetDevGpsBySNAsync(GPSSn);
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("vehicleServiceClient_CheckInstallVehicleCompleted", ex);
            }
        }


        protected override void NextPage()
        {
            try
            {
                IsFinished = false;
                Step2Package package = new Step2Package();
                package.SuiteId = CurrentModel.GpsSn;
                package.SuiteKey = CurrentModel.ID;
                package.VehicleId = InstallInfo.VehicleID;
                package.MDVR_CORE_SN = CurrentModel.GpsUid;
                package.SuiteStatus = (int)DeviceSuiteStatus.Testing;
                package.InstallID = _InstallID;
                package.SelfInspectCheck = -1;
                package.AlarmCheck = -1;
                package.GpsCheck = -1;
                package.VideoCheck = -1;
                package.VideoQualityCheck = -1;
                package.IsSuccess = -1;
                package.SuiteStatus = (int)DeviceSuiteStatus.Working;
                package.SuiteSwitchTime = DateTime.Now.ToUniversalTime();
                package.Organization = organizationid;
                package.ClientID = ApplicationContext.Instance.AuthenticationInfo.ClientID;

                deviceInstallServiceClient.SubmitGPSForStep2Async(package);
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("InputDeviceInfoViewModel NextPage", ex);
            }
        }

        void deviceInstallServiceClient_SubmitGPSForStep2Completed(object sender, SubmitGPSForStep2CompletedEventArgs e)
        {
            if (e.Result.IsSuccess)
            {
                try
                {
                    ApplicationContext.Instance.MessageClient.BeginInstallGPS(CurrentModel.GpsUid);
                }
                catch (Exception ex)
                {
                    ApplicationContext.Instance.Logger.LogException("deviceInstallServiceClient_SubmitForStep2Completed", ex);
                }

                EventAggregator.Publish(new ViewNavigationArgs(InstallationName.InstallVehcileGPSCheckV, new Dictionary<string, object>() { { "ID", _InstallID } }));
            }
            else
            {
                MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString("Tip"), ApplicationContext.Instance.StringResourceReader.GetString(e.Result.Result), MessageDialogButton.Ok);
            }
        }

        protected override void ResetData()
        {
            try
            {
                CurrentModel.GpsSn = string.Empty;
                CurrentModel.GpsUid = string.Empty;
                CurrentModel.GpsSim = string.Empty;
                organizationid = string.Empty;
                GPSSn = string.Empty;

                IsFinished = false;
                IsGetMessage = false;

                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => CurrentModel));
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("InputDeviceInfoViewModel ResetData", ex);
            }
        }
    }
}
