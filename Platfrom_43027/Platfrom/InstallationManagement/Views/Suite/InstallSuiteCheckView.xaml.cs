using Gsafety.PTMS.Constants;
using Jounce.Core.View;
using Jounce.Regions.Core;
/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 70878efc-6b4c-47d0-afe0-3eac6909d7f7      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: GAOZITIAN-PC
/////                 Author: TEST(gaozt)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Installation.Views
/////    Project Description:    
/////             Class Name: InputDeviceInfoView
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/8/16 10:59:23
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/8/16 10:59:23
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System.Windows.Controls;

namespace Gsafety.PTMS.Installation.Views
{
    [ExportAsView(InstallationName.InstallSuiteCheckV)]
    [ExportViewToRegion(InstallationName.InstallSuiteCheckV, ViewContainer.InstallContainer)]
    public partial class InstallSuiteCheckView : UserControl
    {
        public InstallSuiteCheckView()
        {
            InitializeComponent();
            Gsafety.PTMS.Share.ApplicationContext.Instance.StringResourceReader.TranslateDataGrid(ListDataGrid);
        }
    }
}
