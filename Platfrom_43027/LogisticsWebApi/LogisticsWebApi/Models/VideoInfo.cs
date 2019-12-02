using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LogisticsWebApi.Models
{
    public class VideoInfo
    {
        /// <summary>
        ///  文件唯一编号URL
        /// </summary>
    
        public string FileId { get; set; }
        /// <summary>
        /// 文件大小
        /// </summary>
       
        public int FileSize { get; set; }
        /// <summary>
        /// 文件对应视频开始时间
        /// </summary>
     
        public DateTime StartTime { get; set; }
        /// <summary>
        /// 文件对应视频结束时间
        /// </summary>
       
        public DateTime EndTime { get; set; }

        /// <summary>
        /// 通道
        /// </summary>
     
        public int Channel { get; set; }

        public string VideoUrl  { get; set; }   
    
     
    }
}