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
/////             Class Name: UserListViewModel
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/8/12 16:12:13
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/8/12 16:12:13
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using Gsafety.PTMS.Bases.Enums;
using Gsafety.PTMS.Manager.Models;
using Gsafety.PTMS.Manager.Views.UserManageView;
using Gsafety.PTMS.ServiceReference.ADGroupService;
using Gsafety.PTMS.ServiceReference.ADUserInfoService;
using Gsafety.PTMS.ServiceReference.DistrictService;
using Gsafety.PTMS.Share;
using Jounce.Core.Command;
using Jounce.Core.View;
using Jounce.Core.ViewModel;
using Jounce.Framework.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Data;

namespace Gsafety.PTMS.Manager.ViewModels
{
    [ExportAsViewModel(ManagerName.SysManagerUserListViewModel)]
    public class SysManagerUserListViewModel : BaseViewModel
    {
        public IActionCommand<object> AddCommand { get; set; }
        public IActionCommand<object> QueryCommand { get; set; }
        public IActionCommand<object> UpdateCommand { get; set; }
        public IActionCommand<object> DeleteCommand { get; set; }
        public IActionCommand<object> InitPwdCommand { get; set; }
        public IActionCommand<object> ModifyPwdCommand { get; set; }

        public ObservableCollection<UserInfo> UserInfoList { get; set; }
        public string LoginName { get; set; }
        public string UserName { get; set; }
        private bool _IsHandledBusy;

        public bool IsHandledBusy
        {
            get
            {
                return this._IsHandledBusy;
            }
            set
            {
                this._IsHandledBusy = value;
                RaisePropertyChanged(() => this.IsHandledBusy);
            }
        }
        public string BusyContent
        {
            get
            {
                return ApplicationContext.Instance.StringResourceReader.GetString("ALARM_PleaseWait");
            }
        }
        private List<int> _PageSizeList;
        public List<int> PageSizeList
        {
            get
            {
                return _PageSizeList;
            }
            set
            {
                _PageSizeList = value;
            }
        }
        private int _PageSizeValue = 20;
        public int PageSizeValue
        {
            get
            {
                return _PageSizeValue;
            }
            set
            {
                _PageSizeValue = value;
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => PageSizeValue));
                PageSizeChenged();
            }
        }
        private int _PageSize;

        public int PageSize
        {
            get { return _PageSize; }
            set { _PageSize = value; }
        }
        private void PageSizeChenged()
        {
            PageSize = PageSizeValue;

            Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => PageSize));
            QueryAction();
        }
        private UserInfo _currentUserInfo;
        public UserInfo CurrentUserInfo
        {
            get
            {
                return this._currentUserInfo;
            }
            set
            {
                this._currentUserInfo = value;
                if (value != null)
                {
                    CurrentInfoModel = ADAccountInfoModelList.ToList().FirstOrDefault(x => x.UserLoginName == CurrentUserInfo.LoginName);
                }
            }
        }

        public Gsafety.PTMS.ServiceReference.ADGroupService.ADAccountInfo CurrentInfoModel
        {
            get;
            set;
        }

        public ObservableCollection<Gsafety.PTMS.ServiceReference.ADGroupService.ADAccountInfo> ADAccountInfoModelList
        {
            get;
            set;
        }
        ADAccountServiceClient UserInforClient = ServiceClientFactory.Create<ADAccountServiceClient>();
        DistrictServiceClient districtClient = ServiceClientFactory.Create<DistrictServiceClient>();
        UserManagerMenuType _CurrentUserManagerMenuType = UserManagerMenuType.TrafficPart;
        GroupServiceClient groupClient = ServiceClientFactory.Create<GroupServiceClient>();
        public bool GropQueryEnable { get; set; }

        private string _title;
        public string Title
        {
            get
            {
                return this._title;
            }
            set
            {
                this._title = value;
                RaisePropertyChanged(() => Title);
            }
        }

        public SysManagerUserListViewModel()
        {
            PageSizeList = new List<int> { 20, 40, 80 };
            PageSize = 20;
            Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => PageSize));
            UserInforClient.DeleteAccountCompleted += UserInforClient_DeleteAccountCompleted;
            groupClient.GetAccountInfoByGroupAndUserLoginNameCompleted += groupClient_GetAccountInfoByGroupAndUserLoginNameCompleted;
            UserInforClient.ResetPasswordCompleted += UserInforClient_ResetPasswordCompleted;
            districtClient.DeleteUserAuthorityCompleted += districtClient_DeleteUserAuthorityCompleted;
            AddCommand = new ActionCommand<object>(obj => AddAction());
            QueryCommand = new ActionCommand<object>(obj => QueryAction());
            UpdateCommand = new ActionCommand<object>(obj => UpdateAction());
            DeleteCommand = new ActionCommand<object>(obj => DeleteAction());
            InitPwdCommand = new ActionCommand<object>(obj => InitPWDAction(obj));
            ModifyPwdCommand = new ActionCommand<object>(obj => ModifyPWDAction(obj));

            UserInfoList = new ObservableCollection<UserInfo>();
            _CurrentUserManagerMenuType = UserManagerMenuType.TrafficPart;
            Title = ApplicationContext.Instance.StringResourceReader.GetString("SysMangerUserList");
            InitPageInfor();
            QueryAction();

        }

        private void groupClient_GetAccountInfoByGroupAndUserLoginNameCompleted(object sender, GetAccountInfoByGroupAndUserLoginNameCompletedEventArgs e)
        {
            try
            {
                UserInfoList.Clear();
                if (e.Result != null && e.Result.Result.Count > 0)
                {
                    ADAccountInfoModelList = e.Result.Result;
                    foreach (var item in e.Result.Result)
                    {
                        if (item != null)
                        {
                            if ((ApplicationContext.Instance.AuthenticationInfo.GroupName == UserGroup.SecurityAdmin) || (item.UserName == ApplicationContext.Instance.AuthenticationInfo.UserName))
                            {
                                UserInfo myinfo = new UserInfo();
                                myinfo.LoginName = item.UserLoginName;
                                myinfo.UserName = item.UserName;
                                myinfo.Level = item.Level;
                                if (myinfo.Level == 0)
                                {
                                    myinfo.UserLevel = ApplicationContext.Instance.StringResourceReader.GetString("CountryLevel");
                                }
                                if (myinfo.Level == 1)
                                {
                                    myinfo.UserLevel = ApplicationContext.Instance.StringResourceReader.GetString("ProvinceLevel");
                                }
                                if (myinfo.Level == 2)
                                {
                                    myinfo.UserLevel = ApplicationContext.Instance.StringResourceReader.GetString("CityLevel");
                                }
                                myinfo.Phone = item.Phone;
                                myinfo.Note = item.Description;
                                myinfo.Email = item.Email;
                                myinfo.Address = item.Address;
                                myinfo.ProviceName = item.ProvinceName;
                                myinfo.CityName = item.CityName;
                                myinfo.ProvinceCode = item.ProvinceCode;
                                myinfo.CityCode = item.CityCode;
                                myinfo.Description = item.Description;
                                myinfo.UserGroup = item.SecurityGroup;
                                UserInfoList.Add(myinfo);
                            }
                        }


                    }
                    if (UserInfoList.Count == 0)
                    {
                        //MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("MAINTAIN_DataEmpty"), ApplicationContext.Instance.StringResourceReader.GetString("TRAFFIC_TIP"), MessageBoxButton.OK);
                    }
                    TrafficInfoPageView = new PagedCollectionView(UserInfoList);
                    Jounce.Framework.JounceHelper.ExecuteOnUI(() =>
                    {
                        RaisePropertyChanged(() => TrafficInfoPageView);

                    });
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(GetType().FullName, ex);
            }
            finally
            {
                IsHandledBusy = false;
            }
        }

        void districtClient_DeleteUserAuthorityCompleted(object sender, DeleteUserAuthorityCompletedEventArgs e)
        {

        }
        void UserInforClient_ResetPasswordCompleted(object sender, ResetPasswordCompletedEventArgs e)
        {
            if (e.Result.Result == true)
            {
                MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("ResetPwdSuccess"), ApplicationContext.Instance.StringResourceReader.GetString("Tip"), MessageBoxButton.OK);
            }
        }
        private Visibility _ComVisble;

        public Visibility ComVisble
        {
            get { return _ComVisble; }
            set { _ComVisble = value; }
        }
        private bool _SiteVisible;

        public bool SiteVisible
        {
            get { return _SiteVisible; }
            set { _SiteVisible = value; }
        }

        private bool CanModifyPWD(object obj)
        {
            if (obj == null) return false;
            return ((obj as UserInfo).LoginName == ApplicationContext.Instance.AuthenticationInfo.UserName);
        }
        PagedCollectionView _TrafficInfoPageView;

        public PagedCollectionView TrafficInfoPageView
        {
            get { return _TrafficInfoPageView; }
            set { _TrafficInfoPageView = value; }
        }

        private bool CanInitPWD(object obj)
        {
            if (obj == null) return false;
            return (ApplicationContext.Instance.AuthenticationInfo.GroupName == UserGroup.SecurityAdmin);
        }

        private bool CanUpdate(object obj)
        {
            if (obj == null) return false;
            return ((obj as UserInfo).LoginName == ApplicationContext.Instance.AuthenticationInfo.UserName) || (ApplicationContext.Instance.AuthenticationInfo.GroupName == UserGroup.SecurityAdmin);
        }

        private bool CanDelete(object obj)
        {
            if (obj == null) return false;
            return ((obj as UserInfo).LoginName != ApplicationContext.Instance.AuthenticationInfo.UserName) && (ApplicationContext.Instance.AuthenticationInfo.GroupName == UserGroup.SecurityAdmin);
        }
        private void ModifyPWDAction(object obj)
        {
            if (ApplicationContext.Instance.AuthenticationInfo.GroupName == UserGroup.SecurityAdmin || (CurrentUserInfo as UserInfo).LoginName == ApplicationContext.Instance.AuthenticationInfo.UserName)
            {
                EventAggregator.Publish(new ViewNavigationArgs("UserUpdatePasswordView", new Dictionary<string, object>() { { "action", "update" }, { "update", CurrentUserInfo } }));
            }
            else
            {
                MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("NoValidate"), ApplicationContext.Instance.StringResourceReader.GetString("Tip"), MessageBoxButton.OK);
            }
        }

        private void InitPWDAction(object obj)
        {
            if (ApplicationContext.Instance.AuthenticationInfo.GroupName == UserGroup.SecurityAdmin)
            {
                var result = MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("ConfirmInitPwd"), ApplicationContext.Instance.StringResourceReader.GetString("Tip"), MessageBoxButton.OKCancel);
                if (result == MessageBoxResult.OK)
                {
                    ResetPassword resetwindow = new ResetPassword(CurrentUserInfo.UserName);
                    resetwindow.Show();
                }
            }
            else
            {
                MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("NoValidate"), ApplicationContext.Instance.StringResourceReader.GetString("Tip"), MessageBoxButton.OK);
            }
        }

        protected override void ActivateView(string viewName, System.Collections.Generic.IDictionary<string, object> viewParameters)
        {
            base.ActivateView(viewName, viewParameters);
            if (viewParameters.Count > 0)
            {

                if (viewParameters.Keys.Contains("action"))
                {
                    if (viewParameters["action"].Equals("refresh"))
                    {
                        QueryAction();
                    }
                    return;
                    //QueryAction();
                    //return;
                }
                else
                {
                    string userManagerMenuType = viewParameters["UserManagerMenuType"].ToString();
                    if (userManagerMenuType.Equals(UserManagerMenuType.InstallStation.ToString()))
                    {
                        _CurrentUserManagerMenuType = UserManagerMenuType.InstallStation;
                        Title = ApplicationContext.Instance.StringResourceReader.GetString("InstallStation_UserInfor");
                    }

                    if (userManagerMenuType.Equals(UserManagerMenuType.TrafficPart.ToString()))
                    {
                        _CurrentUserManagerMenuType = UserManagerMenuType.TrafficPart;
                        Title = ApplicationContext.Instance.StringResourceReader.GetString("TrafficPart_UserInfor");
                    }
                }

                InitPageInfor();
                QueryAction();
            }
            else
            {
                InitPageInfor();
            }
        }

        private void AddAction()
        {
            if (_CurrentUserManagerMenuType == UserManagerMenuType.TrafficPart)
                EventAggregator.Publish(new ViewNavigationArgs(ManagerName.TrafficUserAddView, new Dictionary<string, object>() { { "groupname", UserGroup.SecurityAdmin } }));
        }
        private void QueryAction()
        {
            try
            {
                IsHandledBusy = true;

                groupClient.GetAccountInfoByGroupAndUserLoginNameAsync(UserGroup.SecurityAdmin, LoginName, UserName);
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(GetType().FullName, ex);
            }
        }

        private void UpdateAction()
        {
            try
            {
                if (CurrentUserInfo != null)
                {
                    if (((CurrentUserInfo as UserInfo).LoginName == ApplicationContext.Instance.AuthenticationInfo.UserName) || (ApplicationContext.Instance.AuthenticationInfo.GroupName == UserGroup.SecurityAdmin))
                    {

                        if (_CurrentUserManagerMenuType == UserManagerMenuType.TrafficPart)
                            EventAggregator.Publish(new ViewNavigationArgs(ManagerName.TrafficUserEditView, new Dictionary<string, object>() { { "currentpage", "SysManagerUserListView" }, { "action", "update" }, { "update", CurrentInfoModel } }));

                    }
                    else
                    {
                        MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("NoValidate"), ApplicationContext.Instance.StringResourceReader.GetString("Tip"), MessageBoxButton.OK);
                    }
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(GetType().FullName, ex);
            }

        }
        public bool isAdmin
        {
            get
            {
                return (ApplicationContext.Instance.AuthenticationInfo.GroupName == UserGroup.SecurityAdmin);
            }
        }
        /// <summary>
        /// delete operation
        /// </summary>
        private void DeleteAction()
        {
            if (CurrentUserInfo == null) return;
            if (((CurrentUserInfo as UserInfo).LoginName != ApplicationContext.Instance.AuthenticationInfo.UserName)
                && (ApplicationContext.Instance.AuthenticationInfo.GroupName == UserGroup.SecurityAdmin))
            {
                var result = MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("ConfirmDelete"), ApplicationContext.Instance.StringResourceReader.GetString("Tip"), MessageBoxButton.OKCancel);

                if (result == MessageBoxResult.OK)
                {
                    try
                    {
                        districtClient.DeleteUserAuthorityAsync(CurrentUserInfo.LoginName);
                        UserInforClient.DeleteAccountAsync(CurrentUserInfo.LoginName);
                    }
                    catch (Exception ex)
                    {
                        ApplicationContext.Instance.Logger.LogException(GetType().FullName, ex);
                    }
                }
            }
            else
            {
                MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("NoValidate"), ApplicationContext.Instance.StringResourceReader.GetString("Tip"), MessageBoxButton.OK);
            }

        }

        void UserInforClient_DeleteAccountCompleted(object sender, DeleteAccountCompletedEventArgs e)
        {
            if (e.Result.Result == true)
            {
                ApplicationContext.Instance.MessageManager.SendDeleteUserMessage(new Gsafety.PTMS.ServiceReference.MessageService.DeleteUser() { DeleteTime = DateTime.Now, UserName = CurrentUserInfo.LoginName });
                MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("DeleteSuccess"), ApplicationContext.Instance.StringResourceReader.GetString("Tip"), MessageBoxButton.OK);
                QueryAction();
            }
            else
            {
                MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("DeleteFailed"), ApplicationContext.Instance.StringResourceReader.GetString("Tip"), MessageBoxButton.OK);
            }
        }
        private void InitPageInfor()
        {
            GropQueryEnable = true;
        }
    }
}
