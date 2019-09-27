using Gsafety.PTMS.ServiceReference.DeviceInstallService;
using Gsafety.PTMS.Share;
using Jounce.Core.Command;
using Jounce.Core.ViewModel;
using Jounce.Framework.Command;
/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: d16fc335-5ac9-463c-92b5-65dcbf6b49df      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-PENGGL
/////                 Author: TEST(penggl)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Maintain.ViewModels.SecuritySuiteViewModel
/////    Project Description:    
/////             Class Name: SuiteInstallingVm
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/11/6 11:23:58
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/11/6 11:23:58
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

namespace Gsafety.PTMS.Maintain.ViewModels.SecuritySuiteViewModel
{
    [ExportAsViewModel(MaintainName.SuiteInstallingVm)]
    public class SuiteInstallingVm : BaseViewModel
    {
        private DeviceInstallServiceClient deviceInstallClient = ServiceClientFactory.Create<DeviceInstallServiceClient>();
        public PagedServerCollection<InstallationInfo> PSC_InstallingInfo { get; set; }

        public string SuiteId { get; set; }

        private int pageSizeValue;
        public int PageSizeValue
        {
            get { return pageSizeValue; }
            set
            {
                pageSizeValue = value;
                if (this.PSC_InstallingInfo != null)
                {
                    this.PSC_InstallingInfo.PageSize = pageSizeValue;
                }
            }
        }
        public List<int> PageSizeList { get; set; }
        public IActionCommand QueryCommand { get; private set; }
        public IActionCommand ViewCommand { get; private set; }
        public IActionCommand DeleteCommand { get; private set; }

        private bool FirstRun;
        protected override void ActivateView(string viewName, System.Collections.Generic.IDictionary<string, object> viewParameters)
        {
            base.ActivateView(viewName, viewParameters);
            if (FirstRun)
            {
                FirstRun = false;
                return;
            }
            PSC_InstallingInfo.ToPage(currentIndex);
        }

        public SuiteInstallingVm()
        {
            FirstRun = true;
            QueryCommand = new ActionCommand<object>(obj => Query());
            PageSizeList = MaintainCommon.PageSizeList; //获取翻页控件下拉列表
            PageSizeValue = PageSizeList[0];//获取默认每页显示记录的数据
            InitPagedServerCollection();
        }

        private int currentIndex = 1;
        private void InitPagedServerCollection()
        {
            deviceInstallClient.GetInstallationInProgressExCompleted += deviceInstallClient_GetInstallationInProgressExCompleted;
            PSC_InstallingInfo = new PagedServerCollection<InstallationInfo>((pageIndex, pageSize) =>
            {
                pageSize = PageSizeValue;
                System.Threading.Interlocked.Exchange(ref currentIndex, pageIndex);
                PagingInfo pagingInfo = new PagingInfo() { PageIndex = pageIndex-1, PageSize = pageSize };
                string suiteId = SuiteId == null ? string.Empty : SuiteId.Trim();
                deviceInstallClient.GetInstallationInProgressExAsync(null, suiteId, null, null, null, null, pagingInfo);
            });
        }

        void deviceInstallClient_GetInstallationInProgressExCompleted(object sender, GetInstallationInProgressExCompletedEventArgs e)
        {
            try
            {
                PSC_InstallingInfo.loader_Finished(new PagedResult<InstallationInfo>
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
            PSC_InstallingInfo.MoveToFirstPage();
        }
    }
}
