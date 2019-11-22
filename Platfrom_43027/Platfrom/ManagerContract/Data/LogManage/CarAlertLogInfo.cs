/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: addc15bf-e2d5-4855-9663-c7ac1be594cb      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-DENGZL
/////                 Author: TEST(dengzl)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Manager.Contract.Data
/////    Project Description:    
/////             Class Name: CarAlertLogInfo
/////          Class Version: v1.0.0.0
/////            Create Time: 7/31/2013 3:18:12 PM
/////      Class Description:  
/////======================================================================
/////          Modified Time: 7/31/2013 3:18:12 PM
/////            Modified by:
/////   Modified Description: 
/////======================================================================
/////          Modified Time: 10/10/2014 10:18:12 PM
/////            Modified by: zhoudd
/////   Modified Description: 
/////======================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace Gsafety.PTMS.Manager.Contract.Data
{
    [DataContract]
    public class CarAlertLogInfo
    {
        /// <summary>
        /// CarNumber
        /// </summary>
        [DataMember]
        public string CarNumber { get; set; }
        /// <summary>
        /// VihcleType
        /// </summary>
        [DataMember]
        public string VihcleType { get; set; }
        /// <summary>
        /// AlertTime
        /// </summary>
        [DataMember]
        public DateTime? AlertTime { get; set; }
        /// <summary>
        /// AlertType
        /// </summary>
        [DataMember]
        public int? AlertType { get; set; }
        /// <summary>
        /// DealTime
        /// </summary>
        [DataMember]
        public string ShowAlertType { get; set; }

        [DataMember]
        public DateTime? DealTime { get; set; }
        /// <summary>
        /// DealPerson
        /// </summary>
        [DataMember]
        public string DealPerson { get; set; }
        /// <summary>
        /// DealContent
        /// </summary>
        [DataMember]
        public string DealContent { get; set; }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            if (!string.IsNullOrEmpty(Convert.ToString(CarNumber)))
            {
                builder.AppendLine("CarNumber:" + CarNumber.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(VihcleType)))
            {
                builder.AppendLine("VihcleType:" + VihcleType.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(AlertTime)))
            {
                builder.AppendLine("AlertTime:" + AlertTime.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(AlertType)))
            {
                builder.AppendLine("AlertType:" + AlertType.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(DealTime)))
            {
                builder.AppendLine("DealTime:" + DealTime.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(DealPerson)))
            {
                builder.AppendLine("DealPerson:" + DealPerson.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(DealContent)))
            {
                builder.AppendLine("DealContent:" + DealContent.ToString());
            }
            return builder.ToString();
        }

    }
}
