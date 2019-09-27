/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: e72e459c-4d9e-48ba-9018-86e482e4f934      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-DENGZL
/////                 Author: TEST(dengzl)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Manager.ViewModels
/////    Project Description:    
/////             Class Name: ManagerMainPageVm
/////          Class Version: v1.0.0.0
/////            Create Time: 8/8/2013 11:37:13 AM
/////      Class Description:  
/////======================================================================
/////          Modified Time: 8/8/2013 11:37:13 AM
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using Gsafety.PTMS.ServiceReference.DistrictService;
using Gsafety.PTMS.Share;
using Jounce.Core.ViewModel;
using Jounce.Framework;
using Jounce.Framework.Command;
using System;
using System.Collections.Generic;
using System.Windows.Input;

namespace Gsafety.PTMS.Manager.ViewModels
{
    [ExportAsViewModel(ManagerName.ManagerMainPageVm)]
    public class ManagerMainPageVm : BaseViewModel
    {
        public ICommand GetCommand { get; private set; }

        public ManagerMainPageVm()
        {
            GetCommand = new ActionCommand<object>(obj => GetAction());
            if (ApplicationContext.Instance.BufferManager.DistrictManager.DistrictByAuthority == null)
            {
                ApplicationContext.Instance.BufferManager.DistrictManager.DistrictByAuthority = new List<District>();
            }
        }

        protected override void ActivateView(string viewName, IDictionary<string, object> viewParameters)
        {
            EventAggregator.Publish(ManagerName.ManagerMenuV.AsViewNavigationArgs());
        }

        private void GetAction()
        {
        }
    }
}
