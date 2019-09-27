/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 0169ab3a-51c1-4a9f-b5d6-8e5d03d41caa      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-DENGZL
/////                 Author: TEST(dengzl)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Alarm.Views
/////    Project Description:    
/////             Class Name: AlarmMainPage
/////          Class Version: v1.0.0.0
/////            Create Time: 8/6/2013 5:35:25 PM
/////      Class Description:  
/////======================================================================
/////          Modified Time: 8/6/2013 5:35:25 PM
/////            Modified by:
/////   Modified Description: 
/////======================================================================
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
using Jounce.Core;
using Jounce.Core.View;
using Jounce.Regions.Core;
using Jounce.Core.ViewModel;
using System.Windows.Navigation;
using Gsafety.PTMS.Constants;

namespace Gsafety.PTMS.Alarm.Views
{
    [ExportAsView(AlarmName.AlarmMainView, Category = "Navigation", MenuName = "VehiclesMonitor", ToolTip = "Click to view some text.")]
    [ExportViewToRegion(AlarmName.AlarmMainView, ViewContainer.CentralMainContainer)]
    public partial class AlarmMainPage : UserControl
    {   
        AlarmInfoPage AlarmDragPanel = new AlarmInfoPage();   
        public AlarmMainPage()
        {
            InitializeComponent();
            
            AlarmDragPanel.HorizontalAlignment = HorizontalAlignment.Left;
            AlarmDragPanel.Margin = new Thickness(0,40,0,0);
            AlarmDragPanel.VerticalAlignment = VerticalAlignment.Top;            
            AlarmDragPanel.IsDrag = true;
            this.GridContent.Children.Add(AlarmDragPanel);
        }

        [ExportAsView(AlarmName.AlarmMenuView)]
        public AlarmMenuPage AlarmMenuView
        {
            get { return alarmMenuView; }
        }

        [ExportAsView(AlarmName.AlarmInfoView)]
        public UserControl AlarmInfoControl
        {
            get { return AlarmDragPanel; }
        }

        #region event
        private void ContentFrame_Navigated(object sender, NavigationEventArgs e)
        {
            
        }

        // If an error occurs during navigation, show an error window
        private void ContentFrame_NavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            e.Handled = true;
        }

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
        #endregion
    }
}
