using Gsafety.PTMS.Share;
using Jounce.Core.View;
using Jounce.Regions.Core;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;
using System.Windows.Navigation;

namespace HistoryQueryManagement.Views
{
    [ExportAsView(HistoryQueryName.HistoryQueryMainV, Category = "Navigation", MenuName = "HistoryQuery", ToolTip = "Click to view some text.")]
    [ExportViewToRegion(HistoryQueryName.HistoryQueryMainV, "CentralMainContainer2")]
    public partial class HistoryQueryMainPage : UserControl
    {
        public HistoryQueryMainPage()
        {
            InitializeComponent();
            Gsafety.PTMS.Share.ApplicationContext.Instance.StringResourceReader.TranslateDataGrid(dgAlarm);
            Gsafety.PTMS.Share.ApplicationContext.Instance.StringResourceReader.TranslateDataGrid(dgAlert);
            Gsafety.PTMS.Share.ApplicationContext.Instance.StringResourceReader.TranslateDataGrid(dgDeviceAlert);
            this.MouseRightButtonDown += ChildWindow_MouseRightButtonDown;
        }

        private void ChildWindow_MouseRightButtonDown(object sender,

System.Windows.Input.MouseButtonEventArgs e)
        {
            e.Handled = true;
        }
        private void ContentFrame_Navigated(object sender, NavigationEventArgs e)
        {

        }
       
        // If an error occurs during navigation, show an error window
        private void ContentFrame_NavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            e.Handled = true;
        }

        private void LayoutRoot_MouseMove_1(object sender, MouseEventArgs e)
        {
            MousePosition.X = e.GetPosition(null).X;
            MousePosition.Y = e.GetPosition(null).Y;
        }

        private void GISContent_DragEnter_1(object sender, DragEventArgs e)
        {

        }

        private void GISContent_DragLeave_1(object sender, DragEventArgs e)
        {

        }

        //Show hidden container control on the left side effect
        private void hiddenL_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var collapseAnimation = (Storyboard)Resources["collapseTransition"];
            collapseAnimation.Completed += (m, n) =>
            {

                if (GoRightShowPane.Visibility == Visibility.Visible)
                {
                    GISContent.SetValue(Grid.ColumnProperty, 0);
                    GISContent.SetValue(Grid.ColumnSpanProperty, 3);
                }
                else
                {
                    GISContent.SetValue(Grid.ColumnProperty, 0);
                    GISContent.SetValue(Grid.ColumnSpanProperty, 2);
                }

                SythesesLeftContent.Visibility = Visibility.Collapsed;
                collapsedPane.Visibility = Visibility.Visible;
                collapsedPane.Opacity = 0.8;
            };
            collapseAnimation.Begin();

        }

        private void showL_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var collapseAnimation = (Storyboard)Resources["expandTransition"];

            collapseAnimation.Begin();

            if (GoRightShowPane.Visibility == Visibility.Visible)
            {
                GISContent.SetValue(Grid.ColumnProperty, 1);
                GISContent.SetValue(Grid.ColumnSpanProperty, 2);
            }
            else
            {
                GISContent.ClearValue(Grid.ColumnSpanProperty);
                GISContent.SetValue(Grid.ColumnProperty, 1);
            }

          
            GISContent.Margin = new Thickness(0, 0, 0, 0);
            SythesesLeftContent.Visibility = Visibility.Visible;
            collapsedPane.Visibility = Visibility.Collapsed;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            this.ContentFrame.Refresh();
        }

        private void AlarmAccordionItem_Selected(object sender, RoutedEventArgs e)
        {
            if (dgAlarm != null)
                dgAlarm.Visibility = System.Windows.Visibility.Visible;

            if (dgAlert != null)
                dgAlert.Visibility = System.Windows.Visibility.Collapsed;

            if (dgDeviceAlert != null)
                dgDeviceAlert.Visibility = System.Windows.Visibility.Collapsed;
        }

        private void DeviceAlertAccordionItem_Selected(object sender, RoutedEventArgs e)
        {
            if (dgAlarm != null)
                dgAlarm.Visibility = System.Windows.Visibility.Collapsed;
            if (dgAlert != null)
                dgAlert.Visibility = System.Windows.Visibility.Collapsed;
            if (dgDeviceAlert != null)
                dgDeviceAlert.Visibility = System.Windows.Visibility.Visible;
        }

        private void AlertAccordionItem_Selected(object sender, RoutedEventArgs e)
        {
            if (dgAlarm != null)
                dgAlarm.Visibility = System.Windows.Visibility.Collapsed;
            if (dgAlert != null)
                dgAlert.Visibility = System.Windows.Visibility.Visible;

            if (dgDeviceAlert != null)
                dgDeviceAlert.Visibility = System.Windows.Visibility.Collapsed;
        }

        ///// <summary>
        ///// 监控列表
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //private void ListshowL_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        //{
        //    var collapseAnimation = (Storyboard)Resources["ListExpandTransition"];
        //    collapseAnimation.Begin();
        //    InfoArea.Visibility = Visibility.Visible;
        //    ListCollapsedPane.Visibility = Visibility.Collapsed;
        //    ContentGrid.SetValue(Grid.RowProperty, 0);
        //    ContentGrid.SetValue(Grid.RowSpanProperty, 1);
        //}


        //向右的部分隐藏
        private void ListhiddenL_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var collapseAnimation = (Storyboard)Resources["ListCollapseTransition"];
            collapseAnimation.Completed += (m, n) =>
            {
                InfoArea.Visibility = Visibility.Collapsed;

                //ContentBorder.SetValue(Grid.ColumnProperty, 0);
                //ContentBorder.SetValue(Grid.ColumnSpanProperty, 2);
                //最左边折叠隐藏了GIS占用3列
                if (collapsedPane.Visibility == Visibility.Visible)
                {
                    GISContent.SetValue(Grid.ColumnProperty, 0);
                    GISContent.SetValue(Grid.ColumnSpanProperty, 3);
                }
                else
                {
                    GISContent.SetValue(Grid.ColumnProperty, 1);
                    GISContent.SetValue(Grid.ColumnSpanProperty, 2);
                }
                //ContentGrid.SetValue(Grid.RowProperty, 0);
                //ContentGrid.SetValue(Grid.RowSpanProperty, 3);
                //ListCollapsedPane.Visibility = Visibility.Visible;
                //ListCollapsedPane.Opacity = 0.8;
                GoRightShowPane.Visibility = Visibility.Visible;
                GoRightShowPane.Opacity = 0.8;
            };
            collapseAnimation.Begin();
        }

        /// <summary>
        /// 向右的部分显示
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GoRightShow_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var collapseAnimation = (Storyboard)Resources["ListExpandTransition"];
            collapseAnimation.Begin();
            if (collapsedPane.Visibility == Visibility.Visible)
            {
                GISContent.SetValue(Grid.ColumnProperty, 0);
                GISContent.SetValue(Grid.ColumnSpanProperty, 2);
            }
            else
            {
                GISContent.ClearValue(Grid.ColumnSpanProperty);
                GISContent.SetValue(Grid.ColumnProperty, 1);
            }
            InfoArea.Visibility = Visibility.Visible;
            InfoArea.SetValue(Grid.ColumnProperty, 2);
            GoRightShowPane.Visibility = Visibility.Collapsed;
        }
    }
}
