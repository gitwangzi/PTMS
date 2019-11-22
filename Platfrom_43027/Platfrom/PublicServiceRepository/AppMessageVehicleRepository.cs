using System;
using System.Collections.Generic;
using System.Linq;
using Gsafety.PTMS.BaseInfo;
using System.ServiceModel;
using Gsafety.PTMS.Base.Contract.Data;
using Gsafety.PTMS.DBEntity;
using Gsafety.PTMS.Common.Data;
using Gsafety.PTMS.PublicService.Contract;
using Gsafety.PTMS.Common.Data.Enum;
namespace GSafety.PTMS.PublicService.Repository
{
    ///<summary>
    ///
    ///</summary>
    public class AppMessageVehicleRepository
    {

        static string FailedToSave = "Failed to Save to DB";
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="model"></param>
        public static SingleMessage<bool> InsertAppMessageVehicle(PTMSEntities context, List<AppMessageVehicle> models, RunAppMessage message)
        {
            List<string> selectedvehicles = models.Select(n => n.VehicleID).ToList();
            List<BSC_VEHICLE_CHAUFFEUR> allappvehicles = (from cv in context.BSC_VEHICLE_CHAUFFEUR
                                                          where selectedvehicles.Contains(cv.VEHICLE_ID)
                                                          select cv).ToList();
            List<string> chaufferids = allappvehicles.Select(n => n.CHAUFFEUR_ID).Distinct().ToList();
            List<string> messageids = models.Select(n => n.MessageID).Distinct().ToList();
            List<RUN_APPMESSAGE_VEHICLE> deliveredvehicles = context.RUN_APPMESSAGE_VEHICLE.Where(n => messageids.Contains(n.MESSAGE_ID)).ToList();

            List<BSC_CHAUFFEUR> chauffeurs = (from c in context.BSC_CHAUFFEUR
                                              where chaufferids.Contains(c.ID)
                                              select c).ToList();

            bool should = false;
            foreach (var item in allappvehicles)
            {
                var cv = deliveredvehicles.FirstOrDefault(n => n.CHAUFFEUR_ID == item.CHAUFFEUR_ID && n.VEHICLE_ID == item.VEHICLE_ID);
                if (cv == null)
                {
                    RUN_APPMESSAGE_VEHICLE entity = new RUN_APPMESSAGE_VEHICLE();
                    entity.CHAUFFEUR_ID = item.CHAUFFEUR_ID;
                    BSC_CHAUFFEUR c = chauffeurs.FirstOrDefault(n => n.ID == entity.CHAUFFEUR_ID);
                    entity.CHAUFFEUR_NAME = c.NAME;
                    entity.CLIENT_ID = c.CLIENT_ID;
                    entity.ID = Guid.NewGuid().ToString();
                    entity.VEHICLE_ID = item.VEHICLE_ID;
                    entity.STATUS = (short)CommandStateEnum.UnDelivered;
                    entity.MESSAGE = message.Message;
                    entity.MESSAGE_ID = message.ID;
                    entity.MESSAGE_TITLE = message.MessageTitle;
                    entity.MESSAGE_TYPE = message.MessageType;
                    entity.CREATE_TIME = DateTime.Now.ToUniversalTime();
                    entity.MOBILE_UID = c.CELLPHONE;

                    context.RUN_APPMESSAGE_VEHICLE.Add(entity);

                    should = true;
                }
            }

            if (!should)
            {
                return new SingleMessage<bool>(true);
            }

            return context.Save();
        }


        /// <summary>
        /// 删除
        /// </summary>
        public static SingleMessage<bool> DeleteAppMessageVehicleByID(PTMSEntities context, string ID)
        {
            RUN_APPMESSAGE_VEHICLE entity = context.RUN_APPMESSAGE_VEHICLE.FirstOrDefault(t => t.ID == ID);
            if (entity != null)
            {
                context.RUN_APPMESSAGE_VEHICLE.Attach(entity);
                context.RUN_APPMESSAGE_VEHICLE.Remove(entity);
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
        public static MultiMessage<AppMessageVehicle> GetAppMessageVehicleList(PTMSEntities context, int pageIndex = 1, int pageSize = 10)
        {
            int totalCount;
            var list = context.RUN_APPMESSAGE_VEHICLE.Page(out totalCount, pageIndex, pageSize, true).ToList();

            var items = list.Select(t => AppMessageVehicleUtility.GetModel(t)).ToList();

            return new MultiMessage<AppMessageVehicle>(items, totalCount);
        }


        public static MultiMessage<AppMessageVehicle> GetAppMessageVehicleListByAppID(PTMSEntities context, string appID, int pageIndex, int pageSize)
        {
            int totalCount;

            var result = from mv in context.RUN_APPMESSAGE_VEHICLE
                         where mv.MESSAGE_ID == appID
                         orderby mv.CREATE_TIME descending
                         select new AppMessageVehicle
                         {
                             ID = mv.ID,
                             ChauffeurID = mv.CHAUFFEUR_ID,
                             ChauffeurName = mv.CHAUFFEUR_NAME,
                             ClientID = mv.CLIENT_ID,
                             Message = mv.MESSAGE,
                             MessageID = mv.MESSAGE_ID,
                             SendTime = mv.SEND_TIME,
                             MessageTitle = mv.MESSAGE_TITLE,
                             VehicleID = mv.VEHICLE_ID,
                             Status = mv.STATUS.Value,
                             CreateTime = mv.CREATE_TIME.Value,
                         };

            var list = result.Page(out totalCount, pageIndex, pageSize, true).ToList();

            return new MultiMessage<AppMessageVehicle>(list, totalCount);
        }

        public static MultiMessage<AppMessageVehicle> GetAllAppMessageVehicleListByAppID(PTMSEntities context, string appID)
        {
            var result = from mv in context.RUN_APPMESSAGE_VEHICLE
                         where mv.MESSAGE_ID == appID
                         orderby mv.CREATE_TIME descending
                         select new AppMessageVehicle
                         {
                             ID = mv.ID,
                             ChauffeurID = mv.CHAUFFEUR_ID,
                             ChauffeurName = mv.CHAUFFEUR_NAME,
                             ClientID = mv.CLIENT_ID,
                             Message = mv.MESSAGE,
                             MessageID = mv.MESSAGE_ID,
                             SendTime = mv.SEND_TIME,
                             MessageTitle = mv.MESSAGE_TITLE,
                             VehicleID = mv.VEHICLE_ID,
                             Status = mv.STATUS.Value,
                             CreateTime = mv.CREATE_TIME.Value,
                         };

            var list = result.ToList();

            return new MultiMessage<AppMessageVehicle>(list, list.Count);
        }

        public static SingleMessage<bool> DeliverAppMessageToVehicle(PTMSEntities context, List<AppMessageVehicle> vehicles)
        {
            List<string> ids = vehicles.Select(n => n.ID).ToList();
            var temp = from av in context.RUN_APPMESSAGE_VEHICLE
                       where ids.Contains(av.ID) && av.STATUS == (short)CommandStateEnum.UnDelivered
                       select av;
            List<RUN_APPMESSAGE_VEHICLE> lists = temp.ToList();
            if (lists.Count == 0)
            {
                return new SingleMessage<bool>(true);
            }
            foreach (var item in lists)
            {
                item.STATUS = (short)CommandStateEnum.WaitForDeliver;
            }

            return context.Save();
        }

        public static MultiMessage<AppMessageVehicle> GetMessage(PTMSEntities context, string vehicleid, string num, string pageIndex, string pageValue, DateTime StartTime, DateTime EndTime, int? type)
        {
            int index = Convert.ToInt32(pageIndex);
            int size = Convert.ToInt32(pageValue);
            var result = from mv in context.RUN_APPMESSAGE_VEHICLE
                         join c in context.BSC_CHAUFFEUR on mv.CHAUFFEUR_ID equals c.ID
                         where c.CELLPHONE == num && mv.CREATE_TIME > StartTime && mv.CREATE_TIME < EndTime && (type == null ? true : mv.MESSAGE_TYPE == type) && mv.STATUS > 0
                         orderby mv.SEND_TIME descending
                         select new AppMessageVehicle
                         {
                             ID = mv.ID,
                             ChauffeurID = mv.CHAUFFEUR_ID,
                             ChauffeurName = mv.CHAUFFEUR_NAME,
                             ClientID = mv.CLIENT_ID,
                             Message = mv.MESSAGE,
                             MessageID = mv.MESSAGE_ID,
                             SendTime = mv.SEND_TIME,
                             MessageTitle = mv.MESSAGE_TITLE,
                             MessageType = mv.MESSAGE_TYPE.Value,
                             VehicleID = mv.VEHICLE_ID,
                             Status = mv.STATUS.Value,
                             CreateTime = mv.CREATE_TIME.Value,
                         };

            int count = 0;
            var list = result.Page(out count, int.Parse(pageIndex), int.Parse(pageValue), true, n => n.CreateTime, false).ToList();

            return new MultiMessage<AppMessageVehicle>(list, count);
        }



        public static SingleMessage<AppMessageVehicle> GetMessageByID(PTMSEntities context, string id)
        {
            var result = from mv in context.RUN_APPMESSAGE_VEHICLE
                         where mv.ID == id
                         select new AppMessageVehicle
                         {
                             ID = mv.ID,
                             ChauffeurID = mv.CHAUFFEUR_ID,
                             ChauffeurName = mv.CHAUFFEUR_NAME,
                             ClientID = mv.CLIENT_ID,
                             Message = mv.MESSAGE,
                             MessageID = mv.MESSAGE_ID,
                             SendTime = mv.SEND_TIME,
                             MessageTitle = mv.MESSAGE_TITLE,
                             VehicleID = mv.VEHICLE_ID,
                             Status = mv.STATUS.Value,
                             MessageType = mv.MESSAGE_TYPE.Value,
                             CreateTime = mv.CREATE_TIME.Value,

                         };

            AppMessageVehicle model = result.FirstOrDefault();

            if (model != null)
            {
                return new SingleMessage<AppMessageVehicle>(model);
            }
            else
            {
                return new SingleMessage<AppMessageVehicle>(false, "");
            }
        }
    }
}

