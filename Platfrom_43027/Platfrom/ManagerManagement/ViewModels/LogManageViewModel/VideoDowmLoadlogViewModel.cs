/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////Guid: 
///// clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
///// Machine Name: PC-SHIHS
///// Author: (ShiHongsheng)
/////======================================================================
///// Project Name: Gsafety.PTMS.Manager.ViewModels.LogManageViewModel
///// Project Description:    
/////Class Name: VideoDowmLoadlogViewModel
///// Class Version: v1.0.0.0
///// Create Time: 2013/9/17 00:00:00
/////Class Description:  
/////======================================================================
/////Modified Time: 2013/9/17 00:00:00
/////Modified by:
/////Modified Description: 
/////======================================================================
using Gsafety.PTMS.ServiceReference.PTMSLogManageService;
using Gsafety.PTMS.Share;
using Jounce.Core.Command;
using Jounce.Core.View;
using Jounce.Core.ViewModel;
using Jounce.Framework.Command;
using Gsafety.PTMS.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Reflection;
using BaseLib.ViewModels;
using BaseLib.Model;
namespace Gsafety.PTMS.Manager.ViewModels.LogManageViewModel
{
    [ExportAsViewModel(ManagerName.VideoDowmLoadlogViewModel)]
    public class VideoDowmLoadlogViewModel : BaseViewModel
    {
        private int currentIndex = 1;
        private VideoDowmloadLogServiceClient antLogClient;
        private bool isFirst;

        #region DataProperties & CommandProperties
        private string username;
        private DateTime starttime;
        private DateTime endtime;
        public bool ExportBtnStatus { get; set; }
        public string UserName { get; set; } //Bingding view item
        public DateTime StartTime { get; set; } //Bingding view item
        public DateTime EndTime { get; set; } //Bingding view item
        public VideoLogInfo CurrentLog { get; set; }
        public PagedServerCollection<VideoLogInfo> LogInfoPage { get; set; }
        public List<int> PageSizeList { get; set; }
        public IActionCommand QueryCommand { get; private set; }
        public IActionCommand ExportCommand { get; private set; }
        public IActionCommand GoDetail { get; set; }
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

        #region Construstion & Initialization
        //Ctor
        public VideoDowmLoadlogViewModel()
        {
            try
            {
                isFirst = true;
                antLogClient = ServiceClientFactory.Create<VideoDowmloadLogServiceClient>();
                QueryCommand = new ActionCommand<object>(obj => Query());
                ExportCommand = new ActionCommand<object>(obj => Export());
                GoDetail = new ActionCommand<object>(o => Detail());
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
            antLogClient.GetVideoDownloadLogCompleted += antLogClient_GetVideoDownloadLogCompleted;
            LogInfoPage = new PagedServerCollection<VideoLogInfo>(new Action<int, int>(InvokServer));
        }
        //Binding view values
        private void UIInit()
        {
            StartTime = DateTime.Now.AddDays(-1);
            EndTime = DateTime.Now;
        }
        #endregion

        #region Functions & Events
        //Search
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
            try
            {
                IsBusy = true;
                pageSize = PageSizeValue;
                System.Threading.Interlocked.Exchange(ref currentIndex, pageIndex);
                string districtCode = string.Empty;
                PagingInfo pagingInfo = new PagingInfo() { PageIndex = pageIndex, PageSize = pageSize };
                try
                {
                    antLogClient.GetVideoDownloadLogAsync(username, "All", StartTime, EndTime, pagingInfo);
                }
                catch
                {
                    new LogHelper().RomoteError();
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }
        //Response server event
        void antLogClient_GetVideoDownloadLogCompleted(object sender, GetVideoDownloadLogCompletedEventArgs e)
        {
            try
            {
                LogInfoPage.loader_Finished(new PagedResult<VideoLogInfo>
                {
                    Count = e.Result.TotalRecord,
                    Items = e.Result.Result,
                    PageIndex = currentIndex,
                });
                if (e.Result.TotalRecord == 0&&!isFirst)
                {
                    setExportBtnStatus(false);
                    //MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("MAINTAIN_DataEmpty"), ApplicationContext.Instance.StringResourceReader.GetString("TRAFFIC_TIP"), MessageBoxButton.OK);
                }
                else
                {
                    setExportBtnStatus(true);
                }
                isFirst = false;
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
                //MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("BASEINFO_Operate_Failed"));
            }
            IsBusy = false;
        }
        //RefreshPage
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

        private void Detail()
        {
            try
            {
                if (CurrentLog != null)
                {
                    var vn = new ViewNavigationArgs("VideoDetail", new Dictionary<string, object>() { 
                { "Player", CurrentLog.Player}
                ,{"ActionTime",CurrentLog.ActionTime}
                ,{"MDVRId",CurrentLog.MDVRId}
                ,{"SubType",CurrentLog.SubType}
                ,{"VehicleID",CurrentLog.VehicleID}
                ,{"ChannelId",CurrentLog.ChannelId}
                ,{"VideoStartTime",CurrentLog.VideoStartTime}
                ,{"VideoEndtime",CurrentLog.VideoEndtime}
                ,{"VideoFileName",CurrentLog.VideoFileName}
                });
                    EventAggregator.Publish(vn);
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
                    VideoDowmloadLogServiceClient client = ServiceClientFactory.Create<VideoDowmloadLogServiceClient>();
                    client.GetVideoDownloadLogCompleted += (s, e) =>
                    {
                        if (e.Result != null && e.Result.TotalRecord > 0)
                        {
                            List<string> Codes = new List<string>();
                            Codes.Add("Player");
                            Codes.Add("ActionTime");
                            Codes.Add("MDVRId");
                            Codes.Add("VehicleID");
                            Codes.Add("ChannelId");

                            List<string> Names = new List<string>();
                            Names.Add(ApplicationContext.Instance.StringResourceReader.GetString("MANAGER_Downloader"));
                            Names.Add(ApplicationContext.Instance.StringResourceReader.GetString("MANAGER_DownLoadTime"));
                            Names.Add(ApplicationContext.Instance.StringResourceReader.GetString("MANAGER_MDVRID"));
                            Names.Add(ApplicationContext.Instance.StringResourceReader.GetString("MANAGER_VehicleID"));
                            Names.Add(ApplicationContext.Instance.StringResourceReader.GetString("MANAGER_ChannelId"));

                            List<Gsafety.PTMS.Spreadsheet.EnumsEx> eList = new List<Gsafety.PTMS.Spreadsheet.EnumsEx>();

                            List<FieldEx> FieldEx1 = new List<FieldEx>();
                            FieldEx1.Add(new FieldEx { Key = "0", Value = "1" });
                            FieldEx1.Add(new FieldEx { Key = "1", Value = "2" });

                            eList.Add(new Gsafety.PTMS.Spreadsheet.EnumsEx { Code = "ChannelId", Content = FieldEx1 });

                            XLSXExporter xlsx = new XLSXExporter();
                            xlsx.Export(e.Result.Result.ToList(), dlg.OpenFile(), Codes, Names, eList);
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
                        client.GetVideoDownloadLogAsync(username, "All", starttime, endtime, pagingInfo);
                    }
                    else
                    {
                        PagingInfo pagingInfo = new PagingInfo() { PageIndex = -1, PageSize = 0 };
                        client.GetVideoDownloadLogAsync(username, "All", starttime, endtime, pagingInfo);
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
