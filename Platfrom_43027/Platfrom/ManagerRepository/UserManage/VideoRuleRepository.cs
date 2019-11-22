using System;
using System.Collections.Generic;
using System.Linq;
using Gsafety.PTMS.BaseInfo;

using Gsafety.PTMS.DBEntity;
using Gsafety.PTMS.Manager.Contract.Data.CommandManage;
using Gsafety.PTMS.Base.Contract.Data;
using Gsafety.PTMS.Manager.Contract.Data;
using Gsafety.PTMS.Common.Data;
using Gsafety.PTMS.Common.Data.Enum;

namespace Gsafety.PTMS.Manager.Repository
{
    ///<summary>
    ///视频设置
    ///</summary>
    public class VideoRuleRepository
    {

        static string FailedToSave = "Failed to Save to DB";
        /// <summary>
        /// 添加视频设置
        /// </summary>
        /// <param name="model">视频设置</param>
        public static SingleMessage<bool> InsertVideoRule(PTMSEntities context, VideoRule model)
        {
            var entity = new TRF_COMMAND_PARAM();
            var hasOne = context.TRF_COMMAND_PARAM.FirstOrDefault(p => p.NAME == model.Name && p.VALID == (short)ValidEnum.Valid);
            if (hasOne != null)
                return new SingleMessage<bool>(false, "");
            VideoRuleUtility.UpdateEntity(entity, model, true);

            context.TRF_COMMAND_PARAM.Add(entity);

            return context.Save();
        }

        /// <summary>
        /// 修改视频设置
        /// </summary>
        public static SingleMessage<bool> UpdateVideoRule(PTMSEntities context, VideoRule model)
        {
            var entity = context.TRF_COMMAND_PARAM.FirstOrDefault(t => t.ID == model.ID && t.VALID == (short)ValidEnum.Valid);
            if (null == entity)
            {
                return new SingleMessage<bool>(false, "");
            }

            VideoRuleUtility.UpdateEntity(entity, model, false);
            context.Entry(entity).State = System.Data.EntityState.Modified;
            return context.Save();
        }

        /// <summary>
        /// 删除视频设置
        /// </summary>
        public static SingleMessage<bool> DeleteVideoRuleByID(PTMSEntities context, string ID)
        {
            TRF_COMMAND_PARAM entity = context.TRF_COMMAND_PARAM.FirstOrDefault(t => t.ID == ID);
            if (entity != null)
            {
                context.TRF_COMMAND_PARAM.Attach(entity);
                context.TRF_COMMAND_PARAM.Remove(entity);
                return context.Save();
            }
            else
            {
                return new SingleMessage<bool>(false, "");
            }
        }

        /// <summary>
        /// 获取视频设置
        /// </summary>
        public static SingleMessage<VideoRule> GetVideoRule(PTMSEntities context, string ID)
        {
            TRF_COMMAND_PARAM entity = context.TRF_COMMAND_PARAM.SingleOrDefault(n => n.ID == ID);
            if (entity != null)
            {
                VideoRule model = VideoRuleUtility.GetModel(entity);
                return new SingleMessage<VideoRule>(model);
            }
            return null;
        }

        /// <summary>
        /// 获取视频设置
        /// </summary>
        /// <summary>

        public static MultiMessage<VideoRule> GetVideoRuleList(PTMSEntities context, int pageIndex, int pageSize, string clientID, string name)
        {
            int totalCount = 0;
            List<TRF_COMMAND_PARAM> list = null;
            if (string.IsNullOrEmpty(name))
            {
                list = context.TRF_COMMAND_PARAM.Where(n => n.CLIENT_ID == clientID && n.TYPE == (short)CommandParaEnum.LED).Page(out totalCount, pageIndex, pageSize, true, n => n.CREATE_TIME, false).ToList();

            }
            else
            {
                list = context.TRF_COMMAND_PARAM.Where(n => n.CLIENT_ID == clientID && n.TYPE == (short)CommandParaEnum.LED && n.NAME.Contains(name)).Page(out totalCount, pageIndex, pageSize, true, n => n.CREATE_TIME, false).ToList();
            }

            var items = list.Select(t => VideoRuleUtility.GetModel(t)).ToList();
            List<VideoRule> items2 = new List<VideoRule>();
            List<string> ids = list.Select(n => n.ID).ToList();
            List<string> referenceids = context.TRF_COMMAND_VEHICLE.Where(v => ids.Contains(v.COMMAND_PARAM_ID)).Select(n => n.COMMAND_PARAM_ID).ToList();
            foreach (var temp in items)
            {
                if (referenceids.Contains(temp.ID))
                {
                    temp.IsVisible = false;
                }
                else
                {
                    temp.IsVisible = true;
                }
                items2.Add(temp);
            }

            return new MultiMessage<VideoRule>(items2, totalCount);
        }


        public static MultiMessage<VideoRule> GetByConditionVideoRuleList(PTMSEntities context, int pageIndex, int pageSize, string ClientID, int Brightness, int Contrast)
        {
            MultiMessage<VideoRule> result = new MultiMessage<VideoRule>();

            var sour = from v in context.TRF_COMMAND_PARAM
                       where v.VALID == 1 && v.CLIENT_ID == ClientID && (Brightness > 0 ? v.BRIGHTNESS == Brightness : true) && (Contrast > 0 ? v.CONTRAST == Contrast : true) && v.TYPE == (short)CommandParaEnum.LED
                       select v;
            List<TRF_COMMAND_PARAM> entitylist = null;
            if (pageIndex > 0)
            {
                result.TotalRecord = sour.Count();
                entitylist = sour.OrderBy(t => t.CREATE_TIME)
                    .Skip(pageSize * (pageIndex - 1))
                    .Take(pageSize)
                    .ToList();
            }
            else
            {
                result.TotalRecord = sour.Count();
                entitylist = sour.OrderBy(t => t.CREATE_TIME).ToList();
            }

            foreach (var item in entitylist)
            {
                result.Result.Add(VideoRuleUtility.GetModel(item));
            }

            return result;

        }

        public static MultiMessage<VideoRule> GetVideoRuleListint(PTMSEntities context, int pageIndex, int pageSize, string ClientID, string name)
        {
            MultiMessage<VideoRule> result = new MultiMessage<VideoRule>();
            List<TRF_COMMAND_PARAM> entitylist = null;
            if (string.IsNullOrEmpty(name))
            {
                var sour = from v in context.TRF_COMMAND_PARAM
                           where v.VALID == 1 && v.CLIENT_ID == ClientID && v.TYPE == (short)CommandParaEnum.LED
                           select v;


                if (pageIndex > 0)
                {
                    result.TotalRecord = sour.Count();
                    entitylist = sour.OrderBy(t => t.CREATE_TIME)
                        .Skip(pageSize * (pageIndex - 1))
                        .Take(pageSize)
                        .ToList();
                }
                else
                {
                    result.TotalRecord = sour.Count();
                    entitylist = sour.OrderBy(t => t.CREATE_TIME).ToList();
                }
            }
            else
            {
                name = name.ToUpper();

                var sour = from v in context.TRF_COMMAND_PARAM
                           where v.VALID == 1 && v.CLIENT_ID == ClientID && v.NAME.ToUpper().Contains(name) && v.TYPE == (short)CommandParaEnum.LED
                           select v;


                if (pageIndex > 0)
                {
                    result.TotalRecord = sour.Count();
                    entitylist = sour.OrderBy(t => t.CREATE_TIME)
                        .Skip(pageSize * (pageIndex - 1))
                        .Take(pageSize)
                        .ToList();
                }
                else
                {
                    result.TotalRecord = sour.Count();
                    entitylist = sour.OrderBy(t => t.CREATE_TIME).ToList();
                }
            }


            foreach (var item in entitylist)
            {
                result.Result.Add(VideoRuleUtility.GetModel(item));
            }

            return result;
        }
    }
}

