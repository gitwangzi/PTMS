/////Copyright (C) Gsafety 2014 .All Rights Reserved.
/////======================================================================
/////                   Guid: f3c18a0e-661b-4e0e-b575-d9bebc2f9aa1      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-ZHOUDD
/////                 Author: GJSY(zhoudd)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Manager
/////    Project Description:    
/////             Class Name: ReportBinding
/////          Class Version: v1.0.0.0
/////            Create Time: 2014/7/24 14:54:12
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2014/7/24 14:54:12
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System.ComponentModel.Composition;
using Jounce.Core.ViewModel;

namespace Gsafety.PTMS.ReportManager
{
    public class ReportBinding
    {
        [Export]
        public ViewModelRoute ReportMenu
        {
            get
            {
                return ViewModelRoute.Create(ReportName.ReportMenuVm, ReportName.ReportMenuV);
            }
        }
        [Export]
        public ViewModelRoute MainPage
        {
            get
            {
                return ViewModelRoute.Create(ReportName.ReportMainPageVm, ReportName.ReportMainPageV);
            }
        }

        [Export]
        public ViewModelRoute AlarmReportView
        {
            get
            {
                return ViewModelRoute.Create(ReportName.Vehicle_AlarmRptV, ReportName.Vehicle_AlarmRptV);
            }
        }
        [Export]
        public ViewModelRoute DeviceSuiteStatusReport
        {
            get
            {
                return ViewModelRoute.Create(ReportName.DeviceSuiteStatusRptVM, ReportName.DeviceSuiteStatusRptV);
            }
        }
        [Export]
        public ViewModelRoute VehicleOnTimeRpt
        {
            get
            {
                return ViewModelRoute.Create(ReportName.VehicleOnTimeRptVM, ReportName.VehicleOnTimeRptV);
            }
        }

        [Export]
        public ViewModelRoute Vehicle_AlertRpt
        {
            get
            {
                return ViewModelRoute.Create(ReportName.Vehicle_AlertRptVM, ReportName.Vehicle_AlertRptV);
            }
        }
        [Export]
        public ViewModelRoute External_AccessRpt
        {
            get
            {
                return ViewModelRoute.Create(ReportName.External_AccessRptVM, ReportName.External_AccessRptV);
            }
        }


        [Export]
        public ViewModelRoute ExceptionSuiteRpt
        {
            get
            {
                return ViewModelRoute.Create(ReportName.ExceptionSuiteRptVM, ReportName.ExceptionSuiteRptV);
            }
        }
        [Export]
        public ViewModelRoute Internal_AccessRpt
        {
            get
            {
                return ViewModelRoute.Create(ReportName.Internal_AccessRptVM, ReportName.Internal_AccessRptV);
            }
        }
        [Export]
        public ViewModelRoute Device_AlertRpt
        {
            get
            {
                return ViewModelRoute.Create(ReportName.Device_AlertRptVM, ReportName.Device_AlertRptV);
            }
        }

        [Export]
        public ViewModelRoute AlarmByYear
        {
            get
            {
                return ViewModelRoute.Create(ReportName.AlarmByYearVm, ReportName.AlarmByYearV);
            }
        }

        [Export]
        public ViewModelRoute AlarmByMonth
        {
            get
            {
                return ViewModelRoute.Create(ReportName.AlarmByMonthVm, ReportName.AlarmByMonthV);
            }
        }

        [Export]
        public ViewModelRoute AlarmByWeek
        {
            get
            {
                return ViewModelRoute.Create(ReportName.AlarmByWeekVm, ReportName.AlarmByWeekV);
            }
        }

        [Export]
        public ViewModelRoute AlarmByDay
        {
            get
            {
                return ViewModelRoute.Create(ReportName.AlarmByDayVm, ReportName.AlarmByDayV);
            }
        }

        [Export]
        public ViewModelRoute AlarmByOrganization
        {
            get
            {
                return ViewModelRoute.Create(ReportName.AlarmByOrganizationVm, ReportName.AlarmByOrganizationV);
            }
        }

        [Export]
        public ViewModelRoute AlarmByProvince
        {
            get
            {
                return ViewModelRoute.Create(ReportName.AlarmByProvinceVm, ReportName.AlarmByProvinceV);
            }
        }

        [Export]
        public ViewModelRoute AlarmByCity
        {
            get
            {
                return ViewModelRoute.Create(ReportName.AlarmByCityVm, ReportName.AlarmByCityV);
            }
        }

        [Export]
        public ViewModelRoute AlarmByVehicle
        {
            get
            {
                return ViewModelRoute.Create(ReportName.AlarmByVehicleVm, ReportName.AlarmByVehicleV);
            }
        }

        [Export]
        public ViewModelRoute BusinessAlertByYear
        {
            get
            {
                return ViewModelRoute.Create(ReportName.BusinessAlertByYearVm, ReportName.BusinessAlertByYearV);
            }
        }

        [Export]
        public ViewModelRoute BusinessAlertByMonth
        {
            get
            {
                return ViewModelRoute.Create(ReportName.BusinessAlertByMonthVm, ReportName.BusinessAlertByMonthV);
            }
        }

        [Export]
        public ViewModelRoute BusinessAlertByWeek
        {
            get
            {
                return ViewModelRoute.Create(ReportName.BusinessAlertByWeekVm, ReportName.BusinessAlertByWeekV);
            }
        }

        [Export]
        public ViewModelRoute BusinessAlertByDay
        {
            get
            {
                return ViewModelRoute.Create(ReportName.BusinessAlertByDayVm, ReportName.BusinessAlertByDayV);
            }
        }

        [Export]
        public ViewModelRoute BusinessAlertByOrganization
        {
            get
            {
                return ViewModelRoute.Create(ReportName.BusinessAlertByOrganizationVm, ReportName.BusinessAlertByOrganizationV);
            }
        }

        [Export]
        public ViewModelRoute BusinessAlertByProvince
        {
            get
            {
                return ViewModelRoute.Create(ReportName.BusinessAlertByProvinceVm, ReportName.BusinessAlertByProvinceV);
            }
        }

        [Export]
        public ViewModelRoute BusinessAlertByCity
        {
            get
            {
                return ViewModelRoute.Create(ReportName.BusinessAlertByCityVm, ReportName.BusinessAlertByCityV);
            }
        }

        [Export]
        public ViewModelRoute BusinessAlertByVehicle
        {
            get
            {
                return ViewModelRoute.Create(ReportName.BusinessAlertByVehicleVm, ReportName.BusinessAlertByVehicleV);
            }
        }

        [Export]
        public ViewModelRoute DeviceAlertByYear
        {
            get
            {
                return ViewModelRoute.Create(ReportName.DeviceAlertByYearVm, ReportName.DeviceAlertByYearV);
            }
        }

        [Export]
        public ViewModelRoute DeviceAlertByMonth
        {
            get
            {
                return ViewModelRoute.Create(ReportName.DeviceAlertByMonthVm, ReportName.DeviceAlertByMonthV);
            }
        }

        [Export]
        public ViewModelRoute DeviceAlertByWeek
        {
            get
            {
                return ViewModelRoute.Create(ReportName.DeviceAlertByWeekVm, ReportName.DeviceAlertByWeekV);
            }
        }

        [Export]
        public ViewModelRoute DeviceAlertByDay
        {
            get
            {
                return ViewModelRoute.Create(ReportName.DeviceAlertByDayVm, ReportName.DeviceAlertByDayV);
            }
        }

        [Export]
        public ViewModelRoute DeviceAlertByOrganization
        {
            get
            {
                return ViewModelRoute.Create(ReportName.DeviceAlertByOrganizationVm, ReportName.DeviceAlertByOrganizationV);
            }
        }

        [Export]
        public ViewModelRoute DeviceAlertByProvince
        {
            get
            {
                return ViewModelRoute.Create(ReportName.DeviceAlertByProvinceVm, ReportName.DeviceAlertByProvinceV);
            }
        }

        [Export]
        public ViewModelRoute DeviceAlertByCity
        {
            get
            {
                return ViewModelRoute.Create(ReportName.DeviceAlertByCityVm, ReportName.DeviceAlertByCityV);
            }
        }
        [Export]
        public ViewModelRoute DeviceAlertByVehicle
        {
            get
            {
                return ViewModelRoute.Create(ReportName.DeviceAlertByVehicleVm, ReportName.DeviceAlertByVehicleV);
            }
        }
        [Export]
        public ViewModelRoute VehicleOffline
        {
            get
            {
                return ViewModelRoute.Create(ReportName.VehicleOfflineVm, ReportName.VehicleOfflineV);
            }
        }

        [Export]
        public ViewModelRoute VehicleUserOnline
        {
            get
            {
                return ViewModelRoute.Create(ReportName.UserOnlineVm, ReportName.UserOnlineV);
            }
        }

        [Export]
        public ViewModelRoute VideoFlow
        {
            get
            {
                return ViewModelRoute.Create(ReportName.VideoFlowVm, ReportName.VideoFlowV);
            }
        }

        [Export]
        public ViewModelRoute HistoryTrace
        {
            get
            {
                return ViewModelRoute.Create(ReportName.HistoryTraceVm, ReportName.HistoryTraceV);
            }
        }
    }
}
