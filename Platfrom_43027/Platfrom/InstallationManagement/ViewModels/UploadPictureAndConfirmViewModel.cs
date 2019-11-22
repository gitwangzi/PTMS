/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 4e08267f-a466-4be8-a27b-c3dde5a74e1b      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: GAOZITIAN-PC
/////                 Author: TEST(gaozt)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Installation.ViewModels
/////    Project Description:    
/////             Class Name: UploadPictureAndConfirmViewModel
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/8/16 16:22:15
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/8/16 16:22:15
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
using Jounce.Core.ViewModel;
using Jounce.Core.Command;
using Gsafety.PTMS.ServiceReference.DeviceInstallService;
using Gsafety.PTMS.Share;
using Jounce.Framework.Command;
using Jounce.Core.View;
using Gsafety.PTMS.ServiceReference.WorkingSuiteService;
using Gsafety.PTMS.ServiceReference.SecuritySuiteService;
using Gsafety.PTMS.ServiceReference.MessageService;
using System.Reflection;

namespace Gsafety.PTMS.Installation.ViewModels
{
    [ExportAsViewModel(InstallationName.UploadPictureAndConfirmVm)]
    public class UploadPictureAndConfirmViewModel : BaseViewModel
    {
        string _InstallID = string.Empty;
        string _ImageSource = null;
        public string ImageSource
        {
            get
            {
                return _ImageSource;
            }
            set
            {
                _ImageSource = value;
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => ImageSource));
            }
        }
        InstallationInfo _InstallInfo = null;
        public InstallationInfo InstallInfo
        {
            get { return _InstallInfo; }
            private set
            {
                _InstallInfo = value;
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => InstallInfo));
            }
        }

        InstallationAudit _AuditInfo = null;
        public InstallationAudit AuditInfo
        {
            get
            {
                return _AuditInfo;
            }
            set
            {
                _AuditInfo = value;
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => AuditInfo));
            }
        }



        public IActionCommand OKCommand { get; private set; }
        DeviceInstallServiceClient deviceinstallserviceClient = null;
        public UploadPictureAndConfirmViewModel()
        {
            try
            {
                deviceinstallserviceClient = ServiceClientFactory.Create<DeviceInstallServiceClient>();
                ImageSource = "Step06.png";
                OKCommand = new ActionCommand<object>(obj => OkAction());
                deviceinstallserviceClient.GetInstallationResultCompleted += deviceinstallserviceClient_GetInstallationResultCompleted;
                deviceinstallserviceClient.SubmitForStep6Completed += deviceinstallserviceClient_SubmitForStep6Completed;
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("UploadPictureAndConfirmViewModel()", ex);
            }            
        }


        private string MdvrId;


        void deviceinstallserviceClient_GetInstallationResultCompleted(object sender, GetInstallationResultCompletedEventArgs e)
        {
            try
            {
                if (e.Result.IsSuccess)
                {
                    if (e.Result.Result.Installation != null)
                    {
                        InstallInfo = e.Result.Result.Installation;
                        MdvrId = InstallInfo.MdvrCoreId;


                        Step6Package package = new Step6Package();
                        package.Id = _InstallID;
                        package.FinishTime = DateTime.Now;
                        package.CheckStep = 7;
                        package.MdvrCoreId = InstallInfo.MdvrCoreId;
                        package.Status = (short)DeviceSuiteStatus.Running;

                        deviceinstallserviceClient.SubmitForStep6Async(package);
                    }
                    else
                    {
                        MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("ID_INSTALL_GetInstallationFailed"), ApplicationContext.Instance.StringResourceReader.GetString("Tip"), MessageBoxButton.OK);
                    }
                    if (e.Result.Result.Audit != null)
                    {
                        AuditInfo = e.Result.Result.Audit;
                    }
                    else
                    {
                        MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("ID_INSTALL_GetAuditFailed"), ApplicationContext.Instance.StringResourceReader.GetString("Tip"), MessageBoxButton.OK);
                    }
                }
                else
                {
                    MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("ID_INSTALL_ServiceError"), ApplicationContext.Instance.StringResourceReader.GetString("Tip"), MessageBoxButton.OK);
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("UploadPictureAndConfirmViewModel", ex);
            }
        }

        void deviceinstallserviceClient_SubmitForStep6Completed(object sender, SubmitForStep6CompletedEventArgs e)
        {
            try
            {
                if (e.Result.IsSuccess)
                {
                    if (string.IsNullOrEmpty(e.Result.Result))
                    {
                        ApplicationContext.Instance.MessageManager.SendDeviceInstallMessage(new DeviceInstall() { MdvrCoreId = MdvrId, InstallCompleteTime = DateTime.Now });
                    }
                    else
                    {

                        MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("ID_INSTALL_AddInstallationFailed"), ApplicationContext.Instance.StringResourceReader.GetString("Tip"), MessageBoxButton.OK);
                    }
                }
                else
                {
                    MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("ID_INSTALL_ServiceError"), ApplicationContext.Instance.StringResourceReader.GetString("Tip"), MessageBoxButton.OK);
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("UploadPictureAndConfirmViewModel", ex);
            }
        }

        protected override void ActivateView(string viewName, System.Collections.Generic.IDictionary<string, object> viewParameters)
        {
            if (viewParameters != null && viewParameters.Count > 0 && viewParameters.Keys.Contains("ID"))
            {
                _InstallID = viewParameters["ID"].ToString();
                ResetData();
                deviceinstallserviceClient.GetInstallationResultAsync(_InstallID);
            }
        }

        private void OkAction()
        {
            ResetData();
            EventAggregator.Publish(new ViewNavigationArgs(InstallationName.DeviceInstallV));
        }

        private void ResetData()
        {
            try
            {
                InstallInfo.VehicleID = string.Empty;
                //InstallInfo.CompanyName = string.Empty;
                InstallInfo.InstallationStationName = string.Empty;
                InstallInfo.InstallationStaff = string.Empty;
                InstallInfo.CreateTime = null;
                InstallInfo.SuiteID = string.Empty;
                InstallInfo.MdvrCoreId = string.Empty;
                AuditInfo.SelfInspectCheck = 2;
                AuditInfo.AlarmCheck = 2;
                AuditInfo.GpsCheck = 2;
                AuditInfo.VideoCheck = 2;
                Jounce.Framework.JounceHelper.ExecuteOnUI(() =>
                {
                    RaisePropertyChanged(() => InstallInfo);
                    RaisePropertyChanged(() => AuditInfo);
                });
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("UploadPictureAndConfirmViewModel", ex);
            }
        }
    }
}
