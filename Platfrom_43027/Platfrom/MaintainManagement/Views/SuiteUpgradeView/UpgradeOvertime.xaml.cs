using Gsafety.PTMS.Constants;
using Jounce.Core.View;
using Jounce.Regions.Core;
/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 2164ab89-9edb-4bb7-8416-c3ae2c97db11      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-PENGGL
/////                 Author: TEST(penggl)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Maintain.Views.SuiteUpgradeView
/////    Project Description:    
/////             Class Name: UpgradeOvertime
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/11/20 10:18:33
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/11/20 10:18:33
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

namespace Gsafety.PTMS.Maintain.Views.SuiteUpgradeView
{
    [ExportAsView(MaintainName.UpgradeOvertimeV, ToolTip = "Click to view some text.", Url = "/UpgradeOvertime")]
    [ExportViewToRegion(MaintainName.UpgradeOvertimeV, ViewContainer.MaintainContainer)]
    public partial class UpgradeOvertime : UserControl
    {
        public UpgradeOvertime()
        {
            InitializeComponent();
            Gsafety.PTMS.Share.ApplicationContext.Instance.StringResourceReader.TranslateDataGrid(UpgradeOvertimeDataGrid);
        }
    }

}
