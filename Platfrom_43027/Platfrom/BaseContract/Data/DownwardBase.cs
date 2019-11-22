/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 3e169fd5-51b1-43b1-9451-fba84110da7b      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-LIW
/////                 Author: TEST(liw)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Base.Entity
/////    Project Description:    
/////             Class Name: DownwardBaseModel
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/8/20 17:11:14
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/8/20 17:11:14
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Gsafety.PTMS.Base.Contract.Data
{
    /// <summary>
    /// Downward Oreder Basic Class
    /// </summary>
    [DataContract]
    [Serializable]
    public class DownwardBase
    {
        /// <summary>
        /// Command Type(CtrRpl)
        /// </summary>
        [DataMember]
        public virtual string CmType { get; set; }

        /// <summary>
        /// DV ID
        /// </summary>
        [DataMember]
        public virtual string DvId { get; set; }

        /// <summary>
        /// Message ID
        /// </summary>
        [DataMember]
        public virtual string MsgId { get; set; }

        /// <summary>
        /// RuleName
        /// </summary>
        [DataMember]
        public string RuleName { get; set; }

        /// <summary>
        /// SendUp ID
        /// </summary>
        [DataMember]
        public string UserName { get; set; }
        /// <summary>
        /// Type
        /// Remove One-Key Alarm:C70;
        /// Shut Down:C154;
        /// Security Suite Recovery:C150
        /// Get Security Suite Cellphone Number and Sim Card :C101
        /// Set Beating Interval:C3
        /// Get Beating Interval:C4
        /// Read the  Parameter of  Fixed Upload Location:C32
        /// Ask Current Location and State:C33
        /// Read the Parameter of Temperature Alarm:C65
        /// Read the Parameter of OverSpeed Alarm:C69
        /// </summary>
        //[DataMember]
        //public virtual string Cmd { get; set; }

        /// <summary>
        /// GPS Time
        /// </summary>
        [DataMember]
        public DateTime GpsTime { get; set; }
      
    }
}
