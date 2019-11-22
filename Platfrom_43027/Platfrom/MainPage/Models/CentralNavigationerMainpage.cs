/////Copyright (C) Gsafety 2014 .All Rights Reserved.
/////======================================================================
/////                   Guid: e529cef0-cfb8-4865-beb6-0836fdd24a8c      
/////             clrversion: 4.0.30319.34011
/////Registered organization: 
/////           Machine Name: DENGZL
/////                 Author: DENGZL(dzl)
/////======================================================================
/////           Project Name: Gsafety.PTMS.MainPage.Models
/////    Project Description:    
/////             Class Name: CentralNavigationerMainpage
/////          Class Version: v1.0.0.0
/////            Create Time: 2014/3/15 04:25:46
/////      Class Description: 这个类是确定中心平台那个是登录后第一个显示的页面，由于角色的功能保存在一个字符串中，所有现在
/////                         借助这个类，二期需要改掉。 
/////======================================================================
/////          Modified Time: 2014/3/15 04:25:46
/////            Modified by:
/////   Modified Description: 
/////======================================================================

using System;
using System.Collections.Generic;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Gsafety.PTMS.Share;

namespace Gsafety.PTMS.MainPage
{
    public class CentralNavigationerMainpage
    {
         Dictionary<string, Uri> MainPageUri = new Dictionary<string, Uri> { 
              { "MAINPAGE_Monitor", new Uri("/MonitorMainPage", UriKind.RelativeOrAbsolute) },
              { "MAINPAGE_Alarm", new Uri("/AlarmMainView", UriKind.RelativeOrAbsolute) },
              { "MAINPAGE_Alert", new Uri("/VehicleAlertView", UriKind.RelativeOrAbsolute) },
              { "MAINPAGE_Traffic", new Uri("/TrafficMainPage", UriKind.RelativeOrAbsolute) },
              { "MAINPAGE_SecuritySuite", new Uri("/SuiteMainPage", UriKind.RelativeOrAbsolute) },
              { "MAINPAGE_BaseInfor", new Uri("/BaseInfoMainPage", UriKind.RelativeOrAbsolute) },
              { "MAINPAGE_Report", new Uri("/ReportMainPage", UriKind.RelativeOrAbsolute) },
              { "MAINPAGE_VideoWall", new Uri("/VedioWallMainPage", UriKind.RelativeOrAbsolute) },
              { "MAINPAGE_VideoDownload", new Uri("/VideoDownLoad_View", UriKind.RelativeOrAbsolute) },
              { "MAINPAGE_SystemManage", new Uri("/ManagerMainPage", UriKind.RelativeOrAbsolute) }
        };


        public Uri GetMainpageUri()
        {
            foreach (var item in MainPageUri)
            {
                if (ApplicationContext.Instance.AuthenticationInfo.FunctionNames.Contains(item.Key))
                    return item.Value;
            }
            return MainPageUri.GetEnumerator().Current.Value;
        }

        
    }
}
