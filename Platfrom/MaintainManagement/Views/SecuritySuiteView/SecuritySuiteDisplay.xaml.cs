using Gsafety.PTMS.Constants;
using Gsafety.PTMS.Share;
using Jounce.Core.View;
using Jounce.Regions.Core;
/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 41b6acec-a47c-4d3c-8f65-b488ab98ebc6      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-PENGGL
/////                 Author: TEST(penggl)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Maintain.Views
/////    Project Description:    
/////             Class Name: SuiteQueryDisplay
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/9/11 17:08:23
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/9/11 17:08:23
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
    [ExportAsView(MaintainName.SecuritySuiteDisplayV)]
    [ExportViewToRegion(MaintainName.SecuritySuiteDisplayV, ViewContainer.MaintainContainer)]
    public partial class SecuritySuiteDisplay : UserControl
    {
        public SecuritySuiteDisplay()
        {
            InitializeComponent();
        }
    }
}
