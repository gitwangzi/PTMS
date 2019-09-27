/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 2f7f02c7-5b5c-4b28-9506-a5dc2a72b6dd      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-PENGGL
/////                 Author: TEST(penggl)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Manager.Views.UserManageView
/////    Project Description:    
/////             Class Name: UserAuthorityManageView
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/10/11 14:09:47
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/10/11 14:09:47
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

namespace Gsafety.PTMS.Manager.Views.UserManageView
{
    [ExportAsView(ManagerName.UserAuthorityManageView)]
    [ExportViewToRegion(ManagerName.UserAuthorityManageView, ManagerName.ManagerContainer)]
    public partial class UserAuthorityManageView : UserControl
    {
        public UserAuthorityManageView()
        {
            InitializeComponent();
            Gsafety.PTMS.Share.ApplicationContext.Instance.StringResourceReader.TranslateDataGrid(MoniterRegionViewDataGrid);
            Gsafety.PTMS.Share.ApplicationContext.Instance.StringResourceReader.TranslateDataGrid(MoniterRegionEditDataGrid);
        }
    }
}
