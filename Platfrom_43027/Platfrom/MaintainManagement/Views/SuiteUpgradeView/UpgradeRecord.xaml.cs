using Gsafety.PTMS.Constants;
using Gsafety.PTMS.Share;
using Jounce.Core.View;
using Jounce.Regions.Core;
/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 573a8c12-6ea0-48c4-ad2c-0328dedfc20d      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-PENGGL
/////                 Author: TEST(penggl)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Maintain.Views
/////    Project Description:    
/////             Class Name: SuiteUpgradeRecord
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/9/11 15:34:43
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/9/11 15:34:43
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
    [ExportAsView(MaintainName.UpgradeRecordV, ToolTip = "Click to view some text.", Url = "/UpgradeRecord")]
    [ExportViewToRegion(MaintainName.UpgradeRecordV, ViewContainer.MaintainContainer)]
    public partial class UpgradeRecord : UserControl
    {
        public UpgradeRecord()
        {
            InitializeComponent();
            Gsafety.PTMS.Share.ApplicationContext.Instance.StringResourceReader.TranslateDataGrid(VehicleDataGrid);
        }
    }
}
