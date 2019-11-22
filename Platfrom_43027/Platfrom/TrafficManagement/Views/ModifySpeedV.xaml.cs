/////Copyright (C) Gsafety 2014 .All Rights Reserved.
/////======================================================================
/////                   Guid: 2edfa645-c0d1-48b1-8237-8952da509e61      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-ZHANGH
/////                 Author: GJSY(zhangh)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Traffic.Views
/////    Project Description:    
/////             Class Name: ModifySpeedV
/////          Class Version: v1.0.0.0
/////            Create Time: 2014/12/23 9:55:48
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2014/12/23 9:55:48
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

namespace Gsafety.PTMS.Traffic.Views
{
    [ExportAsView(TrafficName.ModifyspeedlimitView)]
    [ExportViewToRegion(TrafficName.ModifyspeedlimitView, TrafficName.TrafficContainer)]
    public partial class ModifySpeedV : UserControl
    {
        public ModifySpeedV()
        {
            InitializeComponent();
            this.MouseRightButtonDown += ChildWindow_MouseRightButtonDown;
        }
        private void ChildWindow_MouseRightButtonDown(object sender,

System.Windows.Input.MouseButtonEventArgs e)
        {
            e.Handled = true;
        }

    }
}
