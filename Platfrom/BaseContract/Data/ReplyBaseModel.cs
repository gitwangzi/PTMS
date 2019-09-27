/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 59f09202-dce7-4145-b374-b6a24b83552e      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-LIW
/////                 Author: TEST(liw)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Base.Contract.Data
/////    Project Description:    
/////             Class Name: ReplyBaseModel
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/8/30 10:48:47
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/8/30 10:48:47
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
    [DataContract]
    [Serializable]
    public class ReplyBaseModel
    {
        /// <summary>
        /// MdvrCore ID
        /// </summary>
        [DataMember]
        public string MdvrCoreId { get; set; }

        /// <summary>
        /// AssociationSet ID
        /// For Instance : Electronic Fence ID, OverSpeed Rule ID
        /// </summary>
        [DataMember]
        public string AssociationSetID { get; set; }

        /// <summary>
        /// Command Word V0
        /// </summary>
        [DataMember]
        public string Cmd { get; set; }

        /// <summary>
        /// Longitude
        /// </summary>
        [DataMember]
        public string Longitude { get; set; }

        /// <summary>
        /// Latitude
        /// </summary>
        [DataMember]
        public string Latitude { get; set; }

        /// <summary>
        /// Speed
        /// </summary>
        [DataMember]
        public string Speed { get; set; }

        /// <summary>
        /// Direction
        /// </summary>
        [DataMember]
        public string Direction { get; set; }

        /// <summary>
        /// GPS Time
        /// </summary>
        [DataMember]
        public Nullable<DateTime> GpsTime { get; set; }

        /// <summary>
        /// GPS Validity
        /// </summary>
        [DataMember]
        public string GpsValid { get; set; }

        /// <summary>
        /// Original Command Word
        /// </summary>
        [DataMember]
        public string OriginalCmd { get; set; }

        /// <summary>
        /// Original Time
        /// </summary>
        [DataMember]
        public DateTime OriginalTime { get; set; }

        /// <summary>
        /// 0: AutoReply
        /// 1: Manual Reply 
        /// </summary>
        [DataMember]
        public int ReplyType { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public string OperType { get; set; }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            if (!string.IsNullOrEmpty(Convert.ToString(MdvrCoreId)))
            {
                builder.AppendLine("MdvrCoreId:" + MdvrCoreId.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(AssociationSetID)))
            {
                builder.AppendLine("AssociationSetID:" + AssociationSetID.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Cmd)))
            {
                builder.AppendLine("Cmd:" + Cmd.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Longitude)))
            {
                builder.AppendLine("Longitude:" + Longitude.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Latitude)))
            {
                builder.AppendLine("Latitude:" + Latitude.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Speed)))
            {
                builder.AppendLine("Speed:" + Speed.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Direction)))
            {
                builder.AppendLine("Direction:" + Direction.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(GpsTime)))
            {
                builder.AppendLine("GpsTime:" + GpsTime.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(GpsValid)))
            {
                builder.AppendLine("GpsValid:" + GpsValid.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(OriginalCmd)))
            {
                builder.AppendLine("OriginalCmd:" + OriginalCmd.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(OriginalTime)))
            {
                builder.AppendLine("OriginalTime:" + OriginalTime.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(ReplyType)))
            {
                builder.AppendLine("ReplyType:" + ReplyType.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(OperType)))
            {
                builder.AppendLine("OperType:" + OperType.ToString());
            }
            return builder.ToString();
        }

    }
}
