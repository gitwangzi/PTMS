/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: a4cdfe27-add1-421a-bd76-2062914ba683      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-PENGGL
/////                 Author: TEST(penggl)
/////======================================================================
/////           Project Name: Gsafety.PTMS.BaseInformation.Contract.Data
/////    Project Description:    
/////             Class Name: UserAuthority
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/10/11 15:02:33
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/10/11 15:02:33
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
    /// User Authority
    /// </summary>
    [DataContract]
    public class UserAuthority
    {
        /// <summary>
        /// ID
        /// </summary>
        [DataMember]
        public string ID;
        /// <summary>
        /// UserName
        /// </summary>
        [DataMember]
        public string UserName;
        /// <summary>
        /// LoginName
        /// </summary>
        [DataMember]
        public string LoginName;
        /// <summary>
        /// UserType
        /// </summary>
        [DataMember]
        public UserAuthorityType UserType;
        /// <summary>
        /// RegionsCode
        /// </summary>
        [DataMember]
        public string RegionsCode;
        /// <summary>
        /// RegionsName
        /// </summary>
        [DataMember]
        public string RegionsName;
        /// <summary>
        /// Province
        /// </summary>
        [DataMember]
        public string Province;
        /// <summary>
        /// SecurityGroup
        /// </summary>
        [DataMember]
        public string SecurityGroup;

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            if (!string.IsNullOrEmpty(Convert.ToString(ID)))
            {
                builder.AppendLine("ID:" + ID.ToString());
            }

            if (!string.IsNullOrEmpty(Convert.ToString(UserName)))
            {
                builder.AppendLine("UserName:" + UserName.ToString());
            }

            if (!string.IsNullOrEmpty(Convert.ToString(LoginName)))
            {
                builder.AppendLine("LoginName:" + LoginName.ToString());
            }

            if (!string.IsNullOrEmpty(Convert.ToString(UserType)))
            {
                builder.AppendLine("UserType:" + UserType.ToString());
            }

            if (!string.IsNullOrEmpty(Convert.ToString(RegionsCode)))
            {
                builder.AppendLine("RegionsCode:" + RegionsCode.ToString());
            }

            if (!string.IsNullOrEmpty(Convert.ToString(RegionsName)))
            {
                builder.AppendLine("RegionsName:" + RegionsName.ToString());
            }

            if (!string.IsNullOrEmpty(Convert.ToString(Province)))
            {
                builder.AppendLine("Province:" + Province.ToString());
            }

            if (!string.IsNullOrEmpty(Convert.ToString(SecurityGroup)))
            {
                builder.AppendLine("SecurityGroup:" + SecurityGroup.ToString());
            }
           

            return builder.ToString();
        }
    }

}
