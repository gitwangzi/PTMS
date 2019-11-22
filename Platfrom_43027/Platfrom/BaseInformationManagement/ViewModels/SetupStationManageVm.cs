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
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Linq;
using System.Text.RegularExpressions;

namespace Gsafety.PTMS.BaseInformation.ViewModels
{
    [ExportAsViewModel(BaseInformationName.SetupStationManageVm)]
    public class InstallStationManageVm : BaseInfoViewModelBase
    {
        private InstallStationServiceClient stationclient = ServiceClientFactory.Create<InstallStationServiceClient>();
        public bool ModifyCity { get; private set; }
        private InstallStation InitialInstallStation { get; set; }
        public InstallStation CurrentInstallStation { get; set; }

        //private string action;
        protected override void ActivateView(string viewName, System.Collections.Generic.IDictionary<string, object> viewParameters)
        {

            base.ActivateView(viewName, viewParameters);
            action = viewParameters["action"].ToString();

            switch (action)
            {
                case "view":
                    //modify the title
                    Title = ApplicationContext.Instance.StringResourceReader.GetString("BASEINFO_LookSetupStation");
                    Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => Title));

                    IsReadOnly = true;
                    Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => IsReadOnly));
                    KeyIsReadOnly = true;
                    ModifyCity = false;
                    Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => ModifyCity));
                    Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => KeyIsReadOnly));
                    IsView = Visibility.Visible;
                    Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => IsView));
                    InitialInstallStation = viewParameters["view"] as InstallStation;
                    break;
                case "update":
                    Title = ApplicationContext.Instance.StringResourceReader.GetString("BASEINFO_UpdateSetupStation");
                    Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => Title));

                    IsReadOnly = false;
                    Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => IsReadOnly));
                    KeyIsReadOnly = true;
                    Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => KeyIsReadOnly));
                    IsView = Visibility.Collapsed;
                    Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => IsView));
                    InitialInstallStation = viewParameters["update"] as InstallStation;
                    if (InitialInstallStation.Valid == 0)
                    {
                        ModifyCity = false;
                        Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => ModifyCity));
                    }
                    else if (InitialInstallStation.Valid == 1)
                    {
                        ModifyCity = true;
                        Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => ModifyCity));
                    }
                    break;
                case "add":
                    Title = ApplicationContext.Instance.StringResourceReader.GetString("BASEINFO_AddSetupStation");
                    Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => Title));

                    IsReadOnly = false;
                    Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => IsReadOnly));
                    KeyIsReadOnly = false;
                    ModifyCity = true;
                    Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => ModifyCity));
                    Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => KeyIsReadOnly));
                    IsView = Visibility.Collapsed;
                    Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => IsView));
                    break;
                default:
                    break;
            }
            Reset();
        }

        private void GetCurrentProvinceAndCity()
        {
            try
            {
                if (CurrentInstallStation != null)
                {
                    //string proviceCode = CurrentInstallStation.ProvinceCode;
                    //string cityCode = CurrentInstallStation.CityCode;
                    //CurrentProvince = ProvinceList.FirstOrDefault(x => x.Code == proviceCode);
                    //Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => CurrentProvince));
                    //CurrentCity = CityList.FirstOrDefault(x => x.Code == cityCode);
                    //Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => CurrentCity));
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("GetCurrentProvinceAndCity", ex);
            }
        }

        public InstallStationManageVm()
        {
            try
            {
                stationclient.CheckInstallStationExistByNameCompleted += installStationClient_CheckInstallStationExistByNameCompleted;
                stationclient.UpdateInstallStationCompleted += installStationClient_UpdateInstallStationCompleted;
                stationclient.AddInstallStationCompleted += InstallStationClient_AddInstallStationCompleted;
                GetAllDistrict();
                ReturnCommand = new ActionCommand<object>(obj => Return());
                ResetCommand = new ActionCommand<object>(obj => Reset());
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("InstallStationManageVm()", ex);
            }
        }

        protected override void OnCommitted()
        {
            CurrentInstallStation.Name = Name;
            CurrentInstallStation.Email = Email;
            CurrentInstallStation.ContactPhone = ContactPhone;
            CurrentInstallStation.DirectorPhone = DirectorPhone;
            if (CurrentCity != null)
            {
               // CurrentInstallStation.CityCode = CurrentCity.Code;
            }
            if (action.Equals("update"))
            {
                Update();
            }
            else
            {
                Add();
            }
        }

        protected override void Update()
        {
            stationclient.UpdateInstallStationAsync(CurrentInstallStation);
        }

        void installStationClient_UpdateInstallStationCompleted(object sender, UpdateInstallStationCompletedEventArgs e)
        {
            try
            {
                if (e.Result == null || !e.Result.IsSuccess)
                {
                    MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("BASEINFO_Operate_Failed"), ApplicationContext.Instance.StringResourceReader.GetString("TRAFFIC_TIP"), MessageBoxButton.OK);
                }
                else
                {
                    MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("BASEINFO_UpdateSucess"), ApplicationContext.Instance.StringResourceReader.GetString("TRAFFIC_TIP"), MessageBoxButton.OK);
                }
                Return();
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("installStationClient_UpdateInstallStationCompleted", ex);
            }
        }

        protected override void Add()
        {
            stationclient.CheckInstallStationExistByNameAsync(Name, ApplicationContext.Instance.AuthenticationInfo.ClientID);
        }

        void installStationClient_CheckInstallStationExistByNameCompleted(object sender, CheckInstallStationExistByNameCompletedEventArgs e)
        {
            try
            {
                var prop = ExtractPropertyName(() => Name);
                ClearErrors(prop);
                if (e.Result.IsSuccess && e.Result.Result)
                {
                    SetError(prop, ApplicationContext.Instance.StringResourceReader.GetString("BASEINFO_DataExisted"));//数据已存在
                    Jounce.Framework.JounceHelper.ExecuteOnUI(() =>
                    {
                        RaisePropertyChanged(() => CurrentProvince);
                        RaisePropertyChanged(() => CurrentCity);
                        RaisePropertyChanged(() => Name);
                        RaisePropertyChanged(() => Email);
                        RaisePropertyChanged(() => DirectorPhone);
                        RaisePropertyChanged(() => ContactPhone);
                    });
                }
                else
                {
                    stationclient.AddInstallStationAsync(CurrentInstallStation);
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("installStationClient_CheckInstallStationExistByNameCompleted", ex);
            }
        }

        void InstallStationClient_AddInstallStationCompleted(object sender, AddInstallStationCompletedEventArgs e)
        {
            try
            {
                if (e.Result == null || !e.Result.IsSuccess)
                {
                    MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("BASEINFO_Operate_Failed"), ApplicationContext.Instance.StringResourceReader.GetString("TRAFFIC_TIP"), MessageBoxButton.OK);
                    //Reset();
                }
                else
                {
                    MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("BASEINFO_AddSucess"), ApplicationContext.Instance.StringResourceReader.GetString("TRAFFIC_TIP"), MessageBoxButton.OK);
                    Reset();
                    Return();
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("InstallStationClient_AddInstallStationCompleted", ex);
            }
        }

        protected override void Reset()
        {
            switch (action)
            {
                case "view":
                case "update":
                    CurrentInstallStation = BaseInformationCommon.Clone(InitialInstallStation);
                    GetCurrentProvinceAndCity();
                    Name = CurrentInstallStation.Name;
                    Email = CurrentInstallStation.Email;
                    DirectorPhone = CurrentInstallStation.DirectorPhone;
                    ContactPhone = CurrentInstallStation.ContactPhone;
                    break;
                case "add":
                    CurrentInstallStation = new InstallStation();
                    CurrentProvince = ProvinceList[0];
                    CurrentCity = CityList[0];
                    Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => CurrentProvince));
                    Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => CurrentCity));
                    Name = string.Empty;
                    Email = string.Empty;
                    DirectorPhone = string.Empty;
                    ContactPhone = string.Empty;
                    break;
                default:
                    break;
            }

            Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => CurrentInstallStation));
            Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => Name));
            Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => Email));
            Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => DirectorPhone));
            Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => ContactPhone));
        }

        protected override void Return()
        {
            EventAggregator.Publish(new ViewNavigationArgs(BaseInformationName.SetupStationV, new Dictionary<string, object>() { { "action", "return" } }));
        }

        #region

        private string name;
        public string Name
        {
            get { return name; }
            set
            {
                name = value == null ? null : value.Trim();
                ValidateRequiredField(ExtractPropertyName(() => Name), name);
            }
        }

        private void ValidateRequiredField(string prop, string value)
        {
            ClearErrors(prop);
            if (string.IsNullOrEmpty(value))
            {
                SetError(prop, ApplicationContext.Instance.StringResourceReader.GetString("BASEINFO_MustFilled"));//必填字段
            }
        }

        private string email;
        public string Email
        {
            get { return email; }
            set
            {
                email = value == null ? null : value.Trim();
                ValidateEmail(ExtractPropertyName(() => Email), email);
            }
        }

        private void ValidateEmail(string prop, string value)
        {
            ClearErrors(prop);
            if (!string.IsNullOrEmpty(value) && !Regex.IsMatch(value, "\\w{1,}@\\w{1,}\\.\\w{1,}", RegexOptions.IgnoreCase))
            {
                SetError(prop, ApplicationContext.Instance.StringResourceReader.GetString("BASEINFO_FormatIllegal"));//格式非法
            }
        }

        private void ValidateDistrict(string prop, object value)
        {
            ClearErrors(prop);

            if (value == null || !(value is District) || ((District)value).Code == string.Empty)
            {
                SetError(prop, ApplicationContext.Instance.StringResourceReader.GetString("BASEINFO_MustFilled"));//必填字段
            }
        }

        private string directorPhone;
        public string DirectorPhone
        {
            get { return directorPhone; }
            set
            {
                directorPhone = value == null ? null : value.Trim();
                ValidatePhone(ExtractPropertyName(() => DirectorPhone), directorPhone);
            }
        }

        private string contactPhone;
        public string ContactPhone
        {
            get { return contactPhone; }
            set
            {
                contactPhone = value == null ? null : value.Trim();
                ValidatePhone(ExtractPropertyName(() => ContactPhone), contactPhone);
            }
        }

        private void ValidatePhone(string prop, string value)
        {
            ClearErrors(prop);
            if (!string.IsNullOrEmpty(value) && !BaseInformationCommon.CheckTelephone(value))
            {
                SetError(prop, ApplicationContext.Instance.StringResourceReader.GetString("BASEINFO_PhoneRule"));//格式非法
            }
        }
        #endregion

        #region

        public List<District> ProvinceList { get; set; }
        public List<District> CityList { get; set; }

        private void GetAllDistrict()
        {
            ProvinceList = new List<District>();
            ProvinceList.Add(new District { Name = ApplicationContext.Instance.StringResourceReader.GetString("BASEINFO_PleaseSelect"), Code = string.Empty });
            ProvinceList.AddRange(ApplicationContext.Instance.BufferManager.DistrictManager.DistrictByAuthority.Where(x => x.Code.Length == 2).ToList().OrderBy(x => x.Name));
            Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => ProvinceList));
            CurrentProvince = ProvinceList[0];
            Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => CurrentProvince));
        }

        private District currentProvince;
        public District CurrentProvince
        {
            get { return currentProvince; }
            set
            {
                currentProvince = value;
                if (currentProvince != null)
                {
                    GetCityByProvinceCode(currentProvince.Code);
                    ValidateDistrict(ExtractPropertyName(() => CurrentProvince), value);
                }
            }
        }

        public void GetCityByProvinceCode(string provinceCode)
        {
            CityList = new List<District>();
            CityList.Add(new District { Name = ApplicationContext.Instance.StringResourceReader.GetString("BASEINFO_PleaseSelect"), Code = string.Empty });
            if (provinceCode != string.Empty)
            {
                CityList.AddRange(ApplicationContext.Instance.BufferManager.DistrictManager.DistrictByAuthority.Where(x => x.Code.Length == 5 && x.Code.StartsWith(provinceCode)).ToList().OrderBy(x => x.Name));
            }
            Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => CityList));
            CurrentCity = CityList[0];
            Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => CurrentCity));
        }

        private District currentCity;
        public District CurrentCity
        {
            get { return currentCity; }
            set
            {
                currentCity = value;
                ValidateDistrict(ExtractPropertyName(() => CurrentCity), value);
            }
        }

        #endregion
    }
}
