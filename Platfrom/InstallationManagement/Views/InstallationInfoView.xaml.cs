/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: e5b78bc7-fc6c-4b9e-a318-bc5d706ccf9a      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: GAOZITIAN-PC
/////                 Author: TEST(gaozt)
/////======================================================================
/////           Project Name: Gsafety.Ant.Installation.Views
/////    Project Description:    
/////             Class Name: InstallationInfoView
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/8/16 11:25:10
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/8/16 11:25:10
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
    [ExportAsView(InstallationName.InstallationInfoV)]
    [ExportViewToRegion(InstallationName.InstallationInfoV, Constants.InstallContainer)]
    public partial class InstallationInfoView : UserControl
    {
        public InstallationInfoView()
        {
            InitializeComponent();
        }
    }
}
