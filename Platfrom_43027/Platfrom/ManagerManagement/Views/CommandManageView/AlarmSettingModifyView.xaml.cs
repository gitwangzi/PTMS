/////Copyright (C) Gsafety 2014 .All Rights Reserved.
/////======================================================================
/////                   Guid: 9d066013-2ebc-487b-a3a8-f72dd2e0a7c0      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-ZHANGH
/////                 Author: GJSY(zhangh)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Manager.Views.CommandManageView
/////    Project Description:    
/////             Class Name: AlarmSettingModifyView
/////          Class Version: v1.0.0.0
/////            Create Time: 2014/12/12 17:34:16
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2014/12/12 17:34:16
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

namespace Gsafety.PTMS.Manager.Views.CommandManageView
{
    [ExportAsView(ManagerName.AlarmSettingModifyView)]
    [ExportViewToRegion(ManagerName.AlarmSettingModifyView, ManagerName.ManagerContainer)]
    public partial class AlarmSettingModifyView : UserControl
    {
        public AlarmSettingModifyView()
        {
            InitializeComponent();
        }
    }
}
