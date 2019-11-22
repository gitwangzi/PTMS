using Gsafety.PTMS.Constants;
using Jounce.Core.View;
using Jounce.Regions.Core;
/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: d8578039-d2dd-46aa-b9cd-ec3d8b75eead      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-PENGGL
/////                 Author: TEST(penggl)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Maintain.Views
/////    Project Description:    
/////             Class Name: HandleRecordDetail
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/11/21 14:56:50
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/11/21 14:56:50
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
    [ExportAsView(MaintainName.HandleRecordDetailV)]
    [ExportViewToRegion(MaintainName.HandleRecordDetailV, ViewContainer.MaintainContainer)]
    public partial class HandleRecordDetail : UserControl
    {
        public HandleRecordDetail()
        {
            InitializeComponent();
        }
    }

}
