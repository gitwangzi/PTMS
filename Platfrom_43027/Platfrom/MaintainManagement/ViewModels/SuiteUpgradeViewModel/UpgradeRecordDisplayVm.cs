using Gsafety.PTMS.ServiceReference.UpdateService;
using Jounce.Core.View;
using Jounce.Core.ViewModel;
using Jounce.Framework.ViewModel;
/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 66de213c-eacb-4497-a0a8-d25555325d56      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-PENGGL
/////                 Author: TEST(penggl)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Maintain.ViewModels
/////    Project Description:    
/////             Class Name: SuiteUpgradeRecordDisplayVm
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/9/17 16:39:05
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/9/17 16:39:05
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

namespace Gsafety.PTMS.Maintain.ViewModels
{

    [ExportAsViewModel(MaintainName.UpgradeRecordDisplayVm)]
    public class UpgradeRecordDisplayVm : BaseEntityViewModel
    {
        public SuiteUpdateRecord CurrentSuiteUpdateRecord { get; set; }
        private string action;
        protected override void ActivateView(string viewName, System.Collections.Generic.IDictionary<string, object> viewParameters)
        {

            base.ActivateView(viewName, viewParameters);
            action = viewParameters["action"].ToString();
            if (action == "view")
            {
                CurrentSuiteUpdateRecord = viewParameters["view"] as SuiteUpdateRecord;
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => CurrentSuiteUpdateRecord));
            }
        }

        public UpgradeRecordDisplayVm()
        {

        }

        protected override void OnCommitted()
        {
            EventAggregator.Publish(new ViewNavigationArgs(MaintainName.UpgradeRecordV, new Dictionary<string, object>() { { "action", "return" } }));
        }
    }
}
