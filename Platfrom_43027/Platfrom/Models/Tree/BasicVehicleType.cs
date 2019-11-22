/////Copyright (C)2014Microsoft All Rights Reserved.
/////======================================================================
/////                   Guid: 6c1f52c2-78c1-4039-abcc-b380b2937736      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-JIANGJ
/////                 Author: TEST(JiangJ)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Bases.Models
/////    Project Description:    
/////             Class Name: BasicVehicleType
/////          Class Version: v1.0.0.0
/////            Create Time: 2014/10/31 12:53:14
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2014/10/31 12:53:14
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
using System.Collections.ObjectModel;
using Gsafety.PTMS.Bases.Enums;
using Jounce.Core.Model;
using System.Linq;

namespace Gsafety.PTMS.Bases.Models
{
    public class BasicVehicleType 
    {
        public VehicleType VehicleType;
        public string Name;
        public ObservableCollection<BasicVehicle> Vehicles { get; set; }
        public BasicCity parent;
        public BasicVehicleType(BasicCity bc)
        {
            parent = bc;
            if (Vehicles == null)
            {
                Vehicles = new ObservableCollection<BasicVehicle>();
            }
        }
    }
}
