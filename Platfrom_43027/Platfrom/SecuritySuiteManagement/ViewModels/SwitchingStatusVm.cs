using Gsafety.PTMS.BaseInformation;
using Gsafety.PTMS.ServiceReference.DeviceInstallService;
using Gsafety.PTMS.Share;
using Jounce.Core.Command;
using Jounce.Core.View;
using Jounce.Core.ViewModel;
using System.ComponentModel;
using System.ComponentModel.Composition;
using Jounce.Framework.Command;
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
using System.Reflection;
using BaseLib.ViewModels;
using BaseLib.Model;


namespace Gsafety.PTMS.SecuritySuite.ViewModels
{
    [ExportAsViewModel(SecuritySuiteName.SwitchingStatusVm)]
    public class SwitchingStatusVm : BaseViewModel
    {
        private DeviceInstallServiceClient DeviceInstallClient;
        private bool isFirst;

        #region
        public string SuiteId { get; set; }
        public string VehicleId { get; set; }
        private int pageSizeValue;
        public int PageSizeValue
        {
            get { return pageSizeValue; }
            set
            {
                pageSizeValue = value;
                //if (PSC_SelfInspectInfo != null)
                //{
                //    PSC_SelfInspectInfo.PageSize = value;
                //}
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => PageSizeValue));
            }
        }
        //public SelfInspectDetail CurrentSelfInspectInfo { get; set; }
        //public PagedServerCollection<SelfInspectDetail> PSC_SelfInspectInfo { get; set; }
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
        #endregion
        public List<int> PageSizeList { get; set; }
        public IActionCommand QueryCommand { get; private set; }
        public IActionCommand ViewCommand { get; private set; }

        protected override void ActivateView(string viewName, System.Collections.Generic.IDictionary<string, object> viewParameters)
        {
            base.ActivateView(viewName, viewParameters);
        }

        public SwitchingStatusVm()
        {
            try
            {
                isFirst = true;
                DeviceInstallClient = ServiceClientFactory.Create<DeviceInstallServiceClient>();
                ViewCommand = new ActionCommand<object>(obj => Publish("view"));
                QueryCommand = new ActionCommand<object>(obj => Query());
                PageSizeList = Gsafety.PTMS.BaseInformation.BaseInformationCommon.PageSizeList; //get the list of turning page
                PageSizeValue = PageSizeList[0];//get the number of items in everypage
                InitPagedServerCollection();
                //DeviceInstallClient.GetSuiteInspectFuzzyCompleted += DeviceInstallClient_GetSuiteInspectFuzzyCompleted;
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("SwitchingStatusVm()", ex);
            }
        }

        private int currentIndex = 1;
        private void InitPagedServerCollection()
        {
            try
            {
                //    PSC_SelfInspectInfo = new PagedServerCollection<SelfInspectDetail>((pageIndex, pageSize) =>
                //    {
                //        pageSize = PageSizeValue;
                //        System.Threading.Interlocked.Exchange(ref currentIndex, pageIndex);
                //        PagingInfo pagingInfo = new PagingInfo() { PageIndex = pageIndex, PageSize = pageSize };
                //        string suiteId = SuiteId == null ? string.Empty : SuiteId.Trim();
                //        string vehicleId = VehicleId == null ? string.Empty : VehicleId.Trim();
                //        DateTime beginDate = new DateTime(BeginDate.Year, BeginDate.Month, BeginDate.Day, 0, 0, 0);
                //        DateTime endDate = new DateTime(EndDate.Year, EndDate.Month, EndDate.Day, 23, 59, 59);
                //        ServiceClientFactory.CreateMessageHeader(DeviceInstallClient.InnerChannel);
                //        DeviceInstallClient.GetSuiteInspectFuzzyAsync(vehicleId, suiteId, beginDate, endDate, pagingInfo);
                //    });
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("SwitchingStatusVm InitPagedServerCollection", ex);
            }
        }

        //void DeviceInstallClient_GetSuiteInspectFuzzyCompleted(object sender, GetSuiteInspectFuzzyCompletedEventArgs e)
        //{
        //    try
        //    {
        //        PSC_SelfInspectInfo.loader_Finished(new PagedResult<SelfInspectDetail>
        //        {
        //            Count = e.Result.TotalRecord,
        //            Items = e.Result.Result,
        //            PageIndex = currentIndex,
        //        });
        //        if (e.Result.TotalRecord == 0&&!isFirst)
        //        {
        //            //MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("MAINTAIN_DataEmpty"), ApplicationContext.Instance.StringResourceReader.GetString("TRAFFIC_TIP"), MessageBoxButton.OK);
        //        }
        //        isFirst = false;
        //    }
        //    catch(Exception ex)
        //    {
        //        ApplicationContext.Instance.Logger.LogException("SwitchingStatusVm", ex);
        //        ApplicationContext.Instance.Logger.LogException("DeviceInstallClient_GetSuiteInspectFuzzyCompleted()", ex);
        //    }
        //}

        private void Query()
        {
            try
            {
                //PSC_SelfInspectInfo.MoveToFirstPage();
                //InitPagedServerCollection();
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("SwitchingStatusVm Query", ex);
            }
        }

        private void Publish(string name)
        {
            try
            {
                //EventAggregator.Publish(new ViewNavigationArgs(SecuritySuiteName.SwitchingStatusDisplayV, new Dictionary<string, object>() { { "action", name }, { name, CurrentSelfInspectInfo } }));
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("SwitchingStatusVm Publish", ex);
            }
        }

    }
}
