/////Copyright (C) Gsafety 2014 .All Rights Reserved.
/////======================================================================
/////                   Guid: 332ad090-1f0f-45b9-a73a-aca481a58dfa      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-ZHOUDD
/////                 Author: GJSY(zhoudd)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Message.Contract.Data.CommandManage
/////    Project Description:    
/////             Class Name: SelectInfoModel
/////          Class Version: v1.0.0.0
/////            Create Time: 2014/10/28 14:35:37
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2014/10/28 14:35:37
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Gsafety.PTMS.Base.Contract.Data;

namespace Gsafety.PTMS.Base.Contract.Data
{
    [Serializable]
    [DataContract]
    public class  SelectInfoModel
    {
        /// <summary>
        /// the list value type
        /// </summary>
        [DataMember]
        public SettingType Type;

        [DataMember]
        public string Code;
        /// <summary>
        /// Vehicle Type
        /// Texi=1, Bus = 2,Flota = 3
        /// </summary>
        [DataMember]
        public List<int> VehicleType;

        [DataMember]
        public string GroupID;

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            if (!string.IsNullOrEmpty(Convert.ToString(Type)))
            {
                builder.AppendLine("Type:" + Type.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Code)))
            {
                builder.AppendLine("Code:" + Code.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(VehicleType)))
            {
                builder.AppendLine("VehicleType:" + VehicleType.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(GroupID)))
            {
                builder.AppendLine("GroupID:" + GroupID.ToString());
            }
            return builder.ToString();
        }
    }
}
