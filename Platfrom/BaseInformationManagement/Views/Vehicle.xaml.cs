/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 92240c56-9bfb-40fe-9c01-748532d85e10      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: LIN-20130409ZRS
/////                 Author: TEST(zhujf)
/////======================================================================
/////           Project Name: Gsafety.PTMS.BaseInformation.Views
/////    Project Description:    
/////             Class Name: Vehicle
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/8/13 16:10:36
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/8/13 16:10:36
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
namespace Gsafety.PTMS.BaseInformation.Views
{
    [ExportAsView(BaseInformationName.VehicleV, Category = BaseInformationName.CategoryName,
    MenuName = BaseInformationName.MenuName, MenuTitle = "BASEINFO_Vehicle",
    ToolTip = "Click to view some text.", Url = "/Vehicle", Order=1)]
    [ExportViewToRegion(BaseInformationName.VehicleV, BaseInformationName.BaseInfoContainer)]
    public partial class Vehicle : UserControl
    {
        public Vehicle()
        {
            InitializeComponent();
            Gsafety.PTMS.Share.ApplicationContext.Instance.StringResourceReader.TranslateDataGrid(VehicleDataGrid);        
        }

        //void Vehicle_Loaded(object sender, RoutedEventArgs e)
        //{
           
        //}
    }
}
