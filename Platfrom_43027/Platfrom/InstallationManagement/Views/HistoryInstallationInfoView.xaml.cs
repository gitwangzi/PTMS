/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 6033aa69-82ef-4518-98c0-f38ff2eb8a7a      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: GAOZITIAN-PC
/////                 Author: TEST(gaozt)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Installation.Views
/////    Project Description:    
/////             Class Name: HistoryInstallationInfoView1
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/8/16 10:39:03
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/8/16 10:39:03
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
using Jounce.Core;
using Jounce.Core.View;
using Jounce.Regions.Core;
using Gsafety.PTMS.Share;
using Gsafety.PTMS.Constants;

namespace Gsafety.PTMS.Installation.Views
{
    [ExportAsView(InstallationName.HistoryInstallationInfoV, Category = InstallationName.CategoryName,
       ToolTip = "Click to view some text.", Url = "/HistoryInstallationInfo")]
    [ExportViewToRegion(InstallationName.HistoryInstallationInfoV, ViewContainer.InstallContainer)]
    public partial class HistoryInstallationInfoView : UserControl
    {
        public HistoryInstallationInfoView()
        {
            InitializeComponent();
            Gsafety.PTMS.Share.ApplicationContext.Instance.StringResourceReader.TranslateDataGrid(SuiteDataGrid);
        }
    }
}
