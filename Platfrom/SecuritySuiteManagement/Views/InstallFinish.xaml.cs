/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 723b00c0-2f62-49e0-993d-6584b795e484      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-DENGZL
/////                 Author: TEST(dengzl)
/////======================================================================
/////           Project Name: Gsafety.PTMS.SecuritySuite.Views
/////    Project Description:    
/////             Class Name: InstallHistory
/////          Class Version: v1.0.0.0
/////            Create Time: 8/7/2013 12:03:05 PM
/////      Class Description:  
/////======================================================================
/////          Modified Time: 8/7/2013 12:03:05 PM
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Jounce.Core;
using Jounce.Core.View;
using Jounce.Regions.Core;
using Gsafety.PTMS.Share;
using Gsafety.PTMS.SecuritySuite;

namespace Gsafety.PTMS.SecuritySuite.Views
{
    [ExportAsView(SecuritySuiteName.InstallFinishV, Category = SecuritySuiteName.CategoryName,
        MenuName = SecuritySuiteName.InstallMenuName, MenuTitle = "SUITE_InstallFinish",
        ToolTip = "Click to view some text.", Url = "/InstallFinish", Order = 1)]
    [ExportViewToRegion(SecuritySuiteName.InstallFinishV, SecuritySuiteName.SuiteContainer)]
    public partial class InstallFinish : UserControl
    {
        public InstallFinish()
        {
            InitializeComponent();
            ApplicationContext.Instance.StringResourceReader.TranslateDataGrid(InstallFinishGrid);
        }
    }
}
