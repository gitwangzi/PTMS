/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: b2860390-4858-49d5-9579-0a10ff3daf2a      
/////             clrversion: 4.0.30319.17929
/////Registered organization: 
/////           Machine Name: PC-LANQ
/////                 Author: TEST(lanq)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Share.Enum
/////    Project Description:    
/////             Class Name: InstallStepType
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/9/4 17:30:50
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/9/4 17:30:50
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

namespace Gsafety.PTMS.Bases.Enums
{
    public enum InstallStepType : int
    {
        /// <summary>
        /// VehicleChecking
        /// </summary>
        VehicleChecking = 1,
        /// <summary>
        /// SuiteChecking
        /// </summary>
        SuiteChecking = 2,
        /// <summary>
        /// MdvrVehicleIdChecking
        /// </summary>
        MdvrVehicleIdChecking = 3,
        /// <summary>
        /// Selfchecking
        /// </summary>
        Selfchecking = 4,
        /// <summary>
        /// FunctionChecking
        /// </summary>
        FunctionChecking = 5,
        /// <summary>
        /// InfoConfirm
        /// </summary>
        InfoConfirm = 6,
        /// <summary>
        /// Finish
        /// </summary>
        Finish = 7,
    }
}
