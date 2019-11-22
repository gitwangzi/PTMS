/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 5c50b7d3-0336-4ff8-b933-444e0265da6a      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: WANGJINCAI
/////                 Author: TEST(wangjc)
/////======================================================================
/////           Project Name: Gsafety.Common.CommMessage
/////    Project Description:    
/////             Class Name: VideoDownLoadArgs
/////          Class Version: v1.0.0.0
/////            Create Time: 2013-09-25 09:51:20
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013-09-25 09:51:20
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Gsafety.Common.CommMessage
{
    /// <summary>
    /// query args
    /// </summary>
    public class VideoDownLoadArgs
    {
        public string CarNo { get; set; }

        /// <summary>
        /// MdvrCoreSn
        /// </summary>
        public string MdvrCoreSn { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        /// <summary>
        /// query type
        /// </summary>
        public QueryType ArgType { get; set; }

        public bool CheckSelf()
        {
            bool result = !string.IsNullOrWhiteSpace(CarNo);
            result = result && !string.IsNullOrWhiteSpace(MdvrCoreSn);
            result = result && StartTime > DateTime.MinValue && StartTime < DateTime.MaxValue;
            result = result && this.EndTime > DateTime.MinValue && this.EndTime < DateTime.MaxValue;
            result = result && this.EndTime > this.StartTime;

            return result;

        }
    }


    public enum QueryType
    {
        /// <summary>
        ///query had already exist vedio file list
        /// </summary>
        QueryServerFileList=0,
        /// <summary>
        /// query download vedio file list
        /// </summary>
        QueryServerDownloadFileList=1,
        /// <summary>
        ///query MDVR file list
        /// </summary>
        QueryMdvrFileList=2,

        /// <summary>
        /// downloading vedio
        /// </summary>
        QueryDownloadingFielList=3,
    }
}
