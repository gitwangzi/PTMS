/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 2c515c41-922d-412f-aa1b-65e5274c903b      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: WANGJINCAI
/////                 Author: TEST(wangjc)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Integration.Test
/////    Project Description:    
/////             Class Name: Program
/////          Class Version: v1.0.0.0
/////            Create Time: 2013-09-03 16:32:04
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013-09-03 16:32:04
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gsafety.PTMS.Integration.Test
{
   public  class Program
    {
       private static VedioTest _test = new VedioTest();
       
       static void Main(string[] args)
       {
           _test.CheckAlarmVideo_Test();

           Console.Read();
       }
    }
}
