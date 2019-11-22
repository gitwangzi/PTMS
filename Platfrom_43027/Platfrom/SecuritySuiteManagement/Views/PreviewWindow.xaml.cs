/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: e6b75c99-cec3-4dcd-a576-a5e3747f7c2a      
/////             clrversion: 4.0.30319.17929
/////Registered organization: 
/////           Machine Name: PC-LANQ
/////                 Author: TEST(lanq)
/////======================================================================
/////           Project Name: Gsafety.PTMS.SecuritySuite.Views
/////    Project Description:    
/////             Class Name: PreviewWindow
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/11/26 17:27:55
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/11/26 17:27:55
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
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Gsafety.PTMS.SecuritySuite.Views
{
    public partial class PreviewWindow : ChildWindow
    {
        public PreviewWindow()
        {
            InitializeComponent();
            this.MouseRightButtonDown += ChildWindow_MouseRightButtonDown;
        }
        private void ChildWindow_MouseRightButtonDown(object sender,

System.Windows.Input.MouseButtonEventArgs e)
        {
            e.Handled = true;
        }
        public void ShowPreview(DataGrid grid)
        {
            WriteableBitmap image = new WriteableBitmap(grid, null);
            previewImage.Source = image;
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}

