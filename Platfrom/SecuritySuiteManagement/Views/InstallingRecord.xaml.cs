/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 56d9b20b-f9b9-4985-ad59-05b06e35e21f      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-DENGZL
/////                 Author: TEST(dengzl)
/////======================================================================
/////           Project Name: Gsafety.PTMS.SecuritySuite.Views
/////    Project Description:    
/////             Class Name: InstallingRecord
/////          Class Version: v1.0.0.0
/////            Create Time: 8/7/2013 12:02:35 PM
/////      Class Description:  
/////======================================================================
/////          Modified Time: 8/7/2013 12:02:35 PM
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
    [ExportAsView(SecuritySuiteName.InstallingRecordV, Category = SecuritySuiteName.CategoryName,
        MenuName = SecuritySuiteName.InstallMenuName, MenuTitle = "SUITE_InstallingRecord",
        ToolTip = "Click to view some text.", Url = "/InstallingRecord", Order=2)]
    [ExportViewToRegion(SecuritySuiteName.InstallingRecordV, SecuritySuiteName.SuiteContainer)]
    public partial class InstallingRecord : UserControl
    {
        public InstallingRecord()
        {
            InitializeComponent();
            ApplicationContext.Instance.StringResourceReader.TranslateDataGrid(InstallingRecordGrid);
            this.MouseRightButtonDown += ChildWindow_MouseRightButtonDown;
        }

        private void ChildWindow_MouseRightButtonDown(object sender,

      System.Windows.Input.MouseButtonEventArgs e)
        {
            e.Handled = true;
        }
    }
}
