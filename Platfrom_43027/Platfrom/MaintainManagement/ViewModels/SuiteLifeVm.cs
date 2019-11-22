using Gsafety.PTMS.ServiceReference.MaitenanceRecordService;
using Gsafety.PTMS.Share;
using Jounce.Core.Command;
using Jounce.Core.View;
using Jounce.Core.ViewModel;
using Jounce.Framework.Command;
/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: b4ab3655-c270-44ce-84e7-19daa1a6949e      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-PENGGL
/////                 Author: TEST(penggl)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Maintain.ViewModels
/////    Project Description:    
/////             Class Name: ServiceLifeVm
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/11/21 12:48:33
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/11/21 12:48:33
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

namespace Gsafety.PTMS.Maintain.ViewModels
{

    [ExportAsViewModel(MaintainName.SuiteLifeVm)]
    public class SuiteLifeVm : BaseViewModel
    {
        public string SuiteId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public SuiteLife CurrentItem { get; set; }

        public PagedServerCollection<SuiteLife> SuiteLifePageView { get; set; }    //分页数据

        public IActionCommand QueryCommand { get; set; }
        public IActionCommand QueryDetailCommand { get; set; }

        private MaintenanceRecordServiceClient client;

        private int currentIndex = 1;
        public List<int> PageSizeList { get; set; }

        private int pageSizeValue;
        public int PageSizeValue
        {
            get { return pageSizeValue; }
            set
            {
                pageSizeValue = value;
                if (this.SuiteLifePageView != null)
                {
                    this.SuiteLifePageView.PageSize = value;
                }
            }
        }

        public SuiteLifeVm()
        {
            client = ServiceClientFactory.Create<MaintenanceRecordServiceClient>();
            client.GetSuiteLifeExCompleted += client_GetSuiteLifeExCompleted;
            QueryCommand = new ActionCommand<object>(obj => QueryAction());
            QueryDetailCommand = new ActionCommand<object>(obj => QueryDetailAction());

            StartDate = DateTime.Now.Date;
            EndDate = DateTime.Now.Date;

            PageSizeList = MaintainCommon.PageSizeList;
            PageSizeValue = PageSizeList[0];

            InitPagedServerCollection();
        }

        private void InitPagedServerCollection()
        {
            SuiteLifePageView = new PagedServerCollection<SuiteLife>((pageIndex, pageSize) =>
            {
                pageSize = PageSizeValue;
                System.Threading.Interlocked.Exchange(ref currentIndex, pageIndex);

                client.GetSuiteLifeExAsync(SuiteId, StartDate, EndDate, new PagingInfo { PageIndex = pageIndex, PageSize = pageSize });
            });
        }

        void client_GetSuiteLifeExCompleted(object sender, GetSuiteLifeExCompletedEventArgs e)
        {
            try
            {
                SuiteLifePageView.loader_Finished(new PagedResult<SuiteLife>
                {
                    Count = e.Result.TotalRecord,
                    Items = e.Result.Result,
                    PageIndex = currentIndex,
                });
                if (e.Result.TotalRecord == 0)
                {
                    MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("MAINTAIN_DataEmpty"));
                }
            }
            catch
            {
                //MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("BASEINFO_Operate_Failed"));
            }
        }

        private void QueryAction()
        {
            currentIndex = 1;
            SuiteLifePageView.MoveToFirstPage();
        }

        private void QueryDetailAction()
        {
            EventAggregator.Publish(new ViewNavigationArgs(MaintainName.SuiteLifeDetailV,
               new Dictionary<string, object>() { { "item", CurrentItem } }));
        }
    }
}
