/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: b6e6a310-4705-4db2-b5c4-494064c552e5      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-DENGZL
/////                 Author: TEST(dengzl)
/////======================================================================
/////           Project Name: Gsafety.PTMS.SecuritySuite.Views
/////    Project Description:    
/////             Class Name: SuiteMainPage
/////          Class Version: v1.0.0.0
/////            Create Time: 8/6/2013 5:38:51 PM
/////      Class Description:  
/////======================================================================
/////          Modified Time: 8/6/2013 5:38:51 PM
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
using System.Windows.Navigation;
using Jounce.Core;
using Jounce.Core.View;
using Jounce.Regions.Core;
using Gsafety.PTMS.Constants;
using Gsafety.PTMS.SecuritySuite;


namespace Gsafety.PTMS.SecuritySuite.Views
{
    [ExportAsView(SecuritySuiteName.SuiteMainPageV, Category = "Navigation", MenuName = "Gis Manager", ToolTip = "Click to view some text.")]
    [ExportViewToRegion(SecuritySuiteName.SuiteMainPageV, ViewContainer.CentralMainContainer)]
    public partial class SuiteMainPage : UserControl
    {
        public SuiteMainPage()
        {
            InitializeComponent();
            
        }

        private void ContentFrame_Navigated(object sender, NavigationEventArgs e)
        {
            bool focusOne = false ;
            foreach (var hb in suiteMenu.NavigationButtons)
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

            if (!focusOne && suiteMenu.NavigationButtons != null && suiteMenu.NavigationButtons.Count > 0)
            {
                VisualStateManager.GoToState(suiteMenu.NavigationButtons[0], "ActiveLink", true);
            }
        }

        // If an error occurs during navigation, show an error window
        private void ContentFrame_NavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            e.Handled = true;

        }

        //控制左侧容器显示隐藏效果
        private void hiddenL_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var collapseAnimation = (Storyboard)Resources["collapseTransition"];
            collapseAnimation.Completed += (m, n) =>
            {
                SythesesLeftContent.Visibility = Visibility.Collapsed;
                ContentBorder.SetValue(Grid.ColumnProperty, 0);
                ContentBorder.SetValue(Grid.ColumnSpanProperty, 2);
                CollapsedPane.Visibility = Visibility.Visible;
                CollapsedPane.Opacity = 0.8;
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
            CollapsedPane.Visibility = Visibility.Collapsed;
        }

        [ExportAsView(SecuritySuiteName.SuiteMenuV)]
        public UserControl SuiteMenu
        {
            get { return suiteMenu; }
        }
    }
}
