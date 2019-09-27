/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 7ee11616-9518-4c99-af4e-8eb8b9bb77c2      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-PENGGL
/////                 Author: TEST(penggl)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Manager.Views.UserManageView
/////    Project Description:    
/////             Class Name: UserAuthorityView
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/10/11 14:09:15
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/10/11 14:09:15
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
    [ExportAsView(ManagerName.UserAuthorityView, Category = ManagerName.CategoryName,
    MenuName = ManagerName.UserMangeMenuName, MenuTitle = "MANAGER_User_Authority",
    ToolTip = "Click to view some text.", Url = "/UserAuthorityView", Order = 5)]
    [ExportViewToRegion(ManagerName.UserAuthorityView, ManagerName.ManagerContainer)]
    public partial class UserAuthorityView : UserControl
    {
        public UserAuthorityView()
        {
            InitializeComponent();
            Gsafety.PTMS.Share.ApplicationContext.Instance.StringResourceReader.TranslateDataGrid(MoniterRegionDataGrid);
        }
    }
}
