/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: bcaf6bcc-e304-43f6-a96d-f7e4182b42e5      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: GAOZITIAN-PC
/////                 Author: TEST(gaozt)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Installation.Views
/////    Project Description:    
/////             Class Name: UploadCarNumberView
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/8/16 13:49:49
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/8/16 13:49:49
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
using Gsafety.PTMS.Constants;

namespace Gsafety.PTMS.Installation.Views
{
    [ExportAsView(InstallationName.InstallVehcileSuiteCheckV)]
    [ExportViewToRegion(InstallationName.InstallVehcileSuiteCheckV, ViewContainer.InstallContainer)]
    public partial class InstallVehcileSuiteCheckView : UserControl
    {
        public InstallVehcileSuiteCheckView()
        {
            InitializeComponent();
        }
    }
}
