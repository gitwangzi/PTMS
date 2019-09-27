/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 7d4ebc07-fb78-44d5-bfca-e2ba89a63748      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-LIW
/////                 Author: TEST(liw)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Analysis
/////    Project Description:    
/////             Class Name: BusinessAttribute
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/8/5 14:45:29
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/8/5 14:45:29
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gsafety.PTMS.AnalysisLib
{
    [AttributeUsage(AttributeTargets.Method)]
    public class BusinessAttribute : Attribute
    {
        /// <summary>
        /// Business Type
        /// </summary>
        public string TypeName { get; private set; }

        public BusinessAttribute(string typeName)
        {
            this.TypeName = typeName;
        }
    }
}
