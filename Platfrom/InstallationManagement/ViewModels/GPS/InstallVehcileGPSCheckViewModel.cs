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

namespace Gsafety.PTMS.Installation.ViewModels
{
    [ExportAsViewModel(InstallationName.InstallVehcileGPSCheckVm)]
    public class InstallVehcileGPSCheckViewModel : InstallGPSViewModelBase
    {
        DispatcherTimer StatuTimer = null;
        WorkingSuiteServiceClient workingSuiteServiceClient = null;
        VehicleMonitorServiceClient monitorServiceClient = null;
        public InstallVehcileGPSCheckViewModel()
        {
            try
            {
                step = 3;
                ImageSource = "GpsSetp3.png";
                workingSuiteServiceClient = ServiceClientFactory.Create<WorkingSuiteServiceClient>();
                monitorServiceClient = ServiceClientFactory.Create<VehicleMonitorServiceClient>();
                monitorServiceClient.ValidateGPSGPSCompleted += monitorServiceClient_ValidateGPSGPSCompleted;
                deviceInstallServiceClient.SubmitGPSForStep3Completed += deviceInstallServiceClient_SubmitGPSForStep3Completed;

                StatuTimer = new DispatcherTimer();
                StatuTimer.Interval = TimeSpan.FromMilliseconds(3000);
                StatuTimer.Tick += _statuTimer_Tick;
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("UploadCarNumberViewModel()", ex);
            }
        }

        void deviceInstallServiceClient_SubmitGPSForStep3Completed(object sender, SubmitGPSForStep3CompletedEventArgs e)
        {
            try
            {
                if (e.Error == null)
                {
                    GoNextPage();
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }

        void monitorServiceClient_ValidateGPSGPSCompleted(object sender, ValidateGPSGPSCompletedEventArgs e)
        {
            try
            {
                if (e.Error == null)
                {
                    if (e.Result.IsSuccess && e.Result.Result)
                    {
                        StopTimer();
                        NextPage();
                    }
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }

        protected override void OnGetDetailComplete()
        {
            StatuTimer.Start();
        }

        private void _statuTimer_Tick(object sender, EventArgs e)
        {
            if (this.InstallInfo != null)
            {
                monitorServiceClient.ValidateGPSGPSAsync(InstallInfo.VehicleID, this.InstallInfo.DeviceCoreId);
            }
        }

        protected override void NextPage()
        {
            try
            {
                deviceInstallServiceClient.SubmitGPSForStep3Async(InstallInfo.Id);
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("UploadCarNumberViewModel NextPage", ex);
            }
        }

        protected override void GoNextPage()
        {
            EventAggregator.Publish(new ViewNavigationArgs(InstallationName.InstallGPSConfirmV, new Dictionary<string, object>() { { "ID", _InstallID } }));
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
            base.DeactivateView(viewName);
            StopTimer();
            ResetData();
        }

        private void StopTimer()
        {
            if (StatuTimer.IsEnabled)
                StatuTimer.Stop();
        }
    }
}
