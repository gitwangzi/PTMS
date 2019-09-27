using Gsafety.PTMS.Constants;
using Jounce.Core.View;
using Jounce.Regions.Core;
/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 1da7540a-d448-40aa-a838-e3a7741f1264      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-PENGGL
/////                 Author: TEST(penggl)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Maintain.Views
/////    Project Description:    
/////             Class Name: HandleRecord
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/11/21 14:56:24
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/11/21 14:56:24
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
    [ExportAsView(MaintainName.HandleRecordV, ToolTip = "Click to view some text.", Url = "/HandleRecord")]
    [ExportViewToRegion(MaintainName.HandleRecordV, ViewContainer.MaintainContainer)]
    public partial class HandleRecord : UserControl
    {
        public HandleRecord()
        {
            InitializeComponent();
            Gsafety.PTMS.Share.ApplicationContext.Instance.StringResourceReader.TranslateDataGrid(HandleRecordGrid1);
        }
    }
}
