/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: d52b5eee-07d3-4d93-b8fc-a3566736ec59      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-LIW
/////                 Author: TEST(liw)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Message.Contract.Data.Traffic
/////    Project Description:    
/////             Class Name: RapterMileage
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/10/30 15:42:40
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/10/30 15:42:40
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Gsafety.Common.Util;

namespace Gsafety.PTMS.Message.Contract.Data
{
    [DataContract]
    [Serializable]
    public class RapterMileage
    {
        public RapterMileage() { }

        public RapterMileage(string str, int zone)
        {
            if (!string.IsNullOrEmpty(str))
            {
                string[] array = str.Split(',');

                this.AntGpsSn = array[0];                        ////GPSID
                if (array[1] != null)
                {
                    ////Time
                    var time = ConvertHelper.ConvertStrToDate(array[1], "ddMMyyHHmmss");
                    if (time != null)
                    {
                        ////Time Zone
                        this.UpdateTime = time.Value.AddHours(zone);
                    }
                }
                this.Mileage = int.Parse(array[2]);              ////Mileage
                this.ID = Guid.NewGuid().ToString();
            }
        }

        /// <summary>
        /// ID
        /// </summary>
        [DataMember]
        public string ID { get; set; }

        /// <summary>
        /// Vehicle ID
        /// </summary>
        [DataMember]
        public string VehicleId { get; set; }

        /// <summary>
        /// ANTGPSSN
        /// </summary>
        [DataMember]
        public string AntGpsSn { get; set; }

        /// <summary>
        /// Mileage
        /// </summary>
        [DataMember]
        public Nullable<int> Mileage { get; set; }

        /// <summary>
        /// UpdateTime
        /// </summary>
        [DataMember]
        public Nullable<System.DateTime> UpdateTime { get; set; }
    }
}
