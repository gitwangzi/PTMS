using Jounce.Core.View;
using Jounce.Regions.Core;
/////Copyright (C) Gsafety 2014 .All Rights Reserved.
/////======================================================================
/////                   Guid: 6c265a67-9abf-4b8d-9cbe-79229891ace1      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-ZHOUDD
/////                 Author: GJSY(zhoudd)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Manager.Views.CommandManageView
/////    Project Description:    
/////             Class Name: GpsSettingAdd
/////          Class Version: v1.0.0.0
/////            Create Time: 2014/10/22 15:49:43
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2014/10/22 15:49:43
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Gsafety.PTMS.Manager.Views.CommandManageView
{
    [ExportAsView(ManagerName.GpsSettingToVechileView)]
    [ExportViewToRegion(ManagerName.GpsSettingToVechileView, ManagerName.ManagerContainer)]
    public partial class GpsSettingToVehicleView : UserControl
    {
        public GpsSettingToVehicleView()
        {
            InitializeComponent();
        }
    }
}
