using Gsafety.Common.Controls;
using Gsafety.PTMS.Installation;
using Gsafety.PTMS.Installation.ViewModels;
using Gsafety.PTMS.ServiceReference.DeviceInstallService;
using Gsafety.PTMS.Share;
using Jounce.Core.View;
using Jounce.Framework.ViewModel;
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
    public class InstallGPSViewModelBase : InstallViewModelBase
    {
        public InstallGPSViewModelBase()
            : base()
        {
            deviceInstallServiceClient.GetGPSInstallationDetailCompleted += deviceinstallserviceClient_GetGPSInstallationDetailCompleted;
        }

        protected override void ActivateView(string viewName, System.Collections.Generic.IDictionary<string, object> viewParameters)
        {
            if (viewParameters != null && viewParameters.Count > 0 && viewParameters.Keys.Contains("ID"))
            {
                _InstallID = viewParameters["ID"].ToString();
                ResetData();
                deviceInstallServiceClient.GetGPSInstallationDetailAsync(_InstallID);
            }
        }

        protected virtual void OnGetDetailComplete()
        {

        }

        protected virtual void deviceinstallserviceClient_GetGPSInstallationDetailCompleted(object sender, GetGPSInstallationDetailCompletedEventArgs e)
        {
            try
            {
                if (e.Result.IsSuccess)
                {
                    InstallInfo = e.Result.Result;
                    InstallInfo.CreateTime = InstallInfo.CreateTime.Value.ToLocalTime();
                }

                OnGetDetailComplete();
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("Installation " + step.ToString(), ex);
            }
        }

        protected override void Quit()
        {
            var window = MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString("Tip"), ApplicationContext.Instance.StringResourceReader.GetString("ID_INSTALL_IfCancelInstall"), MessageDialogButton.OkAndCancel);
            window.Closed += closeWindow_Closed;
            //if (result.DialogResult == true)
            //{
            //    ResetData();
            //    EventAggregator.Publish(new ViewNavigationArgs(InstallationName.InstallGPSVehicleCheckV));
            //}
        }

        private void closeWindow_Closed(object sender, EventArgs e)
        {
            SelfMessageBox dialog = sender as SelfMessageBox;
            if (dialog != null)
            {
                if (dialog.DialogResult == true)
                {
                    ResetData();
                    EventAggregator.Publish(new ViewNavigationArgs(InstallationName.InstallGPSVehicleCheckV));
                }
            }
        }

    }
}
