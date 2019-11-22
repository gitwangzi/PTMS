/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 45cd2052-a584-4b7f-a2a6-48d51c12e5ee      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: LIN-20130409DMW
/////                 Author: Admin(zhuyh)
/////======================================================================
/////           Project Name: Gsafety.PTMS.BaseInformation.Repository
/////    Project Description:    
/////             Class Name: VehicleRepository
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/9/2 15:00:00
/////      Class Description:  
/////======================================================================
/////          Modified Time: 
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
namespace Gsafety.PTMS.BaseInformation.Contract.Data
{
    /// <summary>
    /// Monitor Group
    /// </summary>
    [DataContract]
    public class MonitorGroup
    {
        /// <summary>
        /// ID
        /// </summary>
        [DataMember]
        public string ID;
        /// <summary>
        /// GroupName
        /// </summary>
        [DataMember]
        public string GroupName;
        /// <summary>
        /// Note
        /// </summary>
        [DataMember]
        public string Note;
        /// <summary>
        /// CreateUser
        /// </summary>
        [DataMember]
        public string CreateUser;
        /// <summary>
        /// GroupIndex
        /// </summary>
        [DataMember]
        public Nullable<int> GroupIndex;

        [DataMember]
        public string ClientID;

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            if (!string.IsNullOrEmpty(Convert.ToString(ID)))
            {
                builder.AppendLine("ID:" + ID.ToString());
            }

            if (!string.IsNullOrEmpty(Convert.ToString(GroupName)))
            {
                builder.AppendLine("GroupName:" + GroupName.ToString());
            }

            if (!string.IsNullOrEmpty(Convert.ToString(Note)))
            {
                builder.AppendLine("Note:" + Note.ToString());
            }

            if (!string.IsNullOrEmpty(Convert.ToString(CreateUser)))
            {
                builder.AppendLine("CreateUser:" + CreateUser.ToString());
            }

            if (!string.IsNullOrEmpty(Convert.ToString(GroupIndex)))
            {
                builder.AppendLine("GroupIndex:" + GroupIndex.ToString());
            }

            return builder.ToString();
        }
    }
}
