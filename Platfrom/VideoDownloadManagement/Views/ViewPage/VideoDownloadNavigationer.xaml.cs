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

namespace Gsafety.PTMS.VideoDownloadManagement.Views.ViewPage
{
    public partial class VideoDownloadNavigationer : Page
    {
        public VideoDownloadNavigationer()
        {
            InitializeComponent();


            try
            {
                CompositionInitializer.SatisfyImports(this);
                if (VideoDownloadNavContainer.Parent != null)
                {
                    var grid = VideoDownloadNavContainer.Parent as Grid;
                    if (grid != null)
                    {
                        var nav = grid.Parent as VideoDownloadNavigationer;
                        if (nav != null)
                        {
                            nav.LayoutRoot.Children.Remove(VideoDownloadNavContainer);
                        }
                    }
                }
                LayoutRoot.Children.Add(VideoDownloadNavContainer);
                if (!string.IsNullOrEmpty(_lastView))
                    return;
            }
            catch (Exception ex)
            {
            }
        }

        /// <summary>
        /// Event aggregator
        /// </summary>
        [Import]
        public IEventAggregator VideoDownloadEventAggregator { get; set; }

        /// <summary>
        /// Navigation container (holds the region for target views)
        /// </summary>
        [Import]
        public VideoDownloadNavigationContainer VideoDownloadNavContainer { get; set; }

        private static string _lastView = string.Empty;


        // Executes when the user navigates to this page.
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (NavigationContext.QueryString.ContainsKey("view"))
            {
                var newView = NavigationContext.QueryString["view"];
                _lastView = newView;
                VideoDownloadEventAggregator.Publish(_lastView.AsViewNavigationArgs());
                VideoDownloadEventAggregator.Publish(NavigationContext);
            }
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            if (!string.IsNullOrEmpty(_lastView))
            {
                VideoDownloadEventAggregator.Publish(new ViewNavigationArgs(_lastView) { Deactivate = true });
            }
            LayoutRoot.Children.Remove(VideoDownloadNavContainer);
        }

    }
}
