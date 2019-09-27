/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: cfc4d93b-bfde-479a-ae66-8b50ed183f95      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: WANGJINCAI
/////                 Author: TEST(wangjc)
/////======================================================================
/////           Project Name: Gsafety.PTMS.CurrentUserManagement.Views
/////    Project Description:    
/////             Class Name: CurrentUserManagementContiner
/////          Class Version: v1.0.0.0
/////            Create Time: 2013-12-16 13:44:33
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013-12-16 13:44:33
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
using Jounce.Core.View;
using Jounce.Regions.Core;
using Gsafety.PTMS.Constants;

namespace Gsafety.PTMS.CurrentUserManagement.Views
{
    [ExportAsView(CurrentUserName.CurrentUserView, Category = "Navigation", MenuName = "Current User", ToolTip = "Click to view some text.")]
    [ExportViewToRegion(CurrentUserName.CurrentUserView, ViewContainer.CentralMainContainer)]
    public partial class CurrentUserManagementContiner : Page
    {
        public CurrentUserManagementContiner()
        {
            InitializeComponent();
            this.MouseRightButtonDown += ChildWindow_MouseRightButtonDown;
        }

        [ExportAsView(CurrentUserName.CurrentUserView)]
        public UserControl CurrentUserManagementMenuControl
        {
            get { return currentUserManagementMenu; }
        }


        private void hiddenL_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var collapseAnimation = (Storyboard)this.LayoutRoot.Resources["collapseTransition"];
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
            var collapseAnimation = (Storyboard)this.LayoutRoot.Resources["expandTransition"];
            collapseAnimation.Begin();
            ContentBorder.SetValue(Grid.ColumnProperty, 1);
            ContentBorder.Margin = new Thickness(0, 0, 0, 0);
            SythesesLeftContent.Visibility = Visibility.Visible;
            collapsedPane.Visibility = Visibility.Collapsed;
        }


        private void ContentFrame_Navigated(object sender, NavigationEventArgs e)
        {
            var spPanel = this.CurrentUserManagementMenuControl.FindName("LinksSPanel") as StackPanel;
            foreach (UIElement child in spPanel.Children)
            {
                HyperlinkButton hb = child as HyperlinkButton;
                if (hb != null && hb.NavigateUri != null)
                {
                    if (hb.NavigateUri.ToString().Equals(e.Uri.ToString()))
                    {
                        VisualStateManager.GoToState(hb, "ActiveLink", true);
                    }
                    else
                    {
                        VisualStateManager.GoToState(hb, "InactiveLink", true);
                    }
                }
            }
        }

        private void ContentFrame_NavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            
        }

        private void ChildWindow_MouseRightButtonDown(object sender,

System.Windows.Input.MouseButtonEventArgs e)
        {
            e.Handled = true;
        }


    }
}
