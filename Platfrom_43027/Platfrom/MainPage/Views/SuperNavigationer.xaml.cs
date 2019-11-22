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
using System.Windows.Navigation;
using System.ComponentModel.Composition;
using Jounce.Core.Event;
using Jounce.Core.View;
using Jounce.Framework;
namespace Gsafety.Ant.MainPage.Views
{
    public partial class SuperNavigationer : Page
    {
        public SuperNavigationer()
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
            this.KeyDown += MainNavigation_KeyDown;
        }

        void MainNavigation_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
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
        public SuperNavigationContainer NavContainer { get; set; }

        private static string _lastView = string.Empty;


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
