/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 2a50d02f-ddf7-498a-9826-ccc734a3b921      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-DENGZL
/////                 Author: TEST(dengzl)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Monitor.ViewModels
/////    Project Description:    
/////             Class Name: MonitorMainPageVm
/////          Class Version: v1.0.0.0
/////            Create Time: 8/6/2013 11:01:43 AM
/////      Class Description:  
/////======================================================================
/////          Modified Time: 8/6/2013 11:01:43 AM
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
using Gsafety.PTMS.Bases.Models;
using Jounce.Core.ViewModel;
using Jounce.Framework;
using Jounce.Framework.Command;
using GisManagement.Views;
using Jounce.Core.View;
using Gsafety.PTMS.Share;
using Gsafety.PTMS.Monitor;
using System.Reflection;

namespace Gsafety.PTMS.Monitor.ViewModels
{
    [ExportAsViewModel(MonitorName.MonitorMainPageVm)]
    public class MonitorMainPageVm : BaseViewModel
    {
        public MonitorMainPageVm()
        {
        }

        protected override void ActivateView(string viewName, System.Collections.Generic.IDictionary<string, object> viewParameters)
        {
            try
            {
                base.ActivateView(viewName, viewParameters);
                EventAggregator.Publish(MonitorName.MonitorMenuV.AsViewNavigationArgs());
                EventAggregator.Publish(MonitorName.VehicleInfoView.AsViewNavigationArgs());
                EventAggregator.Publish(GisManagement.GisName.MonitorGisView.AsViewNavigationArgs());
                EventAggregator.Publish(GisManagement.GisName.GpsCarList.AsViewNavigationArgs());
                EventAggregator.Publish(GisManagement.GisName.GpsCarHisDataViewMonitor.AsViewNavigationArgs());

                ApplicationContext.Instance.CurrentGISName = GisManagement.GisName.MonitorGisView;

                object mview = ApplicationContext.Instance.MenuManager.Router.ViewQuery(MonitorName.MonitorMainPageV);
                Frame frame = (mview as UserControl).FindName("ContentFrame") as Frame;

                if (frame.CurrentSource == null)
                    return;
                frame.Refresh();
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }
    }
}
