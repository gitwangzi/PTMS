/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 93e6750e-67cd-47d7-8d8f-4f0abb4375a3      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: GAOZITIAN-PC
/////                 Author: TEST(gaozt)
/////======================================================================
/////           Project Name: Gsafety.Ant.Installation.Views
/////    Project Description:    
/////             Class Name: SuiteMaintenanceView
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/8/16 11:56:56
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/8/16 11:56:56
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
using Gsafety.Ant.Share;

namespace Gsafety.Ant.Installation.Views
{
    [ExportAsView(InstallationName.SuiteMaintenanceV)]
    [ExportViewToRegion(InstallationName.SuiteMaintenanceV, Constants.InstallContainer)]
    public partial class SuiteMaintenanceView : UserControl
    {
        public SuiteMaintenanceView()
        {
            InitializeComponent();
        }
    }
}
