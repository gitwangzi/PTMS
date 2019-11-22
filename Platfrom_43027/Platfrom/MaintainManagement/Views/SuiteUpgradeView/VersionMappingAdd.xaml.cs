using Gsafety.PTMS.Constants;
using Gsafety.PTMS.Share;
using Jounce.Core.View;
using Jounce.Regions.Core;
/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 78980463-b480-45a4-974e-7cff7c3d87ac      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-PENGGL
/////                 Author: TEST(penggl)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Maintain.Views
/////    Project Description:    
/////             Class Name: UpgradeVersionAdd
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/9/24 11:39:08
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/9/24 11:39:08
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
    [ExportAsView(MaintainName.VersionMappingAddV, ToolTip = "Click to view some text.", Url = "/VersionMappingAdd")]
    [ExportViewToRegion(MaintainName.VersionMappingAddV, ViewContainer.MaintainContainer)]
    public partial class VersionMappingAdd : UserControl
    {
        public VersionMappingAdd()
        {
            InitializeComponent();
        }
    }
}
