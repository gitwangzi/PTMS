using Gsafety.PTMS.ServiceReference.AppConfigService;
/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 64fd2f2e-c7a8-4f2e-894d-b6f70ead1907      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-DENGZL
/////                 Author: TEST(dengzl)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Share.BufferManage
/////    Project Description:    
/////             Class Name: AlertConfigManage
/////          Class Version: v1.0.0.0
/////            Create Time: 11/12/2013 2:36:16 PM
/////      Class Description:  
/////======================================================================
/////          Modified Time: 11/12/2013 2:36:16 PM
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
using System.Collections.Generic;
using System.Collections.ObjectModel;
namespace Gsafety.PTMS.Share
{
    public class AlertConfigManage
    {
        private ObservableCollection<AppConfig> _ConfigList;

        public ObservableCollection<AppConfig> ConfigList
        {
            get { return _ConfigList; }
            set { _ConfigList = value; }
        }
        public void DataLoading()
        {
            GetAlertTypeColorInfo();
        }

        AppConfigManagerClient appClient = ServiceClientFactory.Create<AppConfigManagerClient>();
        private void GetAlertTypeColorInfo()
        {

            appClient.GetConfigInfoBytypeCompleted += appClient_GetConfigInfoBytypeCompleted;
            appClient.GetConfigInfoBytypeAsync("AlertType", "All");
            ApplicationContext.Instance.Logger.Log(Jounce.Core.Application.LogSeverity.Information, "AlertConfigManage", "begin get alerts");
            ApplicationContext.Instance.BusyInfo.InitLoadingNum++;
        }

        private void appClient_GetConfigInfoBytypeCompleted(object sender, GetConfigInfoBytypeCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                ApplicationContext.Instance.Logger.Log(Jounce.Core.Application.LogSeverity.Information, "AlertConfigManage", "get alerts error");
                appClient.CloseAsync();
                ApplicationContext.Instance.Logger.LogException(GetType().FullName, e.Error);
                ApplicationContext.Instance.BusyInfo.InitLoadingNum--;
                return;
            }
            try
            {
                ApplicationContext.Instance.Logger.Log(Jounce.Core.Application.LogSeverity.Information, "AlertConfigManage", "end get alerts");
                if (e.Result.Result != null)
                {
                    ConfigList = e.Result.Result;
                }
            }
            finally
            {
                ApplicationContext.Instance.Logger.Log(Jounce.Core.Application.LogSeverity.Information, "AlertConfigManage", "get alerts finish");
                ApplicationContext.Instance.BusyInfo.InitLoadingNum--;
            }
        }
    }
}
