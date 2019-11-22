/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 5ae51c19-14e9-4b44-be9a-ba083bea88f5      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-LINGL
/////                 Author: TEST(zhangzl)
/////======================================================================
/////           Project Name: GisManagement.Views
/////    Project Description:    
/////             Class Name: InputText
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/9/25 16:22:57
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/9/25 16:22:57
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

namespace GisManagement.Views
{
    public partial class InputText : ChildWindow
    {
        public InputText()
        {
            InitializeComponent();
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            GisName.addtextwords = this.addtextblock.Text.ToString(); 
            this.DialogResult = true;            
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}

