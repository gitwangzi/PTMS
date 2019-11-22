/////Copyright (C) Gsafety 2014 .All Rights Reserved.
/////======================================================================
/////                   Guid: 1ef9483d-766c-4020-b379-42da43851542      
/////             clrversion: 4.0.30319.34011
/////Registered organization: 
/////           Machine Name: DENGZL
/////                 Author: DENGZL(dzl)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Base.Contract.Data
/////    Project Description:    
/////             Class Name: CommandSendStatus
/////          Class Version: v1.0.0.0
/////            Create Time: 2014/3/11 06:23:35
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2014/3/11 06:23:35
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gsafety.PTMS.Base.Contract.Data
{
    public enum CommandSendStatus
    {
        Failure = 0x00,
        Success = 0x01,
        Wait = 0x02,
        Sending = 0x03,
        None = 0xFF,

    }
}
