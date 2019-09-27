/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: a6e58a11-abbe-4794-82ee-222bb6a11a76      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: GAOZITIAN-PC
/////                 Author: TEST(gaozt)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Installation.ViewModels
/////    Project Description:    
/////             Class Name: CheckDeviceFunctionViewModel
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/8/17 15:26:35
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/8/17 15:26:35
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
using Gsafety.PTMS.ServiceReference.AppConfigService;
using Gsafety.PTMS.ServiceReference.DeviceInstallService;
using Gsafety.PTMS.ServiceReference.SecuritySuiteService;
using Gsafety.PTMS.ServiceReference.VedioService;
using Gsafety.PTMS.ServiceReference.WorkingSuiteService;
using Gsafety.PTMS.Share;
using Jounce.Core.Command;
using Jounce.Core.View;
using Jounce.Core.ViewModel;
using Jounce.Framework.Command;
using System.Reflection;

namespace Gsafety.PTMS.Installation.ViewModels
{
    [ExportAsViewModel(InstallationName.CheckDeviceFunctionVm)]
    public class CheckDeviceFunctionViewModel : InstallViewModelBase
    {
        InstallationAudit _InstallationAudit = null;
        public InstallationAudit InstallationAuditInfo
        {
            get { return _InstallationAudit; }
            private set
            {
                _InstallationAudit = value;
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => InstallationAuditInfo));
            }
        }

        private int validtime = 0;

        VedioServiceClient vedioServiceClient = null;

        AppConfigManagerClient appConfigManagerClient = null;
        public CheckDeviceFunctionViewModel()
        {
            step = 5;
            try
            {
                vedioServiceClient = ServiceClientFactory.Create<VedioServiceClient>();
                appConfigManagerClient = ServiceClientFactory.Create<AppConfigManagerClient>();
                ImageSource = "Step05.png";
                deviceInstallServiceClient.GetInstallationResultCompleted += deviceinstallserviceClient_GetInstallationResultCompleted;
                vedioServiceClient.CheckAlarmVideoCompleted += vedioServiceClient_CheckAlarmVideoCompleted;
                appConfigManagerClient.GetConfigInfoBySectionNameCompleted += appConfigManagerClient_GetConfigInfoBySectionNameCompleted;
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("CheckDeviceFunctionViewModel()", ex);
            }          
        }


        protected override void ActivateView(string viewName, System.Collections.Generic.IDictionary<string, object> viewParameters)
        {
            base.ActivateView(viewName, viewParameters);

            appConfigManagerClient.GetConfigInfoBySectionNameAsync("AlarmVideoTime");
        }

        void appConfigManagerClient_GetConfigInfoBySectionNameCompleted(object sender, GetConfigInfoBySectionNameCompletedEventArgs e)
        {
            try
            {
                if (e.Error != null)
                {
                    return;
                }
                if (e.Result.Result != null)
                {
                    validtime = int.Parse(e.Result.Result.SECTION_VALUE);
                }
                else
                {
                    validtime = 1;
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("appConfigManagerClient_GetConfigInfoBySectionNameCompleted", ex);
                validtime = 1;
            }

        }

        protected override void deviceinstallserviceClient_GetInstallationDetailCompleted(object sender, GetInstallationDetailCompletedEventArgs e)
        {
            try
            {
                if (e.Result.IsSuccess)
                {
                    if (e.Result.Result != null)
                    {
                        InstallInfoResultInfo = e.Result.Result;
                    }
                    else
                    {
                        MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("ID_INSTALL_GetInstallationDetailFailed"), ApplicationContext.Instance.StringResourceReader.GetString("Tip"), MessageBoxButton.OK);
                    }
                }
                else
                {
                    MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("ID_INSTALL_ServiceError"), ApplicationContext.Instance.StringResourceReader.GetString("Tip"), MessageBoxButton.OK);
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("deviceinstallserviceClient_GetInstallationDetailCompleted", ex);
            }
        }

        protected override void Get()
        {
            CheckAlarmVideoArgs arg = new CheckAlarmVideoArgs();
            arg.Alarm_Id = _InstallID;
            arg.Date = DateTime.Now.AddHours(validtime);

            vedioServiceClient.CheckAlarmVideoAsync(arg);
        }

        void vedioServiceClient_CheckAlarmVideoCompleted(object sender, CheckAlarmVideoCompletedEventArgs e)
        {
            try
            {
                if (e.Result.IsSuccess)
                {
                    deviceInstallServiceClient.GetInstallationResultAsync(_InstallID);
                }
                else
                {
                    MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("ID_INSTALL_ServiceError"), ApplicationContext.Instance.StringResourceReader.GetString("Tip"), MessageBoxButton.OK);
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("vedioServiceClient_CheckAlarmVideoCompleted", ex);
            }
        }

        void deviceinstallserviceClient_GetInstallationResultCompleted(object sender, GetInstallationResultCompletedEventArgs e)
        {
            try
            {
                if (e.Result.IsSuccess)
                {
                    if (e.Result.Result.Audit != null)
                    {
                        InstallationAuditInfo = e.Result.Result.Audit;
                        if ((InstallationAuditInfo.AlarmCheck == 1) && (InstallationAuditInfo.GpsCheck == 1) && (InstallationAuditInfo.VideoCheck == 1))
                        {
                            IsFinished = true;
                            IsMaintenance = false;
                        }
                        else
                        {
                            IsFinished = false;
                            IsMaintenance = true;
                        }
                    }
                    else
                    {
                        IsMaintenance = true;
                    }
                }
                else
                {
                    MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("ID_INSTALL_ServiceError"), ApplicationContext.Instance.StringResourceReader.GetString("Tip"), MessageBoxButton.OK);
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("deviceinstallserviceClient_GetInstallationResultCompleted", ex);
            }
        }



        protected override void Quit()
        {
            var result = MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("ID_INSTALL_IfQuitInstall"), ApplicationContext.Instance.StringResourceReader.GetString("Tip"), MessageBoxButton.OKCancel);
            if (result == MessageBoxResult.OK)
            {
                ResetData();
                EventAggregator.Publish(new ViewNavigationArgs(InstallationName.DeviceInstallV));
            }
        }

        protected override void NextPage()
        {
            //TODO
            InstallationInfo installDetailModel = new InstallationInfo()
            {
                /// ID
                Id = _InstallID,
                /// The current installation steps
                CheckStep = 5
            };
            deviceInstallServiceClient.UpdateInstallationAsync(installDetailModel);
        }


        protected override void GoNextPage()
        {
            EventAggregator.Publish(new ViewNavigationArgs(InstallationName.UploadPictureAndConfirmV, new Dictionary<string, object>() { { "ID", _InstallID } }));
        }

        protected override void ResetData()
        {
            try
            {
                InstallationAuditInfo.AlarmCheck = 2;
                InstallationAuditInfo.GpsCheck = 2;
                InstallationAuditInfo.VideoCheck = 2;
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => InstallationAuditInfo));
                IsMaintenance = false;
                IsFinished = false;
                IsGetMessage = true;
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("CheckDeviceFunctionViewModel ResetData", ex);
            }
        }
    }
}
