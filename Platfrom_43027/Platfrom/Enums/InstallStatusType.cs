/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 7ccac012-74d3-4cab-8c52-878b8a124e79      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-PENGGL
/////                 Author: TEST(penggl)
/////======================================================================
/////           Project Name: Gsafety.PTMS.BaseInformation.Contract.Data
/////    Project Description:    
/////             Class Name: InstallStatusType
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/9/12 17:09:48
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/9/12 17:09:48
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gsafety.PTMS.Bases.Enums
{
    /// <summary>
    /// Vehicle and Device Status
    /// </summary>
    public enum InstallStatusType
    {
        /// <summary>
        /// UnInstall
        /// </summary>
        [EnumAttribute(ResourceName = "UnInstall")]
        UnInstall = 1,
        /// <summary>
        /// Installing
        /// </summary>
        [EnumAttribute(ResourceName = "Installing")]
        Installing = 2,
        /// <summary>
        /// Installed
        /// </summary>
        [EnumAttribute(ResourceName = "Installed")]
        Installed = 3,
    }
}
