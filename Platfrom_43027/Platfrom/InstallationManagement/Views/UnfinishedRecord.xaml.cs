/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 167bbf4a-ef46-42c3-a478-71228d7810c3      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-DENGZL
/////                 Author: TEST(dengzl)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Installation.Views
/////    Project Description:    
/////             Class Name: UnfinishedRecord
/////          Class Version: v1.0.0.0
/////            Create Time: 8/14/2013 3:17:29 PM
/////      Class Description:  
/////======================================================================
/////          Modified Time: 8/14/2013 3:17:29 PM
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
using Gsafety.PTMS.Share;
using Gsafety.PTMS.Constants;
using Gsafety.Common.Controls;

namespace Gsafety.PTMS.Installation.Views
{
    [ExportAsView(InstallationName.UnfinishedRecordV, Category = InstallationName.CategoryName,
      ToolTip = "Click to view some text.", Url = "/UnfinishedRecord")]
    [ExportViewToRegion(InstallationName.UnfinishedRecordV, ViewContainer.InstallContainer)]
    public partial class UnfinishedRecord : UserControl
    {
        public UnfinishedRecord()
        {
            InitializeComponent();
            Gsafety.PTMS.Share.ApplicationContext.Instance.StringResourceReader.TranslateDataGrid(SuiteDataGrid);
            this.MouseRightButtonDown += ChildWindow_MouseRightButtonDown;

            comboStatus.DropDownOpened += PopupHandler.OnDropDown;
            comboStatus.DropDownClosed += PopupHandler.OnDropDown;
        }
        private void ChildWindow_MouseRightButtonDown(object sender,

System.Windows.Input.MouseButtonEventArgs e)
        {
            e.Handled = true;
        }


        private void LayoutRoot_MouseRightButtonUp_1(object sender,

System.Windows.Input.MouseButtonEventArgs e)
        {
            e.Handled = true;
        }
    }
}
