/////Copyright (C) Gsafety 2014 .All Rights Reserved.
/////======================================================================
/////                   Guid: cb7d0bbb-2b73-40f7-b038-40eb6b67888a      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: LIN-20130409ZRS
/////                 Author: TEST(zhujf)
/////======================================================================
/////           Project Name: Gsafety.PTMS.CurrentUserManagement.Views.ViewPage
/////    Project Description:    
/////             Class Name: CompanyUserInfoView
/////          Class Version: v1.0.0.0
/////            Create Time: 2014/1/7 11:16:13
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2014/1/7 11:16:13
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
    [ExportAsView(CurrentUserName.CompanyUserInfoView)]
    [ExportViewToRegion(CurrentUserName.CurrentUserView, ViewContainer.CentralMainContainer)]
    public partial class CompanyUserInfoView : Page
    {
        public CompanyUserInfoView()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            
        }

    }
}
