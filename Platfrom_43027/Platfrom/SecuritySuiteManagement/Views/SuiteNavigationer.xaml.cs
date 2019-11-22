/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: d6ef3596-7c00-4ae9-b9a7-f58a00595d9m      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-DENGZL
/////                 Author: TEST(dengzl)
/////======================================================================
/////           Project Name: Gsafety.PTMS.SecuritySuite.Views
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

namespace Gsafety.PTMS.SecuritySuite.Views
{
    public partial class SuiteNavigationer 
    {
        /// <summary>
        ///     Event aggregator
        /// </summary>
        [Import]
        public IEventAggregator SuiteEventAggregator { get; set; }

        /// <summary>
        /// Navigation container (holds the region for target views)
        /// </summary>
        [Import]
        public SuiteNavigationContainer SuiteNavContainer { get; set; }

        private static string _lastView = string.Empty;

        public SuiteNavigationer()
        {
            InitializeComponent();
            try
            {
                CompositionInitializer.SatisfyImports(this);
                LayoutRoot.Children.Add(SuiteNavContainer);
                if (!string.IsNullOrEmpty(_lastView))
                    return;
            }
            catch(Exception ex)
            {
            }
            //EventAggregator.Publish(Constants.VerificationMangerV.AsViewNavigationArgs());
            //_lastView = Constants.InstallRecordV;
        }

        // Executes when the user navigates to this page.
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (NavigationContext.QueryString.ContainsKey("view"))
            {
                var newView = NavigationContext.QueryString["view"];
                _lastView = newView;
                SuiteEventAggregator.Publish(_lastView.AsViewNavigationArgs()); 
                SuiteEventAggregator.Publish(NavigationContext);
            }
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            if (!string.IsNullOrEmpty(_lastView))
            {
                SuiteEventAggregator.Publish(new ViewNavigationArgs(_lastView) {Deactivate = true});
            }
            LayoutRoot.Children.Remove(SuiteNavContainer);
        }
    }
}
