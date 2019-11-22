using Jounce.Core.ViewModel;
/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: b17c9c7a-e50c-4454-bc4d-f0e71b103bdf      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-DENGZL
/////                 Author: TEST(dengzl)
/////======================================================================
/////           Project Name: Gsafety.PTMS.BaseInformation
/////    Project Description:    
/////             Class Name: BaseInformationBinding
/////          Class Version: v1.0.0.0
/////            Create Time: 7/24/2013 5:24:21 PM
/////      Class Description:  
/////======================================================================
/////          Modified Time: 7/24/2013 5:24:21 PM
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System.ComponentModel.Composition;

namespace Gsafety.PTMS.BaseInformation
{
    public class BaseInformationBinding
    {
        [Export]
        public ViewModelRoute BindingVehicle
        {
            get
            {
                return ViewModelRoute.Create(BaseInformationName.VehicleVm, BaseInformationName.VehicleV);
            }
        }

        [Export]
        public ViewModelRoute BindingVehicleAdd
        {
            get
            {
                return ViewModelRoute.Create(BaseInformationName.VehicleManageVm, BaseInformationName.VehicleInfoManageV);
            }
        }

        [Export]
        public ViewModelRoute BindingSetupStation
        {
            get
            {
                return ViewModelRoute.Create(BaseInformationName.SetupStationVm, BaseInformationName.SetupStationV);
            }
        }

        [Export]
        public ViewModelRoute BindingSetupStationManage
        {
            get
            {
                return ViewModelRoute.Create(BaseInformationName.SetupStationManageVm, BaseInformationName.SetupStationManageV);
            }
        }

        [Export]
        public ViewModelRoute BindingSuiteInfo
        {
            get
            {
                return ViewModelRoute.Create(BaseInformationName.SuiteInfoVm, BaseInformationName.SuiteInfoV);
            }
        }

        [Export]
        public ViewModelRoute BindingSuiteManage
        {
            get
            {
                return ViewModelRoute.Create(BaseInformationName.SuiteInfoManageVm, BaseInformationName.SuiteInfoManageV);
            }
        }

        /// <summary>
        /// 实现ANT产品的主界面和其ViewModel的绑定
        /// 创建者刘昌在
        /// </summary>
        [Export]
        public ViewModelRoute BindingAntProductMainPage
        {
            get
            {
                return ViewModelRoute.Create(BaseInformationName.AntProductBaseInfoMainPageVm, BaseInformationName.AntProductBaseInfoMainPageV);
            }
        }



        [Export]
        public ViewModelRoute BindingInstallPlace
        {
            get
            {
                return ViewModelRoute.Create(BaseInformationName.InstallPlaceVm, BaseInformationName.InstallPlaceV);
            }
        }

        [Export]
        public ViewModelRoute BindingInstallPlaceDetail
        {
            get
            {
                return ViewModelRoute.Create(BaseInformationName.InstallPlaceDetailViewVm, BaseInformationName.InstallPlaceDetailViewV);
            }
        }

        /// <summary>
        /// 安全套件管理界面
        /// </summary>
        [Export]
        public ViewModelRoute BindingSafeDeviceInfo
        {
            get
            {
                return ViewModelRoute.Create(BaseInformationName.SafeDeviceInfoVm, BaseInformationName.SafeDeviceInfoV);
            }
        }

        /// <summary>
        /// 安全套件明细界面
        /// </summary>
        [Export]
        public ViewModelRoute BindingSafeDeviceInfoDetail
        {
            get
            {
                return ViewModelRoute.Create(BaseInformationName.SafeDeviceInfoDetailVm, BaseInformationName.SafeDeviceInfoDetailV);
            }
        }

        /// <summary>
        /// 安全套件配件明细界面
        /// </summary>
        [Export]
        public ViewModelRoute BindingSafeDevicePartDetail
        {
            get
            {
                return ViewModelRoute.Create(BaseInformationName.SafeDevicePartDetailVm, BaseInformationName.SafeDevicePartDetailV);
            }
        }


        [Export]
        public ViewModelRoute InstallPlaceBinding
        {
            get
            {
                return ViewModelRoute.Create(BaseInformationName.InstallPlaceBindingVm, BaseInformationName.InstallPlaceBindingV);
            }
        }

        /// <summary>
        /// 驾驶员list界面
        /// </summary>
        [Export]
        public ViewModelRoute BindingDriverInfo
        {
            get
            {
                return ViewModelRoute.Create(BaseInformationName.DriverInfoVm, BaseInformationName.DriverInfoV);
            }
        }
        /// <summary>
        /// 驾驶员明细界面
        /// </summary>
        [Export]
        public ViewModelRoute BindingDriverInfoDetail
        {
            get
            {
                return ViewModelRoute.Create(BaseInformationName.DriverInfoDetailVm, BaseInformationName.DriverInfoDetailV);
            }
        }
        /// <summary>
        /// 驾驶员绑定界面
        /// </summary>
        [Export]
        public ViewModelRoute BindingDriverInfoVehicle
        {
            get
            {
                return ViewModelRoute.Create(BaseInformationName.DriverInfoBindingVm, BaseInformationName.DriverInfoBindingV);
            }
        }


        /// <summary>
        /// DevGps绑定界面
        /// </summary>
        [Export]
        public ViewModelRoute BindingDevGpsMange
        {
            get
            {
                return ViewModelRoute.Create(BaseInformationName.DevGpsManageViewVm, BaseInformationName.DevGpsManageViewV);
            }
        }

        /// <summary>
        /// DevGpsDetail绑定界面
        /// </summary>
        [Export]
        public ViewModelRoute BindingDevGpsDetail
        {
            get
            {
                return ViewModelRoute.Create(BaseInformationName.DevGpsDetailViewVm, BaseInformationName.DevGpsDetailViewV);
            }
        }

        /// <summary>
        /// AntProduct车辆管理V和Vm绑定
        /// </summary>
        [Export]
        public ViewModelRoute BindVehicleManageRoute
        {
            get
            {
                return ViewModelRoute.Create(BaseInformationName.VehicleManageVm, BaseInformationName.VehicleManageV);
            }
        }

        /// <summary>
        /// AntProduct车辆信息V和Vm绑定
        /// </summary>
        [Export]
        public ViewModelRoute BindVehicleRoute
        {
            get
            {
                return ViewModelRoute.Create(BaseInformationName.VehicleDetailVm, BaseInformationName.VehicleDetailV);
            }
        }

        [Export]
        public ViewModelRoute AntProductVehicleDepartmentViewBinding
        {
            get { return ViewModelRoute.Create(BaseInformationName.AntProductVehicleDepartmentVm, BaseInformationName.AntProductVehicleDepartmentV); }
        }

        [Export]
        public ViewModelRoute AntProductAddVehicleDepartmentViewBinding
        {
            get
            {
                return ViewModelRoute.Create(BaseInformationName.AntProductAddVehicleDepartmentVm, BaseInformationName.AntProductAddVehicleDepartmentV);
            }
        }

        [Export]
        public ViewModelRoute AddVehicleInfoFromDepartmentViewBing
        {
            get { return ViewModelRoute.Create(BaseInformationName.AddVehicleInfoFromDepartmentVm, BaseInformationName.AddVehicleInfoFromDepartmentV); }
        }

        [Export]
        public ViewModelRoute VehicleDepartmentListViewBing
        {
            get
            {
                return ViewModelRoute.Create(BaseInformationName.VehicleDepartmentListVm, BaseInformationName.VehicleDepartmentListV);
            }
        }


        #region Test Code
        [Export]
        public ViewModelRoute VehicleTest
        {
            get
            {
                return ViewModelRoute.Create(BaseInformationName.VehicleTestVM, BaseInformationName.VehicleTest);
            }
        }
        #endregion
    }
}
