/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 45cd3052-a584-4b7f-a2a6-48d51c12e5ee      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: LIN-20130409DMW
/////                 Author: Admin(zhuyh)
/////======================================================================
/////           Project Name: Gsafety.PTMS.BaseInformation.Repository
/////    Project Description:    
/////             Class Name: VehicleRepository
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/9/2 15:00:00
/////      Class Description:  
/////======================================================================
/////          Modified Time: 
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using Gsafety.PTMS.BaseInformation.Contract.Data;
using Gsafety.PTMS.DBEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Gsafety.PTMS.BaseInformation.Repository
{
    public class MonitorGroupRepository
    {
        public MonitorGroupRepository()
        {
        }

        public List<MonitorGroup> GetMonitorGroups(string UserID)
        {
            using (PTMSEntities _context = new PTMSEntities())
            {
                var result = (from A in _context.RUN_MONITOR_GROUP
                              where A.AD_USER == UserID
                              orderby A.GROUP_INDEX
                              select new MonitorGroup
                              {
                                  ID = A.ID,
                                  GroupName = A.GROUP_NAME,
                                  GroupIndex = A.GROUP_INDEX,
                                  CreateUser = A.AD_USER,
                                  Note = A.NOTE,
                              }).ToList();
                return result;
            }
        }

        /// <summary>
        /// Add  group
        /// </summary>
        /// <param name="monitorGroup"></param>
        /// <returns></returns>
        public bool AddMonitorGroup(MonitorGroup monitorGroup)
        {
            RUN_MONITOR_GROUP item = new RUN_MONITOR_GROUP();
            item.ID = Guid.NewGuid().ToString();
            item.NOTE = monitorGroup.Note;
            item.AD_USER = monitorGroup.CreateUser;
            item.GROUP_INDEX = monitorGroup.GroupIndex;
            item.GROUP_NAME = monitorGroup.GroupName;
            using (PTMSEntities context = new PTMSEntities())
            {
                context.RUN_MONITOR_GROUP.Add(item);
                int i = context.SaveChanges();
                if (i > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// Batch Add Monitor Group
        /// </summary>
        /// <param name="monitorGroup">group list</param>
        /// <param name="adUser">user</param>
        /// <returns></returns>
        public bool BatchAddMonitorGroup(List<MonitorGroup> monitorGroup, string adUser)
        {
            bool IsSuccess = true;
            var scope = new TransactionScope(TransactionScopeOption.Required);

            try
            {
                using (PTMSEntities context = new PTMSEntities())
                {
                    var result = (from A in context.RUN_MONITOR_GROUP
                                  where A.AD_USER == adUser
                                  orderby A.GROUP_INDEX
                                  select A).ToList();

                    if (result != null)
                    {
                        foreach (RUN_MONITOR_GROUP item in result)
                        {
                            MonitorGroup exits = monitorGroup.Where(w => w.ID == item.ID).FirstOrDefault();
                            if (null != exits)
                            {
                                item.NOTE = exits.Note;
                                item.GROUP_NAME = exits.GroupName;
                                item.GROUP_INDEX = exits.GroupIndex;
                                monitorGroup.Remove(exits);
                            }
                            else
                            {
                                context.RUN_MONITOR_GROUP.Remove(item);
                            }
                        }
                    }
                    foreach (MonitorGroup mg in monitorGroup)
                    {
                        RUN_MONITOR_GROUP newItem = new RUN_MONITOR_GROUP();
                        newItem.ID = mg.ID;
                        newItem.NOTE = mg.Note;
                        newItem.AD_USER = mg.CreateUser;
                        newItem.GROUP_INDEX = mg.GroupIndex;
                        newItem.GROUP_NAME = mg.GroupName;
                        context.RUN_MONITOR_GROUP.Add(newItem);
                    }
                    context.SaveChanges();
                }
                scope.Complete();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                scope.Dispose();
            }

            return IsSuccess;
        }
    }
}
