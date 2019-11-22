using Gsafety.PTMS.Constants;
using Gsafety.PTMS.Share;
using Jounce.Core.View;
using Jounce.Regions.Core;
/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 9f821656-cdf5-4bc8-8026-994c48f92983      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-PENGGL
/////                 Author: TEST(penggl)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Maintain.Views
/////    Project Description:    
/////             Class Name: UpgradeStatus
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/10/28 15:15:24
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/10/28 15:15:24
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
    [ExportAsView(MaintainName.UpgradeStatusV, ToolTip = "Click to view some text.", Url = "/UpgradeStatus")]
    [ExportViewToRegion(MaintainName.UpgradeStatusV, ViewContainer.MaintainContainer)]
    public partial class UpgradeStatus : UserControl
    {
        public UpgradeStatus()
        {
            InitializeComponent();
            Gsafety.PTMS.Share.ApplicationContext.Instance.StringResourceReader.TranslateDataGrid(VersionUpgradeStatusDataGrid);
        }
    }
}
