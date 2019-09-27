/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 9667a459-c190-4ab6-8fa9-40cd390888e4      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-PENGGL
/////                 Author: TEST(penggl)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Manager.ViewModels.UserManageViewModel
/////    Project Description:    
/////             Class Name: UserAuthorityManageViewModel
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/10/11 14:18:37
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/10/11 14:18:37
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using Gsafety.PTMS.ServiceReference.ADUserInfoService;
using Gsafety.PTMS.ServiceReference.DistrictService;
using Gsafety.PTMS.Share;
using Jounce.Core.Command;
using Jounce.Core.View;
using Jounce.Core.ViewModel;
using Jounce.Framework.Command;
using Jounce.Framework.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace Gsafety.PTMS.Manager.ViewModels.UserManageViewModel
{
    [ExportAsViewModel(ManagerName.UserAuthorityManageViewModel)]
    public class UserAuthorityManageViewModel : BaseEntityViewModel
    {
        private DistrictServiceClient client = ServiceClientFactory.Create<DistrictServiceClient>();
        private ADAccountServiceClient ADclient = ServiceClientFactory.Create<ADAccountServiceClient>();
        private UserAuthority CurrentUserAuthority { get; set; }
        public bool IsRefresh { get; set; }
        private UserAuthority InitialUserAuthority { get; set; }
        private string userProvinceCode;
        public bool IsChecked { get; set; }
        public ICommand ResetCommand { get; private set; }
        public ICommand ReturnCommand { get; private set; }
        public ICommand SubmitCommand { get; private set; }
        public IActionCommand AllCheckCommand { get; private set; }
        public IActionCommand CheckCommand { get; private set; }
        public string Title { get; set; }
        public string IsView { get; set; }
        public string LoginName { get; set; }

        private string action;
        protected override void ActivateView(string viewName, System.Collections.Generic.IDictionary<string, object> viewParameters)
        {
            IsRefresh = false;
            base.ActivateView(viewName, viewParameters);
            action = viewParameters["action"].ToString();
            switch (action)
            {
                case "view":
                    Title = ApplicationContext.Instance.StringResourceReader.GetString("MANAGE_ViewRegionDistribute");
                    Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => Title));
                    IsView = "Collapsed";
                    Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => IsView));
                    InitialUserAuthority = viewParameters["view"] as UserAuthority;
                    UserAuthorityClone();
                    break;
                case "update":
                    Title = ApplicationContext.Instance.StringResourceReader.GetString("MANAGE_UpdateRegionDistribute");
                    Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => Title));
                    IsView = "Visible";
                    Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => IsView));
                    InitialUserAuthority = viewParameters["update"] as UserAuthority;
                    UserAuthorityClone();
                    break;
                default:
                    break;
            }
            IsChecked = false;
            Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => IsChecked));
            if (CurrentUserAuthority.UserType == UserAuthorityType.CountryLevel)
            {
                Reset();
            }
            else
            {
                ADclient.GetAccountAsync(CurrentUserAuthority.LoginName);
            }
        }

        private void UserAuthorityClone()
        {
            var ser = new System.Runtime.Serialization.DataContractSerializer(typeof(UserAuthority));
            using (var ms = new MemoryStream())
            {
                ser.WriteObject(ms, InitialUserAuthority);
                ms.Seek(0, SeekOrigin.Begin);
                CurrentUserAuthority = (UserAuthority)ser.ReadObject(ms); ;
            }
        }

        public UserAuthorityManageViewModel()
        {
            IsRefresh = false;
            ReturnCommand = new ActionCommand<object>(obj => Return());
            ResetCommand = new ActionCommand<object>(obj => Reset());
            SubmitCommand = new ActionCommand<object>(obj => Submit());
            CheckCommand = new ActionCommand<object>(obj => Check());
            AllCheckCommand = new ActionCommand<object>(obj => AllCheck());
            client.UpdateUserAuthorityCompleted += client_UpdateUserAuthorityCompleted;
            ADclient.GetAccountCompleted += UserInforClient_GetAccountCompleted;
        }

        private void AllCheck()
        {
            if (RegionCheckList != null)
            {
                GetRegionList();
                if (IsChecked)
                {
                    RegionCheckList.ForEach(x => x.Flag = true);
                }
                else
                {
                    RegionCheckList.ForEach(x => x.Flag = false);
                }
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => RegionCheckList));
            }
        }
        private void Check()
        {
            if (RegionCheckList != null)
            {
                IsChecked = RegionCheckList.Any(x => x.Flag == false) ? false : true;
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => IsChecked));
            }
        }
        void UserInforClient_GetAccountCompleted(object sender, GetAccountCompletedEventArgs e)
        {
            if (e.Result.Result != null && e.Result.Result.ProvinceCode != null)
            {
                userProvinceCode = e.Result.Result.ProvinceCode;
                Reset();
            }
            else
            {
                MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("MANAGE_UserInfoError"), ApplicationContext.Instance.StringResourceReader.GetString("TRAFFIC_TIP"), MessageBoxButton.OK);
                Return();
            }
        }

        void client_UpdateUserAuthorityCompleted(object sender, UpdateUserAuthorityCompletedEventArgs e)
        {
            if (e.Result == null || !e.Result.IsSuccess)
            {
                IsRefresh = false;
                MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("BASEINFO_Operate_Failed"), ApplicationContext.Instance.StringResourceReader.GetString("TRAFFIC_TIP"), MessageBoxButton.OK);
            }
            IsRefresh = true;
            ApplicationContext.Instance.MessageManager.SendChangeUserMessage(new Gsafety.PTMS.ServiceReference.MessageService.ChangeUser() { ChangeTime = DateTime.Now, UserName = this.LoginName });
            Return();
        }

        public List<RegionCheck> RegionCheckList { get; set; }
        private void Submit()
        {
            StringBuilder sb = new StringBuilder();
            string regionCodes = string.Empty;
            if (RegionCheckList.Any(x => x.Flag == false))
            {
                RegionCheckList.ForEach(x =>
                  {
                      if (x.Flag)
                      {
                          sb.Append(x.Code + ", ");
                      }
                  });
                regionCodes = sb.ToString();
                if (regionCodes.EndsWith(", "))
                {
                    regionCodes = regionCodes.Remove(regionCodes.LastIndexOf(", "));
                }
            }
            else
            {
                regionCodes = "*";
            }

            if (CurrentUserAuthority != null)
            {
                if (regionCodes.Trim() == string.Empty)
                {
                    MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("MANAGE_RegionNotNull"), ApplicationContext.Instance.StringResourceReader.GetString("TRAFFIC_TIP"), MessageBoxButton.OK);
                    Reset();
                }
                else
                {
                    CurrentUserAuthority.RegionsCode = regionCodes;
                    client.UpdateUserAuthorityAsync(CurrentUserAuthority);
                }
            }
        }
        private void Return()
        {
            EventAggregator.Publish(new ViewNavigationArgs(ManagerName.UserAuthorityView, new Dictionary<string, object>() { { "action", "return" }, { "refresh", IsRefresh } }));
        }

        private void Reset()
        {
            GetRegionList();
            string[] separator = { ", " };
            if (action.Equals("view"))
            {
                UserAuthorityClone();
                string regionCodes = CurrentUserAuthority.RegionsCode;
                switch (regionCodes)
                {
                    case "*":
                        break;
                    case "":
                    case null:
                        RegionCheckList = new List<RegionCheck>();
                        break;
                    default:
                        string[] temp = regionCodes.Split(separator, StringSplitOptions.None);
                        RegionCheckList = (from x in RegionCheckList where temp.Contains(x.Code) select x).ToList();
                        break;
                }

                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => RegionCheckList));
            }
            else
            {
                UserAuthorityClone();
                string regionCodes = CurrentUserAuthority.RegionsCode;
                switch (regionCodes)
                {
                    case "*":
                        IsChecked = true;
                        RegionCheckList.ForEach(x => x.Flag = true);
                        break;
                    case "":
                    case null:
                        IsChecked = false;
                        RegionCheckList.ForEach(x => x.Flag = false);
                        break;
                    default:
                        string[] temp = regionCodes.Split(separator, StringSplitOptions.None);
                        IsChecked = temp.Length == RegionCheckList.Count ? true : false;
                        temp.ToList().ForEach(x =>
                            {
                                RegionCheckList.ForEach(y =>
                                {
                                    if (y.Code == x)
                                    {
                                        y.Flag = true;
                                    }
                                });
                            });
                        break;
                }
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => IsChecked));
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => RegionCheckList));
            }
            LoginName = CurrentUserAuthority.LoginName;
            Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => LoginName));
        }

        private void GetRegionList()
        {
            RegionCheckList = new List<RegionCheck>();
            if (CurrentUserAuthority.UserType == UserAuthorityType.CountryLevel)
            {
                ApplicationContext.Instance.BufferManager.DistrictManager.DistrictByAuthority.Where(x => x.Code.Length == 2).OrderBy(x => x.Name).ToList().ForEach(x =>
                {
                    RegionCheck item = new RegionCheck();
                    item.Code = x.Code;
                    item.Name = x.Name;
                    RegionCheckList.Add(item);
                });
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => RegionCheckList));
            }
            else
            {
                ApplicationContext.Instance.BufferManager.DistrictManager.DistrictByAuthority.Where(x => x.Code.Length == 5 && x.Code.Substring(0, 2) == userProvinceCode).OrderBy(x => x.Name).ToList().ForEach(x =>
                {
                    RegionCheck item = new RegionCheck();
                    item.Code = x.Code;
                    item.Name = x.Name;
                    RegionCheckList.Add(item);
                });
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => RegionCheckList));
            }
        }
    }

    public class RegionCheck
    {
        /// <summary>
        /// AreaName
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// AreaCode
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// Flag
        /// </summary>
        public bool Flag { get; set; }
    }
}
