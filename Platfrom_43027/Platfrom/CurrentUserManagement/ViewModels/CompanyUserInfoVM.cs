using Gsafety.PTMS.ServiceReference.DistrictService;
using Gsafety.PTMS.Share;
using Jounce.Core.ViewModel;
using Jounce.Framework.ViewModel;
/////Copyright (C) Gsafety 2014 .All Rights Reserved.
/////======================================================================
/////                   Guid: b29a3f28-832b-4aa1-a5b4-bbdcb0aec167      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: LIN-20130409ZRS
/////                 Author: TEST(zhujf)
/////======================================================================
/////           Project Name: Gsafety.PTMS.CurrentUserManagement.ViewModels
/////    Project Description:    
/////             Class Name: CompanyUserInfoVM
/////          Class Version: v1.0.0.0
/////            Create Time: 2014/1/7 11:18:22
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2014/1/7 11:18:22
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

namespace Gsafety.PTMS.CurrentUserManagement.ViewModels
{
    [ExportAsViewModel(CurrentUserName.CompanyUserInfoModel)]
    public class CompanyUserInfoVM : BaseEntityViewModel
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
                ValidatePhone();
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
                Jounce.Framework.JounceHelper.ExecuteOnUI(()=>RaisePropertyChanged(()=>CurrentProvince));
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

        private string _CurrentCompany;
        public string CurrentCompany
        {
            get
            {
                return _CurrentCompany;
            }
            set
            {
                _CurrentCompany = value;
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => CurrentCompany));
            }
        }

        private string ProvinceCode;
        private string CityCode;
        private string CompanyCode;

        DistrictServiceClient districtClient = ServiceClientFactory.Create<DistrictServiceClient>();
        ObservableCollection<District> regionsList = new ObservableCollection<District>();

        ADAccountServiceClient UserInforClient = ServiceClientFactory.Create<ADAccountServiceClient>();

        public ICommand CommitCommand { get; private set; }


        public CompanyUserInfoVM()
        {
            CommitCommand = new ActionCommand<object>(obj => Commit());
            districtClient.GetProvinceAndCityCompleted += districtClient_GetProvinceAndCityCompleted;
            init();
        }

        private void  Commit()
        {
            Gsafety.PTMS.ServiceReference.ADUserInfoService.ADAccountInfo commitinfo = new Gsafety.PTMS.ServiceReference.ADUserInfoService.ADAccountInfo();
            commitinfo.UserName = LoginName;
            commitinfo.DisplayName = UserName;
            commitinfo.UserLoginName = LoginName;
            commitinfo.Address = Address;
            commitinfo.Email = Email;
            commitinfo.Phone = Phone;
            commitinfo.Description = ApplicationContext.Instance.AuthenticationInfo.Description;
            commitinfo.Company = ApplicationContext.Instance.AuthenticationInfo.OrgCode;
            UserInforClient.UpdateAccountAsync(commitinfo);
        }

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

        private void init()
        {
            LoginName = ApplicationContext.Instance.AuthenticationInfo.UserName;
            UserName = ApplicationContext.Instance.AuthenticationInfo.UserShowName;
            Address = ApplicationContext.Instance.AuthenticationInfo.Address;
            Email = ApplicationContext.Instance.AuthenticationInfo.Email;
            Phone = ApplicationContext.Instance.AuthenticationInfo.Phone;
            CurrentGroup = ApplicationContext.Instance.AuthenticationInfo.GroupName;
            ProvinceCode = ApplicationContext.Instance.AuthenticationInfo.Province;
            CityCode = ApplicationContext.Instance.AuthenticationInfo.City;
            CurrentCompany = ApplicationContext.Instance.AuthenticationInfo.OrgName;
            
            districtClient.GetProvinceAndCityAsync();
        }
        protected override void ActivateView(string viewName, System.Collections.Generic.IDictionary<string, object> viewParameters)
        {
            init();
        }

        private void ValidatePhone()
        {
            var prop = ExtractPropertyName(() => Phone);
            ClearErrors(prop);
            Regex regex = new Regex(@"((\d{11})|^((\d{7,9})|(\d{4}|\d{3})-(\d{7,9})|(\d{4}|\d{3})-(\d{7,9})-(\d{4}|\d{3}|\d{2}|\d{1})|(\d{7,9})-(\d{4}|\d{3}|\d{2}|\d{1}))$)", RegexOptions.IgnoreCase);
            if (!regex.Match(Phone).Success && !string.IsNullOrEmpty(Phone))
            {
                SetError(prop, ApplicationContext.Instance.StringResourceReader.GetString("PhoneUnright"));//必填字段
            }
        }
        private void ValidateEmail()
        {
            var prop = ExtractPropertyName(() => Email);
            ClearErrors(prop);
            Regex regex = new Regex(@"^([\d\w_]+[\d\w_.]*)@([0-9a-zA-Z_])+(.[\d\w_])*", RegexOptions.IgnoreCase);
            if (!regex.Match(Email).Success && !string.IsNullOrEmpty(Email))
            {
                SetError(prop, ApplicationContext.Instance.StringResourceReader.GetString("EmailUnright"));//必填字段
            }
        }
    }
}
