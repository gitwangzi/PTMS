using Gsafety.PTMS.ServiceReference.DistrictService;
using Gsafety.PTMS.Share;
using Jounce.Core.ViewModel;
using Jounce.Framework.ViewModel;

/////Copyright (C) Gsafety 2014 .All Rights Reserved.
/////======================================================================
/////                   Guid: 760372ca-eb40-48ee-8a43-098553d35de0      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: LIN-20130409ZRS
/////                 Author: TEST(zhujf)
/////======================================================================
/////           Project Name: Gsafety.PTMS.CurrentUserManagement.ViewModels
/////    Project Description:    
/////             Class Name: TrafficUserInfoVM
/////          Class Version: v1.0.0.0
/////            Create Time: 2014/1/7 11:19:10
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2014/1/7 11:19:10
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System;
using System.Collections.ObjectModel;
using System.Net;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Linq;
using Jounce.Framework.Command;
using Gsafety.PTMS.ServiceReference.ADUserInfoService;
using System.Reflection;
using Gsafety.Common.Controls;

namespace Gsafety.PTMS.CurrentUserManagement.ViewModels
{
    [ExportAsViewModel(CurrentUserName.TrafficUserInfoModel)]
    public class TrafficUserInfoVM : BaseEntityViewModel
    {
        private string _LoginName;
        public string LoginName
        {
            get { return _LoginName; }
            set { _LoginName = value; Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => LoginName)); }
        }

        private string userName;
        public string UserName
        {
            get { return userName; }
            set
            {
                userName = value == null ? null : value.Trim();
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => UserName));
            }
        }

        private string _Phone;
        public string Phone
        {
            get { return _Phone; }
            set
            {
                _Phone = value;
                ValidatePhone(ExtractPropertyName(() => Phone), value);
                RaisePropertyChanged("Phone");
            }
        }
        private string _Address;
        public string Address
        {
            get { return _Address; }
            set
            {
                _Address = value;
                RaisePropertyChanged("Address");
            }
        }

        private string _Email;
        public string Email
        {
            get { return _Email; }
            set
            {
                _Email = value;
                ValidateEmail();
                RaisePropertyChanged("Email");
            }
        }

        private string _CurrentGroup;
        public string CurrentGroup
        {
            get
            {
                return _CurrentGroup;
            }
            set
            {
                _CurrentGroup = value;
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => CurrentGroup));
            }
        }

        private string _CurrentProvince;
        public string CurrentProvince
        {
            get
            {
                return _CurrentProvince;
            }
            set
            {
                _CurrentProvince = value;
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => CurrentProvince));
            }

        }

        private string _CurrentCity;
        public string CurrentCity
        {
            get
            {
                return _CurrentCity;
            }
            set
            {
                _CurrentCity = value;
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => CurrentCity));
            }

        }

        private string _CurrentLevel;
        public string CurrentLevel
        {
            get { return _CurrentLevel; }
            set
            {
                _CurrentLevel = value;
                RaisePropertyChanged(() => CurrentLevel);
            }
        }

        private string _Description;
        public string Description
        {
            get { return _Description; }
            set
            {
                _Description = value;
                RaisePropertyChanged(() => Description);
            }
        }

        private Visibility _IsProVisiual;
        public Visibility IsProVisiual
        {
            get { return _IsProVisiual; }
            set
            {
                _IsProVisiual = value;
                RaisePropertyChanged(() => IsProVisiual);
            }
        }

        private Visibility _IsProTextVisiual;
        public Visibility IsProTextVisiual
        {
            get { return _IsProTextVisiual; }
            set
            {
                _IsProTextVisiual = value;
                RaisePropertyChanged(() => IsProTextVisiual);
            }
        }
        private Visibility _IsCityVisiual;
        public Visibility IsCityVisiual
        {
            get { return _IsCityVisiual; }
            set
            {
                _IsCityVisiual = value;
                RaisePropertyChanged(() => IsProTextVisiual);
            }
        }

        private bool _FinishEnable;
        public bool FinishEnable
        {
            get
            {
                return _FinishEnable;
            }
            set
            {
                _FinishEnable = value;
                RaisePropertyChanged(() => FinishEnable);
            }
        }

        private Visibility _IsCityTextVisiual;
        public Visibility IsCityTextVisiual
        {
            get { return _IsCityTextVisiual; }
            set
            {
                _IsCityTextVisiual = value;
                RaisePropertyChanged(() => IsCityTextVisiual);
            }
        }
        private string ProvinceCode;
        private string CityCode;

        public ICommand CommitCommand { get; private set; }
        ADAccountServiceClient UserInforClient = null;
        private bool ValidatePhone(string prop, string value)
        {
            ClearErrors(prop);
            if (!string.IsNullOrEmpty(value) && !CheckTelephone(value))
            {
                SetError(prop, ApplicationContext.Instance.StringResourceReader.GetString("BASEINFO_PhoneRule"));//格式非法
                return false;
            }
            return true;
        }
        private bool CheckTelephone(string value)
        {
            try
            {
                int num = value.Length;
                if (num == 11)
                {
                    Int64.Parse(value);
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }
        //private void ValidatePhone()
        //{
        //    var prop = ExtractPropertyName(() => Phone);
        //    ClearErrors(prop);
        //    Regex regex = new Regex(@"((\d{11})|^((\d{7,9})|(\d{4}|\d{3})-(\d{7,9})|(\d{4}|\d{3})-(\d{7,9})-(\d{4}|\d{3}|\d{2}|\d{1})|(\d{7,9})-(\d{4}|\d{3}|\d{2}|\d{1}))$)", RegexOptions.IgnoreCase);
        //    if (!regex.Match(Phone).Success && !string.IsNullOrEmpty(Phone))
        //    {
        //        SetError(prop, ApplicationContext.Instance.StringResourceReader.GetString("PhoneUnright"));//必填字段
        //    }
        //}
        private bool ValidateEmail()
        {
            var prop = ExtractPropertyName(() => Email);
            ClearErrors(prop);
            Regex regex = new Regex(@"^([\d\w_]+[\d\w_.]*)@([0-9a-zA-Z_])+(.[\d\w_])*", RegexOptions.IgnoreCase);
            if (!regex.Match(Email).Success && !string.IsNullOrEmpty(Email))
            {
                SetError(prop, ApplicationContext.Instance.StringResourceReader.GetString("EmailUnright"));//必填字段
                return false;
            }
            else
                return true;
        }
        DistrictServiceClient districtClient = null;
        public TrafficUserInfoVM()
        {
            try
            {
                UserInforClient = ServiceClientFactory.Create<ADAccountServiceClient>();
                districtClient = ServiceClientFactory.Create<DistrictServiceClient>();
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
            CommitCommand = new ActionCommand<object>(obj => Commit());
            districtClient.GetProvinceAndCityCompleted += districtClient_GetProvinceAndCityCompleted;
            UserInforClient.UpdateAccountCompleted += UserInforClient_UpdateAccountCompleted;
            init();
        }

        void UserInforClient_UpdateAccountCompleted(object sender, UpdateAccountCompletedEventArgs e)
        {
            if (e.Result.Result)
            {
                ApplicationContext.Instance.AuthenticationInfo.Phone = Phone;
                ApplicationContext.Instance.AuthenticationInfo.Address = Address;
                ApplicationContext.Instance.AuthenticationInfo.Email = Email;
                MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString("CurrentUser_UpdateSuccess"), ApplicationContext.Instance.StringResourceReader.GetString("Tip"), MessageDialogButton.Ok);
            }
            else
            {
                MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString("Current_UpdataFaild"), ApplicationContext.Instance.StringResourceReader.GetString("Tip"), MessageDialogButton.Ok);
            }
        }
        private void init()
        {
            LoginName = ApplicationContext.Instance.AuthenticationInfo.UserName;
            UserName = ApplicationContext.Instance.AuthenticationInfo.UserShowName;
            Address = ApplicationContext.Instance.AuthenticationInfo.Address;
            Email = ApplicationContext.Instance.AuthenticationInfo.Email;
            Phone = ApplicationContext.Instance.AuthenticationInfo.Phone;
            Description = ApplicationContext.Instance.AuthenticationInfo.Description;
            CurrentGroup = ApplicationContext.Instance.AuthenticationInfo.GroupName;
            string level = ApplicationContext.Instance.AuthenticationInfo.Level;
            switch (level)
            {
                case "0":
                    CurrentLevel = ApplicationContext.Instance.StringResourceReader.GetString("CountryLevel");
                    IsProVisiual = Visibility.Collapsed;
                    IsProTextVisiual = Visibility.Collapsed;
                    IsCityVisiual = Visibility.Collapsed;
                    IsCityTextVisiual = Visibility.Collapsed;
                    break;
                case "1":
                    CurrentLevel = ApplicationContext.Instance.StringResourceReader.GetString("ProvinceLevel");
                    ProvinceCode = ApplicationContext.Instance.AuthenticationInfo.Province;
                    IsCityVisiual = Visibility.Collapsed;
                    IsCityTextVisiual = Visibility.Collapsed;
                    break;
                case "2":
                    CurrentLevel = ApplicationContext.Instance.StringResourceReader.GetString("CityLevel");
                    ProvinceCode = ApplicationContext.Instance.AuthenticationInfo.Province;
                    CityCode = ApplicationContext.Instance.AuthenticationInfo.City;
                    break;
            }
            districtClient.GetProvinceAndCityAsync();
        }
        private void Commit()
        {
            if (ValidatePhone(Phone, _Phone) && ValidateEmail())
            {
                Gsafety.PTMS.ServiceReference.ADUserInfoService.ADAccountInfo commitinfo = new Gsafety.PTMS.ServiceReference.ADUserInfoService.ADAccountInfo();
                UserAuthority auth = new UserAuthority();
                commitinfo.UserName = LoginName;
                commitinfo.DisplayName = UserName;
                commitinfo.UserLoginName = LoginName;
                commitinfo.Address = Address;
                commitinfo.Email = Email;
                commitinfo.Phone = Phone;
                commitinfo.Description = Description;
                int userLevel = int.Parse(ApplicationContext.Instance.AuthenticationInfo.Level);
                if (userLevel == 0)
                {
                    commitinfo.Company = "0";
                    auth.RegionsCode = "*";
                }
                if (userLevel == 1)
                {
                    commitinfo.Company = ProvinceCode;
                    auth.UserType = UserAuthorityType.ProvinceLevel;
                    auth.RegionsCode = "*";
                }
                if (userLevel == 2)
                {
                    commitinfo.Company = CityCode;
                    auth.RegionsCode = CityCode;
                    auth.UserType = UserAuthorityType.CityLevel;
                }
                commitinfo.SecurityGroup = CurrentGroup;
                //commitinfo.Description = "";
                districtClient.UpdateUserAuthorityAsync(auth);
                UserInforClient.UpdateAccountAsync(commitinfo);
            }
            else
            { 
              
            }
        }

        ObservableCollection<District> regionsList = new ObservableCollection<District>();
        void districtClient_GetProvinceAndCityCompleted(object sender, GetProvinceAndCityCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                return;
            }
            if (e.Result.Result != null)
            {
                regionsList = e.Result.Result;
                if (regionsList != null)
                {
                    if (!string.IsNullOrEmpty(ProvinceCode))
                    {
                        CurrentProvince = regionsList.Where(x => x.Code.Length == 2).ToList().FirstOrDefault(x => x.Code == ProvinceCode).Name;
                    }
                    if (!string.IsNullOrEmpty(CityCode))
                    {
                        CurrentCity = regionsList.Where(x => x.Code.Length == 5 && x.Code.StartsWith(ProvinceCode)).ToList().FirstOrDefault(x => x.Code == CityCode).Name;

                    }
                }
            }
        }
        protected override void ActivateView(string viewName, System.Collections.Generic.IDictionary<string, object> viewParameters)
        {
            init();
        }
    }
}
