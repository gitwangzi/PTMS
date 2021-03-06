﻿/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: ede3752a-e673-4d85-bb3a-fa9a23b5163b      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-DENGZL
/////                 Author: TEST(dengzl)
/////======================================================================
/////           Project Name: Gsafety.PTMS.SecuritySuite.ViewModels
/////    Project Description:    
/////             Class Name: InstallHistoryVm
/////          Class Version: v1.0.0.0
/////            Create Time: 8/7/2013 12:08:31 PM
/////      Class Description:  
/////======================================================================
/////          Modified Time: 8/7/2013 12:08:31 PM
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System;
using Jounce.Core.ViewModel;
using Jounce.Core.Command;
using Jounce.Framework.Command;
using System.Collections.Generic;
using Gsafety.PTMS.ServiceReference.DeviceInstallService;
using Gsafety.PTMS.Share;
using System.Windows.Data;
using System.Collections.ObjectModel;
using Gsafety.PTMS.ServiceReference.InstallStationService;
using Gsafety.PTMS.BaseInformation;
using System.Windows;
using Gsafety.PTMS.SecuritySuite;
using System.Reflection;
using BaseLib.ViewModels;
using BaseLib.Model;
using Gsafety.Common.Controls;


namespace Gsafety.PTMS.SecuritySuite.ViewModels
{
    [ExportAsViewModel(SecuritySuiteName.InstallFinishVm)]
    public class InstallFinishVm : BaseViewModel
    {
        #region
        public string VehicleID { get; set; }
        public string SuiteID { get; set; }
        public string SetupStation { get; set; }
        public Nullable<DateTime> StartDate { get; set; }
        public Nullable<DateTime> EndDate { get; set; }

        public IEnumerable<InstallStation> SetupStationList { get; set; }

        public PagedServerCollection<InstallationInfo> InstalledRecordsPageView { get; set; }    //paging data

        public IActionCommand QueryCommand { get; set; }

        private DeviceInstallServiceClient client;

        private int currentIndex = 1;
        public List<int> PageSizeList { get; set; }

        private int pageSizeValue;
        public int PageSizeValue
        {
            get { return pageSizeValue; }
            set
            { 
                pageSizeValue = value;
                if (this.InstalledRecordsPageView != null)
                {
                    this.InstalledRecordsPageView.PageSize = value;
                }
            }
        }
        #endregion
        public InstallFinishVm()
        {
            try
            {
                client = ServiceClientFactory.Create<DeviceInstallServiceClient>();
                client.GetInstallationFinishedFuzzyCompleted += client_GetInstallationFinishedFuzzyCompleted;
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
                ApplicationContext.Instance.Logger.LogException("InstallFinishVm()", ex);
            }
        }

        private void InitPagedServerCollection()
        {
            try
            {
                InstalledRecordsPageView = new PagedServerCollection<InstallationInfo>((pageIndex, pageSize) =>
                {
                    pageSize = PageSizeValue;
                    System.Threading.Interlocked.Exchange(ref currentIndex, pageIndex);

                    client.GetInstallationFinishedFuzzyAsync(VehicleID, SuiteID, SetupStation, StartDate, EndDate,
                        new Gsafety.PTMS.ServiceReference.DeviceInstallService.PagingInfo { PageIndex = pageIndex, PageSize = pageSize });
                });
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("InitPagedServerCollection()", ex);
            }
        }

        void client_GetInstallationFinishedFuzzyCompleted(object sender, GetInstallationFinishedFuzzyCompletedEventArgs e)
        {
            try
            {
                InstalledRecordsPageView.loader_Finished(new PagedResult<InstallationInfo>
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
                MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString("BASEINFO_Operate_Failed"));
                ApplicationContext.Instance.Logger.LogException("client_GetInstallationFinishedFuzzyCompleted", ex);
            }
        }


        private void QueryAction()
        {
            try
            {
                currentIndex = 1;
                InstalledRecordsPageView.MoveToFirstPage();
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
                RaisePropertyChanged(() => this.SetupStationList);
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("stationClient_GetInstallStationsByAlphabetCompleted", ex);
            }
        }
    }
}
