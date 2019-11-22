using Gsafety.PTMS.ServiceReference.MaitenanceRecordService;
using Gsafety.PTMS.Share;
using Jounce.Core.Command;
using Jounce.Core.View;
using Jounce.Core.ViewModel;
using Jounce.Framework.Command;
/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: f5fdec44-d83d-41fe-9816-38da3651334c      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-PENGGL
/////                 Author: TEST(penggl)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Maintain.ViewModels
/////    Project Description:    
/////             Class Name: HandleRecordVm
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/11/21 14:57:25
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/11/21 14:57:25
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

namespace Gsafety.PTMS.Maintain.ViewModels
{
    [ExportAsViewModel(MaintainName.HandleRecordVm)]
    public class HandleRecordVm : BaseViewModel
    {
        private MaintenanceRecordServiceClient MaintenanceClient = ServiceClientFactory.Create<MaintenanceRecordServiceClient>();
        public HandleRecord CurrentHandleRecord { get; set; }
        public PagedServerCollection<HandleRecord> PSC_HandleRecord { get; set; }

        public string VehicleId { get; set; }
        private int pageSizeValue;
        public int PageSizeValue
        {
            get { return pageSizeValue; }
            set
            {
                pageSizeValue = value;
                if (this.PSC_HandleRecord != null)
                {
                    this.PSC_HandleRecord.PageSize = pageSizeValue;
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

        //public ObservableCollection<string> SuiteStatusList
        //{
        //    get
        //    {
        //        ObservableCollection<string> enumList = new ObservableCollection<string>(Enum.GetNames(typeof(HandleRecordStatus)).Select(x => x));
        //        enumList.Remove("None");
        //        enumList.Insert(0, ApplicationContext.Instance.StringResourceReader.GetString("BASEINFO_PleaseSelect"));
        //        return enumList;
        //    }
        //}

        protected override void ActivateView(string viewName, System.Collections.Generic.IDictionary<string, object> viewParameters)
        {
            base.ActivateView(viewName, viewParameters);
            if (viewParameters.Count != 0 && viewParameters["action"].ToString() == "return")
            {
                PSC_HandleRecord.ToPage(currentIndex);
            }
        }

        public HandleRecordVm()
        {
            ViewCommand = new ActionCommand<object>(obj => Publish("view"));
            QueryCommand = new ActionCommand<object>(obj => Query());
            //CurrentSuiteStatus = SuiteStatusList[0];

            PageSizeList = MaintainCommon.PageSizeList; //获取翻页控件下拉列表
            PageSizeValue = PageSizeList[0];//获取默认每页显示记录的数据
            InitPagedServerCollection();
        }

        private int currentIndex = 1;

        private void InitPagedServerCollection()
        {
            MaintenanceClient.GetHandleRecordFuzzyCompleted += MaintenanceClient_GetHandleRecordFuzzyCompleted;
            PSC_HandleRecord = new PagedServerCollection<HandleRecord>((pageIndex, pageSize) =>
            {
                pageSize = PageSizeValue;
                string vehicleId = VehicleId == null ? string.Empty : VehicleId.Trim();
                PagingInfo pagingInfo = new PagingInfo() { PageIndex = pageIndex, PageSize = pageSize };
                MaintenanceClient.GetHandleRecordFuzzyAsync(vehicleId, pagingInfo);
            });
        }

        void MaintenanceClient_GetHandleRecordFuzzyCompleted(object sender, GetHandleRecordFuzzyCompletedEventArgs e)
        {
            try
            {
                PSC_HandleRecord.loader_Finished(new PagedResult<HandleRecord>
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
            catch(Exception ex)
            {
                ApplicationContext.Instance.Logger.LogError("HandleRecordVm", ex);
            }
        }

        private void Query()
        {
            currentIndex = 1;
            PSC_HandleRecord.MoveToFirstPage();
        }

        private void Publish(string name)
        {
            EventAggregator.Publish(new ViewNavigationArgs(MaintainName.HandleRecordDetailV, new Dictionary<string, object>() { { "action", name }, { name, CurrentHandleRecord } }));
        }

    }
}
