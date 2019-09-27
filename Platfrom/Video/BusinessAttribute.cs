/////Copyright (C) Gsafety 2014 .All Rights Reserved.
/////======================================================================
/////                   Guid: 29cd0190-cc51-4a44-8a91-3943134aaf99      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-ZHANGY
/////                 Author: GJSY(zhangy)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Video
/////    Project Description:    
/////             Class Name: BusinessAttribute
/////          Class Version: v1.0.0.0
/////            Create Time: 2014/12/15 10:44:41
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2014/12/15 10:44:41
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gsafety.PTMS.Video
{
    [AttributeUsage(AttributeTargets.Method)]
    public class BusinessAttribute : Attribute
    {
        public string TypeName { get; private set; }

        public BusinessAttribute(string typeName)
        {
            this.TypeName = typeName;
        }
    }
}
