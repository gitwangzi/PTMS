/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 4caa7ab0-61f5-433d-92ec-1587fe405821      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-DENGZL
/////                 Author: TEST(dengzl)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Manager.Contract.Data
/////    Project Description:    
/////             Class Name: VideoLogInfo
/////          Class Version: v1.0.0.0
/////            Create Time: 7/31/2013 11:01:06 AM
/////      Class Description:  
/////======================================================================
/////          Modified Time: 7/31/2013 11:01:06 AM
/////            Modified by:
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
    /// VideoLogInfo
    /// </summary>
    [DataContract]
    public class VideoLogInfo
    {
        /// <summary>
        /// Player
        /// </summary>
        [DataMember]
        public string Player { get; set; }

        /// <summary>
        /// ActionTime
        /// </summary>
        [DataMember]
        public DateTime? ActionTime { get; set; }

        /// <summary>
        /// SubType
        /// </summary>
        [DataMember]
        public string SubType { get; set; }

        /// <summary>
        /// Ivoke_Type
        /// </summary>
        [DataMember]
        public string Ivoke_Type { get; set; }

        /// <summary>
        /// MDVRId
        /// </summary>
        [DataMember]
        public string MDVRId { get; set; }

        /// <summary>
        /// VehicleID
        /// </summary>
        [DataMember]
        public string VehicleID { get; set; }

        /// <summary>
        /// ChannelId
        /// </summary>
        [DataMember]
        public string ChannelId { get; set; }

        /// <summary>
        /// VideoStartTime
        /// </summary>
        [DataMember]
        public DateTime? VideoStartTime { get; set; }

        /// <summary>
        /// VideoEndtime
        /// </summary>
        [DataMember]
        public DateTime? VideoEndtime { get; set; }

        /// <summary>
        /// VideoFileName
        /// </summary>
        [DataMember]
        public string VideoFileName { get; set; }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            if (!string.IsNullOrEmpty(Convert.ToString(Player)))
            {
                builder.AppendLine("Player:" + Player.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(ActionTime)))
            {
                builder.AppendLine("ActionTime:" + ActionTime.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(SubType)))
            {
                builder.AppendLine("SubType:" + SubType.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Ivoke_Type)))
            {
                builder.AppendLine("Ivoke_Type:" + Ivoke_Type.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(MDVRId)))
            {
                builder.AppendLine("MDVRId:" + MDVRId.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(VehicleID)))
            {
                builder.AppendLine("VehicleID:" + VehicleID.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(ChannelId)))
            {
                builder.AppendLine("ChannelId:" + ChannelId.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(VideoStartTime)))
            {
                builder.AppendLine("VideoStartTime:" + VideoStartTime.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(VideoEndtime)))
            {
                builder.AppendLine("VideoEndtime:" + VideoEndtime.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(VideoFileName)))
            {
                builder.AppendLine("VideoFileName:" + VideoFileName.ToString());
            }
            return builder.ToString();
        }

    }
}
