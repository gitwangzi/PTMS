/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 1eba4082-e4d8-473c-a9aa-68ad9dc4c9b6      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-WANGZS
/////                 Author: TEST(wangzs)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Manager.ViewModels
/////    Project Description:    
/////             Class Name: UserOnlineViewModel
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/8/12 16:12:13
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/8/12 16:12:13
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using Gsafety.PTMS.Manager.ViewModels.LogManageViewModel;
using Gsafety.PTMS.ServiceReference.PTMSLogManageService;
using Gsafety.PTMS.Share;
using Gsafety.Common.Localization.Resource;
using Jounce.Core.Command;
using Jounce.Core.ViewModel;
using Jounce.Framework.Command;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Data;
using System.Linq;
using System.Reflection;
namespace Gsafety.PTMS.Manager.ViewModels.UserManageViewModel
{
    [ExportAsViewModel(ManagerName.UserOnlineViewModel)]
    public class UserOnlineViewModel : BaseViewModel
    {
        private int currentIndex = 1;
        LoginLogServiceClient antLogClient;

        #region DateProperties & CommandProperties
        public string UserName { get; set; }     //Bingding view item
        public DateTime StartTime { get; set; }     //Bingding view item
        public DateTime EndTime { get; set; }     //Bingding view item
        //public LoginLogInfo CurrentLog { get; set; }

        public PagedCollectionView UserOnlineListPaged { get; set; }

        public IList<string> UserStatus { get; set; }

        private string selectedUserStatus;
        public string SelectedUserStatus
        {
            get
            {
                return this.selectedUserStatus;
            }
            set
            {
                this.selectedUserStatus = value;
                RaisePropertyChanged("SelectedUserStatus");
            }
        }

        private string selectedUserStatusIndex;
        public string SelectedUserStatusIndex
        {
            get
            {
                return this.selectedUserStatusIndex;
            }
            set
            {
                this.selectedUserStatusIndex = value;
                RaisePropertyChanged("SelectedUserStatusIndex");
            }
        }

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
        public IActionCommand ExportPage { get; private set; }

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
        public UserOnlineViewModel()
        {
            try
            {
                antLogClient = ServiceClientFactory.Create<LoginLogServiceClient>();
                QueryCommand = new ActionCommand<object>(obj => Query());
                ExportPage = new ActionCommand<object>(obj => Export());
                PageSizeValue = 20;
                PageSizeList = new List<int> { 20, 40, 80 };
                UIInit();
                InitPagedServerCollection();

                UserStatus = new List<string>();
                UserStatus.Add(ApplicationContext.Instance.StringResourceReader.GetString("BASEINFO_All"));
                UserStatus.Add(ApplicationContext.Instance.StringResourceReader.GetString("MANAGER_User_Online"));
                UserStatus.Add(ApplicationContext.Instance.StringResourceReader.GetString("MANAGER_User_Offline"));
                SelectedUserStatus = UserStatus[0];
                SelectedUserStatusIndex = "0";
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }

        private object Export()
        {
            try
            {
                new LogHelper().ExportExcel();
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
            return null;

        }

        //Init
        private void InitPagedServerCollection()
        {
            try
            {
                antLogClient.GetUserOnlineCompleted += antLogClient_GetUserOnlineCompleted;
                InvokServer(1, this.PageSizeValue);
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }

        //void antLogClient_GetUserOnlineCompleted(object sender, GetUserOnlineCompletedEventArgs e)
        //{
        //    try
        //    {
        //        List<LoginLogInfo> res = e.Result.Result.ToList();
        //        foreach (LoginLogInfo item in res)
        //        {
        //            if (item.LogoutTime != null)
        //            {
        //                if (DateTime.Now.Subtract(item.LogoutTime.Value).TotalMinutes < ApplicationContext.Instance.ServerConfig.LogUpdateInterval)
        //                {
        //                    item.LogoutTime = null;
        //                }
        //            }
        //        }
        //        if (!string.IsNullOrEmpty(SelectedUserStatus))
        //        {
        //            if (SelectedUserStatusIndex.Equals("2"))
        //            {
        //                res = res.Where(x => x.LoginTime != null && x.LogoutTime != null).ToList();
        //            }
        //            else if (SelectedUserStatusIndex.Equals("1"))
        //            {
        //                res = res.Where(x => x.LoginTime != null && x.LogoutTime == null).ToList();
        //            }
        //        }
        //        UserOnlineListPaged = new PagedCollectionView(res);
        //        Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => UserOnlineListPaged));
        //        if (e.Result.TotalRecord == 0)
        //        {
        //            //MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("MAINTAIN_DataEmpty"), ApplicationContext.Instance.StringResourceReader.GetString("TRAFFIC_TIP"), MessageBoxButton.OK);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);

        //        //MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("BASEINFO_Operate_Failed"));
        //    }
        //    IsBusy = false;
        //}
        //Bingding view values
        private void UIInit()
        {
            StartTime = DateTime.Now.AddMonths(-1);
            EndTime = DateTime.Now;
        }
        #endregion

        #region Functions & Events
        //Search
        private void Query()
        {
            if (new LogHelper().SeearchConditionValid(StartTime, EndTime))
            {
                currentIndex = 1;
                this.InvokServer(currentIndex, this.PageSizeValue);
            }
        }
        //// Invoke Server
        //private void InvokServer(int pageIndex, int pageSize)
        //{
        //    IsBusy = true;
        //    pageSize = pageSizeValue;
        //    System.Threading.Interlocked.Exchange(ref currentIndex, pageIndex);
        //    string districtCode = string.Empty;
        //    PagingInfo pagingInfo = new PagingInfo() { PageIndex = pageIndex, PageSize = pageSize };
        //    try
        //    {
        //        antLogClient.GetUserOnlineAsync(string.IsNullOrEmpty(UserName) ? "" : UserName.Trim(), StartTime, EndTime, pagingInfo);
        //    }
        //    catch (Exception ex)
        //    {
        //        ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);

        //        new LogHelper().RomoteError();
        //    }
        //}

        //RefreshPage view
        protected override void ActivateView(string viewName, System.Collections.Generic.IDictionary<string, object> viewParameters)
        {
            try
            {
                //base.ActivateView(viewName, viewParameters);
                //if (viewParameters.Count != 0 && viewParameters["action"].ToString() == "return")
                //{
                //    this.InvokServer(currentIndex, this.PageSizeValue);
                //}
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }
        #endregion
    }
}
