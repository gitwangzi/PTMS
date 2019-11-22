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
/////Class Name: VideoLogViewModel
///// Class Version: v1.0.0.0
///// Create Time: 2013/9/17 00:00:00
/////Class Description:  
/////======================================================================
/////Modified Time: 2013/9/17 00:00:00
/////Modified by:
/////Modified Description: 
/////======================================================================
using Gsafety.Common.Localization.Resource;
using Gsafety.PTMS.Bases.Enums;
using Gsafety.PTMS.ServiceReference.PTMSLogManageService;
using Gsafety.PTMS.Share;
using Jounce.Core.Command;
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
    [ExportAsViewModel(ManagerName.VideoLogViewModel)]
    public class VideoLogViewModel : BaseViewModel
    {
        private int currentIndex = 1;
        private VideoLogSerViceClient ptmsLogClient;
        private bool isFirst;

        #region DataProperties & CommandProperties
        private string type = "All";
        private string username;
        private DateTime starttime;
        private DateTime endtime;
        public bool ExportBtnStatus { get; set; }
        public IList<string> IvokeType { get; private set; } //Bingding view item
        public int SelectedIndex { get; set; }
        public string UserName { get; set; } //Bingding view item
        public DateTime StartTime { get; set; } //Bingding view item
        public DateTime EndTime { get; set; } //Bingding view item
        public VideoLogInfo CurrentLog { get; set; }
        public PagedServerCollection<VideoLogInfo> LogInfoPage { get; set; }
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
        /// wait staus
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

        #region Contrsuction & Initialization
        //Ctor
        public VideoLogViewModel()
        {
            try
            {
                isFirst = true;
                ptmsLogClient = ServiceClientFactory.Create<VideoLogSerViceClient>();
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
            ptmsLogClient.GetVideoLogCompleted += antLogClient_GetVideoLogCompleted;
            LogInfoPage = new PagedServerCollection<VideoLogInfo>(new Action<int, int>(InvokServer));
        }
        //Bingding view values
        private void UIInit()
        {
            StartTime = DateTime.Now.AddDays(-1);
            EndTime = DateTime.Now;
            IvokeType = new EnumAdapter<InvokeVideoType>().GetEnumInfos().Select(f => f.LocalizedString).ToList();
            string all = StringResource.ResourceManager.GetString("All");
            IvokeType.Insert(0, string.IsNullOrEmpty(all) ? "All" : all);
            SelectedIndex = 0;
        }
        #endregion

        #region Function & Events
        //Search
        private void Query()
        {
            try
            {
                username = string.IsNullOrEmpty(UserName) ? string.Empty : UserName.Trim();
                type = SelectedIndex == 0 ? "All" : new EnumAdapter<InvokeVideoType>().GetEnumInfos().Where(f => f.Index == SelectedIndex - 1).First().Name;
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
                ptmsLogClient.GetVideoLogAsync(username, type, starttime, endtime, pagingInfo);
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
                new LogHelper().RomoteError();
            }
        }
        //Response server event
        void antLogClient_GetVideoLogCompleted(object sender, GetVideoLogCompletedEventArgs e)
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
            catch(Exception ex)
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
                    VideoLogSerViceClient client = ServiceClientFactory.Create<VideoLogSerViceClient>();
                    client.GetVideoLogCompleted += (s, e) =>
                    {
                        if (e.Result != null && e.Result.TotalRecord > 0)
                        {
                            List<string> Codes = new List<string>();
                            Codes.Add("Player");
                            Codes.Add("ActionTime");
                            Codes.Add("Ivoke_Type");
                            Codes.Add("MDVRId");
                            Codes.Add("VehicleID");
                            Codes.Add("ChannelId");

                            List<string> Names = new List<string>();
                            Names.Add(ApplicationContext.Instance.StringResourceReader.GetString("MANAGER_Player"));
                            Names.Add(ApplicationContext.Instance.StringResourceReader.GetString("MANAGER_PlayTime"));
                            Names.Add(ApplicationContext.Instance.StringResourceReader.GetString("MANAGER_Ivoke_Type"));
                            Names.Add(ApplicationContext.Instance.StringResourceReader.GetString("MANAGER_MDVRID"));
                            Names.Add(ApplicationContext.Instance.StringResourceReader.GetString("MANAGER_VehicleID"));
                            Names.Add(ApplicationContext.Instance.StringResourceReader.GetString("MANAGER_ChannelId"));

                            List<Gsafety.PTMS.Spreadsheet.EnumsEx> eList = new List<Gsafety.PTMS.Spreadsheet.EnumsEx>();

                            List<FieldEx> FieldEx1 = new List<FieldEx>();
                            FieldEx1.Add(new FieldEx { Key = "0", Value = "1" });
                            FieldEx1.Add(new FieldEx { Key = "1", Value = "2" });
                            eList.Add(new Gsafety.PTMS.Spreadsheet.EnumsEx { Code = "ChannelId", Content = FieldEx1 });

                            List<FieldEx> FieldEx2 = new List<FieldEx>();
                            Gsafety.PTMS.Bases.Enums.InvokeVideoTypeConverter ivtc = new Gsafety.PTMS.Bases.Enums.InvokeVideoTypeConverter();
                            ivtc.EnumInfo.ToList().ForEach(x =>
                                {
                                    FieldEx item = new FieldEx { Key = x.Name, Value = x.LocalizedString };
                                    FieldEx2.Add(item);
                                });
                            eList.Add(new Gsafety.PTMS.Spreadsheet.EnumsEx { Code = "Ivoke_Type", Content = FieldEx2 });

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
                        client.GetVideoLogAsync(username, type, starttime, endtime, pagingInfo);
                    }
                    else
                    {
                        PagingInfo pagingInfo = new PagingInfo() { PageIndex = -1, PageSize = 0 };
                        client.GetVideoLogAsync(username, type, starttime, endtime, pagingInfo);
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
