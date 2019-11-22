/////Copyright (C) Gsafety 2014 .All Rights Reserved.
/////======================================================================
/////                   Guid: 296b7b7d-6b33-4e99-bd31-77169a46e988      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: LIN-20130409ZRS
/////                 Author: TEST(zhujf)
/////======================================================================
/////           Project Name: Gsafety.PTMS.CurrentUserManagement.Views.ViewPage
/////    Project Description:    
/////             Class Name: SysMaintainUserInfoView
/////          Class Version: v1.0.0.0
/////            Create Time: 2014/1/7 11:13:46
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2014/1/7 11:13:46
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
    [ExportAsView(CurrentUserName.SysMaintainUserInfoView)]
    [ExportViewToRegion(CurrentUserName.CurrentUserView, ViewContainer.CentralMainContainer)]
    public partial class SysMaintainUserInfoView : Page
    {
        public SysMaintainUserInfoView()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

    }
}
