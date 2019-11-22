using Gsafety.PTMS.Share;
using Jounce.Core.View;
using Jounce.Regions.Core;
/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 2a22816b-8b95-4c4f-b62e-8a5225876983      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-DENGZL
/////                 Author: TEST(dengzl)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Monitor.Views
/////    Project Description:    
/////             Class Name: MonitorMainPage
/////          Class Version: v1.0.0.0
/////            Create Time: 8/6/2013 10:58:59 AM
/////      Class Description:  
/////======================================================================
/////          Modified Time: 8/6/2013 10:58:59 AM
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;
using System.Windows.Navigation;
namespace Gsafety.PTMS.Monitor.Views
{
    //[ExportAsView(MonitorName.MonitorMainPageV, Category = "Navigation", MenuName = "VehiclesMonitor", ToolTip = "Click to view some text.")]
    //[ExportViewToRegion(MonitorName.MonitorMainPageV, ViewContainer.CentralMainContainer)]
    //[ExportAsView(MonitorName.MonitorMainPageV, Category = "Navigation", MenuName = "VehiclesMonitor", ToolTip = "Click to view some text.")]
    //[ExportViewToRegion(MonitorName.MonitorMainPageV, "CentralMainContainer2")]
    public partial class MonitorMainPage : UserControl
    {

        VehicleInfoPage m_VehicleDragPanel = new VehicleInfoPage();
        public MonitorMainPage()
        {
            InitializeComponent();
            m_VehicleDragPanel.HorizontalAlignment = HorizontalAlignment.Left;
            m_VehicleDragPanel.Margin = new Thickness(0, 40, 0, 0);
            m_VehicleDragPanel.VerticalAlignment = VerticalAlignment.Top;
            m_VehicleDragPanel.IsDrag = true;
            this.ContentGrid.Children.Add(m_VehicleDragPanel);
        }

        [ExportAsView(MonitorName.MonitorMenuV)]
        public UserControl MonitorMenu
        {
            get { return monitorMenu; }
        }

        [ExportAsView(MonitorName.VehicleInfoView)]
        public UserControl VehicleDragPanel
        {
            get { return m_VehicleDragPanel; }
        }

        private void ContentFrame_Navigated(object sender, NavigationEventArgs e)
        {

        }

        // If an error occurs during navigation, show an error window
        private void ContentFrame_NavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            e.Handled = true;
        }

        private void ContentFrame_Drop_1(object sender, DragEventArgs e)
        {

        }

        private void ContentFrame_DragEnter_1(object sender, DragEventArgs e)
        {

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
                SythesesLeftContent.Visibility = Visibility.Collapsed;
                GISContent.SetValue(Grid.ColumnProperty, 0);
                GISContent.SetValue(Grid.ColumnSpanProperty, 2);
                //GisRightContent.Margin = new Thickness(0, 0, 0, 0);
                collapsedPane.Visibility = Visibility.Visible;
                collapsedPane.Opacity = 0.8;
            };
            collapseAnimation.Begin();

        }

        private void showL_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var collapseAnimation = (Storyboard)Resources["expandTransition"];

            collapseAnimation.Begin();

            GISContent.SetValue(Grid.ColumnProperty, 1);
            //ContentBorder.SetValue(Grid.ColumnSpanProperty, 0);
            GISContent.Margin = new Thickness(0, 0, 0, 0);
            SythesesLeftContent.Visibility = Visibility.Visible;
            collapsedPane.Visibility = Visibility.Collapsed;
        }

    }
}
