using Gsafety.PTMS.Constants;
using Gsafety.PTMS.MainPage;
using Gsafety.PTMS.Share;
using Jounce.Core.View;
using Jounce.Regions.Core;
using System;
using System.Windows;
using System.Windows.Browser;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;

namespace Gsafety.Ant.MainPage.Views
{
    [ExportAsView(MainPageName.OrderClientPlatformV)]
    [ExportViewToRegion(MainPageName.OrderClientPlatformV, ViewContainer.LoginContainer)]
    public partial class OrderClientPlatform : UserControl
    {
        public OrderClientPlatform()
        {
            InitializeComponent();
            this.InitilizeLogoButton();
        }

        private void InitilizeLogoButton()
        {
            string currentLangeuage = ApplicationContext.Instance.SystemCutureInfo.Name;
            switch (currentLangeuage)
            {
                //中文
                case "zh-CN":
                    this.logoZh.Visibility = Visibility.Visible;
                    this.logoEn.Visibility = Visibility.Collapsed;
                    this.logoEs.Visibility = Visibility.Collapsed;
                    break;
                //英文
                case "en-US":
                    this.logoZh.Visibility = Visibility.Collapsed;
                    this.logoEn.Visibility = Visibility.Visible;
                    this.logoEs.Visibility = Visibility.Collapsed;
                    break;

                //英文
                case "es-ES":
                    this.logoZh.Visibility = Visibility.Collapsed;
                    this.logoEn.Visibility = Visibility.Collapsed;
                    this.logoEs.Visibility = Visibility.Visible;
                    break;

                default:
                    this.logoZh.Visibility = Visibility.Visible;
                    this.logoEn.Visibility = Visibility.Collapsed;
                    this.logoEs.Visibility = Visibility.Collapsed;
                    break;
            }
        }


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

        private void showL_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var collapseAnimation = (Storyboard)Resources["expandTransition"];
            collapseAnimation.Begin();
            ContentBorder.SetValue(Grid.ColumnProperty, 1);
            ContentBorder.Margin = new Thickness(0, 0, 0, 0);
            SythesesLeftContent.Visibility = Visibility.Visible;
            collapsedPane.Visibility = Visibility.Collapsed;
        }

        string HelpDir = "/OrderClient/index.html";
        private void btnHelp_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string strUri = string.Empty;

                strUri = this.InilitHelpUri(ApplicationContext.Instance.ServerConfig.Culture, this.HelpDir);
                var window = HtmlPage.Window;
                window.SetProperty("title", "Help");
                window.Navigate(new Uri(Application.Current.Host.Source, strUri), "_blank");
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("btnHelp_Click", ex);
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
            if (language == "zh-CN")
            {
                strUri = ApplicationContext.Instance.ServerConfig.ChineseHelpUrl + dir;
            }
            else if (language == "en-US")
            {
                strUri = ApplicationContext.Instance.ServerConfig.EnglishHelpUrl + dir;
            }
            else if (language == "es-ES")
            {
                strUri = ApplicationContext.Instance.ServerConfig.SpanishHelpUrl + dir;
            }

            return strUri;

        }
    }
}
