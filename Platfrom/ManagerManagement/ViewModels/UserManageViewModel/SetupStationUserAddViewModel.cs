/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 6fb23b46-690f-42d8-ad85-a0070d531977      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-WANGZS
/////                 Author: TEST(wangzs)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Manager.ViewModels
/////    Project Description:    
/////             Class Name: SetupStationUserAddViewModel
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/8/12 16:09:12
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/8/12 16:09:12
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using Gsafety.PTMS.Bases.Models;
using Gsafety.PTMS.Manager.Models;
using Gsafety.PTMS.ServiceReference.ADUserInfoService;
using Gsafety.PTMS.ServiceReference.DistrictService;
using Gsafety.PTMS.ServiceReference.InstallStationService;
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
    [ExportAsViewModel(ManagerName.SetupStationUserAddViewModel)]
    public class SetupStationUserAddViewModel : BaseEntityViewModel
    {
        #region Field
        bool isQuery = false;
        ADAccountServiceClient UserInforClient = ServiceClientFactory.Create<ADAccountServiceClient>();
        private InstallStationServiceClient installStationClient = ServiceClientFactory.Create<InstallStationServiceClient>();
        private DistrictServiceClient districtClient = ServiceClientFactory.Create<DistrictServiceClient>();

        private ADAccountInfo _ADAccountInfoModel;

        public ADAccountInfo ADAccountInfoModel
        {
            get { return _ADAccountInfoModel; }
            set { _ADAccountInfoModel = value; }
        }
        #endregion

        #region Property
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
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => CurrentGroup));
            }
        }

        private District currentCity;
        public District CurrentCity
        {
            get { return currentCity; }
            set
            {
                currentCity = value;
                ValidateCity();
                if (currentCity != null && !string.IsNullOrEmpty(currentCity.Code))
                {
                    GetSetupStationByCity(currentCity.Code);
                }
                if (currentCity != null && string.IsNullOrEmpty(currentCity.Code))
                {
                    SiteList = new List<InstallStation>();
                    SiteList.Add(new InstallStation { Name = ApplicationContext.Instance.StringResourceReader.GetString("BASEINFO_PleaseSelect"), ID = string.Empty });
                    Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => SiteList));
                    CurrentSite = SiteList[0];
                    Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => CurrentSite));
                }
                RaisePropertyChanged("CurrentCity");
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
                }
            }
            if (string.IsNullOrEmpty(ResetPassword))
            {
                SetError(prop, ApplicationContext.Instance.StringResourceReader.GetString("RequiredField"));

                return;
            }

        }
        private void ValidateCity()
        {
            var prop = ExtractPropertyName(() => CurrentCity);
            ClearErrors(prop);
            if (CurrentCity == null || string.IsNullOrEmpty(CurrentCity.Code))
            {
                SetError(prop, ApplicationContext.Instance.StringResourceReader.GetString("RequiredField"));
            }
        }

        private List<InstallStation> siteList;
        public List<InstallStation> SiteList
        {
            get
            {
                return siteList;
            }
            set
            {
                siteList = value;
                RaisePropertyChanged("SiteList");
            }
        }

        private List<InstallStation> relateSiteList;
        public List<InstallStation> RelateSiteList
        {
            get
            {
                return relateSiteList;
            }
            set
            {
                relateSiteList = value;
                RaisePropertyChanged("RelateSiteList");
            }
        }

        private InstallStation currentSite;
        public InstallStation CurrentSite
        {
            get
            {
                return currentSite;
            }
            set
            {
                currentSite = value;
                ValidateSite();
                RaisePropertyChanged("CurrentSite");
            }
        }

        private void ValidateSite()
        {
            var prop = ExtractPropertyName(() => CurrentSite);
            ClearErrors(prop);
            if (CurrentSite == null || string.IsNullOrEmpty(CurrentSite.ID))
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
        public ICommand ResetCommand { get; private set; }
        public ICommand ReturnCommand { get; private set; }
        public UserInfo CurrentUserInfo { get; set; }
        public bool IsModifiable
        {
            get;
            set;
        }
        #endregion
        #region Method
        public SetupStationUserAddViewModel()
        {
            CmbCityVisible = Visibility.Collapsed;
            CmbProvinceVisible = Visibility.Collapsed;

            ReturnCommand = new ActionCommand<object>(obj => Return());
            ResetCommand = new ActionCommand<object>(obj => Reset());

            UserInforClient.AddAccountCompleted += UserInforClient_AddAccountCompleted;


            UserInforClient.IsUserExitsCompleted += UserInforClient_IsUserExitsCompleted;
            districtClient.GetProvinceAndCityCompleted += districtClient_GetProvinceAndCityCompleted;
            installStationClient.GetInstallStationsCompleted += installStationClient_GetInstallStationsCompleted;

            GetAllRegions();

            Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => CurrentGroup));

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
                EventAggregator.Publish(new ViewNavigationArgs("SetupStationListView", new Dictionary<string, object>() { { "action", "refresh" } }));
            }
            else
            {
                MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("AddError" + e.Error.Message), ApplicationContext.Instance.StringResourceReader.GetString("TRAFFIC_TIP"), MessageBoxButton.OK);
                Reset();
            }
        }

        protected override void ActivateView(string viewName, System.Collections.Generic.IDictionary<string, object> viewParameters)
        {
            try
            {
                isQuery = false;
                Reset();
                CurrentGroup = new EnumModel { EnumName = viewParameters["groupname"].ToString(), ShowName = ApplicationContext.Instance.StringResourceReader.GetString(viewParameters["groupname"].ToString()) };
                GetAllSetupStation();
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(GetType().FullName, ex);
            }

        }

        public ObservableCollection<InstallStation> SiteModel { get; set; }

        void installStationClient_GetInstallStationsCompleted(object sender, GetInstallStationsCompletedEventArgs e)
        {
            SiteModel = new ObservableCollection<InstallStation>(e.Result.Result);

            SiteList = new List<InstallStation>();
            SiteList.Add(new InstallStation { Name = ApplicationContext.Instance.StringResourceReader.GetString("BASEINFO_PleaseSelect"), ID = string.Empty });
            if (CurrentCity != null && CurrentCity.Code != string.Empty)
            {
                //SiteList.AddRange(SiteModel.Where(x => x.CityCode == CurrentCity.Code).OrderBy(x => x.Name));
            }
            Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => SiteList));
            CurrentSite = SiteList[0];
            Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => CurrentSite));
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

        private void Reset()
        {
            try
            {
                ADAccountInfoModel = new ADAccountInfo();
                UserName = string.Empty;
                UserLoginName = string.Empty;
                UserPassword = string.Empty;
                ResetPassword = string.Empty;
                Email = string.Empty;
                Address = string.Empty;
                Phone = string.Empty;

                CurrentCity = new District();
                CurrentProvince = new District();
                CurrentSite = new InstallStation();
                if (ProvinceList != null && ProvinceList.Count > 0)
                {
                    CurrentProvince = ProvinceList[0];
                }
                if (CityList != null && CityList.Count > 0)
                {
                    CurrentCity = CityList[0];
                }
                if (SiteList != null && SiteList.Count > 0)
                {
                    CurrentSite = SiteList[0];
                }
                Description = string.Empty;
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(GetType().FullName, ex);
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

        private void Return()
        {
            try
            {
                EventAggregator.Publish(new ViewNavigationArgs("SetupStationListView", new Dictionary<string, object>() { { "action", "userinfoadd" }}));
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
                        RaisePropertyChanged(() => CurrentSite);
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
                    ADAccountInfoModel.Description = Description;
                    ADAccountInfoModel.Address = Address;
                    ADAccountInfoModel.Email = Email;
                    ADAccountInfoModel.Company = CurrentSite.ID;
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
                ValidateProvince();
                if (currentProvince != null && !string.IsNullOrEmpty(currentProvince.Code))
                {
                    CityList = GetCitiesByProvince(currentProvince.Code);
                }
                if (currentProvince != null && string.IsNullOrEmpty(currentProvince.Code))
                {
                    CityList = new List<District>();
                    CityList.Add(new District { Name = ApplicationContext.Instance.StringResourceReader.GetString("BASEINFO_PleaseSelect"), Code = string.Empty });
                    Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => CityList));
                    CurrentCity = CityList[0];
                    Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => CurrentCity));
                }
                RaisePropertyChanged("CurrentProvince");
            }
        }

        private void ValidateProvince()
        {
            var prop = ExtractPropertyName(() => CurrentProvince);
            ClearErrors(prop);
            if (CurrentProvince == null || string.IsNullOrEmpty(CurrentProvince.Code))
            {
                SetError(prop, ApplicationContext.Instance.StringResourceReader.GetString("RequiredField"));
            }
        }

        ObservableCollection<District> regionsList = new ObservableCollection<District>();

        private void GetAllRegions()
        {
            districtClient.GetProvinceAndCityAsync();
        }

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
            }
        }

        private void GetSetupStationByCity(string city)
        {
            if (SiteModel == null)
            {
                return;
            }
            SiteList = new List<InstallStation>();
            SiteList.Add(new InstallStation { Name = ApplicationContext.Instance.StringResourceReader.GetString("BASEINFO_PleaseSelect"), ID = string.Empty });
            if (string.IsNullOrEmpty(CurrentProvince.Code))
            {
                SiteList.AddRange(SiteModel.OrderBy(x => x.Name));
            }
            else if (string.IsNullOrEmpty(city))
            {

                //var result = from x in SiteModel where regionsList.Any(y => y.Code == x.CityCode && y.Code.Length == 5 && y.Code.StartsWith(CurrentProvince.Code)) select x;
                //SiteList.AddRange(result.OrderBy(x => x.Name));
            }
            else
            {
               // SiteList.AddRange(SiteModel.Where(x => x.CityCode == CurrentCity.Code).OrderBy(x => x.Name));
            }

            Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => SiteList));
            CurrentSite = SiteList[0];
            Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => CurrentSite));
        }

        private void GetAllSetupStation()
        {
            installStationClient.GetInstallStationsAsync(ApplicationContext.Instance.AuthenticationInfo.ClientID);
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
            }
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
            if (!InputValidate(UserPassword))
            {
                SetError(prop, ApplicationContext.Instance.StringResourceReader.GetString("PasswordIllegal"));
            }
            if (UserPassword.Length > 20 || UserPassword.Length < 7)
            {
                SetError(prop, ApplicationContext.Instance.StringResourceReader.GetString("PasswordAccordRule"));
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
            }
            else if (ResetPassword != UserPassword)
            {
                SetError(prop, ApplicationContext.Instance.StringResourceReader.GetString("NotUnified"));
                return;
            }
            else
            {
            }
            if (!InputValidate(ResetPassword))
            {
                SetError(prop, ApplicationContext.Instance.StringResourceReader.GetString("PasswordIllegal"));
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
                SetError(prop, ApplicationContext.Instance.StringResourceReader.GetString("IllegalFormat"));//
            }
            else
            {
                _ADAccountInfoModel.DisplayName = userName;
            }

        }
        #endregion

        #region UI Control
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
        #endregion
    }
}
