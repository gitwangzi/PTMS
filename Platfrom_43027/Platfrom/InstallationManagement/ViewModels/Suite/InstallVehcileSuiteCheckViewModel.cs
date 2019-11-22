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
using System.Windows.Threading;
using Gsafety.PTMS.ServiceReference.VehicleMonitorService;
using Gsafety.Ant.Installation.ViewModels;
using Gsafety.Common.Controls;

namespace Gsafety.PTMS.Installation.ViewModels
{
    [ExportAsViewModel(InstallationName.InstallVehcileSuiteCheckVm)]
    public class InstallVehcileSuiteCheckViewModel : InstallSuiteViewModelBase
    {
        DispatcherTimer StatuTimer = null;

        VehicleMonitorServiceClient monitorServiceClient = null;
        public InstallVehcileSuiteCheckViewModel()
        {
            try
            {
                step = 3;
                ImageSource = "Step03.png";
                monitorServiceClient = ServiceClientFactory.Create<VehicleMonitorServiceClient>();
                monitorServiceClient.ValidateSuiteGPSCompleted += monitorServiceClient_ValidateSuiteGPSCompleted;

                StatuTimer = new DispatcherTimer();
                StatuTimer.Interval = TimeSpan.FromMilliseconds(3000);
                StatuTimer.Tick += _statuTimer_Tick;

            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("UploadCarNumberViewModel()", ex);
            }
        }

        protected override void ActivateView(string viewName, IDictionary<string, object> viewParameters)
        {
            try
            {
                base.ActivateView(viewName, viewParameters);
                StatuTimer.Start();
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }

        void monitorServiceClient_ValidateSuiteGPSCompleted(object sender, ValidateSuiteGPSCompletedEventArgs e)
        {
            try
            {
                if (e.Error == null)
                {
                    if (e.Result.IsSuccess)
                    {
                        if (e.Result.Result)
                        {
                            StopTimer();
                            NextPage();
                        }
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(e.Result.ErrorMsg))
                        {
                            MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString(LProxy.Caption), ApplicationContext.Instance.StringResourceReader.GetString(LProxy.OperatorServiceError));
                        }
                        else
                        {
                            MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString(LProxy.Caption), ApplicationContext.Instance.StringResourceReader.GetString(e.Result.ErrorMsg));
                        }
                    }
                }
                else
                {
                    MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString(LProxy.Caption), ApplicationContext.Instance.StringResourceReader.GetString(LProxy.ServerError));
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }

        private void _statuTimer_Tick(object sender, EventArgs e)
        {
            if (this.InstallInfo != null)
            {
                monitorServiceClient.ValidateSuiteGPSAsync(InstallInfo.VehicleID, InstallInfo.DeviceCoreId);
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
                ApplicationContext.Instance.Logger.LogException("UploadCarNumberViewModel NextPage", ex);
            }
        }

        protected override void GoNextPage()
        {
            EventAggregator.Publish(new ViewNavigationArgs(InstallationName.InstallInitiateSuiteV, new Dictionary<string, object>() { { "ID", _InstallID } }));
        }

        protected override void ResetData()
        {
            try
            {
                IsGetMessage = true;
                IsFinished = false;
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("UploadCarNumberViewModel ResetData", ex);
            }
        }

        protected override void DeactivateView(string viewName)
        {
            try
            {
                base.DeactivateView(viewName);
                StopTimer();
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }

        private void StopTimer()
        {
            if (StatuTimer != null && StatuTimer.IsEnabled)
                StatuTimer.Stop();
        }
    }
}
