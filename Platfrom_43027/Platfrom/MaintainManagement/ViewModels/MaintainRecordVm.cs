/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: c86d5379-dde4-4dcc-a69a-bad1207db1da      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-DENGZL
/////                 Author: TEST(dengzl)
/////======================================================================
/////           Project Name: Gsafety.PTMS.SecuritySuite.ViewModels
/////    Project Description:    
/////             Class Name: MaintainRecordVm
/////          Class Version: v1.0.0.0
/////            Create Time: 8/9/2013 9:47:14 AM
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/10/10 9:47:14 AM
/////            Modified by:ShiHS
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
using Jounce.Core.ViewModel;
using Gsafety.PTMS.ServiceReference.MaitenanceRecordService;
using System.Collections.ObjectModel;
using System.Windows.Data;
using Jounce.Core.Command;
using Gsafety.PTMS.Share;
using Jounce.Framework.Command;
using System.Collections.Generic;
using Jounce.Core.View;
using Gsafety.Common.Localization.Resource;

namespace Gsafety.PTMS.Maintain.ViewModels
{
    [ExportAsViewModel(MaintainName.MaintainRecordVm)]
    public class MaintainRecordVm : BaseViewModel
    {
        private int PageIndex = 1;
        public string SuiteID { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int PageSizeValue { get; set; }
        public List<int> PageSizeList { get; set; }
        public SuiteMaintainRecord CurrentMaintainRecord { get; set; }
        private ObservableCollection<SuiteMaintainRecord> _maintainModels;
        public ObservableCollection<SuiteMaintainRecord> MaintainModels
        {
            get
            {
                return this._maintainModels;
            }
            set
            {
                this._maintainModels = value;
                RaisePropertyChanged(() => this.MaintainModels);
            }
        }
        private PagedCollectionView _maintainPageView;
        public PagedCollectionView MaintainPageView
        {
            get
            {
                return this._maintainPageView;
            }
            set
            {
                this._maintainPageView = value;
                RaisePropertyChanged(() => this.MaintainPageView);
            }
        }

        public IActionCommand QueryCommand { get; set; }
        public IActionCommand ListCommand { get; set; }
        public IActionCommand ReportCommand { get; set; }
        public IActionCommand SimpleCommand { get; set; }
        public IActionCommand ScapeCommand { get; set; }

        private MaintenanceRecordServiceClient client = ServiceClientFactory.Create<MaintenanceRecordServiceClient>();
        public MaintainRecordVm()
        {
            client.GetMaintenanceRecordsBySuiteIdCompleted += client_GetMaintenanceRecordsBySuiteIdCompleted;
            QueryCommand = new ActionCommand<object>(obj => QueryAction(1));
            ListCommand = new ActionCommand<object>(obj => ListAction());

            SimpleCommand = new ActionCommand<object>(obj => SimpleAction());
            ScapeCommand = new ActionCommand<object>(obj => ScrapAction());
            ReportCommand = new ActionCommand<object>(obj => ReportAction());

            StartDate = DateTime.Now.AddMonths(-1);
            EndDate = DateTime.Now;

            MaintainModels = new ObservableCollection<SuiteMaintainRecord>();
            PageSizeValue = 20;
            PageSizeList = new List<int> { 20, 40, 80 };
            InvokServer(1);
        }

        void MaintainPageView_PageChanged(object sender, EventArgs e)
        {
            PageIndex = ((PagedCollectionView)sender).PageIndex + 1;
            QueryAction(PageIndex);
        }

        //Invoke Server
        private void InvokServer(int pageIndex)
        {
            int pageSize = PageSizeValue;
            System.Threading.Interlocked.Exchange(ref PageIndex, pageIndex);
            string districtCode = string.Empty;
            PagingInfo pagingInfo = new PagingInfo() { PageIndex = pageIndex, PageSize = pageSize };
            client.GetMaintenanceRecordsBySuiteIdAsync(SuiteID, StartDate, EndDate, pagingInfo);
        }

        void client_GetMaintenanceRecordsBySuiteIdCompleted(object sender, GetMaintenanceRecordsBySuiteIdCompletedEventArgs e)
        {
            ApplicationContext.Instance.BusyInfo.IsBusy = false;
            MaintainModels = e.Result.Result;

            if (PageIndex == 1)
            {
                List<int> pageList = new List<int>(e.Result.TotalRecord);
                for (int i = 0; i < e.Result.TotalRecord; i++)
                {
                    pageList.Add(i);
                }
                MaintainPageView = new PagedCollectionView(pageList);
                MaintainPageView.PageSize = this.PageSizeValue;
                MaintainPageView.PageChanged += MaintainPageView_PageChanged;
            }
            if (e.Result.TotalRecord == 0)
            {
                MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("MAINTAIN_DataEmpty"));
            }
        }

        //查询访问
        private void QueryAction(int pageIndex)
        {
            if (StartDate.HasValue && EndDate.HasValue && StartDate.Value.CompareTo(EndDate) > 0)
            {
                string timeError = StringResource.ResourceManager.GetString("TimeError");
                string msg = string.IsNullOrEmpty(timeError) ? "StrartTime can not be greater than EndTime !" : timeError;
                string warning = StringResource.ResourceManager.GetString("Warning");
                MessageBox.Show(msg, string.IsNullOrEmpty(warning) ? "Warning !" : warning, MessageBoxButton.OK);
            }
            else
            {
                ApplicationContext.Instance.BusyInfo.IsBusy = true;
                InvokServer(pageIndex);
            }
        }

        //连接到MaintenanceListView 
        private void ListAction()
        {
            if (!string.IsNullOrEmpty(CurrentMaintainRecord.SuiteId))
            {
                EventAggregator.Publish(new ViewNavigationArgs("MaintenanceListView", new Dictionary<string, object>() { { "SuiteId", CurrentMaintainRecord.SuiteId } }));
            }
            else global::System.Windows.MessageBox.Show("SuiteID can not be empty !", "Error !", System.Windows.MessageBoxButton.OK);
        }

        //连接到简单维修和报废界面
        private void SimpleAction()
        {
            if (!string.IsNullOrEmpty(CurrentMaintainRecord.SuiteId))
            {
                EventAggregator.Publish(new ViewNavigationArgs("MaintenanceSimpleView", new Dictionary<string, object>() { { "SuiteId", CurrentMaintainRecord.SuiteId } }));
            }
            else global::System.Windows.MessageBox.Show("SuiteID can not be empty !", "Error !", System.Windows.MessageBoxButton.OK);
        }

          //连接到简单维修和报废界面
        private void ScrapAction()
        {
            if (!string.IsNullOrEmpty(CurrentMaintainRecord.SuiteId))
            {
                EventAggregator.Publish(new ViewNavigationArgs("MaintenanceScrapView", new Dictionary<string, object>() { { "SuiteId", CurrentMaintainRecord.SuiteId } }));
            }
            else global::System.Windows.MessageBox.Show("SuiteID can not be empty !", "Error !", System.Windows.MessageBoxButton.OK);
        }

        //连接到MaintainRecordReport
        private void ReportAction()
        {
            if (!string.IsNullOrEmpty(CurrentMaintainRecord.SuiteId))
            {
                EventAggregator.Publish(new ViewNavigationArgs("MaintainRecordReport", new Dictionary<string, object>() {
                { "ID", CurrentMaintainRecord.SuiteId }, 
                { "StartUseTime", CurrentMaintainRecord.StartUseTime }, 
                { "LastMaintanceTime", CurrentMaintainRecord.LastMaintainTime } }));
            }
            else global::System.Windows.MessageBox.Show("SuiteID can not be empty !", "Error !", System.Windows.MessageBoxButton.OK);
        }
    }
}
