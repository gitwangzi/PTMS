﻿/////Copyright (C) Gsafety 2014 .All Rights Reserved.
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
using Jounce.Core.View;
using Jounce.Regions.Core;
using System.Windows.Controls;

namespace Gsafety.PTMS.ReportManager.Views
{
    //[ExportAsView(ReportName.External_AccessRptV, Category = ReportName.CategoryName,
    //   MenuName = ReportName.SystemReportMenu, MenuTitle = "Rpt_External_Access_Title",
    //   ToolTip = "Click to view some text.", Url = "/External_AccessRptV", Order = 1)]
    [ExportViewToRegion(ReportName.External_AccessRptV, ReportName.ReportContainer)]
    public partial class External_AccessRptV : UserControl
    {
        public External_AccessRptV()
        {
            InitializeComponent();
        }
    }
}
