using Jounce.Core.View;
using Jounce.Regions.Core;
using PublicServiceManagement;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;
using System.Windows.Navigation;

namespace Gsafety.PTMS.PublicServiceManagement.Views
{
    [ExportAsView(PublicServiceName.PublicServiceMainPageV, Category = "Navigation", MenuName = "Gis Manager", ToolTip = "Click to view some text.")]
    [ExportViewToRegion(PublicServiceName.PublicServiceMainPageV, "CentralMainContainer2")]
    public partial class PublicServiceMainPage : UserControl
    {
        public PublicServiceMainPage()
        {
            InitializeComponent();

            this.Loaded += PublicServiceMainPage_Loaded;
        }

        void PublicServiceMainPage_Loaded(object sender, RoutedEventArgs e)
        {
            this.Loaded -= PublicServiceMainPage_Loaded;
            if (MessageManagement.Items.Count > 0)
            {
                var first = MessageManagement.Items[0] as Gsafety.PTMS.Share.MenuInfo;
                PublicServiceFrame.Source = new Uri(first.Uri, UriKind.Relative);

            }
            else if (RegistryManagement.Items.Count > 0)
            {
                var first = MessageManagement.Items[0] as Gsafety.PTMS.Share.MenuInfo;
                PublicServiceFrame.Source = new Uri(first.Uri, UriKind.Relative);
            }
        }

        private void ContentFrame_Navigated(object sender, NavigationEventArgs e)
        {
            //bool focusOne = false;
            //foreach (var hb in baseInfoMenu.NavigationButtons)
            //{
            //    if (hb.NavigateUri == e.Uri)
            //    {
            //        focusOne = true;
            //        VisualStateManager.GoToState(hb, "ActiveLink", true);
            //    }
            //    else
            //    {
            //        VisualStateManager.GoToState(hb, "InactiveLink", true);
            //    }
            //}

            //if (!focusOne && baseInfoMenu.NavigationButtons != null && baseInfoMenu.NavigationButtons.Count > 0)
            //{
            //    VisualStateManager.GoToState(baseInfoMenu.NavigationButtons[0], "ActiveLink", true);
            //}
        }

        // If an error occurs during navigation, show an error window
        private void ContentFrame_NavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            e.Handled = true;
        }

        //[ExportAsView(BaseInformationName.BaseInfoMenuV)]
        //public UserControl BaseInfoMenu
        //{
        //    get { return baseInfoMenu; }
        //}

        //control left panle display kejian
        private void hiddenL_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var collapseAnimation = (Storyboard)Resources["collapseTransition"];
            collapseAnimation.Completed += (m, n) =>
            {
                SythesesLeftContent.Visibility = Visibility.Collapsed;
                ContentBorder.SetValue(Grid.ColumnProperty, 0);
                ContentBorder.SetValue(Grid.ColumnSpanProperty, 2);
                collapsedPane.Visibility = Visibility.Visible;
                collapsedPane.Opacity = 0.8;
            };
            collapseAnimation.Begin();

        }

        private void showL_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var collapseAnimation = (Storyboard)Resources["expandTransition"];
            collapseAnimation.Begin();
            ContentBorder.SetValue(Grid.ColumnProperty, 1);
            ContentBorder.Margin = new Thickness(0, 0, 0, 0);
            SythesesLeftContent.Visibility = Visibility.Visible;
            collapsedPane.Visibility = Visibility.Collapsed;
        }
    }
}
