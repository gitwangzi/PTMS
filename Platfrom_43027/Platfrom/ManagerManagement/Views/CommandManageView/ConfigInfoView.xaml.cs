/////Copyright (C) Gsafety 2014 .All Rights Reserved.
/////======================================================================
/////                   Guid: 500f0e57-e50a-4804-8a0c-7dcb365f5f91      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-ZHANGH
/////                 Author: GJSY(zhangh)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Manager.Views.CommandManageView
/////    Project Description:    
/////             Class Name: ConfigInfoView
/////          Class Version: v1.0.0.0
/////            Create Time: 2014/11/20 13:42:45
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2014/11/20 13:42:45
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


namespace Gsafety.PTMS.Manager.Views.CommandManageView
{
    [ExportAsView(ManagerName.ConfigInfoView,Category=ManagerName.CategoryName,
       MenuName = ManagerName.CommandManageMenuName, MenuTitle = "MANAGER_ConfigInfo",
       ToolTip = "Click to view some text." ,Url="/ConfigInfoView",Order=0)]
    [ExportViewToRegion(ManagerName.ConfigInfoView, ManagerName.ManagerContainer)]
    public partial class ConfigInfoView : UserControl
    {
        public ConfigInfoView()
        {
            InitializeComponent();
            Gsafety.PTMS.Share.ApplicationContext.Instance.StringResourceReader.TranslateDataGrid(ConfigerationInfoDataGrid);
        }
    }
}
