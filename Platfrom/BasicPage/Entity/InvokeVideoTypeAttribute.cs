/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: b3d08173-b444-42d5-a868-ab990bbe151c      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: WANGJINCAI
/////                 Author: TEST(wangjc)
/////======================================================================
/////           Project Name: Gsafety.PTMS.BasicPage
/////    Project Description:    
/////             Class Name: VideoArgs
/////          Class Version: v1.0.0.0
/////            Create Time: 2013-09-04 14:13:32
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013-09-04 14:13:32
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System;
using System.Net;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
namespace Gsafety.PTMS.Video.Args
{
    [AttributeUsage(AttributeTargets.Class)]
    public class InvokeVideoTypeAttribute:Attribute 
    {
        public VideoType VideoType { get; set; }

        public InvokeVideoTypeAttribute(VideoType vt):base()
        {
            this.VideoType = vt; 
        }
    }

    /// <summary>
    /// VideoType
    /// </summary>
    public enum VideoType
    {
        /// <summary>
        ///  AlarmVideo
        /// </summary>
        AlarmVideo,
        /// <summary>
        /// AlarmVideo15
        /// </summary>
        AlarmVideo15,
        /// <summary>
        ///AtonceVideo
        /// </summary>
        AtonceVideo,
        /// <summary>
        /// FileListVideo
        /// </summary>
        FileListVideo,
    }
}
