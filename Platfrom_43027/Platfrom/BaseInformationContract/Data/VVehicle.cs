using Gsafety.PTMS.Common.Data;
using Gsafety.PTMS.Common.Enum;
/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: b5875ce6-b8e0-47b0-9556-fefc4c223c36      
/////             clrversion: 4.0.30319.17929
/////Registered organization: 
/////           Machine Name: PC-LANQ
/////                 Author: TEST(lanq)
/////======================================================================
/////           Project Name: Gsafety.PTMS.BaseInformation.Contract.Data
/////    Project Description:    
/////             Class Name: Vehicle
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/8/7 16:09:26
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/8/22 16:09:26
/////            Modified by: BilongLiu
/////   Modified Description: 
/////======================================================================
using System;
using System.Runtime.Serialization;
using System.Text;

namespace Gsafety.PTMS.BaseInformation.Contract.Data
{
    ///<summary>
    ///车辆
    ///</summary>
    [DataContract]
    [KnownType(typeof(Vehicle))]
    public class VVehicle
    {
        [DataMember]
        public Vehicle Vehicles { get; set; }

        private bool isChecked = false;
        [DataMember]
        public bool IsChecked
        {
            get { return isChecked; }
            set
            {
                isChecked = value;
            }
        }

    }
}
