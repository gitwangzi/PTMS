/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 8428cb66-e378-4a39-afb8-d077bc7d4276      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-WANGZS
/////                 Author: TEST(wangzs)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Base.Contract.Data
/////    Project Description:    
/////             Class Name: MultiMessage
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/8/7 17:07:52
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/8/7 17:07:52
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Gsafety.PTMS.Base.Contract.Data
{
    [DataContract]
    public class MultiMessage<T>
    {
        public MultiMessage()
        {
            Result = new List<T>();
        }

        public MultiMessage(IList<T> result, int totalCount)
        {
            IsSuccess = true;
            Result = result;
            TotalRecord = totalCount;
        }

        public MultiMessage(bool isSuccess, string errMessage)
        {
            IsSuccess = isSuccess;
            ErrorMsg = errMessage;
        }

        public MultiMessage(bool isSuccess, Exception ex)
        {
            IsSuccess = isSuccess;
            ErrorMsg = ex.Message;
            ErrorDetailMsg = ex.ToString();
            //ExceptionMessage = ex;
        }
        /// <summary>
        /// Single item
        /// </summary>
        [DataMember]
        public IList<T> Result { get; set; }


        /// <summary>
        /// exception message 
        /// </summary>
        [DataMember]
        public Exception ExceptionMessage { get; set; }

        /// <summary>
        /// total record
        /// </summary>
        [DataMember]
        public int TotalRecord { get; set; }

        /// <summary>
        /// Is Success
        /// </summary>
        [DataMember]
        public bool IsSuccess { get; set; }

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
