using System.ComponentModel.Composition;
using System.Windows.Navigation;
using Jounce.Core.Event;
using Jounce.Core.View;
using Jounce.Framework;
using Gsafety.PTMS.Share;
using System;

namespace GisManagement.Views
{
    public partial class GisNavigationer
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
        public GisNavigationContainer NavContainer { get; set; }

        private static string _lastView = string.Empty;

        public GisNavigationer()
        {
            InitializeComponent();
            CompositionInitializer.SatisfyImports(this);
            // if (ApplicationContext.Instance.CurrentGISName != GisName.TrafficGisView)
            try
            {
                if (LayoutRoot.Children.IndexOf(NavContainer) != -1)
                    return;
                LayoutRoot.Children.Add(NavContainer);

                if (!string.IsNullOrEmpty(_lastView)) return;
                //this.Title = ApplicationContext.Instance.StringResourceReader.GetString("TrafficSystem");

                this.KeyDown += MainNavigation_KeyDown;
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("GisNavigationer", ex);
            }

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
            //LayoutRoot.Children.Remove(NavContainer);
        }

        public void RemoveContainer()
        {
            if (!string.IsNullOrEmpty(_lastView))
            {
                EventAggregator.Publish(new ViewNavigationArgs(_lastView) { Deactivate = true });
            }
            LayoutRoot.Children.Remove(NavContainer);
        }
    }
}
