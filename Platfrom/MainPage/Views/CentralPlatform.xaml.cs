using Gsafety.PTMS.Constants;
using Jounce.Core.View;
using Jounce.Regions.Core;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;
/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 84d0698e-10c0-42ce-8b60-2bb38305b6d7      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-DENGZL
/////                 Author: TEST(dengzl)
/////======================================================================
/////           Project Name: MainPage
/////    Project Description:    
/////             Class Name: CentralPlatform
/////          Class Version: v1.0.0.0
/////            Create Time: 8/5/2013 1:19:45 PM
/////      Class Description:  
/////======================================================================
/////          Modified Time: 8/5/2013 1:19:45 PM
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System.Windows.Navigation;


namespace Gsafety.PTMS.MainPage.View
{
    [ExportAsView(MainPageName.CentralPlatformV)]
    [ExportViewToRegion(MainPageName.CentralPlatformV, ViewContainer.LoginContainer)]
    public partial class CentralPlatform : UserControl
    {
        public CentralPlatform()
        {
            InitializeComponent();
            //ApplicationContext.Instance.NavigateManager.AddFrame(CentralPlatMainContentFrame);

        }

        // After the Frame navigates, ensure the HyperlinkButton representing the current page is selected
        //private void ContentFrame_Navigated(object sender, NavigationEventArgs e)
        //{
        //    bool FocusOne = false;

        //    foreach (var hb in
        //        LinksStackPanel.Children.Select(child => child as HyperlinkButton).Where(hb => hb != null && hb.NavigateUri != null))
        //    {
        //        if (hb.NavigateUri == e.Uri)
        //        {
        //            FocusOne = true;
        //            VisualStateManager.GoToState(hb, "ActiveLink", true);
        //        }
        //        else
        //        {
        //            VisualStateManager.GoToState(hb, "InactiveLink", true);
        //        }
        //    }

        //    if (!FocusOne)
        //    {
        //        VisualStateManager.GoToState(MoniterLink, "ActiveLink", true);
        //    }

        //    //总数
        //    Binding vehiclesCountBinding = new Binding("BufferManager.DistrictManager.VehiclesCount");
        //    vehiclesCountBinding.Source = ApplicationContext.Instance;
        //    vehiclesCountBinding.Mode = BindingMode.TwoWay;//IsVehicleLoadComplete
        //    VehicleCountTxt.SetBinding(TextBlock.TextProperty, vehiclesCountBinding);

        //    Binding vehiclesRateBinding = new Binding("BufferManager.DistrictManager.OnlineRateView");
        //    vehiclesRateBinding.Source = ApplicationContext.Instance;
        //    vehiclesRateBinding.Mode = BindingMode.TwoWay;
        //    //vehicleOnlineRateGrid.SetBinding(ToolTipService.ToolTipProperty, vehiclesRateBinding);
        //    VehicleOnlineRate.SetBinding(TextBlock.TextProperty, vehiclesRateBinding);
        //    //是否加载完毕
        //    Binding vehiclesLoadCompleteBinding = new Binding("BufferManager.DistrictManager.IsVehicleLoadComplete");
        //    vehiclesLoadCompleteBinding.Source = ApplicationContext.Instance;
        //    vehiclesLoadCompleteBinding.Mode = BindingMode.TwoWay;//
        //    vehiclesLoadCompleteBinding.Converter = new GeneralColourStateConvert();
        //    vehiclesLoadCompleteBinding.ConverterParameter = "VehicleLoadCompleteColour";
        //    //VehicleCountTxt.SetBinding(TextBlock.ForegroundProperty, vehiclesLoadCompleteBinding);


        //    //在线数 
        //    Binding vehicleOnlineCountBinding = new Binding("BufferManager.DistrictManager.VehiclesOnLineCount");
        //    vehicleOnlineCountBinding.Source = ApplicationContext.Instance;
        //    vehicleOnlineCountBinding.Mode = BindingMode.TwoWay;
        //    VehicleOnlineCountTxt.SetBinding(TextBlock.TextProperty, vehicleOnlineCountBinding);
        //    //在线率
        //    Binding vehicleOnlineCountRate = new Binding("OnlineRate");
        //    vehicleOnlineCountRate.Source = ApplicationContext.Instance.BufferManager.DistrictManager;
        //    vehicleOnlineCountRate.Mode = BindingMode.TwoWay;
        //    VehicleOnlineCountRateProgress.SetBinding(ProgressBar.ValueProperty, vehicleOnlineCountRate);

        //    //VehicleOnlineCountRateTxt.SetBinding(TextBlock.TextProperty, vehicleOnlineCountRate);
        //    Binding vehicleOnlineCountRate2 = new Binding("BufferManager.DistrictManager.OnlineRate");
        //    vehicleOnlineCountRate2.Source = ApplicationContext.Instance;
        //    vehicleOnlineCountRate2.Mode = BindingMode.TwoWay;
        //    vehicleOnlineCountRate2.Converter = new GeneralColourStateConvert();
        //    vehicleOnlineCountRate2.ConverterParameter = "OnlineRateColour";
        //    VehicleOnlineCountRateProgress.SetBinding(ProgressBar.ForegroundProperty, vehicleOnlineCountRate2);
        //}

        // If an error occurs during navigation, show an error window
        private void ContentFrame_NavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            e.Handled = true;
        }

        //[ExportAsView(MainPageName.MessageNotifition)]
        //public UserControl MessageNotiry
        //{
        //    get { return messageNotiry; }
        //}

        //private void check_playMusic_Click(object sender, RoutedEventArgs e)
        //{
        //    var url = (this.check_playMusic.IsChecked ?? false) ? "/ExternalResource;component/Images/MainPage_sound.png" : "/ExternalResource;component/Images/MainPage_sound_off.png";
        //    ((BitmapImage)((Image)((ContentControl)sender).Content).Source).UriSource = new Uri(url, UriKind.Relative);

        //}
        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("实时监控");
        }

        /// <summary>
        /// 展开按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void showL_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var collapseAnimation = (Storyboard)Resources["expandTransition"];
            collapseAnimation.Begin();
            ContentBorder.SetValue(Grid.ColumnProperty, 1);
            ContentBorder.Margin = new Thickness(0, 0, 0, 0);
            SythesesLeftContent.Visibility = Visibility.Visible;
            collapsedPane.Visibility = Visibility.Collapsed;

        }

        /// <summary>
        /// 隐藏左边区域按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HideButton_OnClick(object sender, RoutedEventArgs e)
        {
            var collapseAnimation = (Storyboard)Resources["collapseTransition"];
            collapseAnimation.Completed += (m, n) =>
            {
                SythesesLeftContent.Visibility = Visibility.Collapsed;
                ContentBorder.SetValue(Grid.ColumnProperty, 0);
                ContentBorder.SetValue(Grid.ColumnSpanProperty, 2);
                collapsedPane.Visibility = Visibility.Visible;
                SythesesLeftContent.Visibility = Visibility.Collapsed;
                collapsedPane.Opacity = 0.8;
            };
            collapseAnimation.Begin();
        }

        private void GoToEnter(object sender, MouseEventArgs e)
        {
            VisualStateManager.GoToState(this, "Enter", false);
        }

        private void GoToLeave(object sender, MouseEventArgs e)
        {
            VisualStateManager.GoToState(this, "Leave", false);
        }
    }
}
