/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 60b1c343-0d68-4aec-857f-63df81864c2d      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: WANGJINCAI
/////                 Author: TEST(wangjc)
/////======================================================================
/////           Project Name: Gsafety.PTMS.CurrentUserManagement.Views
/////    Project Description:    
/////             Class Name: CurrentUserManagementMenu
/////          Class Version: v1.0.0.0
/////            Create Time: 2013-12-16 13:47:43
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013-12-16 13:47:43
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
using Gsafety.PTMS.CurrentUserManagement.ViewModels;

namespace Gsafety.PTMS.CurrentUserManagement.Views
{
    public partial class CurrentUserManagementMenu : UserControl
    {
        public CurrentUserManagementMenu()
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
