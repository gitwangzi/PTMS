using Gsafety.PTMS.Constants;
using Gsafety.PTMS.Share;
using Jounce.Core.View;
using Jounce.Regions.Core;
/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 6ef0a94a-f74e-4396-88ee-a941a722caa5      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-PENGGL
/////                 Author: TEST(penggl)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Maintain.Views.SecuritySuiteView
/////    Project Description:    
/////             Class Name: SuiteInspect
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/11/4 18:44:09
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/11/4 18:44:09
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
    [ExportAsView(MaintainName.SuiteInspectV, ToolTip = "Click to view some text.", Url = "/SuiteInspect")]
    [ExportViewToRegion(MaintainName.SuiteInspectV, ViewContainer.MaintainContainer)]
    public partial class SuiteInspect : UserControl
    {
        public SuiteInspect()
        {
            InitializeComponent();
            Gsafety.PTMS.Share.ApplicationContext.Instance.StringResourceReader.TranslateDataGrid(SuiteInspectDataGrid);
        }
    }
}
