/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 0243218f-7b7e-41ad-ae2c-c7cf7523e5e6      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-LINGL
/////                 Author: TEST(zhangzl)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Installation.Views
/////    Project Description:    
/////             Class Name: VehicleRegister
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/9/3 15:11:35
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/9/3 15:11:35
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
using System.Windows.Navigation;
using Jounce.Core.View;
using Jounce.Regions.Core;
using Gsafety.PTMS.Share;
using Gsafety.PTMS.Constants;

namespace Gsafety.PTMS.Installation.Views
{
    [ExportAsView(InstallationName.VehicleRegisterV, Category = InstallationName.CategoryName,
ToolTip = "Click to view some text.", Url = "/VehicleRegister")]
    [ExportViewToRegion(InstallationName.VehicleRegisterV, ViewContainer.InstallContainer)]

    public partial class VehicleRegister : UserControl
    {
        public VehicleRegister()
        {
            InitializeComponent();
        }



    }
}
