/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 46b7dd15-bfee-4931-b196-ba88fcd5daae      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-DENGZL
/////                 Author: TEST(dengzl)
/////======================================================================
/////           Project Name: Gsafety.PTMS.SecuritySuite.Views
/////    Project Description:    
/////             Class Name: OffLine
/////          Class Version: v1.0.0.0
/////            Create Time: 8/8/2013 5:37:26 PM
/////      Class Description:  
/////======================================================================
/////          Modified Time: 8/8/2013 5:37:26 PM
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
using Jounce.Core;
using Jounce.Core.View;
using Jounce.Regions.Core;
using Gsafety.PTMS.Share;
using Gsafety.PTMS.SecuritySuite;
using System.Text.RegularExpressions;

namespace Gsafety.PTMS.SecuritySuite.Views
{
    [ExportAsView(SecuritySuiteName.OffLineV, Category = SecuritySuiteName.CategoryName,
        MenuName = SecuritySuiteName.StatusMenuName, MenuTitle = "SUITE_OffLine",
        ToolTip = "Click to view some text.", Url = "/OffLine", Order=2)]
    [ExportViewToRegion(SecuritySuiteName.OffLineV, SecuritySuiteName.SuiteContainer)]
    public partial class OffLine : UserControl
    {
        public OffLine()
        {
            InitializeComponent();
            ApplicationContext.Instance.StringResourceReader.TranslateDataGrid(OfflineSuiteGrid);
            this.MouseRightButtonDown += ChildWindow_MouseRightButtonDown;
        }
        private string pattern = @"^[0-9]*$";
        private string param1 = string.Empty;
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            Match m = Regex.Match(this.textboxtime.Text, pattern);
            if (!m.Success)
            {
                this.textboxtime.Text = param1;
                this.textboxtime.SelectionStart = this.textboxtime.Text.Length;
            }
            else
            {
                param1 = this.textboxtime.Text;
            }
        }

        private void ChildWindow_MouseRightButtonDown(object sender,

      System.Windows.Input.MouseButtonEventArgs e)
        {
            e.Handled = true;
        }
    }
}
