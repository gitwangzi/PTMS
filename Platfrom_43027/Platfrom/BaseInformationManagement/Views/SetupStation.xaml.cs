/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 9797e89a-7883-434b-9b2d-7534419c59e9      
/////             clrversion: 4.0.30319.17929
/////Registered organization: 
/////           Machine Name: PC-LANQ
/////                 Author: TEST(lanq)
/////======================================================================
/////           Project Name: Gsafety.PTMS.BaseInformation.Views
/////    Project Description:    
/////             Class Name: SetupStation
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/8/13 16:54:25
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/8/13 16:54:25
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
    [ExportAsView(BaseInformationName.SetupStationV, Category = BaseInformationName.CategoryName,
        MenuName = BaseInformationName.MenuName, MenuTitle = "BASEINFO_SetupStation",
        ToolTip = "Click to view some text.", Url = "/SetupStation", Order = 3)]
    [ExportViewToRegion(BaseInformationName.SetupStationV, BaseInformationName.BaseInfoContainer)]
    public partial class SetupStation : UserControl
    {
        public SetupStation()
        {
            
            InitializeComponent();
            Gsafety.PTMS.Share.ApplicationContext.Instance.StringResourceReader.TranslateDataGrid(SetupStationDataGrid);
   
        }
    }
}
