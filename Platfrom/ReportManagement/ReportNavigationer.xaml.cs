/////Copyright (C) Gsafety 2014 .All Rights Reserved.
/////======================================================================
/////                   Guid: a25e170d-d022-4b6f-b59d-cca8b37f6859      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-ZHOUDD
/////                 Author: GJSY(zhoudd)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Managers
/////    Project Description:    
/////             Class Name: ReportNavigationer
/////          Class Version: v1.0.0.0
/////            Create Time: 2014/7/24 11:33:28
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2014/7/24 11:33:28
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System;
using System.ComponentModel.Composition;
using System.Windows.Navigation;
using Jounce.Core.Event;
using Jounce.Core.View;
using Jounce.Framework;

namespace Gsafety.PTMS.ReportManager
{
    public partial class ReportNavigationer
    {
          /// <summary>
        ///     Event aggregator
        /// </summary>
        [Import]
        public IEventAggregator ReportMangerEventAggregator { get; set; }

        /// <summary>
        /// Navigation container (holds the region for target views)
        /// </summary>
        [Import]
        public ReportNavigationContainer ReportManagerNavContainer { get; set; }

        private static string _lastView = string.Empty;

        public ReportNavigationer()
        {
            InitializeComponent();
            try
            {
                CompositionInitializer.SatisfyImports(this);
                LayoutRoot.Children.Add(ReportManagerNavContainer);
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
        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            if (NavigationContext.QueryString.ContainsKey("view"))
            {
                var newView = NavigationContext.QueryString["view"];
                _lastView = newView;
                ReportMangerEventAggregator.Publish(_lastView.AsViewNavigationArgs());
                ReportMangerEventAggregator.Publish(NavigationContext);
            }
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            if (!string.IsNullOrEmpty(_lastView))
            {
                ReportMangerEventAggregator.Publish(new ViewNavigationArgs(_lastView) { Deactivate = true });
            }
            LayoutRoot.Children.Remove(ReportManagerNavContainer);
        }
    }
}
