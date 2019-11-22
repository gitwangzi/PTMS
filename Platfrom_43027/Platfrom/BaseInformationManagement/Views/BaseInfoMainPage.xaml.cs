/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 3569c024-31ff-4a00-ab77-b39ba7d8086c      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-DENGZL
/////                 Author: TEST(dengzl)
/////======================================================================
/////           Project Name: Gsafety.PTMS.BaseInformation.Views
/////    Project Description:    
/////             Class Name: BaseInforMainPage
/////          Class Version: v1.0.0.0
/////            Create Time: 8/6/2013 5:36:45 PM
/////      Class Description:  
/////======================================================================
/////          Modified Time: 8/6/2013 5:36:45 PM
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

namespace Gsafety.PTMS.BaseInformation.Views
{
    [ExportAsView(BaseInformationName.BaseInfoMainPageV, Category = "Navigation", MenuName = "Gis Manager", ToolTip = "Click to view some text.")]
    [ExportViewToRegion(BaseInformationName.BaseInfoMainPageV, ViewContainer.CentralMainContainer)]
    public partial class BaseInfoMainPage : UserControl
    {
        public BaseInfoMainPage()
        {
            InitializeComponent();
        }

        private void ContentFrame_Navigated(object sender, NavigationEventArgs e)
        {
            bool focusOne = false;
            foreach (var hb in baseInfoMenu.NavigationButtons)
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

            if (!focusOne && baseInfoMenu.NavigationButtons != null && baseInfoMenu.NavigationButtons.Count > 0)
            {
                VisualStateManager.GoToState(baseInfoMenu.NavigationButtons[0], "ActiveLink", true);
            }
        }

        // If an error occurs during navigation, show an error window
        private void ContentFrame_NavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            e.Handled = true;
        }

        [ExportAsView(BaseInformationName.BaseInfoMenuV)]
        public UserControl BaseInfoMenu
        {
            get { return baseInfoMenu; }
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
