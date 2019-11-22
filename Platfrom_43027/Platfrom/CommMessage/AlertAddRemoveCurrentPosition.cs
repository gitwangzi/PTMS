/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 591a61d4-8de6-4113-8299-68c272852166      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-ZHANGZG
/////                 Author: TEST(zhangzg)
/////======================================================================
/////           Project Name: Gsafety.Common.CommMessage
/////    Project Description:    
/////             Class Name: AlertAddRemoveCurrentPosition
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/11/5 9:00:15
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/11/5 9:00:15
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
using Gsafety.PTMS.Bases.Enums;

namespace Gsafety.Common.CommMessage
{
    public class AlertAddRemoveCurrentPosition : Gsafety.PTMS.ServiceReference.MessageServiceExt.GPS
    {
        //1 draw，0 delete
        public int Op { get; set; }

        public string CityName { get; set; }

        public string ProvinceName { get; set; }

        public VehicleType VehicleType;

        public DateTime? AlertTime { get; set; }
    }
}
