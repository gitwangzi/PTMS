/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 674d2ac0-d694-49fe-bba2-fe8a931fa2cf      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-DENGZL
/////                 Author: TEST(dengzl)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Base.Contract.Data
/////    Project Description:    
/////             Class Name: SingleMessage
/////          Class Version: v1.0.0.0
/////            Create Time: 8/9/2013 9:39:34 AM
/////      Class Description:  
/////======================================================================
/////          Modified Time: 8/9/2013 9:39:34 AM
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace Gsafety.PTMS.Base.Contract.Data
{
    [DataContract]
    public class SingleMessage<T>
    {
        public SingleMessage()
        {
            IsSuccess = true;
        }

        public SingleMessage(T result)
        {
            IsSuccess = true;
            Result = result;
        }

        public SingleMessage(bool isSuccess, string errMessage)
        {
            IsSuccess = isSuccess;
            ErrorMsg = errMessage;
        }

        public SingleMessage(bool isSuccess, Exception ex)
        {
            IsSuccess = isSuccess;
            ExceptionMessage = ex;
        }

        /// <summary>
        /// IsSuccess :0,fail;，1,success
        /// </summary>
        [DataMember]
        public bool IsSuccess { get; set; }
        /// <summary>
        /// Single item
        /// </summary>
        [DataMember]
        public T Result { get; set; }

        /// <summary>
        /// Exception Message 
        /// </summary>
        [DataMember]
        public Exception ExceptionMessage { get; set; }


        /// <summary>
        /// Error Message
        /// </summary>
        [DataMember]
        public string ErrorMsg { get; set; }


        /// <summary>
        /// Error Detail Message
        /// </summary>
        [DataMember]
        public string ErrorDetailMsg { get; set; }
    }
}
