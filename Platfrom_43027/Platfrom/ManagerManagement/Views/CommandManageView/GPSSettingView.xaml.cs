using Jounce.Core.View;
using Jounce.Regions.Core;
/////Copyright (C) Gsafety 2014 .All Rights Reserved.
/////======================================================================
/////                   Guid: 1ba6312f-e7f0-4ed9-9af1-a530f72246fc      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-ZHOUDD
/////                 Author: GJSY(zhoudd)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Manager.Views.CommandManageView
/////    Project Description:    
/////             Class Name: GPSSettingView
/////          Class Version: v1.0.0.0
/////            Create Time: 2014/10/22 11:19:36
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2014/10/22 11:19:36
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
using Gsafety.PTMS.Share;

namespace Gsafety.PTMS.Manager.Views.CommandManageView
{
    [ExportAsView(ManagerName.GpsSettingView, Category = ManagerName.CategoryName,
        MenuName = ManagerName.CommandManageMenuName, MenuTitle = "MANAGER_GpsSetting",
        ToolTip = "Click to view some text.", Url = "/GpsSettingView", Order = 2)]
    [ExportViewToRegion(ManagerName.GpsSettingView, ManagerName.ManagerContainer)]
    public partial class GpsSettingView : UserControl
    {
        public GpsSettingView()
        {
            InitializeComponent();
            Gsafety.PTMS.Share.ApplicationContext.Instance.StringResourceReader.TranslateDataGrid(GpsSettingDataGrid);
        }

    }
}
