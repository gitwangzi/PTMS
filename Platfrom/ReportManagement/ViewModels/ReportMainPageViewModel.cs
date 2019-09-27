/////Copyright (C) Gsafety 2014 .All Rights Reserved.
/////======================================================================
/////                   Guid: 386b7f50-73fe-4f74-b224-0b4c8bcd5595      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-ZHOUDD
/////                 Author: GJSY(zhoudd)
/////======================================================================
/////           Project Name: Gsafety.PTMS.ReportManager.ViewModels
/////    Project Description:    
/////             Class Name: ReportMainPageViewModel
/////          Class Version: v1.0.0.0
/////            Create Time: 2014/7/24 14:55:20
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2014/7/24 14:55:20
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using Jounce.Core.ViewModel;
using Jounce.Framework;
namespace Gsafety.PTMS.ReportManager.ViewModels
{
    [ExportAsViewModel(ReportName.ReportMainPageVm)]
    public class ReportMainPageViewModel : BaseViewModel
    {
        public ReportMainPageViewModel()
        {

        }
        protected override void ActivateView(string viewName, System.Collections.Generic.IDictionary<string, object> viewParameters)
        {
            EventAggregator.Publish(ReportName.ReportMenuV.AsViewNavigationArgs());
        }
    }
}
