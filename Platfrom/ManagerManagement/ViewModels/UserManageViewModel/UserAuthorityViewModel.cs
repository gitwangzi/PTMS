using BaseLib.Model;
using BaseLib.ViewModels;
/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 350a1a5c-192b-4cf0-9d47-1d0f5f903a30      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-PENGGL
/////                 Author: TEST(penggl)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Manager.ViewModels.UserManageViewModel
/////    Project Description:    
/////             Class Name: UserAuthorityViewModel
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/10/11 14:10:41
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/10/11 14:10:41
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using Gsafety.PTMS.Manager.Models;
using Gsafety.PTMS.ServiceReference.ADGroupService;
using Gsafety.PTMS.ServiceReference.DistrictService;
using Gsafety.PTMS.Share;
using Jounce.Core.Command;
using Jounce.Core.View;
using Jounce.Core.ViewModel;
using Jounce.Framework.Command;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace Gsafety.PTMS.Manager.ViewModels.UserManageViewModel
{
    [ExportAsViewModel(ManagerName.UserAuthorityViewModel)]
    public class UserAuthorityViewModel : BaseViewModel
    {
        public PagedServerCollection<UserAuthority> PSC_UserAuthoritys { get; set; }
        private string loginname;
        public string LoginName { get; set; }
        public UserAuthority CurrentUserAuthority { get; set; }
        public List<int> PageSizeList { get; set; }
        private List<int> pageSizeList
        {
            get
            {
                List<int> pageSizeList = new List<int>();
                pageSizeList.Add(20);
                pageSizeList.Add(40);
                pageSizeList.Add(80);
                return pageSizeList;
            }
        }
        public List<UserInfo> UserInfoList { get; set; }
        public IActionCommand UpdateCommand { get; private set; }
        public IActionCommand QueryCommand { get; private set; }
        public IActionCommand ViewCommand { get; private set; }
        private GroupServiceClient groupClient = ServiceClientFactory.Create<GroupServiceClient>();
        private DistrictServiceClient districtClient = ServiceClientFactory.Create<DistrictServiceClient>();

        private bool isInitialOrQuery;
        protected override void ActivateView(string viewName, System.Collections.Generic.IDictionary<string, object> viewParameters)
        {
            base.ActivateView(viewName, viewParameters);
            if (viewParameters.Count != 0 && viewParameters["action"].ToString() == "return")
            {
                ObservableCollection<string> mylist = new ObservableCollection<string>();
                mylist.Add(UserGroup.SecurityManager);
                mylist.Add(UserGroup.AlarmFilterCommissioner);
                if (viewParameters.Keys.Contains("refresh") && (bool)viewParameters["refresh"])
                {
                    groupClient.GetAccountInfoByGrouplistAsync(mylist);
                }
            }
            else
            {
                if (isInitialOrQuery)
                {
                    ObservableCollection<string> mylist = new ObservableCollection<string>();
                    mylist.Add(UserGroup.SecurityManager);
                    mylist.Add(UserGroup.AlarmFilterCommissioner);
                    groupClient.GetAccountInfoByGrouplistAsync(mylist);
                }
            }
        }

        private int pageSizeValue;
        public int PageSizeValue
        {
            get { return pageSizeValue; }
            set
            {
                pageSizeValue = value;
                if (this.PSC_UserAuthoritys != null)
                {
                    this.PSC_UserAuthoritys.PageSize = value;
                }
            }
        }

        public UserAuthorityViewModel()
        {
            isInitialOrQuery = true;
            groupClient.GetAccountInfoByGrouplistCompleted += groupClient_GetAccountInfoByGrouplistCompleted;

            UpdateCommand = new ActionCommand<object>(obj => Publish("update"));
            ViewCommand = new ActionCommand<object>(obj => Publish("view"));
            QueryCommand = new ActionCommand<object>(obj => Query());

            PageSizeList = pageSizeList;
            PageSizeValue = PageSizeList[0];
            InitPagedServerCollection();
        }

        void client_GetUserAuthorityFuzzyCompleted(object sender, GetUserAuthorityFuzzyCompletedEventArgs e)
        {
            if (UserInfoList != null && UserInfoList.Count > 0)
            {
                e.Result.Result.ToList().ForEach(x =>
                {
                    UserInfoList.ForEach(y =>
                    {
                        if (y.LoginName == x.LoginName)
                        {
                            x.Province = y.ProviceName;
                            x.UserName = y.UserName;
                        }
                    });
                });
            }

            PSC_UserAuthoritys.loader_Finished(new PagedResult<UserAuthority>
            {
                Count = e.Result.TotalRecord,
                Items = e.Result.Result,
                PageIndex = currentIndex,
            });

            if (e.Result.TotalRecord == 0 && isInitialOrQuery)
            {
                //MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("MAINTAIN_DataEmpty"), ApplicationContext.Instance.StringResourceReader.GetString("TRAFFIC_TIP"), MessageBoxButton.OK);
            }
            if (isInitialOrQuery)
            {
                isInitialOrQuery = false;
            }
        }

        private int currentIndex = 1;
        private void InitPagedServerCollection()
        {
            districtClient.GetUserAuthorityFuzzyCompleted += client_GetUserAuthorityFuzzyCompleted;
            PSC_UserAuthoritys = new PagedServerCollection<UserAuthority>((pageIndex, pageSize) =>
            {
                pageSize = PageSizeValue;
                System.Threading.Interlocked.Exchange(ref currentIndex, pageIndex);
                PagingInfo pagingInfo = new PagingInfo() { PageIndex = pageIndex, PageSize = pageSize };
                districtClient.GetUserAuthorityFuzzyAsync(loginname, pagingInfo);
            });
        }

        private void Publish(string name)
        {
            EventAggregator.Publish(new ViewNavigationArgs(ManagerName.UserAuthorityManageView, new Dictionary<string, object>() { { "action", name }, { name, CurrentUserAuthority } }));
        }

        private void Query()
        {
            isInitialOrQuery = true;
            loginname = string.IsNullOrEmpty(LoginName) ? string.Empty : LoginName.Trim();
            currentIndex = 1;
            PSC_UserAuthoritys.MoveToFirstPage();
        }

        public ObservableCollection<ADAccountInfo> ADAccountInfoModelList { get; set; }

        void groupClient_GetAccountInfoByGrouplistCompleted(object sender, GetAccountInfoByGrouplistCompletedEventArgs e)
        {
            if (e.Result.Result != null && e.Result.Result.Count > 0)
            {
                UserInfoList = new List<UserInfo>();
                ADAccountInfoModelList = e.Result.Result;
                foreach (var item in e.Result.Result)
                {
                    if (item != null && (item.Level == (short)UserAuthorityType.ProvinceLevel || item.Level == (short)UserAuthorityType.CountryLevel))
                    {
                        UserInfoList.Add(new UserInfo
                        {
                            LoginName = item.UserLoginName,
                            ProviceName = item.ProvinceName,
                            UserName = item.UserName,
                        });
                    }
                }
            }
            PSC_UserAuthoritys.ToPage(currentIndex);
        }
    }
}
