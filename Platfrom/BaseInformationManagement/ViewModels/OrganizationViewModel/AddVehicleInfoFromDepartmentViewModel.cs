using BaseLib.Model;
using BaseLib.ViewModels;
using Gsafety.Common.CommMessage;
using Gsafety.Common.Controls;
using Gsafety.PTMS.BaseInformation;
using Gsafety.PTMS.ServiceReference.DistrictService;
using Gsafety.PTMS.ServiceReference.VehicleService;
using Gsafety.PTMS.ServiceReference.OrganizationService;
using Gsafety.PTMS.Share;
using Jounce.Core.ViewModel;
using Jounce.Framework;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Windows;
using Gsafety.PTMS.Bases.Models;

namespace Gsafety.Ant.BaseInformation.ViewModels.OrganizationViewModel
{
    /// <summary>
    /// 添加车辆
    /// </summary>
    [ExportAsViewModel(BaseInformationName.AddVehicleInfoFromDepartmentVm)]
    public class AddVehicleInfoFromDepartmentViewModel : DetailViewModel<Gsafety.PTMS.ServiceReference.VehicleService.Vehicle>
    {
        //商用 公用 私有 未知
        string business = ApplicationContext.Instance.StringResourceReader.GetString("Comercial");
        string _public = ApplicationContext.Instance.StringResourceReader.GetString("Public");
        string _private = ApplicationContext.Instance.StringResourceReader.GetString("Private");
        string unKnwon = ApplicationContext.Instance.StringResourceReader.GetString("Unkown");
        /// <summary>
        /// 事件
        /// </summary>
        public event EventHandler<SaveResultArgs> OnSaveResult;

        private string dataOperateType;

        private Visibility resertButtonVisibility;
        public Visibility ResertButtonVisibility
        {
            get { return resertButtonVisibility; }
            set
            {
                resertButtonVisibility = value;
                RaisePropertyChanged(() => ResertButtonVisibility);
            }
        }

        private Visibility saveButtonVisibility;
        public Visibility SaveButtonVisibility
        {
            get { return saveButtonVisibility; }
            set
            {
                saveButtonVisibility = value;
                RaisePropertyChanged(() => SaveButtonVisibility);
            }
        }


        private bool isEnabled;
        public bool IsEnabled
        {
            get { return isEnabled; }
            set
            {
                isEnabled = value;
                RaisePropertyChanged(() => IsEnable);
            }
        }

        private bool isvehiclereadonly;
        public bool IsVehicleReadOnly
        {
            get { return isvehiclereadonly; }
            set
            {
                isvehiclereadonly = value;
                RaisePropertyChanged(() => IsVehicleReadOnly);
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
                RaisePropertyChanged(() => this.VehicleId);
            }
        }
        private string orgnizationname;
        /// <summary>
        /// 所属组织机构名称
        /// </summary>
        public string OrgnizationName
        {
            get { return orgnizationname; }
            set
            {
                orgnizationname = value == null ? null : value.Trim();
                RaisePropertyChanged(() => this.OrgnizationName);
            }
        }

        private string organizationId;
        /// <summary>
        /// 父级组织机构编号
        /// </summary>
        public string OrganizationId
        {
            get { return this.organizationId; }
            set
            {
                this.organizationId = value;
                RaisePropertyChanged(() => this.OrganizationId);
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
                ValidateVehicleSn(ExtractPropertyName(() => VehicleSn), vehiclesn);
                RaisePropertyChanged(() => this.VehicleSn);
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
                ValidateEngineId(ExtractPropertyName(() => EngineId), engineid);
                RaisePropertyChanged(() => this.EngineId);
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
                RaisePropertyChanged(() => this.BrandModel);
            }
        }

         private InstallStatusType InstallStatus = InstallStatusType.UnInstall;

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
                RaisePropertyChanged(() => this.OperationLicense);
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
                JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => Owner));
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
                RaisePropertyChanged(() => this.Contact);

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
                RaisePropertyChanged(() => this.ContactAddress);
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
                ValidateContactEmail(ExtractPropertyName(() => ContactEmail), contactemail);
                RaisePropertyChanged(() => this.ContactEmail);
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
                contactphone = value;
                if (contactphone != null && contactphone != string.Empty)
                {
                    ValidateContactPhone(ExtractPropertyName(() => ContactPhone), contactphone.Trim());
                }
                RaisePropertyChanged(() => this.ContactPhone);
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
                RaisePropertyChanged(() => this.Region);
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
                RaisePropertyChanged(() => this.StartYear);
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
                ValidateVehicleType(ExtractPropertyName(() => VehicleType), vehicletype);
                RaisePropertyChanged(() => this.VehicleType);
            }
        }


        private ComboBoxBasicStruct<VehicleType> vType = new ComboBoxBasicStruct<VehicleType>();
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
        /// 选中的车辆服务类型
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
                if (province != value)
                {
                    province = value;
                    RaisePropertyChanged(() => Province);

                    if (province != null)
                    {
                        InitCities(province.Code);
                    }
                }
            }
        }
        private ObservableCollection<District> provinces = new ObservableCollection<District>();
        /// <summary>
        /// 所有省份
        /// </summary>
        public ObservableCollection<District> Provinces
        {
            get { return provinces; }
            set
            {
                provinces = value;
                RaisePropertyChanged(() => Provinces);
            }
        }


        private ObservableCollection<Organization> organizations = new ObservableCollection<Organization>();
        /// <summary>
        /// 所有机构
        /// </summary>
        public ObservableCollection<Organization> Organizations
        {
            get { return organizations; }
            set
            {
                organizations = value;
                RaisePropertyChanged(() => Organizations);
            }
        }


        private Organization organization = new Organization();
        /// <summary>
        /// 所属机构
        /// </summary>
        public Organization Organization
        {
            get { return organization; }
            set
            {
                organization = value;
                RaisePropertyChanged(() => Organization);
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
        private ObservableCollection<District> cities = new ObservableCollection<District>();
        /// <summary>
        /// 所有市列表
        /// </summary>
        public ObservableCollection<District> Cities
        {
            get { return cities; }
            set
            {
                cities = value;
                RaisePropertyChanged(() => Cities);
            }
        }


        public AddVehicleInfoFromDepartmentViewModel()
        {
            InitProvinces();
            InitVehicleServiceType();
        }


        /// <summary>
        /// 初始化服务客户端方法
        /// </summary>
        /// <returns></returns>
        private VehicleServiceClient InitVehicleServiceClient()
        {
            try
            {
                VehicleServiceClient client = null;
                client = ServiceClientFactory.Create<VehicleServiceClient>();
                client.AddVehicleCompleted += client_AddVehicleCompleted;
                client.UpdateVehicleCompleted += client_UpdateVehicleCompleted;

                return client;
            }
            catch (Exception ex)
            {
                MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString(LProxy.Caption), ApplicationContext.Instance.StringResourceReader.GetString(LProxy.InitServiceClientError));
                return null;
            }

        }

        /// <summary>
        /// 激活视图方法
        /// </summary>
        /// <param name="viewName"></param>
        /// <param name="viewParameters"></param>
        public new void ActivateView(string viewName, IDictionary<string, object> viewParameters)
        {
            try
            {
                //this.Reset();
                base.ActivateView(viewName, viewParameters);
                action = viewParameters["action"].ToString();
                this.OrganizationId = "";
                this.OrgnizationName = "";
                if (viewParameters.Keys.Contains("VehicleTypes"))
                {
                    List<VehicleType> types = (List<VehicleType>)viewParameters["VehicleTypes"];
                    foreach (var item in types)
                    {
                        ComboBoxBasicStruct<VehicleType> cb = new ComboBoxBasicStruct<VehicleType>();
                        cb.Key = item;
                        cb.Value = item.Name;
                        ZVehicleType.Add(cb);
                    }
                    JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => ZVehicleType));
                }

                switch (action)
                {
                    case "view":
                        IsReadOnly = true;
                        IsEnabled = false;
                        InitialModel = viewParameters["data"] as Gsafety.PTMS.ServiceReference.VehicleService.Vehicle;
                        SaveButtonVisibility = Visibility.Collapsed;
                        ResertButtonVisibility = Visibility.Collapsed;
                        IsVehicleReadOnly = true;
                        ViewVehicle();
                        Title = ApplicationContext.Instance.StringResourceReader.GetString("Common_View");
                        break;

                    case "update":
                        IsReadOnly = false;
                        IsEnabled = true;
                        IsVehicleReadOnly = true;
                        InitialModel = viewParameters["data"] as Gsafety.PTMS.ServiceReference.VehicleService.Vehicle;
                        dataOperateType = "update";
                        ViewVehicle();
                        Title = ApplicationContext.Instance.StringResourceReader.GetString("Common_Update");
                        break;

                    case "add":
                        InitialModel = new Gsafety.PTMS.ServiceReference.VehicleService.Vehicle();
                        IsReadOnly = false;
                        IsEnabled = true;
                        IsVehicleReadOnly = false;
                        InstallStatus = InstallStatusType.UnInstall;
                        if (viewParameters.Count > 0)
                        {
                            this.OrganizationId = viewParameters["OrgnizationId"].ToString();
                            this.OrgnizationName = viewParameters["OrgnizationName"].ToString();

                            Organizations = ApplicationContext.Instance.VehicleDepartmentList;

                            Organization = Organizations.FirstOrDefault(x => x.ID == this.OrganizationId);

                          
                        }
                        JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => Organizations));
                        JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => Organization));
                        JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => OrgnizationName));
                        dataOperateType = "add";
                        Title = ApplicationContext.Instance.StringResourceReader.GetString("Common_Add");

                        Reset();
                        break;

                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }

        /// <summary>
        /// 初始化省
        /// </summary>
        protected void InitProvinces()
        {
            var _provices = ApplicationContext.Instance.BufferManager.DistrictManager.DistrictByAuthority.Where(x => x.Code.Length == 2).ToList().OrderBy(x => x.Name);
            foreach (var item in _provices)
            {
                Provinces.Add(item);
            }
        }

        /// <summary>
        /// 初始化城市
        /// </summary>
        /// <param name="provinceCode"></param>
        protected void InitCities(string provinceCode)
        {
            if (provinceCode != null)
            {
                City = null;
                Cities.Clear();
                var _cities = ApplicationContext.Instance.BufferManager.DistrictManager.DistrictByAuthority.Where(x => x.Code.Length == 5 && x.Code.StartsWith(provinceCode)).ToList().OrderBy(x => x.Name);
                foreach (var item in _cities)
                {
                    Cities.Add(item);
                }

                if (Cities.Count != 0)
                {
                    City = Cities[0];
                }
            }
        }

        /// <summary>
        /// 自动填充车辆数据信息大文本框
        /// </summary>
        private void ViewVehicle()
        {
            try
            {
                if (InitialModel.DistrictCode.Length == 2)
                {
                    Province = Provinces.FirstOrDefault(x => x.Code == InitialModel.DistrictCode);
                }

                this.OrgnizationName = InitialModel.OrgnizationName;
                this.OrganizationId = InitialModel.OrgnizationId;



                Organizations = ApplicationContext.Instance.VehicleDepartmentList;

                Organization = Organizations.FirstOrDefault(x => x.ID == InitialModel.OrgnizationId);

                JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => Organizations));
                JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => Organization));


                this.VehicleId = InitialModel.VehicleId;
                this.VehicleSn = InitialModel.VehicleSn;
                this.EngineId = InitialModel.EngineId;
                this.Region = InitialModel.Region;
                this.OperationLicense = InitialModel.OperationLicense;
                this.Owner = InitialModel.Owner;
                this.Contact = InitialModel.Contact;
                this.ContactPhone = InitialModel.ContactPhone;
                this.ContactEmail = InitialModel.ContactEmail;
                this.ContactAddress = InitialModel.ContactAddress;
                this.BrandModel = InitialModel.BrandModel;
                this.StartYear = InitialModel.StartYear;
                this.InstallStatus = InitialModel.InstallStatus;
                if (InitialModel.DistrictCode.Length == 5)
                {
                    Province = Provinces.FirstOrDefault(x => x.Code == InitialModel.DistrictCode.Substring(0, 2));
                    JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => Province));
                    City = Cities.FirstOrDefault(x => x.Code == InitialModel.DistrictCode);
                    JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => City));
                }

                VType = ZVehicleType.FirstOrDefault(x => x.Key.ID == InitialModel.VehicleType.ID);

                VServiceType = ZVehicleServiceType.FirstOrDefault(n => n.Key == InitialModel.ServiceType);

            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }

        }


        /// <summary>
        /// 增加新数据
        /// </summary>
        protected override void OnCommitted()
        {
            VehicleServiceClient client = InitVehicleServiceClient();
            try
            {
                Gsafety.PTMS.ServiceReference.VehicleService.Vehicle v = new Gsafety.PTMS.ServiceReference.VehicleService.Vehicle();
                VehicleType type = new VehicleType();
                v.OrgnizationId = Organization.ID;
                v.ClientId = ApplicationContext.Instance.AuthenticationInfo.ClientID;//非空
                //车牌号
                v.VehicleId = this.VehicleId;//非空
                v.VehicleSn = this.VehicleSn;//车架号
                v.EngineId = this.EngineId;//发动机号
                v.DistrictCode = City == null ? Province.Code : City.Code;//所属区划
                v.BrandModel = this.BrandModel;
                v.OperationLicense = this.OperationLicense;//运行许可证
                v.Owner = this.Owner;//车主
                v.VehicleType = new VehicleType();
                v.VehicleType.ID = VType.Key.ID;//车辆类型表ID
                v.ContactPhone = ContactPhone;
                v.ContactEmail = ContactEmail;
                v.ContactAddress = ContactAddress;
                v.StartYear = StartYear;
                v.Region = Region;//运行区域               
                v.ServiceType = VServiceType.Key;
                v.VehicleStatus = VehicleConditionType.Available;
                v.Contact = this.Contact;//车主身份证号                

                v.InstallStatus = this.InstallStatus;


                Gsafety.PTMS.Bases.Models.Vehicle vehicle = new PTMS.Bases.Models.Vehicle();

                vehicle.OrganizationID = Organization.ID;
                vehicle.OrganizationName = Organization.Name;     
                //车牌号
                vehicle.VehicleId = this.VehicleId;//非空
                vehicle.VehicleSn = this.VehicleSn;//车架号
                vehicle.EngineId = this.EngineId;//发动机号
                vehicle.DistrictCode = City == null ? Province.Code : City.Code;//所属区划
                vehicle.ProvinceName = Province.Name;

                if (City != null)
                {
                    vehicle.CityName = City.Name;
                    vehicle.CityCode = City.Code;
                }          
                vehicle.BrandModel = this.BrandModel;              
                vehicle.Owner = this.Owner;//车主
                vehicle.VehicleTypeDescribe = VType.Key.Name;              
                vehicle.ContactPhone = ContactPhone;                
                vehicle.StartYear = StartYear;
                vehicle.VehicleServiceType = VServiceType.Key; 

                if ("update".Equals(dataOperateType))
                {
                    v.CreateTime = InitialModel.CreateTime.ToUniversalTime();
                    v.Creator = InitialModel.Creator;
                    v.Valid = InitialModel.Valid;                    
                    client.UpdateVehicleAsync(v);
                    UpdateVehicle installvehicle = new UpdateVehicle();
                    installvehicle.Vehicle = vehicle;
                    ApplicationContext.Instance.EventAggregator.Publish(installvehicle);
                }
                else
                {
                    v.Valid = 1;
                    v.CreateTime = DateTime.UtcNow;
                    v.Creator = ApplicationContext.Instance.AuthenticationInfo.Account;
                    client.AddVehicleAsync(v);
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("OnCommittedMethod", ex);
            }
            finally
            {
                CloseVehicleServiceClient(client);
            }
        }

        private void CloseVehicleServiceClient(VehicleServiceClient client)
        {
            if (client != null)
            {
                client.CloseAsync();
            }
            client = null;
        }

        private void CloseVehicleTypeClient(VehicleTypeClient client)
        {
            if (client != null)
            {
                client.CloseAsync();
            }
            client = null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void client_AddVehicleCompleted(object sender, AddVehicleCompletedEventArgs e)
        {
            try
            {
                if (e.Error != null)
                {
                    ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), e.Error);
                    MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString(LProxy.ServerError));
                }
                if (e.Result.IsSuccess == false)
                {
                    if (!string.IsNullOrEmpty(e.Result.ErrorMsg))
                    {
                        ApplicationContext.Instance.Logger.LogError(MethodBase.GetCurrentMethod().ToString(),
                            e.Result.ErrorMsg);
                        MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString(e.Result.ErrorMsg));
                    }
                    else
                    {
                        MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString(LProxy.OperatorServiceError));
                        ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(),
                            e.Result.ExceptionMessage);
                    }
                }
                else
                {
                    SaveResultArgs args = new SaveResultArgs();
                    args.Result = true;

                    if (OnSaveResult != null)
                    {
                        OnSaveResult(this, args);
                    }
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
            finally
            {
                VehicleServiceClient client = sender as VehicleServiceClient;
                CloseVehicleServiceClient(client);
            }
        }

        /// <summary>
        /// 更新完成
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void client_UpdateVehicleCompleted(object sender, UpdateVehicleCompletedEventArgs e)
        {
            try
            {
                if (e.Error != null)
                {
                    ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), e.Error);
                    MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString(LProxy.ServerError));
                }
                else
                {
                    if (e.Result.IsSuccess == false)
                    {
                        if (!string.IsNullOrEmpty(e.Result.ErrorMsg))
                        {
                            ApplicationContext.Instance.Logger.LogError(MethodBase.GetCurrentMethod().ToString(),
                                e.Result.ErrorMsg);
                            MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString(e.Result.ErrorMsg));
                        }
                        else
                        {
                            MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString(LProxy.OperatorServiceError));
                            ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(),
                                e.Result.ExceptionMessage);
                        }
                    }
                    else
                    {
                        SaveResultArgs args = new SaveResultArgs();
                        args.Result = true;

                        if (OnSaveResult != null)
                        {
                            OnSaveResult(this, args);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
            finally
            {
                VehicleServiceClient client = sender as VehicleServiceClient;
                CloseVehicleServiceClient(client);
            }
        }


        /// <summary>
        /// 重置
        /// </summary>
        protected override void Reset()
        {
            try
            {
                if (InitialModel != null && InitialModel.VehicleId != null)
                {
                    VType = ZVehicleType.FirstOrDefault();
                    VServiceType = ZVehicleServiceType.FirstOrDefault();
                    //City = Cities.FirstOrDefault();
                    ViewVehicle();
                }
                else
                {
                    Province = Provinces.FirstOrDefault();
                    this.VehicleId = string.Empty;
                    this.EngineId = string.Empty;
                    this.Region = string.Empty;
                    this.OperationLicense = string.Empty;
                    this.Owner = string.Empty;
                    this.Contact = string.Empty;
                    this.ContactPhone = string.Empty;
                    this.ContactEmail = string.Empty;
                    this.ContactAddress = string.Empty;
                    this.VehicleSn = string.Empty;

                    VType = ZVehicleType.FirstOrDefault();
                    VServiceType = ZVehicleServiceType.FirstOrDefault();
                    City = Cities.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }

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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="prop"></param>
        /// <param name="value"></param>
        private void ValidateVehicleId(string prop, string value)
        {
            ValidateRequire(prop, value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="prop"></param>
        /// <param name="value"></param>
        private void ValidateVehicleSn(string prop, string value)
        {
            ValidateRequire(prop, value);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="prop"></param>
        /// <param name="value"></param>
        private void ValidateEngineId(string prop, string value)
        {
            ValidateRequire(prop, value);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="prop"></param>
        /// <param name="value"></param>
        private void ValidateContactEmail(string prop, string value)
        {
            ValidateEmailFormat(prop, value);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="prop"></param>
        /// <param name="value"></param>
        private void ValidateContactPhone(string prop, string value)
        {
            ClearErrors(prop);
            bool flag = true;
            if (!string.IsNullOrEmpty(value))
            {
                int num = 0;
                char[] cc = value.ToCharArray();
                foreach (var item in cc)
                {
                    if (item != ' ' && item != '-' && item != '(' && item != ')')
                        flag = flag && int.TryParse(item.ToString(), out num);
                }
            }

            if (!flag)
            {
                base.SetError(prop, ApplicationContext.Instance.StringResourceReader.GetString(PTMSBaseViewModel.wrongformat));
            }
            else
            {
                if (value != null && value.Length > 12)
                {
                    base.SetError(prop, ApplicationContext.Instance.StringResourceReader.GetString("PhoneOverLength"));
                }
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="prop"></param>
        /// <param name="value"></param>
        private void ValidateVehicleType(string prop, string value)
        {
            ValidateRequire(prop, value);
        }
    }
}
