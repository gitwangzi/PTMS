using Jounce.Core.ViewModel;
using Jounce.Framework.ViewModel;
/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: b7abab4a-8179-4e1c-925c-f315bda4e315      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-PENGGL
/////                 Author: TEST(penggl)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Maintain.ViewModels
/////    Project Description:    
/////             Class Name: SuiteQueryDisplayVm
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/9/11 17:08:48
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/9/11 17:08:48
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
using Gsafety.PTMS.ServiceReference.SecuritySuiteService;
using System.Collections.ObjectModel;
using Jounce.Framework.Command;
using Jounce.Core.View;
using System.Linq;
using System.Collections.Generic;
using Gsafety.PTMS.Share;
namespace Gsafety.PTMS.Maintain.ViewModels
{
    [ExportAsViewModel(MaintainName.SecuritySuiteDisplayVm)]
    public class SecuritySuiteDisplayVm : BaseEntityViewModel
    {
        public DeviceSuite CurrentSecuritySuite { get; set; }
        public string SuiteStatus { get; set; }
        public string DeviceType { get; set; }
        private string action;
        protected override void ActivateView(string viewName, System.Collections.Generic.IDictionary<string, object> viewParameters)
        {

            base.ActivateView(viewName, viewParameters);
            action = viewParameters["action"].ToString();
            if (action == "view")
            {
                CurrentSecuritySuite = viewParameters["view"] as DeviceSuite;
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => CurrentSecuritySuite));
                SuiteStatus = ApplicationContext.Instance.StringResourceReader.GetString(CurrentSecuritySuite.status.ToString());
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => SuiteStatus));
                DeviceType = ApplicationContext.Instance.StringResourceReader.GetString(CurrentSecuritySuite.DeviceType.ToString());
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => DeviceType));
            }
        }

        public SecuritySuiteDisplayVm()
        {

        }

        protected override void OnCommitted()
        {
            EventAggregator.Publish(new ViewNavigationArgs(MaintainName.SecuritySuiteV, new Dictionary<string, object>() { { "action", "return" } }));
        }
    }
}
