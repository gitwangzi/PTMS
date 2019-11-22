using Jounce.Core.View;
using Jounce.Core.ViewModel;
using Jounce.Framework.Command;
using Jounce.Framework.ViewModel;
/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 7abd01bc-ccc7-459e-b713-75cf24bce38d      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-PENGGL
/////                 Author: TEST(penggl)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Maintain.ViewModels
/////    Project Description:    
/////             Class Name: SuiteQueryVm
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/9/11 16:46:46
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/9/11 16:46:46
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
using Gsafety.PTMS.ServiceReference.SecuritySuiteService;
using Jounce.Core.Command;
using Gsafety.PTMS.Share;
using System.Linq;

namespace Gsafety.PTMS.Maintain.ViewModels
{
    [ExportAsViewModel(MaintainName.SecuritySuiteVm)]
    public class SecuritySuiteVm : BaseViewModel
    {
        private SecuritySuiteServiceClient SecuritySuiteClient = ServiceClientFactory.Create<SecuritySuiteServiceClient>();
        public DeviceSuite CurrentSecuritySuite { get; set; }
        public PagedServerCollection<DeviceSuite> PSC_SecuritySuite { get; set; }

        public string SuiteId { get; set; }
        public string VehicleId { get; set; }
        public string InstallStaffName { get; set; }
        private int pageSizeValue;
        public int PageSizeValue
        {
            get { return pageSizeValue; }
            set
            {
                pageSizeValue = value;
                if (this.PSC_SecuritySuite != null)
                {
                    this.PSC_SecuritySuite.PageSize = pageSizeValue;
                }
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

        public string CurrentSuiteStatus { get; set; }

        public ObservableCollection<string> SuiteStatusList
        {
            get
            {
                ObservableCollection<string> enumList = new ObservableCollection<string>(Enum.GetNames(typeof(DeviceSuiteStatus)).Select(x => x));
                enumList.Remove("None");
                enumList.Insert(0, ApplicationContext.Instance.StringResourceReader.GetString("BASEINFO_PleaseSelect"));
                return enumList;
            }
        }
        private bool FirstRun;
        protected override void ActivateView(string viewName, System.Collections.Generic.IDictionary<string, object> viewParameters)
        {
            base.ActivateView(viewName, viewParameters);
            if (FirstRun)
            {
                FirstRun = false;
                return;
            }
            PSC_SecuritySuite.ToPage(currentIndex);
        }

        public SecuritySuiteVm()
        {
            FirstRun = true;
            ViewCommand = new ActionCommand<object>(obj => Publish("view"));
            QueryCommand = new ActionCommand<object>(obj => Query());
            CurrentSuiteStatus = SuiteStatusList[0];

            PageSizeList = MaintainCommon.PageSizeList; //获取翻页控件下拉列表
            PageSizeValue = PageSizeList[0];//获取默认每页显示记录的数据
            InitPagedServerCollection();
        }

        private int currentIndex = 1;

        private void InitPagedServerCollection()
        {
            SecuritySuiteClient.GetSecuritySuitesFuzzyCompleted += SecuritySuiteClient_GetSecuritySuitesFuzzyCompleted;
            PSC_SecuritySuite = new PagedServerCollection<DeviceSuite>((pageIndex, pageSize) =>
            {
                pageSize = PageSizeValue;
                System.Threading.Interlocked.Exchange(ref currentIndex, pageIndex);
                PagingInfo pagingInfo = new PagingInfo() { PageIndex = pageIndex, PageSize = pageSize };
                string suiteId = SuiteId == null ? string.Empty : SuiteId.Trim();
                string vehicleId = VehicleId == null ? string.Empty : VehicleId.Trim();
                string installStaffName = InstallStaffName == null ? string.Empty : InstallStaffName.Trim();
                SecuritySuiteClient.GetSecuritySuitesFuzzyAsync(vehicleId, suiteId, null, null, null, pagingInfo);
            });
        }

        void SecuritySuiteClient_GetSecuritySuitesFuzzyCompleted(object sender, GetSecuritySuitesFuzzyCompletedEventArgs e)
        {
            try
            {
                PSC_SecuritySuite.loader_Finished(new PagedResult<DeviceSuite>
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
            PSC_SecuritySuite.MoveToFirstPage();
        }

        private void Publish(string name)
        {
            EventAggregator.Publish(new ViewNavigationArgs(MaintainName.SecuritySuiteDisplayV, new Dictionary<string, object>() { { "action", name }, { name, CurrentSecuritySuite } }));
        }

    }
}
