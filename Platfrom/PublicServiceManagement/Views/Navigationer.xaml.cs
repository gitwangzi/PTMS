/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: b0472e1f-fbc5-4110-ab70-12334e567780      
/////             clrversion: 4.0.30319.17929
/////Registered organization: 
/////           Machine Name: PC-LANQ
/////                 Author: TEST(lanq)
/////======================================================================
/////           Project Name: Gsafety.PTMS.BaseInformation.Views
/////    Project Description:    
/////             Class Name: BaseInfoNavigationer
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/8/12 15:30:22
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/8/12 15:30:22
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.ComponentModel.Composition;
using System.Windows.Navigation;
using Jounce.Core.Event;
using Jounce.Core.View;
using Jounce.Framework;

namespace Gsafety.PTMS.PublicServiceManagement.Views
{
    public partial class Navigationer
    {
        /// <summary>
        /// Event aggregator
        /// </summary>
        [Import]
        public IEventAggregator EventAggregator { get; set; }

        /// <summary>
        /// Navigation container (holds the region for target views)
        /// </summary>
        [Import]
        public NavigationContainer NavContainer { get; set; }

        private static string _lastView = string.Empty;

        public Navigationer()
        {
            InitializeComponent();

            try
            {
                CompositionInitializer.SatisfyImports(this);
                if (NavContainer.Parent != null)
                {
                    var grid = NavContainer.Parent as Grid;
                    grid.Children.Remove(NavContainer);
                }
                LayoutRoot.Children.Add(NavContainer);
                if (!string.IsNullOrEmpty(_lastView))
                    return;
            }
            catch (Exception ex)
            {
            }
        }

        // Executes when the user navigates to this page.
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (NavigationContext.QueryString.ContainsKey("view"))
            {
                var newView = NavigationContext.QueryString["view"];
                _lastView = newView;
                EventAggregator.Publish(_lastView.AsViewNavigationArgs());
                EventAggregator.Publish(NavigationContext);
            }
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            if (!string.IsNullOrEmpty(_lastView))
            {
                EventAggregator.Publish(new ViewNavigationArgs(_lastView) { Deactivate = true });
            }
            LayoutRoot.Children.Remove(NavContainer);
        }
    }
}
