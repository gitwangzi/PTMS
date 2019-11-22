/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: c2453119-e2b5-4601-8b5c-c6a82ac22e78      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-LIW
/////                 Author: TEST(liw)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Alarm.ViewModels
/////    Project Description:    
/////             Class Name: AlarmMainPageVm
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/8/19 11:13:38
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/8/19 11:13:38
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
using Jounce.Core.View;
using Jounce.Core.ViewModel;
using Jounce.Framework;
using Gsafety.PTMS.Share;


namespace Gsafety.PTMS.Alarm.ViewModels
{
    [ExportAsViewModel(AlarmName.AlarmMainViewModel)]
    public class AlarmMainPageVm : BaseViewModel
    {
        bool IsActivate = false;
        protected override void ActivateView(string viewName, System.Collections.Generic.IDictionary<string, object> viewParameters)
        {
            base.ActivateView(viewName, viewParameters);
            ApplicationContext.Instance.CurrentView = 1;
            EventAggregator.Publish(AlarmName.AlarmInfoView.AsViewNavigationArgs());       
            EventAggregator.Publish(AlarmName.AlarmMenuView.AsViewNavigationArgs());
              

			
			//EventAggregator.Publish(GisManagement.GisName.GpsCarHisDataViewMonitor.AsViewNavigationArgs());
          
            object mview = ApplicationContext.Instance.MenuManager.Router.ViewQuery(AlarmName.AlarmMainView);
            Frame frame = (mview as UserControl).FindName("ContentFrame") as Frame;
            if (frame.CurrentSource == null)
                return;
            frame.Refresh();
        
        }
        
        protected override void DeactivateView(string viewName)
        {
            base.DeactivateView(viewName);
            ApplicationContext.Instance.CurrentView = -1;
        }
    }
}

