using BaseLib.ViewModels;
using Gsafety.PTMS.Share;
/////Copyright (C) Gsafety 2014 .All Rights Reserved.
/////======================================================================
/////                   Guid: d01e6956-64c5-4199-ac34-7225af7d0bc9      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-ZHOUDD
/////                 Author: GJSY(zhoudd)
/////======================================================================
/////           Project Name: Gsafety.PTMS.ReportManager.ViewModels
/////    Project Description:    
/////             Class Name: ReportsMenuViewModel
/////          Class Version: v1.0.0.0
/////            Create Time: 2014/7/24 14:30:43
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2014/7/24 14:30:43
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using Jounce.Core.ViewModel;
using System.Collections.ObjectModel;
using System.Reflection;
namespace Gsafety.PTMS.ReportManager.ViewModels
{
    [ExportAsViewModel(ReportName.ReportMenuVm)]
    public class ReportsMenuViewModel : PTMSBaseViewModel
    {
        ObservableCollection<MenuInfo> _BaseInfoMenuItems = null;

        ObservableCollection<MenuInfo> _VehicleReportItems = new ObservableCollection<MenuInfo>();
        ObservableCollection<MenuInfo> _AlarmReportItems = new ObservableCollection<MenuInfo>();
        ObservableCollection<MenuInfo> _BusinessMenuItems = new ObservableCollection<MenuInfo>();
        ObservableCollection<MenuInfo> _DeviceMenuItems = new ObservableCollection<MenuInfo>();

        public ObservableCollection<MenuInfo> BaseInfoMenuItems
        {
            get
            {
                return _BaseInfoMenuItems;
            }
        }
        //#region Fields

        //ObservableCollection<MenuInfo> _ManagerMenuItems = null;
        //#endregion

        //#region Attributes

        public ObservableCollection<MenuInfo> VehicleReportItems
        {
            get { return _VehicleReportItems; }
        }

        public ObservableCollection<MenuInfo> AlarmReportItems
        {
            get { return _AlarmReportItems; }
        }


        public ObservableCollection<MenuInfo> BusinessMenuItems
        {
            get { return _BusinessMenuItems; }
        }

        public ObservableCollection<MenuInfo> DeviceMenuItems
        {
            get { return _DeviceMenuItems; }
        }
        //#endregion

        //protected override void InitializeVm()
        //{
        //    base.InitializeVm();
        //    if (_ManagerMenuItems == null || _ManagerMenuItems.Count == 0)
        //    {
        //        _ManagerMenuItems = ApplicationContext.Instance.MenuManager.GetNavigateInfos(Router, ReportName.CategoryName);
        //        Jounce.Framework.JounceHelper.ExecuteOnUI(() =>
        //        {
        //            RaisePropertyChanged(() => VehicleReportItems);
        //            RaisePropertyChanged(() => AlarmReportItems);
        //            RaisePropertyChanged(() => DeviceSuiteStatusItems);
        //            RaisePropertyChanged(() => VehicleOnTimeRptItems);
        //            RaisePropertyChanged(() => BusinessMenuItems);
        //            RaisePropertyChanged(() => DeviceMenuItems);
        //        });
        //    }
        //}

        protected override void OnInitialUI(ObservableCollection<string> FuncItems)
        {
            try
            {
                var videoFlowRpt = new MenuInfo("videoFlowRpt", ApplicationContext.Instance.StringResourceReader.GetString("VedioFlow"), "/" + ReportName.VideoFlowV);
                var offlineVehRpt = new MenuInfo("offlineVehRpt", ApplicationContext.Instance.StringResourceReader.GetString("VehicleOffLine"), "/" + ReportName.VehicleOfflineV);
                var onlineDetailRpt = new MenuInfo("onlineDetailRpt", ApplicationContext.Instance.StringResourceReader.GetString("UserOnlineDetail"), "/" + ReportName.UserOnlineV);
                var historyTraceRpt = new MenuInfo("historyTraceRpt", ApplicationContext.Instance.StringResourceReader.GetString("HistoryRote"), "/" + ReportName.HistoryTraceV);

                if (ApplicationContext.Instance.AuthenticationInfo.Role.FuncItems.Contains("02-05-01-01"))
                {
                    this._VehicleReportItems.Add(videoFlowRpt);
                }
                if (ApplicationContext.Instance.AuthenticationInfo.Role.FuncItems.Contains("02-05-01-02"))
                {
                    this._VehicleReportItems.Add(offlineVehRpt);
                }
                if (ApplicationContext.Instance.AuthenticationInfo.Role.FuncItems.Contains("02-05-01-03"))
                {
                    this._VehicleReportItems.Add(onlineDetailRpt);
                }
                this._VehicleReportItems.Add(historyTraceRpt);

                var alarmByYearRpt = new MenuInfo("alarmByYearRpt", ApplicationContext.Instance.StringResourceReader.GetString("StatisticsByYear"), "/" + ReportName.AlarmByYearV);
                var alarmByMonthRpt = new MenuInfo("alarmByMonthRpt", ApplicationContext.Instance.StringResourceReader.GetString("StatisticsByMonth"), "/" + ReportName.AlarmByMonthV);
                var alarmByWeekRpt = new MenuInfo("alarmByWeekRpt", ApplicationContext.Instance.StringResourceReader.GetString("StatisticsByWeek"), "/" + ReportName.AlarmByWeekV);
                var alarmByDayRpt = new MenuInfo("alarmByDayRpt", ApplicationContext.Instance.StringResourceReader.GetString("StatisticsByDay"), "/" + ReportName.AlarmByDayV);
                var alarmByRegRpt = new MenuInfo("alarmByRegRpt", ApplicationContext.Instance.StringResourceReader.GetString("StatisticsByOrganzation"), "/" + ReportName.AlarmByOrganizationV);
                var alarmByProRpt = new MenuInfo("alarmByProRpt", ApplicationContext.Instance.StringResourceReader.GetString("StatisticsByProvince"), "/" + ReportName.AlarmByProvinceV);
                var alarmByCityRpt = new MenuInfo("alarmByCityRpt", ApplicationContext.Instance.StringResourceReader.GetString("StatisticsByCity"), "/" + ReportName.AlarmByCityV);
                var alarmByVehicleRpt = new MenuInfo("alarmByVehicleRpt", ApplicationContext.Instance.StringResourceReader.GetString("StatisticsByVehicle"), "/" + ReportName.AlarmByVehicleV);

                if (ApplicationContext.Instance.AuthenticationInfo.Role.FuncItems.Contains("02-05-02-01"))
                {
                    this._AlarmReportItems.Add(alarmByYearRpt);
                }
                if (ApplicationContext.Instance.AuthenticationInfo.Role.FuncItems.Contains("02-05-02-02"))
                {
                    this._AlarmReportItems.Add(alarmByMonthRpt);
                }
                if (ApplicationContext.Instance.AuthenticationInfo.Role.FuncItems.Contains("02-05-02-03"))
                {
                    this._AlarmReportItems.Add(alarmByWeekRpt);
                }
                if (ApplicationContext.Instance.AuthenticationInfo.Role.FuncItems.Contains("02-05-02-04"))
                {
                    this._AlarmReportItems.Add(alarmByDayRpt);
                }
                if (ApplicationContext.Instance.AuthenticationInfo.Role.FuncItems.Contains("02-05-02-05"))
                {
                    this._AlarmReportItems.Add(alarmByRegRpt);
                }
                if (ApplicationContext.Instance.AuthenticationInfo.Role.FuncItems.Contains("02-05-02-06"))
                {
                    this._AlarmReportItems.Add(alarmByProRpt);
                }
                if (ApplicationContext.Instance.AuthenticationInfo.Role.FuncItems.Contains("02-05-02-07"))
                {
                    this._AlarmReportItems.Add(alarmByCityRpt);
                }

                this._AlarmReportItems.Add(alarmByVehicleRpt);

                var buAlertByYearRpt = new MenuInfo("buAlertByYearRpt", ApplicationContext.Instance.StringResourceReader.GetString("StatisticsByYear"), "/" + ReportName.BusinessAlertByYearV);
                var buAlertByMonthRpt = new MenuInfo("buAlertByMonthRpt", ApplicationContext.Instance.StringResourceReader.GetString("StatisticsByMonth"), "/" + ReportName.BusinessAlertByMonthV);
                var buAlertByWeekRpt = new MenuInfo("buAlertByWeekRpt", ApplicationContext.Instance.StringResourceReader.GetString("StatisticsByWeek"), "/" + ReportName.BusinessAlertByWeekV);
                var buAlertByDayRpt = new MenuInfo("buAlertByDayRpt", ApplicationContext.Instance.StringResourceReader.GetString("StatisticsByDay"), "/" + ReportName.BusinessAlertByDayV);
                var buAlertByRegRpt = new MenuInfo("buAlertByRegRpt", ApplicationContext.Instance.StringResourceReader.GetString("StatisticsByOrganzation"), "/" + ReportName.BusinessAlertByOrganizationV);
                var buAlertByProRpt = new MenuInfo("buAlertByProRpt", ApplicationContext.Instance.StringResourceReader.GetString("StatisticsByProvince"), "/" + ReportName.BusinessAlertByProvinceV);
                var buAlertByCityRpt = new MenuInfo("buAlertByCityRpt", ApplicationContext.Instance.StringResourceReader.GetString("StatisticsByCity"), "/" + ReportName.BusinessAlertByCityV);
                var buAlertByVehicleRpt = new MenuInfo("buAlertByVehicleRpt", ApplicationContext.Instance.StringResourceReader.GetString("StatisticsByVehicle"), "/" + ReportName.BusinessAlertByVehicleV);


                if (ApplicationContext.Instance.AuthenticationInfo.Role.FuncItems.Contains("02-05-03-01"))
                {
                    this._BusinessMenuItems.Add(buAlertByYearRpt);
                }
                if (ApplicationContext.Instance.AuthenticationInfo.Role.FuncItems.Contains("02-05-03-02"))
                {
                    this._BusinessMenuItems.Add(buAlertByMonthRpt);
                }
                if (ApplicationContext.Instance.AuthenticationInfo.Role.FuncItems.Contains("02-05-03-03"))
                {
                    this._BusinessMenuItems.Add(buAlertByWeekRpt);
                }
                if (ApplicationContext.Instance.AuthenticationInfo.Role.FuncItems.Contains("02-05-03-04"))
                {
                    this._BusinessMenuItems.Add(buAlertByDayRpt);
                }
                if (ApplicationContext.Instance.AuthenticationInfo.Role.FuncItems.Contains("02-05-03-05"))
                {
                    this._BusinessMenuItems.Add(buAlertByRegRpt);
                }
                if (ApplicationContext.Instance.AuthenticationInfo.Role.FuncItems.Contains("02-05-03-06"))
                {
                    this._BusinessMenuItems.Add(buAlertByProRpt);
                }
                if (ApplicationContext.Instance.AuthenticationInfo.Role.FuncItems.Contains("02-05-03-07"))
                {
                    this._BusinessMenuItems.Add(buAlertByCityRpt);
                }

                this._BusinessMenuItems.Add(buAlertByVehicleRpt);

                var deAlertByYearRpt = new MenuInfo("deAlertByYearRpt", ApplicationContext.Instance.StringResourceReader.GetString("StatisticsByYear"), "/" + ReportName.DeviceAlertByYearV);
                var deAlertByMonthRpt = new MenuInfo("deAlertByMonthRpt", ApplicationContext.Instance.StringResourceReader.GetString("StatisticsByMonth"), "/" + ReportName.DeviceAlertByMonthV);
                var deAlertByWeekRpt = new MenuInfo("deAlertByWeekRpt", ApplicationContext.Instance.StringResourceReader.GetString("StatisticsByWeek"), "/" + ReportName.DeviceAlertByWeekV);
                var deAlertByDayRpt = new MenuInfo("deAlertByDayRpt", ApplicationContext.Instance.StringResourceReader.GetString("StatisticsByDay"), "/" + ReportName.DeviceAlertByDayV);
                var deAlertByRegRpt = new MenuInfo("deAlertByRegRpt", ApplicationContext.Instance.StringResourceReader.GetString("StatisticsByOrganzation"), "/" + ReportName.DeviceAlertByOrganizationV);
                var deAlertByProRpt = new MenuInfo("deAlertByProRpt", ApplicationContext.Instance.StringResourceReader.GetString("StatisticsByProvince"), "/" + ReportName.DeviceAlertByProvinceV);
                var deAlertByCityRpt = new MenuInfo("deAlertByCityRpt", ApplicationContext.Instance.StringResourceReader.GetString("StatisticsByCity"), "/" + ReportName.DeviceAlertByCityV);
                var deAlertByVehicleRpt = new MenuInfo("deAlertByVehicleRpt", ApplicationContext.Instance.StringResourceReader.GetString("StatisticsByVehicle"), "/" + ReportName.DeviceAlertByVehicleV);


                if (ApplicationContext.Instance.AuthenticationInfo.Role.FuncItems.Contains("02-05-04-01"))
                {
                    this._DeviceMenuItems.Add(deAlertByYearRpt);
                }
                if (ApplicationContext.Instance.AuthenticationInfo.Role.FuncItems.Contains("02-05-04-02"))
                {
                    this._DeviceMenuItems.Add(deAlertByMonthRpt);
                }
                if (ApplicationContext.Instance.AuthenticationInfo.Role.FuncItems.Contains("02-05-04-03"))
                {
                    this._DeviceMenuItems.Add(deAlertByWeekRpt);
                }
                if (ApplicationContext.Instance.AuthenticationInfo.Role.FuncItems.Contains("02-05-04-04"))
                {
                    this._DeviceMenuItems.Add(deAlertByDayRpt);
                }
                if (ApplicationContext.Instance.AuthenticationInfo.Role.FuncItems.Contains("02-05-04-05"))
                {
                    this._DeviceMenuItems.Add(deAlertByRegRpt);
                }
                if (ApplicationContext.Instance.AuthenticationInfo.Role.FuncItems.Contains("02-05-04-06"))
                {
                    this._DeviceMenuItems.Add(deAlertByProRpt);
                }
                if (ApplicationContext.Instance.AuthenticationInfo.Role.FuncItems.Contains("02-05-04-07"))
                {
                    this._DeviceMenuItems.Add(deAlertByCityRpt);
                }
                this._DeviceMenuItems.Add(deAlertByVehicleRpt);


            }
            catch (System.Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
            //  Jounce.Framework.JounceHelper.ExecuteOnUI(() =>
            //  {
            //      RaisePropertyChanged(() => VehicleReportItems);
            //  });
            //  Jounce.Framework.JounceHelper.ExecuteOnUI(() =>
            //  {
            //      RaisePropertyChanged(() => this.AlarmReportItems);
            //  });
            //  Jounce.Framework.JounceHelper.ExecuteOnUI(() =>
            //  {
            //      RaisePropertyChanged(() => this.BusinessMenuItems);
            //  });
            //  Jounce.Framework.JounceHelper.ExecuteOnUI(() =>
            //{
            //    RaisePropertyChanged(() => this.DeviceMenuItems);
            //});
        }
    }
}

//        private ObservableCollection<MenuInfo> GetMenuInfo(string SubMenuName)
//        {
//            if (_ManagerMenuItems == null || _ManagerMenuItems.Count == 0)
//                return null;
//            var result = _ManagerMenuItems.Where(item => item.SubMenuType.Equals(SubMenuName)).OrderBy(item => item.Order);
//            if (result == null || result.Count() == 0)
//                return null;
//            ObservableCollection<MenuInfo> menuItems = new ObservableCollection<MenuInfo>();
//            foreach (var item in result)
//            {
//                menuItems.Add(item);
//            }
//            return menuItems;
//        }
//    }
//}
