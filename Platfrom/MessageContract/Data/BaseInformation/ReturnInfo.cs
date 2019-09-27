/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: a7157bf7-c0ae-485b-8bab-c9bd45f567e4      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-LIW
/////                 Author: TEST(liw)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Message.Contract.Data.BaseInformation
/////    Project Description:    
/////             Class Name: ReturnInfo
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/10/12 10:05:16
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/10/12 10:05:16
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gsafety.PTMS.Message.Contract.Data
{
    /// <summary>
    /// Return Info
    /// </summary>
    public class ReturnInfo
    {
        /// <summary>
        /// byte Array
        /// </summary>
        public byte[] Message { get; set; }

        /// <summary>
        /// byte Array
        /// </summary>
        public List<Tuple<string, byte[]>> ListMessage { get; set; }

        /// <summary>
        /// RuleKey
        /// </summary>
        public string RuleKey { get; set; }

        /// <summary>
        /// Extended Field
        /// </summary>
        public int ExtendedField { get; set; }
    }
}
