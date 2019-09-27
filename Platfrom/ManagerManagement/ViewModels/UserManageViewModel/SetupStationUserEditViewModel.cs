/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 717d7f62-5109-4133-bcbc-889d69511d81      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-WANGZS
/////                 Author: TEST(wangzs)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Manager.ViewModels
/////    Project Description:    
/////             Class Name: SetupStationUserEditViewModel
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/8/12 16:09:35
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/8/12 16:09:35
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
    [ExportAsViewModel(ManagerName.SetupStationUserEditViewModel)]
    public class SetupStationUserEditViewModel : BaseEntityViewModel
    {
        ADAccountServiceClient UserInforClient = ServiceClientFactory.Create<ADAccountServiceClient>();
        private InstallStationServiceClient installStationClient = ServiceClientFactory.Create<InstallStationServiceClient>();
        private DistrictServiceClient districtClient = ServiceClientFactory.Create<DistrictServiceClient>();
        public Gsafety.PTMS.ServiceReference.ADGroupService.ADAccountInfo UserInfor;

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
        private string _LoginName;

        public string LoginName
        {
            get { return _LoginName; }
            set
            {
                _LoginName = value;
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => LoginName));
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
                    GetSetupStationByCityCode(currentCity.Code);
                }
                if (currentCity != null && string.IsNullOrEmpty(currentCity.Code))
                {
                    SiteList = new List<InstallStation>();
                    SiteList.Add(new InstallStation { Name = ApplicationContext.Instance.StringResourceReader.GetString("BASEINFO_PleaseSelect"), ID = string.Empty });
                    Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => SiteList));
                    CurrentSite = SiteList[0];
                    Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => CurrentSite));
                }

            }
        }

        private void ValidateCity()
        {
            var prop = ExtractPropertyName(() => CurrentCity);
            ClearErrors(prop);
            if (CurrentCity == null || CurrentCity.Code == string.Empty)
            {
                SetError(prop, ApplicationContext.Instance.StringResourceReader.GetString("RequiredField"));
            }
        }
        public List<InstallStation> SiteList
        {
            get;
            set;
        }
        public List<InstallStation> AllSiteList
        {
            get;
            set;
        }
        private void GetSetupStationByCityCode(string cityCode)
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
            else if (string.IsNullOrEmpty(cityCode))
            {

                //var result = from x in SiteModel where regionsList.Any(y => y.Code == x.CityCode && y.Code.Length == 5 && y.Code.StartsWith(CurrentProvince.Code)) select x;
                //SiteList.AddRange(result.OrderBy(x => x.Name));
            }
            else
            {
               // SiteList.AddRange(SiteModel.Where(x => x.CityCode == CurrentCity.Code).OrderBy(x => x.Name));
            }

            Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => SiteList));

            if (!string.IsNullOrEmpty(SiteCode))
            {
                //CurrentSite = SiteList.FirstOrDefault(x => x.Id == SiteCode);
            }
            if (CurrentSite == null)
            {
                CurrentSite = SiteList[0];
            }
            Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => CurrentSite));
        }
        private InstallStation _CurrentSite;

        public InstallStation CurrentSite
        {
            get { return _CurrentSite; }
            set
            {

                _CurrentSite = value;
                ValidateSite();
            }
        }

        private void ValidateSite()
        {
            var prop = ExtractPropertyName(() => CurrentSite);
            ClearErrors(prop);
            if (CurrentSite == null || CurrentSite.ID == string.Empty)
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
        public ICommand ResetCommand { get; private set; }
        public ICommand ReturnCommand { get; private set; }

        public UserInfo CurrentUserInfo { get; set; }

        public SetupStationUserEditViewModel()
        {
            CmbCityVisible = Visibility.Collapsed;
            CmbProvinceVisible = Visibility.Collapsed;

            ReturnCommand = new ActionCommand<object>(obj => Return());
            ResetCommand = new ActionCommand<object>(obj => Reset());

            UserInforClient.UpdateAccountCompleted += UserInforClient_UpdateAccountCompleted;



            districtClient.GetProvinceAndCityCompleted += districtClient_GetProvinceAndCityCompleted;
            installStationClient.GetInstallStationsFuzzyCompleted += installStationClient_GetInstallStationsFuzzyCompleted;
            initpage();
        }

        void districtClient_GetProvinceAndCityCompleted(object sender, GetProvinceAndCityCompletedEventArgs e)
        {
            regionsList = new ObservableCollection<District>(e.Result.Result);
            GetProvice();
        }

        private void GetProvice()
        {
            ProvinceList = new List<District>();
            ProvinceList.Add(new District { Name = ApplicationContext.Instance.StringResourceReader.GetString("BASEINFO_PleaseSelect"), Code = string.Empty });
            ProvinceList.AddRange(regionsList.Where(x => x.Code.Length == 2).ToList().OrderBy(x => x.Name));
            Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => ProvinceList));
            if (!string.IsNullOrEmpty(ProvinceCode) && !string.IsNullOrEmpty(CityCode))
            {
                CurrentProvince = ProvinceList.FirstOrDefault(x => x.Code == ProvinceCode);
                CurrentCity = CityList.FirstOrDefault(x => x.Code == CityCode);
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => CurrentProvince));

                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => CurrentCity));

            }
        }

        void UserInforClient_UpdateAccountCompleted(object sender, UpdateAccountCompletedEventArgs e)
        {
            if (e.Result.Result)
            {
                EventAggregator.Publish(new ViewNavigationArgs("SetupStationListView", new Dictionary<string, object>() { { "action", "refresh" } }));
            }
        }
        private void initpage()
        {
            GroupInfo = new List<EnumModel>();
            string x = UserGroup.SiteManager;
            GroupInfo.Add(new EnumModel { EnumName = x, ShowName = ApplicationContext.Instance.StringResourceReader.GetString(x) });
            Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => GroupInfo));
        }
        private string _SiteCode;

        public string SiteCode
        {
            get { return _SiteCode; }
            set { _SiteCode = value; }
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

        private string originalCompany;
        public string OriginalCompany
        {
            get
            {
                return originalCompany;
            }
            set
            {
                originalCompany = value;
                RaisePropertyChanged("OriginalCompany");
            }
        }
        private string _OriginalSite;

        public string OriginalSite
        {
            get { return _OriginalSite; }
            set { _OriginalSite = value; }
        }
        private string _OriginalCityCode;

        public string OriginalCityCode
        {
            get { return _OriginalCityCode; }
            set { _OriginalCityCode = value; }
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
        private string _OrigiName;

        public string OrigiName
        {
            get { return _OrigiName; }
            set { _OrigiName = value; }
        }
        protected override void ActivateView(string viewName, System.Collections.Generic.IDictionary<string, object> viewParameters)
        {
            try
            {
                if (viewParameters["action"].Equals("update"))
                {
                    UserInfor = viewParameters["update"] as Gsafety.PTMS.ServiceReference.ADGroupService.ADAccountInfo;
                    LoginName = UserInfor.UserLoginName;
                    UserName = UserInfor.UserName;
                    Phone = UserInfor.Phone;
                    Address = UserInfor.Address;
                    Email = UserInfor.Email;
                    OrigiName = UserName;
                    OriginalEmail = Email;
                    OriginalAddress = Address;
                    OriginPhone = Phone;
                    CurrentGroup = GroupInfo.FirstOrDefault(x => x.EnumName == UserInfor.SecurityGroup);
                    OriginalProvince = UserInfor.ProvinceName;
                    OriginalCity = UserInfor.CityName;
                    OriginalCityCode = UserInfor.CityCode;
                    OriginalSite = UserInfor.OrgName;
                    Description = UserInfor.Description;
                    OriginDes = Description;
                    ProvinceCode = UserInfor.ProvinceCode;
                    CityCode = UserInfor.CityCode;
                    SiteCode = UserInfor.OrgCode;
                    districtClient.GetProvinceAndCityAsync();
                   // installStationClient.GetInstallStationsFuzzyAsync(null, null, null, ApplicationContext.Instance.AuthenticationInfo.ClientID);


                }

            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(GetType().FullName, ex);
            }

        }
        public ObservableCollection<InstallStation> SiteModel { get; set; }
        void installStationClient_GetInstallStationsFuzzyCompleted(object sender, GetInstallStationsFuzzyCompletedEventArgs e)
        {
            SiteModel = new ObservableCollection<InstallStation>(e.Result.Result);

            SiteList = new List<InstallStation>();
            SiteList.Add(new InstallStation { Name = ApplicationContext.Instance.StringResourceReader.GetString("BASEINFO_PleaseSelect"), ID = string.Empty });
            if (!string.IsNullOrEmpty(CityCode))
            {
               // SiteList.AddRange(SiteModel.Where(x => x.CityCode == CurrentCity.Code).OrderBy(x => x.Name));
            }
            Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => SiteList));
            CurrentSite = SiteList.FirstOrDefault(x => x.ID == SiteCode);
            Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => CurrentSite));
        }
        protected override void OnCommitted()
        {
            Gsafety.PTMS.ServiceReference.ADUserInfoService.ADAccountInfo myinfo = new Gsafety.PTMS.ServiceReference.ADUserInfoService.ADAccountInfo();
            myinfo.UserLoginName = UserInfor.UserLoginName;
            myinfo.UserName = UserInfor.UserLoginName;
            myinfo.DisplayName = UserName;
            myinfo.Company = CurrentSite.ID;
            myinfo.Email = Email;
            myinfo.Address = Address;
            myinfo.Phone = Phone;
            myinfo.Description = Description;
            if (!string.IsNullOrEmpty(myinfo.DisplayName))
            {
                UserInforClient.UpdateAccountAsync(myinfo);          
            }
            else
                MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("PleaseInputUserName"), ApplicationContext.Instance.StringResourceReader.GetString("Tip"), MessageBoxButton.OK);

        }
        private void Reset()
        {
            try
            {
                ADAccountInfoModel = new Gsafety.PTMS.ServiceReference.ADGroupService.ADAccountInfo();
                UserPassword = string.Empty;
                ResetPassword = string.Empty;
                Phone = OriginPhone;
                Email = OriginalEmail;
                Address = OriginalAddress;
                UserName = OrigiName;
                CurrentProvince = ProvinceList.FirstOrDefault(x => x.Name == OriginalProvince);
                CurrentCity = CityList.FirstOrDefault(x => x.Name == OriginalCity);

                CurrentSite = SiteList.FirstOrDefault(x => x.Name == OriginalSite);
                Description = OriginDes;
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => CurrentProvince));
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => CurrentCity));
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => CurrentSite));
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
                EventAggregator.Publish(new ViewNavigationArgs("SetupStationListView", new Dictionary<string, object>() { { "action", "userinfoadd" }}));
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(GetType().FullName, ex);
            }

        }

        #region Province


        private District currentProvince;
        public District CurrentProvince
        {
            get { return currentProvince; }
            set
            {
                currentProvince = value;
                ValidateProvice();
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
            }
        }

        private void ValidateProvice()
        {
            var prop = ExtractPropertyName(() => CurrentProvince);
            ClearErrors(prop);
            if (CurrentProvince == null || CurrentProvince.Code == string.Empty)
            {
                SetError(prop, ApplicationContext.Instance.StringResourceReader.GetString("RequiredField"));
            }
        }

        private void GetCityByProvinceCode(string p)
        {
            CityList = new List<District>();
            CityList.Add(new District { Name = ApplicationContext.Instance.StringResourceReader.GetString("BASEINFO_PleaseSelect"), Code = string.Empty });
            //regionsList.Where(x => x.Code.Length == 5 && x.Code.StartsWith(p)).ToList().ForEach(x => CityList.Add(x));
            CityList.AddRange(regionsList.Where(x => x.Code.Length == 5 && x.Code.StartsWith(p)).ToList().OrderBy(x => x.Name));
            Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => CityList));
            CurrentCity = CityList[0];
            Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => CurrentCity));

        }

        ObservableCollection<District> regionsList = new ObservableCollection<District>();


        private string _CityCode;

        public string CityCode
        {
            get { return _CityCode; }
            set { _CityCode = value; }
        }
        private string _ProvinceCode;

        public string ProvinceCode
        {
            get { return _ProvinceCode; }
            set { _ProvinceCode = value; }
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
        #endregion


        #region ControlButton
        private bool isFinishEnabled;
        public bool IsFinishEnabled
        {
            get { return isFinishEnabled; }
            set
            {
                isFinishEnabled = value;
                RaisePropertyChanged("IsFinishEnabled");
            }
        }

        private bool isFinishEnabledA;
        public bool IsFinishEnabledA
        {
            get
            {
                return isFinishEnabledA;
            }
            set
            {
                isFinishEnabledA = value;
                IsFinishEnabled = CheckIsFinished();
            }
        }
        private bool isFinishEnabledB;

        public bool IsFinishEnabledB
        {
            get { return isFinishEnabledB; }
            set
            {
                isFinishEnabledB = value;
                IsFinishEnabled = CheckIsFinished();
            }
        }
        private bool isFinishEnabledC;

        public bool IsFinishEnabledC
        {
            get { return isFinishEnabledC; }
            set
            {
                isFinishEnabledC = value;
                IsFinishEnabled = CheckIsFinished();
            }
        }
        private bool CheckIsFinished()
        {
            return IsFinishEnabledB && IsFinishEnabledA && IsFinishEnabledC;
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
        private string _OriginPhone;

        public string OriginPhone
        {
            get { return _OriginPhone; }
            set { _OriginPhone = value; }
        }
        private string _OriginDes;

        public string OriginDes
        {
            get { return _OriginDes; }
            set { _OriginDes = value; }
        }
        #endregion

        #region Input Validate
        private void _ValidateUserpassword()
        {
            var prop = ExtractPropertyName(() => UserPassword);
            ClearErrors(prop);
            if (string.IsNullOrEmpty(UserPassword))
            {
                IsFinishEnabledB = false;
            }
            else if (UserPassword.Length < 7)
            {
                SetError(prop, ApplicationContext.Instance.StringResourceReader.GetString("UserPasswordMoreThan6"));
                IsFinishEnabledB = false;
            }
            else
            {
                IsFinishEnabledB = true;
            }
            Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => IsFinishEnabledB));
        }

        private void _ValidatePassword()
        {
            var prop = ExtractPropertyName(() => ResetPassword);
            ClearErrors(prop);
            Regex regex = new Regex(@"^(?![0-9a-z]+$)(?![0-9A-Z]+$)(?![0-9\W]+$)(?![a-z\W]+$)(?![a-zA-Z]+$)(?![A-Z\W]+$)[a-zA-Z0-9\W_]+$", RegexOptions.IgnoreCase);

            if (string.IsNullOrEmpty(ResetPassword))
            {
                IsFinishEnabledC = false;
                return;
            }
            else if (ResetPassword != UserPassword)
            {
                SetError(prop, ApplicationContext.Instance.StringResourceReader.GetString("NotUnified"));

                IsFinishEnabledC = false;
                return;
            }
            else
            {
                IsFinishEnabledC = true;
            }
            if (!regex.IsMatch(ResetPassword))
            {
                SetError(prop, ApplicationContext.Instance.StringResourceReader.GetString("PasswordIllegal"));
                IsFinishEnabledC = false;
            }
            else
            {
                _ADAccountInfoModel.UserPassword = UserPassword;
                IsFinishEnabledC = true;
            }
            Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => IsFinishEnabledC));
        }

        private void _ValidateName()
        {
            var prop = ExtractPropertyName(() => UserName);
            ClearErrors(prop);

            if (string.IsNullOrEmpty(userName))
            {
                IsFinishEnabledA = false;
            }
            else if (userName.Length < 7)
            {
                IsFinishEnabledA = false;
            }
            else
            {
                _ADAccountInfoModel.UserName = userName;
                IsFinishEnabledA = true;
            }
            Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => IsFinishEnabledA));

        }
        #endregion
    }
}
