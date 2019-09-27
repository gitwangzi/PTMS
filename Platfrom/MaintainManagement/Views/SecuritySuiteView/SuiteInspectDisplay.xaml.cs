using Gsafety.PTMS.Constants;
using Gsafety.PTMS.Share;
using Jounce.Core.View;
using Jounce.Regions.Core;
/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 0dcaf582-de16-4ba1-9a50-9c38aa7f9a6f      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-PENGGL
/////                 Author: TEST(penggl)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Maintain.Views.SecuritySuiteView
/////    Project Description:    
/////             Class Name: SuiteInspectDisplay
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/11/5 10:02:46
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/11/5 10:02:46
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
    [ExportAsView(MaintainName.SuiteInspectDisplayV)]
    [ExportViewToRegion(MaintainName.SuiteInspectDisplayV, ViewContainer.MaintainContainer)]
    public partial class SuiteInspectDisplay : UserControl
    {
        public SuiteInspectDisplay()
        {
            InitializeComponent();
        }
    }
}
