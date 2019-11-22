using Gsafety.PTMS.ServiceReference.PTMSLogManageService;
using Gsafety.PTMS.Share;
using Jounce.Core.Command;
using Jounce.Core.ViewModel;
using Jounce.Framework.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Spreadsheet = Gsafety.PTMS.Spreadsheet;
using System.Reflection;
using BaseLib.ViewModels;
using BaseLib.Model;
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
/////Class Name: LoginLogViewModel
///// Class Version: v1.0.0.0
///// Create Time: 2013/9/17 00:00:00
/////Class Description:  
/////======================================================================
/////Modified Time: 2013/9/17 00:00:00
/////Modified by:
/////Modified Description: 
/////======================================================================

namespace Gsafety.PTMS.Manager.ViewModels.LogManageViewModel
{
    [ExportAsViewModel(ManagerName.LoginLogViewModel)]
    public class LoginLogViewModel : BaseViewModel
    {
        private int currentIndex = 1;
        private LoginLogServiceClient ptmsLogClient;

        #region DateProperties & CommandProperties
        private string username;
        private DateTime starttime;
        private DateTime endtime;
        public bool ExportBtnStatus { get; set; }
        public string UserName { get; set; }     //Bingding view item
        public DateTime StartTime { get; set; }     //Bingding view item
        public DateTime EndTime { get; set; }     //Bingding view item
        //public LoginLogInfo CurrentLog { get; set; }
        //public PagedServerCollection<LoginLogInfo> LogInfoPage { get; set; }
        public List<int> PageSizeList { get; set; }
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
        public IActionCommand QueryCommand { get; private set; }
        public IActionCommand ExportCommand { get; private set; }

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

        #region Construction & Initialization
        //Ctor
        public LoginLogViewModel()
        {
            try
            {
                ptmsLogClient = ServiceClientFactory.Create<LoginLogServiceClient>();
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
            ptmsLogClient.GetLoginLogCompleted += antLogClient_GetLoginLogCompleted;
            LogInfoPage = new PagedServerCollection<LoginLogInfo>(new Action<int, int>(InvokServer));
        }
        //Bingding view values
        private void UIInit()
        {
            StartTime = DateTime.Today.AddMonths(-1);
            EndTime = DateTime.Today;
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
        // Invoke Server
        private void InvokServer(int pageIndex, int pageSize)
        {
            IsBusy = true;
            pageSize = pageSizeValue;
            System.Threading.Interlocked.Exchange(ref currentIndex, pageIndex);
            string districtCode = string.Empty;
            PagingInfo pagingInfo = new PagingInfo() { PageIndex = pageIndex, PageSize = pageSize };
            try
            {
                ptmsLogClient.GetLoginLogAsync(username, starttime, endtime.AddDays(1), pagingInfo);
            }
            catch
            {
                new LogHelper().RomoteError();
            }
        }
        // Respose server event
        void antLogClient_GetLoginLogCompleted(object sender, GetLoginLogCompletedEventArgs e)
        {
            try
            {
                var result = e.Result.Result;
                foreach (var item in result)
                {
                    if (item.LogoutTime != null)
                    {
                        if (DateTime.Now.Subtract(item.LogoutTime.Value).TotalMinutes < ApplicationContext.Instance.ServerConfig.LogUpdateInterval)
                        {
                            item.LogoutTime = null;
                        }
                    }
                }

                LogInfoPage.loader_Finished(new PagedResult<LoginLogInfo>
                {
                    Count = e.Result.TotalRecord,
                    Items = result,
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
        //RefreshPage view
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
                    LoginLogServiceClient client = ServiceClientFactory.Create<LoginLogServiceClient>();
                    client.GetLoginLogCompleted += (s, e) =>
                    {
                        if (e.Result != null && e.Result.TotalRecord > 0)
                        {
                            List<string> Codes = new List<string>();
                            Codes.Add("UserName");
                            Codes.Add("UserRole");
                            Codes.Add("LoginTime");
                            Codes.Add("LogoutTime");

                            List<string> Names = new List<string>();
                            Names.Add(ApplicationContext.Instance.StringResourceReader.GetString("User_LoginName"));
                            Names.Add(ApplicationContext.Instance.StringResourceReader.GetString("MANAGER_UserType"));
                            Names.Add(ApplicationContext.Instance.StringResourceReader.GetString("MANAGER_LoginTime"));
                            Names.Add(ApplicationContext.Instance.StringResourceReader.GetString("MANAGER_LogoutTime"));

                            List<Spreadsheet.EnumsEx> eList = new List<Spreadsheet.EnumsEx>();

                            List<Spreadsheet.FieldEx> FieldEx1 = new List<Spreadsheet.FieldEx>();
                            Gsafety.PTMS.Bases.Enums.UserRoleConverter urc = new Gsafety.PTMS.Bases.Enums.UserRoleConverter();
                            urc.EnumInfo.ToList().ForEach(x =>
                            {
                                Spreadsheet.FieldEx item = new Spreadsheet.FieldEx { Key = x.Name.ToString(), Value = x.LocalizedString };
                                FieldEx1.Add(item);
                            });
                            eList.Add(new Gsafety.PTMS.Spreadsheet.EnumsEx { Code = "UserRole", Content = FieldEx1 });

                            Spreadsheet.XLSXExporter xlsx = new Spreadsheet.XLSXExporter();
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
                        client.GetLoginLogAsync(username, starttime, endtime.AddDays(1), pagingInfo);
                    }
                    else
                    {
                        PagingInfo pagingInfo = new PagingInfo() { PageIndex = -1, PageSize = 0 };
                        client.GetLoginLogAsync(username, starttime, endtime.AddDays(1), pagingInfo);
                    }
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
            finally
            {
                setExportBtnStatus(true);
            }
        }
        #endregion
    }
}
