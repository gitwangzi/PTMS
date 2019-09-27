/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: f2013141-dc5a-4635-9cb3-e94eadda179a      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-WANGZS
/////                 Author: TEST(wangzs)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Manager.Views
/////    Project Description:    
/////             Class Name: UserUpdatePasswordView
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/8/6 15:39:19
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/8/6 15:39:19
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
namespace Gsafety.PTMS.Manager.Views
{
    [ExportAsView(ManagerName.UserUpdatePasswordView)]
    [ExportViewToRegion(ManagerName.UserUpdatePasswordView, ManagerName.ManagerContainer)]
    public partial class UserUpdatePasswordView : UserControl
    {
        public UserUpdatePasswordView()
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
