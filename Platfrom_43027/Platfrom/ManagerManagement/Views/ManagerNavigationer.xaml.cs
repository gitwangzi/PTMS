/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: d6ef3596-7c00-4ae9-b9a7-f58ba0595d9m      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-DENGZL
/////                 Author: TEST(dengzl)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Manager.Views
/////    Project Description:    
/////             Class Name: SuiteNavigationer
/////          Class Version: v1.0.0.0
/////            Create Time: 8/8/2013 9:21:34 AM
/////      Class Description:  
/////======================================================================
/////          Modified Time: 8/8/2013 9:21:34 AM
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System;
using System.ComponentModel.Composition;
using System.Windows.Navigation;
using Jounce.Core.Event;
using Jounce.Core.View;
using Jounce.Framework;
using Gsafety.PTMS.Share;
using System.Windows.Controls;

namespace Gsafety.PTMS.Manager.Views
{
    public partial class ManagerNavigationer
    {
        /// <summary>
        ///     Event aggregator
        /// </summary>
        [Import]
        public IEventAggregator MangerEventAggregator { get; set; }

        /// <summary>
        /// Navigation container (holds the region for target views)
        /// </summary>
        [Import]
        public ManagerNavigationContainer ManagerNavContainer { get; set; }

        private static string _lastView = string.Empty;

        public ManagerNavigationer()
        {
            InitializeComponent();
            try
            {
                CompositionInitializer.SatisfyImports(this);
                if (ManagerNavContainer.Parent != null)
                {
                    var grid = ManagerNavContainer.Parent as Grid;
                    grid.Children.Remove(ManagerNavContainer);
                }
                LayoutRoot.Children.Add(ManagerNavContainer);
                if (!string.IsNullOrEmpty(_lastView))
                    return;
            }
            catch (Exception ex)
            {
            }
            //EventAggregator.Publish(Constants.VerificationMangerV.AsViewNavigationArgs());
            //_lastView = Constants.InstallRecordV;
        }

        // Executes when the user navigates to this page.
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            try
            {
                if (NavigationContext.QueryString.ContainsKey("view"))
                {
                    var newView = NavigationContext.QueryString["view"];
                    _lastView = newView;
                    MangerEventAggregator.Publish(_lastView.AsViewNavigationArgs());
                    MangerEventAggregator.Publish(NavigationContext);


                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(GetType().ToString(), ex);
            }
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            try
            {

                if (!string.IsNullOrEmpty(_lastView))
                {
                    MangerEventAggregator.Publish(new ViewNavigationArgs(_lastView) { Deactivate = true });
                }
                LayoutRoot.Children.Remove(ManagerNavContainer);
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(GetType().ToString(), ex);
            }
        }
    }
}
