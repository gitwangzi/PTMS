/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: ec733aa5-70b1-44c2-a68b-8dc095924c3b      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-DENGZL
/////                 Author: TEST(dengzl)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Manager.Contract.Data
/////    Project Description:    
/////             Class Name: LoginLogInfo
/////          Class Version: v1.0.0.0
/////            Create Time: 7/31/2013 10:14:13 AM
/////      Class Description:  
/////======================================================================
/////          Modified Time: 8/27/2013 10:14:13 AM
/////            Modified by: BilongLiu
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
    /// <summary>
    /// Login log
    /// </summary>
    [DataContract]
    public class LoginLogInfo
    {
        /// <summary>
        /// UserName
        /// </summary>
        [DataMember]
        public string UserName { get; set; }
        /// <summary>
        /// UserRole
        /// </summary>
        [DataMember]
        public string UserRole { get; set; }
        /// <summary>
        /// Organization
        /// </summary>
        [DataMember]
        public string Organization { get; set; }
        /// <summary>
        /// LoginTime
        /// </summary>
        [DataMember]
        public DateTime? LoginTime { get; set; }
        /// <summary>
        /// LogoutTime
        /// </summary>
        [DataMember]
        public DateTime? LogoutTime { get; set; }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            if (!string.IsNullOrEmpty(Convert.ToString(UserName)))
            {
                builder.AppendLine("UserName:" + UserName.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(UserRole)))
            {
                builder.AppendLine("UserRole:" + UserRole.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Organization)))
            {
                builder.AppendLine("Organization:" + Organization.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(LoginTime)))
            {
                builder.AppendLine("LoginTime:" + LoginTime.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(LogoutTime)))
            {
                builder.AppendLine("LogoutTime:" + LogoutTime.ToString());
            }
            return builder.ToString();
        }

    }
}
