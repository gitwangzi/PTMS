/////Copyright (C)2014Microsoft All Rights Reserved.
/////======================================================================
/////                   Guid: 62b68f3c-e9b2-4ffa-8507-2eb84371bd17      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-JIANGJ
/////                 Author: TEST(JiangJ)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Bases.Models
/////    Project Description:    
/////             Class Name: UserDefineGroupVehicleDataRefreshMessage
/////          Class Version: v1.0.0.0
/////            Create Time: 2014/11/20 14:06:56
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2014/11/20 14:06:56
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

namespace Gsafety.PTMS.Bases.Models
{
    public class UserDefineGroupVehicleDataRefreshMessage
    {
        public string oldGroup;
        public Vehicle vehicle{get;set;}
        public UserDefineGroupOperator doOperator { get; set; }
    }

    public enum UserDefineGroupOperator
    { 
        Add,
        Delete,
        Move
    }
}
