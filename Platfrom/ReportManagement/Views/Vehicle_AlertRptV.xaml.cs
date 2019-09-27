﻿using Gsafety.PTMS.ReportManager;
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
using Jounce.Core.View;
using Jounce.Regions.Core;
using System.Windows.Controls;
using System;
using Gsafety.PTMS.Bases.Models;
using System.Collections.Generic;
using Gsafety.PTMS.Share;
namespace Gsafety.PTMS.ReportManager.Views
{
    [ExportAsView(ReportName.Vehicle_AlertRptV, Category = ReportName.CategoryName,
       MenuName = ReportName.VehicleReportMenu, MenuTitle = "Rpt_Vehicle_Alert_Title",
       ToolTip = "Click to view some text.", Url = "/Vehicle_AlertRptV", Order = 1)]
    [ExportViewToRegion(ReportName.Vehicle_AlertRptV, ReportName.ReportContainer)]
    public partial class Vehicle_AlertRptV : UserControl
    {
        public Vehicle_AlertRptV()
        {
            InitializeComponent();          
        }      
    }
}
