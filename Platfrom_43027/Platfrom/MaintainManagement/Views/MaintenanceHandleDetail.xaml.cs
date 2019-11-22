using Gsafety.PTMS.Constants;
using Gsafety.PTMS.Share;
using Jounce.Core.View;
using Jounce.Regions.Core;
/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: a1977f69-c839-44ec-9807-89f7adbcb8ac      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-PENGGL
/////                 Author: TEST(penggl)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Maintain.Views
/////    Project Description:    
/////             Class Name: AbnormalSuiteMaintainHandle
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/9/11 15:38:19
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/9/11 15:38:19
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
    [ExportAsView(MaintainName.MaintenanceHandleDetailV)]
    [ExportViewToRegion(MaintainName.MaintenanceHandleDetailV, ViewContainer.MaintainContainer)]
    public partial class MaintenanceHandleDetail : UserControl
    {
        public MaintenanceHandleDetail()
        {
            InitializeComponent();
        }
    }
}
