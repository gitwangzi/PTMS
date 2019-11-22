/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 9b1eda98-b0f7-41a4-aa35-fef8305d5902      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: GAOZITIAN-PC
/////                 Author: TEST(gaozt)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Installation.ViewModels
/////    Project Description:    
/////             Class Name: UploadCarNumberViewModel
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/8/16 16:21:52
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/8/16 16:21:52
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
using Jounce.Framework.Command;
using Jounce.Core.Command;
using Jounce.Core.View;
using Gsafety.PTMS.ServiceReference.DeviceInstallService;
using Gsafety.PTMS.Share;
using System.Collections.Generic;
using Gsafety.PTMS.ServiceReference.SecuritySuiteService;
using Gsafety.PTMS.ServiceReference.WorkingSuiteService;
using System.Reflection;

namespace Gsafety.PTMS.Installation.ViewModels
{
    [ExportAsViewModel(InstallationName.UploadCarNumberVm)]
    public class UploadCarNumberViewModel : InstallViewModelBase
    {

        string _CarNumber = string.Empty;
        public string CarNumber
        {
            get
            {
                return _CarNumber;
            }
            set
            {
                _CarNumber = value;
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => CarNumber));
            }
        }

        SecuritySuiteServiceClient securitySuiteServiceClient = null;
        WorkingSuiteServiceClient workingSuiteServiceClient = null;

        public UploadCarNumberViewModel()
        {           
            try
            {
                step = 3;
                ImageSource = "Step03.png";
                securitySuiteServiceClient = ServiceClientFactory.Create<SecuritySuiteServiceClient>();
                workingSuiteServiceClient = ServiceClientFactory.Create<WorkingSuiteServiceClient>();
                deviceInstallServiceClient.GetInstallingSuiteVehicleIdCompleted += deviceinstallserviceClient_GetInstallingSuiteVehicleIdCompleted;
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("UploadCarNumberViewModel()", ex);
            }            
        }

        protected override void Get()
        {
            deviceInstallServiceClient.GetInstallingSuiteVehicleIdAsync(InstallInfoResultInfo.MdvrCoreId);
        }

        void deviceinstallserviceClient_GetInstallingSuiteVehicleIdCompleted(object sender, GetInstallingSuiteVehicleIdCompletedEventArgs e)
        {
            try
            {
                if (e.Result.IsSuccess)
                {
                    if (e.Result.Result != null)
                    {
                        CarNumber = e.Result.Result;
                        if (CarNumber.Equals(InstallInfoResultInfo.VehicleID))
                        {
                            IsFinished = true;
                            IsMaintenance = false;
                        }
                        else
                        {
                            IsFinished = false;
                            IsMaintenance = true;
                            MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("ID_INSTALL_VehicleIDWrong"), ApplicationContext.Instance.StringResourceReader.GetString("Tip"), MessageBoxButton.OK);
                        }
                    }
                    else
                    {
                        IsMaintenance = true;
                        MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("ID_INSTALL_GetInstallingSuiteVehicleIdFailed"), ApplicationContext.Instance.StringResourceReader.GetString("Tip"), MessageBoxButton.OK);
                    }
                }
                else
                {
                    MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("ID_INSTALL_ServiceError"), ApplicationContext.Instance.StringResourceReader.GetString("Tip"), MessageBoxButton.OK);
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("deviceinstallserviceClient_GetInstallingSuiteVehicleIdCompleted", ex);
            }
        }

        protected override void NextPage()
        {
            try
            {
                //TODO
                InstallationInfo installDetailModel = new InstallationInfo()
                {
                    /// ID
                    Id = _InstallID,
                    /// The current installation steps
                    CheckStep = 3
                };
                deviceInstallServiceClient.UpdateInstallationAsync(installDetailModel);
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("UploadCarNumberViewModel NextPage",ex);
            }
        }

        protected override void GoNextPage()
        {
            EventAggregator.Publish(new ViewNavigationArgs(InstallationName.DeviceSelftestV, new Dictionary<string, object>() { { "ID", _InstallID } }));
        }

        protected override void Quit()
        {
            var result = MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("ID_INSTALL_IfCancelInstall"), ApplicationContext.Instance.StringResourceReader.GetString("Tip"), MessageBoxButton.OKCancel);
            if (result == MessageBoxResult.OK)
            {
                ResetData();
                EventAggregator.Publish(new ViewNavigationArgs(InstallationName.DeviceInstallV));
            }
        }

        protected override void ResetData()
        {
            try
            {
                CarNumber = string.Empty;
                IsGetMessage = true;
                IsFinished = false;
                IsMaintenance = false;
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("UploadCarNumberViewModel ResetData", ex);
            }
        }
    }
}
