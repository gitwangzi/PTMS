using Gsafety.PTMS.ServiceReference.MaitenanceRecordService;
using Gsafety.PTMS.Share;
using Jounce.Core.View;
using Jounce.Core.ViewModel;
using Jounce.Framework.Command;
using System.Collections.Generic;
using System.Windows.Input;
using Jounce.Core.Command;
using System.Windows;

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
    [ExportAsViewModel(MaintainName.MaintenanceDetailViewModel)]
    public class MaintenanceDetailViewModel : BaseViewModel
    {
        #region Jiaoyx
        //    private MaintenanceRecordServiceClient client;
        //    public ICommand PrintCommand { get; set; }
        //    public ICommand BackCommand { get; set; }
        //    public ICommand QueryCommand { get; set; }

        //    public CompareSuiteInfo RecordDetail { get; set; }

        //    private ObservableCollection<MatchResult> _matchResults;

        //    public ObservableCollection<MatchResult> MatchResults
        //    {
        //        get { return _matchResults; }
        //        set
        //        {
        //            _matchResults = value;
        //            RaisePropertyChanged(() => this.MatchResults);
        //        }
        //    }


        //    public string MaintenanceID { get; set; }

        //    protected override void ActivateView(string viewName, System.Collections.Generic.IDictionary<string, object> viewParameters)
        //    {
        //        MaintenanceID = viewParameters["ID"].ToString();
        //        base.ActivateView(viewName, viewParameters);
        //    }

        //    public MaintenanceDetailViewModel()
        //    {
        //        client = ServiceClientFactory.Create<MaintenanceRecordServiceClient>();
        //        PrintCommand = new ActionCommand<object>(obj => PrintAction());
        //        BackCommand = new ActionCommand<object>(obj => BackAction());
        //        QueryCommand = new ActionCommand<object>(obj => QueryAction());
        //        client.GetMaintenanceRecordCompleted += client_GetMaintenanceRecordCompleted;
        //        MatchResults = new ObservableCollection<MatchResult>();
        //    }

        //    void client_GetMaintenanceRecordCompleted(object sender, GetMaintenanceRecordCompletedEventArgs e)
        //    {
        //        RecordDetail = e.Result.Result;
        //        if (RecordDetail != null)
        //        {
        //            MatchResults = new ObservableCollection<MatchResult>(Compare(RecordDetail.OldRecord, RecordDetail.NewRecord));
        //        }
        //    }

        //    private void QueryAction()
        //    {
        //        client.GetMaintenanceRecordAsync(MaintenanceID);
        //    }

        //    private void BackAction()
        //    {
        //        EventAggregator.Publish(new ViewNavigationArgs("MaintenanceListView", new Dictionary<string, object>() { { "action", "return" } }));
        //    }

        //    private void PrintAction()
        //    {

        //    }

        //    private List<MatchResult> Compare(SecuritySuiteInfo x, SecuritySuiteInfo y)
        //    {
        //        var result = new List<MatchResult>();

        //        foreach (var item in typeof(SecuritySuiteInfo).GetProperties())
        //        {
        //            string oldvalue = item.GetValue(x, null) == null ? null : item.GetValue(x, null).ToString();
        //            string newValue = item.GetValue(y, null) == null ? null : item.GetValue(y, null).ToString();
        //            if (oldvalue != newValue)
        //            {
        //                result.Add(new MatchResult
        //                {
        //                    Name = item.Name,
        //                    IsRePaired = true,
        //                    OldValue = oldvalue,
        //                    NewValue = newValue
        //                });
        //            }
        //            else
        //            {
        //                result.Add(new MatchResult
        //                {
        //                    Name = item.Name,
        //                    IsRePaired = false,
        //                    OldValue = oldvalue,
        //                    NewValue = newValue
        //                });

        //            }
        //        }
        //        return result;
        //    }
        //}

        //public class MatchResult : BaseNotify
        //{
        //    public string Name { get; set; }
        //    public string OldValue { get; set; }
        //    public string NewValue { get; set; }
        //    public bool IsRePaired { get; set; }
        #endregion

        MaintenanceRecordServiceClient client = ServiceClientFactory.Create<MaintenanceRecordServiceClient>();
        public MaintainsReports CurrentDataInfo { get; set; }

        public PagedServerCollection<MaintainsReports> DataInfoPage { get; set; }

        public IActionCommand QueryCommand { get; private set; }
        public ICommand BackCommand { get; set; }

        private int currentIndex = 1;
        public int PageSizeValue { get; set; }
        public string MaintanceID { get; set; }

        public string SuiteID { get; set; }
        public string Maintainer { get; set; }
        public string MaintainTime { get; set; }

        public List<int> PageSizeList { get; set; }

        public MaintenanceDetailViewModel()
        {
            QueryCommand = new ActionCommand<object>(obj => Query());  //查询事件
            BackCommand = new ActionCommand<object>(obj => GoBackAction());    //返回事件
            client.GetMaintainsDetailCompleted += client_GetMaintainsDetailCompleted;  //调用完成事件
            PageSizeList = new List<int> { 20, 40, 80 };
            PageSizeValue = 20;
            InitPagedServerCollection();
            Query();
        }

        private void Query()
        {
            currentIndex = 1;
            DataInfoPage.MoveToFirstPage();
        }

        private void InitPagedServerCollection()
        {
            DataInfoPage = new PagedServerCollection<MaintainsReports>((pageIndex, pageSize) =>
            {
                pageSize = PageSizeValue;
                System.Threading.Interlocked.Exchange(ref currentIndex, pageIndex);
                string districtCode = string.Empty;
                Gsafety.PTMS.ServiceReference.MaitenanceRecordService.PagingInfo pagingInfo = new Gsafety.PTMS.ServiceReference.MaitenanceRecordService.PagingInfo() { PageIndex = pageIndex, PageSize = pageSize };
                if (!string.IsNullOrEmpty(MaintanceID))
                {
                    client.GetMaintainsDetailAsync(MaintanceID, pagingInfo);
                }
            });
        }

        //远程服务对象操作
        void client_GetMaintainsDetailCompleted(object sender, GetMaintainsDetailCompletedEventArgs e)
        {
            try
            {
                DataInfoPage.loader_Finished(new PagedResult<MaintainsReports>
                {
                    Count = e.Result.TotalRecord,
                    Items = e.Result.Result as IEnumerable<MaintainsReports>,
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

        protected override void ActivateView(string viewName, System.Collections.Generic.IDictionary<string, object> viewParameters)
        {
            base.ActivateView(viewName, viewParameters);
            if (viewParameters.Count != 0)
            {
                MaintanceID = viewParameters["ID"] == null ? string.Empty : viewParameters["ID"].ToString();
                Maintainer = viewParameters["Maintainer"] == null ? string.Empty : viewParameters["Maintainer"].ToString();
                MaintainTime = viewParameters["MaintainTime"] == null ? string.Empty : viewParameters["MaintainTime"].ToString();
                SuiteID = viewParameters["SuiteId"] == null ? string.Empty : viewParameters["SuiteId"].ToString();
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(new string[] { "MaintanceID", "Maintainer", "MaintainTime", "SuiteID" }));
                DataInfoPage.MoveToFirstPage();
            }
        }

        private void GoBackAction()
        {
            EventAggregator.Publish(new ViewNavigationArgs("MaintenanceListView", new Dictionary<string, object>() { { "action", "return" } }));
        }
    }
}
