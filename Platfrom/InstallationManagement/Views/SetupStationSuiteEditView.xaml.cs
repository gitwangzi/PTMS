/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: e42efcbe-747c-4961-82a9-e4aaea1542cb      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: GAOZITIAN-PC
/////                 Author: TEST(gaozt)
/////======================================================================
/////           Project Name: Gsafety.Ant.Installation.Views
/////    Project Description:    
/////             Class Name: SetupStationSuiteEditView
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/8/16 11:34:39
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/8/16 11:34:39
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
    [ExportAsView(InstallationName.SetupStationSuiteEditV)]
    [ExportViewToRegion(InstallationName.SetupStationSuiteEditV, Constants.InstallContainer)]
    public partial class SetupStationSuiteEditView : UserControl
    {
        public SetupStationSuiteEditView()
        {
            InitializeComponent();
        }
    }
}
