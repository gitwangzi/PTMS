/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: ab10a4bc-7822-4521-8b38-80b98de26687      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-WANGZS
/////                 Author: TEST(wangzs)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Manager.ViewModels
/////    Project Description:    
/////             Class Name: TrafficUserAddViewModel
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/8/12 16:11:01
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/8/12 16:11:01
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using Gsafety.PTMS.Bases.Models;
using Gsafety.PTMS.Manager.Models;
using Gsafety.PTMS.ServiceReference.ADUserInfoService;
using Gsafety.PTMS.ServiceReference.DistrictService;
using Gsafety.PTMS.Share;
using Jounce.Core.View;
using Jounce.Core.ViewModel;
using Jounce.Framework.Command;
using Jounce.Framework.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;

namespace Gsafety.PTMS.Manager.ViewModels
{
    [ExportAsViewModel(ManagerName.TrafficUserAddViewModel)]
    public class TrafficUserAddViewModel : BaseEntityViewModel
    {
        bool isRefresh = false;
        private string _title;
        public string Title
        {
            get { return _title; }
            set
            {
                _title = value;
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => Title));
            }
        }
        private ADAccountInfo _ADAccountInfoModel;

        public ADAccountInfo ADAccountInfoModel
        {
            get { return _ADAccountInfoModel; }
            set { _ADAccountInfoModel = value; }
        }

        private string userName;
        public string UserName
        {
            get { return userName; }
            set
            {
                userName = value == null ? null : value.Trim();
                _ValidateName();
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => UserName));
            }
        }

        private string userPassword;
        public string UserPassword
        {
            get { return userPassword; }
            set
            {
                userPassword = value;
                _ValidateUserpassword();
                ValidateNewPwd();
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => UserPassword));
            }
        }

        private void ValidateNewPwd()
        {
            var prop = ExtractPropertyName(() => ResetPassword);
            ClearErrors(prop);
            if (!string.IsNullOrEmpty(ResetPassword))
            {
                if (!ResetPassword.Equals(UserPassword))
                {
                    SetError(prop, ApplicationContext.Instance.StringResourceReader.GetString("NotUnified"));
                    return;
                }
            }
            if (string.IsNullOrEmpty(ResetPassword))
            {
                SetError(prop, ApplicationContext.Instance.StringResourceReader.GetString("RequiredField"));

                return;
            }

        }
        private string _Address;

        public string Address
        {
            get { return _Address; }
            set
            {
                _Address = value;
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => Address));
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
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => Email));
            }
        }

        private void ValidateEmail()
        {
            var prop = ExtractPropertyName(() => Email);
            ClearErrors(prop);
            Regex regex = new Regex("\\w{1,}@\\w{1,}\\.\\w{1,}", RegexOptions.IgnoreCase);
            if (!regex.Match(Email).Success && !string.IsNullOrEmpty(Email))
            {
                SetError(prop, ApplicationContext.Instance.StringResourceReader.GetString("EmailUnright"));
            }
        }
        private void ValidatePhone()
        {
            var prop = ExtractPropertyName(() => Phone);
            ClearErrors(prop);
            if (!string.IsNullOrEmpty(Phone) && !ManagerCommon.CheckTelephone(Phone))
            {
                SetError(prop, ApplicationContext.Instance.StringResourceReader.GetString("BASEINFO_PhoneRule"));
            }
        }
        private string resetPassword;
        public string ResetPassword
        {
            get { return resetPassword; }
            set
            {
                resetPassword = value;
                _ValidatePassword();
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => ResetPassword));
            }
        }

        private string _Phone;
        public string Phone
        {
            get { return _Phone; }
            set
            {
                _Phone = value;
                _ADAccountInfoModel.Phone = value;
                ValidatePhone();
                RaisePropertyChanged("Phone");
            }
        }



        private EnumModel _CurrentGroup;
        public EnumModel CurrentGroup
        {
            get
            {
                return _CurrentGroup;
            }
            set
            {
                _CurrentGroup = value;
                _ADAccountInfoModel.SecurityGroup = _CurrentGroup.EnumName;
                GroupChange();
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => CurrentGroup));
            }
        }

        private void GroupChange()
        {
            if (CurrentGroup.EnumName == UserGroup.SecurityAdmin)
            {
                LevelEnable = false;
                this.CmbCityVisible = Visibility.Collapsed;
                this.CmbProvinceVisible = Visibility.Collapsed;
                CurrentLevel = ApplicationContext.Instance.StringResourceReader.GetString("CountryLevel");
                ClearErrors(cityprop);
                ClearErrors(provinceprop);
            }
            else
            {
                LevelEnable = true;
            }
            Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => LevelEnable));
            Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => CmbCityVisible));
            Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => CmbProvinceVisible));
            Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => CurrentLevel));

        }

        private string _UserLoginName;

        public string UserLoginName
        {
            get { return _UserLoginName; }
            set
            {
                _UserLoginName = value;
                ValidateUserLogin();
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => UserLoginName));
            }
        }

        private void ValidateUserLogin()
        {
            var prop = ExtractPropertyName(() => UserLoginName);
            ClearErrors(prop);

            if (string.IsNullOrEmpty(UserLoginName))
            {
                SetError(prop, ApplicationContext.Instance.StringResourceReader.GetString("RequiredField"));
            }
            else if (!Regex.IsMatch(UserLoginName, @"^[a-zA-Z0-9_]{1,20}$", RegexOptions.IgnoreCase))
            {
                SetError(prop, ApplicationContext.Instance.StringResourceReader.GetString("IllegalFormat"));
            }
            else
            {
                _ADAccountInfoModel.UserName = UserLoginName;
            }
        }
        private List<string> _LevelList;
        public List<string> LevelList
        {
            get { return _LevelList; }
            set { _LevelList = value; }
        }
        private string _CurrentLevel;
        public string CurrentLevel
        {
            get { return _CurrentLevel; }
            set
            {
                _CurrentLevel = value;
                _ValidateCurrentLevel();
                LevelChange();
            }
        }

        private void _ValidateCurrentLevel()
        {
            var prop = ExtractPropertyName(() => CurrentLevel);
            ClearErrors(prop);

            if (string.IsNullOrEmpty(CurrentLevel))
            {
                SetError(prop, ApplicationContext.Instance.StringResourceReader.GetString("RequiredField"));

            }
        }


        private string description;
        public string Description
        {
            get { return description; }
            set
            {
                description = value;
                RaisePropertyChanged("Description");
            }
        }
        ADAccountServiceClient UserInforClient = ServiceClientFactory.Create<ADAccountServiceClient>();
        DistrictServiceClient districtClient = ServiceClientFactory.Create<DistrictServiceClient>();

        public ICommand ResetCommand { get; private set; }
        public ICommand ReturnCommand { get; private set; }
        public ICommand LevelGroup { get; private set; }
        public UserInfo CurrentUserInfo { get; set; }

        public bool IsModifiable { get; set; }
        public TrafficUserAddViewModel()
        {
            CmbCityVisible = Visibility.Collapsed;
            CmbProvinceVisible = Visibility.Collapsed;

            LevelList = new List<string>() { ApplicationContext.Instance.StringResourceReader.GetString("CountryLevel"), ApplicationContext.Instance.StringResourceReader.GetString("ProvinceLevel"), ApplicationContext.Instance.StringResourceReader.GetString("CityLevel") };
            ReturnCommand = new ActionCommand<object>(obj => Return());
            ResetCommand = new ActionCommand<object>(obj => Reset());
            UserInforClient.AddAccountCompleted += UserInforClient_AddAccountCompleted;


            UserInforClient.IsUserExitsCompleted += UserInforClient_IsUserExitsCompleted;


            districtClient.GetProvinceAndCityCompleted += districtClient_GetProvinceAndCityCompleted;
            districtClient.GetProvinceAndCityAsync();
            districtClient.AddUserAuthorityCompleted += districtClient_AddUserAuthorityCompleted;


            if (ApplicationContext.Instance.AuthenticationInfo.GroupName == UserGroup.SecurityAdmin)
            {
                IsModifiable = true;
            }
            else
            {
                IsModifiable = false;
            }
            Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => IsModifiable));
        }

        void districtClient_AddUserAuthorityCompleted(object sender, AddUserAuthorityCompletedEventArgs e)
        {

        }

        void districtClient_GetProvinceAndCityCompleted(object sender, GetProvinceAndCityCompletedEventArgs e)
        {
            regionsList = new ObservableCollection<District>(e.Result.Result);
            GetProvince();
        }

        private void GetProvince()
        {
            ProvinceList = new List<District>();
            ProvinceList.Add(new District { Name = ApplicationContext.Instance.StringResourceReader.GetString("BASEINFO_PleaseSelect"), Code = string.Empty });
            ProvinceList.AddRange(regionsList.Where(x => x.Code.Length == 2).ToList().OrderBy(x => x.Name));
            Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => ProvinceList));
            CurrentProvince = ProvinceList[0];
            Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => CurrentProvince));
        }

        void UserInforClient_AddAccountCompleted(object sender, AddAccountCompletedEventArgs e)
        {
            if (e.Result.Result == true)
            {
                isRefresh = true;
                Return();
            }
            else
            {
                MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("AddError") + e.Result.ExceptionMessage, ApplicationContext.Instance.StringResourceReader.GetString("TRAFFIC_TIP"), MessageBoxButton.OK);
                Reset();
            }
        }

        private void LevelChange()
        {
            if (CurrentLevel == ApplicationContext.Instance.StringResourceReader.GetString("CountryLevel") || string.IsNullOrEmpty(CurrentLevel))
            {
                this.CmbBothVisible = Visibility.Collapsed;
                this.CmbCityVisible = Visibility.Collapsed;
                this.CmbProvinceVisible = Visibility.Collapsed;
                ClearErrors(cityprop);
                ClearErrors(provinceprop);
            }
            else if (CurrentLevel == ApplicationContext.Instance.StringResourceReader.GetString("ProvinceLevel"))
            {
                CurrentProvince = ProvinceList[0];
                this.CmbBothVisible = Visibility.Visible;
                this.CmbCityVisible = Visibility.Collapsed;
                this.CmbProvinceVisible = Visibility.Visible;
                ClearErrors(cityprop);
            }
            else if (CurrentLevel == ApplicationContext.Instance.StringResourceReader.GetString("CityLevel"))
            {
                CurrentProvince = ProvinceList[0];
                this.CmbBothVisible = Visibility.Visible;
                this.CmbCityVisible = Visibility.Visible;
                this.CmbProvinceVisible = Visibility.Visible;
            }
            Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => CmbBothVisible));
            Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => CurrentProvince));
            Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => CmbCityVisible));
            Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => CmbProvinceVisible));
        }

        private bool CheckOnLegal()
        {
            if (string.IsNullOrEmpty(CurrentLevel))
                return false;
            if (CurrentLevel == ApplicationContext.Instance.StringResourceReader.GetString("CountryLevel"))
            {
                return true;
            }
            if (CurrentLevel == ApplicationContext.Instance.StringResourceReader.GetString("ProvinceLevel"))
            {
                return CurrentProvince != null;
            }
            if (CurrentLevel == ApplicationContext.Instance.StringResourceReader.GetString("CityLevel"))
            {
                return CurrentProvince != null && CurrentCity != null;
            }
            return false;
        }

        protected override void ActivateView(string viewName, System.Collections.Generic.IDictionary<string, object> viewParameters)
        {
            try
            {
                isRefresh = false;
                Reset();
                CurrentGroup = new EnumModel() { EnumName = viewParameters["groupname"].ToString(), ShowName = ApplicationContext.Instance.StringResourceReader.GetString(viewParameters["groupname"].ToString()) };
                Title = ApplicationContext.Instance.StringResourceReader.GetString("Add") + CurrentGroup.ShowName;
                if (CurrentGroup.EnumName.Equals(UserGroup.SecurityAdmin))
                {
                    LevelVisible = Visibility.Collapsed;
                }
                else if (CurrentGroup.EnumName.Equals(UserGroup.SecurityManager))
                {
                    LevelVisible = Visibility.Visible;
                }
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => CurrentGroup));

            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(GetType().FullName, ex);
            }

        }

        protected override void OnCommitted()
        {
            try
            {
                UserInforClient.IsUserExitsAsync(UserLoginName);
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(GetType().FullName, ex);
            }
        }


        #region UI Control
        private Visibility _CmbBothVisible;

        public Visibility CmbBothVisible
        {
            get { return _CmbBothVisible; }
            set
            {
                _CmbBothVisible = value;
                RaisePropertyChanged("CmbBothVisible");
            }
        }

        private Visibility _CmbProvinceVisible;

        public Visibility CmbProvinceVisible
        {
            get { return _CmbProvinceVisible; }
            set
            {
                _CmbProvinceVisible = value;
                RaisePropertyChanged("CmbProvinceVisible");
            }
        }
        private Visibility _LevelVisible;

        public Visibility LevelVisible
        {
            get { return _LevelVisible; }
            set
            {
                _LevelVisible = value;
                RaisePropertyChanged("LevelVisible");
            }
        }

        private Visibility _CmbCityVisible;

        public Visibility CmbCityVisible
        {
            get { return _CmbCityVisible; }
            set
            {
                _CmbCityVisible = value;
                RaisePropertyChanged("CmbCityVisible");
            }
        }
        private bool _levelEnable;

        public bool LevelEnable
        {
            get { return _levelEnable; }
            set { _levelEnable = value; }
        }
        #endregion

        #region InputValidate
        private bool InputValidate(string inputstr)
        {
            if (string.IsNullOrEmpty(inputstr))
            {
                return false;
            }
            int[] roleFlag = new int[4];

            foreach (var x in inputstr)
            {
                if (Char.IsDigit(x))
                {
                    roleFlag[0] = 1;
                }
                else if (Char.IsUpper(x))
                {
                    roleFlag[1] = 1;
                }
                else if (Char.IsLower(x))
                {
                    roleFlag[2] = 1;
                }
                else
                {
                    roleFlag[3] = 1;
                }

            }
            if (roleFlag.Sum() > 2)
            {
                return true;
            }
            else
            {
                return false;
            }

        }
        private void _ValidateUserpassword()
        {
            var prop = ExtractPropertyName(() => UserPassword);
            ClearErrors(prop);
            Regex regex = new Regex(@"^(?![0-9a-z]+$)(?![0-9A-Z]+$)(?![0-9\W]+$)(?![a-z\W]+$)(?![a-zA-Z]+$)(?![A-Z\W]+$)[a-zA-Z0-9\W_]+$", RegexOptions.IgnoreCase);
            if (string.IsNullOrEmpty(UserPassword))
            {
                SetError(prop, ApplicationContext.Instance.StringResourceReader.GetString("RequiredField"));
                return;
            }
            else
            {
                if (!string.IsNullOrEmpty(UserLoginName))
                {
                    if (UserPassword.ToUpper().Contains(UserLoginName.ToUpper()))
                    {
                        SetError(prop, ApplicationContext.Instance.StringResourceReader.GetString("PasswordLikeUserName"));
                        return;
                    }
                }
                else
                {
                    SetError("UserLoginName", ApplicationContext.Instance.StringResourceReader.GetString("RequiredField"));
                    return;
                }
            }
            if (!InputValidate(UserPassword))
            {
                SetError(prop, ApplicationContext.Instance.StringResourceReader.GetString("PasswordIllegal"));
                return;
            }
            if (UserPassword.Length > 20 || UserPassword.Length < 7)
            {
                SetError(prop, ApplicationContext.Instance.StringResourceReader.GetString("PasswordAccordRule"));
                return;
            }

        }

        private void _ValidatePassword()
        {
            var prop = ExtractPropertyName(() => ResetPassword);
            ClearErrors(prop);
            Regex regex = new Regex(@"^(?![0-9a-z]+$)(?![0-9A-Z]+$)(?![0-9\W]+$)(?![a-z\W]+$)(?![a-zA-Z]+$)(?![A-Z\W]+$)[a-zA-Z0-9\W_]+$", RegexOptions.IgnoreCase);
            if (string.IsNullOrEmpty(ResetPassword))
            {
                SetError(prop, ApplicationContext.Instance.StringResourceReader.GetString("RequiredField"));

                return;
            }
            if (ResetPassword.Length > 20 || ResetPassword.Length < 7)
            {
                SetError(prop, ApplicationContext.Instance.StringResourceReader.GetString("PasswordAccordRule"));
                return;
            }
            else if (ResetPassword != UserPassword)
            {
                SetError(prop, ApplicationContext.Instance.StringResourceReader.GetString("NotUnified"));

                return;
            }

            if (!InputValidate(ResetPassword))
            {
                SetError(prop, ApplicationContext.Instance.StringResourceReader.GetString("PasswordIllegal"));
                return;
            }
            else
            {
                _ADAccountInfoModel.UserPassword = UserPassword;

            }
        }

        private void _ValidateName()
        {
            var prop = ExtractPropertyName(() => UserName);
            ClearErrors(prop);

            if (string.IsNullOrEmpty(userName))
            {
                SetError(prop, ApplicationContext.Instance.StringResourceReader.GetString("RequiredField"));
            }
            else if (!Regex.IsMatch(userName, @"^[a-zA-Z0-9_]{1,20}$", RegexOptions.IgnoreCase))
            {
                SetError(prop, ApplicationContext.Instance.StringResourceReader.GetString("IllegalFormat"));
            }
            else
            {
                _ADAccountInfoModel.DisplayName = userName;
            }

        }
        #endregion

        private void Reset()
        {
            try
            {
                ADAccountInfoModel = new ADAccountInfo();
                UserName = string.Empty;
                UserLoginName = string.Empty;
                UserPassword = string.Empty;
                ResetPassword = string.Empty;
                Phone = string.Empty;
                Description = string.Empty;
                Email = string.Empty;
                Address = string.Empty;
                CurrentLevel = ApplicationContext.Instance.StringResourceReader.GetString("CountryLevel");
                CurrentCity = null;
                CurrentProvince = null;
                Email = string.Empty;
                Address = string.Empty;
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => CurrentLevel));

                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => CurrentProvince));
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(GetType().FullName, ex);
            }
        }

        private void Return()
        {
            try
            {
                string strViewName = null;

                Reset();
                if (CurrentGroup.EnumName == UserGroup.SecurityAdmin.ToString())
                {
                    strViewName = ManagerName.SysManagerUserListView;
                }
                else
                {
                    strViewName = "UserListView";
                }
                if (isRefresh)
                {
                    EventAggregator.Publish(new ViewNavigationArgs(strViewName, new Dictionary<string, object>() { { "action", "refresh" } }));
                }
                else
                {
                    EventAggregator.Publish(new ViewNavigationArgs(strViewName, new Dictionary<string, object>() { { "action", "userinfoadd" } }));
                }

            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(GetType().FullName, ex);
            }

        }
        private void CheckUserIsExit(string p)
        {
            try
            {
                UserInforClient.IsUserExitsAsync(p);
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(GetType().FullName, ex);
            }
        }

        void UserInforClient_IsUserExitsCompleted(object sender, IsUserExitsCompletedEventArgs e)
        {
            try
            {
                var prop = ExtractPropertyName(() => UserLoginName);
                ClearErrors(prop);
                if (e.Result.Result == true)
                {
                    SetError(prop, ApplicationContext.Instance.StringResourceReader.GetString("UserExist"));

                    Jounce.Framework.JounceHelper.ExecuteOnUI(() =>
                    {
                        RaisePropertyChanged(() => UserLoginName);
                        RaisePropertyChanged(() => UserName);
                        RaisePropertyChanged(() => UserPassword);
                        RaisePropertyChanged(() => ResetPassword);
                        RaisePropertyChanged(() => Phone);
                        RaisePropertyChanged(() => Address);
                        RaisePropertyChanged(() => Email);
                        RaisePropertyChanged(() => Description);
                        RaisePropertyChanged(() => CurrentLevel);
                        RaisePropertyChanged(() => CurrentCity);
                        RaisePropertyChanged(() => CurrentProvince);
                    });
                }
                else
                {
                    ADAccountInfoModel.UserName = UserLoginName;
                    ADAccountInfoModel.DisplayName = UserName;
                    ADAccountInfoModel.UserPassword = UserPassword;
                    ADAccountInfoModel.SecurityGroup = CurrentGroup.EnumName;
                    ADAccountInfoModel.Phone = Phone;
                    ADAccountInfoModel.Address = Address;
                    ADAccountInfoModel.Email = Email;
                    ADAccountInfoModel.Description = Description;
                    UserAuthority authdata = new UserAuthority();
                    authdata.LoginName = UserLoginName;
                    authdata.SecurityGroup = CurrentGroup.EnumName;

                    if (CurrentGroup.EnumName == UserGroup.SecurityManager || CurrentGroup.EnumName == UserGroup.SecurityAdmin)
                    {
                        if (CurrentLevel == ApplicationContext.Instance.StringResourceReader.GetString("CountryLevel"))
                        {
                            ADAccountInfoModel.Company = "0";
                            authdata.UserType = UserAuthorityType.CountryLevel;
                            authdata.RegionsCode = "*";
                        }
                        else if (CurrentLevel == ApplicationContext.Instance.StringResourceReader.GetString("ProvinceLevel"))
                        {
                            ADAccountInfoModel.Company = CurrentProvince.Code;
                            authdata.UserType = UserAuthorityType.ProvinceLevel;
                            authdata.RegionsCode = "*";
                        }
                        else if (CurrentLevel == ApplicationContext.Instance.StringResourceReader.GetString("CityLevel"))
                        {
                            ADAccountInfoModel.Company = CurrentCity.Code;
                            authdata.UserType = UserAuthorityType.CityLevel;
                            authdata.RegionsCode = CurrentCity.Code;
                        }
                    }

                    districtClient.AddUserAuthorityAsync(authdata);
                    UserInforClient.AddAccountAsync(ADAccountInfoModel);

                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(GetType().FullName, ex);
            }

        }
        private District currentProvince;
        public District CurrentProvince
        {
            get { return currentProvince; }
            set
            {
                currentProvince = value;

                provinceprop = ExtractPropertyName(() => CurrentProvince);
                ClearErrors(provinceprop);
                if (currentProvince != null && !string.IsNullOrEmpty(currentProvince.Code))
                {
                    CityList = GetCitiesByProvince(currentProvince.Code);
                }
                else if (currentProvince != null && string.IsNullOrEmpty(currentProvince.Code))
                {
                    CityList = new List<District>();
                    CityList.Add(new District { Name = ApplicationContext.Instance.StringResourceReader.GetString("BASEINFO_PleaseSelect"), Code = string.Empty });
                    CurrentCity = CityList[0];
                    Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => CurrentCity));
                }
                if (CurrentLevel != ApplicationContext.Instance.StringResourceReader.GetString("CountryLevel") && (currentProvince == null || currentProvince.Code == string.Empty))
                {
                    SetError(provinceprop, ApplicationContext.Instance.StringResourceReader.GetString("RequiredField"));
                }
            }
        }
        private string cityprop;
        private string provinceprop;
        private District currentCity;
        public District CurrentCity
        {
            get { return currentCity; }
            set
            {
                currentCity = value;
                cityprop = ExtractPropertyName(() => CurrentCity);
                ClearErrors(cityprop);
                if ((currentCity == null || currentCity.Code == string.Empty) && CurrentLevel != ApplicationContext.Instance.StringResourceReader.GetString("CountryLevel") && CurrentLevel != ApplicationContext.Instance.StringResourceReader.GetString("ProvinceLevel"))
                {
                    SetError(cityprop, ApplicationContext.Instance.StringResourceReader.GetString("RequiredField"));
                }

            }
        }
        ObservableCollection<District> regionsList = new ObservableCollection<District>();
        public List<District> GetCitiesByProvince(string province)
        {
            CityList = new List<District>();
            CityList.Add(new District { Name = ApplicationContext.Instance.StringResourceReader.GetString("BASEINFO_PleaseSelect"), Code = string.Empty });

            CityList.AddRange(regionsList.Where(x => x.Code.Length == 5 && x.Code.StartsWith(province)).ToList().OrderBy(x => x.Name));
            Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => CityList));
            CurrentCity = CityList[0];
            Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => CurrentCity));
            return CityList;
        }

        private List<District> provinceList;
        public List<District> ProvinceList
        {
            get
            {
                return provinceList;
            }
            set
            {
                provinceList = value;
                RaisePropertyChanged("ProvinceList");
                CurrentProvince = null;
            }
        }
        private List<District> cityList;
        public List<District> CityList
        {
            get
            {
                return cityList;
            }
            set
            {
                cityList = value;
                RaisePropertyChanged("CityList");
                CurrentCity = null;
            }
        }
    }
}
