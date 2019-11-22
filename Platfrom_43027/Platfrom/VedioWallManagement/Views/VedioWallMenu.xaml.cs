/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 4ed9b454-dc15-46ec-9f3f-232cef463df5      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-DENGZL
/////                 Author: TEST(dengzl)
/////======================================================================
/////           Project Name: Gsafety.PTMS.VedioWall.Views
/////    Project Description:    
/////             Class Name: VedioWallMenu
/////          Class Version: v1.0.0.0
/////            Create Time: 9/11/2013 4:41:15 PM
/////      Class Description:  
/////======================================================================
/////          Modified Time: 9/11/2013 4:41:15 PM
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
using Gsafety.Common.Utilities;
using Gsafety.PTMS.Bases.Librarys;
using Gsafety.PTMS.Share;
using Gsafety.PTMS.Bases.Models;
namespace Gsafety.PTMS.VedioWall.Views
{
    public partial class VedioWallMenu : UserControl
    {
        public VedioWallMenu()
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
