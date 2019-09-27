using System;
using System.Collections.Generic;
using System.Linq;
using Gsafety.PTMS.BaseInfo;
using System.ServiceModel;
using Gsafety.PTMS.Base.Contract.Data;
using Gsafety.PTMS.DBEntity;
using Gsafety.PTMS.PublicService.Contract;
using Gsafety.PTMS.Common.Data.Enum;
using System.Data;
namespace GSafety.PTMS.PublicService.Repository
{
    ///<summary>
    ///App消息
    ///</summary>
    public class RunAppMessageRepository
    {

        static string FailedToSave = "Failed to Save to DB";
        /// <summary>
        /// 添加App消息
        /// </summary>
        /// <param name="model">App消息</param>
        public static SingleMessage<bool> InsertRunAppMessage(PTMSEntities context, RunAppMessage model)
        {
            var entity = new RUN_APP_MESSAGE();
            RunAppMessageUtility.UpdateEntity(entity, model, true);

            context.RUN_APP_MESSAGE.Add(entity);

            return context.Save();
        }

        /// <summary>
        /// 修改App消息
        /// </summary>
        public static SingleMessage<bool> UpdateRunAppMessage(PTMSEntities context, RunAppMessage model)
        {
            var entity = context.RUN_APP_MESSAGE.FirstOrDefault(t => t.ID == model.ID);
            if (null == entity)
            {
                return new SingleMessage<bool>(false, "");
            }

            RunAppMessageUtility.UpdateEntity(entity, model, false);
            context.Entry(entity).State = EntityState.Modified;
            return context.Save();
        }

        /// <summary>
        /// 删除App消息
        /// </summary>
        public static SingleMessage<bool> DeleteRunAppMessageByID(PTMSEntities context, string ID)
        {
            RUN_APP_MESSAGE entity = context.RUN_APP_MESSAGE.FirstOrDefault(t => t.ID == ID);
            if (entity != null)
            {
                context.RUN_APP_MESSAGE.Attach(entity);
                context.RUN_APP_MESSAGE.Remove(entity);
                return context.Save();
            }
            else
            {
                return new SingleMessage<bool>(false, "");
            }
        }

        /// <summary>
        /// 获取App消息
        /// </summary>
        public static SingleMessage<RunAppMessage> GetRunAppMessage(PTMSEntities context, string ID)
        {
            RUN_APP_MESSAGE entity = context.RUN_APP_MESSAGE.SingleOrDefault(n => n.ID == ID);
            if (entity != null)
            {
                RunAppMessage model = RunAppMessageUtility.GetModel(entity);
                return new SingleMessage<RunAppMessage>(model);
            }
            return null;
        }

        /// <summary>
        /// 获取App消息
        /// </summary>
        public static MultiMessage<RunAppMessage> GetRunAppMessageList(PTMSEntities context, string clientID, string title, int pageIndex = 1, int pageSize = 10)
        {
            int totalCount;
            var list = context.RUN_APP_MESSAGE.Where(s => s.CLIENT_ID == clientID && (string.IsNullOrEmpty(title) ? true : s.MESSAGE_TITLE.Contains(title)))
                .Page(out totalCount, pageIndex, pageSize, true).OrderByDescending(t => t.CREATE_TIME).ToList();

            var items = list.Select(t => RunAppMessageUtility.GetModel(t)).ToList();

            List<string> ids = new List<string>();
            foreach (var item in items)
            {
                ids.Add(item.ID);
            }

            var dic = (from mv in context.RUN_APPMESSAGE_VEHICLE
                       where ids.Contains(mv.MESSAGE_ID)
                       group mv by mv.MESSAGE_ID into g
                       select g.Key).ToList();

            foreach (var item in items)
            {
                if (dic.Contains(item.ID))
                {
                    item.CanDelete = false;
                }
                else
                {
                    item.CanDelete = true;
                }
            }

            return new MultiMessage<RunAppMessage>(items, totalCount);
        }

        /// <summary>
        /// 添加App消息
        /// </summary>
        /// <param name="model">App消息</param>
        public static SingleMessage<bool> SendRunAppMessage(PTMSEntities context, RunAppMessage model, string vehicleId)
        {
            var app_message = new RUN_APP_MESSAGE();
            RunAppMessageUtility.UpdateEntity(app_message, model, true);
            context.RUN_APP_MESSAGE.Add(app_message);

            List<BSC_VEHICLE_CHAUFFEUR> allappvehicles = (from cv in context.BSC_VEHICLE_CHAUFFEUR
                                                          where vehicleId == cv.VEHICLE_ID
                                                          select cv).ToList();

            List<string> chaufferids = allappvehicles.Select(n => n.CHAUFFEUR_ID).Distinct().ToList();

            List<BSC_CHAUFFEUR> chauffeurs = (from c in context.BSC_CHAUFFEUR
                                              where chaufferids.Contains(c.ID)
                                              select c).ToList();


            foreach (var item in allappvehicles)
            {
                RUN_APPMESSAGE_VEHICLE entity = new RUN_APPMESSAGE_VEHICLE();
                entity.CHAUFFEUR_ID = item.CHAUFFEUR_ID;
                BSC_CHAUFFEUR c = chauffeurs.FirstOrDefault(n => n.ID == entity.CHAUFFEUR_ID);
                entity.CHAUFFEUR_NAME = c.NAME;
                entity.CLIENT_ID = c.CLIENT_ID;
                entity.ID = Guid.NewGuid().ToString();
                entity.VEHICLE_ID = item.VEHICLE_ID;
                entity.STATUS = (short)CommandStateEnum.WaitForDeliver;
                entity.MESSAGE = model.Message;
                entity.MESSAGE_ID = model.ID;
                entity.MESSAGE_TITLE = model.MessageTitle;
                entity.MESSAGE_TYPE = model.MessageType;
                entity.CREATE_TIME = DateTime.Now.ToUniversalTime();
                entity.MOBILE_UID = c.CELLPHONE;
                context.RUN_APPMESSAGE_VEHICLE.Add(entity);
            }

            return context.Save();
        }
    }
}

