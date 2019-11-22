using Gsafety.PTMS.Constants;
using Gsafety.PTMS.Share;
using Jounce.Core.View;
using Jounce.Regions.Core;
/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 3383aa0e-d42c-4d22-9df2-a91be4d3de1a      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: LIN-20130409ZRS
/////                 Author: TEST(zhujf)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Installation.Views
/////    Project Description:    
/////             Class Name: SuiteMaintaining
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/8/30 10:48:10
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/8/30 10:48:10
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

namespace Gsafety.PTMS.Installation.Views
{
    [ExportAsView(InstallationName.SuiteMaintainingV, Category = InstallationName.CategoryName,
        ToolTip = "Click to view some text.", Url = "/SuiteMaintaining")]
    [ExportViewToRegion(InstallationName.SuiteMaintainingV, ViewContainer.InstallContainer)]
    public partial class SuiteMaintaining : UserControl
    {      
        public SuiteMaintaining()
        {
            InitializeComponent();
            Gsafety.PTMS.Share.ApplicationContext.Instance.StringResourceReader.TranslateDataGrid(MaintainningDataGrid);
        }
    }
}
