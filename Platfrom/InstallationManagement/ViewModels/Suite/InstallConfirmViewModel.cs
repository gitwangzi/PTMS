using Gsafety.Ant.Installation.ViewModels;
using Gsafety.Common.Controls;
using Gsafety.PTMS.ServiceReference.DeviceAlertService;
using Gsafety.PTMS.ServiceReference.DeviceInstallService;
using Gsafety.PTMS.Share;
using Jounce.Core.Command;
using Jounce.Core.View;
using Jounce.Core.ViewModel;
using Jounce.Framework.Command;
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
using System.Collections.ObjectModel;
using System.Reflection;

namespace Gsafety.PTMS.Installation.ViewModels
{
    [ExportAsViewModel(InstallationName.InstallConfirmVm)]
    public class InstallConfirmViewModel : InstallSuiteViewModelBase
    {
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

        public ObservableCollection<DeviceAlertEx> DeviceAlerts { get; set; }

        public IActionCommand OKCommand { get; private set; }
        DeviceAlertServiceClient deviceAlertClient = null;
        public InstallConfirmViewModel()
        {
            try
            {
                deviceAlertClient = ServiceClientFactory.Create<DeviceAlertServiceClient>();

                ImageSource = "Step06.png";
                OKCommand = new ActionCommand<object>(obj => OkAction());

                deviceInstallServiceClient.GetInstallationResultCompleted += deviceinstallserviceClient_GetInstallationResultCompleted;
                deviceInstallServiceClient.SubmitForStep6Completed += deviceinstallserviceClient_SubmitForStep6Completed;
                deviceAlertClient.GetDeviceAlertEx1Completed += deviceAlertClient_GetDeviceAlertEx1Completed;
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("UploadPictureAndConfirmViewModel()", ex);
            }
        }

        void deviceAlertClient_GetDeviceAlertEx1Completed(object sender, GetDeviceAlertEx1CompletedEventArgs e)
        {
            try
            {
                if (e.Cancelled == true)
                {
                    return;
                }

                if (e.Error != null)
                {
                    MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString(LProxy.Caption), ApplicationContext.Instance.StringResourceReader.GetString(LProxy.OperatorServiceError), MessageDialogButton.Ok);

                    ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), e.Error);
                    return;
                }

                var result = e.Result;
                if (result.IsSuccess == false)
                {
                    if (string.IsNullOrWhiteSpace(result.ErrorMsg) == false)
                    {
                        MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString(LProxy.Caption), ApplicationContext.Instance.StringResourceReader.GetString(result.ErrorMsg), MessageDialogButton.Ok);

                        ApplicationContext.Instance.Logger.LogError(MethodBase.GetCurrentMethod().ToString(), result.ErrorMsg);
                    }

                    if (result.ExceptionMessage != null)
                    {
                        ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), result.ExceptionMessage);
                    }
                }
                else
                {
                    DeviceAlerts = result.Result;
                    //foreach (var item in DeviceAlerts)
                    //{
                    //    switch ((int)item.AlertType)
                    //    {
                    //        case -1:
                    //            item.ShowType = ApplicationContext.Instance.StringResourceReader.GetString("MAINTAIN_Full_N");
                    //            break;
                    //        case 0:
                    //            item.ShowType = ApplicationContext.Instance.StringResourceReader.GetString("GNSSModelError");
                    //            break;
                    //        case 1:
                    //            item.ShowType = ApplicationContext.Instance.StringResourceReader.GetString("GNSSNoAntenna");
                    //            break;
                    //        case 2:
                    //            item.ShowType = ApplicationContext.Instance.StringResourceReader.GetString("GNSSCircuit");
                    //            break;
                    //        case 3:
                    //            item.ShowType = ApplicationContext.Instance.StringResourceReader.GetString("PowerSourceNoVoltage");
                    //            break;
                    //        case 4:
                    //            item.ShowType = ApplicationContext.Instance.StringResourceReader.GetString("PowerSourceNoPower");
                    //            break;
                    //        case 5:
                    //            item.ShowType = ApplicationContext.Instance.StringResourceReader.GetString("LEDError");
                    //            break;
                    //        case 6:
                    //            item.ShowType = ApplicationContext.Instance.StringResourceReader.GetString("TTSError");
                    //            break;
                    //        case 7:
                    //            item.ShowType = ApplicationContext.Instance.StringResourceReader.GetString("VidiconError");
                    //            break;
                    //        default:
                    //            item.ShowType = ApplicationContext.Instance.StringResourceReader.GetString("MAINTAIN_Full_N");
                    //            break;
                    //    }

                    //    item.AlertTime = item.AlertTime.Value.ToLocalTime();
                    //}

                    RaisePropertyChanged(() => DeviceAlerts);
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }

        protected override void ActivateView(string viewName, System.Collections.Generic.IDictionary<string, object> viewParameters)
        {
            if (viewParameters != null && viewParameters.Count > 0 && viewParameters.Keys.Contains("ID"))
            {
                _InstallID = viewParameters["ID"].ToString();
                ResetData();
                deviceInstallServiceClient.GetInstallationResultAsync(_InstallID);
            }
        }

        void deviceinstallserviceClient_GetInstallationResultCompleted(object sender, GetInstallationResultCompletedEventArgs e)
        {
            try
            {
                if (e.Result.IsSuccess)
                {
                    if (e.Result.Result.Installation != null)
                    {
                        InstallInfo = e.Result.Result.Installation;
                        if (InstallInfo.CreateTime.HasValue)
                        {
                            InstallInfo.CreateTime = InstallInfo.CreateTime.Value.ToLocalTime();
                        }
                    }
                    else
                    {
                        //MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("ID_INSTALL_GetInstallationFailed"), ApplicationContext.Instance.StringResourceReader.GetString("Tip"), MessageBoxButton.OK);
                        MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString("Tip"), ApplicationContext.Instance.StringResourceReader.GetString("ID_INSTALL_GetInstallationFailed"), MessageDialogButton.Ok);
                    }

                    if (e.Result.Result.Audit != null)
                    {
                        AuditInfo = e.Result.Result.Audit;
                    }
                    else
                    {
                        //MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("ID_INSTALL_GetAuditFailed"), ApplicationContext.Instance.StringResourceReader.GetString("Tip"), MessageBoxButton.OK);
                        MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString("Tip"), ApplicationContext.Instance.StringResourceReader.GetString("ID_INSTALL_GetAuditFailed"), MessageDialogButton.Ok);
                    }

                    var time = DateTime.UtcNow;
                    var page = new Gsafety.PTMS.ServiceReference.DeviceAlertService.PagingInfo()
                    {
                        PageIndex = 1,
                        PageSize = -1,
                    };
                    deviceAlertClient.GetDeviceAlertEx1Async(InstallInfo.VehicleID, InstallInfo.DeviceCoreId, null, time.AddHours(-1), time, page);
                }
                else
                {
                    //MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("ID_INSTALL_ServiceError"), ApplicationContext.Instance.StringResourceReader.GetString("Tip"), MessageBoxButton.OK);
                    MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString("Tip"), ApplicationContext.Instance.StringResourceReader.GetString("ID_INSTALL_ServiceError"), MessageDialogButton.Ok);
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("UploadPictureAndConfirmViewModel", ex);
            }
        }

        private void OkAction()
        {
            try
            {
                Step6Package package = new Step6Package();
                package.Id = _InstallID;
                package.FinishTime = DateTime.Now.ToUniversalTime();
                package.CheckStep = 7;
                package.MdvrCoreId = InstallInfo.DeviceCoreId;
                package.Status = (short)DeviceSuiteStatus.Running;

                deviceInstallServiceClient.SubmitForStep6Async(package);
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
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
                        ApplicationContext.Instance.MessageClient.CompleteInstallSuite(InstallInfo.DeviceCoreId, InstallInfo.Organization, InstallInfo.VehicleID);

                        ResetData();
                        EventAggregator.Publish(new ViewNavigationArgs(InstallationName.InstallVehicleCheckV));
                    }
                    else
                    {
                        //MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("ID_INSTALL_AddInstallationFailed"), ApplicationContext.Instance.StringResourceReader.GetString("Tip"), MessageBoxButton.OK);
                        MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString("Tip"), ApplicationContext.Instance.StringResourceReader.GetString("ID_INSTALL_AddInstallationFailed"), MessageDialogButton.Ok);
                    }
                }
                else
                {
                    //MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("ID_INSTALL_ServiceError"), ApplicationContext.Instance.StringResourceReader.GetString("Tip"), MessageBoxButton.OK);
                    MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString("Tip"), ApplicationContext.Instance.StringResourceReader.GetString("ID_INSTALL_ServiceError"), MessageDialogButton.Ok);
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("UploadPictureAndConfirmViewModel", ex);
            }
        }

        protected override void ResetData()
        {
            try
            {
                base.ResetData();
                AuditInfo = null;
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("UploadPictureAndConfirmViewModel", ex);
            }
        }
    }
}
