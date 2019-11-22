/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 70878efc-6b4c-47d0-afe0-3eac6909d7f7      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: GAOZITIAN-PC
/////                 Author: TEST(gaozt)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Installation.Views
/////    Project Description:    
/////             Class Name: InputDeviceInfoView
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/8/16 10:59:23
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/8/16 10:59:23
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

namespace Gsafety.PTMS.Installation.Views
{
    [ExportAsView(InstallationName.InstallGPSCheckV)]
    [ExportViewToRegion(InstallationName.InstallGPSCheckV, ViewContainer.InstallContainer)]
    public partial class InstallGPSCheckView : UserControl
    {
        public InstallGPSCheckView()
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
