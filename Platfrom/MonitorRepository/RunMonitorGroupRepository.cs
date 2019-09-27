using Gsafety.PTMS.Base.Contract.Data;
using Gsafety.PTMS.BaseInformation.Repository;
using Gsafety.PTMS.DBEntity;
using Gsafety.PTMS.Monitor.Contract.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using Gsafety.PTMS.BaseInfo;
using System.Collections.ObjectModel;

namespace Gsafety.PTMS.Monitor.Repository
{
    ///<summary>
    ///分组
    ///</summary>
    public class RunMonitorGroupRepository
    {
        static string FailedToSave = "Failed to Save to DB";


        public static SingleMessage<bool> ChangeRunMonitorGroup(PTMSEntities context, ObservableCollection<RunMonitorGroup> groupModel, string userID)
        {
            SingleMessage<bool> result = new SingleMessage<bool>(true, "");
            result.Result = true;

            bool change = false;
            List<RUN_MONITOR_GROUP> updateObj = new List<RUN_MONITOR_GROUP>();

            var deletedObj = context.RUN_MONITOR_GROUP.Where(n => n.AD_USER == userID).ToList();

            for (int i = deletedObj.Count - 1; i >= 0; i--)
            {
                foreach (var el in groupModel)
                {
                    if (deletedObj[i].ID == el.ID)
                    {
                        updateObj.Add(deletedObj[i]);
                        deletedObj.Remove(deletedObj[i]);
                        break;
                    }
                }
            }

            if (deletedObj.Count > 0)
            {
                change = true;
                foreach (var item in deletedObj)
                {
                    context.RUN_MONITOR_GROUP.Remove(item);
                }
            }

            for (int i = 0; i < updateObj.Count; i++)
            {
                change = true;
                string id = updateObj[i].ID;
                RUN_MONITOR_GROUP _entity = updateObj[i];
                RunMonitorGroup model = groupModel.Where(x => x.ID == id).FirstOrDefault();
                RunMonitorGroupUtility.UpdateEntity(_entity, model, false);
                groupModel.Remove(model);
            }

            foreach (var newModel in groupModel)
            {
                change = true;
                var entity = new RUN_MONITOR_GROUP();
                RunMonitorGroupUtility.UpdateEntity(entity, newModel, true);

                context.RUN_MONITOR_GROUP.Add(entity);
            }

            if (!change)
            {
                return new SingleMessage<bool>(true);
            }
            else
            {
                result = context.Save();

                return result;
            }
        }


        /// <summary>
        /// 添加分组
        /// </summary>
        /// <param name="model">分组</param>
        public static SingleMessage<bool> InsertRunMonitorGroup(PTMSEntities context, ObservableCollection<RunMonitorGroup> obj, string carNo, RunMonitorGroupVehicle groupVehicle)
        {
            SingleMessage<bool> result = new SingleMessage<bool>(true, "");
            result.Result = true;

            try
            {
                List<RUN_MONITOR_GROUP> updateObj = new List<RUN_MONITOR_GROUP>();

                #region 挑选出删除的监控组

                var deletedObj = context.RUN_MONITOR_GROUP.ToList();

                for (int i = deletedObj.Count - 1; i >= 0; i--)
                {
                    foreach (var el in obj)
                    {
                        if (deletedObj[i].ID == el.ID)
                        {
                            updateObj.Add(deletedObj[i]);
                            deletedObj.Remove(deletedObj[i]);
                            break;
                        }
                    }
                }

                if (deletedObj.Count > 0)
                {
                    foreach (var item in deletedObj)
                    {
                        context.RUN_MONITOR_GROUP.Remove(item);
                        result = context.Save();

                        if (!result.Result)
                            break;
                    }
                }

                #endregion

                #region 更新已经存在的监控组

                if (result.Result)
                    for (int i = 0; i < updateObj.Count; i++)
                    {
                        string id = updateObj[i].ID;
                        RUN_MONITOR_GROUP _entity = updateObj[i];
                        RunMonitorGroup model = obj.Where(x => x.ID == id).FirstOrDefault();

                        var entity = context.RUN_MONITOR_GROUP.Where(x => x.ID == id)
                            .FirstOrDefault();
                        RunMonitorGroupUtility.UpdateEntity(entity, model, true);
                        context.Save();
                        obj.Remove(model);
                    }

                #endregion

                #region 插入新的监控组

                if (result.Result)
                    foreach (var newModel in obj)
                    {
                        var entity = new RUN_MONITOR_GROUP();
                        RunMonitorGroupUtility.UpdateEntity(entity, newModel, true);

                        context.RUN_MONITOR_GROUP.Add(entity);

                        result = context.Save();
                        if (!result.Result)
                            break;
                    }

                #endregion


                InsertRunMonitorGroupVehicle(context, groupVehicle, carNo);


            }
            catch (Exception)
            {
            }

            return result;
        }

        /// <summary>
        /// 修改分组
        /// </summary>
        public static SingleMessage<bool> UpdateRunMonitorGroup(PTMSEntities context, RunMonitorGroup model)
        {
            var entity = context.RUN_MONITOR_GROUP.FirstOrDefault(t => t.ID == model.ID);
            if (null == entity)
            {
                return new SingleMessage<bool>(false, "");
            }

            RunMonitorGroupUtility.UpdateEntity(entity, model, false);

            return context.Save();
        }

        /// <summary>
        /// 删除分组
        /// </summary>
        public static SingleMessage<bool> DeleteRunMonitorGroupByID(PTMSEntities context, string ID)
        {
            RUN_MONITOR_GROUP entity = context.RUN_MONITOR_GROUP.FirstOrDefault(t => t.ID == ID);
            if (entity != null)
            {
                context.RUN_MONITOR_GROUP.Attach(entity);
                context.RUN_MONITOR_GROUP.Remove(entity);
                return context.Save();
            }
            else
            {
                return new SingleMessage<bool>(false, "");
            }
        }

        /// <summary>
        /// 获取分组
        /// </summary>
        public static SingleMessage<RunMonitorGroup> GetRunMonitorGroup(PTMSEntities context, string ID)
        {
            RUN_MONITOR_GROUP entity = context.RUN_MONITOR_GROUP.SingleOrDefault(n => n.ID == ID);
            if (entity != null)
            {
                RunMonitorGroup model = RunMonitorGroupUtility.GetModel(entity);
                return new SingleMessage<RunMonitorGroup>(model);
            }
            return null;
        }

        /// <summary>
        /// 获取分组
        /// </summary>
        public static MultiMessage<RunMonitorGroup> GetRunMonitorGroupList(PTMSEntities context, string userID, int pageIndex = 1, int pageSize = 10)
        {
            var list = new List<RUN_MONITOR_GROUP>();
            if (!string.IsNullOrWhiteSpace(userID))
            {
                list = (from g in context.RUN_MONITOR_GROUP
                        where g.AD_USER == userID
                        orderby g.GROUP_INDEX
                        select g).ToList();
            }


            var items = list.Select(t => RunMonitorGroupUtility.GetModel(t)).ToList();

            var vehicles = context.RUN_MONITOR_GROUP_VEHICLE.ToList();

            foreach (var item in items)
            {
                item.GroupVehicle = vehicles.Where(x => x.GROUP_ID == item.ID).Select(t => RunMonitorGroupVehicleUtility.GetModel(t)).ToList();
            }

            return new MultiMessage<RunMonitorGroup>(items, items.Count());
        }


        //=============




        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="carNo"></param>
        /// <param name="groupId"></param>
        /// <returns></returns>
        public static SingleMessage<bool> ChangeRunMonitorGroupVehicle(PTMSEntities context, string carNo, string groupId)
        {
            var entity = context.RUN_MONITOR_GROUP_VEHICLE.FirstOrDefault(x => x.VEHICLE_ID == carNo);
            RunMonitorGroupVehicle model = new RunMonitorGroupVehicle();
            if (entity != null)
            {
                //Update
                entity.GROUP_ID = groupId;
            }
            else
            {
                entity = new RUN_MONITOR_GROUP_VEHICLE();
                model.GroupId = groupId;
                model.TracedFlag = 1;
                if (context.RUN_MONITOR_GROUP_VEHICLE != null)
                    model.VehicleIndex = context.RUN_MONITOR_GROUP_VEHICLE.Max(x => x.VEHICLE_INDEX) == null ? 0 :
                        context.RUN_MONITOR_GROUP_VEHICLE.Max(x => x.VEHICLE_INDEX).Value;
                else
                    model.VehicleIndex = 0;
                model.VehicleId = carNo;
                //Add
                RunMonitorGroupVehicleUtility.UpdateEntity(entity, model, true);
                context.RUN_MONITOR_GROUP_VEHICLE.Add(entity);
            }

            return context.Save();

        }



        /// <summary>
        /// 添加分组车辆
        /// </summary>
        /// <param name="model">分组车辆</param>
        public static SingleMessage<bool> InsertRunMonitorGroupVehicle(PTMSEntities context, RunMonitorGroupVehicle model, string carNo)
        {
            try
            {
                var monitorVehicle = context.RUN_MONITOR_GROUP_VEHICLE.Where(x => x.VEHICLE_ID == carNo.Trim()).FirstOrDefault();
                if (monitorVehicle != null)
                {
                    //更新
                    var entity = context.RUN_MONITOR_GROUP_VEHICLE.FirstOrDefault(x => x.VEHICLE_ID == carNo.Trim());
                    model.ID = entity.ID;
                    model.TracedFlag = 1;//标记为车辆是否被监控
                    model.VehicleIndex = (decimal)entity.VEHICLE_INDEX;
                    RunMonitorGroupVehicleUtility.UpdateEntity(entity, model, true);
                }
                else
                {
                    int? vehicleIndex = context.RUN_MONITOR_GROUP_VEHICLE.Max(x => x.VEHICLE_INDEX);

                    if (vehicleIndex != null && vehicleIndex >= 0)
                    {
                        model.ID = Guid.NewGuid().ToString();
                        model.TracedFlag = 1;//标记为车辆是否被监控
                        model.VehicleIndex = (decimal)vehicleIndex;
                    }
                    else
                    {
                        model.ID = Guid.NewGuid().ToString();
                        model.TracedFlag = 1;//标记为车辆是否被监控
                        model.VehicleIndex = 0;
                    }

                    var entity = new RUN_MONITOR_GROUP_VEHICLE();
                    RunMonitorGroupVehicleUtility.UpdateEntity(entity, model, true);

                    context.RUN_MONITOR_GROUP_VEHICLE.Add(entity);
                }
            }
            catch (Exception)
            {

            }

            return context.Save();
        }

        /// <summary>
        /// 修改分组车辆
        /// </summary>
        public static SingleMessage<bool> UpdateRunMonitorGroupVehicle(PTMSEntities context, RunMonitorGroupVehicle model)
        {
            var entity = context.RUN_MONITOR_GROUP_VEHICLE.FirstOrDefault(t => t.ID == model.ID);
            if (null == entity)
            {
                return new SingleMessage<bool>(false, "");
            }

            RunMonitorGroupVehicleUtility.UpdateEntity(entity, model, false);

            return context.Save();
        }

        /// <summary>
        /// 删除分组车辆
        /// </summary>
        public static SingleMessage<bool> DeleteRunMonitorGroupVehicleByID(PTMSEntities context, string ID, string userID)
        {
            RUN_MONITOR_GROUP_VEHICLE entity = (from gv in context.RUN_MONITOR_GROUP_VEHICLE
                                                join g in context.RUN_MONITOR_GROUP on gv.GROUP_ID equals g.ID
                                                where g.AD_USER == userID && gv.VEHICLE_ID == ID
                                                select gv).FirstOrDefault();
            if (entity != null)
            {
                context.RUN_MONITOR_GROUP_VEHICLE.Remove(entity);
                return context.Save();
            }
            else
            {
                return new SingleMessage<bool>(false, "");
            }
        }

        /// <summary>
        /// 获取分组车辆
        /// </summary>
        public static SingleMessage<RunMonitorGroupVehicle> GetRunMonitorGroupVehicle(PTMSEntities context, string ID)
        {
            RUN_MONITOR_GROUP_VEHICLE entity = context.RUN_MONITOR_GROUP_VEHICLE.SingleOrDefault(n => n.ID == ID);
            if (entity != null)
            {
                RunMonitorGroupVehicle model = RunMonitorGroupVehicleUtility.GetModel(entity);
                return new SingleMessage<RunMonitorGroupVehicle>(model);
            }
            return null;
        }

        /// <summary>
        /// 获取分组车辆
        /// </summary>
        public static MultiMessage<RunMonitorGroupVehicle> GetRunMonitorGroupVehicleList(PTMSEntities context, string userID, int pageIndex = 1, int pageSize = 10)
        {
            //int totalCount;
            var list = context.RUN_MONITOR_GROUP_VEHICLE.ToList();
            var groupList = context.RUN_MONITOR_GROUP.Where(n => n.AD_USER == userID).ToList();

            var items = list.Select(t => RunMonitorGroupVehicleUtility.GetModel(t)).ToList();
            var groupItems = groupList.Select(t => RunMonitorGroupUtility.GetModel(t)).ToList();

            foreach (var item in items)
            {
                item.MonitorGroup = groupItems.Where(x => x.ID == item.GroupId).FirstOrDefault();
            }

            return new MultiMessage<RunMonitorGroupVehicle>(items, items.Count);
        }

    }
}

