using Gsafety.Ant.BaseInformation.Repository.Utilties;
using Gsafety.PTMS.Base.Contract.Data;
using Gsafety.PTMS.BaseInfo;
using Gsafety.PTMS.BaseInformation.Contract.Data;
using Gsafety.PTMS.Common.Data;
using Gsafety.PTMS.DBEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Gsafety.PTMS.BaseInfo;
using Gsafety.PTMS.Common.Enum;
using System.Data;

namespace Gsafety.Ant.BaseInformation.Repository
{
    ///<summary>
    ///GPS设备
    ///</summary>
    public class DevGpsRepository
    {

        static string FailedToSave = "FailedToSave";
        /// <summary>
        /// 添加GPS设备
        /// </summary>
        /// <param name="model">GPS设备</param>
        public static SingleMessage<bool> InsertDevGps(PTMSEntities context, DevGps model)
        {
            var entity = new BSC_DEV_GPS();
            entity = DevGpsUtility.UpdateEntity(entity, model, true);
            entity.VALID = 1;

            bool exist = context.BSC_DEV_GPS.Any(x => x.GPS_SN == model.GpsSn && x.VALID == 1);
            if (exist)
            {
                return new SingleMessage<bool>(false, "SNDuplicate");
            }

            exist = context.BSC_DEV_GPS.Any(x => x.GPS_UID == model.GpsUid && x.VALID == 1);
            if (exist)
            {
                return new SingleMessage<bool>(false, "UidDuplicate");
            }

            exist = context.BSC_DEV_GPS.Any(x => x.GPS_SIM == model.GpsSim && x.VALID == 1);
            if (exist)
            {
                return new SingleMessage<bool>(false, "SIMDuplicate");
            }

            context.BSC_DEV_GPS.Add(entity);
            return context.Save();

        }

        /// <summary>
        /// 修改GPS设备
        /// </summary>
        public static SingleMessage<bool> UpdateDevGps(PTMSEntities context, DevGps model)
        {
            bool exist = context.BSC_DEV_GPS.Any(x => x.GPS_SN == model.GpsSn && x.VALID == 1 && x.ID != model.ID);
            if (exist)
            {
                return new SingleMessage<bool>(false, "SNDuplicate");
            }

            exist = context.BSC_DEV_GPS.Any(x => x.GPS_UID == model.GpsUid && x.VALID == 1 && x.ID != model.ID);
            if (exist)
            {
                return new SingleMessage<bool>(false, "UidDuplicate");
            }

            exist = context.BSC_DEV_GPS.Any(x => x.GPS_SIM == model.GpsSim && x.VALID == 1 && x.ID != model.ID);
            if (exist)
            {
                return new SingleMessage<bool>(false, "SIMDuplicate");
            }

            var entity = context.BSC_DEV_GPS.FirstOrDefault(t => t.ID == model.ID && t.VALID == (short)ValidEnum.Valid);
            if (null == entity)
            {
                return new SingleMessage<bool>(false, "");
            }

            DevGpsUtility.UpdateEntity(entity, model, false);
            context.Entry(entity).State = EntityState.Modified;
            return context.Save();
        }

        /// <summary>
        /// 删除GPS设备
        /// </summary>
        public static SingleMessage<bool> DeleteDevGpsByID(PTMSEntities context, string ID)
        {
            bool reference = context.MTN_GPS_INSTALLATION_DETAIL.Any(n => n.GPS_ID == ID);
            if (reference)
            {
                return new SingleMessage<bool>(false, "GPSInstallReference");
            }

            BSC_DEV_GPS entity = context.BSC_DEV_GPS.FirstOrDefault(t => t.ID == ID);
            if (entity != null)
            {
                entity.VALID = 0;

                return context.Save();
            }
            else
            {
                return new SingleMessage<bool>(false, "");
            }
        }

        /// <summary>
        /// 获取GPS设备
        /// </summary>
        public static SingleMessage<DevGps> GetDevGps(PTMSEntities context, string ID)
        {
            BSC_DEV_GPS entity = context.BSC_DEV_GPS.SingleOrDefault(n => n.ID == ID);
            if (entity != null)
            {
                DevGps model = DevGpsUtility.GetModel(entity);
                return new SingleMessage<DevGps>(model);
            }
            return null;
        }

        /// <summary>
        /// 获取GPS设备
        /// </summary>
        public static MultiMessage<DevGps> GetDevGpsList(PTMSEntities context, int pageIndex, int pageSize, string ClientID)
        {
            //int totalCount;
            //var list = context.BSC_DEV_GPS.Page(out totalCount, pageIndex, pageSize, true).ToList();

            //var items = list.Select(t => DevGpsUtility.GetModel(t)).ToList();

            //return new MultiMessage<DevGps>(items, totalCount);

            MultiMessage<DevGps> result = new MultiMessage<DevGps>();

            var sour = from v in context.BSC_GPS_VIEW
                       where v.VALID == 1 && v.CLIENT_ID == ClientID
                       select v;
            List<BSC_GPS_VIEW> entitylist = null;
            if (pageIndex > 0)
            {
                result.TotalRecord = sour.Count();
                entitylist = sour.OrderByDescending(t => t.CREATE_TIME)
                    .Skip(pageSize * (pageIndex - 1))
                    .Take(pageSize)
                    .ToList();
            }
            else
            {
                result.TotalRecord = sour.Count();
                entitylist = sour.OrderByDescending(t => t.CREATE_TIME).ToList();
            }

            foreach (var item in entitylist)
            {
                DevGps gps = new DevGps();

                if (item.STATUS == (short)DeviceSuiteStatus.Initial)
                {
                    gps.InstallStatus = InstallStatusType.UnInstall;
                }
                else if (item.CHECKSTEP.HasValue && item.CHECKSTEP < 4)
                {
                    gps.InstallStatus = InstallStatusType.Installing;
                }
                else if (item.CHECKSTEP.HasValue && item.CHECKSTEP == 4)
                {
                    gps.InstallStatus = InstallStatusType.Installed;
                }

                gps.ID = item.ID;
                gps.ClientId = item.CLIENT_ID;
                gps.GpsSn = item.GPS_SN;
                gps.GpsUid = item.GPS_UID;
                gps.GpsSim = item.GPS_SIM;
                gps.Status = item.STATUS;
                gps.CreateTime = item.CREATE_TIME;
                gps.VehicleID = item.VEHICLE_ID;
                gps.DistrictCode = item.DISTRICT_CODE;
                gps.Valid = item.VALID;
                result.Result.Add(gps);
            }

            return result;
        }


        public static MultiMessage<DevGps> GetByNameDevGpsList(PTMSEntities context, int pageIndex, int pageSize, string ClientID, string name, string vehicleID,InstallStatusType? installStatus, string mdvrSim)
        {
            List<DevGps> result = new List<DevGps>();

            Expression<Func<BSC_GPS_VIEW, bool>> filter = v => v.VALID == 1 && v.CLIENT_ID == ClientID;
            if (string.IsNullOrEmpty(name) == false)
            {
                filter = filter.And(v => v.GPS_SN.ToUpper().Contains(name.ToUpper()));
            }

            if (string.IsNullOrEmpty(vehicleID) == false)
            {
                filter = filter.And(v => v.VEHICLE_ID.ToUpper().Contains(vehicleID.ToUpper()));
            }

            if (string.IsNullOrEmpty(mdvrSim) == false)
            {
                filter = filter.And(v => v.GPS_SIM.ToUpper().Contains(mdvrSim.ToUpper()));
            }
           
            if (installStatus.HasValue)
            {
                if (installStatus.Value == InstallStatusType.UnInstall)
                {
                    filter = filter.And(v => v.STATUS.Equals((short)DeviceSuiteStatus.Initial));
                   
                }
                else if (installStatus.Value == InstallStatusType.Installing)
                {
                    filter = filter.And(v => v.CHECKSTEP.HasValue && v.CHECKSTEP < 4);
                   
                }
                else if (installStatus.Value == InstallStatusType.Installed)
                {

                    filter = filter.And(v => v.CHECKSTEP.HasValue && v.CHECKSTEP == 4);
                }
            }

            int totalCount;
            var list = context.BSC_GPS_VIEW.Where(filter).Page(out totalCount, pageIndex, pageSize, true, n => n.CREATE_TIME, false);

            foreach (var item in list)
            {
                DevGps gps = new DevGps();

                if (item.STATUS == (short)DeviceSuiteStatus.Initial)
                {
                    gps.InstallStatus = InstallStatusType.UnInstall;
                }
                else if (item.CHECKSTEP.HasValue && item.CHECKSTEP < 4)
                {
                    gps.InstallStatus = InstallStatusType.Installing;
                }
                else if (item.CHECKSTEP.HasValue && item.CHECKSTEP == 4)
                {
                    gps.InstallStatus = InstallStatusType.Installed;
                }

                gps.ID = item.ID;
                gps.ClientId = item.CLIENT_ID;
                gps.GpsSn = item.GPS_SN;
                gps.GpsUid = item.GPS_UID;
                gps.GpsSim = item.GPS_SIM;
                gps.Status = item.STATUS;
                gps.CreateTime = item.CREATE_TIME;
                gps.VehicleID = item.VEHICLE_ID;
                gps.DistrictCode = item.DISTRICT_CODE;
                gps.Valid = item.VALID;
                result.Add(gps);
            }

            return new MultiMessage<DevGps>(result, totalCount);

        }

        public static SingleMessage<DevGps> GetDevGpsBySN(PTMSEntities context, string sn)
        {
            BSC_DEV_GPS entity = context.BSC_DEV_GPS.SingleOrDefault(n => n.GPS_SN == sn);
            if (entity != null)
            {
                DevGps model = DevGpsUtility.GetModel(entity);
                return new SingleMessage<DevGps>(model);
            }
            return new SingleMessage<DevGps>(false, "GPSDeviceNoExist");
        }

        public static bool BatchAddDevGps(List<DevGps> devGpsList)
        {
            using (PTMSEntities context = new PTMSEntities())
            {
                foreach (var item in devGpsList)
                {
                    context.BSC_DEV_GPS.Add(new BSC_DEV_GPS
                    {
                        ID = Guid.NewGuid().ToString(),
                        CLIENT_ID = item.ClientId,
                        GPS_SN = item.GpsSn,
                        GPS_UID = item.GpsUid,
                        GPS_SIM = item.GpsSim,
                        DISTRICT_CODE = item.DistrictCode,
                        CREATOR = item.Creator,
                        CREATE_TIME = DateTime.UtcNow,
                        VALID = 1,
                        STATUS = (short)DeviceSuiteStatus.Initial
                    });
                }
                context.SaveChanges();
                return true;
            }
        }

        public static MultiMessage<DevGps> CheckDevGpsExist(List<DevGps> devGpsList)
        {
            using (PTMSEntities context = new PTMSEntities())
            {
                Func<DevGps, bool> filter = tv => context.BSC_DEV_GPS.Local.Any(v => v.VALID == 1 && (v.GPS_SN == tv.GpsSn || v.GPS_UID == tv.GpsUid))
                     || context.BSC_DEV_GPS.Any(v => v.VALID == 1 && (v.GPS_SN == tv.GpsSn || v.GPS_UID == tv.GpsUid));

                var list = devGpsList.Where(filter).ToList();
                return new MultiMessage<DevGps>(list, list.Count);
            }
        }
    }
}

