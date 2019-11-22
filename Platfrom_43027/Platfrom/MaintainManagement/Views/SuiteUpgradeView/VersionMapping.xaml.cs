using Gsafety.PTMS.Constants;
using Gsafety.PTMS.Share;
using Jounce.Core.View;
using Jounce.Regions.Core;
/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 9e0bc0b9-e195-4061-a1b2-d787638adace      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-PENGGL
/////                 Author: TEST(penggl)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Maintain.Views
/////    Project Description:    
/////             Class Name: UpgradeVersionDetail
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/9/11 15:38:43
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/9/11 15:38:43
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
    [ExportAsView(MaintainName.VersionMappingV, ToolTip = "Click to view some text.", Url = "/UpgradeVersionDetail")]
    [ExportViewToRegion(MaintainName.VersionMappingV, ViewContainer.MaintainContainer)]
    public partial class VersionMapping : UserControl
    {
        public VersionMapping()
        {
            InitializeComponent();
           
            Gsafety.PTMS.Share.ApplicationContext.Instance.StringResourceReader.TranslateDataGrid(SuiteVersionMapDataGrid);
        }
    }
}
