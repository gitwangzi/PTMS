using Gsafety.PTMS.ServiceReference.MaitenanceRecordService;
using Gsafety.PTMS.Share;
using Jounce.Core.ViewModel;
using System.Windows.Input;
using Jounce.Framework.Command;
using Jounce.Core.View;
using System.Collections.Generic;
using System;
using Jounce.Core.Command;
using System.Windows;
using Gsafety.Common.Localization.Resource;
using System.Windows.Data;
using System.Collections.ObjectModel;

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
/////Modified Time: 2013/9/27 00:00:00
/////Modified by:
/////Modified Description: 
/////======================================================================

namespace Gsafety.PTMS.Maintain.ViewModels
{
    [ExportAsViewModel(MaintainName.MaintenanceListViewModel)]
    public class MaintenanceListViewModel : BaseViewModel
    {
#if oldCode
        #region  Shihs  2013/09/25

        private int currentIndex = 1;
        MaintenanceRecordServiceClient client = ServiceClientFactory.Create<MaintenanceRecordServiceClient>();
        //public MaintainsReports CurrentDataInfo { get; set; }
        public MaintainanceDetail CurrentDataInfo { get; set; }
        public PagedServerCollection<MaintainanceDetail> DataInfoPage { get; set; }
        public string SuiteID { get; set; }
        public string QuerySuiteID { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public List<int> PageSizeList { get; set; }

        public IActionCommand QueryCommand { get; private set; }
        public ICommand GoBackCommand { get; set; }
        public ICommand DetailCommand { get; set; }


        //public int PageSizeValue { get; set; }

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

        #region 构造与初始化
        //Ctor
        public MaintenanceListViewModel()
        {
            QueryCommand = new ActionCommand<object>(obj => Query());
            GoBackCommand = new ActionCommand<object>(obj => GoBackAction());
            DetailCommand = new ActionCommand<object>(obj => DetailAction());
            client.GetMaintenanceRecordsCompleted += client_GetMaintenanceRecordsCompleted;
            UIInit();
            //InitPagedServerCollection();
        }

        private void InitPagedServerCollection()
        {
        }

        //Bingding view  values
        private void UIInit()
        {
            PageSizeList = new List<int> { 20, 40, 80 };
            PageSizeValue = 20;
            StartTime = DateTime.Now.AddMonths(-1);
            EndTime = DateTime.Now;
        }
        #endregion

        //Invoke Server
        private void InvokServer(int pageIndex, int pageSize)
        {
            pageSize = PageSizeValue;
            System.Threading.Interlocked.Exchange(ref currentIndex, pageIndex);
            string districtCode = string.Empty;
            PagingInfo pagingInfo = new PagingInfo() { PageIndex = pageIndex, PageSize = pageSize };
            client.GetMaintenanceRecordsAsync(SuiteID, StartTime, EndTime, pagingInfo);
        }

        //Response Server
        void client_GetMaintenanceRecordsCompleted(object sender, GetMaintenanceRecordsCompletedEventArgs e)
        {
            DataInfoPage.loader_Finished(new PagedResult<MaintainanceDetail>
            {
                Count = e.Result.TotalRecord,
                Items = e.Result.Result as IEnumerable<MaintainanceDetail>,
                PageIndex = currentIndex,
            });
            Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(new string[] { "DataInfoPage", "PageSizeValue" }));
        }

        private void DetailAction()
        {
            string s = CurrentDataInfo.ID;
            EventAggregator.Publish(new ViewNavigationArgs("MaintenanceDetailView",
                new Dictionary<string, object>() { { "ID", CurrentDataInfo.ID }, { "Maintainer", CurrentDataInfo.Maintainer }, { "MaintainTime", CurrentDataInfo.MaintainTime }, { "SuiteId", CurrentDataInfo.SuiteId } }));
        }

        private void Query()
        {
            if (StartTime.HasValue && EndTime.HasValue && StartTime.Value.CompareTo(EndTime) > 0)
            {
                string timeError = StringResource.ResourceManager.GetString("TimeError");
                string msg = string.IsNullOrEmpty(timeError) ? "StrartTime can not be greater than EndTime !" : timeError;
                string warning = StringResource.ResourceManager.GetString("Warning");
                MessageBox.Show(msg, string.IsNullOrEmpty(warning) ? "Warning !" : warning, MessageBoxButton.OK);
            }
            else
            {
                DataInfoPage = new PagedServerCollection<MaintainanceDetail>((pageIndex, pageSize) =>
                {
                    pageSize = PageSizeValue;
                    System.Threading.Interlocked.Exchange(ref currentIndex, pageIndex);
                    string districtCode = string.Empty;
                    PagingInfo pagingInfo = new PagingInfo() { PageIndex = pageIndex, PageSize = pageSize };
                    client.GetMaintenanceRecordsAsync(QuerySuiteID, StartTime, EndTime, pagingInfo);
                });
                DataInfoPage.ToPage(currentIndex);
            }
        }

        protected override void ActivateView(string viewName, System.Collections.Generic.IDictionary<string, object> viewParameters)
        {
            base.ActivateView(viewName, viewParameters);

            object value = "";
            if (viewParameters.TryGetValue("SuiteId", out value))
            {
                SuiteID = viewParameters["SuiteId"].ToString();
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged("SuiteID"));
                DataInfoPage = new PagedServerCollection<MaintainanceDetail>(new Action<int, int>(InvokServer));
                DataInfoPage.ToPage(currentIndex);
            }
        }

        private void GoBackAction()
        {
            EventAggregator.Publish(new ViewNavigationArgs("MaintainRecord", new Dictionary<string, object>() { { "action", "return" } }));
        }
        #endregion

#endif



        private int currentIndex = 1;
        MaintenanceRecordServiceClient client = ServiceClientFactory.Create<MaintenanceRecordServiceClient>();
        //public PagedServerCollection<MaintainanceDetail> DataInfoPage { get; set; }
        public PagedServerCollection<MaintainInfo> DataInfoPage { get; set; }
        public string SuiteID { get; set; }
        public List<int> PageSizeList { get; set; }
        private PagedCollectionView gpGrid;

        public PagedCollectionView GpGrid
        {
            get { return gpGrid; }
            set { gpGrid = value; RaisePropertyChanged(() => this.GpGrid); }
        }

        public ICommand GoBackCommand { get; set; }

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

        #region 构造与初始化
        //Ctor
        public MaintenanceListViewModel()
        {
            GoBackCommand = new ActionCommand<object>(obj => GoBackAction());
            client.GetMaintenanceRecordsCompleted += client_GetMaintenanceRecordsCompleted;
            UIInit();
            //InitPagedServerCollection();
        }

        private void InitPagedServerCollection()
        {
        }

        //Bingding view  values
        private void UIInit()
        {
            PageSizeList = new List<int> { 20, 40, 80 };
            PageSizeValue = 20;
        }
        #endregion

        //Invoke Server
        private void InvokServer(int pageIndex, int pageSize)
        {
            pageSize = PageSizeValue;
            System.Threading.Interlocked.Exchange(ref currentIndex, pageIndex);
            string districtCode = string.Empty;
            PagingInfo pagingInfo = new PagingInfo() { PageIndex = pageIndex, PageSize = pageSize };
            client.GetMaintenanceRecordsAsync(SuiteID, null, null, pagingInfo);
        }

        //Response Server
        void client_GetMaintenanceRecordsCompleted(object sender, GetMaintenanceRecordsCompletedEventArgs e)
        {
            try
            {
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(new string[] { "DataInfoPage", "PageSizeValue" }));
                GpGrid = new PagedCollectionView(DataConverter(e.Result.Result));
                GpGrid.GroupDescriptions.Add(new PropertyGroupDescription("MaintainTime"));
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

        private IEnumerable<MaintainInfo> DataConverter(ObservableCollection<MaintainanceDetail> os)
        {
            ObservableCollection<MaintainInfo> oss = new ObservableCollection<MaintainInfo>();
            foreach (var item in os)
            {
                MaintainInfo m = new MaintainInfo();
                m.Maintainer = item.Maintainer;
                m.MaintainTime = item.MaintainTime.HasValue?item.MaintainTime.Value.ToString():string.Empty;
                m.SuiteID = item.SuiteId;
                m.DeviceName = item.DeviceName;
                m.OldCode = item.OldID;
                m.NewCode = item.NewSuiteInfoID;
                m.IsMaintained = item.IsRePaired;
                oss.Add(m);
            }
            return oss;
        }

        protected override void ActivateView(string viewName, System.Collections.Generic.IDictionary<string, object> viewParameters)
        {
            base.ActivateView(viewName, viewParameters);
            object value = "";
            if (viewParameters.TryGetValue("SuiteId", out value))
            {
                SuiteID = viewParameters["SuiteId"].ToString();
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged("SuiteID"));
                DataInfoPage = new PagedServerCollection<MaintainInfo>(new Action<int, int>(InvokServer));
                DataInfoPage.ToPage(currentIndex);
            }
        }

        private void GoBackAction()
        {
            EventAggregator.Publish(new ViewNavigationArgs("MaintainRecord", new Dictionary<string, object>() { { "action", "return" } }));
        }
    }
}
