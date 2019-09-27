/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 7f62998d-d7b7-4b1f-81e3-a94f2827ac74      
/////             clrversion: 4.0.30319.17929
/////Registered organization: 
/////           Machine Name: PC-LANQ
/////                 Author: TEST(lanq)
/////======================================================================
/////           Project Name: Gsafety.PTMS.BaseInformation.Views
/////    Project Description:    
/////             Class Name: SuiteInfo
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/8/12 16:07:19
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/8/12 16:07:19
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

namespace Gsafety.PTMS.BaseInformation.Views
{
    [ExportAsView(BaseInformationName.SuiteInfoV, Category = BaseInformationName.CategoryName,
        MenuName = BaseInformationName.MenuName, MenuTitle = "BASEINFO_SuiteInfo",
        ToolTip = "Click to view some text.", Url = "/SuiteInfo", Order = 2)]
    [ExportViewToRegion(BaseInformationName.SuiteInfoV, BaseInformationName.BaseInfoContainer)]
    public partial class SuiteInfo : UserControl
    {
        public SuiteInfo()
        {
            InitializeComponent();
            Gsafety.PTMS.Share.ApplicationContext.Instance.StringResourceReader.TranslateDataGrid(SuiteDataGrid);
          
        }
    }
}
