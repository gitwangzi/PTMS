using Gsafety.PTMS.Constants;
using Gsafety.PTMS.Share;
using Jounce.Core.View;
using Jounce.Regions.Core;
/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: c564a320-dcef-4e5d-8b8c-be9538974ec9      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-PENGGL
/////                 Author: TEST(penggl)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Maintain.Views
/////    Project Description:    
/////             Class Name: AbnormalSuiteQuery
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/9/12 11:27:45
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/9/12 11:27:45
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
    [ExportAsView(MaintainName.MaintenanceHandleV, ToolTip = "Click to view some text.", Url = "/MaintenanceHandle")]
    [ExportViewToRegion(MaintainName.MaintenanceHandleV, ViewContainer.MaintainContainer)]
    public partial class MaintenanceHandle : UserControl
    {
        public MaintenanceHandle()
        {
            InitializeComponent();
            Gsafety.PTMS.Share.ApplicationContext.Instance.StringResourceReader.TranslateDataGrid(WorkingSuiteDataGrid);
        }
    }
}
