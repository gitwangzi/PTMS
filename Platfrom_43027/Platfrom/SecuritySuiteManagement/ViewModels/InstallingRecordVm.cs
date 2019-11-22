/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 9f36785b-dcbd-4453-b64d-c63f5ba35f57      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-DENGZL
/////                 Author: TEST(dengzl)
/////======================================================================
/////           Project Name: Gsafety.PTMS.SecuritySuite.ViewModels
/////    Project Description:    
/////             Class Name: InstallRecordVm
/////          Class Version: v1.0.0.0
/////            Create Time: 8/7/2013 12:08:57 PM
/////      Class Description:  
/////======================================================================
/////          Modified Time: 8/7/2013 12:08:57 PM
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
using Jounce.Core.Command;
using Gsafety.PTMS.SecuritySuite.Views;
using Jounce.Framework.Command;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Gsafety.PTMS.ServiceReference.DeviceInstallService;
using Gsafety.PTMS.Share;
using System.Windows.Data;
using Gsafety.PTMS.ServiceReference.InstallStationService;
using Gsafety.PTMS.BaseInformation;
using Gsafety.PTMS.SecuritySuite;
using System.Reflection;
using BaseLib.ViewModels;
using BaseLib.Model;

namespace Gsafety.PTMS.SecuritySuite.ViewModels
{
    [ExportAsViewModel(SecuritySuiteName.InstallingRecordVm)]
    public class InstallingRecordVm : BaseViewModel
    {
        private DeviceInstallServiceClient client;
        #region properties and command
        public string VehicleID { get; set; }
        public string SuiteID { get; set; }
        public string SetupStation { get; set; }
        public Nullable<DateTime> StartDate { get; set; }
        public Nullable<DateTime> EndDate { get; set; }
        public IEnumerable<InstallStation> SetupStationList { get; set; }
        public IActionCommand QueryCommand { get; set; }

        private int currentIndex = 1;
        public List<int> PageSizeList { get; set; }
        public PagedServerCollection<InstallationInfo> InstallingRecordsPageView { get; set; }    //paging data



        private int pageSizeValue;
        public int PageSizeValue
        {
            get { return pageSizeValue; }
            set
            {
                pageSizeValue = value;
                if (this.InstallingRecordsPageView != null)
                    this.InstallingRecordsPageView.PageSize = value;
            }
        }
        #endregion
        public InstallingRecordVm()
        {
            try
            {
                client = ServiceClientFactory.Create<DeviceInstallServiceClient>();
                client.GetInstallationInProgressFuzzyCompleted += client_GetInstallationInProgressFuzzyCompleted;
                QueryCommand = new ActionCommand<object>(obj => QueryAction());

                StartDate = DateTime.Now.Date.AddMonths(-1);
                EndDate = DateTime.Now.Date;

                getSetupStationList();

                PageSizeList = Gsafety.PTMS.BaseInformation.BaseInformationCommon.PageSizeList;
                PageSizeValue = PageSizeList[0];

                InitPagedServerCollection();
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("InstallingRecordVm()", ex);
            }
        }

        private void InitPagedServerCollection()
        {
            try
            {
                InstallingRecordsPageView = new PagedServerCollection<InstallationInfo>((pageIndex, pageSize) =>
                {
                    pageSize = PageSizeValue;
                    System.Threading.Interlocked.Exchange(ref currentIndex, pageIndex);

                    //Invoke Server
                    ////Optimize WCF service  
                    if (!string.IsNullOrEmpty(VehicleID))
                        VehicleID = VehicleID.Trim();
                    if (!string.IsNullOrEmpty(SuiteID))
                        SuiteID = SuiteID.Trim();
                    client.GetInstallationInProgressFuzzyAsync(VehicleID, SuiteID, SetupStation, StartDate, EndDate,
                    new Gsafety.PTMS.ServiceReference.DeviceInstallService.PagingInfo { PageIndex = pageIndex, PageSize = pageSize });
                });
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("InitPagedServerCollection()", ex);
            }
        }

        void client_GetInstallationInProgressFuzzyCompleted(object sender, GetInstallationInProgressFuzzyCompletedEventArgs e)
        {
            try
            {
                InstallingRecordsPageView.loader_Finished(new PagedResult<InstallationInfo>
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
            catch(Exception ex)
            {
                //MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("BASEINFO_Operate_Failed"));
                ApplicationContext.Instance.Logger.LogException("client_GetInstallationInProgressFuzzyCompleted()", ex);
            }
        }

        private void QueryAction()
        {
            try
            {
                currentIndex = 1;
                InstallingRecordsPageView.MoveToFirstPage();
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("QueryAction()", ex);
            }
        }
        private void getSetupStationList()
        {
            try
            {
                InstallStationServiceClient stationClient = ServiceClientFactory.Create<InstallStationServiceClient>();
                stationClient.GetInstallStationsByAlphabetCompleted += stationClient_GetInstallStationsByAlphabetCompleted;
                stationClient.GetInstallStationsByAlphabetAsync(new Gsafety.PTMS.ServiceReference.InstallStationService.PagingInfo { PageIndex = 1, PageSize = 999 }, ApplicationContext.Instance.AuthenticationInfo.ClientID);
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("getSetupStationList()", ex);
            }
        }

        void stationClient_GetInstallStationsByAlphabetCompleted(object sender, GetInstallStationsByAlphabetCompletedEventArgs e)
        {
            try
            {
                List<InstallStation> list = new List<InstallStation>();
                list.Add(new InstallStation() { Name = ApplicationContext.Instance.StringResourceReader.GetString("All"), ID = string.Empty });
                list.AddRange(e.Result.Result);
                SetupStationList = list;
                SetupStation = string.Empty;
                RaisePropertyChanged(() => this.SetupStationList);  //Refresh UI 
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("stationClient_GetInstallStationsByAlphabetCompleted()", ex);
            }
        }
    }
}
