using Gsafety.PTMS.ServiceReference.MaitenanceRecordService;
using Jounce.Core.View;
using Jounce.Core.ViewModel;
using Jounce.Framework.ViewModel;
/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 9572519c-e294-490e-a9f7-21c1467f174d      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-PENGGL
/////                 Author: TEST(penggl)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Maintain.ViewModels
/////    Project Description:    
/////             Class Name: HandleRecordDetailVm
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/11/21 14:57:45
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/11/21 14:57:45
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
    [ExportAsViewModel(MaintainName.HandleRecordDetailVm)]
    public class HandleRecordDetailVm : BaseEntityViewModel
    {
        public HandleRecord CurrentHandleRecord { get; set; }
        public string SuiteStatus { get; set; }
        public string DeviceType { get; set; }
        private string action;
        protected override void ActivateView(string viewName, System.Collections.Generic.IDictionary<string, object> viewParameters)
        {

            base.ActivateView(viewName, viewParameters);
            action = viewParameters["action"].ToString();
            if (action == "view")
            {
                CurrentHandleRecord = viewParameters["view"] as HandleRecord;
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => CurrentHandleRecord));
                //SuiteStatus = ApplicationContext.Instance.StringResourceReader.GetString(CurrentHandleRecord.status.ToString());
                //Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => SuiteStatus));
                //DeviceType = ApplicationContext.Instance.StringResourceReader.GetString(CurrentHandleRecord.DeviceType.ToString());
                //Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => DeviceType));
            }
        }

        public HandleRecordDetailVm()
        {

        }

        protected override void OnCommitted()
        {
            EventAggregator.Publish(new ViewNavigationArgs(MaintainName.HandleRecordV, new Dictionary<string, object>() { { "action", "return" } }));
        }
    }

}
