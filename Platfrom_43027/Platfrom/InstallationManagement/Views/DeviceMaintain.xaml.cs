/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 5a42dcb7-0853-4f1c-a13f-ae1002b597d8      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-DENGZL
/////                 Author: TEST(dengzl)
/////======================================================================
/////           Project Name: Gsafety.Ant.Installation.Views
/////    Project Description:    
/////             Class Name: DeviceMaintain
/////          Class Version: v1.0.0.0
/////            Create Time: 8/14/2013 3:19:44 PM
/////      Class Description:  
/////======================================================================
/////          Modified Time: 8/14/2013 3:19:44 PM
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
using Gsafety.Ant.Share;

namespace Gsafety.Ant.Installation.Views
{
    [ExportAsView(InstallationName.DeviceMaintainV, Category = InstallationName.CategoryName,
      ToolTip = "Click to view some text.", Url = "/DeviceMaintain")]
    [ExportViewToRegion(InstallationName.DeviceMaintainV, Constants.InstallContainer)]
    public partial class DeviceMaintain : UserControl
    {
        public DeviceMaintain()
        {
            InitializeComponent();
        }
    }
}
