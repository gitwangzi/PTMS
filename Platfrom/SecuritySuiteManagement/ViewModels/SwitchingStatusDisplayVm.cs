using Gsafety.PTMS.ServiceReference.DeviceInstallService;
using Gsafety.PTMS.Share;
using Jounce.Core.View;
using Jounce.Core.ViewModel;
using Jounce.Framework.ViewModel;
using System;
using System.Collections.Generic;
using System.Net;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Gsafety.PTMS.SecuritySuite.ViewModels
{
    [ExportAsViewModel(SecuritySuiteName.SwitchingStatusDisplayVm)]
    public class SwitchingStatusDisplayVm : BaseEntityViewModel
    {
        //public SelfInspectDetail CurrentInspectInfo { get; set; }
        private string action;
        protected override void ActivateView(string viewName, System.Collections.Generic.IDictionary<string, object> viewParameters)
        {
            try
            {
                base.ActivateView(viewName, viewParameters);
                action = viewParameters["action"].ToString();
                if (action == "view")
                {
                    //CurrentInspectInfo = viewParameters["view"] as SelfInspectDetail;
                    //Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => CurrentInspectInfo));
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("SwitchingStatusDisplayVm ActivateView", ex);
            }
        }

        public SwitchingStatusDisplayVm()
        {
            try
            {

            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("SwitchingStatusDisplayVm()", ex);
            }

        }

        protected override void OnCommitted()
        {
            try
            {
                EventAggregator.Publish(new ViewNavigationArgs(SecuritySuiteName.SwitchingStatusV, new Dictionary<string, object>() { { "action", "return" } }));
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("SwitchingStatusDisplayVm OnCommitted", ex);
            }
        }
    }
}
