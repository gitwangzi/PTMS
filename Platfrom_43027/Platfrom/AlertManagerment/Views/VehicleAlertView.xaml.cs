using Gsafety.PTMS.Constants;
using Jounce.Core.View;
using Jounce.Core.ViewModel;
using Jounce.Regions.Core;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Gsafety.Common.Utilities;
using Gsafety.PTMS.Alert.ViewModels;

namespace Gsafety.PTMS.Alert.Views
{
    [ExportAsView(AlertName.VehicleAlertView)]
    [ExportViewToRegion(AlertName.VehicleAlertView, ViewContainer.CentralMainContainer)]
    public partial class VehicleAlertView : UserControl
    {
        GisManagement.ViewModels.GisViewModel _alertGisViewModel = new GisManagement.ViewModels.GisViewModel();
        Gsafety.PTMS.Alert.ViewModels.AlertDetailPageViewModel _alertDetailViewModel = new ViewModels.AlertDetailPageViewModel();
        AlertDetailPage alertinfopage = new AlertDetailPage();

        [ExportAsView(AlertName.AlertDetailPage)]
        public UserControl AlertDetailView
        {
            get { return alertinfopage; }
        }

        public VehicleAlertView()
        {
            InitializeComponent();
            //HandleGrid
            Gsafety.PTMS.Share.ApplicationContext.Instance.StringResourceReader.TranslateDataGrid(UnhandleGrid); 
            Gsafety.PTMS.Share.ApplicationContext.Instance.StringResourceReader.TranslateDataGrid(HandleGrid);
            alertinfopage.HorizontalAlignment = HorizontalAlignment.Left;
            alertinfopage.Margin = new Thickness(0,40,0,0);
            alertinfopage.VerticalAlignment = VerticalAlignment.Top;
            alertinfopage.IsDrag = true;
            this.ContentGrid.Children.Add(alertinfopage);
            this.Accordion.SelectedIndex = 0;
            HandleGrid.DoubleClickHook(HandleGridDbClick_CallBack);
            UnhandleGrid.DoubleClickHook(UnHandleGridDbClick_CallBack);
        }

        #region method
        private void HandleGridDbClick_CallBack(object source)
        {
            GridDbClickOper(HandleGrid);
        }

        private void UnHandleGridDbClick_CallBack(object source)
        {
            GridDbClickOper(UnhandleGrid);
        }

        private void GridDbClickOper(DataGrid dg)
        {
            if (dg.SelectedItem != null)
            {
                ((VehicleAlertViewModel)this.LayoutRoot.DataContext).OpenDetailViewClick_Event(null);
            }

        }
        #endregion

        #region event
        private void ContentFrame_Navigated(object sender, NavigationEventArgs e)
        {

        }

        // If an error occurs during navigation, show an error window
        private void ContentFrame_NavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            e.Handled = true;
        }

        //Shows the effect of control on the left side of the container
        private void hiddenL_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var collapseAnimation = (Storyboard)Resources["collapseTransition"];
            collapseAnimation.Completed += (m, n) =>
            {
                SuiteLeftContent.Visibility = Visibility.Collapsed;
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
            SuiteLeftContent.Visibility = Visibility.Visible;
            collapsedPane.Visibility = Visibility.Collapsed;
        }
        #endregion
    }
}
