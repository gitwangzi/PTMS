/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 91d0b9e7-f0e1-4910-9de4-e768b2c6d7ca      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-WANGZS
/////                 Author: TEST(wangzs)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Manager.Views
/////    Project Description:    
/////             Class Name: SetupStationUserEditView
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/8/6 15:07:29
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/8/6 15:07:29
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
namespace Gsafety.PTMS.Manager.Views
{
    [ExportAsView(ManagerName.SetupStationUserEditView)]
    [ExportViewToRegion(ManagerName.SetupStationUserEditView, ManagerName.ManagerContainer)]
    public partial class SetupStationUserEditView : UserControl
    {
        public SetupStationUserEditView()
        {
            InitializeComponent();
        }
    }
}
