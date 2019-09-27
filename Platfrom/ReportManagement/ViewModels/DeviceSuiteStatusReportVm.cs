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
namespace Gsafety.PTMS.ReportManager.ViewModels
{
    [ExportAsViewModel(ReportName.DeviceSuiteStatusRptVM)]
    public class DeviceSuiteStatusRptVM : BaseReportViewModel
    {
        public string ReportTitle { get { return ApplicationContext.Instance.StringResourceReader.GetString("Report_SuiteInfo_Title"); } }

        public DeviceSuiteStatusRptVM() :
            base("Gsafety.PTMS.Reports.SuiteInfoRpt, Reports, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")
        {

        }
    }
}
