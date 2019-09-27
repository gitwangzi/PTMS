/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 81b73dea-5e13-4a49-9327-4b369cba70da      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-PENGGL
/////                 Author: TEST(penggl)
/////======================================================================
/////           Project Name: Gsafety.PTMS.BaseInformation.Contract.Data
/////    Project Description:    
/////             Class Name: VehicleConditionType
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/8/29 19:01:28
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/8/29 19:01:28
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gsafety.PTMS.Common.Data.Enum
{
    public enum VehicleConditionType
    {
        /// <summary>
        /// Conditions do not have to install
        /// </summary>
        Unavailable = 0,
        /// <summary>
        /// Has the installation condtitions (default)
        /// </summary>
        Available = 1,

    }
}
