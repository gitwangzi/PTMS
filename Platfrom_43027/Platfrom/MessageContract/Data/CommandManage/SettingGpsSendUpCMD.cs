using Gsafety.PTMS.Base.Contract.Data;
using Gsafety.PTMS.Common.Enum;
/////Copyright (C) Gsafety 2014 .All Rights Reserved.
/////======================================================================
/////                   Guid: e242fb2d-953c-46ec-a8bd-d1825ee4265a      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-ZHOUDD
/////                 Author: GJSY(zhoudd)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Message.Contract.Data
/////    Project Description:    
/////             Class Name: SettingGpsSendUpCMD
/////          Class Version: v1.0.0.0
/////            Create Time: 2014/10/23 11:39:28
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2014/10/23 11:39:28
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
    /// GPS Upload Setting Cmd
    /// </summary>
    [Serializable]
    [DataContract]
    public class SettingGpsSendUpCMD : DownwardBase
    {
        /// <summary>
        /// Send Type(0:Distance 1: Time 2 DistanceAndTime)
        /// </summary>
        [DataMember]
        public GpsSendType SendType;
        /// <summary>
        /// Start and Stop Marker 
        /// 0:Disable
        /// 1:Using
        /// </summary>
        [DataMember]
        public int IsUsing;
        /// <summary>
        /// Upload times
        /// 0-65535
        /// </summary>
        [DataMember]
        public int? SendNum;
        /// <summary>
        /// Distance Value
        /// 5-65535
        /// </summary>
        [DataMember]
        public int? DistanceValue;
        /// <summary>
        /// Time Value
        /// 0-65535
        /// </summary>
        [DataMember]
        public int? TimeValue;
        /// <summary>
        /// Setting time
        /// </summary>
        [DataMember]
        public DateTime SendTime;

        //public override string ToString()
        //{
        //    StringBuilder strCmd = new StringBuilder();
        //    strCmd.Append("99dcXXXX,")
        //        .Append(this.DvId)
        //        .Append(",")
        //        .Append(this.MsgId)
        //        .Append(",")
        //        .Append(CmType)
        //        .Append(",")
        //        .Append(this.SendTime.ToString("yyMMdd HHmmss"))
        //        .Append(",")
        //        .Append(IsUsing)
        //        .Append(",")
        //        .Append(GetValue(DistanceValue))
        //        .Append(",")
        //        .Append(GetValue(TimeValue))
        //        .Append(",")
        //        .Append(IsUsing.Equals(1) ? SendNum.ToString() : "")
        //        .Append(GetEndCommand())
        //        .Append("#");
        //    ////Replace length minus 8 bytes（99dcxxxx）
        //    strCmd.Replace("XXXX", (strCmd.Length - 8).ToString("D5").Substring(1));
        //    return strCmd.ToString();
        //}

        public string ToString(RuleOperationType OperationType)
        {
            StringBuilder strCmd = new StringBuilder();
            strCmd.Append("99dcXXXX,")
                .Append(this.DvId)
                .Append(",")
                .Append(string.Format("{0}:{1}", this.MsgId, (int)OperationType))
                .Append(",")
                .Append(CmType)
                .Append(",")
                .Append(this.SendTime.ToString("yyMMdd HHmmss"))
                .Append(",")
                .Append(IsUsing)
                .Append(",")
                .Append(GetValue(DistanceValue))
                .Append(",")
                .Append(GetValue(TimeValue))
                .Append(",")
                .Append(IsUsing.Equals(1) ? SendNum.ToString() : "")
                .Append(GetEndCommand())
                .Append("#");
            ////Replace length minus 8 bytes（99dcxxxx）
            strCmd.Replace("XXXX", (strCmd.Length - 8).ToString("D5").Substring(1));
            return strCmd.ToString();
        }

        private string GetValue(int? param)
        {
            if (IsUsing.Equals(0))
            {
                return "";
            }
            else if (param.Equals(null))
            {
                return "";
            }
            else 
            {
                return param.ToString();
            }
        }

        private string GetEndCommand()
        {
            if (DistanceValue != null && TimeValue != null)
            {
                return ",0003";
            }
            else if (DistanceValue != null)
            {
                return ",0001";
            }
            else if (TimeValue != null)
            {
                return ",0002";
            }
            else
            {
                return ",0000";
            }
        }
    }
}

