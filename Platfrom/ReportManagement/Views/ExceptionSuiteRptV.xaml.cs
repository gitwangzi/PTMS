/////Copyright (C) Gsafety 2014 .All Rights Reserved.
/////======================================================================
/////                   Guid: e6cab8a1-f231-4e58-9730-f967d0bb0693      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-ZHOUDD
/////                 Author: GJSY(zhoudd)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Manager
/////    Project Description:    
/////             Class Name: AlarmReportMain
/////          Class Version: v1.0.0.0
/////            Create Time: 2014/7/24 11:09:58
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2014/7/24 11:09:58
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System.Windows.Controls;
using Jounce.Core.View;
using Jounce.Regions.Core;

namespace Gsafety.PTMS.ReportManager.Views
{
    //[ExportAsView(ReportName.ExceptionSuiteRptV, Category = ReportName.CategoryName,
    //   MenuName = ReportName.DeviceReportMenu, MenuTitle = "Rpt_ExceptionSuite_Title",
    //   ToolTip = "Click to view some text.", Url = "/ExceptionSuiteRptV", Order = 1)]
    [ExportViewToRegion(ReportName.ExceptionSuiteRptV, ReportName.ReportContainer)]
    public partial class ExceptionSuiteRptV : UserControl
    {
        public ExceptionSuiteRptV()
        {
            InitializeComponent();
        }
    }
}
