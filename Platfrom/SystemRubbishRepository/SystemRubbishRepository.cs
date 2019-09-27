using Gsafety.PTMS.Base.Contract.Data;
using Gsafety.PTMS.BaseInfo;
using Gsafety.PTMS.BaseInformation.Contract.Data;
using Gsafety.PTMS.Common.Data;
using Gsafety.PTMS.Common.Data.Enum;
using Gsafety.PTMS.Common.Enum;
using Gsafety.PTMS.DBEntity;
using Gsafety.PTMS.Manager.Contract.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Gsafety.PTMS.SystemRubbishRepository.Repository
{
    public class SystemRubbishRepository : Gsafety.PTMS.BaseInfo.BaseRepository
    {
        public SystemRubbishRepository()
        {

        }

        /// <summary>
        /// 获取废弃车辆列表
        /// </summary>
        /// <param name="context"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public static MultiMessage<Vehicle> GetAbandonVehicleList(PTMSEntities context, string clientID, string SearchVehicleId, string SearchOwner, string vehicletypeid)
        {
            try
            {
                Expression<Func<BSC_VEHICLE, bool>> filter = f => f.VALID == 0 && f.CLIENT_ID == clientID;
                if (!string.IsNullOrEmpty(SearchVehicleId))
                {
                    filter = filter.And(f => f.VEHICLE_ID.Contains(SearchVehicleId));
                }
                if (!string.IsNullOrEmpty(SearchOwner))
                {
                    filter = filter.And(f => f.OWNER.Contains(SearchOwner));
                }
                if (!string.IsNullOrEmpty(vehicletypeid))
                {
                    filter = filter.And(v => v.VEHICLE_TYPE == vehicletypeid);
                }
                var source = context.BSC_VEHICLE.Where(filter);
                var items = from c in source
                            join t in context.BSC_VEHICLE_TYPE on c.VEHICLE_TYPE equals t.ID
                            select new Vehicle()
                            {
                                VehicleId = c.VEHICLE_ID,
                                ClientId = c.CLIENT_ID,
                                OrgnizationId = c.ORGNIZATION_ID,
                                VehicleSn = c.VEHICLE_SN,
                                EngineId = c.ENGINE_ID,
                                BrandModel = c.BRAND_MODEL,
                                DistrictCode = c.DISTRICT_CODE,
                                OperationLicense = c.OPERATION_LICENSE,
                                VehicleStatus = (VehicleConditionType)c.VEHICLE_STATUS,
                                Owner = c.OWNER,
                                Contact = c.CONTACT,
                                ContactAddress = c.CONTACT_ADDRESS,
                                ContactEmail = c.CONTACT_EMAIL,
                                ContactPhone = c.CONTACT_PHONE,
                                Region = c.REGION,
                                StartYear = c.START_YEAR,
                                ServiceType = (VehicleSeviceType)c.SERVICE_TYPE,
                                Note = c.NOTE,
                                Creator = c.CREATOR,
                                CreateTime = c.CREATE_TIME == null ? DateTime.UtcNow : (DateTime)c.CREATE_TIME,
                                VehicleType = new VehicleType() { ID = c.VEHICLE_TYPE },
                                VehicleTypeDescribe = t.NAME,
                                VehicleTypeImage = t.ICON,
                                Valid = c.VALID.Value,
                            };

                List<Vehicle> vehicles = items.ToList();                
                return new MultiMessage<Vehicle>(vehicles, vehicles.Count);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public SingleMessage<bool> RecoverVehicle(PTMSEntities context,Vehicle model)
        {

            bool exist = context.BSC_VEHICLE.Where(x => x.VEHICLE_ID == model.VehicleId && x.VALID == 1).Any();
            if (exist)
            {
                return new SingleMessage<bool>(false, "VehicleIDDuplicate");
            }
            exist = context.BSC_VEHICLE.Where(x => x.VEHICLE_SN == model.VehicleSn && x.VALID == 1).Any();
            if (exist)
            {
                return new SingleMessage<bool>(false, "VehicleSnDuplicate");
            }
            exist = context.BSC_VEHICLE.Where(x => x.ENGINE_ID == model.EngineId && x.VALID == 1).Any();
            if (exist)
            {
                return new SingleMessage<bool>(false, "EngineIDDuplicate");
            }
            var result = (from x in context.BSC_VEHICLE
                          where x.VEHICLE_ID == model.VehicleId
                          select x).FirstOrDefault();
            if (result != null)
            {
                result.VALID = 1;
                int i = context.SaveChanges();
                if (i > 0)
                {
                    return new SingleMessage<bool>(true);
                }
                else
                {
                    return new SingleMessage<bool>(false);
                }
            }
            else
            {
                return new SingleMessage<bool>(false);
            }
        }

        /// <summary>
        /// 获取删除安全套件列表
        /// </summary>
        public static MultiMessage<DevSuite> GetAbandonDevSuiteList(PTMSEntities context, string clientID, string suitID, string mdvrCoreSn, string mdvrSn, string mdvrSim)
        {
            var result = from x in context.BSC_SUITE_VIEW.Where(item => item.VALID == 0 && item.CLIENT_ID == clientID)
                         where (string.IsNullOrEmpty(suitID) ? true : x.SUITE_ID.ToUpper().Contains(suitID.ToUpper()))
                         && (string.IsNullOrEmpty(mdvrCoreSn) ? true : x.MDVR_CORE_SN.ToUpper().Contains(mdvrCoreSn.ToUpper()))
                         && (string.IsNullOrEmpty(mdvrSn) ? true : x.MDVR_SN.ToUpper().Contains(mdvrSn.ToUpper()))
                         && (string.IsNullOrEmpty(mdvrSim) ? true : x.MDVR_SIM.ToUpper().Contains(mdvrSim.ToUpper()))
                         select x;

            List<DevSuite> items = new List<DevSuite>();

            foreach (var item in result)
            {
                DevSuite suite = new DevSuite();

                if (item.STATUS == (short)DeviceSuiteStatus.Initial)
                {
                    suite.InstallStatus = InstallStatusType.UnInstall;
                }
                else if (item.CHECKSTEP.HasValue && item.CHECKSTEP < 7)
                {
                    suite.InstallStatus = InstallStatusType.Installing;
                }
                else if (item.CHECKSTEP.HasValue && item.CHECKSTEP == 7)
                {
                    suite.InstallStatus = InstallStatusType.Installed;
                }

                suite.SuiteInfoID = item.SUITE_INFO_ID;
                suite.ClientID = item.CLIENT_ID;
                suite.SuiteID = item.SUITE_ID;
                suite.MdvrCoreSn = item.MDVR_CORE_SN;
                suite.MdvrSn = item.MDVR_SN;
                suite.MdvrSim = item.MDVR_SIM;
                suite.MdvrSimMobile = item.MDVR_SIM_MOBILE;
                suite.SdSn = item.SD_SN;
                suite.SoftwareVersion = item.SOFTWARE_VERSION;
                suite.Model = item.MODEL;
                suite.Protocol = (ProtocolTypeEnum)item.PROTOCOL;
                suite.Status = item.STATUS;
                suite.Note = item.NOTE;
                suite.UpsSn = item.UPS_SN;
                suite.CreateTime = item.CREATE_TIME;
                suite.VehicleID = item.VEHICLE_ID;
                items.Add(suite);
            }

            return new MultiMessage<DevSuite>(items, items.Count);
        }

      
        /// <summary>
        /// 恢复已删除安全套件
        /// </summary>
        public static SingleMessage<bool> RecoverDevSuite(PTMSEntities context, DevSuite model)
        {
                           

            bool exist = context.BSC_DEV_SUITE.Any(n => n.VALID == 1 && n.SUITE_ID == model.SuiteID);
            if (exist)
            {
                return new SingleMessage<bool>(false, "SuiteIDDuplicate");
            }

            exist = context.BSC_DEV_SUITE.Any(n => n.VALID == 1 && n.MDVR_CORE_SN == model.MdvrCoreSn);
            if (exist)
            {
                return new SingleMessage<bool>(false, "MDVRCoreSNDuplicate");
            }
            if (model.MdvrSim != null && model.MdvrSim != "")
            {
                exist = context.BSC_DEV_SUITE.Any(n => n.VALID == 1 && n.MDVR_SIM == model.MdvrSim);
                if (exist)
                {
                    return new SingleMessage<bool>(false, "MdvrSimDuplicate");
                }
            }
            var result = (from x in context.BSC_DEV_SUITE
                          where x.SUITE_INFO_ID == model.SuiteInfoID
                          select x).FirstOrDefault();
            if (result != null)
            {
                result.VALID = 1;
                int i = context.SaveChanges();
                if (i > 0)
                {
                    return new SingleMessage<bool>(true);
                }
                else
                {
                    return new SingleMessage<bool>(false);
                }
            }
            else
            {
                return new SingleMessage<bool>(false);
            }
        }

        public static MultiMessage<GUser> GetAbandonGUserList(PTMSEntities context, string clientID, string qureyUserName)
        {
            if (string.IsNullOrWhiteSpace(clientID))
            {
                return new MultiMessage<GUser>(new List<GUser>(), 0);
            }

            Expression<Func<USR_GUSER, bool>> filter = n => n.CLIENT_ID == clientID && n.VALID == 0;
            if (string.IsNullOrWhiteSpace(qureyUserName) == false)
            {
                qureyUserName = qureyUserName.Trim().ToUpper();
                filter = filter.And(t => t.USER_NAME.ToUpper().Contains(qureyUserName));
            }

            var list = context.USR_GUSER
                .Where(filter)
                .Select(entity => new GUser()
                {
                    ID = entity.ID,
                    Account = entity.ACCOUNT,
                    UserName = entity.USER_NAME,
                    CreateTime = entity.CREATE_TIME,
                    Phone = entity.PHONE,
                    Mobile = entity.MOBILE,
                    Email = entity.EMAIL,
                    Address = entity.ADDRESS,
                    Description = entity.DESCRIPTION,
                    RoleID = entity.ROLE_ID,
                    Creator = context.USR_GUSER.FirstOrDefault(t => t.ID == entity.CREATOR).ACCOUNT,
                    Department = entity.DEPARTMENT,
                    ClientID = entity.CLIENT_ID,
                    RoleName = context.USR_ROLE.FirstOrDefault(t => t.ID == entity.ROLE_ID).NAME,
                    RoleCategory = context.USR_ROLE.FirstOrDefault(t => t.ID == entity.ROLE_ID).ROLE_CATEGORY,
                })
                .ToList();

            return new MultiMessage<GUser>(list, list.Count);
        }


        public static SingleMessage<bool> RecoverGUser(PTMSEntities context, GUser model)
        {

           

            if (string.IsNullOrWhiteSpace(model.Account) || context.USR_GUSER.Any(t => t.ACCOUNT == model.Account && t.VALID == 1))
            {
                return new SingleMessage<bool>(false, CommonErrorMessage.AccountExist);
            }       
          
           
            var result = (from x in context.USR_GUSER
                          where x.ID == model.ID
                          select x).FirstOrDefault();
            if (result != null)
            {
                result.VALID = 1;
                int i = context.SaveChanges();
                if (i > 0)
                {
                    return new SingleMessage<bool>(true);
                }
                else
                {
                    return new SingleMessage<bool>(false);
                }
            }
            else
            {
                return new SingleMessage<bool>(false);
            }
        }

    }
}
