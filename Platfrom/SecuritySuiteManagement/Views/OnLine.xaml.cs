/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 05125d90-35dd-414f-b649-23b3314c42c2      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-DENGZL
/////                 Author: TEST(dengzl)
/////======================================================================
/////           Project Name: Gsafety.PTMS.SecuritySuite.Views
/////    Project Description:    
/////             Class Name: OnLine
/////          Class Version: v1.0.0.0
/////            Create Time: 8/8/2013 5:37:14 PM
/////      Class Description:  
/////======================================================================
/////          Modified Time: 8/8/2013 5:37:14 PM
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
    [ExportAsView(SecuritySuiteName.OnLineV, Category = SecuritySuiteName.CategoryName,
        MenuName = SecuritySuiteName.StatusMenuName, MenuTitle = "SUITE_OnLine",
        ToolTip = "Click to view some text.", Url = "/OnLine", Order=1)]
    [ExportViewToRegion(SecuritySuiteName.OnLineV, SecuritySuiteName.SuiteContainer)]
    public partial class OnLine : UserControl
    {
        public OnLine()
        {
            InitializeComponent();
            ApplicationContext.Instance.StringResourceReader.TranslateDataGrid(OnlineSuiteGrid);
        }
        private string pattern = @"^[0-9]*$";
        private string param1 =string.Empty ;
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


    }
}
