/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 1c8fd935-9c5e-4fbd-9142-53d722b97127      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-WANGZS
/////                 Author: TEST(wangzs)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Manager.ViewModels
/////    Project Description:    
/////             Class Name: TrafficUserEditViewModel
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/8/12 16:11:17
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/8/12 16:11:17
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using Gsafety.PTMS.Bases.Models;
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
    [ExportAsViewModel(ManagerName.TrafficUserEditViewModel)]
    public class TrafficUserEditViewModel : BaseEntityViewModel
    {
        bool isQuery = false;
        private Gsafety.PTMS.ServiceReference.ADGroupService.ADAccountInfo _ADAccountInfoModel;

        public Gsafety.PTMS.ServiceReference.ADGroupService.ADAccountInfo ADAccountInfoModel
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

                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => UserName));
            }
        }
        /// <summary>
        /// go to page
        /// </summary>
        public string PreviousPage { get; set; }
        private string _Phone;
        public string Phone
        {
            get { return _Phone; }
            set
            {
                _Phone = value;
                ValidatePhone();
                RaisePropertyChanged("Phone");
            }
        }

        public List<EnumModel> GroupInfo { get; set; }
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

        private List<string> _LevelList;
        public List<string> LevelList
        {
            get { return _LevelList; }
            set
            {
                _LevelList = value;
                RaisePropertyChanged(() => LevelList);
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
                LevelChange();
                //  GetProvince();
            }
        }
        private District currentProvince;
        public District CurrentProvince
        {
            get { return currentProvince; }
            set
            {
                currentProvince = value;
                if (currentProvince != null && !string.IsNullOrEmpty(currentProvince.Code))
                {
                    GetCityByProvinceCode(currentProvince.Code);
                }
                if (currentProvince != null && string.IsNullOrEmpty(currentProvince.Code))
                {
                    CityList = new List<District>();
                    CityList.Add(new District { Name = ApplicationContext.Instance.StringResourceReader.GetString("BASEINFO_PleaseSelect"), Code = string.Empty });
                    Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => CityList));
                    CurrentCity = CityList[0];
                    Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => CurrentCity));
                }
                if (CmbProvinceVisible == Visibility.Visible)
                {
                    ValidateDistrict();
                }
            }
        }

        private void ValidateDistrict()
        {
            provinceprop = ExtractPropertyName(() => CurrentProvince);
            ClearErrors(provinceprop);

            if (CurrentProvince == null || !(CurrentProvince is District) || ((District)CurrentProvince).Code == string.Empty)
            {
                SetError(provinceprop, ApplicationContext.Instance.StringResourceReader.GetString("RequiredField"));
            }
        }
        private District currentCity;
        public District CurrentCity
        {
            get { return currentCity; }
            set
            {
                currentCity = value;
                if (CmbCityVisible == Visibility.Visible)
                {
                    Validatecity();
                }

                RaisePropertyChanged("CurrentCity");
            }
        }

        private void Validatecity()
        {
            cityprop = ExtractPropertyName(() => CurrentCity);
            ClearErrors(cityprop);
            if (CurrentCity == null || CurrentCity.Code == string.Empty)
            {
                SetError(cityprop, ApplicationContext.Instance.StringResourceReader.GetString("RequiredField"));
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
        public Gsafety.PTMS.ServiceReference.ADGroupService.ADAccountInfo CurrentUserInfo { get; set; }

        public TrafficUserEditViewModel()
        {
            CmbCityVisible = Visibility.Collapsed;
            CmbProvinceVisible = Visibility.Collapsed;

            LevelList = new List<string>() { ApplicationContext.Instance.StringResourceReader.GetString("CountryLevel"), ApplicationContext.Instance.StringResourceReader.GetString("ProvinceLevel"), ApplicationContext.Instance.StringResourceReader.GetString("CityLevel") };

            ReturnCommand = new ActionCommand<object>(obj => Return());
            ResetCommand = new ActionCommand<object>(obj => Reset());
            districtClient.GetProvinceAndCityCompleted += districtClient_GetProvinceAndCityCompleted;
            UserInforClient.UpdateAccountCompleted += UserInforClient_UpdateAccountCompleted;
            initpage();
        }

        void districtClient_GetProvinceAndCityCompleted(object sender, GetProvinceAndCityCompletedEventArgs e)
        {
            regionsList = new ObservableCollection<District>(e.Result.Result);
            GetProvince();
        }

        void UserInforClient_UpdateAccountCompleted(object sender, UpdateAccountCompletedEventArgs e)
        {
            if (e.Result.Result)
            {
                isQuery = false;

                MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("UpdatedSuccess"), ApplicationContext.Instance.StringResourceReader.GetString("Tip"), MessageBoxButton.OK);
                EventAggregator.Publish(new ViewNavigationArgs(PreviousPage, new Dictionary<string, object>() { { "action", "refresh" }, { "isreturn", isQuery } }));
            }
        }
        private void GetProvince()
        {
            ProvinceList = new List<District>();
            ProvinceList.Add(new District { Name = ApplicationContext.Instance.StringResourceReader.GetString("BASEINFO_PleaseSelect"), Code = string.Empty });
            ProvinceList.AddRange(regionsList.Where(x => x.Code.Length == 2).ToList().OrderBy(x => x.Name));
            Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => ProvinceList));
            CurrentProvince = ProvinceList[0];
            if (!string.IsNullOrEmpty(ProvinceCode))
            {
                CurrentProvince = ProvinceList.FirstOrDefault(x => x.Code == ProvinceCode);
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => CurrentProvince));

            }
            if (!string.IsNullOrEmpty(CityCode))
            {
                CurrentCity = CityList.FirstOrDefault(x => x.Code == CityCode);
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => CurrentCity));
            }
        }

        private void initpage()
        {
            GroupInfo = new List<EnumModel>();
            string x = UserGroup.SecurityManager;
            string y = UserGroup.SecurityAdmin;
            GroupInfo.Add(new EnumModel { EnumName = x, ShowName = ApplicationContext.Instance.StringResourceReader.GetString(x) });
            GroupInfo.Add(new EnumModel { EnumName = y, ShowName = ApplicationContext.Instance.StringResourceReader.GetString(y) });
            Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => GroupInfo));

        }

        private void LevelChange()
        {
            if (CurrentLevel == ApplicationContext.Instance.StringResourceReader.GetString("CountryLevel"))
            {
                ClearErrors(cityprop);
                ClearErrors(provinceprop);
                this.CmbCityVisible = Visibility.Collapsed;
                this.CmbProvinceVisible = Visibility.Collapsed;
                this.CmbBothVisible = Visibility.Collapsed;

            }
            else if (CurrentLevel == ApplicationContext.Instance.StringResourceReader.GetString("ProvinceLevel"))
            {
                this.CmbBothVisible = Visibility.Visible;
                this.CmbCityVisible = Visibility.Collapsed;
                this.CmbProvinceVisible = Visibility.Visible;
                if (ProvinceList != null)
                {
                    CurrentProvince = ProvinceList[0];
                }
                ClearErrors(cityprop);
            }
            else if (CurrentLevel == ApplicationContext.Instance.StringResourceReader.GetString("CityLevel"))
            {
                this.CmbBothVisible = Visibility.Visible;
                this.CmbCityVisible = Visibility.Visible;
                this.CmbProvinceVisible = Visibility.Visible;
                if (ProvinceList != null)
                {
                    CurrentProvince = ProvinceList[0];
                }
                Validatecity();
            }
            Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => CmbBothVisible));
            Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => CmbCityVisible));
            Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => CmbProvinceVisible));
            Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => CurrentProvince));
        }

        private Gsafety.PTMS.ServiceReference.ADGroupService.ADAccountInfo userInfo;

        private string originalLevel;

        public string OriginalLevel
        {
            get { return originalLevel; }
            set { originalLevel = value; }
        }

        private string originalProvince;
        public string OriginalProvince
        {
            get
            {
                return originalProvince;
            }
            set
            {
                originalProvince = value;
                RaisePropertyChanged("OriginalProvince");
            }
        }
        private string originalCity;
        public string OriginalCity
        {
            get
            {
                return originalCity;
            }
            set
            {
                originalCity = value;
                RaisePropertyChanged("OriginalCity");
            }
        }
        private string _OriginalPhone;

        public string OriginalPhone
        {
            get { return _OriginalPhone; }
            set { _OriginalPhone = value; }
        }
        private string _OriginNote;

        public string OriginNote
        {
            get { return _OriginNote; }
            set { _OriginNote = value; }
        }
        private bool _levelEnable;

        public bool LevelEnable
        {
            get { return _levelEnable; }
            set { _levelEnable = value; }
        }
        private string _LoginName;

        public string LoginName
        {
            get { return _LoginName; }
            set { _LoginName = value; }
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
        private string _OriginalEmail;

        public string OriginalEmail
        {
            get { return _OriginalEmail; }
            set { _OriginalEmail = value; }
        }
        private string _OriginalAddress;

        public string OriginalAddress
        {
            get { return _OriginalAddress; }
            set { _OriginalAddress = value; }
        }
        private string _OriginalUserName;
        /// <summary>
        /// OriginalUserName
        /// </summary>
        public string OriginalUserName
        {
            get { return _OriginalUserName; }
            set { _OriginalUserName = value; }
        }
        protected override void ActivateView(string viewName, System.Collections.Generic.IDictionary<string, object> viewParameters)
        {
            try
            {
                isQuery = false;
                userInfo = viewParameters["update"] as Gsafety.PTMS.ServiceReference.ADGroupService.ADAccountInfo; ;
                PreviousPage = viewParameters["currentpage"].ToString();

                CurrentGroup = GroupInfo.FirstOrDefault(x => x.EnumName == userInfo.SecurityGroup);
                LoginName = userInfo.UserLoginName;
                UserName = userInfo.UserName;
                OriginalUserName = UserName;
                Address = userInfo.Address;
                Email = userInfo.Email;
                OriginalEmail = Email;
                OriginalAddress = Address;
                Phone = userInfo.Phone;
                OriginalPhone = Phone;
                OriginNote = userInfo.Description;
                ProvinceCode = userInfo.ProvinceCode;
                CityCode = userInfo.CityCode;
                OriginalLevel = userInfo.Level.ToString();
                SetLevelChange(OriginalLevel);
                OriginalProvince = userInfo.ProvinceName;
                OriginalCity = userInfo.CityName;
                Description = userInfo.Description;
                if (UserName == ApplicationContext.Instance.AuthenticationInfo.UserName && CurrentGroup.EnumName != UserGroup.SecurityAdmin)
                {
                    LevelEnable = false;
                }
                if (CurrentGroup.EnumName == UserGroup.SecurityAdmin)
                {
                    LevelEnable = false;
                    ClearErrors(cityprop);
                    ClearErrors(provinceprop);
                }
                else
                {
                    LevelEnable = true;
                }
                districtClient.GetProvinceAndCityAsync();
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => LoginName));
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => LevelEnable));
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => CurrentGroup));
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(GetType().FullName, ex);
            }

        }

        private void SetLevelChange(string OriginalLevel)
        {
            if (OriginalLevel == "0")
            {
                CurrentLevel = LevelList[0];
            }
            if (OriginalLevel == "1")
            {
                CurrentLevel = LevelList[1];
            }
            if (OriginalLevel == "2")
            {
                CurrentLevel = LevelList[2];
            }
            Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => CurrentLevel));
        }

        protected override void OnCommitted()
        {
            try
            {

                Gsafety.PTMS.ServiceReference.ADUserInfoService.ADAccountInfo myinfo = new Gsafety.PTMS.ServiceReference.ADUserInfoService.ADAccountInfo();
                myinfo.UserLoginName = userInfo.UserLoginName;
                myinfo.UserName = userInfo.UserLoginName;
                myinfo.DisplayName = UserName;
                myinfo.Email = Email;
                myinfo.Address = Address;
                myinfo.Phone = Phone;
                myinfo.Level = LevelToInt(CurrentLevel);
                UserAuthority auth = new UserAuthority();
                auth.LoginName = userInfo.UserLoginName;
                auth.SecurityGroup = CurrentGroup.EnumName;
                auth.UserType = (UserAuthorityType)myinfo.Level;
                if (myinfo.Level == 0)
                {
                    myinfo.Company = "0";
                    auth.RegionsCode = "*";
                }
                if (myinfo.Level == 1)
                {
                    myinfo.Company = CurrentProvince.Code;
                    auth.UserType = UserAuthorityType.ProvinceLevel;
                    auth.RegionsCode = "*";
                }
                if (myinfo.Level == 2)
                {
                    myinfo.Company = CurrentCity.Code;
                    auth.RegionsCode = CurrentCity.Code;
                    auth.UserType = UserAuthorityType.CityLevel;
                }
                myinfo.SecurityGroup = CurrentGroup.EnumName;
                myinfo.Description = Description;
                if (!string.IsNullOrEmpty(myinfo.DisplayName))
                {
                    districtClient.UpdateUserAuthorityAsync(auth);
                    UserInforClient.UpdateAccountAsync(myinfo);
                }
                else
                    MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("PleaseInputUserName"), ApplicationContext.Instance.StringResourceReader.GetString("Tip"), MessageBoxButton.OK);


            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(GetType().FullName, ex);
            }
        }
        private int LevelToInt(string level)
        {
            return LevelList.IndexOf(level);
        }


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
        #endregion

        private void Reset()
        {
            try
            {
                ADAccountInfoModel = new Gsafety.PTMS.ServiceReference.ADGroupService.ADAccountInfo();
                CurrentLevel = OriginalLevel;
                SetLevelChange(OriginalLevel);
                if (!string.IsNullOrEmpty(OriginalProvince))
                {
                    CurrentProvince = ProvinceList.FirstOrDefault(x => x.Name == OriginalProvince);
                }
                if (!string.IsNullOrEmpty(OriginalCity))
                {
                    CurrentCity = CityList.FirstOrDefault(x => x.Name == OriginalCity);
                }
                Phone = OriginalPhone;
                Description = OriginNote;
                UserName = OriginalUserName;
                Email = OriginalEmail;
                Address = OriginalAddress;
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => CurrentProvince));
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => CurrentCity));
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

                EventAggregator.Publish(new ViewNavigationArgs(PreviousPage, new Dictionary<string, object>() { { "action", "userinfoadd" }, { "isreturn", isQuery } }));
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(GetType().FullName, ex);
            }

        }
        private void GetCityByProvinceCode(string p)
        {

            CityList = new List<District>();
            CityList.Add(new District { Name = ApplicationContext.Instance.StringResourceReader.GetString("BASEINFO_PleaseSelect"), Code = string.Empty });

            CityList.AddRange(regionsList.Where(x => x.Code.Length == 5 && x.Code.StartsWith(p)).ToList().OrderBy(x => x.Name));

            Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => CityList));
            CurrentCity = CityList[0];
            Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => CurrentCity));
        }

        ObservableCollection<District> regionsList = new ObservableCollection<District>();

        private void GetAllRegions()
        {
            districtClient.GetProvinceAndCityAsync();
        }

        private string _CityCode;

        public string CityCode
        {
            get { return _CityCode; }
            set { _CityCode = value; }
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
            }
        }
        private string _ProvinceCode;
        private string cityprop;
        private string provinceprop;
        public string ProvinceCode
        {
            get { return _ProvinceCode; }
            set { _ProvinceCode = value; }
        }
    }
}
