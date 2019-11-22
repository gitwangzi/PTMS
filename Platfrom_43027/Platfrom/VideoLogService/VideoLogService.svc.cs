////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: f14adf0c-3ff2-4812-a893-e88877886936      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: WANGJINCAI
/////                 Author: TEST(wangjc)
/////======================================================================
/////           Project Name: Gsafety.PTMS.VideoLog.Service
/////    Project Description:    
/////             Class Name: VideoLogService
/////          Class Version: v1.0.0.0
/////            Create Time: 2013-10-14 13:46:49
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013-10-14 13:46:49
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.Text;
using Gsafety.PTMS.VideoLog.Contract;
using Gsafety.PTMS.VideoLog.Repository;
using Gsafety.Common.Logging;
using Gsafety.PTMS.BaseInfo;
using Gsafety.PTMS.DBEntity;

namespace Gsafety.PTMS.VideoLog.Service
{
    [AspNetCompatibilityRequirements, ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall)]
    public class VideoLogService : BaseService, IVideoLogService
    {
        private VideoLogRepository _helper;

        public VideoLogService()
        {
            _helper = new VideoLogRepository();
        }

        public int AddVideoLog(PTMSEntities context, AddVideoLogArgs args)
        {
            try
            {
                Info("AddVideoLog");
                Info("args:" + Convert.ToString(args));
                return _helper.AddVideoLog(context, args);
            }
            catch (Exception exp)
            {
                Error(exp);
                return 2;
            }
        }

        public VerifyUserResult VerifyUser(VerifyUserArgs args)
        {
            VerifyUserResult result = new VerifyUserResult { result = "0" };
            try
            {
                Info("VerifyUser");
                Info("args:" + Convert.ToString(args));
                result = _helper.VerifyUser(args);
            }
            catch (Exception exp)
            {
                Error(exp);
            }
            _helper.FixVerifyUserResult(ref result, args.md5_str);
            return result;
        }
    }
}
