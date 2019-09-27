using Gsafety.PTMS.Base.Contract.Data;
using Gsafety.PTMS.BaseInformation.Contract.Data;
using Gsafety.PTMS.Common.Data;
using Gsafety.PTMS.DBEntity;
using Gsafety.PTMS.BaseInfo;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Gsafety.PTMS.BaseInformation.Repository
{
    ///<summary>
    ///
    ///</summary>
    public class VehicleTypeRepository
    {
        static string FailedToSave = "Failed to Save to DB";
        static string FailedToDelete = "Failed to Delete to DB";
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="model"></param>
        public static SingleMessage<bool> InsertVehicleType(PTMSEntities context, VehicleType model)
        {
            bool exist = context.BSC_VEHICLE_TYPE.Any(n => n.NAME == model.Name && n.CLIENT_ID == model.ClientID && n.VALID == 1);
            if (exist)
            {
                return new SingleMessage<bool>(false, "SameNameExist");
            }
            BSC_VEHICLE_TYPE vehicleType = new BSC_VEHICLE_TYPE();
            vehicleType = VehicleTypeUtility.UpdateEntity(model, vehicleType, true);
            vehicleType.VALID = 1;
            context.BSC_VEHICLE_TYPE.Add(vehicleType);

           
            int result = context.SaveChanges();
            if (result > 0)
            {
                return new SingleMessage<bool>(true);
            }
            else
            {
                return new SingleMessage<bool>(false);
            }

        }


        public static MultiMessage<VehicleTypeColor> GetVehicleTypeColorList(PTMSEntities context, string typeid)
        {
            MultiMessage<VehicleTypeColor> result = new MultiMessage<VehicleTypeColor>();
            int totalCount;
            var sour = from v in context.BSC_VEHICLE_SPEEDCOLOR
                       where v.VALID == 1 && v.TYPE_ID == typeid 
                       select v;

            var items = sour.ToList().Select(t => VehicleTypeUtility.GetColorModel(t)).ToList();

            return new MultiMessage<VehicleTypeColor>(items, items.Count);

        }

        public static MultiMessage<VehicleTypeColor> GetAllVehicleTypeColorList(PTMSEntities context, string clientID)
        {
            MultiMessage<VehicleTypeColor> result = new MultiMessage<VehicleTypeColor>();
            int totalCount;
            var sour = from v in context.BSC_VEHICLE_SPEEDCOLOR
                       join type in context.BSC_VEHICLE_TYPE on v.TYPE_ID equals type.ID
                       where v.VALID == 1 && type.CLIENT_ID == clientID && type.VALID == 1
                       select new VehicleTypeColor() 
                       
                       {
                           ID = v.ID,
                           TypeID=type.ID,
                           TypeName = type.NAME,
                           Color=v.COLOR,
                           MinSpeed =v.MINSPEED.Value,
                           MaxSpeed = v.MAXSPEED.Value                     

                       
                       };

            var items = sour.ToList();

            return new MultiMessage<VehicleTypeColor>(items, items.Count);

        }

        public static SingleMessage<bool> InsertVehicleTypeColor(PTMSEntities context, List<VehicleTypeColor> color)
        {          

            if (color != null)
            {
                foreach (var item in color)
                {
                    BSC_VEHICLE_SPEEDCOLOR vehiclecolor = new BSC_VEHICLE_SPEEDCOLOR();
                    vehiclecolor.ID = item.ID;
                    vehiclecolor.TYPE_ID = item.TypeID;
                    vehiclecolor.MINSPEED = item.MinSpeed;
                    vehiclecolor.MAXSPEED = item.MaxSpeed;
                    vehiclecolor.COLOR = item.Color;
                    vehiclecolor.VALID = 1;
                    context.BSC_VEHICLE_SPEEDCOLOR.Add(vehiclecolor);
                }

            }
            int result = context.SaveChanges();
            if (result > 0)
            {
                return new SingleMessage<bool>(true);
            }
            else
            {
                return new SingleMessage<bool>(false);
            }

        }

        public static SingleMessage<bool> UpdateVehicleTypeColor(PTMSEntities context, VehicleTypeColor color)
        {


            var entity = context.BSC_VEHICLE_SPEEDCOLOR.FirstOrDefault(t => t.ID == color.ID);
            if (null == entity)
            {
                return new SingleMessage<bool>(false, "");
            }
          
            entity.TYPE_ID = color.TypeID;
            entity.MINSPEED = color.MinSpeed;
            entity.MAXSPEED = color.MaxSpeed;
            entity.COLOR = color.Color;
            entity.VALID = 1;
           
            context.Entry(entity).State = System.Data.EntityState.Modified;
            int res = context.SaveChanges();

            if (res > 0)
                return new SingleMessage<bool>(true);
            else
                return new SingleMessage<bool>(false, FailedToSave);

        }

        public static SingleMessage<bool> DeleteVehicleTypeColor(PTMSEntities context, string colorid)
        {

            BSC_VEHICLE_SPEEDCOLOR entity = context.BSC_VEHICLE_SPEEDCOLOR.FirstOrDefault(t => t.ID == colorid);
           
            if (entity != null)
            {
                entity.VALID = 0;
                int result = context.SaveChanges();
                if (result > 0)
                    return new SingleMessage<bool>(true);
                else
                    return new SingleMessage<bool>(false, FailedToDelete);
            }
            else
            {
                return new SingleMessage<bool>(false, "ReferenceByVehicle");
            }

        }

        /// <summary>
        /// 修改
        /// </summary>
        public static SingleMessage<bool> UpdateVehicleType(PTMSEntities context, VehicleType model)
        {
            bool exist = context.BSC_VEHICLE_TYPE.Any(n => n.NAME == model.Name && n.CLIENT_ID == model.ClientID && n.VALID == 1 && n.ID != model.ID);
            if (exist)
            {
                return new SingleMessage<bool>(false, "SameNameExist");
            }
            var entity = context.BSC_VEHICLE_TYPE.FirstOrDefault(t => t.ID == model.ID);
            if (null == entity)
            {
                return new SingleMessage<bool>(false, "");
            }

            VehicleTypeUtility.UpdateEntity(model, entity, false);
            context.Entry(entity).State = System.Data.EntityState.Modified;
            int res = context.SaveChanges();

            if (res > 0)
                return new SingleMessage<bool>(true);
            else
                return new SingleMessage<bool>(false, FailedToSave);
        }

        /// <summary>
        /// 删除
        /// </summary>
        public static SingleMessage<bool> DeleteVehicleTypeByID(PTMSEntities context, string ID)
        {
            BSC_VEHICLE_TYPE entity = context.BSC_VEHICLE_TYPE.FirstOrDefault(t => t.ID == ID);
            BSC_VEHICLE hasVehicle = context.BSC_VEHICLE.FirstOrDefault(v => v.VEHICLE_TYPE == entity.ID);//判断是否被应用
            if (entity != null && hasVehicle == null)
            {
                entity.VALID = 0;
                int result = context.SaveChanges();
                if (result > 0)
                    return new SingleMessage<bool>(true);
                else
                    return new SingleMessage<bool>(false, FailedToDelete);
            }
            else
            {
                return new SingleMessage<bool>(false, "ReferenceByVehicle");
            }
        }

        /// <summary>
        /// 获取
        /// </summary>
        public static SingleMessage<VehicleType> GetVehicleType(PTMSEntities context, string ID)
        {
            BSC_VEHICLE_TYPE entity = context.BSC_VEHICLE_TYPE.SingleOrDefault(n => n.ID == ID);
            if (entity != null)
            {
                VehicleType model = VehicleTypeUtility.GetModel(entity);
                return null;
            }
            return null;
        }

        /// <summary>
        /// 获取所有类型
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static MultiMessage<VehicleType> GetByNameVehicleTypeList(PTMSEntities context, int pageIndex, int pageSize, string ClientID, string name)
        {
            MultiMessage<VehicleType> result = new MultiMessage<VehicleType>();
            int totalCount;
            var sour = from v in context.BSC_VEHICLE_TYPE
                       where v.VALID == 1 && v.CLIENT_ID == ClientID && (string.IsNullOrEmpty(name) ? true : v.NAME.ToUpper().Contains(name.ToUpper()))
                       select v;

            var items = sour.Page(out totalCount, pageIndex, pageSize, true, n => n.CREATE_TIME, false)
                .ToList()
                .Select(t => VehicleTypeUtility.GetModel(t))
                .ToList();

            return new MultiMessage<VehicleType>(items, totalCount);

        }



        public static MultiMessage<VehicleType> GetByPageVehicleTypeList(PTMSEntities context, int pageIndex, int pageSize, string ClientID)
        {
            MultiMessage<VehicleType> result = new MultiMessage<VehicleType>();
            int totalCount;
            var sour = from v in context.BSC_VEHICLE_TYPE
                       where v.VALID == 1 && v.CLIENT_ID == ClientID
                       select v;

            var items = sour.Page(out totalCount, pageIndex, pageSize, true, n => n.CREATE_TIME, false)
               .ToList()
               .Select(t => VehicleTypeUtility.GetModel(t))
               .ToList();

            return new MultiMessage<VehicleType>(items, totalCount);

        }

        #region zhangxw 160708

        /// <summary>
        /// 获取车辆
        /// </summary>
        public static SingleMessage<Vehicle> GetBscVehicle(PTMSEntities context, string VehicleId)
        {
            BSC_VEHICLE entity = context.BSC_VEHICLE.SingleOrDefault(n => n.VEHICLE_ID == VehicleId);
            if (entity != null)
            {
                //Vehicle model = BscVehicleUtility.GetModel(entity);
                //return model;
            }
            return null;
        }

        /// <summary>
        /// 获取车辆
        /// </summary>
        public static MultiMessage<Vehicle> GetBscVehicleList(PTMSEntities context, int pageIndex = 1, int pageSize = 10)
        {
            int totalCount;
            //var list = context.BSC_VEHICLE.Page(out totalCount, pageIndex, pageSize, true).ToList();

            //var items = list.Select(t => BscVehicleUtility.GetModel(t)).ToList();

            //return new MultiMessage<Vehicle>(items, totalCount);

            return null;
        }
        #endregion
    }
}

