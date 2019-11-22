/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: c2354ee9-7311-429c-9104-3d9a403a6472      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-WANGZS
/////                 Author: TEST(wangzs)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Manager.Views
/////    Project Description:    
/////             Class Name: SetupStationListView
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/8/6 14:48:37
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/8/6 14:48:37
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
namespace Gsafety.PTMS.Manager.Views
{
    [ExportAsView(ManagerName.SetupStationListView, Category = ManagerName.CategoryName,
         MenuName = ManagerName.UserMangeMenuName, MenuTitle = "MANAGER_User_InstallStation",
         ToolTip = "Click to view some text.", Url = "/SetupStationListView", Order = 4)]
    [ExportViewToRegion(ManagerName.SetupStationListView, ManagerName.ManagerContainer)]
    public partial class SetupStationListView : UserControl
    {
        public SetupStationListView()
        {
            InitializeComponent();
            Gsafety.PTMS.Share.ApplicationContext.Instance.StringResourceReader.TranslateDataGrid(UserListDataGrid);
        }
    }
}
