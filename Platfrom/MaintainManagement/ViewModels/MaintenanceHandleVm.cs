
using Gsafety.PTMS.Share;
using Jounce.Core.Command;
using Jounce.Core.View;
using Jounce.Core.ViewModel;
using Jounce.Framework.Command;
/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 2251a571-63a3-4ba4-958d-607b7f9a90e2      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-PENGGL
/////                 Author: TEST(penggl)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Maintain.ViewModels
/////    Project Description:    
/////             Class Name: AbnormalSuiteQueryVm
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/9/12 11:28:02
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/9/12 11:28:02
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
using Gsafety.PTMS.ServiceReference.WorkingSuiteService;
using System.Text;
using Jounce.Core.Event;

namespace Gsafety.PTMS.Maintain.ViewModels
{

    [ExportAsViewModel(MaintainName.MaintenanceHandleVm)]
    public class MaintenanceHandleVm : BaseViewModel,
        IEventSink<BasicInfo>,   ////
        IEventSink<Enviroment>,  ////
        IEventSink<Hardware>     ////
    {
        private WorkingSuiteServiceClient workingSuiteClient = ServiceClientFactory.Create<WorkingSuiteServiceClient>();
        public WorkingSuite CurrentWorkingSuite { get; set; }
        public PagedServerCollection<WorkingSuite> PSC_WorkingSuite { get; set; }
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
                if (this.PSC_WorkingSuite != null)
                {
                    this.PSC_WorkingSuite.PageSize = pageSizeValue;
                }
            }
        }
        public List<int> PageSizeList { get; set; }
        public IActionCommand GetCommand { get; private set; }
        public IActionCommand ArrangeCommand { get; private set; }
        public IActionCommand QueryCommand { get; private set; }
        public IActionCommand ViewCommand { get; private set; }

        protected override void ActivateView(string viewName, System.Collections.Generic.IDictionary<string, object> viewParameters)
        {
            base.ActivateView(viewName, viewParameters);
            if (viewParameters.Count != 0 && viewParameters["action"].ToString() == "return")
            {
                PSC_WorkingSuite.ToPage(currentIndex);
            }
        }

        public MaintenanceHandleVm()
        {
            workingSuiteClient.GetLastestSuiteRunningDetailCompleted += workingSuiteClient_GetLastestSuiteRunningDetailCompleted;
            QueryCommand = new ActionCommand<object>(obj => QueryAbnormalSuite());
            ArrangeCommand = new ActionCommand<object>(obj => Publish("arrange"));
            GetCommand = new ActionCommand<object>(obj => GetRunningInfo());
            ViewCommand = new ActionCommand<object>(obj => ViewRunningInfo());

            PageSizeList = MaintainCommon.PageSizeList; //获取翻页控件下拉列表
            PageSizeValue = PageSizeList[0];//获取默认每页显示记录的数据
            InitPagedServerCollection();
        }

        private int currentIndex = 1;
        private void InitPagedServerCollection()
        {
            workingSuiteClient.GetWorkingSuiteFuzzyCompleted += workingSuiteClient_GetWorkingSuiteFuzzyCompleted;
            PSC_WorkingSuite = new PagedServerCollection<WorkingSuite>((pageIndex, pageSize) =>
            {
                pageSize = PageSizeValue;
                System.Threading.Interlocked.Exchange(ref currentIndex, pageIndex);
                PagingInfo pagingInfo = new PagingInfo() { PageIndex = pageIndex, PageSize = pageSize };
                string suiteId = SuiteId == null ? string.Empty : SuiteId.Trim();
                string vehicleId = VehicleId == null ? string.Empty : VehicleId.Trim();
                workingSuiteClient.GetWorkingSuiteFuzzyAsync(vehicleId, suiteId, (int)Gsafety.PTMS.ServiceReference.SecuritySuiteService.DeviceSuiteStatus.Abnormal, pagingInfo);
            });
        }

        void workingSuiteClient_GetLastestSuiteRunningDetailCompleted(object sender, GetLastestSuiteRunningDetailCompletedEventArgs e)
        {
            if (e.Result != null && e.Result.Result != null)
            {
                SuiteRunningDetail CurrentSuiteRunningDetail = e.Result.Result;
                CurrentSuiteRunningDetail.SuiteId = CurrentWorkingSuite.SuiteId;
                CurrentSuiteRunningDetail.SuiteRunningBasicInfo.MdvrCoreId = CurrentWorkingSuite.MdvrCoreId;
                EventAggregator.Publish(new ViewNavigationArgs(MaintainName.SuiteRunningDisplayV, new Dictionary<string, object>() { { "action", "newRunningInfo" }, { "newRunningInfo", CurrentSuiteRunningDetail } }));
            }
            else
            {
                MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("MAINTAIN_DataEmpty"));
            }
        }
       
        void workingSuiteClient_GetWorkingSuiteFuzzyCompleted(object sender, GetWorkingSuiteFuzzyCompletedEventArgs e)
        {
            try
            {
                string symbol = ", ";
                string[] separator = { symbol };
                Gsafety.PTMS.ServiceReference.DeviceAlertService.DeviceAlertType value;
                e.Result.Result.ToList().ForEach(x =>
                    {
                        if (string.IsNullOrEmpty(x.AbnormalCause))
                        {
                            x.AbnormalCause = string.Empty;
                        }
                        else
                        {
                            string[] strArray = x.AbnormalCause.Split(separator, StringSplitOptions.None);
                            StringBuilder sb = new StringBuilder();
                            strArray.ToList().ForEach(y =>
                                {
                                    if (Enum.TryParse(y, true, out value))
                                    {
                                        sb.Append(ApplicationContext.Instance.StringResourceReader.GetString(value.ToString()));
                                        sb.Append(symbol);
                                    }
                                });
                            if (sb.ToString().Length > symbol.Length)
                            {
                                x.AbnormalCause = sb.ToString().Substring(0, sb.ToString().Length - symbol.Length);
                            }
                        }
                    });
                PSC_WorkingSuite.loader_Finished(new PagedResult<WorkingSuite>
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

        private void QueryAbnormalSuite()
        {
            currentIndex = 1;
            PSC_WorkingSuite.MoveToFirstPage();

        }

        private void GetRunningInfo()
        {
            MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("MAINTAIN_StartGetMaintainInfo"));
            ApplicationContext.Instance.MessageManager.SendGetSuiteRunintStatusCMD(
                new Gsafety.PTMS.ServiceReference.MessageService.SuiteRunintStatusCMD()
                {
                    DvId = CurrentWorkingSuite.MdvrCoreId,
                    VehicleId =CurrentWorkingSuite.VehicleId,
                    SendTime = DateTime.Now,
                    MsgId = "2",
                    SuiteRunintStatusID = Guid.NewGuid().ToString(),
                });

        }

        private void ViewRunningInfo()
        {
            workingSuiteClient.GetLastestSuiteRunningDetailAsync(CurrentWorkingSuite.MdvrCoreId, CurrentWorkingSuite.FaultTime);
        }

        private void Publish(string name)
        {
            EventAggregator.Publish(new ViewNavigationArgs(MaintainName.MaintenanceHandleDetailV, new Dictionary<string, object>() { { "action", name }, { name, CurrentWorkingSuite } }));
        }


        public void HandleEvent(BasicInfo publishedEvent)
        {
            //throw new NotImplementedException();

        }

        public void HandleEvent(Enviroment publishedEvent)
        {
            //throw new NotImplementedException();
        }

        public void HandleEvent(Hardware publishedEvent)
        {
            //throw new NotImplementedException();
        }
    }
}
