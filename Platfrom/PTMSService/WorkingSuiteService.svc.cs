/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: ebe8e57b-7f1d-449c-b241-a8ae8d3d0a45      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-WANGJINCAI
/////                 Author: TEST(gaozt)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Alarm.Contract.Data
/////    Project Description:    
/////             Class Name: AlarmInfo
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/8/8 11:07:41
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/8/8 11:07:41
/////            Modified by:
/////   Modified Description: 
/////======================================================================

using Gsafety.PTMS.Base.Contract.Data;
using Gsafety.PTMS.SecuritySuite.Contract;
using Gsafety.PTMS.SecuritySuite.Contract.Data;
using Gsafety.PTMS.SecuritySuite.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Gsafety.Common.Logging;
using Gsafety.PTMS.BaseInfo;
using Gsafety.PTMS.DBEntity;

namespace Gs.PTMS.Service
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "WorkingSuiteService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select WorkingSuiteService.svc or WorkingSuiteService.svc.cs at the Solution Explorer and start debugging.
    public class WorkingSuiteService : BaseService, IWorkingSuiteService
    {
        private WorkingSuiteRepository Repository = new WorkingSuiteRepository();
        public SingleMessage<int> AddWorkingSuite(WorkingSuite workingSuite)
        {
            try
            {
                Info("AddWorkingSuite");
                Info("workingSuite:" + Convert.ToString(workingSuite));

                var temp = 0;
                using (PTMSEntities context = new PTMSEntities())
                {
                    temp = Repository.AddWorkingSuite(context, workingSuite);
                }

                SingleMessage<int> result = new SingleMessage<int> { Result = temp, IsSuccess = true };
                Log<int>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new SingleMessage<int>() { ExceptionMessage = ex, IsSuccess = false };
            }
        }

        public SingleMessage<bool> UpdateWorkingSuite(WorkingSuite workingSuite)
        {
            try
            {
                Info("UpdateWorkingSuite");
                Info("workingSuite:" + Convert.ToString(workingSuite));

                var temp = false;
                using (PTMSEntities context = new PTMSEntities())
                {
                    temp = Repository.UpdateWorkingSuite(context, workingSuite);
                }

                SingleMessage<bool> result = new SingleMessage<bool> { Result = temp, IsSuccess = true };
                Log<bool>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new SingleMessage<bool>() { ExceptionMessage = ex, IsSuccess = false };
            }
        }

        public SingleMessage<bool> DeleteWorkingSuite(string id)
        {
            try
            {
                Info("DeleteWorkingSuite");
                Info("id:" + Convert.ToString(id));
                var temp = false;
                using (PTMSEntities context = new PTMSEntities())
                {
                    temp = Repository.DeleteWorkingSuite(context, id);
                }

                SingleMessage<bool> result = new SingleMessage<bool> { Result = temp, IsSuccess = true };
                Log<bool>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new SingleMessage<bool>() { ExceptionMessage = ex, IsSuccess = false };
            }
        }
    }
}
