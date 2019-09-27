using Gsafety.Common.CommMessage;
using Gsafety.PTMS.Share;
using Gsafety.PTMS.Traffic.ViewModels;
using Jounce.Core.Event;
using Jounce.Core.View;
using Jounce.Regions.Core;
/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 4467b7c8-21bb-4238-a8d1-36e9835d9f81      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-DENGZL
/////                 Author: TEST(dengzl)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Traffic.Views
/////    Project Description:    
/////             Class Name: TrafficMainPage
/////          Class Version: v1.0.0.0
/////            Create Time: 8/6/2013 5:38:35 PM
/////      Class Description:  
/////======================================================================
/////          Modified Time: 8/6/2013 5:38:35 PM
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System;
using System.ComponentModel.Composition;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;
using System.Windows.Navigation;

namespace Gsafety.PTMS.Traffic.Views
{
    [ExportAsView(TrafficName.TrafficMainPage)]
    [ExportViewToRegion(TrafficName.TrafficMainPage, "CentralMainContainer2")]
    public partial class TrafficMainPage : UserControl
    {
        [Import]
        public IEventAggregator EventAggregator { get; set; }
        public TrafficMainPage()
        {
            InitializeComponent();
            this.Loaded += TrafficMainPage_Loaded;

            Gsafety.PTMS.Share.ApplicationContext.Instance.StringResourceReader.TranslateDataGrid(ListDataGrid);
        }


        void TrafficMainPage_Loaded(object sender, RoutedEventArgs e)
        {
            if (ContentFrame.CurrentSource != null)
                this.ContentFrame.Refresh();
        }


        private void ContentFrame_Navigated(object sender, NavigationEventArgs e)
        {
            bool focusOne = false;
            foreach (var hb in trafficMenu.NavigationButtons)
            {
                #region MyRegion
                if (hb.NavigateUri == e.Uri)
                {
                    focusOne = true;
                    VisualStateManager.GoToState(hb, "ActiveLink", true);
                }
                else
                {
                    VisualStateManager.GoToState(hb, "InactiveLink", true);
                }
                #endregion
            }

            if (!focusOne && trafficMenu.NavigationButtons != null && trafficMenu.NavigationButtons.Count > 0)
            {
                VisualStateManager.GoToState(trafficMenu.NavigationButtons[0], "ActiveLink", true);
            }
        }

        private void ContentFrame_NavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            e.Handled = true;
        }

        [ExportAsView(TrafficName.TrafficMenuV)]
        public UserControl TrafficMenu
        {
            get { return trafficMenu; }
        }

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
                    GISContent.SetValue(Grid.ColumnProperty, 0);
                    GISContent.SetValue(Grid.ColumnSpanProperty, 3);
                }
                else
                {
                    GISContent.SetValue(Grid.ColumnProperty, 0);
                    GISContent.SetValue(Grid.ColumnSpanProperty, 2);
                }

                //GISContent.SetValue(Grid.ColumnProperty, 0);
                //GISContent.SetValue(Grid.ColumnSpanProperty, 2);

            };
            collapseAnimation.Begin();
        }

        private void showL_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var collapseAnimation = (Storyboard)Resources["expandTransition"];

            collapseAnimation.Begin();

            if (GoRightcollapsedPane.Visibility == Visibility.Visible)
            {
                GISContent.SetValue(Grid.ColumnProperty, 1);
                GISContent.SetValue(Grid.ColumnSpanProperty, 2);
            }
            else
            {
                GISContent.ClearValue(Grid.ColumnSpanProperty);
                GISContent.SetValue(Grid.ColumnProperty, 1);
            }


            GISContent.Margin = new Thickness(0, 0, 0, 0);
            SythesesLeftContent.Visibility = Visibility.Visible;
            collapsedPane.Visibility = Visibility.Collapsed;
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

        private void ListhiddenL_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var collapseAnimation = (Storyboard)Resources["ListCollapseTransition"];
            collapseAnimation.Completed += (m, n) =>
            {
                ListBottomContent.Visibility = Visibility.Collapsed;
                ContentGrid.SetValue(Grid.RowProperty, 0);
                ContentGrid.SetValue(Grid.RowSpanProperty, 3);
                ListCollapsedPane.Visibility = Visibility.Visible;
                ListCollapsedPane.Opacity = 0.8;
            };
            collapseAnimation.Begin();
        }

        private void ListshowL_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var collapseAnimation = (Storyboard)Resources["ListExpandTransition"];
            collapseAnimation.Begin();
            ListBottomContent.Visibility = Visibility.Visible;
            ListCollapsedPane.Visibility = Visibility.Collapsed;
            ContentGrid.SetValue(Grid.RowProperty, 0);
            ContentGrid.SetValue(Grid.RowSpanProperty, 1);
        }

        private void OperatorCollapseGridImage_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.ShowInfoScrollViewer.Visibility = Visibility.Collapsed;
            this.OperatorCollapseGridImage.Visibility = Visibility.Collapsed;
            this.OperatorExpendGridImage.Visibility = Visibility.Visible;
        }

        private void OperatorExpendGridImage_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.ShowInfoScrollViewer.Visibility = Visibility.Visible;
            this.OperatorCollapseGridImage.Visibility = Visibility.Visible;
            this.OperatorExpendGridImage.Visibility = Visibility.Collapsed;
        }

        private void GoRighthiddenL_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var collapseAnimation = (Storyboard)Resources["ListCollapseTransition"];
            collapseAnimation.Completed += (m, n) =>
            {
                //ListBottomContent.Visibility = Visibility.Collapsed;
                //ContentGrid.SetValue(Grid.RowProperty, 0);
                //ContentGrid.SetValue(Grid.RowSpanProperty, 3);
                //ListCollapsedPane.Visibility = Visibility.Visible;
                //ListCollapsedPane.Opacity = 0.8;
                ListBottomContent.Visibility = Visibility.Collapsed;
                if (collapsedPane.Visibility == Visibility.Visible)
                {
                    GISContent.SetValue(Grid.ColumnProperty, 0);
                    GISContent.SetValue(Grid.ColumnSpanProperty, 3);
                }
                else
                {
                    GISContent.SetValue(Grid.ColumnProperty, 1);
                    GISContent.SetValue(Grid.ColumnSpanProperty, 2);
                }
                GoRightcollapsedPane.Visibility = Visibility.Visible;
                GoRightcollapsedPane.Opacity = 0.8;
            };
            collapseAnimation.Begin();
        }

        private void GoRightShow_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var collapseAnimation = (Storyboard)Resources["ListExpandTransition"];
            collapseAnimation.Begin();
            ListBottomContent.Visibility = Visibility.Visible;
            GoRightcollapsedPane.Visibility = Visibility.Collapsed;

            if (collapsedPane.Visibility == Visibility.Visible)
            {
                GISContent.SetValue(Grid.ColumnProperty, 0);
                GISContent.SetValue(Grid.ColumnSpanProperty, 2);
            }
            else
            {
                GISContent.ClearValue(Grid.ColumnSpanProperty);
                GISContent.SetValue(Grid.ColumnProperty, 1);
                //GISContent.SetValue(Grid.ColumnSpanProperty, 2);
            }

            //GISContent.SetValue(Grid.ColumnProperty, 1);
            GISContent.Margin = new Thickness(0, 0, 0, 0);
        }
    }
}
