/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 4f8ba034-6349-40e2-ba50-6d447bb3dd47      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-DENGZL
/////                 Author: TEST(dengzl)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Installation.Views
/////    Project Description:    
/////             Class Name: InstalledRecord
/////          Class Version: v1.0.0.0
/////            Create Time: 8/14/2013 2:51:18 PM
/////      Class Description:  
/////======================================================================
/////          Modified Time: 8/14/2013 2:51:18 PM
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
using Jounce.Core.View;
using Jounce.Regions.Core;
using Gsafety.PTMS.Share;
using Jounce.Core;
using Gsafety.PTMS.Constants;
using Gsafety.Common.Controls;

namespace Gsafety.PTMS.Installation.Views
{
    [ExportAsView(InstallationName.InstalledGPSRecordV, Category = InstallationName.CategoryName,
       ToolTip = "Click to view some text.", Url = "/InstalledGPSRecordV")]
    [ExportViewToRegion(InstallationName.InstalledGPSRecordV, ViewContainer.InstallContainer)]
    public partial class InstalledGPSRecord : UserControl
    {
        public InstalledGPSRecord()
        {
            InitializeComponent();
            Gsafety.PTMS.Share.ApplicationContext.Instance.StringResourceReader.TranslateDataGrid(SuiteDataGrid);
            this.MouseRightButtonDown += ChildWindow_MouseRightButtonDown;
            comboStatus.DropDownOpened += PopupHandler.OnDropDown;
            comboStatus.DropDownClosed += PopupHandler.OnDropDown;
        }

        private void ChildWindow_MouseRightButtonDown(object sender,

System.Windows.Input.MouseButtonEventArgs e)
        {
            e.Handled = true;
        }

        private void LayoutRoot_MouseRightButtonUp_1(object sender,

System.Windows.Input.MouseButtonEventArgs e)
        {
            e.Handled = true;
        }


    }
}
