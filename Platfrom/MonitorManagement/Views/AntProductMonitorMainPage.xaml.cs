using Gsafety.Common.Controls;
using Gsafety.PTMS.Constants;
using Gsafety.PTMS.Monitor;
using Gsafety.PTMS.Monitor.Views;
using Gsafety.PTMS.Share;
using Jounce.Core.Event;
using Jounce.Core.View;
using Jounce.Regions.Core;
using System;
using System.ComponentModel.Composition;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Navigation;

namespace Gsafety.Ant.Monitor.Views
{
    [ExportAsView(MonitorName.AntProductMonitorMainPageV, Category = "Navigation", MenuName = "VehiclesMonitor", ToolTip = "Click to view some text.")]
    [ExportViewToRegion(MonitorName.AntProductMonitorMainPageV, ViewContainer.CentralMainContainer2)]
    public partial class AntProductMonitorMainPage : UserControl
    {

        VehicleInfoPage m_VehicleDragPanel = new VehicleInfoPage();

        [ExportAsView(MonitorName.VehicleInfoView)]
        public UserControl VehicleDragPanel
        {
            get { return m_VehicleDragPanel; }
        }

        AlarmInfoPage AlarmDragPanel = new AlarmInfoPage();

        [ExportAsView(MonitorName.MonitorAlarmInfoView)]
        public UserControl AlarmInfoControl
        {
            get { return AlarmDragPanel; }
        }

        AlertDetailPage alertinfopage = new AlertDetailPage();

        [ExportAsView(MonitorName.MonitorAlertInfoView)]
        public UserControl AlertDetailView
        {
            get { return alertinfopage; }
        }

        AlarmHandlePage alarmhandle = new AlarmHandlePage();
        [ExportAsView(MonitorName.MonitorAlarmHandleView)]
        public UserControl AlarmHandleView
        {
            get { return alarmhandle; }
        }


        SelectFence selectfence = new SelectFence();
        [ExportAsView(MonitorName.MonitorSelectFenceView)]
        public UserControl SelectFenceView
        {
            get { return selectfence; }
        }

        AlertHandlePage alerthandle = new AlertHandlePage();
        [ExportAsView(MonitorName.MonitorAlertHandleView)]
        public UserControl AlertHandleView
        {
            get
            {
                return alerthandle;
            }
        }

        AlarmHandlePage manualhandle = new AlarmHandlePage();
        [ExportAsView(MonitorName.MonitorManualAlarmHandleView)]
        public UserControl ManualAlarmHandle
        {
            get { return manualhandle; }
        }

        [Import]
        public IEventAggregator EventAggregator { get; set; }

        public AntProductMonitorMainPage()
        {
            try
            {
                try
                {
                    InitializeComponent();
                    Gsafety.PTMS.Share.ApplicationContext.Instance.StringResourceReader.TranslateDataGrid(DataGridMonitorList);
                    comboStatus.DropDownOpened += PopupHandler.OnDropDown;
                    comboStatus.DropDownClosed += PopupHandler.OnDropDown;

                    comboStatus2.DropDownOpened += PopupHandler.OnDropDown;
                    comboStatus2.DropDownClosed += PopupHandler.OnDropDown;

                }
                catch (Exception)
                {

                }

                m_VehicleDragPanel.HorizontalAlignment = HorizontalAlignment.Left;
                m_VehicleDragPanel.Margin = new Thickness(0, 0, 0, 0);
                m_VehicleDragPanel.VerticalAlignment = VerticalAlignment.Top;
                m_VehicleDragPanel.IsDrag = false;
                m_VehicleDragPanel.HorizontalAlignment = HorizontalAlignment.Stretch;
                this.BasicInfoArea.Child = m_VehicleDragPanel;

                AlarmDragPanel.HorizontalAlignment = HorizontalAlignment.Left;
                AlarmDragPanel.Margin = new Thickness(0, 0, 0, 0);
                AlarmDragPanel.VerticalAlignment = VerticalAlignment.Top;
                AlarmDragPanel.IsDrag = false;
                AlarmDragPanel.HorizontalAlignment = HorizontalAlignment.Stretch;
                this.AlarmInfoArea.Child = AlarmDragPanel;

                alertinfopage.HorizontalAlignment = HorizontalAlignment.Left;
                alertinfopage.Margin = new Thickness(0, 0, 0, 0);
                alertinfopage.VerticalAlignment = VerticalAlignment.Top;
                alertinfopage.HorizontalAlignment = HorizontalAlignment.Stretch;
                //alertinfopage.IsDrag = false;
                this.AlertInfoArea.Child = alertinfopage;

                alarmhandle.HorizontalAlignment = HorizontalAlignment.Left;
                alarmhandle.Margin = new Thickness(0, 40, 0, 0);
                alarmhandle.VerticalAlignment = VerticalAlignment.Top;
                alarmhandle.IsDrag = true;

                this.ContentGrid.Children.Add(alarmhandle);

                selectfence.HorizontalAlignment = HorizontalAlignment.Left;
                selectfence.Margin = new Thickness(0, 40, 0, 0);
                selectfence.VerticalAlignment = VerticalAlignment.Top;
                selectfence.IsDrag = true;

                this.ContentGrid.Children.Add(selectfence);

                alerthandle.HorizontalAlignment = HorizontalAlignment.Left;
                alerthandle.Margin = new Thickness(0, 40, 0, 0);
                alerthandle.VerticalAlignment = VerticalAlignment.Top;
                alerthandle.IsDrag = true;

                this.ContentGrid.Children.Add(alerthandle);

                manualhandle.HorizontalAlignment = HorizontalAlignment.Left;
                manualhandle.Margin = new Thickness(0, 40, 0, 0);
                manualhandle.VerticalAlignment = VerticalAlignment.Top;
                manualhandle.IsDrag = true;

                this.ContentGrid.Children.Add(manualhandle);

                Gsafety.PTMS.Share.ApplicationContext.Instance.StringResourceReader.TranslateDataGrid(dgAlarm);
                Gsafety.PTMS.Share.ApplicationContext.Instance.StringResourceReader.TranslateDataGrid(dgAlert);
                Gsafety.PTMS.Share.ApplicationContext.Instance.StringResourceReader.TranslateDataGrid(dgDeviceAlert);

            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("AntProductMonitorMainPage", ex);
            }
        }


        private void LayoutRoot_MouseRightButtonUp_1(object sender,

System.Windows.Input.MouseButtonEventArgs e)
        {
            e.Handled = true;
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



        //最左边隐藏
        private void hiddenL_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var collapseAnimation = (Storyboard)Resources["collapseTransition"];
            collapseAnimation.Completed += (m, n) =>
            {
                SythesesLeftContent.Visibility = Visibility.Collapsed;
                collapsedPane.Visibility = Visibility.Visible;
                collapsedPane.Opacity = 0.8;
                if (GoRightcollapsedPane.Visibility == Visibility.Visible)
                {
                    ContentBorder.SetValue(Grid.ColumnProperty, 0);
                    ContentBorder.SetValue(Grid.ColumnSpanProperty, 3);
                }
                else
                {
                    ContentBorder.SetValue(Grid.ColumnProperty, 0);
                    ContentBorder.SetValue(Grid.ColumnSpanProperty, 2);
                }

            };
            collapseAnimation.Begin();
        }

        //左边区域显示按钮事件
        private void showL_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var collapseAnimation = (Storyboard)Resources["expandTransition"];
            collapseAnimation.Begin();
            //GISContent.SetValue(Grid.ColumnProperty, 1);
            //GISContent.Margin = new Thickness(0, 0, 0, 0);
            //右边区域隐藏了，GIS要占用右边的区域
            if (GoRightcollapsedPane.Visibility == Visibility.Visible)
            {
                ContentBorder.SetValue(Grid.ColumnProperty, 1);
                ContentBorder.SetValue(Grid.ColumnSpanProperty, 2);
            }
            else
            {
                ContentBorder.ClearValue(Grid.ColumnSpanProperty);
                ContentBorder.SetValue(Grid.ColumnProperty, 1);
            }
            ContentBorder.Margin = new Thickness(0, 0, 0, 0);
            SythesesLeftContent.Visibility = Visibility.Visible;
            collapsedPane.Visibility = Visibility.Collapsed;
        }


        private void AccordionItem_Selected_Alarm(object sender, RoutedEventArgs e)
        {
            dgAlarm.Visibility = System.Windows.Visibility.Visible;
            dgAlert.Visibility = System.Windows.Visibility.Collapsed;
            dgDeviceAlert.Visibility = System.Windows.Visibility.Collapsed;
        }

        private void AccordionItem_Selected_Alert(object sender, RoutedEventArgs e)
        {
            dgAlarm.Visibility = System.Windows.Visibility.Collapsed;
            dgAlert.Visibility = System.Windows.Visibility.Visible;
            dgDeviceAlert.Visibility = System.Windows.Visibility.Collapsed;
        }

        private void AccordionItem_Selected_DeviceAlert(object sender, RoutedEventArgs e)
        {
            dgAlarm.Visibility = System.Windows.Visibility.Collapsed;
            dgAlert.Visibility = System.Windows.Visibility.Collapsed;
            dgDeviceAlert.Visibility = System.Windows.Visibility.Visible;
        }

        private double mapActualHeight { get; set; }
        private void AccordionItem_Selected_Monitor(object sender, RoutedEventArgs e)
        {
            this.DataGridMonitorList.Visibility = Visibility.Visible;
            mapActualHeight = ContentBorder.ActualHeight;

            dgAlarm.Visibility = System.Windows.Visibility.Collapsed;
            dgAlert.Visibility = System.Windows.Visibility.Collapsed;
            dgDeviceAlert.Visibility = System.Windows.Visibility.Collapsed;
        }

        private void stackPanel_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            var panel = sender as FrameworkElement;

            for (int i = 0; i < 20; i++)
            {
                panel = VisualTreeHelper.GetParent(panel) as FrameworkElement;
                if (panel is TreeViewItem)
                {
                    var item = panel as TreeViewItem;
                    if (item.IsSelected == false)
                    {
                        item.IsSelected = true;
                    }
                    else
                    {
                        item.IsSelected = false;
                        item.IsSelected = true;
                    }
                    treeViewContextMenu.Visibility = Visibility.Visible;

                    break;
                }
            }
        }

        private void DataGridMonitorList_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.MouseRightButtonDown += (s, a) =>
                {
                    var dataGrid = sender as DataGrid;
                    var currentIndex = (s as DataGridRow).GetIndex();
                    if (dataGrid.SelectedIndex != currentIndex)
                    {
                        dataGrid.SelectedIndex = currentIndex;
                        (s as DataGridRow).Focus();
                    }

                    var contextMenu = ContextMenuService.GetContextMenu(dataGrid);
                    contextMenu.Visibility = Visibility.Visible;

                    //else
                    //{
                    //    dataGrid.SelectedIndex = -1;
                    //    dataGrid.SelectedIndex = currentIndex;
                    //}
                    //a.Handled = true;
                };

            e.Row.MouseLeftButtonUp += (s, a) =>
            {
                var dataGrid = sender as DataGrid;

                var currentIndex = (s as DataGridRow).GetIndex();
                if (dataGrid.SelectedIndex != currentIndex)
                {
                    dataGrid.SelectedIndex = currentIndex;
                    (s as DataGridRow).Focus();
                }
                else
                {
                    dataGrid.SelectedIndex = -1;
                    dataGrid.SelectedIndex = currentIndex;
                }
                a.Handled = true;
            };
        }


        //最右边向右隐藏点击按钮
        private void GoRighthiddenL_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var collapseAnimation = (Storyboard)Resources["GoRightCollapseTransition"];
            collapseAnimation.Completed += (m, n) =>
            {
                InfoShowGridRegion.Visibility = Visibility.Collapsed;
                if (collapsedPane.Visibility == Visibility.Visible)
                {
                    //InfoShowGridRegion.Visibility = Visibility.Collapsed;
                    ContentBorder.SetValue(Grid.ColumnProperty, 0);
                    ContentBorder.SetValue(Grid.ColumnSpanProperty, 3);
                }
                else
                {

                    ContentBorder.SetValue(Grid.ColumnProperty, 1);
                    ContentBorder.SetValue(Grid.ColumnSpanProperty, 2);

                }
                GoRightcollapsedPane.Visibility = Visibility.Visible;
                collapsedPane.Opacity = 0.8;
            };
            collapseAnimation.Begin();
        }

        /// <summary>
        /// 最右边点击显示按钮触发的事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GoRightShow_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var collapseAnimation = (Storyboard)Resources["GoRightExpandTransition"];
            collapseAnimation.Begin();
            // ListBottomContent.Visibility = Visibility.Visible;
            //  ListCollapsedPane.Visibility = Visibility.Collapsed;

            //最左边也隐藏了
            if (collapsedPane.Visibility == Visibility.Visible)
            {
                ContentBorder.SetValue(Grid.ColumnSpanProperty, 2);
                ContentBorder.SetValue(Grid.ColumnProperty, 0);
            }
            else
            {
                ContentBorder.ClearValue(Grid.ColumnSpanProperty);
                ContentBorder.SetValue(Grid.ColumnProperty, 1);

                //SythesesLeftContent.Visibility = Visibility.Visible;
                //SythesesLeftContent.SetValue(Grid.ColumnProperty,0);
            }
            ContentBorder.Margin = new Thickness(0, 0, 0, 0);
            InfoShowGridRegion.SetValue(Grid.ColumnProperty, 2);
            InfoShowGridRegion.Visibility = Visibility.Visible;
            GoRightcollapsedPane.Visibility = Visibility.Collapsed;
        }

        private void ContextMenu_Loaded(object sender, RoutedEventArgs e)
        {
            var menu = sender as ContextMenu;
            foreach (Button button in menu.Items)
            {
                button.Click -= button_Click;
                button.Click += button_Click;
            }
            menu.Closed -= menu_Closed;
            menu.Closed += menu_Closed;
        }

        void menu_Closed(object sender, RoutedEventArgs e)
        {
            var menu = sender as ContextMenu;
            menu.Visibility = Visibility.Collapsed;
        }

        void button_Click(object sender, RoutedEventArgs e)
        {
            var contextMenu = (sender as Button).Parent as ContextMenu;
            if (contextMenu != null && contextMenu.IsOpen)
            {
                contextMenu.IsOpen = false;
            }
        }

        private void stackPanel_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var panel = sender as FrameworkElement;

            for (int i = 0; i < 20; i++)
            {
                panel = VisualTreeHelper.GetParent(panel) as FrameworkElement;
                if (panel is TreeViewItem)
                {
                    var item = panel as TreeViewItem;
                    if (item.IsSelected == false)
                    {
                        item.IsSelected = true;
                    }
                    else
                    {
                        item.IsSelected = false;
                        item.IsSelected = true;
                    }
                    break;
                }
            }
        }
    }
}
