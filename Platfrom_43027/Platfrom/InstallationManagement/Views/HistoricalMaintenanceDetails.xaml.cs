using Gsafety.PTMS.Constants;
using Gsafety.PTMS.Share;
using Jounce.Core.View;
using Jounce.Regions.Core;
/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 728886e8-45cb-45cc-b2db-0cd7ca7c4185      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: GAOZITIAN-PC
/////                 Author: TEST(gaozt)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Installation.Views
/////    Project Description:    
/////             Class Name: HistoricalMaintenanceDetails
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/9/6 11:20:08
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/9/6 11:20:08
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

namespace Gsafety.PTMS.Installation.Views
{
    [ExportAsView(InstallationName.HistoricalMaintenanceDetailsV)]
    [ExportViewToRegion(InstallationName.HistoricalMaintenanceDetailsV, ViewContainer.InstallContainer)]
    public partial class HistoricalMaintenanceDetails : UserControl
    {
        public HistoricalMaintenanceDetails()
        {
            InitializeComponent();
        }
    }
}
