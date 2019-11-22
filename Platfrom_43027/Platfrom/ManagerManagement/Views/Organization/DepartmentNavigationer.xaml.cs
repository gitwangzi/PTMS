using Jounce.Core.Event;
using Jounce.Core.View;
using Jounce.Framework;
using System.ComponentModel.Composition;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;

namespace Gsafety.PTMS.Manager.Views.Organization
{
    public partial class DepartmentNavigationer : Page
    {
        public DepartmentNavigationer()
        {
            InitializeComponent();
            CompositionInitializer.SatisfyImports(this);
            LayoutRoot.Children.Add(NavContainer);
            if (!string.IsNullOrEmpty(_lastView)) return;
            this.KeyDown += MainNavigation_KeyDown;
        }

        void MainNavigation_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Back)
                e.Handled = true;
        }

        /// <summary>
        ///     Event aggregator
        /// </summary>
        [Import]
        public IEventAggregator EventAggregator { get; set; }

        /// <summary>
        ///     Navigation container (holds the region for target views)
        /// </summary>
        [Import]
        public DepartmentNavigationContainer NavContainer { get; set; }

        private static string _lastView = string.Empty;

        // Executes when the user navigates to this page.
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (NavigationContext.QueryString.ContainsKey("view"))
            {
                var newView = NavigationContext.QueryString["view"];
                _lastView = newView;
                //EventAggregator.Publish(_lastView.AsViewNavigationArgs());
                //EventAggregator.Publish(new ViewNavigationArgs(){});
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
