using Gsafety.Common.Controls;
using Gsafety.PTMS.Installation;
using Gsafety.PTMS.Installation.ViewModels;
using Gsafety.PTMS.ServiceReference.DeviceInstallService;
using Gsafety.PTMS.Share;
using Jounce.Core.View;
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

namespace Gsafety.Ant.Installation.ViewModels
{
    public class InstallSuiteViewModelBase : InstallViewModelBase
    {
        public InstallSuiteViewModelBase()
            : base()
        {
            deviceInstallServiceClient.GetInstallationDetailCompleted += deviceinstallserviceClient_GetInstallationDetailCompleted;
            deviceInstallServiceClient.UpdateInstallationCompleted += deviceinstallserviceClient_UpdateInstallationCompleted;
        }

        void deviceinstallserviceClient_UpdateInstallationCompleted(object sender, UpdateInstallationCompletedEventArgs e)
        {
            try
            {
                if (e.Result.IsSuccess)
                {
                    if (e.Result.Result)
                    {
                        ResetData();
                        GoNextPage();
                    }
                    else
                    {
                        MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString("Tip"), ApplicationContext.Instance.StringResourceReader.GetString("ID_INSTALL_AddInstallationFailed"), MessageDialogButton.Ok);
                    }
                }
                else
                {
                    MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString("Tip"), ApplicationContext.Instance.StringResourceReader.GetString("ID_INSTALL_ServiceError"), MessageDialogButton.Ok);
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("Installation" + step.ToString(), ex);
            }
        }

        protected override void ActivateView(string viewName, System.Collections.Generic.IDictionary<string, object> viewParameters)
        {
            if (viewParameters != null && viewParameters.Count > 0 && viewParameters.Keys.Contains("ID"))
            {
                _InstallID = viewParameters["ID"].ToString();
                ResetData();
                deviceInstallServiceClient.GetInstallationDetailAsync(_InstallID);
            }
        }

        protected virtual void deviceinstallserviceClient_GetInstallationDetailCompleted(object sender, GetInstallationDetailCompletedEventArgs e)
        {
            try
            {
                if (e.Result.IsSuccess)
                {
                    InstallInfo = e.Result.Result;
                    InstallInfo.CreateTime = InstallInfo.CreateTime.Value.ToLocalTime();
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("Installation " + step.ToString(), ex);
            }
        }

        protected override void Quit()
        {
            ResetData();
            var result = MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString("ID_INSTALL_IfQuitInstall"), MessageDialogButton.OkAndCancel);
            result.Closed += result_Closed;
        }

        void result_Closed(object sender, EventArgs e)
        {
            var result = sender as ChildWindow;
            if (result.DialogResult == true)
            {
                EventAggregator.Publish(new ViewNavigationArgs(InstallationName.InstallVehicleCheckV));
            }
        }
    }
}
