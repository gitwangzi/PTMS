using System;
using System.Collections.Specialized;
using Gsafety.PTMS.BaseInformation;
using Jounce.Core.View;
using Jounce.Regions.Core;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;
using System.Windows.Navigation;

namespace Gsafety.Ant.BaseInformation.Views
{
    [ExportAsView(BaseInformationName.AntProductBaseInfoMainPageV, Category = "Navigation", MenuName = "Gis Manager", ToolTip = "Click to view some text.")]
    [ExportViewToRegion(BaseInformationName.AntProductBaseInfoMainPageV, "CentralMainContainer2")]
    public partial class AntProductBaseInfoMainPage : UserControl
    {
        public AntProductBaseInfoMainPage()
        {
            InitializeComponent();

            this.DataContextChanged += AntProductBaseInfoMainPage_DataContextChanged;
        }

        void AntProductBaseInfoMainPage_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (OrgManageControl2.Items.Count > 0)
            {
                var first = OrgManageControl2.Items[0] as Gsafety.PTMS.Share.MenuInfo;
                BaseInfoContentFrame.Source = new Uri(first.Uri, UriKind.Relative);
            }
        }

        // If an error occurs during navigation, show an error window
        private void ContentFrame_NavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            e.Handled = true;
        }

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
