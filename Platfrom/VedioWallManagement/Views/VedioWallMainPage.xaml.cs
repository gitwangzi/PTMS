/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 2a65e8bc-9d30-4e2d-a50f-768080e259e6      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-DENGZL
/////                 Author: TEST(dengzl)
/////======================================================================
/////           Project Name: Gsafety.PTMS.VedioWall.Views
/////    Project Description:    
/////             Class Name: VedioWallMainPage
/////          Class Version: v1.0.0.0
/////            Create Time: 9/11/2013 4:41:43 PM
/////      Class Description:  
/////======================================================================
/////          Modified Time: 9/11/2013 4:41:43 PM
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
using Jounce.Core.View;
using Jounce.Regions.Core;
using Gsafety.PTMS.Constants;

namespace Gsafety.PTMS.VedioWall.Views
{
    [ExportAsView(VideoManagementName.VedioWallMainPageV, Category = "Navigation", MenuName = "VedioWall", ToolTip = "Click to view some text.")]
    [ExportViewToRegion(VideoManagementName.VedioWallMainPageV, ViewContainer.CentralMainContainer2)]
    public partial class VedioWallMainPage : UserControl
    {
        public VedioWallMainPage()
        {
            InitializeComponent();
        }

        [ExportAsView(VideoManagementName.VedioWallMenuV)]
        public UserControl VedioWallMenuControl
        {
            get { return vedioWallMenu; }
        }

        [ExportAsView(VideoManagementName.VedioWallControlV)]
        public UserControl VedioWallControl
        {
            get { return vedioDisplay; }
        }

        //Show hidden container control on the left side effect
        private void hiddenL_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var collapseAnimation = (Storyboard)Resources["collapseTransition"];
            collapseAnimation.Completed += (m, n) =>
            {
                SythesesLeftContent.Visibility = Visibility.Collapsed;
                VedioContent.SetValue(Grid.ColumnProperty, 0);
                VedioContent.SetValue(Grid.ColumnSpanProperty, 2);
                //GisRightContent.Margin = new Thickness(0, 0, 0, 0);
                //VedioContent.Margin = new Thickness(10, 0, 10, 0);
                collapsedPane.Visibility = Visibility.Visible;
                collapsedPane.Opacity = 0.8;
            };
            collapseAnimation.Begin();
        }

        private void showL_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var collapseAnimation = (Storyboard)Resources["expandTransition"];

            collapseAnimation.Begin();

            VedioContent.SetValue(Grid.ColumnProperty, 1);
            VedioContent.SetValue(Grid.ColumnSpanProperty, 1);
            //VedioContent.Margin = new Thickness(0, 0, 0, 0);
            SythesesLeftContent.Visibility = Visibility.Visible;
            collapsedPane.Visibility = Visibility.Collapsed;
        }
    }
}
