using Gsafety.PTMS.Constants;
using Gsafety.PTMS.Share;
using Jounce.Core.View;
using Jounce.Regions.Core;
/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 7e73948f-435c-4e65-bbeb-4d098078bb9a      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-PENGGL
/////                 Author: TEST(penggl)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Maintain.Views.SecuritySuiteView
/////    Project Description:    
/////             Class Name: SuiteInstalling
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/11/6 11:23:14
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/11/6 11:23:14
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

namespace Gsafety.PTMS.Maintain.Views.SecuritySuiteView
{
    [ExportAsView(MaintainName.SuiteInstallingV, ToolTip = "Click to view some text.", Url = "/SuiteInstalling")]
    [ExportViewToRegion(MaintainName.SuiteInstallingV, ViewContainer.MaintainContainer)]
    public partial class SuiteInstalling : UserControl
    {
        public SuiteInstalling()
        {
            InitializeComponent();
            Gsafety.PTMS.Share.ApplicationContext.Instance.StringResourceReader.TranslateDataGrid(SuiteInstallingDataGrid);
        }
    }
}
