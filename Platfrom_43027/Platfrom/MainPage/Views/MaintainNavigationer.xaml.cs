/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 554d060d-0db3-4ec4-8ad6-3fbe14301c18      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-DENGZL
/////                 Author: TEST(dengzl)
/////======================================================================
/////           Project Name: Gsafety.PTMS.MainPage.Views
/////    Project Description:    
/////             Class Name: MaintainNavigationer
/////          Class Version: v1.0.0.0
/////            Create Time: 8/14/2013 1:04:22 PM
/////      Class Description:  
/////======================================================================
/////          Modified Time: 8/14/2013 1:04:22 PM
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System.ComponentModel.Composition;
using System.Windows.Navigation;
using Jounce.Core.Event;
using Jounce.Core.View;
using Jounce.Framework;

namespace Gsafety.PTMS.MainPage.Views
{
    public partial class MaintainNavigationer 
    {

        /// <summary>
        ///     Event aggregator
        /// </summary>
        [Import]
        public IEventAggregator EventAggregator { get; set; }

        /// <summary>
        ///     Navigation container (holds the region for target views)
        /// </summary>
        [Import]
        public MaintainNavigationContainer NavContainer { get; set; }

        private static string _lastView = string.Empty;

        public MaintainNavigationer()
        {
            InitializeComponent();
            CompositionInitializer.SatisfyImports(this);
            LayoutRoot.Children.Add(NavContainer);
            if (!string.IsNullOrEmpty(_lastView)) return;
            //this.Title = ApplicationContext.Instance.StringResourceReader.GetString("TrafficSystem");
            this.KeyDown += MainNavigation_KeyDown;

        }

        void MainNavigation_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Back)
                e.Handled = true;
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
