/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////Guid: 0a68efed-aa84-4309-93e6-0d7cd1749664      
///// clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
///// Machine Name: PC-SHIHS
///// Author: (ShiHongsheng)
/////======================================================================
///// Project Name: Gsafety.PTMS.Manager.ViewModels.LogManageViewModel
///// Project Description:    
/////Class Name: AlarmDealLogViewModel
///// Class Version: v1.0.0.0
///// Create Time: 2013/9/16 00:00:00
/////Class Description:  
/////======================================================================
/////Modified Time: 2013/9/16 00:00:00
/////Modified by:
/////Modified Description: 
/////======================================================================
using Gsafety.PTMS.ServiceReference.PTMSLogManageService;
using Gsafety.PTMS.Share;
using Gsafety.Common.Localization.Resource;
using Jounce.Core.Command;
using Jounce.Core.ViewModel;
using Jounce.Framework.Command;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using Gsafety.PTMS.Spreadsheet;
using Gsafety.PTMS.Spreadsheet.Parts;
using FiftyNine.Ag.OpenXML.Common.Storage;
using System.Linq;
using System.Reflection;
using BaseLib.ViewModels;
using BaseLib.Model;
namespace Gsafety.PTMS.Manager.ViewModels.LogManageViewModel
{
    [ExportAsViewModel(ManagerName.AlarmDealLogViewModel)]
    public class AlarmDealLogViewModel : BaseViewModel
    {
        private int currentIndex = 1;
        private AlarmDealLogServiceClient dealLogClient;

        # region    DataProperties & CommandProperties
        private string username;
        private DateTime starttime;
        private DateTime endtime;
        public bool ExportBtnStatus { get; set; }
        public string UserName { get; set; }  //Bingding view item
        public DateTime StartTime { get; set; } //Bingding view item
        public DateTime EndTime { get; set; } //Bingding view item
        public AlarmDealLogInfo CurrentLog { get; set; }
        public PagedServerCollection<AlarmDealLogInfo> LogInfoPage { get; set; }
        public List<int> PageSizeList { get; set; }
        public IActionCommand QueryCommand { get; private set; }
        public IActionCommand ExportCommand { get; private set; }
        private int pageSizeValue;
        public int PageSizeValue
        {
            get { return pageSizeValue; }
            set
            {
                pageSizeValue = value;
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(new string[] { "LogInfoPage", "PageSizeValue" }));
            }
        }
        /// <summary>
        /// wait status
        /// </summary>
        private bool _IsBusy = false;
        public bool IsBusy
        {
            get { return _IsBusy; }
            set
            {
                _IsBusy = value;
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => IsBusy));
            }
        }
        #endregion

        #region  Construction & Initialization
        //Cotr
        public AlarmDealLogViewModel()
        {
            try
            {
                dealLogClient = ServiceClientFactory.Create<AlarmDealLogServiceClient>();
                ExportBtnStatus = true;
                QueryCommand = new ActionCommand<object>(obj => Query());
                ExportCommand = new ActionCommand<object>(obj => Export());
                PageSizeValue = 20;
                PageSizeList = new List<int> { 20, 40, 80 };
                UIInit();
                starttime = StartTime;
                endtime = EndTime;
                InitPagedServerCollection();
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }

        //Init
        private void InitPagedServerCollection()
        {
            try
            {
                dealLogClient.GetAlarmDealLogCompleted += dealLogClient_GetAlarmDealLogCompleted;
                LogInfoPage = new PagedServerCollection<AlarmDealLogInfo>(new Action<int, int>(InvokServer));
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }
        //Bingding view  values
        private void UIInit()
        {
            StartTime = DateTime.Now.AddMonths(-1);
            EndTime = DateTime.Now;
        }
        #endregion

        #region  Fuctions & Events
        // Search
        private void Query()
        {
            try
            {
                username = string.IsNullOrEmpty(UserName) ? string.Empty : UserName.Trim();
                starttime = StartTime;
                endtime = EndTime;
                if (new LogHelper().SeearchConditionValid(StartTime, EndTime))
                {
                    currentIndex = 1;
                    LogInfoPage.MoveToFirstPage();
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }
        //Invoke Server
        private void InvokServer(int pageIndex, int pageSize)
        {
            IsBusy = true;
            pageSize = PageSizeValue;
            System.Threading.Interlocked.Exchange(ref currentIndex, pageIndex);
            string districtCode = string.Empty;
            PagingInfo pagingInfo = new PagingInfo() { PageIndex = pageIndex, PageSize = pageSize };
            try
            {
                //dealLogClient.GetAlarmDealLogAsync(username, starttime, endtime, pagingInfo);
            }
            catch (Exception ex)
            {
                new LogHelper().RomoteError();
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }
        //Respose Server event
        void dealLogClient_GetAlarmDealLogCompleted(object sender, GetAlarmDealLogCompletedEventArgs e)
        {
            try
            {
                LogInfoPage.loader_Finished(new PagedResult<AlarmDealLogInfo>
                {
                    Count = e.Result.TotalRecord,
                    Items = e.Result.Result,
                    PageIndex = currentIndex,
                });
                if (e.Result.TotalRecord == 0)
                {
                    setExportBtnStatus(false);
                    //MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("MAINTAIN_DataEmpty"), ApplicationContext.Instance.StringResourceReader.GetString("TRAFFIC_TIP"), MessageBoxButton.OK);
                }
                else
                {
                    setExportBtnStatus(true);
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
                //MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("BASEINFO_Operate_Failed"));
            }
            IsBusy = false;
        }
        // RefreshPage View
        protected override void ActivateView(string viewName, System.Collections.Generic.IDictionary<string, object> viewParameters)
        {
            try
            {
                base.ActivateView(viewName, viewParameters);
                if (viewParameters.Count != 0 && viewParameters["action"].ToString() == "return")
                {
                    LogInfoPage.ToPage(currentIndex);
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }

        private void setExportBtnStatus(bool Flag)
        {
            ExportBtnStatus = Flag;
            Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => ExportBtnStatus));
        }

        private void Export()
        {
            try
            {
                SaveFileDialog dlg = new SaveFileDialog();
                dlg.Filter = "Excel Document (.xlsx)|*.xlsx";
                dlg.DefaultExt = ".xlsx";
                bool? dialogResult = dlg.ShowDialog();
                if (dialogResult == true)
                {
                    setExportBtnStatus(false);
                    AlarmDealLogServiceClient client = ServiceClientFactory.Create<AlarmDealLogServiceClient>();
                    client.GetAlarmDealLogCompleted += (s, e) =>
                    {
                        if (e.Result != null && e.Result.TotalRecord > 0)
                        {
                            List<string> Codes = new List<string>();
                            Codes.Add("AlarmVihcleID");
                            Codes.Add("AlarmTime");
                            Codes.Add("DealPerson");
                            Codes.Add("DealTime");

                            List<string> Names = new List<string>();
                            Names.Add(ApplicationContext.Instance.StringResourceReader.GetString("MANAGER_AlarmVihcleID"));
                            Names.Add(ApplicationContext.Instance.StringResourceReader.GetString("MANAGER_AlarmTime"));
                            Names.Add(ApplicationContext.Instance.StringResourceReader.GetString("MANAGER_Staff"));
                            Names.Add(ApplicationContext.Instance.StringResourceReader.GetString("MANAGER_DealTime"));

                            XLSXExporter xlsx = new XLSXExporter();
                            xlsx.Export(e.Result.Result.ToList(), dlg.OpenFile(), Codes, Names);
                            setExportBtnStatus(true);
                            MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("Manager_LogExportSucceed"), ApplicationContext.Instance.StringResourceReader.GetString("TRAFFIC_TIP"), MessageBoxButton.OK);
                        }
                        else
                        {
                            MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("Manager_LogExportFailed"), ApplicationContext.Instance.StringResourceReader.GetString("TRAFFIC_TIP"), MessageBoxButton.OK);
                        }
                    };

                    if (LogInfoPage.TotalItemCount > 10000)
                    {
                        PagingInfo pagingInfo = new PagingInfo() { PageIndex = 1, PageSize = 10000 };
                        //client.GetAlarmDealLogAsync(username, starttime, endtime, pagingInfo);
                    }
                    else
                    {
                        PagingInfo pagingInfo = new PagingInfo() { PageIndex = -1, PageSize = 0 };
                        //client.GetAlarmDealLogAsync(username, starttime, endtime, pagingInfo);
                    }
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);

            }
        }
        #endregion
    }
}
