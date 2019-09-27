using Gsafety.PTMS.Constants;
using Gsafety.PTMS.Share;
using Jounce.Core.View;
using Jounce.Regions.Core;
/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: ee19b690-4d37-4ebf-ba6c-1c072be48eff      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: LIN-20130409ZRS
/////                 Author: TEST(zhujf)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Installation.Views
/////    Project Description:    
/////             Class Name: ScrappedRegistration
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/8/30 17:06:25
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/8/30 17:06:25
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
    [ExportAsView(InstallationName.ScrappedRegistrationV)]
    [ExportViewToRegion(InstallationName.ScrappedRegistrationV, ViewContainer.InstallContainer)]
    public partial class ScrappedRegistration : UserControl
    {
        public ScrappedRegistration()
        {
            InitializeComponent();
        }
    }
}
