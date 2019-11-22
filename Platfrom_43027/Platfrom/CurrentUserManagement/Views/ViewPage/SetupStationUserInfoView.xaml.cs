/////Copyright (C) Gsafety 2014 .All Rights Reserved.
/////======================================================================
/////                   Guid: bf711dbc-e0c9-4c64-8600-3339650950f0      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: LIN-20130409ZRS
/////                 Author: TEST(zhujf)
/////======================================================================
/////           Project Name: Gsafety.PTMS.CurrentUserManagement.Views.ViewPage
/////    Project Description:    
/////             Class Name: SetupStationUserInfoView
/////          Class Version: v1.0.0.0
/////            Create Time: 2014/1/7 11:15:26
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2014/1/7 11:15:26
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
using Gsafety.PTMS.Constants;

namespace Gsafety.PTMS.CurrentUserManagement.Views.ViewPage
{
    [ExportAsView(CurrentUserName.SetupStationUserInfoView)]
    [ExportViewToRegion(CurrentUserName.CurrentUserView, ViewContainer.CentralMainContainer)]
    public partial class SetupStationUserInfoView : Page
    {
        public SetupStationUserInfoView()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

    }
}
