/////Copyright (C) Gsafety 2014 .All Rights Reserved.
/////======================================================================
/////                   Guid: 2fd2dd35-59a8-4468-aade-d959f7f3b4b4      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-ZHOUDD
/////                 Author: GJSY(zhoudd)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Manager
/////    Project Description:    
/////             Class Name: ReportsMenu
/////          Class Version: v1.0.0.0
/////            Create Time: 2014/7/24 11:16:37
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2014/7/24 11:16:37
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System.Collections.Generic;
using System.Windows.Controls;
using Gsafety.Common.Utilities;
using Gsafety.PTMS.ReportManager.Views;
using Jounce.Core.View;
using Jounce.Regions.Core;

namespace Gsafety.PTMS.ReportManager
{
    public partial class ReportsMenu : UserControl
    {
        List<HyperlinkButton> _NavigationButton;

        public List<HyperlinkButton> NavigationButtons
        {
            get
            {
                if (_NavigationButton == null || _NavigationButton.Count == 0)
                {
                    VisualTreeExtedHelper vtHelper = new VisualTreeExtedHelper();
                    _NavigationButton = vtHelper.GetChildObjects<HyperlinkButton>(this.LayoutRoot, "");
                }
                return _NavigationButton;
            }
        }
        public ReportsMenu()
        {
            InitializeComponent();
        }

        AlarmView alarmbyyear = new AlarmView();
        [ExportAsView(ReportName.AlarmByYearV, Category = ReportName.CategoryName,
      MenuName = ReportName.AlarmMenu, MenuTitle = "按年统计", Url = "/AlarmByYearView", Order = 1)]
        [ExportViewToRegion(ReportName.AlarmByYearV, ReportName.ReportContainer)]
        public AlarmView AlarmByYear
        {
            get { return alarmbyyear; }
            set { alarmbyyear = value; }
        }

        AlarmView alarmbymonth = new AlarmView();
        [ExportAsView(ReportName.AlarmByMonthV, Category = ReportName.CategoryName,
      MenuName = ReportName.AlarmMenu, MenuTitle = "按月统计", Url = "/AlarmByMonthView", Order = 2)]
        [ExportViewToRegion(ReportName.AlarmByMonthV, ReportName.ReportContainer)]
        public AlarmView AlarmByMonth
        {
            get { return alarmbymonth; }
            set { alarmbymonth = value; }
        }


        AlarmView alarmbyweek = new AlarmView();
        [ExportAsView(ReportName.AlarmByWeekV, Category = ReportName.CategoryName,
      MenuName = ReportName.AlarmMenu, MenuTitle = "按星期统计", Url = "/AlarmByWeekView", Order = 3)]
        [ExportViewToRegion(ReportName.AlarmByWeekV, ReportName.ReportContainer)]
        public AlarmView AlarmByWeek
        {
            get { return alarmbyweek; }
            set { alarmbyweek = value; }
        }

        AlarmView alarmbyday = new AlarmView();
        [ExportAsView(ReportName.AlarmByDayV, Category = ReportName.CategoryName,
      MenuName = ReportName.AlarmMenu, MenuTitle = "按日统计", Url = "/AlarmByDayView", Order = 4)]
        [ExportViewToRegion(ReportName.AlarmByDayV, ReportName.ReportContainer)]
        public AlarmView AlarmByDay
        {
            get { return alarmbyday; }
            set { alarmbyday = value; }
        }

        AlarmView alarmbyorganization = new AlarmView();
        [ExportAsView(ReportName.AlarmByOrganizationV, Category = ReportName.CategoryName,
      MenuName = ReportName.AlarmMenu, MenuTitle = "按机构统计", Url = "/AlarmByOrganizationView", Order = 5)]
        [ExportViewToRegion(ReportName.AlarmByOrganizationV, ReportName.ReportContainer)]
        public AlarmView AlarmByOrganization
        {
            get { return alarmbyorganization; }
            set { alarmbyorganization = value; }
        }

        AlarmView alarmbyprovince = new AlarmView();
        [ExportAsView(ReportName.AlarmByProvinceV, Category = ReportName.CategoryName,
      MenuName = ReportName.AlarmMenu, MenuTitle = "按省统计", Url = "/AlarmByProvinceView", Order = 6)]
        [ExportViewToRegion(ReportName.AlarmByProvinceV, ReportName.ReportContainer)]
        public AlarmView AlarmByProvince
        {
            get { return alarmbyprovince; }
            set { alarmbyprovince = value; }
        }

        AlarmView alarmbycity = new AlarmView();
        [ExportAsView(ReportName.AlarmByCityV, Category = ReportName.CategoryName,
      MenuName = ReportName.AlarmMenu, MenuTitle = "按市统计", Url = "/AlarmByCityView", Order = 7)]
        [ExportViewToRegion(ReportName.AlarmByCityV, ReportName.ReportContainer)]
        public AlarmView AlarmByCity
        {
            get { return alarmbycity; }
            set { alarmbycity = value; }
        }

        AlarmView alarmbyvehicle = new AlarmView();
        [ExportAsView(ReportName.AlarmByVehicleV, Category = ReportName.CategoryName,
      MenuName = ReportName.AlarmMenu, MenuTitle = "按车辆统计", Url = "/AlarmByVehicleView", Order = 8)]
        [ExportViewToRegion(ReportName.AlarmByVehicleV, ReportName.ReportContainer)]
        public AlarmView AlarmByVehicle
        {
            get { return alarmbyvehicle; }
            set { alarmbyvehicle = value; }
        }

        BusinessAlertView businessalertbyyear = new BusinessAlertView();
        [ExportAsView(ReportName.BusinessAlertByYearV, Category = ReportName.CategoryName,
      MenuName = ReportName.BusinessAlertMenu, MenuTitle = "按年统计", Url = "/BusinessAlertByYearView", Order = 1)]
        [ExportViewToRegion(ReportName.BusinessAlertByYearV, ReportName.ReportContainer)]
        public BusinessAlertView BusinessAlertByYear
        {
            get { return businessalertbyyear; }
            set { businessalertbyyear = value; }
        }

        BusinessAlertView businessalertbymonth = new BusinessAlertView();
        [ExportAsView(ReportName.BusinessAlertByMonthV, Category = ReportName.CategoryName,
      MenuName = ReportName.BusinessAlertMenu, MenuTitle = "按月统计", Url = "/BusinessAlertByMonthView", Order = 2)]
        [ExportViewToRegion(ReportName.BusinessAlertByMonthV, ReportName.ReportContainer)]
        public BusinessAlertView BusinessAlertByMonth
        {
            get { return businessalertbymonth; }
            set { businessalertbymonth = value; }
        }


        BusinessAlertView businessalertbyweek = new BusinessAlertView();
        [ExportAsView(ReportName.BusinessAlertByWeekV, Category = ReportName.CategoryName,
      MenuName = ReportName.BusinessAlertMenu, MenuTitle = "按星期统计", Url = "/BusinessAlertByWeekView", Order = 3)]
        [ExportViewToRegion(ReportName.BusinessAlertByWeekV, ReportName.ReportContainer)]
        public BusinessAlertView BusinessAlertByWeek
        {
            get { return businessalertbyweek; }
            set { businessalertbyweek = value; }
        }

        BusinessAlertView businessalertbyday = new BusinessAlertView();
        [ExportAsView(ReportName.BusinessAlertByDayV, Category = ReportName.CategoryName,
      MenuName = ReportName.BusinessAlertMenu, MenuTitle = "按日统计", Url = "/BusinessAlertByDayView", Order = 4)]
        [ExportViewToRegion(ReportName.BusinessAlertByDayV, ReportName.ReportContainer)]
        public BusinessAlertView BusinessAlertByDay
        {
            get { return businessalertbyday; }
            set { businessalertbyday = value; }
        }

        BusinessAlertView businessalertbyorganization = new BusinessAlertView();
        [ExportAsView(ReportName.BusinessAlertByOrganizationV, Category = ReportName.CategoryName,
      MenuName = ReportName.BusinessAlertMenu, MenuTitle = "按机构统计", Url = "/BusinessAlertByOrganizationView", Order = 5)]
        [ExportViewToRegion(ReportName.BusinessAlertByOrganizationV, ReportName.ReportContainer)]
        public BusinessAlertView BusinessAlertByOrganization
        {
            get { return businessalertbyorganization; }
            set { businessalertbyorganization = value; }
        }

        BusinessAlertView businessalertbyprovince = new BusinessAlertView();
        [ExportAsView(ReportName.BusinessAlertByProvinceV, Category = ReportName.CategoryName,
      MenuName = ReportName.BusinessAlertMenu, MenuTitle = "按省统计", Url = "/BusinessAlertByProvinceView", Order = 6)]
        [ExportViewToRegion(ReportName.BusinessAlertByProvinceV, ReportName.ReportContainer)]
        public BusinessAlertView BusinessAlertByProvince
        {
            get { return businessalertbyprovince; }
            set { businessalertbyprovince = value; }
        }

        BusinessAlertView businessalertbycity = new BusinessAlertView();
        [ExportAsView(ReportName.BusinessAlertByCityV, Category = ReportName.CategoryName,
      MenuName = ReportName.BusinessAlertMenu, MenuTitle = "按市统计", Url = "/BusinessAlertByCityView", Order = 7)]
        [ExportViewToRegion(ReportName.BusinessAlertByCityV, ReportName.ReportContainer)]
        public BusinessAlertView BusinessAlertByCity
        {
            get { return businessalertbycity; }
            set { businessalertbycity = value; }
        }
        BusinessAlertView businessalertbyvehicle = new BusinessAlertView();
        [ExportAsView(ReportName.BusinessAlertByVehicleV, Category = ReportName.CategoryName,
      MenuName = ReportName.BusinessAlertMenu, MenuTitle = "按车辆统计", Url = "/BusinessAlertByVehicleView", Order = 8)]
        [ExportViewToRegion(ReportName.BusinessAlertByVehicleV, ReportName.ReportContainer)]
        public BusinessAlertView BusinessAlertByVehicle
        {
            get { return businessalertbyvehicle; }
            set { businessalertbyvehicle = value; }
        }

        DeviceAlertView devicealertbyyear = new DeviceAlertView();
        [ExportAsView(ReportName.DeviceAlertByYearV, Category = ReportName.CategoryName,
      MenuName = ReportName.DeviceAlertMenu, MenuTitle = "按年统计", Url = "/DeviceAlertByYearView", Order = 1)]
        [ExportViewToRegion(ReportName.DeviceAlertByYearV, ReportName.ReportContainer)]
        public DeviceAlertView DeviceAlertByYear
        {
            get { return devicealertbyyear; }
            set { devicealertbyyear = value; }
        }

        DeviceAlertView devicealertbymonth = new DeviceAlertView();
        [ExportAsView(ReportName.DeviceAlertByMonthV, Category = ReportName.CategoryName,
      MenuName = ReportName.DeviceAlertMenu, MenuTitle = "按月统计", Url = "/DeviceAlertByMonthView", Order = 2)]
        [ExportViewToRegion(ReportName.DeviceAlertByMonthV, ReportName.ReportContainer)]
        public DeviceAlertView DeviceAlertByMonth
        {
            get { return devicealertbymonth; }
            set { devicealertbymonth = value; }
        }


        DeviceAlertView devicealertbyweek = new DeviceAlertView();
        [ExportAsView(ReportName.DeviceAlertByWeekV, Category = ReportName.CategoryName,
      MenuName = ReportName.DeviceAlertMenu, MenuTitle = "按星期统计", Url = "/DeviceAlertByWeekView", Order = 3)]
        [ExportViewToRegion(ReportName.DeviceAlertByWeekV, ReportName.ReportContainer)]
        public DeviceAlertView DeviceAlertByWeek
        {
            get { return devicealertbyweek; }
            set { devicealertbyweek = value; }
        }

        DeviceAlertView devicealertbyday = new DeviceAlertView();
        [ExportAsView(ReportName.DeviceAlertByDayV, Category = ReportName.CategoryName,
      MenuName = ReportName.DeviceAlertMenu, MenuTitle = "按日统计", Url = "/DeviceAlertByDayView", Order = 4)]
        [ExportViewToRegion(ReportName.DeviceAlertByDayV, ReportName.ReportContainer)]
        public DeviceAlertView DeviceAlertByDay
        {
            get { return devicealertbyday; }
            set { devicealertbyday = value; }
        }

        DeviceAlertView devicealertbyorganization = new DeviceAlertView();
        [ExportAsView(ReportName.DeviceAlertByOrganizationV, Category = ReportName.CategoryName,
      MenuName = ReportName.DeviceAlertMenu, MenuTitle = "按机构统计", Url = "/DeviceAlertByOrganizationView", Order = 5)]
        [ExportViewToRegion(ReportName.DeviceAlertByOrganizationV, ReportName.ReportContainer)]
        public DeviceAlertView DeviceAlertByOrganization
        {
            get { return devicealertbyorganization; }
            set { devicealertbyorganization = value; }
        }

        DeviceAlertView devicealertbyprovince = new DeviceAlertView();
        [ExportAsView(ReportName.DeviceAlertByProvinceV, Category = ReportName.CategoryName,
      MenuName = ReportName.DeviceAlertMenu, MenuTitle = "按省统计", Url = "/DeviceAlertByProvinceView", Order = 6)]
        [ExportViewToRegion(ReportName.DeviceAlertByProvinceV, ReportName.ReportContainer)]
        public DeviceAlertView DeviceAlertByProvince
        {
            get { return devicealertbyprovince; }
            set { devicealertbyprovince = value; }
        }

        DeviceAlertView devicealertbycity = new DeviceAlertView();
        [ExportAsView(ReportName.DeviceAlertByCityV, Category = ReportName.CategoryName,
      MenuName = ReportName.DeviceAlertMenu, MenuTitle = "按市统计", Url = "/DeviceAlertByCityView", Order = 7)]
        [ExportViewToRegion(ReportName.DeviceAlertByCityV, ReportName.ReportContainer)]
        public DeviceAlertView DeviceAlertByCity
        {
            get { return devicealertbycity; }
            set { devicealertbycity = value; }
        }

        DeviceAlertView devicealertbyvehicle = new DeviceAlertView();
        [ExportAsView(ReportName.DeviceAlertByVehicleV, Category = ReportName.CategoryName,
      MenuName = ReportName.DeviceAlertMenu, MenuTitle = "按车辆统计", Url = "/DeviceAlertByVehicleView", Order = 8)]
        [ExportViewToRegion(ReportName.DeviceAlertByVehicleV, ReportName.ReportContainer)]
        public DeviceAlertView DevicesAlertByVehicle
        {
            get { return devicealertbyvehicle; }
            set { devicealertbyvehicle = value; }
        }
    }
}
