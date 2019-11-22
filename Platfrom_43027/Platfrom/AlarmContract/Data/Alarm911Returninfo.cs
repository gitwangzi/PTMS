/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: f3a28bce-7740-4693-b848-c1401ffec2a1      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-WANGZS
/////                 Author: TEST(wangzs)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Alarm.Contract.Data
/////    Project Description:    
/////             Class Name: Alarm911Returninfo
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/12/19 10:27:25
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/12/19 10:27:25
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Gsafety.PTMS.Alarm.Contract.Data
{
    [DataContract]
    public class  Returninfo
    {
        /// <summary>
        /// Alarm911 returninto：
        ///     unhandle-1
        ///     handleing-2
        ///     forwardalarming-3
        ///     aleadyforward-4
        ///     handlecomplete-5
        /// </summary>
        [DataMember]
        public int Incidentstatus { get; set; }

        [DataMember]
        public string IncidentAddress { get; set; }
        /// <summary>
        /// content IncidentTypeId ID:IncidentTypeName
        /// </summary>
        [DataMember]
        public string Content { get; set; }

        /// <summary>
        /// serach successful：1； serach faile：0
        /// </summary>
        [DataMember]
        public int status { get; set; }

        /// <summary>
        /// errormsg，when Status = 1 this is ””, when Status=0 this is errormsg
        /// </summary>
        [DataMember]
        public string ErrorMsg { get; set; }

        public override string ToString()    
        {
            StringBuilder builder = new StringBuilder();
            if (!string.IsNullOrEmpty(Convert.ToString(Incidentstatus)))
            {
                builder.AppendLine("Incidentstatus:" + Incidentstatus.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Content)))
            {
                builder.AppendLine("Content:" + Content.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(status)))
            {
                builder.AppendLine("status:" + status.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(ErrorMsg)))
            {
                builder.AppendLine("ErrorMsg:" + ErrorMsg.ToString());
            }

            return builder.ToString();
        }
    }
}
