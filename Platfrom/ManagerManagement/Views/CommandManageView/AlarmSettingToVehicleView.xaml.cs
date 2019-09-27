using Jounce.Core.View;
using Jounce.Regions.Core;
/////Copyright (C) Gsafety 2015 .All Rights Reserved.
/////======================================================================
/////                   Guid: 82a17d6c-5713-43e2-86b6-3b77b9cc0d20      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-ZHANGH
/////                 Author: GJSY(zhangh)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Manager.Views.CommandManageView
/////    Project Description:    
/////             Class Name: AlarmSettingToVehicleView
/////          Class Version: v1.0.0.0
/////            Create Time: 2015/1/13 16:30:58
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2015/1/13 16:30:58
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
    [ExportAsView(ManagerName.AlarmSettingToVehicleView)]
    [ExportViewToRegion(ManagerName.AlarmSettingToVehicleView, ManagerName.ManagerContainer)]
    public partial class AlarmSettingToVehicleView : UserControl
    {
        public AlarmSettingToVehicleView()
        {
            InitializeComponent();
        }
    }
}
