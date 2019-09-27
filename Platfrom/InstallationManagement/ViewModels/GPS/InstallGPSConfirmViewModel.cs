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
using Gsafety.Ant.Installation.ViewModels;
using Gsafety.PTMS.ServiceReference.DevGpsService;

namespace Gsafety.PTMS.Installation.ViewModels
{
    [ExportAsViewModel(InstallationName.InstallGPSConfirmVm)]
    public class InstallGPSConfirmViewModel : InstallGPSViewModelBase
    {
        public IActionCommand OKCommand { get; private set; }

        DevGpsServiceClient devGPSServiceClient = null;

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

        public InstallGPSConfirmViewModel()
            : base()
        {
            try
            {
                devGPSServiceClient = ServiceClientFactory.Create<DevGpsServiceClient>();
                devGPSServiceClient.GetDevGpsCompleted += devGPSServiceClient_GetDevGpsCompleted;
                ImageSource = "GpsSetp4.png";
                OKCommand = new ActionCommand<object>(obj => OkAction());
                deviceInstallServiceClient.SubmitGPSForStep4Completed += deviceinstallserviceClient_SubmitGPSForStep4Completed;
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("UploadPictureAndConfirmViewModel()", ex);
            }
        }

        void devGPSServiceClient_GetDevGpsCompleted(object sender, GetDevGpsCompletedEventArgs e)
        {
            try
            {
                if (e.Error == null)
                {
                    CurrentModel = e.Result.Result;
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }

        void deviceinstallserviceClient_SubmitGPSForStep4Completed(object sender, SubmitGPSForStep4CompletedEventArgs e)
        {
            try
            {
                if (e.Error == null)
                {
                    ResetData();
                    ApplicationContext.Instance.MessageClient.CompleteInstallGPS(InstallInfo.DeviceCoreId, InstallInfo.Organization, InstallInfo.VehicleID);
                    EventAggregator.Publish(new ViewNavigationArgs(InstallationName.InstallGPSVehicleCheckV));
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }

        private void OkAction()
        {
            deviceInstallServiceClient.SubmitGPSForStep4Async(_InstallID);
        }

        protected override void OnGetDetailComplete()
        {
            devGPSServiceClient.GetDevGpsAsync(InstallInfo.DeviceKey);
        }

        protected override void ResetData()
        {
            try
            {
                InstallInfo = new InstallationInfo();

                CurrentModel = new DevGps();
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("UploadPictureAndConfirmViewModel", ex);
            }
        }
    }
}
