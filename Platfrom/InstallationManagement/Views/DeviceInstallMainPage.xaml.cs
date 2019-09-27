/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 3569c024-31ff-4a00-ab77-b39ba7d8086c      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-DENGZL
/////                 Author: TEST(dengzl)
/////======================================================================
/////           Project Name: Gsafety.Ant.BaseInformation.Views
/////    Project Description:    
/////             Class Name: BaseInforMainPage
/////          Class Version: v1.0.0.0
/////            Create Time: 8/6/2013 5:36:45 PM
/////      Class Description:  
/////======================================================================
/////          Modified Time: 8/6/2013 5:36:45 PM
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
using System.Windows.Navigation;

namespace Gsafety.Ant.Installation.Views
{
    [ExportAsView(ManagerName.ManagerMainPageV)]
    [ExportViewToRegion(ManagerName.ManagerMainPageV, Gsafety.Ant.Share.Constants.CentralMainContainer)]
    //[ExportAsView(InstallationName.DevInstallMainPageV, Category = "Navigation", MenuName = "Gis Manager", ToolTip = "Click to view some text.")]
    //[ExportViewToRegion(InstallationName.DevInstallMainPageV, Gsafety.Ant.Share.Constants.CentralMainContainer)]
    public partial class BaseInfoMainPage : UserControl
    {
        public BaseInfoMainPage()
        {
            InitializeComponent();
        }

        private void ContentFrame_Navigated(object sender, NavigationEventArgs e)
        {
        }

        // If an error occurs during navigation, show an error window
        private void ContentFrame_NavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            e.Handled = true;
        }

        [ExportAsView(BaseInformationName.BaseInfoMenuV)]
        public UserControl BaseInfoMenu
        {
            get { return baseInfoMenu; }
        }
    }
}
