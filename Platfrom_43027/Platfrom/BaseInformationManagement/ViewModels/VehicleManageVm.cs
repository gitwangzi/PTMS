using Gsafety.PTMS.Bases.Models;
/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: bfe08123-b8f0-4110-9996-9e34d0778e88      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: LIN-20130409ZRS
/////                 Author: TEST(zhujf)
/////======================================================================
/////           Project Name: Gsafety.PTMS.BaseInformation.ViewModels
/////    Project Description:    
/////             Class Name: VehicleAddVm
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/8/17 11:17:05
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/8/17 11:17:05
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using Gsafety.PTMS.ServiceReference.DistrictService;
using Gsafety.PTMS.ServiceReference.VehicleService;
using Gsafety.PTMS.Share;
using Jounce.Core.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;

namespace Gsafety.PTMS.BaseInformation.ViewModels
{
    //[ExportAsViewModel(BaseInformationName.VehicleManageVm)]
    public class VehicleManageVm : BaseInfoViewModelBase
    {
        private VehicleServiceClient vehicleclient = ServiceClientFactory.Create<VehicleServiceClient>();
        private Gsafety.PTMS.ServiceReference.VehicleService.Vehicle InitialVehicle { get; set; }
        public Gsafety.PTMS.ServiceReference.VehicleService.Vehicle CurrentVehicle { get; set; }

        public EnumModel CurrentVehicleType { get; set; }
        public List<EnumModel> VehicleTypeList { get; set; }

        public EnumModel CurrentVehicleSeviceType { get; set; }
        public List<EnumModel> VehicleSeviceTypeList { get; set; }

        #region Property....

        private bool vehicleTypeSerive;
        public bool VehicleTypeSerive
        {
            get { return vehicleTypeSerive; }
            set
            {
                vehicleTypeSerive = value;
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => VehicleTypeSerive));
            }
        }

        private string provincetext;
        public string ProvinceText
        {
            get { return provincetext; }
            set
            {
                provincetext = value;
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => ProvinceText));
            }
        }

        private string citytext;
        public string CityText
        {
            get { return citytext; }
            set
            {
                citytext = value;
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => CityText));
            }
        }

        private void ValidateYear(string prop, string value)
        {
            ClearErrors(prop);
            if (!string.IsNullOrEmpty(value))
            {
                int year;
                bool ret = int.TryParse(value, out year);
                if (ret)
                {
                    if (year > 2050 || year < 1950)
                    {
                        SetError(prop, ApplicationContext.Instance.StringResourceReader.GetString("BASEINFO_StartYearRule"));
                    }
                }
                else
                    SetError(prop, ApplicationContext.Instance.StringResourceReader.GetString("BASEINFO_StartYearRule"));
            }
        }


        private string startyear;
        public string StartYear
        {
            get { return startyear; }
            set
            {
                startyear = value;
                ValidateYear(ExtractPropertyName(() => StartYear), startyear);
            }
        }

        private string vehicletypetext;
        public string VehicleTypeText
        {
            get { return vehicletypetext; }
            set
            {
                vehicletypetext = value;
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => VehicleTypeText));
            }
        }



        #endregion


        private void InitialVehicleType()
        {
            VehicleTypeList = new List<EnumModel>();

            //EnumModel m1 = new EnumModel();
            //m1.EnumName="卡车";
            //m1.EnumValue=1;
            //m1.ShowName="卡车";

            //EnumModel m2 = new EnumModel();
            //m2.EnumName="汽车";
            //m2.EnumValue=1;
            //m2.ShowName="汽车";

            //VehicleTypeList.Add(m1);
            //VehicleTypeList.Add(m2);

            //Enum.GetNames(typeof(Gsafety.PTMS.ServiceReference.VehicleService.VehicleType)).ToList().ForEach(x =>
            //{
            //    EnumModel item = new EnumModel { EnumName = x, ShowName = ApplicationContext.Instance.StringResourceReader.GetString(x) };
            //    VehicleTypeList.Add(item);
            //});

            //vehicleclient.

            //vehicleclient.GetVehicleTypeAsync(OwnerId);

        }

        private void InitialVehicleSeviceType()
        {
            //在此初始化车辆类型

            VehicleSeviceTypeList = new List<EnumModel>();
            //Enum.GetNames(typeof(Gsafety.PTMS.ServiceReference.VehicleService.VehicleSeviceType)).ToList().ForEach(x =>
            //{
            //    EnumModel item = new EnumModel { EnumName = x, ShowName = ApplicationContext.Instance.StringResourceReader.GetString(x) };
            //    VehicleSeviceTypeList.Add(item);
            //});

            EnumModel m1 = new EnumModel();
            m1.EnumName = "vip";
            m1.EnumValue = 0;
            m1.ShowName = "vip服务";

            EnumModel m2 = new EnumModel();
            m2.EnumName = "nomal";
            m2.EnumValue = 1;
            m2.ShowName = "普通服务";

            VehicleSeviceTypeList.Add(m1);
            VehicleSeviceTypeList.Add(m2);
        }

        //private string action;
        protected override void ActivateView(string viewName, System.Collections.Generic.IDictionary<string, object> viewParameters)
        {
            InitialVehicleType();
            InitialVehicleSeviceType();
            base.ActivateView(viewName, viewParameters);
            action = viewParameters["action"].ToString();
            switch (action)
            {
                case "view":
                    Title = ApplicationContext.Instance.StringResourceReader.GetString("BASEINFO_LookVehicle");
                    Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => Title));
                    IsReadOnly = true;
                    Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => IsReadOnly));
                    KeyIsReadOnly = true;
                    Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => KeyIsReadOnly));
                    IsView = Visibility.Collapsed;
                    VehicleTypeSerive = false;
                    Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => IsView));
                    IsNotView = Visibility.Visible;
                    Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => IsNotView));
                    InitialVehicle = viewParameters["view"] as Gsafety.PTMS.ServiceReference.VehicleService.Vehicle;
                    CurrentVehicle = BaseInformationCommon.Clone(InitialVehicle);
                    //VehicleTypeText = VehicleSeviceTypeList.FirstOrDefault(x => x.EnumName == CurrentVehicle.ServerType.ToString()).ShowName;
                    ProvinceText = ApplicationContext.Instance.BufferManager.DistrictManager.DistrictByAuthority.FirstOrDefault(x => x.Code.Length == 2 && x.Code.StartsWith(CurrentVehicle.CityCode.Substring(0, 2))).Name;
                    CityText = ApplicationContext.Instance.BufferManager.DistrictManager.DistrictByAuthority.FirstOrDefault(x => x.Code == CurrentVehicle.CityCode).Name;
                    break;
                case "update":
                    Title = ApplicationContext.Instance.StringResourceReader.GetString("BASEINFO_UpdateVehicle");
                    Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => Title));
                    IsReadOnly = false;
                    Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => IsReadOnly));
                    KeyIsReadOnly = true;
                    VehicleTypeSerive = true;
                    Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => KeyIsReadOnly));
                    IsNotView = Visibility.Collapsed;
                    Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => IsNotView));

                    IsView = Visibility.Visible;
                    Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => IsView));

                    InitialVehicle = viewParameters["update"] as Gsafety.PTMS.ServiceReference.VehicleService.Vehicle;
                    CurrentVehicle = BaseInformationCommon.Clone(InitialVehicle);
                    VehicleSn = CurrentVehicle.VehicleSn;
                    BrandModel = CurrentVehicle.BrandModel;
                    OperatingLicense = CurrentVehicle.OperationLicense;
                    break;
                case "add":
                    Title = ApplicationContext.Instance.StringResourceReader.GetString("BASEINFO_AddVehicle");
                    Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => Title));
                    IsReadOnly = false;
                    Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => IsReadOnly));
                    KeyIsReadOnly = false;
                    VehicleTypeSerive = true;
                    IsNotView = Visibility.Collapsed;
                    Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => IsNotView));
                    Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => KeyIsReadOnly));
                    IsView = Visibility.Visible;
                    Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => IsView));

                    break;
                default:
                    break;
            }

            Reset();
        }

        private int OperateCount;
        private bool isDataExisted;
        public VehicleManageVm()
        {
            try
            {
                vehicleclient.CheckVehicleExistByVehicleIdCompleted += VehicleClient_CheckVehicleExistByVehicleIdCompleted;
                vehicleclient.CheckVehicleExistByVehicleSnCompleted += VehicleClient_CheckVehicleExistByVehicleSnCompleted;
                vehicleclient.AddVehicleCompleted += VehicleClient_AddVehicleCompleted;
                vehicleclient.UpdateVehicleCompleted += VehicleClient_UpdateVehicleCompleted;
                // GetAllDistrict();
                InitialVehicleType();
                InitialVehicleSeviceType();
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("VehicleManageVm()", ex);
            }
        }

        protected override void OnCommitted()
        {
            //CurrentVehicle.ServerType = (Gsafety.PTMS.ServiceReference.VehicleService.VehicleSeviceType)
            //    Enum.Parse(typeof(Gsafety.PTMS.ServiceReference.VehicleService.VehicleSeviceType),
            //    CurrentVehicleSeviceType.EnumName,
            //    true);
            //CurrentVehicle.Type = (Gsafety.PTMS.ServiceReference.VehicleService.VehicleType)Enum.Parse(typeof(Gsafety.PTMS.ServiceReference.VehicleService.VehicleType),
            //    CurrentVehicleType.EnumName,
            //    true);

            //CurrentVehicle.VehicleId = VehicleId;
            //CurrentVehicle.OwnerEmail = OwnerEmail;
            //CurrentVehicle.Owner = Owner;
            //CurrentVehicle.OwnerId = OwnerId;
            //CurrentVehicle.OwnerPhone = OwnerPhone;
            //CurrentVehicle.StartYear = StartYear; 
            //CurrentVehicle.VehicleSn = vehicleSn;
            //CurrentVehicle.OperatingLicense = OperatingLicense;
            //CurrentVehicle.BrandModel = BrandModel;
            if (CurrentCity != null && CurrentCity.Code != string.Empty)
            {
                CurrentVehicle.CityCode = CurrentCity.Code;
            }

            OperateCount = 0;
            isDataExisted = false;
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
            if (InitialVehicle.VehicleId != VehicleId)
            {
                vehicleclient.CheckVehicleExistByVehicleIdAsync(VehicleId);
                OperateCount++;
            }
            if (InitialVehicle.VehicleSn != VehicleSn)
            {
                vehicleclient.CheckVehicleExistByVehicleSnAsync(VehicleSn);
                OperateCount++;
            }
            if (OperateCount == 0)
            {
                vehicleclient.UpdateVehicleAsync(CurrentVehicle);
            }
        }

        void VehicleClient_UpdateVehicleCompleted(object sender, UpdateVehicleCompletedEventArgs e)
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
                ApplicationContext.Instance.Logger.LogException("VehicleClient_UpdateVehicleCompleted", ex);
            }
        }

        protected override void Add()
        {
            OperateCount = 2;
            vehicleclient.CheckVehicleExistByVehicleIdAsync(VehicleId);
            vehicleclient.CheckVehicleExistByVehicleSnAsync(VehicleSn);
        }

        void VehicleClient_CheckVehicleExistByVehicleIdCompleted(object sender, CheckVehicleExistByVehicleIdCompletedEventArgs e)
        {
            var prop = ExtractPropertyName(() => VehicleId);
            ClearErrors(prop);

            try
            {
                if (e.Result.IsSuccess && e.Result.Result)
                {
                    isDataExisted = true;
                    SetError(prop, ApplicationContext.Instance.StringResourceReader.GetString("BASEINFO_DataExisted"));//数据已存在 
                    Jounce.Framework.JounceHelper.ExecuteOnUI(() =>
                    {
                        RaisePropertyChanged(() => CurrentProvince);
                        RaisePropertyChanged(() => CurrentCity);
                        RaisePropertyChanged(() => VehicleId);
                        RaisePropertyChanged(() => OwnerEmail);
                        RaisePropertyChanged(() => OwnerId);
                        RaisePropertyChanged(() => OwnerPhone);
                    });
                }
                else
                {
                    CurrentVehicle.InstallStatus = Gsafety.PTMS.ServiceReference.VehicleService.InstallStatusType.UnInstall;
                }
                System.Threading.Interlocked.Decrement(ref OperateCount);
                OperateData();
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("VehicleClient_CheckVehicleExistByVehicleIdCompleted", ex);
            }
        }

        private void OperateData()
        {
            if (OperateCount == 0 && isDataExisted == false)
            {
                switch (action)
                {
                    case "update":
                        vehicleclient.UpdateVehicleAsync(CurrentVehicle);
                        break;
                    case "add":
                        vehicleclient.AddVehicleAsync(CurrentVehicle);
                        break;
                    default:
                        break;
                }
            }
        }

        void VehicleClient_CheckVehicleExistByVehicleSnCompleted(object sender, CheckVehicleExistByVehicleSnCompletedEventArgs e)
        {
            var prop = ExtractPropertyName(() => VehicleSn);
            ClearErrors(prop);
            try
            {
                if (e.Result.IsSuccess && e.Result.Result)
                {
                    isDataExisted = true;
                    SetError(prop, ApplicationContext.Instance.StringResourceReader.GetString("BASEINFO_DataExisted"));//数据已存在
                    Jounce.Framework.JounceHelper.ExecuteOnUI(() =>
                    {
                        RaisePropertyChanged(() => CurrentProvince);
                        RaisePropertyChanged(() => CurrentCity);
                        RaisePropertyChanged(() => VehicleId);
                        RaisePropertyChanged(() => OwnerEmail);
                        RaisePropertyChanged(() => OwnerId);
                        RaisePropertyChanged(() => OwnerPhone);
                    });
                }
                else
                {
                    CurrentVehicle.InstallStatus = Gsafety.PTMS.ServiceReference.VehicleService.InstallStatusType.UnInstall;
                }
                System.Threading.Interlocked.Decrement(ref OperateCount);
                OperateData();
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("VehicleClient_CheckVehicleExistByVehicleSnCompleted", ex);
            }
        }

        void VehicleClient_AddVehicleCompleted(object sender, AddVehicleCompletedEventArgs e)
        {
            try
            {
                if (e.Result == null || !e.Result.IsSuccess)
                {
                    MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("BASEINFO_Operate_Failed"), ApplicationContext.Instance.StringResourceReader.GetString("TRAFFIC_TIP"), MessageBoxButton.OK);
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
                ApplicationContext.Instance.Logger.LogException("VehicleClient_AddVehicleCompleted", ex);
            }
        }

        protected override void Reset()
        {
            switch (action)
            {
                case "view":
                case "update":
                    CurrentVehicle = BaseInformationCommon.Clone(InitialVehicle);
                    GetCurrentDistrict(CurrentVehicle.CityCode);
                    //CurrentVehicleType = VehicleTypeList.FirstOrDefault(x => x.EnumName == CurrentVehicle.Type.ToString());
                    //CurrentVehicleSeviceType = VehicleSeviceTypeList.FirstOrDefault(x => x.EnumName == CurrentVehicle.ServerType.ToString());
                    if (!CurrentVehicle.DeleteFlag)
                    {
                        KeyIsReadOnly = true;
                        Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => KeyIsReadOnly));
                    }
                    break;
                case "add":
                    CurrentVehicle = new Gsafety.PTMS.ServiceReference.VehicleService.Vehicle();
                    CurrentVehicle.StartYear = DateTime.Now.Year.ToString();
                    CurrentVehicle.VehicleStatus = Gsafety.PTMS.ServiceReference.VehicleService.VehicleConditionType.Available;
                    // CurrentProvince = ProvinceList[0];
                    // CurrentCity = CityList[0];
                    Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => CurrentProvince));
                    Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => CurrentCity));
                    CurrentVehicleType = VehicleTypeList[1];
                    CurrentVehicleSeviceType = VehicleSeviceTypeList[0];
                    break;
                default:
                    break;
            }

            Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => CurrentVehicle));
            Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => CurrentVehicleSeviceType));
            Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => CurrentVehicleType));

            VehicleId = CurrentVehicle.VehicleId;
            OwnerEmail = CurrentVehicle.ContactEmail;
            OwnerId = CurrentVehicle.Contact;
            OwnerPhone = CurrentVehicle.ContactPhone;
            VehicleSn = CurrentVehicle.VehicleSn;
            BrandModel = CurrentVehicle.BrandModel;
            OperatingLicense = CurrentVehicle.OperationLicense;
            Owner = CurrentVehicle.Owner;
            StartYear = CurrentVehicle.StartYear;

            Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => StartYear));
            Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => VehicleId));
            Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => OwnerEmail));
            Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => OwnerId));
            Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => OwnerPhone));
            Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => VehicleSn));
            Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => BrandModel));
            Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => OperatingLicense));
            Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => Owner));
        }

        protected override void Return()
        {
            EventAggregator.Publish(new ViewNavigationArgs(BaseInformationName.VehicleV, new Dictionary<string, object>() { { "action", "return" } }));
        }

        private void GetCurrentDistrict(string districtCode)
        {
            try
            {
                if (!string.IsNullOrEmpty(districtCode))
                {
                    CurrentProvince = ApplicationContext.Instance.BufferManager.DistrictManager.DistrictByAuthority.FirstOrDefault(x => x.Code.Length == 2 && x.Code.StartsWith(districtCode.Substring(0, 2)));
                    Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => CurrentProvince));
                    CurrentCity = CityList.FirstOrDefault(x => x.Code == districtCode);
                    Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => CurrentCity));
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("GetCurrentDistrict()", ex);
            }
        }

        #region

        private string vehicleId;
        public string VehicleId
        {
            get { return vehicleId; }
            set
            {
                vehicleId = value == null ? null : value.Trim();
                ValidateVehicleId(ExtractPropertyName(() => VehicleId), vehicleId);
            }
        }

        private void ValidateVehicleId(string prop, string value)
        {
            ClearErrors(prop);
            if (string.IsNullOrEmpty(value))
            {
                SetError(prop, ApplicationContext.Instance.StringResourceReader.GetString("BASEINFO_MustFilled"));//必填字段
            }
        }

        private string vehicleSn;
        public string VehicleSn
        {
            get
            {
                return vehicleSn;
            }
            set
            {
                vehicleSn = value == null ? null : value.Trim();
                ValidateVehicleSn(ExtractPropertyName(() => VehicleSn), vehicleSn);

            }

        }
        private void ValidateVehicleSn(string prop, string value)
        {
            ClearErrors(prop);
            if (string.IsNullOrEmpty(value))
            {
                SetError(prop, ApplicationContext.Instance.StringResourceReader.GetString("BASEINFO_MustFilled"));
            }


        }
        private string brandModel;
        public string BrandModel
        {
            get
            {
                return brandModel;
            }
            set
            {
                brandModel = value == null ? null : value.Trim();
                ValidateBrandModel(ExtractPropertyName(() => BrandModel), brandModel);
            }

        }
        private void ValidateBrandModel(string prop, string value)
        {
            ClearErrors(prop);
            if (string.IsNullOrEmpty(value))
            {
                SetError(prop, ApplicationContext.Instance.StringResourceReader.GetString("BASEINFO_MustFilled"));
            }
        }
        private string operatingLicense;
        public string OperatingLicense
        {
            get
            {
                return operatingLicense;
            }
            set
            {
                operatingLicense = value == null ? null : value.Trim();
            }
        }

        private string owner;
        public string Owner
        {
            get { return owner; }
            set
            {
                owner = value == null ? null : value.Trim();
                ValidateOwner(ExtractPropertyName(() => Owner), owner);
            }
        }

        private void ValidateOwner(string prop, string value)
        {
            ClearErrors(prop);
            if (string.IsNullOrEmpty(value))
            {
                SetError(prop, ApplicationContext.Instance.StringResourceReader.GetString("BASEINFO_MustFilled"));
            }
        }

        private string ownerEmail;
        public string OwnerEmail
        {
            get { return ownerEmail; }
            set
            {
                ownerEmail = value == null ? null : value.Trim();
                ValidateEmail(ExtractPropertyName(() => OwnerEmail), ownerEmail);
            }
        }

        private string ownerPhone;
        public string OwnerPhone
        {
            get { return ownerPhone; }
            set
            {
                ownerPhone = value == null ? null : value.Trim();
                ValidatePhone(ExtractPropertyName(() => OwnerPhone), ownerPhone);
            }
        }
        private void ValidatePhone(string prop, string value)
        {
            ClearErrors(prop);
            if (string.IsNullOrEmpty(value))
            {
                SetError(prop, ApplicationContext.Instance.StringResourceReader.GetString("BASEINFO_MustFilled"));
            }
            if (!string.IsNullOrEmpty(value) && !BaseInformationCommon.CheckTelephone(value))
            {
                SetError(prop, ApplicationContext.Instance.StringResourceReader.GetString("BASEINFO_PhoneRule"));//格式非法
            }
        }

        private string ownerId;
        public string OwnerId
        {
            get { return ownerId; }
            set
            {
                ownerId = value == null ? null : value.Trim();
                ValidateIdentityID(ExtractPropertyName(() => OwnerId), ownerId);
            }
        }

        private void ValidateIdentityID(string prop, string value)
        {
            //目前不检查身份证的合法性
            //ClearErrors(prop);
            //if (!string.IsNullOrEmpty(value) && !BaseInformationCommon.CheckIdentity(value))
            //{
            //    SetError(prop, ApplicationContext.Instance.StringResourceReader.GetString("BASEINFO_FormatIllegal"));//格式非法
            //}
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
        #endregion


        #region property..2

        public List<District> ProvinceList { get; set; }
        public string ProviceCode { get; set; }
        public List<District> CityList { get; set; }
        public string CityCode { get; set; }

        private void GetAllDistrict()
        {
            ProvinceList = new List<District>();
            ProvinceList.Add(new District { Name = ApplicationContext.Instance.StringResourceReader.GetString("BASEINFO_PleaseSelect"), Code = string.Empty });
            ProvinceList.AddRange(ApplicationContext.Instance.BufferManager.DistrictManager.DistrictByAuthority.Where(x => x.Code.Length == 2).ToList().OrderBy(x => x.Name));
            District d1 = new District();
            d1.Name = "北京";
            District d2 = new District();
            d2.Name = "上海";
            ProvinceList.Add(d1);
            ProvinceList.Add(d2);

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
                ValidateDistrict(ExtractPropertyName(() => CurrentProvince), value);
                if (currentProvince != null)
                {
                    GetCityByProvinceCode(currentProvince.Code);
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
