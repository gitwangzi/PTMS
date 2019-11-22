
using Gsafety.PTMS.Share;
using Gsafety.PTMS.VideoDownloadManagement.Views.ViewPage;
using Jounce.Core.View;
using Jounce.Regions.Core;
/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: dc814c04-197e-4485-80f6-1463cbe47818      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: WANGJINCAI
/////                 Author: TEST(wangjc)
/////======================================================================
/////           Project Name: Gsafety.PTMS.VideoDownloadManagement.Views
/////    Project Description:    
/////             Class Name: ContinerPage
/////          Class Version: v1.0.0.0
/////            Create Time: 2013-11-15 14:01:57
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013-11-15 14:01:57
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;
using System.Windows.Navigation;

namespace Gsafety.PTMS.VideoDownloadManagement.Views
{
    //[ExportAsView(VideoDownloadName.VideoDownLoadMainView, Category = "Navigation", MenuName = "Video DownLoad", ToolTip = "Click to view some text.")]
    //[ExportViewToRegion(VideoDownloadName.VideoDownLoadMainView, ViewContainer.CentralMainContainer)]
    [ExportAsView(VideoDownloadName.VideoDownLoadMainView, Category = "Navigation", MenuName = "Video DownLoad", ToolTip = "Click to view some text.")]
    [ExportViewToRegion(VideoDownloadName.VideoDownLoadMainView, "CentralMainContainer2")]
    public partial class ContinerPage : UserControl
    {
        public ContinerPage()
        {
            InitializeComponent();
            ApplicationContext.Instance.NavigateManager.AddFrame(this.VideoFrame);
        }

        [ExportAsView(VideoDownloadName.VideoDownLoadMenuView)]
        public UserControl VedioDownLoadControl
        {
            get { return videoDownLoadMenu; }
        }


        //Shows the effect of control on the left side of the container
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


        // After the navigation Frame, please make sure that the selected HyperlinkButton said the current page
        private void ContentFrame_Navigated(object sender, NavigationEventArgs e)
        {
            bool isNavigated = false;
            foreach (UIElement child in videoDownLoadMenu.LinksStackPanel.Children)
            {
                HyperlinkButton hb = child as HyperlinkButton;
                if (hb != null && hb.NavigateUri != null)
                {
                    //if (hb.NavigateUri.ToString().Equals(e.Uri.ToString()) && !isNavigated
                    //    && ApplicationContext.Instance.AuthenticationInfo.FunctionNames.Contains(hb.Name))
                    //{
                    //    VisualStateManager.GoToState(hb, "ActiveLink", true);
                    //    isNavigated = true;
                    //}
                    if (hb.NavigateUri.ToString().Equals(e.Uri.ToString()) && !isNavigated)
                    {
                        VisualStateManager.GoToState(hb, "ActiveLink", true);
                        isNavigated = true;
                    }
                    else
                    {
                        VisualStateManager.GoToState(hb, "InactiveLink", true);
                    }
                }
            }
        }

        // If there is an error in the process of navigation, display an error window
        private void ContentFrame_NavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            e.Handled = true;
            ChildWindow errorWin = new ErrorWindow(e.Uri);
            errorWin.Show();
        }
    }
}
