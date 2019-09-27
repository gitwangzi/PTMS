using Jounce.Core.View;
using Jounce.Regions.Core;
/////Copyright (C) Gsafety 2014 .All Rights Reserved.
/////======================================================================
/////                   Guid: 032c7131-f1bb-4c5f-9cb3-dd85496afb23      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-ZHANGH
/////                 Author: GJSY(zhangh)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Manager.Views.CommandManageView
/////    Project Description:    
/////             Class Name: GpsSettingModifyView
/////          Class Version: v1.0.0.0
/////            Create Time: 2014/12/12 15:15:32
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2014/12/12 15:15:32
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

namespace Gsafety.PTMS.Manager.Views.CommandManageView
{
    [ExportAsView(ManagerName.GpsSettingModifyView)]
    [ExportViewToRegion(ManagerName.GpsSettingModifyView, ManagerName.ManagerContainer)]
    public partial class GpsSettingModifyView : UserControl
    {
        public GpsSettingModifyView()
        {
            InitializeComponent();
        }
    }
}
