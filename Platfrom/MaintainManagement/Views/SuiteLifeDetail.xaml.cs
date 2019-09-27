using Gsafety.PTMS.Constants;
using Gsafety.PTMS.SecuritySuite.Views;
using Gsafety.PTMS.Share;
using Jounce.Core.View;
using Jounce.Regions.Core;
/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: e033ddf7-85ed-4c78-a59e-b0283e0de081      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-PENGGL
/////                 Author: TEST(penggl)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Maintain.Views
/////    Project Description:    
/////             Class Name: ServiceLifeDetail
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/11/21 12:49:11
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/11/21 12:49:11
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
using System.Windows.Printing;
using System.Windows.Shapes;

namespace Gsafety.PTMS.Maintain.Views
{
    [ExportAsView(MaintainName.SuiteLifeDetailV)]
    [ExportViewToRegion(MaintainName.SuiteLifeDetailV, ViewContainer.MaintainContainer)]
    public partial class SuiteLifeDetail : UserControl
    {
        public SuiteLifeDetail()
        {
            InitializeComponent();
            ApplicationContext.Instance.StringResourceReader.TranslateDataGrid(SuiteLifeDetailGrid1);
        }

        private void btnPrint_Click(object sender, RoutedEventArgs e)
        {
            PreviewWindow pw = new PreviewWindow();
            pw.ShowPreview(SuiteLifeDetailGrid1);
            pw.HasCloseButton = false;

            pw.Closed += (t, a) =>
            {
                if (pw.DialogResult.Value)
                {
                    PrintDocument doc = new PrintDocument();
                    doc.PrintPage += (s, args) =>
                    {
                        args.PageVisual = SuiteLifeDetailGrid1;
                        args.HasMorePages = false;
                    };
                    doc.EndPrint += (s, args) =>
                    {
                        MessageBox.Show("ok");
                        if (args.Error != null)
                            MessageBox.Show(args.Error.Message);
                    };
                    doc.Print(ApplicationContext.Instance.StringResourceReader.GetString("SUITE_ServiceLifeReport"));
                }
            };
            pw.Show();
        }
    }
}
