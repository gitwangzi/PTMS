using Gsafety.PTMS.Bases.Enums;
using Gsafety.PTMS.Bases.Models;
using Gsafety.PTMS.ServiceReference.MessageService;
using Gsafety.PTMS.ServiceReference.MessageServiceExt;
using Gsafety.PTMS.ServiceReference.OrganizationService;
using Gsafety.PTMS.ServiceReference.RunMonitorGroupService;
using Gsafety.PTMS.ServiceReference.VehicleService;
using Jounce.Core.Event;
using Jounce.Core.Model;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Reflection;
using Gsafety.PTMS.ServiceReference.VehicleService;

namespace Gsafety.PTMS.Share
{
    public class VehicleOrganizationManage : BaseNotify, IPartImportsSatisfiedNotification,
                                   IEventSink<DeviceMaintain>, IEventSink<OnOfflineEx>, IEventSink<Gsafety.PTMS.ServiceReference.MessageServiceExt.Vehicle>
    {
        #region 字段

        [Import]
        private IEventAggregator EventAggregator;

        private bool _hasLoadData = true;

        public bool HasLoadData
        {
            get { return _hasLoadData; }
            set { _hasLoadData = value; }
        }

        public bool HasLoadVehcile { get; set; }
        #endregion

        #region 属性

        #region AuthorityVehicleOrgs 授权的车辆组织机构
        private ObservableCollection<Organization> _authorityVehicleOrgs = new ObservableCollection<Organization>();
        public ObservableCollection<Organization> AuthorityVehicleOrgs
        {
            get
            {
                //if (!_hasLoadData)
                //    DataLoading();
                return
                    _authorityVehicleOrgs;
            }
            set { _authorityVehicleOrgs = value; }
        }
        #endregion

        #region 已安装车辆
        private ObservableCollection<Gsafety.PTMS.Bases.Models.Vehicle> _VehicleList = new ObservableCollection<Gsafety.PTMS.Bases.Models.Vehicle>();
        public ObservableCollection<Gsafety.PTMS.Bases.Models.Vehicle> VehicleList
        {
            get { return _VehicleList; }
            set { _VehicleList = value; }
        }
        #endregion


        private ObservableCollection<VehicleTypeColor> _speedcolorlist = new ObservableCollection<VehicleTypeColor>();
        public ObservableCollection<VehicleTypeColor> SpeedColorList
        {
            get { return _speedcolorlist; }
            set
            {
                _speedcolorlist = value;
                
            }
        }

        #region 数量统计
        public int TotalCount
        {
            get
            {
                return VehicleList.Count;
            }
        }

        public int OnlineCount
        {
            get
            {
                return VehicleList.Where(t => t.IsOnLine).Count();
            }
        }

        public string OnlineRate
        {
            get
            {
                if (TotalCount == 0)
                {
                    return "";
                }

                return (OnlineCount * 1.0 / TotalCount).ToString("##%");
            }
        }

        public void RaiseVehicleCountChanged()
        {
            RaisePropertyChanged(() => TotalCount);
            RaisePropertyChanged(() => OnlineCount);
            RaisePropertyChanged(() => OnlineRate);
        }
        #endregion

        private bool _IsVehicleLoadComplete = false;
        public bool IsVehicleLoadComplete
        {
            get { return _IsVehicleLoadComplete; }
            set
            {
                _IsVehicleLoadComplete = value;
                RaisePropertyChanged(() => IsVehicleLoadComplete);
            }
        }

        #endregion


        public VehicleOrganizationManage()
        {
            CompositionInitializer.SatisfyImports(this);
        }

        public void OnImportsSatisfied()
        {
            EventAggregator.SubscribeOnDispatcher<OnOfflineEx>(this);
            EventAggregator.SubscribeOnDispatcher<DeviceMaintain>(this);
            EventAggregator.SubscribeOnDispatcher<Gsafety.PTMS.ServiceReference.MessageServiceExt.Vehicle>(this);
        }

        public void DataLoading()
        {
            _VehicleList.Clear();
            _authorityVehicleOrgs.Clear();

            GetVehicleOrganizations();
            GetVehicleColor();
        }

        private void GetVehicleColor()
        {
            VehicleTypeClient client = ServiceClientFactory.Create<VehicleTypeClient>();

            client.GetAllVehicleTypeColorListCompleted += client_GetAllVehicleTypeColorListCompleted;

            client.GetAllVehicleTypeColorListAsync(ApplicationContext.Instance.AuthenticationInfo.ClientID);
        
        }


        void client_GetAllVehicleTypeColorListCompleted(object sender, GetAllVehicleTypeColorListCompletedEventArgs e)
        {
            try
            {
                if (e.Error == null)
                {
                    if (e.Result.IsSuccess)
                    {
                        SpeedColorList = new ObservableCollection<VehicleTypeColor>(e.Result.Result);


                    }
                }
               
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("client_GetByNameVehicleTypeListCompleted", ex);
            }

        }
        private void GetVehicleOrganizations()
        {
            var orgClient = ServiceClientFactory.Create<OrganizationClient>();
            if (ApplicationContext.Instance.AuthenticationInfo.Role.RoleCategory == (short)RoleCategory.SecurityAdmin || ApplicationContext.Instance.AuthenticationInfo.Role.RoleCategory == (short)RoleCategory.SecurityMonitor)
            {
                orgClient.GetOrganizationByUserCompleted += orgClient_GetOrganizationByUserCompleted;
                orgClient.GetOrganizationByUserAsync(ApplicationContext.Instance.AuthenticationInfo.UserID);
            }
            else if (ApplicationContext.Instance.AuthenticationInfo.Role.RoleCategory == (short)RoleCategory.MaintainAdmin || ApplicationContext.Instance.AuthenticationInfo.Role.RoleCategory == (short)RoleCategory.MaintainMonitor)
            {
                orgClient.GetAllOrganizationCompleted += orgClient_GetAllOrganizationCompleted;
                orgClient.GetAllOrganizationAsync(ApplicationContext.Instance.AuthenticationInfo.ClientID);
            }
            ApplicationContext.Instance.Logger.LogInforMession(MethodBase.GetCurrentMethod().ToString(), "begin get GetOrganizationByUserAsync");
            ApplicationContext.Instance.BusyInfo.InitLoadingNum++;
        }

        void orgClient_GetAllOrganizationCompleted(object sender, GetAllOrganizationCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                _hasLoadData = false;
                return;
            }

            if (e.Error != null)
            {
                _hasLoadData = false;
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), e.Error);
                ApplicationContext.Instance.BusyInfo.InitLoadingNum--;
                return;
            }

            try
            {
                var result = e.Result;
                if (result.IsSuccess == false)
                {
                    if (string.IsNullOrWhiteSpace(result.ErrorMsg) == false)
                    {
                        ApplicationContext.Instance.Logger.LogError(MethodBase.GetCurrentMethod().ToString(), result.ErrorMsg);
                    }

                    if (result.ExceptionMessage != null)
                    {
                        ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), result.ExceptionMessage);
                    }
                }



                if (result.Result != null)
                {
                    _authorityVehicleOrgs = result.Result;
                }
            }
            catch (System.Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
            finally
            {
                ApplicationContext.Instance.Logger.LogInforMession(MethodBase.GetCurrentMethod().ToString(), "end GetOrganizationByUserCompleted");
                ApplicationContext.Instance.BusyInfo.InitLoadingNum--;
            }
        }

        void orgClient_GetOrganizationByUserCompleted(object sender, GetOrganizationByUserCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                _hasLoadData = false;
                return;
            }

            if (e.Error != null)
            {
                _hasLoadData = false;
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), e.Error);
                ApplicationContext.Instance.BusyInfo.InitLoadingNum--;
                return;
            }

            try
            {
                var result = e.Result;
                if (result.IsSuccess == false)
                {
                    if (string.IsNullOrWhiteSpace(result.ErrorMsg) == false)
                    {
                        ApplicationContext.Instance.Logger.LogError(MethodBase.GetCurrentMethod().ToString(), result.ErrorMsg);
                    }

                    if (result.ExceptionMessage != null)
                    {
                        ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), result.ExceptionMessage);
                    }
                }



                if (result.Result != null)
                {
                    _authorityVehicleOrgs = result.Result;

                    GetInstallVehicle();
                }
            }
            catch (System.Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
            finally
            {
                ApplicationContext.Instance.Logger.LogInforMession(MethodBase.GetCurrentMethod().ToString(), "end GetOrganizationByUserCompleted");
                ApplicationContext.Instance.BusyInfo.InitLoadingNum--;
            }
        }
        private void GetInstallVehicle()
        {
            VehicleServiceClient vehicleServiceClient = ServiceClientFactory.Create<VehicleServiceClient>();
            vehicleServiceClient.GetInstallVehiclesByAuthorityCompleted += vehicleServiceClient_GetInstallVehiclesByAuthorityCompleted;

            ObservableCollection<string> organizations = new ObservableCollection<string>();
            foreach (var item in ApplicationContext.Instance.AuthenticationInfo.Organizations)
            {
                organizations.Add(item.ID);
            }

            vehicleServiceClient.GetInstallVehiclesByAuthorityAsync(organizations);
            ApplicationContext.Instance.Logger.Log(Jounce.Core.Application.LogSeverity.Information, "VechileOrganizationManager", "begin get SecuritVehiclesByAuthority");
            ApplicationContext.Instance.BusyInfo.InitLoadingNum++;
        }

        private void vehicleServiceClient_GetInstallVehiclesByAuthorityCompleted(object sender, GetInstallVehiclesByAuthorityCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                _hasLoadData = false;
                return;
            }

            if (e.Error != null)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), e.Error);
                _hasLoadData = false;
                ApplicationContext.Instance.Logger.Log(Jounce.Core.Application.LogSeverity.Information, "VechileOrganizationManager", "get vechileorganition error");
                ApplicationContext.Instance.BusyInfo.InitLoadingNum--;
                return;
            }
            try
            {
                ApplicationContext.Instance.Logger.LogInforMession("DistrictManager", "end get SecuritVehiclesByAuthority");

                var result = e.Result;
                if (result.IsSuccess == false)
                {
                    if (string.IsNullOrWhiteSpace(result.ErrorMsg) == false)
                    {
                        ApplicationContext.Instance.Logger.LogError(MethodBase.GetCurrentMethod().ToString(), result.ErrorMsg);
                    }

                    if (result.ExceptionMessage != null)
                    {
                        ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), result.ExceptionMessage);
                    }
                }

                ApplicationContext.Instance.Logger.LogInforMession(MethodBase.GetCurrentMethod().ToString(), "end GetOrganizationByUserCompleted");


                var vehicleList = result.Result;
                foreach (Gsafety.PTMS.ServiceReference.VehicleService.Vehicle item in vehicleList)
                {
                    _VehicleList.Add(InitAddVehicle(item));
                }

                if (true)
                {
                    ApplicationContext.Instance.BufferManager.AlarmManager.DataLoading();
                }

                if (true)
                {
                    ApplicationContext.Instance.BufferManager.VehicleAlertManager.DataLoading();
                }

                HasLoadVehcile = true;
            }
            catch
            {
                _hasLoadData = false;
            }
            finally
            {
                ApplicationContext.Instance.Logger.Log(Jounce.Core.Application.LogSeverity.Information, "VechileOrganizationManager", "get vechileorganition finish");
                ApplicationContext.Instance.BusyInfo.InitLoadingNum--;
            }
        }

        private Gsafety.PTMS.Bases.Models.Vehicle InitAddVehicle(Gsafety.PTMS.ServiceReference.VehicleService.Vehicle vehicle)
        {
            var vehicleModle = new Gsafety.PTMS.Bases.Models.Vehicle();
            vehicleModle.VehicleId = vehicle.VehicleId;

         
            vehicleModle.UniqueId = vehicle.MDVR_SN;

            if (string.IsNullOrEmpty(vehicle.MDVR_SN))
            {
                vehicleModle.UniqueId = vehicle.GPS_SN;
            
            }
            vehicleModle.BrandModel = vehicle.BrandModel;
            vehicleModle.EngineId = vehicle.EngineId;
            vehicleModle.Owner = vehicle.Owner;
            vehicleModle.StartYear = vehicle.StartYear;
            vehicleModle.VehicleSn = vehicle.VehicleSn;
            vehicleModle.CityCode = vehicle.CityCode;
            vehicleModle.CityName = vehicle.CityName;
            vehicleModle.ProvinceName = vehicle.ProvinceName;
            vehicleModle.OrganizationID = vehicle.OrgnizationId;
            vehicleModle.MDVROnline = vehicle.MDVROnline == null ? new Nullable<bool>() : vehicle.MDVROnline.Value == 1;
            vehicleModle.GPSOnline = vehicle.GPSOnline == null ? new Nullable<bool>() : vehicle.GPSOnline.Value == 1;
            vehicleModle.MobileOnline = vehicle.MobileOnline == null ? new Nullable<bool>() : vehicle.MobileOnline.Value == 1;
            vehicleModle.Note = vehicle.Note;
            vehicleModle.ContactPhone = vehicle.ContactPhone;
            vehicleModle.OrganizationName = vehicle.OrgnizationName;
            vehicleModle.VehicleServiceType = vehicle.ServiceType;
            vehicleModle.VehicleTypeDescribe = vehicle.VehicleTypeDescribe;
            vehicleModle.CityName = vehicle.CityName;
            vehicleModle.ProvinceName = vehicle.ProvinceName;
            vehicleModle.DistrictCode = vehicle.DistrictCode;
            vehicleModle.Ficha = vehicle.Ficha;
            vehicleModle.VehicleTypeImage = vehicle.VehicleTypeImage;
            return vehicleModle;
        }

        public Gsafety.PTMS.Bases.Models.Vehicle GetVehicle(string vehicleId)
        {
            if (VehicleList == null && VehicleList.Count == 0)
                return null;
            return VehicleList.Where(item => item.VehicleId.ToLower().Contains(vehicleId.Trim().ToLower())).FirstOrDefault();
        }

        public Gsafety.PTMS.Bases.Models.Vehicle getVehicleByMdvrid(string mdvrid)
        {
            if (_VehicleList == null || _VehicleList.Count == 0)
                return null;
            Gsafety.PTMS.Bases.Models.Vehicle vehiclel =
                _VehicleList.Where(item => item.UniqueId.Trim().Equals(mdvrid)).FirstOrDefault();
            return vehiclel;

        }

        public void HandleEvent(OnOfflineEx offlinevecile)
        {
            if (null != offlinevecile)
            {
                var vehicle = VehicleList.FirstOrDefault(n => n.VehicleId == offlinevecile.VehicleId);

                if (vehicle != null)
                {
                    bool vehicleonline = vehicle.IsOnLine;
                    if (offlinevecile.SourceMode == 0)
                    {
                        vehicle.MDVROnline = offlinevecile.IsOnline == 1;
                    }
                    else if (offlinevecile.SourceMode == 1)
                    {
                        vehicle.GPSOnline = offlinevecile.IsOnline == 1;
                    }
                    else if (offlinevecile.SourceMode == 2)
                    {
                        vehicle.MobileOnline = offlinevecile.IsOnline == 1;
                    }

                    RaiseVehicleCountChanged();
                }
            }
        }



        public void HandleEvent(DeviceMaintain maintaindevice)
        {
            ApplicationContext.Instance.Logger.LogInforMession(GetType().FullName, "Suite Repair Message");

            if (null != maintaindevice)
            {
                ApplicationContext.Instance.Logger.LogInforMession(
                    GetType().FullName,
                    "Suite Repair MDVRCOREID  Is" + maintaindevice.MdvrCoreId.Trim() + "！");

                //RemoveVehicles(maintaindevice.MdvrCoreId.Trim());
            }
        }

        public void HandleEvent(ServiceReference.MessageServiceExt.Vehicle publishedEvent)
        {
            if (null != publishedEvent)
            {
                var vehicle = VehicleList.FirstOrDefault(n => n.VehicleId == publishedEvent.VehicleId);

                if (vehicle != null)
                {
                    bool vehicleonline = vehicle.IsOnLine;

                    vehicle.MDVROnline = publishedEvent.MDVROnline == null ? new Nullable<bool>() : publishedEvent.MDVROnline.Value == 1;
                    vehicle.GPSOnline = publishedEvent.GPSOnline == null ? new Nullable<bool>() : publishedEvent.GPSOnline.Value == 1;
                    vehicle.MobileOnline = publishedEvent.MobileOnline == null ? new Nullable<bool>() : publishedEvent.MobileOnline.Value == 1;
                    vehicle.UniqueId = publishedEvent.MDVR_SN;

                    if (vehicleonline != vehicle.IsOnLine)
                    {
                        RaiseVehicleCountChanged();
                    }
                }
                else
                {
                    vehicle = new Gsafety.PTMS.Bases.Models.Vehicle();
                    vehicle.VehicleId = publishedEvent.VehicleId;
                    vehicle.UniqueId = publishedEvent.MDVR_SN;
                    vehicle.BrandModel = publishedEvent.BrandModel;
                    vehicle.EngineId = publishedEvent.EngineId;
                    vehicle.Owner = publishedEvent.Owner;
                    vehicle.StartYear = publishedEvent.StartYear;
                    vehicle.VehicleSn = publishedEvent.VehicleSn;
                    vehicle.CityCode = publishedEvent.CityCode;
                    vehicle.CityName = publishedEvent.CityName;
                    vehicle.ProvinceName = publishedEvent.ProvinceName;
                    vehicle.OrganizationID = publishedEvent.OrgnizationId;
                    vehicle.MDVROnline = publishedEvent.MDVROnline == null ? new Nullable<bool>() : publishedEvent.MDVROnline.Value == 1;
                    vehicle.GPSOnline = publishedEvent.GPSOnline == null ? new Nullable<bool>() : publishedEvent.GPSOnline.Value == 1;
                    vehicle.MobileOnline = publishedEvent.MobileOnline == null ? new Nullable<bool>() : publishedEvent.MobileOnline.Value == 1;
                    vehicle.Note = publishedEvent.Note;
                    vehicle.ContactPhone = publishedEvent.ContactPhone;
                    vehicle.OrganizationName = publishedEvent.OrgnizationName;
                    //vehicle.VehicleServiceType = publishedEvent.ServiceType;
                    vehicle.VehicleTypeDescribe = publishedEvent.VehicleTypeDescribe;
                    vehicle.VehicleTypeImage = publishedEvent.VehicleTypeImage;
                    //vehicle.CityName = publishedEvent.CityName;
                    //vehicle.ProvinceName = publishedEvent.ProvinceName;
                    vehicle.DistrictCode = publishedEvent.DistrictCode;
                    if (vehicle.DistrictCode.Length == 5)
                    {
                        string provicecode = vehicle.DistrictCode.Substring(0, 2);
                        var province = ApplicationContext.Instance.BufferManager.DistrictManager.Provinces.FirstOrDefault(n => n.Code == provicecode);
                        if (province != null)
                        {
                            vehicle.ProvinceName = province.Name;
                        }

                        var city = ApplicationContext.Instance.BufferManager.DistrictManager.Cities.FirstOrDefault(n => n.Code == vehicle.DistrictCode);
                        if (city != null)
                        {
                            vehicle.CityName = city.Name;
                        }

                    }
                    else if (vehicle.DistrictCode.Length == 2)
                    {
                        var province = ApplicationContext.Instance.BufferManager.DistrictManager.Provinces.FirstOrDefault(n => n.Code == vehicle.DistrictCode);
                        if (province != null)
                        {
                            vehicle.ProvinceName = province.Name;
                        }
                    }

                    VehicleList.Add(vehicle);
                    DeviceInstallVehicle installvehicle = new DeviceInstallVehicle();
                    installvehicle.Vehicle = vehicle;
                    EventAggregator.Publish(installvehicle);

                    var org = AuthorityVehicleOrgs.FirstOrDefault(n => n.ID == vehicle.OrganizationID);
                    if (org != null)
                    {
                        RaiseVehicleCountChanged();
                    }
                }
            }
        }
    }
}
