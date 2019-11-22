using Gsafety.PTMS.BaseInformation.Contract.Data;
using Gsafety.PTMS.DBEntity;
using Gsafety.PTMS.Message.Contract.Data;
using Gsafety.PTMS.SecuritySuite.Contract.Data;
/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 3db9825f-b7a4-48ca-996c-449214e5d358      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: GAOZITIAN-PC
/////                 Author: TEST(gaozt)
/////======================================================================
/////           Project Name: Gsafety.PTMS.SecuritySuite.Repository
/////    Project Description:    
/////             Class Name: WorkingSuiteRepository
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/9/2 17:02:14
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/9/2 17:02:14
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gsafety.PTMS.SecuritySuite.Contract;

namespace Gsafety.PTMS.SecuritySuite.Repository
{
    public class WorkingSuiteRepository
    {
        public int AddWorkingSuite(PTMSEntities context, WorkingSuite workingSuite)
        {
            //SECURITY_SUITE_WORKING checkitem = context.SECURITY_SUITE_WORKING.FirstOrDefault(d => d.MDVR_CORE_SN == workingSuite.MdvrCoreId);
            //if (checkitem == null)
            //{
            //    SECURITY_SUITE_WORKING item = new SECURITY_SUITE_WORKING();
            //    if (workingSuite.Id != null) item.SUITE_INFO_ID = workingSuite.Id;
            //    if (workingSuite.VehicleId != null) item.VEHICLE_ID = workingSuite.VehicleId;
            //    if (workingSuite.MdvrCoreId != null) item.MDVR_CORE_SN = workingSuite.MdvrCoreId;
            //    if (workingSuite.Status != null) item.STATUS = (short?)workingSuite.Status;
            //    if (workingSuite.SwitchTime != null) item.SWITCH_TIME = workingSuite.SwitchTime;
            //    if (workingSuite.OnlineFlag != null) item.ONLINE_FLAG = (short?)workingSuite.OnlineFlag;
            //    if (workingSuite.AbnormalCause != null) item.ABNORMAL_CAUSE = workingSuite.AbnormalCause;
            //    context.SECURITY_SUITE_WORKING.Add(item);
                if (context.SaveChanges() > 0)
                    return 1;
                else
                    return 0;
            //}
            //else
            //{
            //    return 2;
            //}
        }

        public bool UpdateWorkingSuite(PTMSEntities context, WorkingSuite workingSuite)
        {
            //SECURITY_SUITE_WORKING item = context.SECURITY_SUITE_WORKING.Single(d => d.MDVR_CORE_SN == workingSuite.MdvrCoreId);
            //if (workingSuite.Id != null) item.SUITE_INFO_ID = workingSuite.Id;
            //if (workingSuite.VehicleId != null) item.VEHICLE_ID = workingSuite.VehicleId;
            //if (workingSuite.MdvrCoreId != null) item.MDVR_CORE_SN = workingSuite.MdvrCoreId;
            //if (workingSuite.Status != null) item.STATUS = (short?)workingSuite.Status;
            //if (workingSuite.SwitchTime != null) item.SWITCH_TIME = workingSuite.SwitchTime;
            //if (workingSuite.OnlineFlag != null) item.ONLINE_FLAG = (short?)workingSuite.OnlineFlag;
            //if (workingSuite.AbnormalCause != null) item.ABNORMAL_CAUSE = workingSuite.AbnormalCause;
            if (context.SaveChanges() > 0)
                return true;
            else
                return false;

        }

        public bool DeleteWorkingSuite(PTMSEntities context, string id)
        {
            //SECURITY_SUITE_WORKING item = context.SECURITY_SUITE_WORKING.SingleOrDefault(a => a.SUITE_INFO_ID == id);
            //if (item != null)
            //    context.SECURITY_SUITE_WORKING.Remove(item);
            if (context.SaveChanges() > 0)
                return true;
            else
                return false;

        }
    }
}
