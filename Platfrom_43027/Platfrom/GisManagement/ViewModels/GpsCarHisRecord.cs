/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 712e1684-bba1-447d-88b0-38b7b377bb55      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-LINGL
/////                 Author: TEST(zhangzl)
/////======================================================================
/////           Project Name: GisManagement.ViewModels
/////    Project Description:    
/////             Class Name: GpsCarHisRecord
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/9/17 13:24:44
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/9/17 13:24:44
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace GisManagement.ViewModels
{


    public class GpsCarHisRecord
    {
        public string CarNo { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public Color LineColor { get; set; }
        public GpsCarHisDataViewModel.PlayState status { get; set; }
    }
}
