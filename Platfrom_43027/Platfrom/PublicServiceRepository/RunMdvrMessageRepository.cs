using System;
using System.Collections.Generic;
using System.Linq;
using Gsafety.PTMS.BaseInfo;
using System.ServiceModel;
using Gsafety.PTMS.DBEntity;
using Gsafety.PTMS.PublicService.Contract;
using Gsafety.PTMS.Base.Contract.Data;
using System.Data;
namespace GSafety.PTMS.PublicService.Repository
{
    ///<summary>
    ///LED消息
    ///</summary>
    public class RunMdvrMessageRepository
    {

        static string FailedToSave = "Failed to Save to DB";
        /// <summary>
        /// 添加LED消息
        /// </summary>
        /// <param name="model">LED消息</param>
        public static SingleMessage<bool> InsertRunMdvrMessage(PTMSEntities context, RunMdvrMessage model)
        {
            var entity = new RUN_MDVR_MESSAGE();
            RunMdvrMessageUtility.UpdateEntity(entity, model, true);

            context.RUN_MDVR_MESSAGE.Add(entity);

            return context.Save();
        }

        /// <summary>
        /// 修改LED消息
        /// </summary>
        public static SingleMessage<bool> UpdateRunMdvrMessage(PTMSEntities context, RunMdvrMessage model)
        {
            var entity = context.RUN_MDVR_MESSAGE.FirstOrDefault(t => t.ID == model.ID);
            if (null == entity)
            {
                return new SingleMessage<bool>(false, "");
            }

            RunMdvrMessageUtility.UpdateEntity(entity, model, false);
            context.Entry(entity).State = EntityState.Modified;
            return context.Save();
        }

        /// <summary>
        /// 删除LED消息
        /// </summary>
        public static SingleMessage<bool> DeleteRunMdvrMessageByID(PTMSEntities context, string ID)
        {
            RUN_MDVR_MESSAGE entity = context.RUN_MDVR_MESSAGE.FirstOrDefault(t => t.ID == ID);
            if (entity != null)
            {
                context.RUN_MDVR_MESSAGE.Attach(entity);
                context.RUN_MDVR_MESSAGE.Remove(entity);
                return context.Save();
            }
            else
            {
                return new SingleMessage<bool>(false, "");
            }
        }

        /// <summary>
        /// 获取LED消息
        /// </summary>
        public static SingleMessage<RunMdvrMessage> GetRunMdvrMessage(PTMSEntities context, string ID)
        {
            RUN_MDVR_MESSAGE entity = context.RUN_MDVR_MESSAGE.SingleOrDefault(n => n.ID == ID);
            if (entity != null)
            {
                RunMdvrMessage model = RunMdvrMessageUtility.GetModel(entity);
                return new SingleMessage<RunMdvrMessage>(model);
            }
            return null;
        }

        /// <summary>
        /// 获取LED消息
        /// </summary>
        public static MultiMessage<RunMdvrMessage> GetRunMdvrMessageList(PTMSEntities context, string clientID, string title, int type, string name, int pageIndex = 1, int pageSize = 10)
        {
            int totalCount;
            // var list = context.RUN_MDVR_MESSAGE.Page(out totalCount, pageIndex, pageSize, true).ToList();

            var sour = from m in context.RUN_MDVR_MESSAGE
                       where m.CLIENT_ID == clientID &&
                       (string.IsNullOrEmpty(name) ? true : m.CONTENT.Contains(name)) &&
                       (string.IsNullOrEmpty(title) ? true : m.MESSAGE_TITLE.Contains(title)) &&
                       (type == -1 ? true : m.MESSAGE_TYPE == type)
                       select m;
            var items = sour.Page(out totalCount, pageIndex, pageSize, true).ToList()
                .OrderByDescending(t => t.CREATE_TIME)
                .Select(t => RunMdvrMessageUtility.GetModel(t)).ToList();

            List<RunMdvrMessage> result = new List<RunMdvrMessage>();
            List<string> messageids = items.Select(n => n.ID).ToList();

            var dic = (from mv in context.RUN_MDVRMESSAGE_VEHICLE
                       where messageids.Contains(mv.MESSAGE_ID)
                       group mv by mv.MESSAGE_ID into g
                       select g.Key).ToList();



            foreach (var item in items)
            {
                if (dic.Contains(item.ID))
                {
                    item.IsVisible = false;
                }
                else
                {
                    item.IsVisible = true;
                }
                

                result.Add(item);
            }

            return new MultiMessage<RunMdvrMessage>(result, totalCount);
        }

    }
}

