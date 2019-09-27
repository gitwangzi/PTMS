/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 7353e9de-bd78-4064-ad01-94347b71e516      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-LIW
/////                 Author: TEST(liw)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Message.Contract.Data.UserMessage
/////    Project Description:    
/////             Class Name: DeleteInstall
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/9/23 14:39:24
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/9/23 14:39:24
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Gsafety.PTMS.Message.Contract.Data
{
    /// <summary>
    /// The Install Of Delete  Procedure
    /// </summary>
    [Serializable]
    [DataContract]
    public class DeleteInstall
    {
        /// <summary>
        /// MDVR Core ID
        /// </summary>
        [DataMember]
        public string MdvrCoreId { get; set; }

        /// <summary>
        /// Delete Install Time
        /// </summary>
        [DataMember]
        public DateTime DeleteInstallTime { get; set; }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            if (!string.IsNullOrEmpty(Convert.ToString(MdvrCoreId)))
            {
                builder.AppendLine("MdvrCoreId:" + MdvrCoreId.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(DeleteInstallTime)))
            {
                builder.AppendLine("DeleteInstallTime:" + DeleteInstallTime.ToString());
            }
            return builder.ToString();
        }

    }
}
