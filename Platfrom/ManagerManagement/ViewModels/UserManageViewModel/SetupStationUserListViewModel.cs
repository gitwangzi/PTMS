/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 82af9391-5371-486c-8914-df451a129a57      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-WANGZS
/////                 Author: TEST(wangzs)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Manager.ViewModels
/////    Project Description:    
/////             Class Name: SetupStationUserListViewModel
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/8/12 16:09:53
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/8/12 16:09:53
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using Gsafety.PTMS.Bases.Enums;
using Gsafety.PTMS.Bases.Models;
using Gsafety.PTMS.Manager.Models;
using Gsafety.PTMS.Manager.Views.UserManageView;
using Gsafety.PTMS.ServiceReference.ADGroupService;
using Gsafety.PTMS.ServiceReference.ADUserInfoService;
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
    [ExportAsViewModel(ManagerName.SetupStationListViewModel)]
    public class SetupStationUserListViewModel : BaseViewModel
    {
        public IActionCommand<object> AddCommand { get; set; }
        public IActionCommand<object> QueryCommand { get; set; }
        public IActionCommand<object> UpdateCommand { get; set; }
        public IActionCommand<object> DeleteCommand { get; set; }
        public IActionCommand<object> InitPwdCommand { get; set; }
        public IActionCommand<object> ModifyPwdCommand { get; set; }
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
        public ObservableCollection<UserInfo> UserInfoList { get; set; }
        public List<EnumModel> GroupInfo { get; set; }
        public EnumModel CurrentGroup { get; set; }
        PagedCollectionView _SetupStationPageView;

        public PagedCollectionView SetupStationPageView
        {
            get { return _SetupStationPageView; }
            set { _SetupStationPageView = value; }
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

        public SetupStationUserListViewModel()
        {
            PageSizeList = new List<int> { 20, 40, 80 };
            PageSize = 20;
            Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => PageSize));
            UserInforClient.DeleteAccountCompleted += UserInforClient_DeleteAccountCompleted;
            groupClient.GetAccountInfoByGrouplistCompleted += groupClient_GetAccountInfoByGrouplistCompleted;
            groupClient.GetAccountInfoByGroupNameCompleted += groupClient_GetAccountInfoByGroupNameCompleted;
            UserInforClient.ResetPasswordCompleted += UserInforClient_ResetPasswordCompleted;
            groupClient.GetAccountInfoByGroupAndUserLoginNameCompleted += groupClient_GetAccountInfoByGroupAndUserLoginNameCompleted;
            AddCommand = new ActionCommand<object>(obj => AddAction());
            QueryCommand = new ActionCommand<object>(obj => QueryAction());
            UpdateCommand = new ActionCommand<object>(obj => UpdateAction());
            DeleteCommand = new ActionCommand<object>(obj => DeleteAction());
            InitPwdCommand = new ActionCommand<object>(obj => InitPWDAction(obj));
            ModifyPwdCommand = new ActionCommand<object>(obj => ModifyPWDAction(obj));

            UserInfoList = new ObservableCollection<UserInfo>();
            GroupInfo = new List<EnumModel>();

            _CurrentUserManagerMenuType = UserManagerMenuType.InstallStation;
            Title = ApplicationContext.Instance.StringResourceReader.GetString("InstallStation_UserInfor");
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
                                UserInfoList.Add(new UserInfo
                                {
                                    LoginName = item.UserLoginName,
                                    UserName = item.UserName,
                                    Level = item.Level,
                                    Phone = item.Phone,
                                    Note = item.Description,
                                    Support_StationName = item.OrgName,
                                    ProviceName = item.ProvinceName,
                                    CityName = item.CityName,
                                    SiteCode = item.OrgCode,
                                    ProvinceCode = item.ProvinceCode,
                                    CityCode = item.CityCode,
                                    Description = item.Description,
                                    UserGroup = item.SecurityGroup
                                });
                            }
                        }
                    }
                    if (UserInfoList.Count == 0)
                    {
                        //MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("MAINTAIN_DataEmpty"), ApplicationContext.Instance.StringResourceReader.GetString("TRAFFIC_TIP"), MessageBoxButton.OK);
                    }
                    SetupStationPageView = new PagedCollectionView(UserInfoList);
                    Jounce.Framework.JounceHelper.ExecuteOnUI(() =>
                    {
                        RaisePropertyChanged(() => SetupStationPageView);
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



        void groupClient_GetAccountInfoByGroupNameCompleted(object sender, GetAccountInfoByGroupNameCompletedEventArgs e)
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
                                UserInfoList.Add(new UserInfo
                                {
                                    LoginName = item.UserLoginName,
                                    UserName = item.UserName,
                                    Level = item.Level,
                                    Phone = item.Phone,
                                    Note = item.Description,
                                    Support_StationName = item.OrgName,
                                    ProviceName = item.ProvinceName,
                                    CityName = item.CityName,
                                    SiteCode = item.OrgCode,
                                    ProvinceCode = item.ProvinceCode,
                                    CityCode = item.CityCode,
                                    Description = item.Description,
                                    UserGroup = item.SecurityGroup
                                });
                            }
                        }
                    }
                    if (UserInfoList.Count == 0)
                    {
                        //MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("MAINTAIN_DataEmpty"), ApplicationContext.Instance.StringResourceReader.GetString("TRAFFIC_TIP"), MessageBoxButton.OK);
                    }
                    SetupStationPageView = new PagedCollectionView(UserInfoList);
                    Jounce.Framework.JounceHelper.ExecuteOnUI(() =>
                    {
                        RaisePropertyChanged(() => SetupStationPageView);
                    });
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(GetType().FullName, ex);
            }
        }

        void groupClient_GetAccountInfoByGrouplistCompleted(object sender, GetAccountInfoByGrouplistCompletedEventArgs e)
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
                                UserInfoList.Add(new UserInfo
                                {
                                    LoginName = item.UserLoginName,
                                    UserName = item.UserName,
                                    Level = item.Level,
                                    Phone = item.Phone,
                                    Note = item.Description,
                                    Support_StationName = item.OrgName,
                                    ProviceName = item.ProvinceName,
                                    CityName = item.CityName,
                                    SiteCode = item.OrgCode,
                                    ProvinceCode = item.ProvinceCode,
                                    CityCode = item.CityCode,
                                    Email = item.Email,
                                    Address = item.Address,
                                    Description = item.Description,
                                    UserGroup = item.SecurityGroup

                                });
                            }
                        }


                    }
                    if (UserInfoList.Count == 0)
                    {
                        //MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("MAINTAIN_DataEmpty"), ApplicationContext.Instance.StringResourceReader.GetString("TRAFFIC_TIP"), MessageBoxButton.OK);
                    }
                    SetupStationPageView = new PagedCollectionView(UserInfoList);

                    Jounce.Framework.JounceHelper.ExecuteOnUI(() =>
                    {
                        RaisePropertyChanged(() => SetupStationPageView);
                    });
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(GetType().FullName, ex);
            }
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
                    //
                    //Fixed the third bug No.20 By XiangboLiu 2015/06/11
                    //
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
            if (_CurrentUserManagerMenuType == UserManagerMenuType.InstallStation)
                EventAggregator.Publish(new ViewNavigationArgs(ManagerName.SetupStationUserAddView, new Dictionary<string, object>() { { "groupname", UserGroup.SiteManager } }));

        }

        private void QueryAction()
        {
            try
            {
                IsHandledBusy = true;
                groupClient.GetAccountInfoByGroupAndUserLoginNameAsync(UserGroup.SiteManager, LoginName, UserName);
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
                        if (_CurrentUserManagerMenuType == UserManagerMenuType.InstallStation)
                            EventAggregator.Publish(new ViewNavigationArgs(ManagerName.SetupStationUserEditView, new Dictionary<string, object>() { { "action", "update" }, { "update", CurrentInfoModel } }));

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

        private void UserUpdateView_Closed(object sender, EventArgs e)
        {

        }
        /// <summary>
        /// Delete Operation
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
            GroupInfo.Clear();
            GroupInfo.Add(new EnumModel { ShowName = ApplicationContext.Instance.StringResourceReader.GetString("BASEINFO_PleaseSelect"), EnumName = string.Empty });
            if (!ApplicationContext.Instance.AuthenticationInfo.GroupName.Equals(UserGroup.SecurityAdmin))
            {
                string x = ApplicationContext.Instance.AuthenticationInfo.GroupName;
                GroupInfo.Add(new EnumModel { EnumName = x, ShowName = ApplicationContext.Instance.StringResourceReader.GetString(x) });
                GropQueryEnable = false;
            }
            else
            {
                if (_CurrentUserManagerMenuType == UserManagerMenuType.InstallStation)
                {
                    string x = UserGroup.SiteManager;
                    GroupInfo.Add(new EnumModel { EnumName = x, ShowName = ApplicationContext.Instance.StringResourceReader.GetString(x) });
                }

                else if (_CurrentUserManagerMenuType == UserManagerMenuType.TrafficPart)
                {
                    string x = UserGroup.SecurityManager;
                    string y = UserGroup.SecurityAdmin;
                    GroupInfo.Add(new EnumModel { EnumName = x, ShowName = ApplicationContext.Instance.StringResourceReader.GetString(x) });
                    GroupInfo.Add(new EnumModel { EnumName = x, ShowName = ApplicationContext.Instance.StringResourceReader.GetString(x) });
                }
                GropQueryEnable = true;
            }
            if (GroupInfo.Count > 0)
                CurrentGroup = GroupInfo[0];
            if (GroupInfo.Count > 1)
            {
                if (ApplicationContext.Instance.AuthenticationInfo.GroupName == UserGroup.SiteManager)
                {
                    CurrentGroup = GroupInfo.FirstOrDefault(x => x.EnumName == ApplicationContext.Instance.AuthenticationInfo.GroupName);
                }

            }
            if (ApplicationContext.Instance.AuthenticationInfo.GroupName.Equals(UserGroup.SecurityAdmin) || ApplicationContext.Instance.AuthenticationInfo.GroupName.Equals(UserGroup.SecurityManager))
            {
                ComVisble = Visibility.Collapsed;
                SiteVisible = false;
            }

            else if (ApplicationContext.Instance.AuthenticationInfo.GroupName.Equals(UserGroup.SiteManager))
            {
                ComVisble = Visibility.Collapsed;
                SiteVisible = true;
            }
            Jounce.Framework.JounceHelper.ExecuteOnUI(() =>
            {
                RaisePropertyChanged(() => GroupInfo);
                RaisePropertyChanged(() => CurrentGroup);
                RaisePropertyChanged(() => GropQueryEnable);
            });
        }
    }
}
