/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 4eead79f-81ab-4cec-9723-735ace51bef5      
/////             clrversion: 4.0.30319.17929
/////Registered organization: 
/////           Machine Name: PC-LANQ
/////                 Author: TEST(lanq)
/////======================================================================
/////           Project Name: Gsafety.PTMS.BaseInformation.ViewModels
/////    Project Description:    
/////             Class Name: BaseInfoMainPageVm
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/8/9 11:41:11
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/8/9 11:41:11
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
using Jounce.Framework;
using Gsafety.PTMS.ServiceReference.DistrictService;
using Gsafety.PTMS.Share;
using System.Collections.ObjectModel;
using System.Collections.Generic;

namespace Gsafety.PTMS.BaseInformation.ViewModels
{
    [ExportAsViewModel(BaseInformationName.BaseInfoMainPageVm)]
    public class BaseInfoMainPageVm : BaseViewModel
    {
        public BaseInfoMainPageVm()
        {
            if (ApplicationContext.Instance.BufferManager.DistrictManager.DistrictByAuthority == null)
            {
                ApplicationContext.Instance.BufferManager.DistrictManager.DistrictByAuthority = new List<District>();
            }
        }

        protected override void ActivateView(string viewName, System.Collections.Generic.IDictionary<string, object> viewParameters)
        {
            EventAggregator.Publish(BaseInformationName.BaseInfoMenuV.AsViewNavigationArgs());
        }
    }
}
