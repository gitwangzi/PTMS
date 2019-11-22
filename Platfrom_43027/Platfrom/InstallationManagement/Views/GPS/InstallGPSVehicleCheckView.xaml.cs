/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: a5cd8f24-f555-47ee-9531-7132f9193c35      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-DENGZL
/////                 Author: TEST(dengzl)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Installation.Views
/////    Project Description:    
/////             Class Name: InstallingDevice
/////          Class Version: v1.0.0.0
/////            Create Time: 8/14/2013 3:16:09 PM
/////      Class Description:  
/////======================================================================
/////          Modified Time: 8/14/2013 3:16:09 PM
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
using Gsafety.PTMS.Share;
using Gsafety.PTMS.Constants;

namespace Gsafety.PTMS.Installation.Views
{
    [ExportAsView(InstallationName.InstallGPSVehicleCheckV, Category = InstallationName.CategoryName,
      ToolTip = "Click to view some text.", Url = "/InstallGPSVehicleCheckV")]
    [ExportViewToRegion(InstallationName.InstallGPSVehicleCheckV, ViewContainer.InstallContainer)]
    public partial class InstallGPSVehicleCheckView : UserControl
    {
        public InstallGPSVehicleCheckView()
        {
            InitializeComponent();
            this.MouseRightButtonDown += ChildWindow_MouseRightButtonDown;
        }
        private void ChildWindow_MouseRightButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            e.Handled = true;
        }
    }
}
