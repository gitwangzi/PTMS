using Gsafety.PTMS.Constants;
using Gsafety.PTMS.Share;
using Jounce.Core.View;
using Jounce.Regions.Core;
/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 6b055c0e-1a35-4e6d-afcb-3c0cfa976872      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-PENGGL
/////                 Author: TEST(penggl)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Maintain.Views
/////    Project Description:    
/////             Class Name: SuiteUpgradeRecordDispaly
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/9/17 16:38:26
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/9/17 16:38:26
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
    [ExportAsView(MaintainName.UpgradeRecordDisplayV)]
    [ExportViewToRegion(MaintainName.UpgradeRecordDisplayV, ViewContainer.MaintainContainer)]
    public partial class UpgradeRecordDispaly : UserControl
    {
        public UpgradeRecordDispaly()
        {
            InitializeComponent();
        }
    }
}
