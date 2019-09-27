using Gsafety.PTMS.ServiceReference.DeviceInstallService;
using Gsafety.PTMS.Share;
using Jounce.Core.View;
using Jounce.Core.ViewModel;
using Jounce.Framework.ViewModel;
/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 2df611b0-be72-4e9d-a281-98a83d25c09d      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-PENGGL
/////                 Author: TEST(penggl)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Maintain.ViewModels.SecuritySuiteViewModel
/////    Project Description:    
/////             Class Name: SuiteInspectDisplayVm
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/11/5 10:03:18
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/11/5 10:03:18
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

namespace Gsafety.PTMS.Maintain.ViewModels.SecuritySuiteViewModel
{
    [ExportAsViewModel(MaintainName.SuiteInspectDisplayVm)]
    public class SuiteInspectDisplayVm : BaseEntityViewModel
    {
        public SelfInspectInfo CurrentInspectInfo { get; set; }
        private string action;
        protected override void ActivateView(string viewName, System.Collections.Generic.IDictionary<string, object> viewParameters)
        {

            base.ActivateView(viewName, viewParameters);
            action = viewParameters["action"].ToString();
            if (action == "view")
            {
                CurrentInspectInfo = viewParameters["view"] as SelfInspectInfo;
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => CurrentInspectInfo));
                //SuiteStatus = ApplicationContext.Instance.StringResourceReader.GetString(CurrentSecuritySuite.status.ToString());
                //Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => SuiteStatus));
                //DeviceType = ApplicationContext.Instance.StringResourceReader.GetString(CurrentSecuritySuite.DeviceType.ToString());
                //Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => DeviceType));
            }
        }

        public SuiteInspectDisplayVm()
        {

        }

        protected override void OnCommitted()
        {
            EventAggregator.Publish(new ViewNavigationArgs(MaintainName.SuiteInspectV, new Dictionary<string, object>() { { "action", "return" } }));
        }
    }
}
