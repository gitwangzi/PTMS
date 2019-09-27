/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: c73b869d-28ae-463a-b399-592b445027af      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-DENGZL
/////                 Author: TEST(dengzl)
/////======================================================================
/////           Project Name: Gsafety.PTMS.SecuritySuite.Views
/////    Project Description:    
/////             Class Name: MaintainRecord
/////          Class Version: v1.0.0.0
/////            Create Time: 8/9/2013 9:21:44 AM
/////      Class Description:  
/////======================================================================
/////          Modified Time: 8/9/2013 9:21:44 AM
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

namespace Gsafety.PTMS.Maintain.Views
{
    [ExportAsView(MaintainName.MaintainRecordV,  MenuTitle = "SUITE_MaintainRecord",
        ToolTip = "Click to view some text.", Url = "/MaintainRecord", Order=2)]
    [ExportViewToRegion(MaintainName.MaintainRecordV, ViewContainer.MaintainContainer)]
    public partial class MaintainRecord : UserControl
    {
        public MaintainRecord()
        {
            InitializeComponent();
            ApplicationContext.Instance.StringResourceReader.TranslateDataGrid(MaintainDataGrid);
        }
    }
}
