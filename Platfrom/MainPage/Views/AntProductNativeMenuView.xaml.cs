using Gsafety.PTMS.MainPage;
using Gsafety.PTMS.Share;
using Jounce.Core.View;
using System.Windows;
using System.Windows.Controls;

namespace Gsafety.Ant.MainPage.Views
{
    [ExportAsView(MainPageName.AntProductNativeMenuV)]
    public partial class AntProductNativeMenuView : ChildWindow
    {
        /// <summary>
        /// 导航索引
        /// 0是实时监控
        /// 1是历史查询
        /// 2是交通管理
        /// 3是基础信息
        /// 4是统计报表
        /// 5是视频管理
        /// 6是公共服务
        /// 7是系统管理
        /// 8是视频墙
        /// </summary>
        public int Index { get; set; }

        public AntProductNativeMenuView()
        {
            try
            {
                InitializeComponent();
                this.Index = -1; 
                this.MouseRightButtonDown += ChildWindow_MouseRightButtonDown;

            }
            catch(System.Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("AntProductNativeMenuView", ex);
            }
        }

        /// <summary>
        /// 实时监控
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MoniorButton_OnClick(object sender, RoutedEventArgs e)
        {
            this.Index = 0;
            this.DialogResult = true;
        }

        private void ChildWindow_MouseRightButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            e.Handled = true;
        }
        /// <summary>
        /// 历史查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HistoryQueryButton_OnClick(object sender, RoutedEventArgs e)
        {
            this.Index = 1;
            this.DialogResult = true;
        }

        private void TrafficManageButton_OnClick(object sender, RoutedEventArgs e)
        {
            this.Index = 2;
            this.DialogResult = true;
        }

        private void BaseInfoButton_OnClick(object sender, RoutedEventArgs e)
        {
            this.Index = 3;
            this.DialogResult = true;
        }

        private void StatisticReportButton_OnClick(object sender, RoutedEventArgs e)
        {
            this.Index = 4;
            this.DialogResult = true;
        }

        private void VedioManageButton_OnClick(object sender, RoutedEventArgs e)
        {
            this.Index = 5;
            this.DialogResult = true;
        }

        private void PubicServiceButton_OnClick(object sender, RoutedEventArgs e)
        {
            this.Index = 6;
            this.DialogResult = true;
        }

        private void SystemManageButton_OnClick(object sender, RoutedEventArgs e)
        {
            this.Index = 7;
            this.DialogResult = true;
        }

        private void VedioWallButton_OnClick(object sender, RoutedEventArgs e)
        {
            this.Index = 8;
            this.DialogResult = true;
        }

        //点击任意地方取消。
        private void LayoutRoot_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            this.Index = -1;
            this.DialogResult = true;
        }
    }
}

