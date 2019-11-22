using Gsafety.PTMS.Constants;
using Gsafety.PTMS.Share;
using Jounce.Core.View;
using Jounce.Regions.Core;
/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 5d293ad3-ec36-4c98-951f-50aad13c1e72      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-PENGGL
/////                 Author: TEST(penggl)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Maintain.Views
/////    Project Description:    
/////             Class Name: SuiteQuery
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/9/11 15:33:18
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/9/11 15:33:18
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

namespace Gsafety.PTMS.Maintain.Views
{
    [ExportAsView(MaintainName.SecuritySuiteV, ToolTip = "Click to view some text.", Url = "/SecuritySuite")]
    [ExportViewToRegion(MaintainName.SecuritySuiteV, ViewContainer.MaintainContainer)]
    public partial class SecuritySuite : UserControl
    {
        public SecuritySuite()
        {
            InitializeComponent();
            Gsafety.PTMS.Share.ApplicationContext.Instance.StringResourceReader.TranslateDataGrid(SuiteDataGrid);
        }
    }
}
