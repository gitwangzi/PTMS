/////Copyright (C) Gsafety 2014 .All Rights Reserved.
/////======================================================================
/////                   Guid: c27f679e-bbbf-42ce-8723-d510b7805b6d      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-ZHOUDD
/////                 Author: GJSY(zhoudd)
/////======================================================================
/////           Project Name: Gsafety.PTMS.ReportManager.ViewModels
/////    Project Description:    
/////             Class Name: AlarmReportPageViewModel
/////          Class Version: v1.0.0.0
/////            Create Time: 2014/7/24 15:44:05
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2014/7/24 15:44:05
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using DevExpress.Xpf.Printing;
using Gsafety.Common.Utilities;
using Gsafety.PTMS.Report.Repository;
using Gsafety.PTMS.ReportManager.Base;
using Gsafety.PTMS.ServiceReference.DistrictService;
using Gsafety.PTMS.Share;
using Jounce.Core.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using System.Threading;
using Gsafety.PTMS.Bases.Models;
namespace Gsafety.PTMS.ReportManager.ViewModels
{
    [ExportAsViewModel(ReportName.Vehicle_AlarmRptV)]
    public class Vehicle_AlarmRptVM : BaseReportViewModel
    {
        #region Property
        public List<EnumModel> TimeTypeList { get; set; }
        public bool typeEnabled { get; set; }
        private EnumModel _TimeType;
        public EnumModel TimeType
        {
            get { return _TimeType; }
            set
            {
                _TimeType = value;
                if (value.EnumName == "Rpt_CustomPeriodTime")
                {
                    typeEnabled = true;
                    Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => typeEnabled));
                }
                else
                {
                    typeEnabled = false;
                    Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => typeEnabled));
                    ConfirmedTime(value.EnumName);
                }

            }
        }
        #endregion

        public string ReportTitle { get { return ApplicationContext.Instance.StringResourceReader.GetString("Rpt_Alarm_Title"); } }

        public Vehicle_AlarmRptVM() :
            base("Gsafety.PTMS.Reports.AlarmReport, Reports, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")
        {
            TimeTypeList = new List<EnumModel>();
            Enum.GetNames(typeof(timeType)).ToList().ForEach(x =>
            {
                EnumModel item = new EnumModel { EnumName = x, ShowName = ApplicationContext.Instance.StringResourceReader.GetString(x) };
                TimeTypeList.Add(item);
            });
            TimeType = TimeTypeList[0];
        }
        public enum timeType
        {
            Rpt_CustomPeriodTime = 0,
            Rpt_TodayTime = 1,
            Rpt_WeekTime = 2,
            Rpt_MonthTime = 3,
        }
        public void ConfirmedTime(string timeType)
        {
            switch (timeType)
            {
                case "Rpt_TodayTime":
                    BeginTime = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd") + " 00:00:00");
                    EndTime = DateTime.Now;
                    Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => BeginTime));
                    Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => EndTime));
                    break;
                case "Rpt_WeekTime":
                    int iDay = (int)DateTime.Now.DayOfWeek;
                    if (iDay == 0) iDay = -6;
                    else iDay = -(iDay - 1);
                    BeginTime = DateTime.Parse(DateTime.Now.AddDays(iDay).ToString("yyyy-MM-dd") + " 00:00:00");
                    EndTime = DateTime.Now;
                    Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => BeginTime));
                    Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => EndTime));
                    break;
                case "Rpt_MonthTime":
                    BeginTime = DateTime.Parse(DateTime.Now.ToString("yyyy-MM") + "-01 00:00:00");
                    EndTime = DateTime.Now;
                    Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => BeginTime));
                    Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => EndTime));
                    break;
            }
        }
    }
}