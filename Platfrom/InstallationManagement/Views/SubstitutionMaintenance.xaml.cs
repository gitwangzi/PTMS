using Gsafety.PTMS.Constants;
using Gsafety.PTMS.Share;
using Jounce.Core.View;
using Jounce.Regions.Core;
/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 0ed2e920-5be1-4736-8a81-a64402ec38d3      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: LIN-20130409ZRS
/////                 Author: TEST(zhujf)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Installation.Views
/////    Project Description:    
/////             Class Name: PeplaceMaintenance
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/8/30 17:10:14
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/8/30 17:10:14
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

namespace Gsafety.PTMS.Installation.Views
{
    [ExportAsView(InstallationName.SubstitutionMaintenanceV)]
    [ExportViewToRegion(InstallationName.SubstitutionMaintenanceV, ViewContainer.InstallContainer)]
    public partial class SubstitutionMaintenance : UserControl
    {
        public SubstitutionMaintenance()
        {
            InitializeComponent();
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {

        }
    }
}
