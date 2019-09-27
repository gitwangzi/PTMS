/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: a9c2fbdb-5d00-466a-9321-0be8458a802a      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: WANGJINCAI
/////                 Author: TEST(wangjc)
/////======================================================================
/////           Project Name: Gsafety.PTMS.CurrentUserManagement.Views.ViewPage
/////    Project Description:    
/////             Class Name: CurrentUserInfo
/////          Class Version: v1.0.0.0
/////            Create Time: 2013-12-16 14:04:43
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013-12-16 14:04:43
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

namespace Gsafety.PTMS.CurrentUserManagement.Views.ViewPage
{
    public partial class CurrentUserInfo : Page
    {
        public CurrentUserInfo()
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
