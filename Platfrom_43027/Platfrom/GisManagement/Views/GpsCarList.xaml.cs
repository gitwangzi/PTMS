/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 4185b1e4-059d-480a-b2cd-cceb266fecb4      
/////             clrversion: 4.0.30319.17929
/////Registered organization: 
/////           Machine Name: PC-LILF
/////                 Author: TEST(lilf)
/////======================================================================
/////           Project Name: GisManagement.Views
/////    Project Description:    
/////             Class Name: GpsCarList
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/9/5 16:37:49
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/9/5 16:37:49
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

namespace GisManagement.Views
{
    public partial class GpsCarList : UserControl
    {
        public GpsCarList()
        {
            InitializeComponent();
            Gsafety.PTMS.Share.ApplicationContext.Instance.StringResourceReader.TranslateDataGrid(RequestGpsDataGrid);
            Gsafety.PTMS.Share.ApplicationContext.Instance.StringResourceReader.TranslateDataGrid(OneKeyAlarmDataGrid);//
            Gsafety.PTMS.Share.ApplicationContext.Instance.StringResourceReader.TranslateDataGrid(DisposedOneKeyAlarmDataGrid);
        }
    }
}
