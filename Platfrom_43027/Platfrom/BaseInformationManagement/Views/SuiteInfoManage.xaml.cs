/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: d89ab375-5841-4331-b2e2-2b15adfe352a      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-PENGGL
/////                 Author: TEST(penggl)
/////======================================================================
/////           Project Name: Gsafety.PTMS.BaseInformation.Views
/////    Project Description:    
/////             Class Name: SuiteManage
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/8/22 8:42:51
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/8/22 8:42:51
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
using Jounce.Core.View;
using Jounce.Regions.Core;

namespace Gsafety.PTMS.BaseInformation.Views
{
    [ExportAsView(BaseInformationName.SuiteInfoManageV)]
    [ExportViewToRegion(BaseInformationName.SuiteInfoManageV, BaseInformationName.BaseInfoContainer)]
    public partial class SuiteInfoManage : UserControl
    {
        public SuiteInfoManage()
        {
            InitializeComponent();
        }
    }
}
