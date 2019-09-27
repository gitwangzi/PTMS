using Gsafety.PTMS.ServiceReference.UpdateService;
using Gsafety.PTMS.Share;
using Jounce.Core.Command;
using Jounce.Core.View;
using Jounce.Core.ViewModel;
using Jounce.Framework.Command;
/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: af764320-636a-4310-9e7f-3f967115829f      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-PENGGL
/////                 Author: TEST(penggl)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Maintain.ViewModels
/////    Project Description:    
/////             Class Name: UpgradeStatusVm
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/10/28 15:16:10
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/10/28 15:16:10
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace Gsafety.PTMS.Maintain.ViewModels
{
    [ExportAsViewModel(MaintainName.UpgradeStatusVm)]
    public class UpgradeStatusVm : BaseViewModel
    {
        private UpdateServiceClient updateClient = ServiceClientFactory.Create<UpdateServiceClient>();
        public PagedServerCollection<VersionUpgradeStatus> PSC_VersionUpgradeStatus { get; set; }
        private int pageSizeValue;
        public int PageSizeValue
        {
            get { return pageSizeValue; }
            set
            {
                pageSizeValue = value;
                if (this.PSC_VersionUpgradeStatus != null)
                {
                    this.PSC_VersionUpgradeStatus.PageSize = pageSizeValue;
                }
            }
        }
        public List<int> PageSizeList { get; set; }
        public string VersionId { get; set; }
        public IActionCommand QueryCommand { get; private set; }

        protected override void ActivateView(string viewName, System.Collections.Generic.IDictionary<string, object> viewParameters)
        {
            base.ActivateView(viewName, viewParameters);
            if (viewParameters.Count != 0 && viewParameters["action"].ToString() == "return")
            {
                PSC_VersionUpgradeStatus.ToPage(currentIndex);
            }
            else
            {

            }
        }

        public UpgradeStatusVm()
        {
            QueryCommand = new ActionCommand<object>(obj => Query());
            PageSizeList = MaintainCommon.PageSizeList; //获取翻页控件下拉列表
            PageSizeValue = PageSizeList[0];//获取默认每页显示记录的数据
            InitPagedServerCollection();
        }

        private int currentIndex = 1;
        private void InitPagedServerCollection()
        {
            updateClient.GetUpgradeStatusFuzzyCompleted += updateClient_GetUpgradeStatusFuzzyCompleted;
            PSC_VersionUpgradeStatus = new PagedServerCollection<VersionUpgradeStatus>((pageIndex, pageSize) =>
            {
                pageSize = PageSizeValue;
                System.Threading.Interlocked.Exchange(ref currentIndex, pageIndex);
                Gsafety.PTMS.ServiceReference.UpdateService.PagingInfo pagingInfo = new Gsafety.PTMS.ServiceReference.UpdateService.PagingInfo() { PageIndex = pageIndex, PageSize = pageSize };
                updateClient.GetUpgradeStatusFuzzyAsync(VersionId, pagingInfo);
            });
        }

        void updateClient_GetUpgradeStatusFuzzyCompleted(object sender, GetUpgradeStatusFuzzyCompletedEventArgs e)
        {
            try
            {
                PSC_VersionUpgradeStatus.loader_Finished(new PagedResult<VersionUpgradeStatus>
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
            PSC_VersionUpgradeStatus.MoveToFirstPage();
        }
    }
}
