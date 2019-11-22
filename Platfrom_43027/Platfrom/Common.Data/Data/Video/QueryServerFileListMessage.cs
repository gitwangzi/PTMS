/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 6f934fbe-c283-4d78-b222-977a188299c3      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: WANGJINCAI
/////                 Author: TEST(wangjc)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Integration.Contract.VideoSrv
/////    Project Description:    
/////             Class Name: QueryServerFileListMessage
/////          Class Version: v1.0.0.0
/////            Create Time: 2013-09-02 10:58:39
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013-09-02 10:58:39
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Gsafety.PTMS.Common.Data
{
    [DataContract]
    [Serializable]
    public class QueryServerFileListMessage
    {
        /// <summary>
        ///  文件唯一编号URL（由捷诺产生，以后根据该编号进行文件下载）
        /// </summary>
        [DataMember]
        public string FileID { get; set; }
        /// <summary>
        /// 文件大小
        /// </summary>
        [DataMember]
        public decimal FileSize { get; set; }
        /// <summary>
        /// 文件对应视频开始时间
        /// </summary>
        [DataMember]
        public DateTime StartTime { get; set; }
        /// <summary>
        /// 文件对应视频结束时间
        /// </summary>
        [DataMember]
        public DateTime EndTime { get; set; }

        /// <summary>
        /// 通道
        /// </summary>
        [DataMember]
        public decimal Channel { get; set; }

        [DataMember]
        public string UUID { get; set; }

        [DataMember]
        public string VehicleSN { get; set; }

        [DataMember]
        public int VideoType { get; set; }

        [DataMember]
        public int DownloadStatus { get; set; }

        [DataMember]
        public string Note { get; set; }

        //[DataMember]
        //public int CameraInstallLocation
        //{
        //    get
        //    {
        //        return (int)Channel;
        //    }
        //    set
        //    {
        //        Channel = value;
        //    }
        //}

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            if (!string.IsNullOrEmpty(Convert.ToString(FileID)))
            {
                builder.AppendLine("File_Id:" + FileID.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(FileSize)))
            {
                builder.AppendLine("File_Size:" + FileSize.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(StartTime)))
            {
                builder.AppendLine("Start_Time:" + StartTime.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(EndTime)))
            {
                builder.AppendLine("End_Time:" + EndTime.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Channel)))
            {
                builder.AppendLine("Channel:" + Channel.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(UUID)))
            {
                builder.AppendLine("UUID:" + UUID.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(VehicleSN)))
            {
                builder.AppendLine("VehicleSN:" + VehicleSN.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(VideoType)))
            {
                builder.AppendLine("VideoType:" + VideoType.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(VideoType)))
            {
                builder.AppendLine("VideoType:" + VideoType.ToString());
            }

            return builder.ToString();
        }
    }
}
