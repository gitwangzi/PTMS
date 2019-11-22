/////Copyright (C)2014Microsoft All Rights Reserved.
/////======================================================================
/////                   Guid: 41268bb5-c9cd-42d2-8d78-405f31e99606      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-JIANGJ
/////                 Author: TEST(JiangJ)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Share.BufferManage
/////    Project Description:    
/////             Class Name: MonitorGroupManager
/////          Class Version: v1.0.0.0
/////            Create Time: 2014/11/17 15:04:14
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2014/11/17 15:04:14
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

using System.Collections.ObjectModel;
using System.Linq;
using System.Collections.Generic;
using Jounce.Core.Model;
using Jounce.Core.Event;
using Gsafety.PTMS.Bases.Models;
using Gsafety.PTMS.Bases.Librarys;
using Gsafety.PTMS.Share;
using System.ComponentModel.Composition;
using Gsafety.PTMS.ServiceReference.RunMonitorGroupService;

namespace Gsafety.PTMS.Share
{
    public class MonitorGroupManager : BaseNotify
    {
        private ObservableCollection<RunMonitorGroup> _monitorGroups = new ObservableCollection<RunMonitorGroup>();

        public ObservableCollection<RunMonitorGroup> MoniterGroupManagerOC
        {
            get { return _monitorGroups; }
            set { value = _monitorGroups; }
        }

        public void DataLoading()
        {
            GetMonitorGroupInfo();
        }

        private void GetMonitorGroupInfo()
        {
            try
            {
                RunMonitorGroupServiceClient client = ServiceClientFactory.Create<RunMonitorGroupServiceClient>();
                client.GetRunMonitorGroupListCompleted += client_GetRunMonitorGroupListCompleted;
                client.GetRunMonitorGroupListAsync(0, int.MaxValue, ApplicationContext.Instance.AuthenticationInfo.UserID);
            }
            catch
            {

            }
        }


        void client_GetRunMonitorGroupListCompleted(object sender, GetRunMonitorGroupListCompletedEventArgs e)
        {
            try
            {
                if (e.Result.Result.Count() > 0)
                {
                    _monitorGroups = e.Result.Result;
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("GetRunMonitorGroupListCompleted", ex);
            }
            finally
            {
                RunMonitorGroupServiceClient client = sender as RunMonitorGroupServiceClient;
                client.CloseAsync();
                client = null;
            }
        }

        private static MonitorGroupManager _Instance = null;

        public static MonitorGroupManager Instance
        {
            get
            {
                if (_Instance == null)
                {
                    _Instance = new MonitorGroupManager();
                }
                return _Instance;
            }
        }
    }
}
