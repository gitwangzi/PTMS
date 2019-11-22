using Gsafety.PTMS.Constants;
using Gsafety.PTMS.Share;
using Jounce.Core.View;
using Jounce.Regions.Core;
/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 2f61053e-e3d0-4e58-b8ae-f6b5eee2003e      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-PENGGL
/////                 Author: TEST(penggl)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Maintain.Views
/////    Project Description:    
/////             Class Name: UpgradeVersionEdit
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/9/22 10:44:25
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/9/22 10:44:25
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
    [ExportAsView(MaintainName.VersionMappingEditV, ToolTip = "Click to view some text.", Url = "/VersionMappingEdit")]
    [ExportViewToRegion(MaintainName.VersionMappingEditV, ViewContainer.MaintainContainer)]
    public partial class VersionMappingEdit : UserControl
    {
        public VersionMappingEdit()
        {
            InitializeComponent();
        }
    }
}
