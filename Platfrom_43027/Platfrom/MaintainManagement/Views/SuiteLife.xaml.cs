using Gsafety.PTMS.Constants;
using Jounce.Core.View;
using Jounce.Regions.Core;
/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 01c7483f-0eda-47cd-8059-c2ebdddc51f5      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-PENGGL
/////                 Author: TEST(penggl)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Maintain.Views
/////    Project Description:    
/////             Class Name: ServiceLife
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/11/21 12:47:22
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/11/21 12:47:22
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

    [ExportAsView(MaintainName.SuiteLifeV, ToolTip = "Click to view some text.", Url = "/LifeTime")]
    [ExportViewToRegion(MaintainName.SuiteLifeV, ViewContainer.MaintainContainer)]
    public partial class SuiteLife : UserControl
    {
        public SuiteLife()
        {
            InitializeComponent();

            Gsafety.PTMS.Share.ApplicationContext.Instance.StringResourceReader.TranslateDataGrid(SuiteLifeGrid);
        }
    }
}
