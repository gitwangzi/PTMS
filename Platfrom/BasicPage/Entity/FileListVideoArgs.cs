/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: a7afecf2-63a8-4348-83dd-e173626c6cf0      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: WANGJINCAI
/////                 Author: TEST(wangjc)
/////======================================================================
/////           Project Name: Gsafety.PTMS.BasicPage
/////    Project Description:    
/////             Class Name: FileListVideoArgs
/////          Class Version: v1.0.0.0
/////            Create Time: 2013-09-04 14:27:54
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013-09-04 14:27:54
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
namespace Gsafety.PTMS.Video.Args
{

    [InvokeVideoType(Args.VideoType.FileListVideo)]
    public class FileListVideoArgs : ArgsBase
    {
        public string FileList { get; set; }
    }
}
