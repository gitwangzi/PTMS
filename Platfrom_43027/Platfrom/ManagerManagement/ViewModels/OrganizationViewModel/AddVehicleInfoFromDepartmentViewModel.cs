using BaseLib.ViewModels;
using Jounce.Core.ViewModel;
using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Gsafety.PTMS.ServiceReference.VehicleService;
using Jounce.Framework;
using Jounce.Core.View;
using Gsafety.PTMS.Share;
using Gsafety.PTMS.Manager.Models;
using System.Collections.Generic;
using System.Linq;
using Gsafety.PTMS.ServiceReference.DistrictService;
using System.Threading.Tasks;
using System.Threading;
using Gsafety.Common.Controls;
using BaseLib.Model;

namespace Gsafety.PTMS.Manager.ViewModels.OrganizationViewModel
{
    [ExportAsViewModel(ManagerName.AddVehicleInfoFromDepartmentVm)]
    public class AddVehicleInfoFromDepartmentViewModel : DetailViewModel<Vehicle>
    {
        string notNull = ApplicationContext.Instance.StringResourceReader.GetString("NotNull");

        string have = ApplicationContext.Instance.StringResourceReader.GetString("ID_INSTALL_Yes");//具备
        string without = ApplicationContext.Instance.StringResourceReader.GetString("ID_INSTALL_No");//不具备
        //商用 公用 私有 未知
        string business = ApplicationContext.Instance.StringResourceReader.GetString("Comercial");
        string _public = ApplicationContext.Instance.StringResourceReader.GetString("Public");
        string _private = ApplicationContext.Instance.StringResourceReader.GetString("Private");
        string unKnwon = ApplicationContext.Instance.StringResourceReader.GetString("Unkown");

        VehicleServiceClient client = null;

        public AddVehicleInfoFromDepartmentViewModel()
        {
            client = ServiceClientFactory.Create<VehicleServiceClient>();
            client.AddVehicleCompleted += client_AddVehicleCompleted;
            client.GetBscVehicleTypeListCompleted += client_GetBscVehicleTypeListCompleted;
            client.UpdateVehicleCompleted += client_UpdateVehicleCompleted;

            InitProvinces();
            InitVehicleProperty();
            InitVehicleServiceType();
            InitVehicleStatus();
        }

        /// <summary>
        /// 更新完成
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void client_UpdateVehicleCompleted(object sender, UpdateVehicleCompletedEventArgs e)
        {
            OperationCompleted(e.Result.Result);
        }

        protected void InitProvinces()
        {
            var _provices = ApplicationContext.Instance.BufferManager.DistrictManager.DistrictByAuthority.Where(x => x.Code.Length == 2).ToList().OrderBy(x => x.Name);
            foreach (var item in _provices)
            {
                Provinces.Add(item);
            }
            JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => Provinces));
        }

        protected void InitCities(string provinceCode)
        {
            if (provinceCode != null)
            {
                Cities = new List<District>();
                var _cities = ApplicationContext.Instance.BufferManager.DistrictManager.DistrictByAuthority.Where(x => x.Code.Length == 5 && x.Code.Contains(provinceCode)).ToList().OrderBy(x => x.Name);
                foreach (var item in _cities)
                {
                    Cities.Add(item);
                }
                JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => Cities));
            }
        }


        protected string dataOperateType;
        protected string action;
        protected override void ActivateView(string viewName, System.Collections.Generic.IDictionary<string, object> viewParameters)
        {
            base.ActivateView(viewName, viewParameters);
            action = viewParameters["action"].ToString();

            switch (action)
            {
                case "view":
                    JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => Title));
                    IsReadOnly = true;
                    JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => IsReadOnly));
                    IsEnabled = false;
                    JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => IsEnabled));
                    InitialModel = viewParameters["data"] as Vehicle;
                    SaveButtonVisibility = Visibility.Collapsed;
                    ResertButtonVisibility = Visibility.Collapsed;
                    JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => SaveButtonVisibility));
                    JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => ResertButtonVisibility));
                    ViewVehicle();
                    break;

                case "update":
                    IsReadOnly = false;
                    JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => IsReadOnly));
                    IsEnabled = true;
                    JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => IsEnabled));
                    InitialModel = viewParameters["data"] as Vehicle;
                    dataOperateType = "update";
                    ViewVehicle();
                    break;

                case "add":
                    InitialModel = viewParameters["data"] as Vehicle;
                    IsReadOnly = false;
                    JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => IsReadOnly));
                    IsEnabled = true;
                    JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => IsEnabled));
                    this.OrgnizationName = InitialModel.OrgnizationName;
                    JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => OrgnizationName));
                    dataOperateType = "add";
                    break;

                default:
                    break;
            }
        }


        /// <summary>
        /// 自动填充车辆数据信息大文本框
        /// </summary>
        private void ViewVehicle()
        {
            if (InitialModel.DistrictCode.Length == 2)
            {
                Province = Provinces.FirstOrDefault(x => x.Code == InitialModel.DistrictCode);
            }

            this.OrgnizationName = InitialModel.OrgnizationName;
            JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => OrgnizationName));
            this.VehicleId = InitialModel.VehicleId;
            JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => VehicleId));
            this.VehicleSn = InitialModel.VehicleSn;
            JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => VehicleSn));
            this.EngineId = InitialModel.EngineId;
            JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => EngineId));
            this.Region = InitialModel.Region;
            JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => Region));
            this.OperationLicense = InitialModel.OperationLicense;
            JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => OperationLicense));
            this.Owner = InitialModel.Owner;
            JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => Owner));
            this.Contact = InitialModel.Contact;
            JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => Contact));
            this.ContactPhone = InitialModel.ContactPhone;
            JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => ContactPhone));
            this.ContactEmail = InitialModel.ContactEmail;
            JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => ContactEmail));
            this.ContactAddress = InitialModel.ContactAddress;
            JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => ContactAddress));
            this.Note = InitialModel.Note;
            JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => Note));
            this.BrandModel = InitialModel.BrandModel;
            JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => BrandModel));
            this.StartYear = InitialModel.StartYear;
            JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => StartYear));
            //-------------------------------------------------------------------

            VServiceType = ZVehicleServiceType.FirstOrDefault(x => x.Key == InitialModel.ServiceType);

            VStatus = ZVehicleStatus.FirstOrDefault(x => x.Key == InitialModel.VehicleStatus);

            VType = ZVehicleType.FirstOrDefault(x => x.Key == InitialModel.VehicleType);

            if (InitialModel.DistrictCode.Length == 5)
            {
                Province = Provinces.FirstOrDefault(x => x.Code == InitialModel.DistrictCode.Substring(0, 2));
                JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => Province));
                City = Cities.FirstOrDefault(x => x.Code == InitialModel.DistrictCode);
                JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => City));
            }

            //JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => ZVehicleServiceType));
            JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => VServiceType));
            //JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => ZVehicleStatus));
            JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => VStatus));
            //JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => ZVehicleType));
            JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => VType));
        }


        /// <summary>
        /// 增加新数据
        /// </summary>
        protected override void OnCommitted()
        {
            try
            {
                Vehicle v = new Vehicle();
                VehicleType type = new VehicleType();
                v.OrgnizationId = SelectedOrgStatus.OrgId;//非空
                v.ClientId = ApplicationContext.Instance.AuthenticationInfo.ClientID;//非空
                v.VehicleId = this.VehicleId;//非空

                v.VehicleSn = this.VehicleSn;//车架号
                v.EngineId = this.EngineId;//发动机号
                v.DistrictCode = City.Code;// == null ? Province.Key.Code : City.Key.Code;//所属区划
                v.BrandModel = this.BrandModel;
                v.OperationLicense = this.OperationLicense;//运行许可证
                v.Owner = this.Owner;//车主
                v.VehicleType = new VehicleType();
                v.VehicleType.ID = this.VType.Key.ID;//车辆类型表ID
                v.ContactPhone = this.ContactPhone;
                v.ContactEmail = this.ContactEmail;
                v.ContactAddress = this.ContactAddress;
                v.StartYear = this.StartYear;
                v.Region = this.Region;//运行区域               
                v.VehicleStatus = this.VStatus.Key;//车况   
                v.ServiceType = this.VServiceType.Key;// VehicleSeviceType.Public; //this.VServiceType;//服务类型
                v.Contact = this.Contact;//车主身份证号                
                v.Note = this.Note;

                v.Creator = ApplicationContext.Instance.AuthenticationInfo.Account;
                v.InstallStatus = InstallStatusType.UnInstall;

                if ("update".Equals(dataOperateType))
                {
                    client.UpdateVehicleAsync(v);
                }
                else
                {
                    v.Valid = 1;
                    v.CreateTime = DateTime.UtcNow;
                    client.AddVehicleAsync(v);
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("OnCommittedMethod", ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void client_AddVehicleCompleted(object sender, AddVehicleCompletedEventArgs e)
        {
            OperationCompleted(e.Result.Result);
        }

        /// <summary>
        /// 重置
        /// </summary>
        protected override void Reset()
        {
            this.VehicleId = string.Empty;
            JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => VehicleId));
            this.EngineId = string.Empty;
            JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => EngineId));
            this.Region = string.Empty;
            JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => Region));
            this.OperationLicense = string.Empty;
            JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => OperationLicense));
            this.Owner = string.Empty;
            JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => Owner));
            this.Contact = string.Empty;
            JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => Contact));
            this.ContactPhone = string.Empty;
            JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => ContactPhone));
            this.ContactEmail = string.Empty;
            JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => ContactEmail));
            this.ContactAddress = string.Empty;
            JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => ContactAddress));
            this.Note = string.Empty;
            JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => Note));
        }
        /// <summary>
        /// 返回
        /// </summary>
        protected override void Return()
        {
            EventAggregator.Publish(new ViewNavigationArgs(ManagerName.VehicleDepartmentListV, new System.Collections.Generic.Dictionary<string, object>() { { "action", "return" } }));
        }


        /// <summary>
        /// 初始化车辆所属类型
        /// </summary>
        void InitVehicleProperty()
        {
            try
            {
                client.GetBscVehicleTypeListAsync();
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("InitVehicleProperty", ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void client_GetBscVehicleTypeListCompleted(object sender, GetBscVehicleTypeListCompletedEventArgs e)
        {
            foreach (var item in e.Result.Result)
            {
                ComboBoxBasicStruct<VehicleType> cb = new ComboBoxBasicStruct<VehicleType>();
                cb.Key = item;
                cb.Value = item.Name;
                ZVehicleType.Add(cb);
            }
            Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => ZVehicleType));
        }

        /// <summary>
        /// 初始化车况
        /// </summary>
        void InitVehicleStatus()
        {
            try
            {
                ZVehicleStatus.Add(new ComboBoxBasicStruct<VehicleConditionType>()
                {
                    Key = VehicleConditionType.Available,
                    Value = have
                });
                ZVehicleStatus.Add(new ComboBoxBasicStruct<VehicleConditionType>()
                {
                    Key = VehicleConditionType.Unavailable,
                    Value = without
                });
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("InitVehicleStatus", ex);
            }
        }

        /// <summary>
        /// 初始化车辆服务类型
        /// </summary>
        void InitVehicleServiceType()
        {
            try
            {
                ZVehicleServiceType.Add(new ComboBoxBasicStruct<VehicleSeviceType>()
                {
                    Key = VehicleSeviceType.Comercial,
                    Value = business
                });
                ZVehicleServiceType.Add(new ComboBoxBasicStruct<VehicleSeviceType>()
                {
                    Key = VehicleSeviceType.Public,
                    Value = _public
                });
                ZVehicleServiceType.Add(new ComboBoxBasicStruct<VehicleSeviceType>()
                {
                    Key = VehicleSeviceType.Private,
                    Value = _private
                });
                ZVehicleServiceType.Add(new ComboBoxBasicStruct<VehicleSeviceType>()
                {
                    Key = VehicleSeviceType.Unknown,
                    Value = unKnwon
                });
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("InitVehicleServiceType", ex);
            }
        }


        private void OperationCompleted(bool mark)
        {
            if (mark)
            {
                MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString("Tip"), ApplicationContext.Instance.StringResourceReader.GetString("OperatedSuccessed"), MessageDialogButton.Ok);
            }
            else
            {
                MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString("Tip"), ApplicationContext.Instance.StringResourceReader.GetString("BASEINFO_Operate_Failed"), MessageDialogButton.Ok);
            }
        }


        #region ComboBox List


        private ComboBoxBasicStruct<VehicleConditionType> vStatus;
        /// <summary>
        /// 选中的车辆情况
        /// </summary>
        public ComboBoxBasicStruct<VehicleConditionType> VStatus
        {
            get { return vStatus; }
            set
            {
                vStatus = value;
                RaisePropertyChanged(() => VStatus);
            }
        }
        private List<ComboBoxBasicStruct<VehicleConditionType>> _vehiclestatus = new List<ComboBoxBasicStruct<VehicleConditionType>>();
        /// <summary>
        /// 车辆情况
        /// </summary>
        public List<ComboBoxBasicStruct<VehicleConditionType>> ZVehicleStatus
        {
            get { return _vehiclestatus; }
            set
            {
                _vehiclestatus = value;
                RaisePropertyChanged(() => ZVehicleStatus);
            }
        }

        private ComboBoxBasicStruct<VehicleType> vType;
        /// <summary>
        /// 选中的车辆类型
        /// </summary>
        public ComboBoxBasicStruct<VehicleType> VType
        {
            get { return vType; }
            set
            {
                vType = value;
                RaisePropertyChanged(() => VType);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private List<ComboBoxBasicStruct<VehicleType>> _vehicletype = new List<ComboBoxBasicStruct<VehicleType>>();

        /// <summary>
        /// 车辆类型
        /// </summary>
        public List<ComboBoxBasicStruct<VehicleType>> ZVehicleType
        {
            get { return _vehicletype; }
            set
            {
                _vehicletype = value;
                RaisePropertyChanged(() => ZVehicleType);
            }
        }

        private ComboBoxBasicStruct<VehicleSeviceType> vServiceType = new ComboBoxBasicStruct<VehicleSeviceType>();
        /// <summary>
        /// 选中的车辆类型
        /// </summary>
        public ComboBoxBasicStruct<VehicleSeviceType> VServiceType
        {
            get { return vServiceType; }
            set
            {
                vServiceType = value;
                RaisePropertyChanged(() => VServiceType);
            }
        }

        /// <summary>
        /// 服务类型
        /// </summary>
        private List<ComboBoxBasicStruct<VehicleSeviceType>> _vehicleServiceType = new List<ComboBoxBasicStruct<VehicleSeviceType>>();
        /// <summary>
        /// 服务类型
        /// </summary>
        public List<ComboBoxBasicStruct<VehicleSeviceType>> ZVehicleServiceType
        {
            get { return _vehicleServiceType; }
            set
            {
                _vehicleServiceType = value;
                RaisePropertyChanged(() => ZVehicleServiceType);
            }
        }

        private District province;
        /// <summary>
        /// 省级区划
        /// </summary>
        public District Province
        {
            get { return province; }
            set
            {
                province = value;
                RaisePropertyChanged(() => Province);

                if (province != null)
                {
                    InitCities(province.Code);
                }
            }
        }
        private List<District> provinces = new List<District>();
        /// <summary>
        /// 所有省份
        /// </summary>
        public List<District> Provinces
        {
            get { return provinces; }
            set
            {
                provinces = value;
                RaisePropertyChanged(() => Provinces);
            }
        }


        private District city;
        /// <summary>
        /// 市级区划
        /// </summary>
        public District City
        {
            get { return city; }
            set
            {
                city = value;
                try
                {
                    RaisePropertyChanged(() => City);
                }
                catch (Exception ex)
                {
                    ApplicationContext.Instance.Logger.LogException("CityProperty", ex);
                }
            }
        }
        private List<District> cities = new List<District>();
        /// <summary>
        /// 所有市列表
        /// </summary>
        public List<District> Cities
        {
            get { return cities; }
            set
            {
                cities = value;
                RaisePropertyChanged(() => Cities);
            }
        }

        #endregion

        #region VehicleFieldValid

        /// <summary>
        /// 
        /// </summary>
        /// <param name="prop"></param>
        /// <param name="value"></param>
        private void ValidateVehicleId(string prop, string value)
        {
            ClearErrors(prop);
            if (string.IsNullOrEmpty(VehicleId))
            {
                base.SetError(prop, notNull);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="prop"></param>
        /// <param name="value"></param>
        private void ValidateVehicleSn(string prop, string value)
        {
            ClearErrors(prop);
            if (string.IsNullOrEmpty(VehicleSn))
            {
                base.SetError(prop, notNull);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="prop"></param>
        /// <param name="value"></param>
        private void ValidateEngineId(string prop, string value)
        {
            ClearErrors(prop);
            if (string.IsNullOrEmpty(EngineId))
            {
                base.SetError(prop, notNull);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="prop"></param>
        /// <param name="value"></param>
        private void ValidateBrandModel(string prop, string value)
        {
            ClearErrors(prop);
            if (string.IsNullOrEmpty(BrandModel))
            {
                base.SetError(prop, notNull);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="prop"></param>
        /// <param name="value"></param>
        private void ValidateDistrictCode(string prop, string value)
        {
            ClearErrors(prop);
            if (string.IsNullOrEmpty(DistrictCode))
            {
                base.SetError(prop, notNull);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="prop"></param>
        /// <param name="value"></param>
        private void ValidateOperationLicense(string prop, string value)
        {
            ClearErrors(prop);
            if (string.IsNullOrEmpty(OperationLicense))
            {
                base.SetError(prop, notNull);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="prop"></param>
        /// <param name="value"></param>
        private void ValidateVehicleStatus(string prop, string value)
        {
            ClearErrors(prop);
            if (string.IsNullOrEmpty(VehicleStatus))
            {
                base.SetError(prop, notNull);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="prop"></param>
        /// <param name="value"></param>
        private void ValidateOwner(string prop, string value)
        {
            ClearErrors(prop);
            if (string.IsNullOrEmpty(Owner))
            {
                base.SetError(prop, notNull);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="prop"></param>
        /// <param name="value"></param>
        private void ValidateContact(string prop, string value)
        {
            ClearErrors(prop);
            if (string.IsNullOrEmpty(Contact))
            {
                base.SetError(prop, notNull);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="prop"></param>
        /// <param name="value"></param>
        private void ValidateContactAddress(string prop, string value)
        {
            ClearErrors(prop);
            if (string.IsNullOrEmpty(ContactAddress))
            {
                base.SetError(prop, notNull);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="prop"></param>
        /// <param name="value"></param>
        private void ValidateContactEmail(string prop, string value)
        {
            ClearErrors(prop);
            if (string.IsNullOrEmpty(ContactEmail))
            {
                base.SetError(prop, notNull);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="prop"></param>
        /// <param name="value"></param>
        private void ValidateContactPhone(string prop, string value)
        {
            ClearErrors(prop);
            if (string.IsNullOrEmpty(ContactPhone))
            {
                base.SetError(prop, notNull);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="prop"></param>
        /// <param name="value"></param>
        private void ValidateRegion(string prop, string value)
        {
            ClearErrors(prop);
            if (string.IsNullOrEmpty(Region))
            {
                base.SetError(prop, notNull);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="prop"></param>
        /// <param name="value"></param>
        private void ValidateStartYear(string prop, string value)
        {
            ClearErrors(prop);
            if (string.IsNullOrEmpty(StartYear))
            {
                base.SetError(prop, notNull);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="prop"></param>
        /// <param name="value"></param>
        private void ValidateServiceType(string prop, string value)
        {
            ClearErrors(prop);
            if (string.IsNullOrEmpty(ServiceType))
            {
                base.SetError(prop, notNull);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="prop"></param>
        /// <param name="value"></param>
        private void ValidateNote(string prop, string value)
        {
            ClearErrors(prop);
            if (string.IsNullOrEmpty(Note))
            {
                base.SetError(prop, notnull);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="prop"></param>
        /// <param name="value"></param>
        private void ValidateCreator(string prop, string value)
        {
            ClearErrors(prop);
            if (string.IsNullOrEmpty(Creator))
            {
                base.SetError(prop, notnull);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="prop"></param>
        /// <param name="value"></param>
        private void ValidateCreateTime(string prop, string value)
        {
            ClearErrors(prop);
            if (string.IsNullOrEmpty(CreateTime))
            {
                base.SetError(prop, notnull);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="prop"></param>
        /// <param name="value"></param>
        private void ValidateVehicleType(string prop, string value)
        {
            ClearErrors(prop);
            if (string.IsNullOrEmpty(VehicleType))
            {
                base.SetError(prop, notnull);
            }
        }

        #endregion

        #region VehicleField



        private Visibility resertButtonVisibility;
        public Visibility ResertButtonVisibility
        {
            get { return resertButtonVisibility; }
            set
            {
                resertButtonVisibility = value;
                ExtractPropertyName(() => ResertButtonVisibility);
            }
        }

        private Visibility saveButtonVisibility;
        public Visibility SaveButtonVisibility
        {
            get { return saveButtonVisibility; }
            set
            {
                saveButtonVisibility = value;
                ExtractPropertyName(() => SaveButtonVisibility);
            }
        }


        private bool isEnabled;
        public bool IsEnabled
        {
            get { return isEnabled; }
            set
            {
                isEnabled = value;
                ExtractPropertyName(() => IsEnabled);
            }
        }


        private string vehicleid;
        /// <summary>
        /// 
        /// </summary>
        public string VehicleId
        {
            get { return vehicleid; }
            set
            {
                vehicleid = value == null ? null : value.Trim();
                ValidateVehicleId(ExtractPropertyName(() => VehicleId), vehicleid);
            }
        }
        private string orgnizationname;
        /// <summary>
        /// 
        /// </summary>
        public string OrgnizationName
        {
            get { return orgnizationname; }
            set
            {
                orgnizationname = value == null ? null : value.Trim();
            }
        }

        private string vehiclesn;
        /// <summary>
        /// 车架号
        /// </summary>
        public string VehicleSn
        {
            get { return vehiclesn; }
            set
            {
                vehiclesn = value == null ? null : value.Trim();
                //ValidateVehicleSn(ExtractPropertyName(() => VehicleSn), vehiclesn);
            }
        }
        private string engineid;
        /// <summary>
        /// 
        /// </summary>
        public string EngineId
        {
            get { return engineid; }
            set
            {
                engineid = value == null ? null : value.Trim();
                //ValidateEngineId(ExtractPropertyName(() => EngineId), engineid);
            }
        }
        private string brandmodel;
        /// <summary>
        /// 
        /// </summary>
        public string BrandModel
        {
            get { return brandmodel; }
            set
            {
                brandmodel = value == null ? null : value.Trim();
                //ValidateBrandModel(ExtractPropertyName(() => BrandModel), brandmodel);
            }
        }
        private string districtcode;
        /// <summary>
        /// 
        /// </summary>
        public string DistrictCode
        {
            get { return districtcode; }
            set
            {
                districtcode = value == null ? null : value.Trim();
                //ValidateDistrictCode(ExtractPropertyName(() => DistrictCode), districtcode);
            }
        }
        private string operationlicense;
        /// <summary>
        /// 
        /// </summary>
        public string OperationLicense
        {
            get { return operationlicense; }
            set
            {
                operationlicense = value == null ? null : value.Trim();
                //ValidateOperationLicense(ExtractPropertyName(() => OperationLicense), operationlicense);
            }
        }
        private string vehiclestatus;
        /// <summary>
        /// 
        /// </summary>
        public string VehicleStatus
        {
            get { return vehiclestatus; }
            set
            {
                vehiclestatus = value == null ? null : value.Trim();
                //ValidateVehicleStatus(ExtractPropertyName(() => VehicleStatus), vehiclestatus);
            }
        }
        private string owner;
        /// <summary>
        /// 
        /// </summary>
        public string Owner
        {
            get { return owner; }
            set
            {
                owner = value == null ? null : value.Trim();
                //ValidateOwner(ExtractPropertyName(() => Owner), owner);
            }
        }
        private string contact;
        /// <summary>
        /// 
        /// </summary>
        public string Contact
        {
            get { return contact; }
            set
            {
                contact = value == null ? null : value.Trim();
                //ValidateContact(ExtractPropertyName(() => Contact), contact);
            }
        }
        private string contactaddress;
        /// <summary>
        /// 
        /// </summary>
        public string ContactAddress
        {
            get { return contactaddress; }
            set
            {
                contactaddress = value == null ? null : value.Trim();
                //ValidateContactAddress(ExtractPropertyName(() => ContactAddress), contactaddress);
            }
        }
        private string contactemail;
        /// <summary>
        /// 
        /// </summary>
        public string ContactEmail
        {
            get { return contactemail; }
            set
            {
                contactemail = value == null ? null : value.Trim();
                //ValidateContactEmail(ExtractPropertyName(() => ContactEmail), contactemail);
            }
        }
        private string contactphone;
        /// <summary>
        /// 
        /// </summary>
        public string ContactPhone
        {
            get { return contactphone; }
            set
            {
                contactphone = value == null ? null : value.Trim();
                //ValidateContactPhone(ExtractPropertyName(() => ContactPhone), contactphone);
            }
        }
        private string region;
        /// <summary>
        /// 
        /// </summary>
        public string Region
        {
            get { return region; }
            set
            {
                region = value == null ? null : value.Trim();
                //ValidateRegion(ExtractPropertyName(() => Region), region);
            }
        }
        private string startyear;
        /// <summary>
        /// 
        /// </summary>
        public string StartYear
        {
            get { return startyear; }
            set
            {
                startyear = value == null ? null : value.Trim();
                //ValidateStartYear(ExtractPropertyName(() => StartYear), startyear);
            }
        }
        private string servicetype;
        /// <summary>
        /// 
        /// </summary>
        public string ServiceType
        {
            get { return servicetype; }
            set
            {
                servicetype = value == null ? null : value.Trim();
                //ValidateServiceType(ExtractPropertyName(() => ServiceType), servicetype);
            }
        }
        private string note;
        /// <summary>
        /// 
        /// </summary>
        public string Note
        {
            get { return note; }
            set
            {
                note = value == null ? null : value.Trim();
                //ValidateNote(ExtractPropertyName(() => Note), note);
            }
        }
        private string creator;
        /// <summary>
        /// 
        /// </summary>
        public string Creator
        {
            get { return creator; }
            set
            {
                creator = value == null ? null : value.Trim();
                //ValidateCreator(ExtractPropertyName(() => Creator), creator);
            }
        }
        private string createtime;
        /// <summary>
        /// 
        /// </summary>
        public string CreateTime
        {
            get { return createtime; }
            set
            {
                createtime = value == null ? null : value.Trim();
                //ValidateCreateTime(ExtractPropertyName(() => CreateTime), createtime);
            }
        }
        private string vehicletype;
        /// <summary>
        /// 
        /// </summary>
        public string VehicleType
        {
            get { return vehicletype; }
            set
            {
                vehicletype = value;
                //ValidateVehicleType(ExtractPropertyName(() => VehicleType), vehicletype);
            }
        }

        #endregion


    }
}
