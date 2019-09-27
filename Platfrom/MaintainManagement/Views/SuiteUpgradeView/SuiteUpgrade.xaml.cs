﻿using Gsafety.PTMS.Constants;
using Gsafety.PTMS.Share;
using Jounce.Core.View;
using Jounce.Regions.Core;
/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 148b6de9-97b3-439f-9bae-a84f8cf09a43      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-PENGGL
/////                 Author: TEST(penggl)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Maintain.Views
/////    Project Description:    
/////             Class Name: SuiteUpgrade
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/9/11 15:39:01
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/9/11 15:39:01
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
    [ExportAsView(MaintainName.SuiteUpgradeV, ToolTip = "Click to view some text.", Url = "/SuiteUpgrade")]
    [ExportViewToRegion(MaintainName.SuiteUpgradeV, ViewContainer.MaintainContainer)]
    public partial class SuiteUpgrade : UserControl
    {
        public SuiteUpgrade()
        {
            InitializeComponent();
            Gsafety.PTMS.Share.ApplicationContext.Instance.StringResourceReader.TranslateDataGrid(SuiteUpdateRecordDataGrid);
        }
    }
}
