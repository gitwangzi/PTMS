using Gsafety.PTMS.Constants;
using Gsafety.PTMS.MainPage;
using Gsafety.PTMS.Share;
using Gsafety.PTMS.VideoManagement.ViewModels;
using Jounce.Core.Event;
using Jounce.Core.View;
using Jounce.Regions.Core;
using System;
using System.ComponentModel.Composition;
using System.Windows;
using System.Windows.Browser;
using System.Windows.Controls;
namespace Gsafety.Ant.MainPage.Views
{
    [ExportAsView(MainPageName.CentralPlatformV2)]
    [ExportViewToRegion(MainPageName.CentralPlatformV2, ViewContainer.LoginContainer)]
    public partial class CentralPlatform2 : UserControl
    {
        /// <summary>
        ///     Event aggregator
        /// </summary>
        [Import]
        public IEventAggregator EventAggregator { get; set; }
        private MediaPlayerContainerFactory MediaPlayerContainerFactory;

        private AntProductNativeMenuView systemNativeMenu = null;

        /// <summary>
        /// 帮助按钮需要导航到的路径
        /// </summary>
        private string helpNative = "";
        private string currentLangeuage = "";
        private bool IsDefault = true;

        private string HelpDir = "";
        public CentralPlatform2()
        {
            InitializeComponent();
            systemNativeMenu = new AntProductNativeMenuView();
            systemNativeMenu.Closed += systemNativeMenu_Closed;

            MediaPlayerContainerFactory = new MediaPlayerContainerFactory();
            MediaPlayerContainerFactory.Root = this.rootGrid;
            this.currentLangeuage = ApplicationContext.Instance.SystemCutureInfo.Name;
            this.IsDefault = true;
            this.InitilizeLogoButton();
            this.Loaded += CentralPlatform2_Loaded;

        }

        void CentralPlatform2_Loaded(object sender, RoutedEventArgs e)
        {
            //菜单栏
           // this.systemNativeMenu.Show();

            int index = 0;
            this.NativeView(index);
        }

        private void InitilizeLogoButton()
        {
            switch (this.currentLangeuage)
            {
                //中文
                case "zh-CN":
                    this.LogButton.Visibility = Visibility.Visible;
                    this.LogoEnButton.Visibility = Visibility.Collapsed;
                    this.LogoESButton.Visibility = Visibility.Collapsed;
                    this.LogoByButton.Visibility = Visibility.Collapsed;
                    break;
                //英文
                case "en-US":
                    this.LogButton.Visibility = Visibility.Collapsed;
                    this.LogoEnButton.Visibility = Visibility.Visible;
                    this.LogoESButton.Visibility = Visibility.Collapsed;
                    this.LogoByButton.Visibility = Visibility.Collapsed;
                    break;

                //英文
                case "es-ES":
                    this.LogButton.Visibility = Visibility.Collapsed;
                    this.LogoEnButton.Visibility = Visibility.Collapsed;
                    this.LogoESButton.Visibility = Visibility.Visible;
                    this.LogoByButton.Visibility = Visibility.Collapsed;
                    break;

                case "pt-BR":
                    this.LogButton.Visibility = Visibility.Collapsed;
                    this.LogoEnButton.Visibility = Visibility.Collapsed;
                    this.LogoESButton.Visibility = Visibility.Collapsed;
                    this.LogoByButton.Visibility = Visibility.Visible;
                    break;

                default:
                    this.LogButton.Visibility = Visibility.Visible;
                    this.LogoEnButton.Visibility = Visibility.Collapsed;
                    this.LogoESButton.Visibility = Visibility.Collapsed;
                    break;
            }
        }

        void systemNativeMenu_Closed(object sender, System.EventArgs e)
        {
            int index = systemNativeMenu.Index;
            this.NativeView(index);
        }

        private void LogButton_OnClick(object sender, RoutedEventArgs e)
        {
            systemNativeMenu.Show();
        }

        //菜单栏
        private void MoniorButton_OnClick(object sender, RoutedEventArgs e)
        {
            int index = 0;
            this.NativeView(index);
        }

        private void HistoryQueryButton_OnClick(object sender, RoutedEventArgs e)
        {

            int index = 1;
            this.NativeView(index);

        }

        private void TrafficManageButton_OnClick(object sender, RoutedEventArgs e)
        {
            int index = 2;
            this.NativeView(index);

        }

        private void BaseInfoButton_OnClick(object sender, RoutedEventArgs e)
        {
            int index = 3;
            this.NativeView(index);

        }

        private void StatisticReportButton_OnClick(object sender, RoutedEventArgs e)
        {
            int index = 4;
            this.NativeView(index);

        }

        private void VedioManageButton_OnClick(object sender, RoutedEventArgs e)
        {
            int index = 5;
            this.NativeView(index);

        }

        private void PubicServiceButton_OnClick(object sender, RoutedEventArgs e)
        {
            int index = 6;
            this.NativeView(index);

        }

        private void SystemManageButton_OnClick(object sender, RoutedEventArgs e)
        {
            int index = 7;
            this.NativeView(index);

        }

        private void VedioWallButton_OnClick(object sender, RoutedEventArgs e)
        {
            int index = 8;
            this.NativeView(index);

        }
        private void NativeView(int index)
        {
            switch (index)
            {
                //实时监控
                case 0:
                    this.SuperFrame.Navigate(new Uri("/AntProductMonitorMainPageV", UriKind.Relative));
                    this.IsDefault = false;
                    this.HelpDir = @"/Monitor/index.html?module=020000";
                    break;
                //历史查询
                case 1:
                    this.SuperFrame.Navigate(new Uri("/HistoryQueryMainV", UriKind.RelativeOrAbsolute));
                    this.IsDefault = false;
                    this.HelpDir = @"/Monitor/index.html?module=030000";
                    break;
                //交通管理
                case 2:
                    this.SuperFrame.Navigate(new Uri("/TrafficMainPage", UriKind.Relative));
                    this.IsDefault = false;
                    this.HelpDir = @"/Monitor/index.html?module=040000";
                    break;
                //基础信息
                case 3:
                    this.SuperFrame.Navigate(new Uri("/AntProductBaseInfoMainPageV", UriKind.RelativeOrAbsolute));
                    this.IsDefault = false;
                    this.HelpDir = @"/Monitor/index.html?module=050000";
                    break;
                //报表管理
                case 4:
                    this.SuperFrame.Navigate(new Uri("/ReportMainPage", UriKind.RelativeOrAbsolute));
                    this.IsDefault = false;
                    this.HelpDir = @"/Monitor/index.html?module=060000";
                    break;

                //视频管理
                case 5:
                    this.SuperFrame.Navigate(new Uri("/VideoDownLoadMain_View", UriKind.RelativeOrAbsolute));
                    this.IsDefault = false;
                    this.HelpDir = @"/Monitor/index.html?module=070000";
                    break;

                //公共服务
                case 6:
                    this.SuperFrame.Navigate(new Uri("/PublicServiceMainPageView", UriKind.RelativeOrAbsolute));
                    this.IsDefault = false;
                    this.HelpDir = @"/Monitor/index.html?module=080000";
                    break;

                //系统管理
                case 7:
                    this.SuperFrame.Navigate(new Uri("/AntProductManagerMainPageV", UriKind.RelativeOrAbsolute));
                    this.IsDefault = false;
                    this.HelpDir = @"/Monitor/index.html?module=090000";
                    break;

                //视频墙
                case 8:
                    this.SuperFrame.Navigate(new Uri("/VedioWallMainPage", UriKind.RelativeOrAbsolute));
                    this.IsDefault = false;
                    this.HelpDir = @"/Monitor/index.html?module=100000";
                    break;

            }
        }

        /// <summary>
        /// 初始化帮助文件路径
        /// </summary>
        /// <param name="language">语言</param>
        /// <param name="isDefault">是否是默认</param>
        /// <param name="dir">文本路径</param>
        /// <returns></returns>
        private string InilitHelpUri(string language, string dir)
        {
            string strUri = string.Empty;
            if (currentLangeuage == "zh-CN")
            {
                strUri = ApplicationContext.Instance.ServerConfig.ChineseHelpUrl + dir;
            }
            else if (currentLangeuage == "en-US")
            {
                strUri = ApplicationContext.Instance.ServerConfig.EnglishHelpUrl + dir;
            }
            else if (currentLangeuage == "es-ES")
            {
                strUri = ApplicationContext.Instance.ServerConfig.SpanishHelpUrl + dir;
            }

            return strUri;

        }

        private void btnHelp_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                string strUri = string.Empty;
                if (this.IsDefault)
                {
                    this.HelpDir = @"/default.html";
                    strUri = this.InilitHelpUri(this.currentLangeuage, this.HelpDir);
                }
                else
                {
                    strUri = this.InilitHelpUri(this.currentLangeuage, "");
                }
                var window = HtmlPage.Window;
                window.SetProperty("title", "Help");
                window.Navigate(new Uri(Application.Current.Host.Source, strUri), "_blank");
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("btnHelp_Click", ex);
            }
        }
    }
}
