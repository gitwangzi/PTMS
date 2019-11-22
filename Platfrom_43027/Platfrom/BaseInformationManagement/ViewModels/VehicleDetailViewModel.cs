using BaseLib.Model;
using BaseLib.ViewModels;
using Gsafety.Common.Controls;
using Gsafety.PTMS.BaseInformation;
using Gsafety.PTMS.Manager.Models;
using Gsafety.PTMS.ServiceReference.DistrictService;
using Gsafety.PTMS.ServiceReference.VehicleService;
using Gsafety.PTMS.Share;
using Jounce.Core.ViewModel;
using Jounce.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows;

namespace Gsafety.Ant.BaseInformation.ViewModels
{
    [ExportAsViewModel(BaseInformationName.VehicleDetailVm)]
    public class VehicleDetailViewModel : DetailViewModel<Vehicle>
    {
        #region 属性

        private Visibility resertButtonVisibility;
        /// <summary>
        /// 重置按钮是否可见
        /// </summary>
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
        /// <summary>
        /// 保存按钮是否可见
        /// </summary>
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
        /// <summary>
        /// UI控件是否可用
        /// </summary>
        public bool IsEnabled
        {
            get { return isEnabled; }
            set
            {
                isEnabled = value;
                RaisePropertyChanged(() => IsEnabled);
            }
        }

        private string orgnizationname;
        /// <summary>
        /// 当前所属组织机构的名称
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



        private VehicleServiceClient client = null;

        protected string dataOperateType;

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
                //ValidateVehicleId(ExtractPropertyName(() => VehicleId), vehicleid);
                RaisePropertyChanged(() => this.VehicleId);
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
                //ValidateEngineId(ExtractPropertyName(() => EngineId), engineid);
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
                //ValidateBrandModel(ExtractPropertyName(() => BrandModel), brandmodel);
                RaisePropertyChanged(() => this.BrandModel);
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
                RaisePropertyChanged(() => this.DistrictCode);
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
                RaisePropertyChanged(() => this.OperationLicense);
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
                RaisePropertyChanged(() => this.VehicleStatus);
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
                RaisePropertyChanged(() => this.Owner);
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
                //ValidateContactAddress(ExtractPropertyName(() => ContactAddress), contactaddress);
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
                //ValidateContactEmail(ExtractPropertyName(() => ContactEmail), contactemail);
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
                contactphone = value == null ? null : value.Trim();
                //ValidateContactPhone(ExtractPropertyName(() => ContactPhone), contactphone);
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
                //ValidateRegion(ExtractPropertyName(() => Region), region);
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
                //ValidateStartYear(ExtractPropertyName(() => StartYear), startyear);
                RaisePropertyChanged(() => this.StartYear);
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
                RaisePropertyChanged(() => this.ServiceType);
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
                RaisePropertyChanged(() => this.Note);
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
                RaisePropertyChanged(() => this.Creator);
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
                RaisePropertyChanged(() => this.VehicleType);
            }
        }

        private District province;
        /// <summary>
        /// 选中的省级区划
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


        #endregion

        #region 构造函数

        public VehicleDetailViewModel()
        {
            this.InlilizeViewModel();
        }

        #endregion

        #region 方法

        private void InlilizeViewModel()
        {
            this.ZVehicleType = new List<ComboBoxBasicStruct<VehicleType>>();
            this.VType = new ComboBoxBasicStruct<VehicleType>();
            this.ZVehicleServiceType = new List<ComboBoxBasicStruct<VehicleSeviceType>>();
            this.ResertButtonVisibility = Visibility.Visible;
            this.SaveButtonVisibility = Visibility.Visible;
            this.IsEnabled = true;
            this.dataOperateType = "";
            this.Province = new District();
            this.City = new District();
            this.VStatus = new ComboBoxBasicStruct<VehicleConditionType>();
            this.VServiceType = new ComboBoxBasicStruct<VehicleSeviceType>();
            this.InlizeClient();
            this.InlitizeVehicleType();
            this.InlitizeVehicleServiceType();
            this.InitProvinces();
            this.InitVehicleStatus();
        }

        /// <summary>
        /// 初始化客户端
        /// </summary>
        private void InlizeClient()
        {
            this.client = ServiceClientFactory.Create<VehicleServiceClient>();
            client.AddVehicleCompleted += client_AddVehicleCompleted;
            client.UpdateVehicleCompleted += client_UpdateVehicleCompleted;
            client.GetBscVehicleTypeListCompleted += client_GetBscVehicleTypeListCompleted;
        }

        /// <summary>
        /// 初始化车辆类别
        /// </summary>
        private void InlitizeVehicleType()
        {
            this.client.GetBscVehicleTypeListAsync();
        }

        /// <summary>
        /// 初始化服务类型
        /// </summary>
        private void InlitizeVehicleServiceType()
        {
            //商用
            ZVehicleServiceType.Add(new ComboBoxBasicStruct<VehicleSeviceType>()
            {
                Key = VehicleSeviceType.Comercial,
                Value = ApplicationContext.Instance.StringResourceReader.GetString(LProxy.Commercial)
            });
            //公用
            ZVehicleServiceType.Add(new ComboBoxBasicStruct<VehicleSeviceType>()
            {
                Key = VehicleSeviceType.Public,
                Value = ApplicationContext.Instance.StringResourceReader.GetString(LProxy.Public)
            });
            //私有
            ZVehicleServiceType.Add(new ComboBoxBasicStruct<VehicleSeviceType>()
            {
                Key = VehicleSeviceType.Private,
                Value = ApplicationContext.Instance.StringResourceReader.GetString(LProxy.Private)
            });
            //未知
            ZVehicleServiceType.Add(new ComboBoxBasicStruct<VehicleSeviceType>()
            {
                Key = VehicleSeviceType.Unknown,
                Value = ApplicationContext.Instance.StringResourceReader.GetString(LProxy.Unknown)
            });
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

        /// <summary>
        /// 初始化车况
        /// </summary>
        /// <param name="provinceCode"></param>
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
                    Value = ApplicationContext.Instance.StringResourceReader.GetString("ID_INSTALL_Yes")//具备
                });
                ZVehicleStatus.Add(new ComboBoxBasicStruct<VehicleConditionType>()
                {
                    Key = VehicleConditionType.Unavailable,
                    Value = ApplicationContext.Instance.StringResourceReader.GetString("ID_INSTALL_No")//不具备
                });
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("InitVehicleStatus", ex);
            }
        }

        /// <summary>
        /// 获取车辆类别列表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void client_GetBscVehicleTypeListCompleted(object sender, GetBscVehicleTypeListCompletedEventArgs e)
        {
            try
            {
                if (e.Cancelled)
                {
                    return;
                }
                if (e.Error != null)
                {
                    ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), e.Error);
                }
                if (e.Result.IsSuccess == false)
                {
                    if (!string.IsNullOrEmpty(e.Result.ErrorMsg))
                    {
                        ApplicationContext.Instance.Logger.LogError(MethodBase.GetCurrentMethod().ToString(),
                            e.Result.ErrorMsg);
                    }
                    else
                    {

                        ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(),
                            e.Result.ExceptionMessage);
                    }
                }
                //成功获取
                else
                {
                    this.ZVehicleType = new List<ComboBoxBasicStruct<VehicleType>>();
                    foreach (var item in e.Result.Result)
                    {
                        ComboBoxBasicStruct<VehicleType> cb = new ComboBoxBasicStruct<VehicleType>();
                        cb.Key = item;
                        cb.Value = item.Name;
                        ZVehicleType.Add(cb);
                    }
                    this.RaisePropertyChanged(() => this.ZVehicleType);
                }

            }
            catch (Exception ex)
            {
                this.InlilizeViewModel();
                ApplicationContext.Instance.Logger.LogException("client_GetBscVehicleTypeListCompleted", ex);
            }
            finally
            {
                if (this.client != null)
                {
                    this.client.CloseAsync();
                }
            }
        }

        /// <summary>
        /// 更新车辆信息完成
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void client_UpdateVehicleCompleted(object sender, UpdateVehicleCompletedEventArgs e)
        {
            try
            {
                if (e.Cancelled)
                {
                    return;
                }
                if (e.Error != null)
                {
                    ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), e.Error);
                }
                if (e.Result.IsSuccess == false)
                {
                    if (!string.IsNullOrEmpty(e.Result.ErrorMsg))
                    {
                        ApplicationContext.Instance.Logger.LogError(MethodBase.GetCurrentMethod().ToString(),
                            e.Result.ErrorMsg);
                    }
                    else
                    {

                        ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(),
                            e.Result.ExceptionMessage);
                    }
                }
                //成功获取
                else
                {
                    MessageBoxHelper.ShowDialog(
                        ApplicationContext.Instance.StringResourceReader.GetString(LProxy.Caption),
                        ApplicationContext.Instance.StringResourceReader.GetString(LProxy.OperatedSuccessed));
                }

            }
            catch (Exception ex)
            {
                this.InlilizeViewModel();
                ApplicationContext.Instance.Logger.LogException("client_UpdateVehicleCompleted", ex);

            }
            finally
            {
                if (this.client != null)
                {
                    this.client.CloseAsync();
                }
                this.client = null;
            }
        }

        /// <summary>
        /// 新增车辆信息完成
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void client_AddVehicleCompleted(object sender, AddVehicleCompletedEventArgs e)
        {
            try
            {
                if (e.Cancelled)
                {
                    return;
                }
                if (e.Error != null)
                {
                    ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), e.Error);
                }
                if (e.Result.IsSuccess == false)
                {
                    if (!string.IsNullOrEmpty(e.Result.ErrorMsg))
                    {
                        ApplicationContext.Instance.Logger.LogError(MethodBase.GetCurrentMethod().ToString(),
                            e.Result.ErrorMsg);
                    }
                    else
                    {

                        ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(),
                            e.Result.ExceptionMessage);
                    }
                }
                //成功获取
                else
                {
                    MessageBoxHelper.ShowDialog(
                        ApplicationContext.Instance.StringResourceReader.GetString(LProxy.Caption),
                        ApplicationContext.Instance.StringResourceReader.GetString(LProxy.OperatedSuccessed));
                }

            }
            catch (Exception ex)
            {
                this.InlilizeViewModel();
                ApplicationContext.Instance.Logger.LogException("client_AddVehicleCompleted", ex);

            }
            finally
            {
                if (this.client != null)
                {
                    this.client.CloseAsync();
                }
                this.client = null;
            }
        }



        protected override void ActivateView(string viewName, IDictionary<string, object> viewParameters)
        {
            base.ActivateView(viewName, viewParameters);

            action = viewParameters.Count == 0 ? "" : viewParameters["action"].ToString();
            switch (action)
            {
                case "View":
                    IsReadOnly = true;
                    IsEnabled = false;
                    InitialModel = viewParameters["data"] as Vehicle;
                    SaveButtonVisibility = Visibility.Collapsed;
                    ResertButtonVisibility = Visibility.Collapsed;

                    break;

                case "Update":
                    IsReadOnly = false;
                    IsEnabled = true;
                    InitialModel = viewParameters["data"] as Vehicle;

                    break;

                case "Add":
                    //InitialModel = viewParameters["data"] as Vehicle;
                    IsReadOnly = false;
                    IsEnabled = true;
                    this.dataOperateType = "Add";
                    if (InitialModel != null)
                    {
                        this.OrgnizationName = InitialModel.OrgnizationName;
                    }
                    else
                    {
                        this.OrgnizationName = "";
                    }

                    break;

                default:
                    break;
            }


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
                //v.OrgnizationId = SelectedOrgStatus.OrgId;//非空
                v.OrgnizationId = "6ada6453-855c-4a00-b547-527740fa31a3";
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
                v.Valid = 1;
                v.CreateTime = DateTime.UtcNow;
                v.Creator = ApplicationContext.Instance.AuthenticationInfo.UserID;
                v.InstallStatus = InstallStatusType.UnInstall;

                if (this.client == null)
                {
                    this.InlizeClient();
                }

                if ("Update".Equals(dataOperateType))
                {
                    client.UpdateVehicleAsync(v);
                }
                else
                {
                    client.AddVehicleAsync(v);
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("OnCommittedMethod", ex);
            }
        }


        #endregion


    }
}
