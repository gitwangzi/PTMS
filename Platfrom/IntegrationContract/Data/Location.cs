/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 71e3bb3d-109e-4aa7-b96f-3f58bf8350bf      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-LIW
/////                 Author: TEST(liw)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Alarm.Contract.Data
/////    Project Description:    
/////             Class Name: GPS
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/8/8 11:12:00
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/8/8 11:12:00
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Gsafety.PTMS.Integration.Contract.Data
{
    /// <summary>
    /// Gps位置信息
    /// </summary>
    [DataContract]
    public class Location
    {
        /// <summary>
        /// 经度，(-180,180]
        /// </summary>
        [DataMember]
        public decimal LONGITUDE { get; set; }
        /// <summary>
        /// 纬度，[-90,90]，南极-90，赤道0，北极90
        /// </summary>
        [DataMember]
        public decimal LATITUDE { get; set; }
        /// <summary>
        /// 速度
        /// </summary>
        [DataMember]
        public decimal SPEED { get; set; }
        /// <summary>
        /// 方向，[0,360)，正北为0度，正东90度，正南180度，正西270度
        /// </summary>
        [DataMember]
        public decimal DIRECTION { get; set; }
        /// <summary>
        /// 时间，0时区
        /// </summary>
        [DataMember]       
        public DateTime GPS_TIME { get; set; }
        /// <summary>
        /// 车牌号
        /// </summary>
        [DataMember]
        public string CAR_NUMBER { get; set; }
    }
}
