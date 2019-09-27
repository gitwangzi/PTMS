using Gsafety.PTMS.Share;
using Jounce.Core.Event;
using Jounce.Core.View;
using Jounce.Framework;
using System;
using System.ComponentModel.Composition;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace Gsafety.PTMS.MainPage.View
{
    public partial class CentralNavigationer2
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
        public CentralNavigationContainer2 NavContainer { get; set; }

        private static string _lastView = string.Empty;

        public CentralNavigationer2()
        {
            InitializeComponent();
            CompositionInitializer.SatisfyImports(this);
            var parent = NavContainer.Parent as Grid;
            if (parent != null)
            {
                parent.Children.Remove(NavContainer);
            }
            LayoutRoot.Children.Add(NavContainer);
            if (!string.IsNullOrEmpty(_lastView)) return;
            this.Title = ApplicationContext.Instance.StringResourceReader.GetString("MAINPAGE_Log");
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
                if (string.IsNullOrEmpty(newView))
                    return;
                _lastView = newView;
                EventAggregator.Publish(_lastView.AsViewNavigationArgs());
                EventAggregator.Publish(NavigationContext);
            }
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(_lastView))
                {
                    EventAggregator.Publish(new ViewNavigationArgs(_lastView) { Deactivate = true });
                }
                LayoutRoot.Children.Remove(NavContainer);

                object mview = ApplicationContext.Instance.MenuManager.Router.ViewQuery(_lastView);
                Frame frame = (mview as UserControl).FindName("ContentFrame") as Frame;
                if (frame == null)
                {
                    return;
                }
                else
                {
                    if (frame.Content != null && frame.Content is GisManagement.Views.GisNavigationer)
                    {
                        (frame.Content as GisManagement.Views.GisNavigationer).RemoveContainer();
                    }
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(GetType().FullName, ex);
            }
        }
    }
}
