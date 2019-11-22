using Gsafety.PTMS.Constants;
using Gsafety.PTMS.Share;
using Jounce.Core.View;
using Jounce.Regions.Core;
/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 01b86628-51c0-47d7-a444-931d23fed427      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-PENGGL
/////                 Author: TEST(penggl)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Maintain.Views.SecuritySuiteView
/////    Project Description:    
/////             Class Name: SuiteRunning
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/11/5 18:44:48
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/11/5 18:44:48
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

namespace Gsafety.PTMS.Maintain.Views.SecuritySuiteView
{
    [ExportAsView(MaintainName.SuiteRunningV, ToolTip = "Click to view some text.", Url = "/SuiteRunning")]
    [ExportViewToRegion(MaintainName.SuiteRunningV, ViewContainer.MaintainContainer)]
    public partial class SuiteRunning : UserControl
    {
        public SuiteRunning()
        {
            InitializeComponent();
            Gsafety.PTMS.Share.ApplicationContext.Instance.StringResourceReader.TranslateDataGrid(SuiteRunningDataGrid);
        }
    }
}
