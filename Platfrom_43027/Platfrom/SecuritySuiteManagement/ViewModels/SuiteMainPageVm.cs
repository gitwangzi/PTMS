/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 9ebb1556-ffb5-483c-84f1-cd7f71b773ac      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-DENGZL
/////                 Author: TEST(dengzl)
/////======================================================================
/////           Project Name: Gsafety.PTMS.SecuritySuite.ViewModels
/////    Project Description:    
/////             Class Name: SuiteMainPageVm
/////          Class Version: v1.0.0.0
/////            Create Time: 8/7/2013 9:14:04 AM
/////      Class Description:  
/////======================================================================
/////          Modified Time: 8/7/2013 9:14:04 AM
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

namespace Gsafety.PTMS.SecuritySuite.ViewModels
{
    [ExportAsViewModel(SecuritySuiteName.SuiteMainPageVm)]
    public class SuiteMainPageVm:BaseViewModel
    {
        protected override void ActivateView(string viewName, System.Collections.Generic.IDictionary<string, object> viewParameters)
        {
            EventAggregator.Publish(SecuritySuiteName.SuiteMenuV.AsViewNavigationArgs());
        }
    }
}
