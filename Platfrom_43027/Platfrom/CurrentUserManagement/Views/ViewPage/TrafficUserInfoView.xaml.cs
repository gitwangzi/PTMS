/////Copyright (C) Gsafety 2014 .All Rights Reserved.
/////======================================================================
/////                   Guid: 2ae870e0-c998-458d-bb22-5cc8bd2128f8      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: LIN-20130409ZRS
/////                 Author: TEST(zhujf)
/////======================================================================
/////           Project Name: Gsafety.PTMS.CurrentUserManagement.Views.ViewPage
/////    Project Description:    
/////             Class Name: TrafficUserInfoView
/////          Class Version: v1.0.0.0
/////            Create Time: 2014/1/7 11:06:15
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2014/1/7 11:06:15
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
    [ExportAsView(CurrentUserName.TrafficUserInfoView, Category = "Navigation", Url = "/TrafficUserInfoView")]
    [ExportViewToRegion(CurrentUserName.CurrentUserView, ViewContainer.CentralMainContainer)]
    public partial class TrafficUserInfoView : Page
    {
        public TrafficUserInfoView()
        {
            InitializeComponent();
            this.MouseRightButtonDown += ChildWindow_MouseRightButtonDown;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }


        private void ChildWindow_MouseRightButtonDown(object sender,

      System.Windows.Input.MouseButtonEventArgs e)
        {
            e.Handled = true;
        }




    }
}
