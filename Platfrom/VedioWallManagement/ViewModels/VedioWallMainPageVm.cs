/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 5dbcb3bb-bb9b-4ff0-89bd-af704ac69e29      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-DENGZL
/////                 Author: TEST(dengzl)
/////======================================================================
/////           Project Name: Gsafety.PTMS.VedioWall.ViewModels
/////    Project Description:    
/////             Class Name: VedioWallMainPageVm
/////          Class Version: v1.0.0.0
/////            Create Time: 9/11/2013 4:42:55 PM
/////      Class Description:  
/////======================================================================
/////          Modified Time: 9/11/2013 4:42:55 PM
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
using System.Linq;
using Jounce.Core.ViewModel;
using Jounce.Core.View;
using Jounce.Framework;
using Jounce.Core.Event;
using System.Collections.Generic;
using Gsafety.PTMS.VedioWall.Views;
using Gsafety.PTMS.VedioWall;
using System.Reflection;
using Gsafety.PTMS.Share;

namespace Gsafety.PTMS.VideoManagement.ViewModels
{
    [ExportAsViewModel(VideoManagementName.VedioWallMainPageVm)]
    public class VedioWallMainPageVm : BaseViewModel
    {
        public VedioWallMainPageVm()
        {
            
        }

        protected override void ActivateView(string viewName, System.Collections.Generic.IDictionary<string, object> viewParameters)
        {
            try
            {
                base.ActivateView(viewName, viewParameters);

                EventAggregator.Publish(VideoManagementName.VedioWallMenuV.AsViewNavigationArgs());
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }
    }
}
