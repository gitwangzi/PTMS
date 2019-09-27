using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Jounce.Core.ViewModel;
using Jounce.Core.Command;
using Jounce.Core.Event;
using Jounce.Framework.Command;
using Jounce.Core.View;
using Jounce.Framework.ViewModel;
using System.Collections.Generic;
using Gsafety.PTMS.ServiceReference.DeviceAlertService;
using System.Collections.ObjectModel;
using Gsafety.PTMS.Share;
using System.Windows.Data;

namespace Gsafety.PTMS.Installation.ViewModels
{
    [ExportAsViewModel(InstallationName.WaitMaintainVm)]
    public class WaitMaintainViewModel : BaseViewModel
    {
        private DeviceAlertServiceClient DeviceAlertClient = ServiceClientFactory.Create<DeviceAlertServiceClient>();
        public DeviceAlertHandle CurrentDeviceAlertHandle { get; set; }
        public PagedServerCollection<DeviceAlertHandle> PSC_DeviceAlertHandle { get; set; }

        public string VehicleId { get; set; }
        public string InstallStaffName { get; set; }
        public DateTime BeginDate { get; set; }
        public DateTime EndDate { get; set; }

        private string InstallstationId = ApplicationContext.Instance.AuthenticationInfo.OrgCode;

        public List<int> PageSizeList { get; set; }
        private List<int> pageSizeList
        {
            get
            {
                List<int> pageSizeList = new List<int>();
                pageSizeList.Add(20);
                pageSizeList.Add(40);
                pageSizeList.Add(80);
                return pageSizeList;
            }
        }
        private int pageSizeValue;
        public int PageSizeValue
        {
            get { return pageSizeValue; }
            set
            {
                pageSizeValue = value;
                if (this.PSC_DeviceAlertHandle != null)
                {
                    this.PSC_DeviceAlertHandle.PageSize = pageSizeValue;
                }
            }
        }
        public IActionCommand QueryCommand { get; private set; }
        public IActionCommand RegisterCommand { get; private set; }

        private bool FirstRun;
        protected override void ActivateView(string viewName, System.Collections.Generic.IDictionary<string, object> viewParameters)
        {
            base.ActivateView(viewName, viewParameters);
            if (FirstRun)
            {
                FirstRun = false;
                return;
            }
            PSC_DeviceAlertHandle.ToPage(currentIndex);
        }

        public WaitMaintainViewModel()
        {
            FirstRun = true;
            BeginDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
            EndDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 23, 59, 59);
            QueryCommand = new ActionCommand<object>(obj => Query());
            RegisterCommand = new ActionCommand<object>(obj => Register());

            PageSizeList = pageSizeList; //Get flip control drop-down list
            PageSizeValue = PageSizeList[0];//Gets the default page to display the recorded data
            InitPagedServerCollection();
        }

        private int currentIndex = 1;
        private void InitPagedServerCollection()
        {
            DeviceAlertClient.GetDeviceUnhandleAlertFuzzyCompleted += DeviceAlertClient_GetDeviceUnhandleAlertFuzzyCompleted;
            PSC_DeviceAlertHandle = new PagedServerCollection<DeviceAlertHandle>((pageIndex, pageSize) =>
            {
                pageSize = PageSizeValue;
                System.Threading.Interlocked.Exchange(ref currentIndex, pageIndex);
                PagingInfo pagingInfo = new PagingInfo() { PageIndex = pageIndex, PageSize = pageSize };
                string vehicleId = VehicleId == null ? string.Empty : VehicleId.Trim();
                string installStaffName = InstallStaffName == null ? string.Empty : InstallStaffName.Trim();
                DateTime beginDate = new DateTime(BeginDate.Year, BeginDate.Month, BeginDate.Day, 0, 0, 0);
                DateTime endDate = new DateTime(EndDate.Year, EndDate.Month, EndDate.Day, 23, 59, 59);
                DeviceAlertClient.GetDeviceUnhandleAlertFuzzyAsync(InstallstationId, vehicleId, beginDate, endDate, pagingInfo);
            });
        }

        void DeviceAlertClient_GetDeviceUnhandleAlertFuzzyCompleted(object sender, GetDeviceUnhandleAlertFuzzyCompletedEventArgs e)
        {
            try
            {
                PSC_DeviceAlertHandle.loader_Finished(new PagedResult<DeviceAlertHandle>
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
                MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("BASEINFO_Operate_Failed"));
            }
        }

        private void Query()
        {
            currentIndex = 1;
            PSC_DeviceAlertHandle.MoveToFirstPage();
        }

        private void Register()
        {
            EventAggregator.Publish(new ViewNavigationArgs(InstallationName.VehicleRegisterV, new Dictionary<string, object>() { { "Id", CurrentDeviceAlertHandle.Id }, { "vehicleId", CurrentDeviceAlertHandle.VehicleId }, { "mdvrCoreId", CurrentDeviceAlertHandle.MdvrCoreId } }));
        }

    }
}
