using Gsafety.PTMS.BaseInformation.Contract.Data;
using Gsafety.PTMS.DBEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gsafety.PTMS.BaseInformation.Repository
{
    public class MonitorGroupVehicleRepository
    {
        public MonitorGroupVehicleRepository()
        {
        }

        public List<MonitorGroupVehicle> GetAllMonitorGroupsVehicle(string userid)
        {
            //using (PTMSEntities _context = new PTMSEntities())
            //{
            //    var result = (from A in _context.MONITOR_GROUP_VEHICLE
            //                  join B in _context.VEHICLE on A.VEHICLE_ID.Trim() equals B.VEHICLE_ID.Trim()
            //                  join G in _context.MONITOR_GROUP on A.GROUP_ID.Trim() equals G.ID
            //                  join W in _context.SECURITY_SUITE_WORKING on A.VEHICLE_ID.Trim() equals W.VEHICLE_ID.Trim() into ww
            //                  from W in ww.DefaultIfEmpty()
            //                  join S in _context.DEV_SUITE on W.SUITE_INFO_ID.Trim() equals S.SUITE_INFO_ID.Trim() into ss
            //                  from S in ss.DefaultIfEmpty()
            //                  where (W.STATUS == 24 || W.STATUS == 23) && G.AD_USER == userid
            //                  orderby A.VEHICLE_INDEX
            //                  select new MonitorGroupVehicle
            //                  {
            //                      Group_ID = A.GROUP_ID,
            //                      Vehicle_ID = A.VEHICLE_ID,
            //                      Traced_Flag = A.TRACED_FLAG,
            //                      Vehicle_Index = A.VEHICLE_INDEX,
            //                      Vehicle_SN = B.VEHICLE_SN,
            //                      Type = B.VEHICLE_TYPE == null ? VehicleType.Bus : (VehicleType)B.VEHICLE_TYPE,
            //                      MDVRID = S.MDVR_CORE_SN,

            //                      IsOnLine = W.ONLINE_FLAG,
            //                  });
            //    return result.ToList();
            //}
            return null;
        }

        public bool AddMonitorGroupsVehicle(string Group_ID, string Vehicle_ID, short? TRACED_FLAG, int? VEHICLE_INDEX)
        {
            RUN_MONITOR_GROUP_VEHICLE item = new RUN_MONITOR_GROUP_VEHICLE();
            item.ID = Guid.NewGuid().ToString();
            item.VEHICLE_ID = Vehicle_ID;
            item.GROUP_ID = Group_ID;
            item.TRACED_FLAG = TRACED_FLAG;
            item.VEHICLE_INDEX = VEHICLE_INDEX;

            using (PTMSEntities context = new PTMSEntities())
            {
                var Vehicle = (from a in context.RUN_MONITOR_GROUP_VEHICLE
                               where a.VEHICLE_ID == Vehicle_ID && a.GROUP_ID == Group_ID
                               select a).FirstOrDefault();
                if (Vehicle != null)
                {
                    context.RUN_MONITOR_GROUP_VEHICLE.Remove(Vehicle);
                }
                context.RUN_MONITOR_GROUP_VEHICLE.Add(item);

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

        public bool RemoveMonitorGroupsVehicle(string Group_ID, string Vehicle_ID)
        {
            using (PTMSEntities context = new PTMSEntities())
            {
                var result = (from x in context.RUN_MONITOR_GROUP_VEHICLE
                              where x.GROUP_ID.Trim() == Group_ID.Trim() && x.VEHICLE_ID.Trim() == Vehicle_ID.Trim()
                              select x).FirstOrDefault();
                if (result != null)
                {
                    context.RUN_MONITOR_GROUP_VEHICLE.Remove(result);
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
                else
                {
                    return false;
                }
            }
        }

        public bool MoveMonitorGroupsVehicle(string OldGroup_ID, string NewGroup_ID, string Vehicle_ID, short? TRACED_FLAG, int? VEHICLE_INDEX)
        {
            using (PTMSEntities context = new PTMSEntities())
            {
                var result = (from x in context.RUN_MONITOR_GROUP_VEHICLE
                              where x.GROUP_ID.Trim() == OldGroup_ID.Trim()
                                  && x.VEHICLE_ID.Trim() == Vehicle_ID.Trim()
                              select x).FirstOrDefault();
                if (result != null)
                {
                    result.GROUP_ID = NewGroup_ID;
                    result.TRACED_FLAG = TRACED_FLAG;
                    result.VEHICLE_INDEX = VEHICLE_INDEX;
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
                else
                {
                    return false;
                }
            }

        }
        /// <summary>
        ///  Set TRACED_FLAG to Monitor Group Vehicles
        /// </summary>
        /// <param name="monitorGroup_ID"></param>
        /// <param name="Vehicle_ID"></param>
        /// <param name="IsTrace"></param>
        /// <returns></returns>
        public bool SetMonitorGroupsVehicleState(string monitorGroup_ID, string Vehicle_ID, bool IsTrace)
        {
            using (PTMSEntities context = new PTMSEntities())
            {
                var result = (from x in context.RUN_MONITOR_GROUP_VEHICLE
                              where x.GROUP_ID == monitorGroup_ID && x.VEHICLE_ID.Trim() == Vehicle_ID.Trim()
                              select x).FirstOrDefault();
                if (result != null)
                {
                    result.TRACED_FLAG = IsTrace ? (short)1 : (short)0;
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
                else
                {
                    return false;
                }
            }

        }
    }
}
