using Gsafety.Common.Controls;
using Gsafety.Common.Utilities;
using Gsafety.PTMS.Constants;
using Gsafety.PTMS.Share;
using Jounce.Core.View;
using Jounce.Regions.Core;
using System;
/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 75dd059c-159d-4c69-9088-e9352ac4c1f2      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-DENGZL
/////                 Author: TEST(dengzl)
/////======================================================================
/////           Project Name: Gsafety.PTMS.MainPage.Views
/////    Project Description:    
/////             Class Name: InstallPlatform
/////          Class Version: v1.0.0.0
/////            Create Time: 8/13/2013 10:54:04 AM
/////      Class Description:  
/////======================================================================
/////          Modified Time: 8/13/2013 10:54:04 AM
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System.Collections.Generic;
using System.Windows;
using System.Windows.Browser;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;
using System.Windows.Navigation;

namespace Gsafety.PTMS.MainPage.Views
{
    [ExportAsView(MainPageName.InstallPlatformV)]
    [ExportViewToRegion(MainPageName.InstallPlatformV, ViewContainer.LoginContainer)]
    public partial class InstallPlatform : UserControl
    {

        List<HyperlinkButton> _NavigationButton;

        public List<HyperlinkButton> NavigationButtons
        {
            get
            {
                if (_NavigationButton == null || _NavigationButton.Count == 0)
                {
                    VisualTreeExtedHelper vtHelper = new VisualTreeExtedHelper();
                    _NavigationButton = vtHelper.GetChildObjects<HyperlinkButton>(this.LayoutRoot, "");
                }
                return _NavigationButton;
            }
        }

        public InstallPlatform()
        {
            InitializeComponent();
            ApplicationContext.Instance.NavigateManager.AddFrame(InstallContentFrame);
            //string languageName = System.Threading.Thread.CurrentThread.CurrentCulture.Name;
            //if (string.IsNullOrEmpty(languageName))
            //{
            //    languageName = "es-ES";
            //}
            //switch (languageName)
            //{
            //    case "zh-CN":
            //        grid_es_ES.Visibility = System.Windows.Visibility.Collapsed;
            //        grid_zh_CN.Visibility = System.Windows.Visibility.Visible;
            //        break;

            //    case "es-ES":
            //        grid_es_ES.Visibility = System.Windows.Visibility.Visible;
            //        grid_zh_CN.Visibility = System.Windows.Visibility.Collapsed;
            //        break;

            //    default:
            //        break;
            //}
        }

        // After the Frame navigates, ensure the HyperlinkButton representing the current page is selected
        private void ContentFrame_Navigated(object sender, NavigationEventArgs e)
        {
            bool focusOne = false;
            foreach (var hb in NavigationButtons)
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

            if (!focusOne && NavigationButtons != null && NavigationButtons.Count > 0)
            {
                VisualStateManager.GoToState(NavigationButtons[0], "ActiveLink", true);
            }
        }

        // If an error occurs during navigation, show an error window
        private void ContentFrame_NavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            e.Handled = true;
        }

        private void AccordionControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            ((MenuItemWidth)((FrameworkElement)sender).Tag).PanelWidth = e.NewSize.Width;
        }

        //显示
        private void showL_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var collapseAnimation = (Storyboard)Resources["expandTransition"];
            collapseAnimation.Begin();
            ContentBorder.SetValue(Grid.ColumnProperty, 1);
            ContentBorder.Margin = new Thickness(0, 0, 0, 0);
            SuiteLeftContent.Visibility = Visibility.Visible;
            collapsedPane.Visibility = Visibility.Collapsed;
        }

        //隐藏
        private void hiddenL_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var collapseAnimation = (Storyboard)Resources["collapseTransition"];
            collapseAnimation.Completed += (m, n) =>
            {
                SuiteLeftContent.Visibility = Visibility.Collapsed;
                ContentBorder.SetValue(Grid.ColumnProperty, 0);
                ContentBorder.SetValue(Grid.ColumnSpanProperty, 2);
                collapsedPane.Visibility = Visibility.Visible;
                collapsedPane.Opacity = 0.8;
            };
            collapseAnimation.Begin();
        }
        string HelpDir = "";
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
