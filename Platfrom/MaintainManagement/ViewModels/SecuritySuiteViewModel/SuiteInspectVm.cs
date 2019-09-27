using Gsafety.PTMS.ServiceReference.DeviceInstallService;
using Gsafety.PTMS.Share;
using Jounce.Core.Command;
using Jounce.Core.View;
using Jounce.Core.ViewModel;
using Jounce.Framework.Command;
/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: e19b2ec8-24d3-4624-8359-fd418a6f42df      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-PENGGL
/////                 Author: TEST(penggl)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Maintain.ViewModels.SecuritySuiteViewModel
/////    Project Description:    
/////             Class Name: SuiteInspectVm
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/11/4 18:44:40
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/11/4 18:44:40
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
    [ExportAsViewModel(MaintainName.SuiteInspectVm)]
    public class SuiteInspectVm : BaseViewModel
    {
        private DeviceInstallServiceClient DeviceInstallClient = ServiceClientFactory.Create<DeviceInstallServiceClient>();
        public SelfInspectInfo CurrentSelfInspectInfo { get; set; }
        public PagedServerCollection<SelfInspectInfo> PSC_SelfInspectInfo { get; set; }

        public string SuiteId { get; set; }
        public string VehicleId { get; set; }
        private int pageSizeValue;
        public int PageSizeValue
        {
            get { return pageSizeValue; }
            set
            {
                pageSizeValue = value;
                if (this.PSC_SelfInspectInfo != null)
                {
                    this.PSC_SelfInspectInfo.PageSize = pageSizeValue;
                }
            }
        }

        private DateTime _BeginDate = DateTime.Now.AddDays(-1);
        public DateTime BeginDate
        {
            get
            {
                return _BeginDate;
            }
            set
            {
                _BeginDate = value;
            }
        }

        private DateTime _EndDate = DateTime.Now;
        public DateTime EndDate
        {
            get
            {
                return _EndDate;
            }
            set
            {
                _EndDate = value.AddHours(24); ;
            }
        }

        public List<int> PageSizeList { get; set; }
        public IActionCommand UpdateCommand { get; private set; }
        public IActionCommand DeleteCommand { get; private set; }
        public IActionCommand QueryCommand { get; private set; }
        public IActionCommand ViewCommand { get; private set; }
        public IActionCommand AddCommand { get; private set; }
        public IActionCommand DownloadCommand { get; private set; }
        public IActionCommand UploadCommand { get; private set; }

        private bool FirstRun;
        protected override void ActivateView(string viewName, System.Collections.Generic.IDictionary<string, object> viewParameters)
        {
            base.ActivateView(viewName, viewParameters);
            if (FirstRun)
            {
                FirstRun = false;
                return;
            }
            PSC_SelfInspectInfo.ToPage(currentIndex);
        }

        public SuiteInspectVm()
        {
            FirstRun = true;
            ViewCommand = new ActionCommand<object>(obj => Publish("view"));
            QueryCommand = new ActionCommand<object>(obj => Query());
            PageSizeList = MaintainCommon.PageSizeList; //获取翻页控件下拉列表
            PageSizeValue = PageSizeList[0];//获取默认每页显示记录的数据
            InitPagedServerCollection();
        }

        private int currentIndex = 1;

        private void InitPagedServerCollection()
        {
            DeviceInstallClient.GetSuiteInspectFuzzyCompleted += DeviceInstallClient_GetSuiteInspectFuzzyCompleted;
            PSC_SelfInspectInfo = new PagedServerCollection<SelfInspectInfo>((pageIndex, pageSize) =>
            {
                pageSize = PageSizeValue;
                System.Threading.Interlocked.Exchange(ref currentIndex, pageIndex);
                PagingInfo pagingInfo = new PagingInfo() { PageIndex = pageIndex, PageSize = pageSize };
                string suiteId = SuiteId == null ? string.Empty : SuiteId.Trim();
                string vehicleId = VehicleId == null ? string.Empty : VehicleId.Trim();
                DateTime beginDate = new DateTime(BeginDate.Year, BeginDate.Month, BeginDate.Day, 0, 0, 0);
                DateTime endDate = new DateTime(EndDate.Year, EndDate.Month, EndDate.Day, 23, 59, 59);
                DeviceInstallClient.GetSuiteInspectFuzzyAsync(vehicleId,suiteId,  beginDate, endDate, pagingInfo);
            });
        }

        void DeviceInstallClient_GetSuiteInspectFuzzyCompleted(object sender, GetSuiteInspectFuzzyCompletedEventArgs e)
        {
            try
            {
                PSC_SelfInspectInfo.loader_Finished(new PagedResult<SelfInspectInfo>
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
            PSC_SelfInspectInfo.MoveToFirstPage();
        }

        private void Publish(string name)
        {
            EventAggregator.Publish(new ViewNavigationArgs(MaintainName.SuiteInspectDisplayV, new Dictionary<string, object>() { { "action", name }, { name, CurrentSelfInspectInfo } }));
        }

    }
}
