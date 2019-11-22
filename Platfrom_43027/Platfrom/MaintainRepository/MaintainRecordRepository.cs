using System;
using System.Collections.Generic;
using System.Linq;
using Gsafety.PTMS.BaseInfo;
using Gsafety.PTMS.Base.Contract.Data;
using Gsafety.PTMS.DBEntity;
using Gsafety.PTMS.Common.Data;
using System.Data;
namespace Gsafety.PTMS.Maintain.Repository
{
    ///<summary>
    ///
    ///</summary>
    public class MaintainRecordRepository
    {
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="model"></param>
        public static SingleMessage<bool> InsertMaintainRecord(PTMSEntities context, MaintainRecord model)
        {
            var entity = new MTN_MAINTAIN_RECORD();
            var en = context.MTN_MAINTAIN_APPLICATION.FirstOrDefault(t => t.ID == model.ApplicationID);

            var hasOne = context.MTN_MAINTAIN_RECORD.FirstOrDefault(r => r.APPLICATION_ID == model.ApplicationID);

            if (hasOne == null)
            {
                model.Worker = en.WORKER == null ? "-" : en.WORKER;
                model.Note = en.NOTE == null ? "-" : en.NOTE;
                model.VehicleID = en.VEHCILE_ID;

                MaintainRecordUtility.UpdateEntity(entity, model, true);
                context.MTN_MAINTAIN_RECORD.Add(entity);
            }
            else
            {
                model.Worker = en.WORKER == null ? "-" : en.WORKER;
                model.Note = en.WORKER == null ? "-" : en.WORKER;
                model.VehicleID = en.VEHCILE_ID;
                MaintainRecordUtility.UpdateEntity(hasOne, model, false);
            }

            try
            {
                MaintainApplication map = MaintainApplicationUtility.GetModel(en);
                map.Status = model.EndTime == null ? 3 : 4;

                var entity2 = context.MTN_MAINTAIN_APPLICATION.FirstOrDefault(t => t.ID == map.ID);
                if (null == entity2)
                {
                    return new SingleMessage<bool>(false, "");
                }
                MaintainApplicationUtility.UpdateEntity(entity2, map, false);
            }
            catch (Exception em)
            {
                throw;
            }


            return context.Save();
        }

        /// <summary>
        /// 修改
        /// </summary>
        public static SingleMessage<bool> UpdateMaintainRecord(PTMSEntities context, MaintainRecord model)
        {
            var entity = context.MTN_MAINTAIN_RECORD.FirstOrDefault(t => t.ID == model.ID);
            if (null == entity)
            {
                return new SingleMessage<bool>(false, "");
            }

            MaintainRecordUtility.UpdateEntity(entity, model, false);
            context.Entry(entity).State = EntityState.Modified;
            return context.Save();
        }

        /// <summary>
        /// 删除
        /// </summary>
        public static SingleMessage<bool> DeleteMaintainRecordByID(PTMSEntities context, string ID)
        {
            MTN_MAINTAIN_RECORD entity = context.MTN_MAINTAIN_RECORD.FirstOrDefault(t => t.ID == ID);
            if (entity != null)
            {
                context.MTN_MAINTAIN_RECORD.Attach(entity);
                context.MTN_MAINTAIN_RECORD.Remove(entity);
                return context.Save();
            }
            else
            {
                return new SingleMessage<bool>(false, "");
            }
        }

        /// <summary>
        /// 获取
        /// </summary>
        public static SingleMessage<MaintainRecord> GetMaintainRecord(PTMSEntities context, string ID)
        {
            MTN_MAINTAIN_RECORD entity = context.MTN_MAINTAIN_RECORD.SingleOrDefault(n => n.ID == ID);
            if (entity != null)
            {
                MaintainRecord model = MaintainRecordUtility.GetModel(entity);
                return new SingleMessage<MaintainRecord>(model);
            }
            return null;
        }

        /// <summary>
        /// 获取
        /// </summary>
        public static MultiMessage<MaintainRecord> GetMaintainRecordList(PTMSEntities context, string clientID, string worker, DateTime? beginTime, DateTime? endTime, int pageIndex = 1, int pageSize = 10)
        {
            int totalCount;

           
            var result = from r in context.MTN_MAINTAIN_RECORD
                         join m in context.MTN_MAINTAIN_APPLICATION on r.APPLICATION_ID equals m.ID
                         where (string.IsNullOrEmpty(worker) ? true : r.WORKER.Contains(worker)) &&
                          (beginTime == null ? true : r.START_TIME >= beginTime) &&                         
                           m.CLIENT_ID == clientID && r.STATUS == 2
                         select new MaintainRecord()
                         {
                             Worker = r.WORKER,
                             VehicleID = m.VEHCILE_ID,
                             SetupStation = m.SETUP_STATION,
                             Note = r.NOTE,

                             ScheduleDate = (m.SCHEDULE_DATE.HasValue ? (DateTime)m.SCHEDULE_DATE : r.START_TIME), //已维修
                             StartTime = r.START_TIME,
                             EndTime = r.END_TIME,
                             CreateTime = r.CREATE_TIME,
                             Status = r.STATUS
                         };
            var items = result.Page(out totalCount, pageIndex, pageSize, true).ToList().OrderByDescending(s => s.CreateTime).ToList();

            return new MultiMessage<MaintainRecord>(items, totalCount);
        }

    }
}

