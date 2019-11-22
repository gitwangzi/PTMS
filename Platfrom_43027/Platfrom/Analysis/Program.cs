/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: d6742f65-a254-41b8-b76b-afb4f1c2d2f6      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-LIW
/////                 Author: TEST(liw)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Analysis
/////    Project Description:    
/////             Class Name: Program
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/7/31 14:57:37
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/7/31 14:57:37
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace Gsafety.PTMS.Analysis
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            //System.Diagnostics.Debugger.Launch();
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[] 
            { 
                new BusinessService()
            };
            ServiceBase.Run(ServicesToRun);
        }
    }
}
