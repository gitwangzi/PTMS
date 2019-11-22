using System;
using System.Collections.Generic;
using System.Linq;
using Gsafety.PTMS.BaseInfo;
using System.ServiceModel;
using Gsafety.PTMS.DBEntity;
using Gsafety.PTMS.Common.Data;
using Gsafety.PTMS.Base.Contract.Data;
using Gsafety.PTMS.Common.Data.Enum;
namespace LogServiceRepository
{
    ///<summary>
    ///
    ///</summary>
    public class LogVideoRepository
    {

        static string FailedToSave = "Failed to Save to DB";
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="model"></param>
        public static SingleMessage<bool> InsertLogVideo(PTMSEntities context, List<LogVideo> models)
        {
            foreach (var model in models)
            {
                var entity = new LOG_VIDEO();
                BSC_DEV_SUITE suite = context.BSC_DEV_SUITE.FirstOrDefault(n => n.MDVR_CORE_SN == model.MdvrCoreSn);
                if (suite != null)
                {
                    model.SuiteSn = suite.SUITE_ID;
                }
                LogVideoUtility.UpdateEntity(entity, model, true);

                context.LOG_VIDEO.Add(entity);
            }


            return context.Save();
        }

        /// <summary>
        /// 修改
        /// </summary>
        public static SingleMessage<bool> UpdateLogVideo(PTMSEntities context, LogVideo model)
        {
            var entity = context.LOG_VIDEO.FirstOrDefault(t => t.ID == model.ID);
            if (null == entity)
            {
                return new SingleMessage<bool>(false, "");
            }

            LogVideoUtility.UpdateEntity(entity, model, false);

            return context.Save();
        }

        /// <summary>
        /// 删除
        /// </summary>
        public static SingleMessage<bool> DeleteLogVideoByID(PTMSEntities context, string ID)
        {
            LOG_VIDEO entity = context.LOG_VIDEO.FirstOrDefault(t => t.ID == ID);
            if (entity != null)
            {
                context.LOG_VIDEO.Attach(entity);
                context.LOG_VIDEO.Remove(entity);
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
        public static SingleMessage<LogVideo> GetLogVideo(PTMSEntities context, string ID)
        {
            LOG_VIDEO entity = context.LOG_VIDEO.SingleOrDefault(n => n.ID == ID);
            if (entity != null)
            {
                LogVideo model = LogVideoUtility.GetModel(entity);
                return new SingleMessage<LogVideo>(model);
            }
            return null;
        }

        /// <summary>
        /// 获取
        /// </summary>
        public static MultiMessage<LogVideo> GetLogVideoList(PTMSEntities context, int pageIndex = 1, int pageSize = 10)
        {
            int totalCount;
            var list = context.LOG_VIDEO.Page(out totalCount, pageIndex, pageSize, true, t => t.OPERATE_TIME).ToList();

            var items = list.Select(t => LogVideoUtility.GetModel(t)).ToList();

            return new MultiMessage<LogVideo>(items, totalCount);
        }


        public static MultiMessage<LogVideo> GetVideoPlayLogList(PTMSEntities context, string clientID, string vehicleID, string userName, string mdvrCoreSN, DateTime? startTime, DateTime? endTime, int pageIndex, int pageSize)
        {
            int totalCount;
            var list = from v in context.LOG_VIDEO
                       where v.CLIENT_ID == clientID && (startTime.HasValue ? v.OPERATE_TIME > startTime.Value : true) && (endTime.HasValue ? v.OPERATE_TIME < endTime : true)
                       && (string.IsNullOrEmpty(vehicleID) ? true : v.VEHICLE_ID.ToUpper().Contains(vehicleID.ToUpper()))
                       && (string.IsNullOrEmpty(userName) ? true : v.OPERATOR_NAME.ToUpper().Contains(userName.ToUpper()))
                       && (string.IsNullOrEmpty(mdvrCoreSN) ? true : v.MDVR_CORE_SN.ToUpper().Contains(mdvrCoreSN.ToUpper()))
                       && v.LOG_TYPE == (short)VideoLogTypeEnum.Play
                       select v;


            var items = list.Page(out totalCount, pageIndex, pageSize, true, n => n.OPERATE_TIME, false).ToList();

            var temp = items.Select(n => LogVideoUtility.GetModel(n)).ToList();

            return new MultiMessage<LogVideo>(temp, totalCount);
        }

        public static MultiMessage<LogVideo> GetVideoDownloadLogList(PTMSEntities context, string clientID, string vehicleID, string userName, string mdvrCoreSN, DateTime? startTime, DateTime? endTime, int pageIndex, int pageSize)
        {
            int totalCount;
            var list = from v in context.LOG_VIDEO
                       where v.CLIENT_ID == clientID && (startTime.HasValue ? v.OPERATE_TIME > startTime.Value : true) && (endTime.HasValue ? v.OPERATE_TIME < endTime : true)
                       && (string.IsNullOrEmpty(vehicleID) ? true : v.VEHICLE_ID.ToUpper().Contains(vehicleID.ToUpper()))
                       && (string.IsNullOrEmpty(userName) ? true : v.OPERATOR_NAME.ToUpper().Contains(userName.ToUpper()))
                       && (string.IsNullOrEmpty(mdvrCoreSN) ? true : v.MDVR_CORE_SN.ToUpper().Contains(mdvrCoreSN.ToUpper()))
                       && v.LOG_TYPE == (short)VideoLogTypeEnum.Download
                       select v;


            var items = list.Page(out totalCount, pageIndex, pageSize, true, n => n.OPERATE_TIME, false).ToList();

            var temp = items.Select(n => LogVideoUtility.GetModel(n)).ToList();

            return new MultiMessage<LogVideo>(temp, totalCount);
        }

        public static bool ClearVideoPlayLog(PTMSEntities context, string ClientID, LogOperate log)
        {
            bool suc = false;
            try
            {
                List<LOG_VIDEO> list = context.LOG_VIDEO.Where(n => n.CLIENT_ID == ClientID && n.LOG_TYPE == (short)VideoLogTypeEnum.Play).ToList();
                foreach (var item in list)
                {
                    context.LOG_VIDEO.Remove(item);
                }

                LOG_OPERATE data = new LOG_OPERATE();
                data.CLIENT_ID = log.ClientID;
                data.OPERATE_CONTENT = log.OperateContent;
                data.OPERATE_TIME = log.OperateTime;
                data.OPERATE_TYPE = (short)OperateTypeEnum.LogManagement;
                data.OPERATOR_ID = log.OperatorID;
                data.OPERATOR_NAME = log.OperatorName;
                data.ID = log.ID;
                context.LOG_OPERATE.Add(data);

                return context.SaveChanges() > 0;
            }
            catch
            {
                suc = false;
            }

            return suc;
        }

        public static bool ClearVideoDownloadLog(PTMSEntities context, string ClientID, LogOperate log)
        {
            bool suc = false;
            try
            {
                List<LOG_VIDEO> list = context.LOG_VIDEO.Where(n => n.CLIENT_ID == ClientID && n.LOG_TYPE == (short)VideoLogTypeEnum.Download).ToList();
                foreach (var item in list)
                {
                    context.LOG_VIDEO.Remove(item);
                }

                LOG_OPERATE data = new LOG_OPERATE();
                data.CLIENT_ID = log.ClientID;
                data.OPERATE_CONTENT = log.OperateContent;
                data.OPERATE_TIME = log.OperateTime;
                data.OPERATE_TYPE = (short)OperateTypeEnum.LogManagement;
                data.OPERATOR_ID = log.OperatorID;
                data.OPERATOR_NAME = log.OperatorName;
                data.ID = log.ID;
                context.LOG_OPERATE.Add(data);

                return context.SaveChanges() > 0;
            }
            catch
            {
                suc = false;
            }

            return suc;
        }
    }
}

