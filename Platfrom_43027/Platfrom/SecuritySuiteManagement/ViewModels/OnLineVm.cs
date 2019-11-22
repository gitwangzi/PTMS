/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: b2e33ce3-08a7-4ae8-9b00-137af7a7f7de      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-DENGZL
/////                 Author: TEST(dengzl)
/////======================================================================
/////           Project Name: Gsafety.PTMS.SecuritySuite.ViewModels
/////    Project Description:    
/////             Class Name: OnLineVm
/////          Class Version: v1.0.0.0
/////            Create Time: 8/8/2013 5:47:15 PM
/////      Class Description:  
/////======================================================================
/////          Modified Time: 8/8/2013 5:47:15 PM
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using Jounce.Core.ViewModel;
using Jounce.Core.Command;
using Jounce.Framework.Command;
using System.Collections.Generic;
using Gsafety.PTMS.ServiceReference.VehicleStatusService;
using Gsafety.PTMS.Share;
using Gsafety.PTMS.BaseInformation;
using Jounce.Core.View;
using System;
using System.Windows;
using System.Reflection;
using BaseLib.ViewModels;
using BaseLib.Model;

namespace Gsafety.PTMS.SecuritySuite.ViewModels
{
    [ExportAsViewModel(SecuritySuiteName.OnLineVm)]
    public class OnLineVm : BaseViewModel
    {
        private int currentIndex = 1;
        VehicleStatusServiceClient client ;
        #region properties and commands
        public string VehicleID { get; set; }
        public List<int> PageSizeList { get; set; }
        public SuiteStatus CurrentSuiteStatus { get; set; }
        public PagedServerCollection<SuiteStatus> SuiteStatusInfoPage { get; set; }
        public  string  timespan { set; get; }
        private bool isFirst;
        private int pageSizeValue;
        public int PageSizeValue
        {
            get { return pageSizeValue; }
            set 
            { 
                pageSizeValue = value;
                if (this.SuiteStatusInfoPage != null)
                {
                    this.SuiteStatusInfoPage.PageSize = value;
                }
            }
        }

        public IActionCommand QueryCommand { get; private set; }
        #endregion

        //Ctor & Initialization
        public OnLineVm()
        {
            try
            {
                isFirst = true;
                client = ServiceClientFactory.Create<VehicleStatusServiceClient>();
                client.GetVehicleTimeSpanFuzzyCompleted += client_GetVehicleTimeSpanFuzzyCompleted;
                QueryCommand = new ActionCommand<object>(obj => Query());
                PageSizeList = Gsafety.PTMS.BaseInformation.BaseInformationCommon.PageSizeList;
                PageSizeValue = PageSizeList[0];
                InitPagedServerCollection();              
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("OnLineVm()", ex);
            }
        }

        private void InitPagedServerCollection()
        {
            SuiteStatusInfoPage = new PagedServerCollection<SuiteStatus>(new Action<int, int>(InvokServer));
        }

        //Invoke Server
        private void InvokServer(int pageIndex, int pageSize)
        {
            try
            {
                pageSize = PageSizeValue;
                System.Threading.Interlocked.Exchange(ref currentIndex, pageIndex);
                string suiteId = string.Empty;
                PagingInfo pagingInfo = new PagingInfo() { PageIndex = pageIndex, PageSize = pageSize };
                client.GetVehicleTimeSpanFuzzyAsync(VehicleID, suiteId, 1, timespan, pagingInfo);  // 0 is offline,1 is online
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("InvokServer()", ex);
            }
        }

        //Response server
        void client_GetVehicleTimeSpanFuzzyCompleted(object sender, GetVehicleTimeSpanFuzzyCompletedEventArgs e)
        {
            try
            {
                if (e.Error != null)
                {
                    ApplicationContext.Instance.Logger.LogException(GetType().FullName, e.Error);
                    return;
                }
                SuiteStatusInfoPage.loader_Finished(new PagedResult<SuiteStatus>
                {
                    Count = e.Result.TotalRecord,
                    Items = e.Result.Result,
                    PageIndex = currentIndex,
                });
                if (e.Result.TotalRecord == 0&&!isFirst)
                {
                    //MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("MAINTAIN_DataEmpty"), ApplicationContext.Instance.StringResourceReader.GetString("TRAFFIC_TIP"), MessageBoxButton.OK);
                }
                isFirst = false;
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("client_GetVehicleTimeSpanFuzzyCompleted()", ex);
            }
        }
        protected override void ActivateView(string viewName, System.Collections.Generic.IDictionary<string, object> viewParameters)
        {

        }
        private void Query()
        {
            try
            {
                SuiteStatusInfoPage.MoveToFirstPage();
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("OnLineVm Query", ex);
            }
        }
    }
}