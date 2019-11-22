using Gsafety.PTMS.Constants;
using Jounce.Core.View;
using Jounce.Regions.Core;
/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 7fb490fb-01b9-471b-8b91-7cad7c395e2b      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: GAOZITIAN-PC
/////                 Author: TEST(gaozt)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Installation.Views
/////    Project Description:    
/////             Class Name: UploadPictureAndConfirmView
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/8/16 13:56:19
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/8/16 13:56:19
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System.Windows.Controls;

namespace Gsafety.PTMS.Installation.Views
{
    [ExportAsView(InstallationName.InstallConfirmV)]
    [ExportViewToRegion(InstallationName.InstallConfirmV, ViewContainer.InstallContainer)]
    public partial class InstallConfirmView : UserControl
    {
        public InstallConfirmView()
        {
            InitializeComponent();
            Gsafety.PTMS.Share.ApplicationContext.Instance.StringResourceReader.TranslateDataGrid(dgAlert);
        }
    }
}
