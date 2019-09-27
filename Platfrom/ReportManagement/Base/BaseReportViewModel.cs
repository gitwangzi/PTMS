using DevExpress.Xpf.Printing;
using Gsafety.Common.Controls;
using Gsafety.Common.Utilities;
using Gsafety.PTMS.BasicPage.Views;
using Gsafety.PTMS.Report.Repository;
using Gsafety.PTMS.ServiceReference.DistrictService;
using Gsafety.PTMS.ServiceReference.VehicleService;
using Gsafety.PTMS.Share;
using Jounce.Core.ViewModel;
using Jounce.Framework;
/////Copyright (C) Gsafety 2014 .All Rights Reserved.
/////======================================================================
/////                   Guid: 0222916f-19c6-4461-bba3-626c2e081cac      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-ZHOUDD
/////                 Author: GJSY(zhoudd)
/////======================================================================
/////           Project Name: Gsafety.PTMS.ReportManager.Base
/////    Project Description:    
/////             Class Name: BaseViewModel
/////          Class Version: v1.0.0.0
/////            Create Time: 2014/10/17 11:36:44
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2014/10/17 11:36:44
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
namespace Gsafety.PTMS.ReportManager.Base
{
    public class BaseReportViewModel : BaseViewModel
    {
        #region Property
        readonly ReportPreviewModel _previewModel;
        public IDocumentPreviewModel PreviewModel { get { return _previewModel; } }
        readonly DevExpress.Xpf.Mvvm.DelegateCommand<object> _sercher;
        public ICommand SearcherCommand { get { return _sercher; } }
        int groupmode = -1;
        protected int GroupMode
        {
            get
            {
                return groupmode;
            }
            set
            {
                groupmode = value;
            }
        }

        readonly DevExpress.Xpf.Mvvm.DelegateCommand<object> _selectOrganizationCommand;
        public ICommand SelectOrganizationCommand
        {
            get
            {
                return this._selectOrganizationCommand;
            }
        }

        DateTime now = DateTime.Now;
        public DateTime Now
        {
            get { return now; }
        }

        DateTime? beginTime = DateTime.Now.Date;
        public DateTime? BeginTime
        {
            get
            {
                return beginTime;
            }
            set
            {
                if (value != beginTime)
                {
                    beginTime = value;
                    RaisePropertyChanged("BeginTime");
                }

            }
        }
        DateTime endTime = DateTime.Now;
        public DateTime EndTime
        {
            get { return endTime; }
            set
            {
                if (value != endTime)
                {
                    endTime = value;
                    RaisePropertyChanged("EndTime");
                }
            }
        }

        bool enableSearch = true;
        public bool EnableSearch
        {
            get { return enableSearch; }
            set
            {
                if (value != enableSearch)
                {
                    enableSearch = value;
                    RaisePropertyChanged("EnableSearch");
                }
            }
        }

        bool enabledStartDate = true;
        public bool EnabledStartDate
        {
            get { return enabledStartDate; }
            set
            {
                if (value != enabledStartDate)
                {
                    enabledStartDate = value;
                    RaisePropertyChanged("EnabledStartDate");
                }
            }
        }

        bool enabledEndDate = true;
        public bool EnabledEndDate
        {
            get { return enabledEndDate; }
            set
            {
                if (value != enabledEndDate)
                {
                    enabledEndDate = value;
                    RaisePropertyChanged("EnabledEndDate");
                }
            }
        }

        private string _organizationName;
        /// <summary>
        /// 前台显示的组织机构名称
        /// </summary>
        public string OrganizationName
        {
            get
            {
                return this._organizationName;
            }
            set
            {
                this._organizationName = value;
                RaisePropertyChanged(() => OrganizationName);
            }
        }

        protected string _organizationID = string.Empty;

        protected void InitProvinces()
        {
            var _provices = ApplicationContext.Instance.BufferManager.DistrictManager.DistrictByAuthority.Where(x => x.Code.Length == 2).ToList().OrderBy(x => x.Name);
            foreach (var item in _provices)
            {
                Provinces.Add(item);
            }
            //Provinces.Insert(0, new District() { Code = "", Name = "全部" });
            Provinces.Insert(0, new District() { Code = "", Name = ApplicationContext.Instance.StringResourceReader.GetString("All") });
            JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => Provinces));

            Province = Provinces[0];
        }

        protected void InitCities(string provinceCode)
        {
            Cities = new List<District>();
            if (!string.IsNullOrEmpty(provinceCode))
            {
                var _cities = ApplicationContext.Instance.BufferManager.DistrictManager.DistrictByAuthority.Where(x => x.Code.Length == 5 && x.Code.StartsWith(provinceCode)).ToList().OrderBy(x => x.Name);
                foreach (var item in _cities)
                {
                    Cities.Add(item);
                }
            }

            //Cities.Insert(0, new District() { Code = "", Name = "全部" });
            Cities.Insert(0, new District() { Code = "", Name = ApplicationContext.Instance.StringResourceReader.GetString("All") });
            JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => Cities));

            City = Cities[0];
        }

        protected void InitVehicles(string CityCode)
        {
            Vehicles = new List<Gsafety.PTMS.Bases.Models.Vehicle>();
            if (!string.IsNullOrEmpty(CityCode))
            {

                if (VehicleType != null)
                {
                    var _vehicles = ApplicationContext.Instance.BufferManager.VehicleOrganizationManage.VehicleList.Where(x => x.DistrictCode == CityCode&&x.VehicleTypeDescribe==VehicleType.Name).ToList();
                    foreach (var item in _vehicles)
                    {
                        Vehicles.Add(item);
                    }

                }
                else
                {
                    var _vehicles = ApplicationContext.Instance.BufferManager.VehicleOrganizationManage.VehicleList.Where(x => x.DistrictCode == CityCode).ToList();
                    foreach (var item in _vehicles)
                    {
                        Vehicles.Add(item);
                    }
                }
            }

            //Cities.Insert(0, new District() { Code = "", Name = "全部" });
            Vehicles.Insert(0, new Gsafety.PTMS.Bases.Models.Vehicle() {  ID = "", VehicleId = ApplicationContext.Instance.StringResourceReader.GetString("All") });
            JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => Vehicles));

            Vehicle = Vehicles[0];
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
                RaisePropertyChanged(() => City);
                if (city != null)
                {
                    InitVehicles(city.Code);
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

        private List<Gsafety.PTMS.Bases.Models.Vehicle> vehicles = new List<Gsafety.PTMS.Bases.Models.Vehicle>();
        /// <summary>
        /// 所有车辆列表
        /// </summary>
        public List<Gsafety.PTMS.Bases.Models.Vehicle> Vehicles
        {
            get { return vehicles; }
            set
            {
                vehicles = value;
                RaisePropertyChanged(() => Vehicles);
            }
        }

        private Gsafety.PTMS.Bases.Models.Vehicle vehicle;
        /// <summary>
        /// 市级车辆列表
        /// </summary>
        public Gsafety.PTMS.Bases.Models.Vehicle Vehicle
        {
            get { return vehicle; }
            set
            {
                vehicle = value;
                RaisePropertyChanged(() => Vehicle);
            }
        }

        private List<VehicleType> vehicletypes = new List<VehicleType>();
        /// <summary>
        /// 所有省份
        /// </summary>
        public List<VehicleType> VehicleTypes
        {
            get { return vehicletypes; }
            set
            {
                vehicletypes = value;
                RaisePropertyChanged(() => VehicleTypes);
            }
        }

        private VehicleType vehicletype = null;

        public VehicleType VehicleType
        {
            get { return vehicletype; }
            set
            {
                vehicletype = value;
                RaisePropertyChanged(() => VehicleType);
                if (city != null)
                {
                    InitVehicles(city.Code);
                }

            }
        }


        #endregion
        public string PreviewModelServiceUri { get; set; }
        public string PreviewModelReportName { get; set; }
        public BaseReportViewModel(string reportName)
        {
            try
            {
                _previewModel = new ReportPreviewModel()
                    {
                        ServiceUri = ServiceClientFactory.ServiceConfig("ReportServiceClient"),
                        ReportName = reportName
                    };
                _sercher = new DevExpress.Xpf.Mvvm.DelegateCommand<object>(Searchered);

                this.OrganizationName = "";
                this._selectOrganizationCommand = new DevExpress.Xpf.Mvvm.DelegateCommand<object>(SelectOrganizationCommandExecute);

                InitProvinces();

                InitVehicleType();
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("BaseReportViewModel.BaseReportViewModel()", ex);
            }
        }

        private void InitVehicleType()
        {
            if (VehicleTypeManager.VehicleTypes == null)
            {
                VehicleTypeClient client = ServiceClientFactory.Create<VehicleTypeClient>();
                client.GetVehicleTypeListCompleted += client_GetVehicleTypeListCompleted;
                client.GetVehicleTypeListAsync(ApplicationContext.Instance.AuthenticationInfo.ClientID);
            }
            else
            {
                PopulateVehicleTypes();
            }
        }

        void client_GetVehicleTypeListCompleted(object sender, GetVehicleTypeListCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                VehicleTypeManager.VehicleTypes = new List<VehicleType>();
                foreach (var item in e.Result.Result)
                {
                    VehicleTypeManager.VehicleTypes.Add(item);
                }

                PopulateVehicleTypes();
            }
        }

        private void PopulateVehicleTypes()
        {
            foreach (var item in VehicleTypeManager.VehicleTypes)
            {
                vehicletypes.Add(item);
            }

            vehicletypes.Insert(0, new VehicleType() { ID = string.Empty, Name = ApplicationContext.Instance.StringResourceReader.GetString("All") });

            JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => VehicleTypes));

            VehicleType = VehicleTypes[0];

            JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => VehicleType));
        }

        SelecSignleOrganizationWindow window = null;
        private void SelectOrganizationCommandExecute(object obj)
        {
            window = new SelecSignleOrganizationWindow(ApplicationContext.Instance.AuthenticationInfo.UserID);
            window.Show();
            window.Closed += window_Closed;
        }

        void window_Closed(object sender, EventArgs e)
        {
            if (window.DialogResult == true)
            {
                this.OrganizationName = "Selected";
                var result = window._viewModel.SelectedOrganizationItem;
                if (result != null)
                {
                    this.OrganizationName = result.Name;
                    _organizationID = result.ID;
                }
            }
        }

        public virtual void Searchered(object obj)
        {
            try
            {
                if (!Validate())
                {
                    return;
                }

                ReportWhereInfo whereInfo = GenerateParameter();
                _previewModel.Parameters["whereInfo"].Value = JsonHelper.ToJsonString(whereInfo);
                _previewModel.CreateDocument();
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("BaseReportViewModel.Searchered", ex);
            }
        }

        protected virtual ReportWhereInfo GenerateParameter()
        {
            ReportWhereInfo whereInfo = new ReportWhereInfo();

            if (string.IsNullOrEmpty(Province.Code))
            {
                whereInfo.Province = null;
            }
            else
            {
                whereInfo.Province = Province.Code;

                if (string.IsNullOrEmpty(City.Code))
                {
                    whereInfo.City = null;
                }
                else
                {
                    whereInfo.City = City.Code;

                    if (Vehicle.ID=="")
                    {
                        whereInfo.VehicleId = null;
                    }
                    else
                    {
                        whereInfo.VehicleId = Vehicle.VehicleId;
                    }
                }
            }

            whereInfo.BeginTime = BeginTime.Value.Date.ToUniversalTime();
            whereInfo.EndTime = EndTime.Date.AddDays(1).ToUniversalTime();
            whereInfo.BeginTimeLocal = BeginTime.Value.Date;
            whereInfo.EndTimeLocal = EndTime.Date;
            if (_organizationID.Trim() == string.Empty)
            {
                foreach (var item in ApplicationContext.Instance.BufferManager.VehicleOrganizationManage.AuthorityVehicleOrgs)
                {
                    whereInfo.OrgCode += "'" + item.ID + "',";
                }

                whereInfo.OrgCode = whereInfo.OrgCode.TrimEnd(",".ToCharArray());
            }
            else
            {
                whereInfo.OrgCode = "'" + _organizationID + "'";
            }
            whereInfo.TimeZone = TimeZoneInfo.Local.BaseUtcOffset.TotalHours;
            whereInfo.ClientID = ApplicationContext.Instance.AuthenticationInfo.ClientID;
            whereInfo.GroupMode = GroupMode;
            whereInfo.LocalTime = DateTime.Now;

            if (!string.IsNullOrEmpty(VehicleType.ID))
            {
                whereInfo.VehicleType = VehicleType.ID;
            }
            else
            {
                whereInfo.VehicleType = null;
            }


            return whereInfo;
        }

        protected virtual bool Validate()
        {
            if (BeginTime.Value > endTime)
            {
                MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString("MAINPAGE_Report"), ApplicationContext.Instance.StringResourceReader.GetString("VDM_TimeSettingError"), MessageDialogButton.Ok);
                return false;
            }
            if (endTime > DateTime.Now)
            {
                MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString("MAINPAGE_Report"), ApplicationContext.Instance.StringResourceReader.GetString("Rpt_EndTimeError"), MessageDialogButton.Ok);
                return false;
            }

            return true;
        }
    }
}
