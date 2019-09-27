/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 9c31728d-f994-4592-9d42-9c84afe6456e      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-DENGZL
/////                 Author: TEST(dengzl)
/////======================================================================
/////           Project Name: Gsafety.PTMS.SecuritySuite.ViewModels
/////    Project Description:    
/////             Class Name: OffLineVm
/////          Class Version: v1.0.0.0
/////            Create Time: 8/8/2013 5:47:26 PM
/////      Class Description:  
/////======================================================================
/////          Modified Time: 8/8/2013 5:47:26 PM
/////            Modified by:
/////   Modified Description: 
/////======================================================================
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
using Gsafety.PTMS.ServiceReference.VehicleStatusService;
using Gsafety.PTMS.Share;
using System.Collections.ObjectModel;
using Jounce.Framework.Command;
using System.Windows.Data;
using System.Collections.Generic;
using Gsafety.PTMS.BaseInformation;
using Jounce.Core.Command;
using Jounce.Core.View;
using System.Reflection;
using BaseLib.ViewModels;
using BaseLib.Model;

namespace Gsafety.PTMS.SecuritySuite.ViewModels
{
    [ExportAsViewModel(SecuritySuiteName.OffLineVm)]
    public class OffLineVm : BaseViewModel
    {
        private int currentIndex = 1;
        VehicleStatusServiceClient client;

        #region properties and command
        public string VehicleID { get; set; }
        public List<int> PageSizeList { get; set; }
        public string timespan { get; set; }
        public SuiteStatus CurrentSuiteStatus { get; set; }
        public PagedServerCollection<SuiteStatus> SuiteStatusInfoPage { get; set; }
        private int pageSizeValue;
        public int PageSizeValue
        {
            get { return pageSizeValue; }
            set
            {
                pageSizeValue = value; 
                if (SuiteStatusInfoPage != null)
                {
                    SuiteStatusInfoPage.PageSize = value;
                }
            }
        }

        public IActionCommand QueryCommand { get; private set; }
        #endregion

        //Ctor & Initialization
        public OffLineVm()
        {
            try
            {
                client = ServiceClientFactory.Create<VehicleStatusServiceClient>();
                client.GetVehicleTimeSpanFuzzyCompleted += client_GetVehicleTimeSpanFuzzyCompleted;
                QueryCommand = new ActionCommand<object>(obj => Query());
                PageSizeList = Gsafety.PTMS.BaseInformation.BaseInformationCommon.PageSizeList;
                PageSizeValue = PageSizeList[0];
                InitPagedServerCollection();
                
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("OffLineVm()", ex);
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
                client.GetVehicleTimeSpanFuzzyAsync(VehicleID, suiteId, 0, timespan, pagingInfo);  //0 is offline 1, is online
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("OffLineVm InvokServer", ex);
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
                if (e.Result.TotalRecord == 0)
                {
                    //MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("MAINTAIN_DataEmpty"), ApplicationContext.Instance.StringResourceReader.GetString("TRAFFIC_TIP"), MessageBoxButton.OK);
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("client_GetVehicleTimeSpanFuzzyCompleted()", ex);
            }
        }

        private void Query()
        {
            try
            {
                SuiteStatusInfoPage.MoveToFirstPage();
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("OffLineVm Query", ex);
            }
        }
    }
}
