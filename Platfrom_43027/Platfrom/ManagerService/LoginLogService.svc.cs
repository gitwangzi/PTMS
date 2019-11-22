/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: d469a3b6-80b8-45aa-b128-f134fe8cdb8f      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: WANGJINCAI
/////                 Author: TEST(wangjc)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Manager.Service
/////    Project Description:    
/////             Class Name: LoginLogService
/////          Class Version: v1.0.0.0
/////            Create Time: 2013-10-16 14:21:39
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013-10-16 14:21:39
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Text;
using Gsafety.PTMS.Base.Contract.Data;
using Gsafety.PTMS.Manager.Contract;
using Gsafety.PTMS.Manager.Contract.Data;
using Gsafety.Common.Logging;
using Gsafety.PTMS.BaseInfo;

namespace Gsafety.PTMS.Manager.Service
{
    [
    ServiceContract
   , AspNetCompatibilityRequirements
  , ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall)

  ]
    public class LoginLogService : BaseService
    {

        [OperationContract]
        [WebGet(UriTemplate = "AddLogoutLog?userName={userName}&id={id}", ResponseFormat = WebMessageFormat.Json)]
        public void AddLogoutLog(string userName, string id)
        {
            try
            {
                Info("AddLogoutLog");
                Info("userName:" + Convert.ToString(userName) + ";" + "id:" + Convert.ToString(id));
                var srvImp = new PTMSLogManageService();
                srvImp.AddLogoutLog(userName, id);
                Info(string.Format("AddLogoutLog User:{0},Id:{1}", userName, id));
            }
            catch (Exception ex)
            {
                Error(ex);
            }

        }
    }
}
