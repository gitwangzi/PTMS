using Gsafety.PTMS.ServiceReference.MaitenanceRecordService;
using Gsafety.PTMS.Share;
using Jounce.Core.View;
using Jounce.Core.ViewModel;
using Jounce.Framework.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Gsafety.PTMS.Maintain.ViewModels
{
    [ExportAsViewModel(MaintainName.MaintenanceSimpleVm)]
    public class MaintenanceSimpleVm : BaseViewModel
    {
        
        private int currentIndex = 1;
        MaintenanceRecordServiceClient client = ServiceClientFactory.Create<MaintenanceRecordServiceClient>();
        //public PagedServerCollection<MaintainanceDetail> DataInfoPage { get; set; }

        public PagedServerCollection<MaintainanceDetail> PSC_MaintainanceDetail { get; set; }

        //public PagedServerCollection<MaintainInfo> DataInfoPage { get; set; }
        public string SuiteID { get; set; }
        public List<int> PageSizeList { get; set; }


        //public PagedCollectionView GpGrid
        //{
        //    get { return gpGrid; }
        //    set { gpGrid = value; RaisePropertyChanged(() => this.GpGrid); }
        //}

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
                    PSC_MaintainanceDetail.MoveToFirstPage();
                }
            }
        }

        #region 构造与初始化
        //Ctor
        public MaintenanceSimpleVm()
        {
            GoBackCommand = new ActionCommand<object>(obj => GoBackAction());
            
            UIInit();
            InitPagedServerCollection();
        }

        //private void InitPagedServerCollection()
        //{
        //}

        private void InitPagedServerCollection()
        {
            client.GetMaintenanceNoteRecordsCompleted += client_GetMaintenanceNoteRecordsCompleted;
            PSC_MaintainanceDetail = new PagedServerCollection<MaintainanceDetail>((pageIndex, pageSize) =>
            {
                pageSize = PageSizeValue;
                System.Threading.Interlocked.Exchange(ref currentIndex, pageIndex);
                PagingInfo pagingInfo = new PagingInfo() { PageIndex = pageIndex, PageSize = pageSize };
                //string suiteId = SuiteId == null ? string.Empty : SuiteId.Trim();
                //string vehicleId = VehicleId == null ? string.Empty : VehicleId.Trim();
                //string installStaffName = InstallStaffName == null ? string.Empty : InstallStaffName.Trim();
                client.GetMaintenanceNoteRecordsAsync(SuiteID, RepairType.Simple, null, null, pagingInfo);
            });
        }

        //Bingding view  values
        private void UIInit()
        {
            PageSizeList = new List<int> { 20, 40, 80 };
            PageSizeValue = 20;
        }
        #endregion

        //Invoke Server
        //private void InvokServer(int pageIndex, int pageSize)
        //{
        //    pageSize = PageSizeValue;
        //    System.Threading.Interlocked.Exchange(ref currentIndex, pageIndex);
        //    string districtCode = string.Empty;
        //    PagingInfo pagingInfo = new PagingInfo() { PageIndex = pageIndex, PageSize = pageSize };
        //    client.GetMaintenanceNoteRecordsAsync(SuiteID, null, null, pagingInfo);
        //}

        //Response Server
        void client_GetMaintenanceNoteRecordsCompleted(object sender, GetMaintenanceNoteRecordsCompletedEventArgs e)
        {

            try
            {
                PSC_MaintainanceDetail.loader_Finished(new PagedResult<MaintainanceDetail>
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

        //private IEnumerable<MaintainInfo> DataConverter(ObservableCollection<MaintainanceDetail> os)
        //{
        //    ObservableCollection<MaintainInfo> oss = new ObservableCollection<MaintainInfo>();
        //    foreach (var item in os)
        //    {
        //        MaintainInfo m = new MaintainInfo();
        //        m.Maintainer = item.Maintainer;
        //        m.MaintainTime = item.MaintainTime.HasValue?item.MaintainTime.Value.ToString():string.Empty;
        //        m.SuiteID = item.SuiteId;
        //        m.DeviceName = item.DeviceName;
        //        m.OldCode = item.OldID;
        //        m.NewCode = item.NewSuiteInfoID;
        //        m.IsMaintained = item.IsRePaired;
        //        oss.Add(m);
        //    }
        //    return oss;
        //}

        protected override void ActivateView(string viewName, System.Collections.Generic.IDictionary<string, object> viewParameters)
        {
            base.ActivateView(viewName, viewParameters);
            object value = "";
            if (viewParameters.TryGetValue("SuiteId", out value))
            {
                SuiteID = viewParameters["SuiteId"].ToString();
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged("SuiteID"));
                //DataInfoPage = new PagedServerCollection<MaintainInfo>(new Action<int, int>(InvokServer));
                PSC_MaintainanceDetail.ToPage(currentIndex);
            }
        }

        private void GoBackAction()
        {
            EventAggregator.Publish(new ViewNavigationArgs("MaintainRecord", new Dictionary<string, object>() { { "action", "return" } }));
        }
    }
}
