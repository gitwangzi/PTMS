/////Copyright (C) Gsafety 2014 .All Rights Reserved.
/////======================================================================
/////                   Guid: df978f6d-bf48-4673-815e-a30efba48fc2      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-ZHOUDD
/////                 Author: GJSY(zhoudd)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Manager
/////    Project Description:    
/////             Class Name: ReportMainPage
/////          Class Version: v1.0.0.0
/////            Create Time: 2014/7/24 11:16:55
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2014/7/24 11:16:55
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using Gsafety.PTMS.Constants;
using Jounce.Core.View;
using Jounce.Regions.Core;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;

namespace Gsafety.PTMS.ReportManager
{
    //[ExportAsView(ReportName.ReportMainPageV, Category = "Navigation", MenuName = "ReportManager")]
    //[ExportViewToRegion(ReportName.ReportMainPageV, ViewContainer.CentralMainContainer)]
    [ExportAsView(ReportName.ReportMainPageV, Category = "Navigation", MenuName = "ReportManager")]
    [ExportViewToRegion(ReportName.ReportMainPageV, "CentralMainContainer2")]
    public partial class ReportMainPage : UserControl
    {
        public ReportMainPage()
        {
            InitializeComponent();
        }
        [ExportAsView(ReportName.ReportMenuV)]
        public UserControl ReporstMenu
        {
            get { return reportMenu; }
        }
        private void ContentFrame_Navigated(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            bool focusOne = false;
            foreach (var hb in reportMenu.NavigationButtons)
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

            if (!focusOne && reportMenu.NavigationButtons != null && reportMenu.NavigationButtons.Count > 0)
            {
                VisualStateManager.GoToState(reportMenu.NavigationButtons[0], "ActiveLink", true);
            }

        }

        private void ContentFrame_NavigationFailed(object sender, System.Windows.Navigation.NavigationFailedEventArgs e)
        {
            e.Handled = true;
        }

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
