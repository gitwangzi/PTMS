using Gsafety.PTMS.Share;
using Jounce.Core.View;
using Jounce.Regions.Core;
/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 5e9a8211-c37f-4456-ba9c-105432480ef1      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-PENGGL
/////                 Author: TEST(penggl)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Maintain.Views
/////    Project Description:    
/////             Class Name: SuiteHistoryRecord
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/9/11 15:33:53
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/9/11 15:33:53
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
using Gsafety.PTMS.Constants;

namespace Gsafety.PTMS.Maintain.Views
{
    [ExportAsView(MaintainName.SuiteHistoryRecordV, ToolTip = "Click to view some text.", Url = "/SuiteHistoryRecord")]
    [ExportViewToRegion(MaintainName.SuiteHistoryRecordV, ViewContainer.MaintainContainer)]
    public partial class SuiteHistoryRecord : UserControl
    {
        public SuiteHistoryRecord()
        {
            InitializeComponent();
            Gsafety.PTMS.Share.ApplicationContext.Instance.StringResourceReader.TranslateDataGrid(SuiteDataGrid);
        }
    }
}
