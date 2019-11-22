using Gsafety.PTMS.Share;
using Jounce.Core.View;
using Jounce.Regions.Core;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;
using System.Windows.Navigation;
using System.Linq;
using System.Windows.Media;
using System.Collections.ObjectModel;

namespace Gsafety.PTMS.Manager.Views
{
    /// <summary>
    /// 新增的ANT产品的系统管理主界面
    /// </summary>
    [ExportAsView(ManagerName.AntProductManagerMainPageV, Category = "Navigation", MenuName = "Gis Manager", ToolTip = "Click to view some text.")]
    [ExportViewToRegion(ManagerName.AntProductManagerMainPageV, "CentralMainContainer2")]
    public partial class AntProductManagerMainPage : UserControl
    {
        public AntProductManagerMainPage()
        {
            InitializeComponent();
            this.MouseRightButtonDown += ChildWindow_MouseRightButtonDown;
            this.Loaded += AntProductManagerMainPage_Loaded;
        }

        void AntProductManagerMainPage_Loaded(object sender, RoutedEventArgs e)
        {
            this.Loaded -= AntProductManagerMainPage_Loaded;

            try
            {
                var firstItem = NativeAccordion.Items.FirstOrDefault(t => (t as AccordionItem).Visibility == Visibility.Visible) as AccordionItem;

                if (firstItem != null)
                {
                    var meuns = firstItem.Tag as ObservableCollection<MenuInfo>;
                    var menu = meuns.FirstOrDefault();
                    SystemManageContentFrame.Source = new Uri(menu.Uri, UriKind.Relative);
                }
            }
            catch (System.Exception ex)
            {
            }
        }

        private void ChildWindow_MouseRightButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            e.Handled = true;
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
