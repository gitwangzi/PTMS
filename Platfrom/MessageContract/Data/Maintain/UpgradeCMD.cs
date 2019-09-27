/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: d8f72080-e2b3-4629-843a-29128f88dd15      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-LIW
/////                 Author: TEST(liw)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Message.Contract.Data.Monitor
/////    Project Description:    
/////             Class Name: UpgradeCMD
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/8/28 14:36:49
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/8/28 14:36:49
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gsafety.PTMS.Base.Contract.Data;
using System.Runtime.Serialization;

namespace Gsafety.PTMS.Message.Contract.Data
{
    /// <summary>
    /// Upgrade CMD
    /// </summary>
    [DataContract]
    [Serializable]
    public class UpgradeCMD : DownwardBase
    {
        /// <summary>
        /// Suite Upgrade Record ID
        /// </summary>
        [DataMember]
        public string SuiteUpgradeRecordId { get; set; }

        /// <summary>
        /// Cmd Send Time
        /// </summary>
        [DataMember]
        public DateTime SendTime { get; set; }

        /// <summary>
        /// Get New UUID(Negligible)
        /// </summary>
        [DataMember]
        public string UUId { get; set; }

        /// <summary>
        /// Data Packet Count ID
        /// The Integer of ASCII .unit is byte, The Range is 0 to  4,294,967,295
        /// </summary>
        [DataMember]
        public int DataPacketCount { get; set; }

        /// <summary>
        ///MD5 Code
        ///</summary>
        [DataMember]
        public string MD5Code { get; set; }

        /// <summary>
        ///FTP Address
        ///ASCII Code,like "61.141.158.118". Only is IP
        ///</summary>
        [DataMember]
        public string FTPAddress { get; set; }

        /// <summary>
        ///Port
        ///</summary>
        [DataMember]
        public string Port { get; set; }

        /// <summary>
        ///UserName
        ///</summary>
        [DataMember]
        public string UserName { get; set; }

        /// <summary>
        ///Password
        ///</summary>
        [DataMember]
        public string Password { get; set; }

        /// <summary>
        ///File Name
        ///ASCII String,including Path,can not contain comma. The Max Count of Bytes is 256 
        ///</summary>
        [DataMember]
        public string FileName { get; set; }

        /// <summary>
        /// Operator
        /// </summary>
        [DataMember]
        public string Operator { get; set; }

        /// <summary>
        /// Version
        /// </summary>
        [DataMember]
        public string Version { get; set; }

        /// <summary>
        /// Context
        /// </summary>
        [DataMember]
        public string Context { get; set; }

        /// <summary>
        /// Convert to CMD
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder strCmd= new StringBuilder();

            strCmd.Append("99dcXXXX,")
                .Append(this.DvId)
                .Append(",")
                .Append(this.MsgId)
                .Append(",")
                .Append("C152")
                .Append(",")
                .Append(this.SendTime.ToString("yyMMdd HHmmss"))
                .Append(",")
                .Append(this.UUId)
                .Append(",")
                .Append(this.DataPacketCount)
                .Append(",")
                .Append(this.MD5Code)
                .Append(",")
                .Append(this.FTPAddress)
                .Append(",")
                .Append(this.Port)
                .Append(",")
                .Append(this.UserName)
                .Append(",")
                .Append(this.Password)
                .Append(",")
                .Append(this.FileName)
                .Append("#");
            ////Replace Length,Reduce 8 bytes(99dcxxxx)
            strCmd.Replace("XXXX", (strCmd.Length - 8).ToString("D5").Substring(1));
            this.Context = strCmd.ToString();
            return strCmd.ToString();
        } 
    }
}
