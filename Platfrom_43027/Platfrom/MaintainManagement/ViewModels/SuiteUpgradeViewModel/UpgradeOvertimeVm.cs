using Gsafety.PTMS.ServiceReference.UpdateService;
using Gsafety.PTMS.Share;
using Jounce.Core.Command;
using Jounce.Core.ViewModel;
using Jounce.Framework.Command;
/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: cb51d10e-4ab7-49d4-b835-3df85d83e7d4      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-PENGGL
/////                 Author: TEST(penggl)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Maintain.ViewModels.SuiteUpgradeViewModel
/////    Project Description:    
/////             Class Name: UpgradeOvertimeVm
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/11/20 10:19:22
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/11/20 10:19:22
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

namespace Gsafety.PTMS.Maintain.ViewModels.SuiteUpgradeViewModel
{
    [ExportAsViewModel(MaintainName.UpgradeOvertimeVm)]
    public class UpgradeOvertimeVm : BaseViewModel
    {

        private UpdateServiceClient updateClient = ServiceClientFactory.Create<UpdateServiceClient>();
        public PagedServerCollection<UpgradeOvertime> PSC_UpgradeOvertime { get; set; }

        private int pageSizeValue;
        public int PageSizeValue
        {
            get { return pageSizeValue; }
            set
            {
                pageSizeValue = value;
                if (this.PSC_UpgradeOvertime != null)
                {
                    this.PSC_UpgradeOvertime.PageSize = pageSizeValue;
                }
            }
        }

        public List<int> TimeList
        {
            get
            {
                List<int> timeList = new List<int>();
                timeList.Add(24);
                timeList.Add(48);
                timeList.Add(72);
                return timeList;
            }
        }

        public int CurrentTime { get; set; }
        public List<int> PageSizeList { get; set; }
        public IActionCommand QueryCommand { get; private set; }

        protected override void ActivateView(string viewName, System.Collections.Generic.IDictionary<string, object> viewParameters)
        {
            base.ActivateView(viewName, viewParameters);
            if (viewParameters.Count != 0 && viewParameters["action"].ToString() == "return")
            {
                PSC_UpgradeOvertime.ToPage(currentIndex);
            }
        }

        public UpgradeOvertimeVm()
        {
            QueryCommand = new ActionCommand<object>(obj => Query());
            PageSizeList = MaintainCommon.PageSizeList; //获取翻页控件下拉列表
            PageSizeValue = PageSizeList[0];//获取默认每页显示记录的数据
            CurrentTime = TimeList[0];
            InitPagedServerCollection();
        }

        private int currentIndex = 1;
        private void InitPagedServerCollection()
        {
            updateClient.GetUpgradeOvertimeFuzzyCompleted += updateClient_GetUpgradeOvertimeFuzzyCompleted;
            PSC_UpgradeOvertime = new PagedServerCollection<UpgradeOvertime>((pageIndex, pageSize) =>
            {
                pageSize = PageSizeValue;
                System.Threading.Interlocked.Exchange(ref currentIndex, pageIndex);
                PagingInfo pagingInfo = new PagingInfo() { PageIndex = pageIndex, PageSize = pageSize };
                updateClient.GetUpgradeOvertimeFuzzyAsync(CurrentTime, pagingInfo);
            });
        }

        void updateClient_GetUpgradeOvertimeFuzzyCompleted(object sender, GetUpgradeOvertimeFuzzyCompletedEventArgs e)
        {
            try
            {
                PSC_UpgradeOvertime.loader_Finished(new PagedResult<UpgradeOvertime>
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

        private void Query()
        {
            currentIndex = 1;
            PSC_UpgradeOvertime.MoveToFirstPage();
        }

    }
}
