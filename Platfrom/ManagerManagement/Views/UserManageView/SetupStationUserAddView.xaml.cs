/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 9937a7ce-88ee-4394-93bc-bf3fba5b86fa      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-WANGZS
/////                 Author: TEST(wangzs)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Manager.Views
/////    Project Description:    
/////             Class Name: SetupStationUserAddView
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/8/6 14:59:35
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/8/6 14:59:35
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
    [ExportAsView(ManagerName.SetupStationUserAddView)]
    [ExportViewToRegion(ManagerName.SetupStationUserAddView, ManagerName.ManagerContainer)]
    public partial class SetupStationUserAddView : UserControl
    {
        public SetupStationUserAddView()
        {
            InitializeComponent();
        }
    }
}
