/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: d1154a4e-3723-4e43-bf96-48a41e761e3b      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: GAOZITIAN-PC
/////                 Author: TEST(gaozt)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Installation.Views
/////    Project Description:    
/////             Class Name: InstallverifyView
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/8/24 10:30:49
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/8/24 10:30:49
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
using Gsafety.PTMS.Constants;

namespace Gsafety.PTMS.Installation.Views
{
    [ExportAsView(InstallationName.InstallverifyV, Category = InstallationName.CategoryName,
       ToolTip = "Click to view some text.", Url = "/InstallverifyView")]
    [ExportViewToRegion(InstallationName.InstallverifyV, ViewContainer.InstallContainer)]
    public partial class InstallverifyView : UserControl
    {
        public InstallverifyView()
        {
            InitializeComponent();
            Gsafety.PTMS.Share.ApplicationContext.Instance.StringResourceReader.TranslateDataGrid(SuiteDataGrid);
        }
    }
}
