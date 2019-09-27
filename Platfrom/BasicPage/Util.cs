/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: ba563614-132b-49b3-871a-f8379bd49072      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: WANGJINCAI
/////                 Author: TEST(wangjc)
/////======================================================================
/////           Project Name: Gsafety.PTMS.BasicPage
/////    Project Description:    
/////             Class Name: Util
/////          Class Version: v1.0.0.0
/////            Create Time: 2013-09-09 09:21:57
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013-09-09 09:21:57
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Linq;
using Gsafety.PTMS.Share;
using System.Collections.Generic;
using System.Windows.Browser;
using Jounce.Core.Event;
using System.ComponentModel.Composition;
using Jounce.Core.View;
using Gsafety.PTMS.Constants;
using Gsafety.PTMS.Video.Args;
using Gsafety.Common.Controls;


namespace Gsafety.PTMS.BasicPage.VideoDisplay
{
    public class Util
    {
        private const string _pageUrl = "/ocxrelated/ocxplay/OcxContiner.aspx";
        private static string _baseHostUrl;
        private static Dictionary<string, HtmlWindow> _urlMap;

        public static IEventAggregator _EventAggregator { get; set; }

        static Util()
        {
            AnlisyUrl();
        }
        private static void AnlisyUrl()
        {
            var partUrlStrs = Application.Current.Host.Source.AbsoluteUri.Split('/');
            _baseHostUrl = string.Join("/", partUrlStrs.Take(partUrlStrs.Length - 2));
            _baseHostUrl += _pageUrl;
            _urlMap = new Dictionary<string, HtmlWindow>();

        }

        /// <summary>
        /// get OCXUrl
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        internal static string GetVideoUrl<T>(T args) where T : ArgsBase
        {
            var qs = System.Windows.Browser.HttpUtility.UrlEncode(QuetyHtmlHelp.GetQueryString(args));
            var vt = System.Windows.Browser.HttpUtility.UrlEncode(QuetyHtmlHelp.GetVideoType(args.GetType()));
            var url = string.Format("{0}?{1}={2}&{3}={4}", _baseHostUrl, QuetyHtmlHelp.QueryName, qs, QuetyHtmlHelp.VideoTypePara, vt);

            return url;
        }

        public static void OpenVideoPage<T>(T args, int width = 800, int height = 600, params bool[] visible) where T : ArgsBase
        {
            args.Height = height < 400 ? 400 : height;
            args.Width = width < 500 ? 500 : width;
            args.UserName = ApplicationContext.Instance.AuthenticationInfo.UserName;
            var url = GetVideoUrl(args);
            url += "&pop=1";

            if (!_urlMap.ContainsKey(url))
            {
                string realUrl = url;
                string temFileList = "";
                if (args is FileListVideoArgs)
                {
                    temFileList = (args as FileListVideoArgs).FileList;
                    realUrl = FileListArgOper(args as FileListVideoArgs);
                }

                var childWindow = (HtmlWindow)System.Windows.Browser.HtmlPage.Window.Invoke("openPopWindow", realUrl, width, height);

                if (childWindow != null)
                {

                    #region onload

                    childWindow.AttachEvent("onload", (object s, EventArgs e) =>
                    {
                        var doc = (HtmlDocument)childWindow.GetProperty("document");
                        if (visible != null && visible.Length > 0)
                        {
                            if (doc.GetElementById("div_menu") != null && visible[0] == false)
                            {
                                doc.GetElementById("div_menu").SetStyleAttribute("display", "none");
                                doc.GetElementById("img_Tip").SetStyleAttribute("display", "none");
                                return;
                            }

                        }

                        doc.GetElementById("div_menu").AttachEvent("click", (object s1, EventArgs e1) =>
                        {
                            try
                            {
                                if (args.Ifdivmenu == false)
                                {
                                    if (_EventAggregator != null)
                                    {
                                        if (!string.IsNullOrWhiteSpace(temFileList))
                                        {
                                            System.Windows.Browser.HtmlPage.Document.GetElementById("txt_ocx").SetAttribute("value", temFileList);
                                        }
                                        ApplicationContext.Instance.NavigateManager.Navigate(NavigationView.VedioWallView, Gsafety.PTMS.Constants.NavigationFrame.CentralPlatMainContentFrame);
                                        var paramters = new System.Collections.Generic.Dictionary<string, object>();
                                        paramters.Add(NavigationView.VedioWallView, args);
                                        _EventAggregator.Publish(new ViewNavigationArgs(NavigationView.VedioWallView, paramters));
                                    }
                                    childWindow.Invoke("close");
                                }
                            }
                            catch (Exception exp)
                            {
                                MessageBoxHelper.ShowDialog(exp.Message);
                            }
                        });
                    });
                    #endregion

                    #region onunload

                    childWindow.AttachEvent("onunload", (object s2, EventArgs e2) =>
                    {
                        try
                        {
                            _urlMap.Remove(url);
                        }
                        catch
                        {
                        }
                    });
                    #endregion

                    _urlMap.Add(url, childWindow);
                }
            }
            else
            {
                try
                {
                    _urlMap[url].Invoke("focus");
                }
                catch { }
            }
        }

        private static string FileListArgOper(FileListVideoArgs args)
        {
            System.Windows.Browser.HtmlPage.Document.GetElementById("txt_ocx").SetAttribute("value", args.FileList);
            //args.FileList = "";
            var url = GetVideoUrl(args);
            url += "&pop=1";
            return url;
        }

    }
}