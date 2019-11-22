using Gsafety.PTMS.ServiceReference.WorkingSuiteService;
using Gsafety.PTMS.Share;
using Jounce.Core.View;
using Jounce.Core.ViewModel;
using Jounce.Framework.ViewModel;
/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: cb811b7a-6d32-4987-9095-ca78609c17ce      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-PENGGL
/////                 Author: TEST(penggl)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Maintain.ViewModels.SecuritySuiteViewModel
/////    Project Description:    
/////             Class Name: SuiteRunningDisplayVm
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/11/5 18:46:47
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/11/5 18:46:47
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
    [ExportAsViewModel(MaintainName.SuiteRunningDisplayVm)]
    public class SuiteRunningDisplayVm : BaseEntityViewModel
    {
        private WorkingSuiteServiceClient WorkingSuiteClient = ServiceClientFactory.Create<WorkingSuiteServiceClient>();
        public SuiteRunningDetail CurrentSuiteRunningDetail { get; set; }
        private string action;
        protected override void ActivateView(string viewName, System.Collections.Generic.IDictionary<string, object> viewParameters)
        {

            base.ActivateView(viewName, viewParameters);
            action = viewParameters["action"].ToString();
            if (action == "view" || action == "newRunningInfo")
            {
                CurrentSuiteRunningDetail = viewParameters[action] as SuiteRunningDetail;
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => CurrentSuiteRunningDetail));
            }
        }

        public SuiteRunningDisplayVm()
        {

        }

        protected override void OnCommitted()
        {
            if (action == "view")
                EventAggregator.Publish(new ViewNavigationArgs(MaintainName.SuiteRunningV, new Dictionary<string, object>() { { "action", "return" } }));
            else
                EventAggregator.Publish(new ViewNavigationArgs(MaintainName.MaintenanceHandleV, new Dictionary<string, object>() { { "action", "return" } }));
        }
    }
}
