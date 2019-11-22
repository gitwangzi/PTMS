using Jounce.Core.View;
using Jounce.Core.ViewModel;
using Jounce.Framework.Command;
using System.Collections.Generic;
using System.Windows.Input;
using Gsafety.PTMS.Share;
using Jounce.Core.Command;
using Gsafety.PTMS.ServiceReference.MaitenanceRecordService;
using System;

/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////Guid: 
///// clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
///// Machine Name: PC-SHIHS
///// Author: (Shihs)
/////======================================================================
///// Project Name: Gsafety.PTMS.Manager.ViewModels.LogManageViewModel
///// Project Description:    
/////Class Name: CarAlertLogViewModel
///// Class Version: v1.0.0.0
///// Create Time: 2013/9/22 00:00:00
/////Class Description:  
/////======================================================================
/////Modified Time: 
/////Modified by:dengzl
/////Modified Description: 
/////======================================================================

namespace Gsafety.PTMS.Maintain.ViewModels
{
    [ExportAsViewModel(MaintainName.MaintainRecordReportVM)]
    public class MaintainRecordReportVM : BaseViewModel
    {
        private int currentIndex = 1;
        MaintenanceRecordServiceClient client = ServiceClientFactory.Create<MaintenanceRecordServiceClient>();

        public MaintainsReports CurrentDataInfo { get; set; }
        public PagedServerCollection<MaintainsReports> DataInfoPage { get; set; }
        public string SuiteID { get; set; }
        public string StartUseTime { get; set; }
        public string LastMaintanceTime { get; set; }
        public ICommand GoBackCommand { get; set; }
        public List<int> PageSizeList { get; set; }
        private int pageSizeValue;
        public int PageSizeValue
        {
            get { return pageSizeValue; }
            set
            {
                pageSizeValue = value;
                if (!string.IsNullOrEmpty(SuiteID))
                {
                    DataInfoPage.MoveToFirstPage();
                }
            }
        }

        //Invoke Server
        private void InvokServer(int pageIndex, int pageSize)
        {
            pageSize = PageSizeValue;
            System.Threading.Interlocked.Exchange(ref currentIndex, pageIndex);
            string districtCode = string.Empty;
            Gsafety.PTMS.ServiceReference.MaitenanceRecordService.PagingInfo pagingInfo = new Gsafety.PTMS.ServiceReference.MaitenanceRecordService.PagingInfo() { PageIndex = pageIndex, PageSize = pageSize };
            client.GetMaintainsReportsAsync(SuiteID, pagingInfo);
        }

        public MaintainRecordReportVM()
        {
            GoBackCommand = new ActionCommand<object>(obj => GoBackAction());
            client.GetMaintainsReportsCompleted += client_GetMaintainsReportsCompleted;
            UIInit();
            //InitPagedServerCollection();
            //Query();
        }

        private void InitPagedServerCollection()
        {
            if (!string.IsNullOrEmpty(SuiteID))
                DataInfoPage = new PagedServerCollection<MaintainsReports>(new Action<int, int>(InvokServer));
        }

        private void UIInit()
        {
            PageSizeList = new List<int> { 20, 40, 80 };
            PageSizeValue = 20;
        }

        void client_GetMaintainsReportsCompleted(object sender, GetMaintainsReportsCompletedEventArgs e)
        {
            DataInfoPage.loader_Finished(new PagedResult<MaintainsReports>
            {
                Count = e.Result.TotalRecord,
                Items = e.Result.Result as IEnumerable<MaintainsReports>,
                PageIndex = currentIndex,
            });
            var p = e.Result.Result;
            //StartUseTime = e != null ?(e.Result.Result.Count>0?e.Result.Result[0].StartUseTime.Value.ToString():string.Empty) : string.Empty;
            Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(new string[] { "DataInfoPage", "PageSizeValue", "StartUseTime" }));
        }

        protected override void ActivateView(string viewName, System.Collections.Generic.IDictionary<string, object> viewParameters)
        {
            base.ActivateView(viewName, viewParameters);
            if (viewParameters.Count != 0)
            {
                SuiteID = viewParameters["ID"].ToString();
                LastMaintanceTime = viewParameters["LastMaintanceTime"].ToString();
                StartUseTime = viewParameters["StartUseTime"].ToString();
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(new string[] { "LastMaintanceTime", "SuiteID", "StartUseTime" }));
                DataInfoPage = new PagedServerCollection<MaintainsReports>(new Action<int, int>(InvokServer));
                DataInfoPage.ToPage(1);
            }
        }

        private void GoBackAction()
        {
            EventAggregator.Publish(new ViewNavigationArgs("MaintainRecord", new Dictionary<string, object>() { { "action", "return" } }));
        }
    }
}
