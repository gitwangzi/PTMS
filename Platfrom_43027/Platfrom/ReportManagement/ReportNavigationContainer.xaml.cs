/////Copyright (C) Gsafety 2014 .All Rights Reserved.
/////======================================================================
/////                   Guid: ad9ef60c-4d2f-471b-bdf6-5aa24eb79aef      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-ZHOUDD
/////                 Author: GJSY(zhoudd)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Manager
/////    Project Description:    
/////             Class Name: ReportNavigationContainer
/////          Class Version: v1.0.0.0
/////            Create Time: 2014/7/24 11:18:20
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2014/7/24 11:18:20
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System.ComponentModel.Composition;
using System.Windows.Controls;

namespace Gsafety.PTMS.ReportManager
{
    [Export]
    public partial class ReportNavigationContainer : UserControl
    {
        public ReportNavigationContainer()
        {
            InitializeComponent();
        }
    }
}
