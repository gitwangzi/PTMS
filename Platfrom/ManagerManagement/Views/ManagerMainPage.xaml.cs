/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 95b139d3-c23f-488d-b566-01939410a985      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-DENGZL
/////                 Author: TEST(dengzl)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Manager.Views
/////    Project Description:    
/////             Class Name: ManagerMainPage
/////          Class Version: v1.0.0.0
/////            Create Time: 8/6/2013 5:37:30 PM
/////      Class Description:  
/////======================================================================
/////          Modified Time: 8/6/2013 5:37:30 PM
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
using System.Windows.Navigation;
using Gsafety.PTMS.Constants;

namespace Gsafety.PTMS.Manager.Views
{
    [ExportAsView(ManagerName.ManagerMainPageV)]
    [ExportViewToRegion(ManagerName.ManagerMainPageV, ViewContainer.CentralMainContainer)]
    public partial class ManagerMainPage : UserControl
    {
        public ManagerMainPage()
        {
            InitializeComponent();
            this.MouseRightButtonDown += ChildWindow_MouseRightButtonDown;
        }
        private void ChildWindow_MouseRightButtonDown(object sender,

System.Windows.Input.MouseButtonEventArgs e)
        {
            e.Handled = true;
        }

        private void ContentFrame_Navigated(object sender, NavigationEventArgs e)
        {
            bool focusOne = false;
            foreach (var hb in managerMenu.NavigationButtons)
            {
                if (hb.NavigateUri == e.Uri)
                {
                    focusOne = true;
                    VisualStateManager.GoToState(hb, "ActiveLink", true);
                }
                else
                {
                    VisualStateManager.GoToState(hb, "InactiveLink", true);
                }
            }

            if (!focusOne && managerMenu.NavigationButtons != null && managerMenu.NavigationButtons.Count > 0)
            {
                VisualStateManager.GoToState(managerMenu.NavigationButtons[0], "ActiveLink", true);
            }
        }

        // If an error occurs during navigation, show an error window
        private void ContentFrame_NavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            e.Handled = true;
        }

        [ExportAsView(ManagerName.ManagerMenuV)]
        public UserControl ManagerMenu
        {
            get { return managerMenu; }
        }


        private void hiddenL_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var collapseAnimation = (Storyboard)Resources["collapseTransition"];
            collapseAnimation.Completed += (m, n) =>
            {
                SythesesLeftContent.Visibility = Visibility.Collapsed;
                ContentBorder.SetValue(Grid.ColumnProperty, 0);
                ContentBorder.SetValue(Grid.ColumnSpanProperty, 2);
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

            ContentBorder.SetValue(Grid.ColumnProperty, 1);
            //ContentBorder.SetValue(Grid.ColumnSpanProperty, 0);
            ContentBorder.Margin = new Thickness(0, 0, 0, 0);
            SythesesLeftContent.Visibility = Visibility.Visible;
            collapsedPane.Visibility = Visibility.Collapsed;
        }
    }
}
