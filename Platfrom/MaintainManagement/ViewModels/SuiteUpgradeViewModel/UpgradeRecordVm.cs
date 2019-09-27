using Gsafety.PTMS.ServiceReference.DistrictService;
using Gsafety.PTMS.Share;
using Jounce.Core.Command;
using Jounce.Core.ViewModel;
using Jounce.Framework.Command;
/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: caa6828f-3aff-4572-8036-2349d3467fa7      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-PENGGL
/////                 Author: TEST(penggl)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Maintain.ViewModels
/////    Project Description:    
/////             Class Name: SuiteUpgradeRecordVm
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/9/17 15:05:06
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/9/17 15:05:06
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System;
using System.Collections.Generic;
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
//using Gsafety.PTMS.ServiceReference.VehicleCompanyService;
using Gsafety.PTMS.ServiceReference.VehicleService;
using System.Collections.ObjectModel;
using Gsafety.PTMS.ServiceReference.UpdateService;
using Jounce.Core.View;

namespace Gsafety.PTMS.Maintain.ViewModels
{
    [ExportAsViewModel(MaintainName.UpgradeRecordVm)]
    public class UpgradeRecordVm : BaseViewModel
    {
        private DistrictServiceClient districtClient = ServiceClientFactory.Create<DistrictServiceClient>();
        private UpdateServiceClient updateClient = ServiceClientFactory.Create<UpdateServiceClient>();
        //private VehicleCompanyServiceClient companyClient = ServiceClientFactory.Create<VehicleCompanyServiceClient>();
        public SuiteUpdateRecord CurrentUpgradeRecord { get; set; }
        public PagedServerCollection<SuiteUpdateRecord> PSC_UpgradeRecord { get; set; }
        public List<District> ProvinceAndCityList { get; set; }
        public List<District> ProvinceList { get; set; }
        public List<District> CityList { get; set; }
        //public List<VehicleCompany> CompanyList { get; set; }
        //public VehicleCompany CurrentCompany { get; set; }
        //public ObservableCollection<VehicleCompany> AllCompanyList { get; set; }
        private int pageSizeValue;
        public int PageSizeValue
        {
            get { return pageSizeValue; }
            set
            {
                pageSizeValue = value;
                if (this.PSC_UpgradeRecord != null)
                {
                    this.PSC_UpgradeRecord.PageSize = pageSizeValue;
                }
            }
        }
        public List<int> PageSizeList { get; set; }
        public string VehicleId { get; set; }
        private DateTime? startTime;
        private DateTime? endTime;
        private string companyID;
        private string districtCode;
        private string vehicleId;
        private DateTime _BeginDate = DateTime.Now.AddDays(-1);
        public DateTime BeginDate
        {
            get
            {
                return _BeginDate;
            }
            set
            {
                _BeginDate = value;
            }
        }

        private DateTime _EndDate = DateTime.Now;
        public DateTime EndDate
        {
            get
            {
                return _EndDate;
            }
            set
            {
                _EndDate = value.AddHours(24); ;
            }
        }
        public IActionCommand UpdateCommand { get; private set; }
        public IActionCommand DeleteCommand { get; private set; }
        public IActionCommand QueryCommand { get; private set; }
        public IActionCommand ViewCommand { get; private set; }
        public IActionCommand AddCommand { get; private set; }
        public IActionCommand DownloadCommand { get; private set; }
        public IActionCommand UploadCommand { get; private set; }

        protected override void ActivateView(string viewName, System.Collections.Generic.IDictionary<string, object> viewParameters)
        {
            base.ActivateView(viewName, viewParameters);
            if (viewParameters.Count != 0 && viewParameters["action"].ToString() == "return")
            {
                PSC_UpgradeRecord.ToPage(currentIndex);
            }
            else
            {
                //companyClient.GetVehicleCompaniesAsync();
            }
        }

        public UpgradeRecordVm()
        {
            districtClient.GetProvinceAndCityCompleted += districtClient_GetProvinceAndCityCompleted;
            districtClient.GetProvinceAndCityAsync();

            //companyClient.GetVehicleCompaniesCompleted += companyClient_GetVehicleCompaniesCompleted;
            AddCommand = new ActionCommand<object>(obj => Publish("add"));
            UpdateCommand = new ActionCommand<object>(obj => Publish("update"));
            ViewCommand = new ActionCommand<object>(obj => Publish("view"));
            QueryCommand = new ActionCommand<object>(obj => Query());

            PageSizeList = MaintainCommon.PageSizeList; //获取翻页控件下拉列表
            PageSizeValue = PageSizeList[0];//获取默认每页显示记录的数据
            InitPagedServerCollection();
        }

        void districtClient_GetProvinceAndCityCompleted(object sender, GetProvinceAndCityCompletedEventArgs e)
        {
            ProvinceAndCityList = new List<District>(e.Result.Result);

            GetProvinceAndCity();//获取全部区域
        }

        private int currentIndex = 1;
        private void InitPagedServerCollection()
        {
            updateClient.GetSuiteUpdateRecordsFuzzyCompleted += updateClient_GetSuiteUpdateRecordsFuzzyCompleted;

            PSC_UpgradeRecord = new PagedServerCollection<SuiteUpdateRecord>((pageIndex, pageSize) =>
            {
                pageSize = PageSizeValue;
                System.Threading.Interlocked.Exchange(ref currentIndex, pageIndex);
                Gsafety.PTMS.ServiceReference.UpdateService.PagingInfo pagingInfo = new Gsafety.PTMS.ServiceReference.UpdateService.PagingInfo() { PageIndex = pageIndex, PageSize = pageSize };
                updateClient.GetSuiteUpdateRecordsFuzzyAsync(districtCode, companyID, startTime, endTime, pagingInfo);
            });
        }


        void updateClient_GetSuiteUpdateRecordsFuzzyCompleted(object sender, GetSuiteUpdateRecordsFuzzyCompletedEventArgs e)
        {
            try
            {
                PSC_UpgradeRecord.loader_Finished(new PagedResult<SuiteUpdateRecord>
                {
                    Count = e.Result.TotalRecord,
                    Items = e.Result.Result,
                    PageIndex = currentIndex,
                });
                if (e.Result.TotalRecord == 0)
                {
                    MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("MAINTAIN_DataEmpty"));
                }
            }
            catch
            {
                //MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("BASEINFO_Operate_Failed"));
            }
        }


        private void Query()
        {
            companyID = string.Empty;
            districtCode = string.Empty;

            //if (CurrentCompany != null && CurrentCompany.Id != string.Empty)
            //{
            //    companyID = CurrentCompany.Id;
            //}
            //else
            if (CurrentCity != null && CurrentCity.Code != string.Empty)
            {
                districtCode = CurrentCity.Code;
            }
            else if (CurrentProvince != null && CurrentProvince.Code != string.Empty)
            {
                districtCode = CurrentProvince.Code;
            }

            vehicleId = VehicleId == null ? string.Empty : VehicleId.Trim();
            startTime = BeginDate;
            endTime = EndDate;
            
            currentIndex = 1;
            PSC_UpgradeRecord.MoveToFirstPage();
        }

        private void Publish(string name)
        {
            EventAggregator.Publish(new ViewNavigationArgs(MaintainName.UpgradeRecordDisplayV, new Dictionary<string, object>() { { "action", name }, { name, CurrentUpgradeRecord } }));
        }


        #region 省市公司联动下拉菜单

        //void companyClient_GetVehicleCompaniesCompleted(object sender, GetVehicleCompaniesCompletedEventArgs e)
        //{
        //    AllCompanyList = new ObservableCollection<VehicleCompany>(e.Result.Result);

        //    CompanyList = new List<VehicleCompany>();
        //    CompanyList.Add(new VehicleCompany { Name = ApplicationContext.Instance.StringResourceReader.GetString("BASEINFO_All"), Id = string.Empty });
        //    CompanyList.AddRange(AllCompanyList);
        //    Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => CompanyList));
        //    CurrentCompany = CompanyList[0];
        //    Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => CurrentCompany));
        //}

        private void GetProvinceAndCity()
        {
            ProvinceList = new List<District>();
            ProvinceList.Add(new District { Name = ApplicationContext.Instance.StringResourceReader.GetString("BASEINFO_All"), Code = string.Empty });
            ProvinceAndCityList.Where(x => x.Code.Length == 2).ToList().ForEach(x => ProvinceList.Add(x));
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
                }
            }
        }

        public void GetCityByProvinceCode(string provinceCode)
        {
            CityList = new List<District>();
            CityList.Add(new District { Name = ApplicationContext.Instance.StringResourceReader.GetString("BASEINFO_All"), Code = string.Empty });

            if (!string.IsNullOrEmpty(provinceCode))
            {
                ProvinceAndCityList.Where(x => x.Code.Length == 5 && x.Code.StartsWith(provinceCode)).ToList().ForEach(x => CityList.Add(x));
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
                //if (currentCity != null)
                //{
                //    GetCompany();
                //}
            }
        }

        //public void GetCompany()
        //{
        //    if (AllCompanyList == null)
        //    {
        //        return;
        //    }
        //    CompanyList = new List<VehicleCompany>();
        //    CompanyList.Add(new VehicleCompany { Name = ApplicationContext.Instance.StringResourceReader.GetString("All"), Id = string.Empty });
        //    if (string.IsNullOrEmpty(CurrentProvince.Code))
        //    {
        //        CompanyList.AddRange(AllCompanyList);
        //    }
        //    else if (string.IsNullOrEmpty(CurrentCity.Code))
        //    {
        //        //var result = from x in CompanyModels
        //        //             join y in DistrictModels on x.RegionCode equals y.Code
        //        //             where y.ParentCode == CurrentProvince.Code
        //        //             select x;
        //        var result = from x in AllCompanyList where ProvinceAndCityList.Any(y => y.Code == x.CityCode && y.Code.Length == 5 && y.Code.StartsWith(CurrentProvince.Code)) select x;
        //        CompanyList.AddRange(result);
        //    }
        //    else
        //    {
        //        CompanyList.AddRange(AllCompanyList.Where(x => x.CityCode == CurrentCity.Code));
        //    }

        //    Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => CompanyList));
        //    CurrentCompany = CompanyList[0];
        //    Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => CurrentCompany));
        //}

        #endregion
    }
}
