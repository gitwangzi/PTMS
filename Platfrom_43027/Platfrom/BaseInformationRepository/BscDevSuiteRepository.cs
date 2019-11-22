using Gsafety.PTMS.Base.Contract.Data;
using Gsafety.PTMS.BaseInfo;
using Gsafety.PTMS.BaseInformation.Contract.Data;
using Gsafety.PTMS.Common.Data;
using Gsafety.PTMS.Common.Enum;
using Gsafety.PTMS.DBEntity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Gsafety.PTMS.BaseInformation.Repository
{
    ///<summary>
    ///安全套件
    ///</summary>
    public class BscDevSuiteRepository
    {
        private static string _parameterCannotBeNull = "NotNull";
        private static string _DevSuiteNotExist = "NoExit";

        /// <summary>
        /// 添加安全套件
        /// </summary>
        /// <param name="model">安全套件</param>
        public static SingleMessage<bool> InsertBscDevSuite(PTMSEntities context, DevSuite model)
        {
            if (string.IsNullOrWhiteSpace(model.SuiteInfoID))
            {
                return new SingleMessage<bool>(false, "model.SuiteInfoID" + _parameterCannotBeNull);
            }

            if (string.IsNullOrWhiteSpace(model.ClientID))
            {
                return new SingleMessage<bool>(false, "model.ClientID" + _parameterCannotBeNull);
            }

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
            var entity = new BSC_DEV_SUITE();
            BscDevSuiteUtility.UpdateEntity(entity, model, true);

            context.BSC_DEV_SUITE.Add(entity);


            return context.Save();
        }

        /// <summary>
        /// 修改安全套件
        /// </summary>
        public static SingleMessage<bool> UpdateBscDevSuite(PTMSEntities context, DevSuite model)
        {
            bool reference = context.MTN_INSTALLATION_DETAIL.Any(n => n.VALID == 1 && n.SUITE_INFO_ID == model.SuiteInfoID);
            if (reference)
            {
                return new SingleMessage<bool>(false, "SuiteReferenced");
            }
            if (string.IsNullOrWhiteSpace(model.SuiteInfoID))
            {
                return new SingleMessage<bool>(false, "model.SuiteInfoID" + _parameterCannotBeNull);
            }

            var entity = context.BSC_DEV_SUITE.FirstOrDefault(t => t.VALID == (short)ValidEnum.Valid && t.SUITE_INFO_ID == model.SuiteInfoID);
            if (null == entity)
            {
                return new SingleMessage<bool>(false, _DevSuiteNotExist);
            }
            bool exist = context.BSC_DEV_SUITE.Any(n => n.VALID == 1 && n.SUITE_ID == model.SuiteID && n.SUITE_INFO_ID != model.SuiteInfoID);
            if (exist)
            {
                return new SingleMessage<bool>(false, "SuiteIDDuplicate");
            }

            exist = context.BSC_DEV_SUITE.Any(n => n.VALID == 1 && n.MDVR_CORE_SN == model.MdvrCoreSn && n.SUITE_INFO_ID != model.SuiteInfoID);
            if (exist)
            {
                return new SingleMessage<bool>(false, "MDVRCoreSNDuplicate");
            }
            if (model.MdvrSim != null && model.MdvrSim != "")
            {
                exist = context.BSC_DEV_SUITE.Any(n => n.VALID == 1 && n.MDVR_SIM == model.MdvrSim && n.SUITE_INFO_ID != model.SuiteInfoID);
                if (exist)
                {
                    return new SingleMessage<bool>(false, "MdvrSimDuplicate");
                }
            }
            BscDevSuiteUtility.UpdateEntity(entity, model, false);
            context.Entry(entity).State = EntityState.Modified;

            return context.Save();
        }

        /// <summary>
        /// 删除安全套件
        /// </summary>
        public static SingleMessage<bool> DeleteBscDevSuiteByID(PTMSEntities context, string SuiteInfoID)
        {
            if (string.IsNullOrWhiteSpace(SuiteInfoID))
            {
                return new SingleMessage<bool>(false, "SuiteInfoID " + _parameterCannotBeNull);
            }
            bool reference = context.MTN_INSTALLATION_DETAIL.Any(n => n.VALID == 1 && n.SUITE_INFO_ID == SuiteInfoID);
            if (reference)
            {
                return new SingleMessage<bool>(false, "SuiteReferenced");
            }
            var entity = context.BSC_DEV_SUITE.FirstOrDefault(t => t.SUITE_INFO_ID == SuiteInfoID && t.VALID == (short)ValidEnum.Valid);
            if (entity == null)
            {
                return new SingleMessage<bool>(false, _DevSuiteNotExist);
            }

            entity.VALID = (short)ValidEnum.UnValid;

            var parts = context.BSC_DEV_SUITE_PART.Where(t => t.SUITE_INFO_ID == SuiteInfoID && t.VALID == (short)ValidEnum.Valid).ToList();
            parts.ForEach(part => part.VALID = (short)ValidEnum.UnValid);

            return context.Save();
        }

        /// <summary>
        /// 获取安全套件
        /// </summary>
        public static SingleMessage<DevSuite> GetBscDevSuite(PTMSEntities context, string SuiteInfoID)
        {
            if (string.IsNullOrWhiteSpace(SuiteInfoID))
            {
                return new SingleMessage<DevSuite>(false, "SuiteInfoID " + _parameterCannotBeNull);
            }

            var entity = context.BSC_DEV_SUITE.FirstOrDefault(t => t.SUITE_INFO_ID == SuiteInfoID && t.VALID == (short)ValidEnum.Valid);
            if (entity == null)
            {
                return new SingleMessage<DevSuite>(false, _DevSuiteNotExist);
            }

            var model = BscDevSuiteUtility.GetModel(entity);

            var parts = context.BSC_DEV_SUITE_PART.Where(t => t.SUITE_INFO_ID == SuiteInfoID && t.VALID == (short)ValidEnum.Valid).ToList();
            parts.ForEach(part => model.BscDevSuiteParts.Add(BscDevSuitePartUtility.GetModel(part)));

            return new SingleMessage<DevSuite>(model);
        }

        /// <summary>
        /// 获取安全套件
        /// </summary>
        public static SingleMessage<DevSuite> GetDevSuiteBySuiteID(PTMSEntities context, string SuiteID)
        {
            if (string.IsNullOrWhiteSpace(SuiteID))
            {
                return new SingleMessage<DevSuite>(false, "SuiteInfoID " + _parameterCannotBeNull);
            }

            var entity = context.BSC_DEV_SUITE.FirstOrDefault(t => t.SUITE_ID == SuiteID && t.VALID == (short)ValidEnum.Valid);
            if (entity == null)
            {
                return new SingleMessage<DevSuite>(false, _DevSuiteNotExist);
            }

            var model = BscDevSuiteUtility.GetModel(entity);

            var parts = context.BSC_DEV_SUITE_PART.Where(t => t.SUITE_INFO_ID == entity.SUITE_INFO_ID && t.VALID == (short)ValidEnum.Valid).ToList();
            parts.ForEach(part => model.BscDevSuiteParts.Add(BscDevSuitePartUtility.GetModel(part)));

            return new SingleMessage<DevSuite>(model);
        }

        /// <summary>
        /// 获取安全套件
        /// </summary>
        public static MultiMessage<DevSuite> GetBscDevSuiteList(PTMSEntities context, string clientID, InstallStatusType? installStatus, string vehicleSn, string suitID, string mdvrCoreSn, string mdvrSn, string mdvrSim, int pageIndex = 1, int pageSize = 10)
        {
            var result = from x in context.BSC_SUITE_VIEW.Where(item => item.VALID == 1 && item.CLIENT_ID == clientID)
                         where (string.IsNullOrEmpty(suitID) ? true : x.SUITE_ID.ToUpper().Contains(suitID.ToUpper()))
                         && (string.IsNullOrEmpty(vehicleSn) ? true : x.VEHICLE_ID.ToUpper().Contains(vehicleSn.ToUpper()))
                         && (string.IsNullOrEmpty(mdvrCoreSn) ? true : x.MDVR_CORE_SN.ToUpper().Contains(mdvrCoreSn.ToUpper()))
                         && (string.IsNullOrEmpty(mdvrSn) ? true : x.MDVR_SN.ToUpper().Contains(mdvrSn.ToUpper()))
                         && (string.IsNullOrEmpty(mdvrSim) ? true : x.MDVR_SIM.ToUpper().Contains(mdvrSim.ToUpper()))
                         select x;
            if (installStatus.HasValue)
            {
                if (installStatus.Value == InstallStatusType.UnInstall)
                {
                    result = result.Where(n => n.STATUS == (short)DeviceSuiteStatus.Initial);
                }
                else if (installStatus.Value == InstallStatusType.Installing)
                {
                    result = result.Where(n => n.CHECKSTEP.HasValue && n.CHECKSTEP < 7);
                }
                else if (installStatus.Value == InstallStatusType.Installed)
                {
                    result = result.Where(n => n.CHECKSTEP.HasValue && n.CHECKSTEP == 7);
                }
            }


            int totalCount;
            var list = result.Page(out totalCount, pageIndex, pageSize, true, n => n.CREATE_TIME, false).ToList();

            List<DevSuite> items = new List<DevSuite>();

            foreach (var item in list)
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

            return new MultiMessage<DevSuite>(items, totalCount);
        }

        /// <summary>
        /// 根据车牌号查询安全套件号
        /// </summary>
        /// <param name="context"></param>
        /// <param name="vehicleSN"></param>
        /// <returns></returns>
        public static SingleMessage<string> GetBscDevSuiteIDByVehicleSN(PTMSEntities context, string vehicleSN)
        {
            if (string.IsNullOrWhiteSpace(vehicleSN))
            {
                return new SingleMessage<string>(false, "vehicleSN " + _parameterCannotBeNull);
            }

            var entity = context.BSC_SUITE_VIEW.FirstOrDefault(t => t.VEHICLE_ID == vehicleSN && t.VALID == (short)ValidEnum.Valid);
            if (entity == null)
            {
                return new SingleMessage<string>(false, _DevSuiteNotExist);
            }

            return new SingleMessage<string>(entity.SUITE_ID);
        }

        //*********************************************************************


        public static SingleMessage<bool> InsertBscDevSuitePart(PTMSEntities context, DevSuitePart model)
        {
            if (string.IsNullOrWhiteSpace(model.SuiteInfoID))
            {
                return new SingleMessage<bool>(false, "model.SuiteInfoID" + _parameterCannotBeNull);
            }

            var entity = new BSC_DEV_SUITE();

            var result = context.BSC_DEV_SUITE.Where(x => x.SUITE_INFO_ID == model.SuiteInfoID).FirstOrDefault();
            if (result != null)
            {
                var partyEntity = new BSC_DEV_SUITE_PART();
                BscDevSuitePartUtility.UpdateEntity(partyEntity, model, true);

                context.BSC_DEV_SUITE_PART.Add(partyEntity);

                return context.Save();
            }
            else
            {
                return new SingleMessage<bool>(false, "model.SuiteInfoID" + _parameterCannotBeNull);
            }
        }

        public static SingleMessage<bool> UpdateBscDevSuitePart(PTMSEntities context, DevSuitePart model)
        {
            var partyEntity = context.BSC_DEV_SUITE_PART.FirstOrDefault(t => t.ID == model.ID);
            BscDevSuitePartUtility.UpdateEntity(partyEntity, model, false);
            context.Entry(partyEntity).State = EntityState.Modified;
            return context.Save();
        }

        public static SingleMessage<bool> DeleteBscDevSuitePartByID(PTMSEntities context, string partID)
        {
            if (!string.IsNullOrEmpty(partID))
            {
                BSC_DEV_SUITE_PART part = context.BSC_DEV_SUITE_PART.FirstOrDefault(n => n.ID == partID);
                if (part != null)
                {
                    context.BSC_DEV_SUITE_PART.Remove(part);
                    return context.Save();
                }

            }
            return new SingleMessage<bool>(false, "PartIDcantEmptyorNull");
        }

        public static MultiMessage<DevSuitePart> GetCameraListBySuiteInfoID(PTMSEntities context, string suitInfoID)
        {
            var list = context.BSC_DEV_SUITE_PART.Where(t => t.SUITE_INFO_ID == suitInfoID && t.PART_TYPE == (short)BscDevSuitePartTypeEnum.Camera).ToList();

            int totalCount = list.Count;
            var items = list.Select(t => BscDevSuitePartUtility.GetModel(t)).ToList();

            return new MultiMessage<DevSuitePart>(items, totalCount);
        }

        public static bool BatchAdd(List<DevSuite> suiteList)
        {
            //int errorIndex = -1;
            using (PTMSEntities context = new PTMSEntities())
            {
                for (int i = 0; i < suiteList.Count; i++)
                {
                    var item = suiteList[i];
                    string suitInfoId = Guid.NewGuid().ToString();
                    context.BSC_DEV_SUITE.Add(new BSC_DEV_SUITE
                    {
                        SUITE_INFO_ID = suitInfoId,
                        CLIENT_ID = item.ClientID,
                        SUITE_ID = item.SuiteID,
                        MDVR_CORE_SN = item.MdvrCoreSn,
                        MDVR_SN = item.MdvrSn,
                        MDVR_SIM = item.MdvrSim,
                        MDVR_SIM_MOBILE = item.MdvrSimMobile,
                        UPS_SN = item.UpsSn,
                        SD_SN = item.SdSn,
                        SOFTWARE_VERSION = item.SoftwareVersion,
                        MODEL = item.Model,
                        PROTOCOL = (short)item.Protocol,
                        STATUS = (short)DeviceSuiteStatus.Initial,
                        NOTE = item.Note,
                        CREATE_TIME = DateTime.UtcNow,
                        VALID = 1
                    });

                    for (int j = 0; j < item.BscDevSuiteParts.Count; j++)
                    {
                        var part = item.BscDevSuiteParts[j];
                        context.BSC_DEV_SUITE_PART.Add(new BSC_DEV_SUITE_PART
                        {
                            ID = Guid.NewGuid().ToString(),
                            SUITE_INFO_ID = suitInfoId,
                            PART_SN = part.PartSn,
                            NAME = part.Name,
                            MODEL = part.Model,
                            PART_TYPE = (short)part.PartType,
                            PRODUCE_TIME = part.ProduceTime,
                            CREATE_TIME = DateTime.UtcNow,
                            VALID = 1
                        });
                    }
                }
                context.SaveChanges();
                return true;
            }
        }

        public static MultiMessage<DevSuite> CheckSecuritySuiteExist(List<DevSuite> deviceSuiteList)
        {
            using (PTMSEntities context = new PTMSEntities())
            {
                var clientID = deviceSuiteList[0].ClientID;

                var currentClientSuiteList = context.BSC_DEV_SUITE
                    .Where(v => v.CLIENT_ID == clientID && v.VALID == (short)ValidEnum.Valid)
                    .Select(t => new
                    {
                        SUITE_ID = t.SUITE_ID,
                        MDVR_CORE_SN = t.MDVR_CORE_SN,
                        MDVR_SN = t.MDVR_SN,
                        SUITE_INFO_ID = t.SUITE_INFO_ID
                    }).ToList();

                var suitInfoIDList = currentClientSuiteList.Select(t => t.SUITE_INFO_ID).ToList();

                var currentClientSuitePartList = context.BSC_DEV_SUITE_PART
                    .Where(t => t.VALID == (short)ValidEnum.Valid && suitInfoIDList.Contains(t.SUITE_INFO_ID))
                    .Select(t => new
                    {
                        PART_SN = t.PART_SN,
                    }).ToList();

                Func<DevSuite, bool> filter = tv => context.BSC_DEV_SUITE.Local.Any(v => v.SUITE_ID == tv.SuiteID || v.MDVR_SN == tv.MdvrSn || v.MDVR_CORE_SN == tv.MdvrCoreSn)
                     || currentClientSuiteList.Any(v => v.SUITE_ID == tv.SuiteID || v.MDVR_SN == tv.MdvrSn || v.MDVR_CORE_SN == tv.MdvrCoreSn);

                var list = deviceSuiteList.Where(filter).ToList();

                Func<DevSuitePart, bool> filterPart = pv => context.BSC_DEV_SUITE_PART.Local.Any(v => v.PART_SN == pv.PartSn)
                     || currentClientSuitePartList.Any(v => v.PART_SN == pv.PartSn);
                List<DevSuite> devSuitList = new List<DevSuite>();
                foreach (var deviceSuite in deviceSuiteList)
                {
                    var checkList = deviceSuite.BscDevSuiteParts.Where(filterPart).ToList();
                    if (checkList.Count > 0)
                    {
                        deviceSuite.BscDevSuiteParts = checkList;
                        devSuitList.Add(deviceSuite);
                    }
                }

                //安全套件与配件都存在重复的
                var shareSuitList = list.Where(x => devSuitList.Select(y => y.SuiteID).Contains(x.SuiteID)).ToList();
                //安全套件重复，配件不重复
                var suit = list.Where(x => !shareSuitList.Select(y => y.SuiteID).Contains(x.SuiteID)).ToList();
                //安全套件不重复，配件重复
                var part = devSuitList.Where(x => !shareSuitList.Select(y => y.SuiteID).Contains(x.SuiteID)).ToList();

                foreach (var item in suit)
                {
                    item.Note = "0";
                }
                foreach (var item in part)
                {
                    item.Note = "1";
                }

                foreach (var item in shareSuitList)
                {
                    item.Note = "2";
                }

                var temp = suit.Union(part).Union(shareSuitList).ToList();
                return new MultiMessage<DevSuite>(temp, temp.Count);
            }
        }

        public static MultiMessage<DevSuite> GetBscDevSuiteExportList(PTMSEntities context, string clientID, InstallStatusType? installStatus, string vehicleSn, string suitID, string mdvrCoreSn, string mdvrSn, string mdvrSim, int pageIndex = 1, int pageSize = 10)
        {
            var result = from x in context.BSC_SUITE_VIEW.Where(item => item.VALID == 1 && item.CLIENT_ID == clientID)
                         where (string.IsNullOrEmpty(suitID) ? true : x.SUITE_ID.ToUpper().Contains(suitID.ToUpper()))
                         && (string.IsNullOrEmpty(vehicleSn) ? true : x.VEHICLE_ID.ToUpper().Contains(vehicleSn.ToUpper()))
                         && (string.IsNullOrEmpty(mdvrCoreSn) ? true : x.MDVR_CORE_SN.ToUpper().Contains(mdvrCoreSn.ToUpper()))
                         && (string.IsNullOrEmpty(mdvrSn) ? true : x.MDVR_SN.ToUpper().Contains(mdvrSn.ToUpper()))
                         && (string.IsNullOrEmpty(mdvrSim) ? true : x.MDVR_SIM.ToUpper().Contains(mdvrSim.ToUpper()))
                         select x;

            if (installStatus.HasValue)
            {
                if (installStatus.Value == InstallStatusType.UnInstall)
                {
                    result = result.Where(n => n.STATUS == (short)DeviceSuiteStatus.Initial);
                }
                else if (installStatus.Value == InstallStatusType.Installing)
                {
                    result = result.Where(n => n.CHECKSTEP.HasValue && n.CHECKSTEP < 7);
                }
                else if (installStatus.Value == InstallStatusType.Installed)
                {
                    result = result.Where(n => n.CHECKSTEP.HasValue && n.CHECKSTEP == 7);
                }
            }

            int totalCount;
            var list = result.Page(out totalCount, pageIndex, pageSize, true, n => n.CREATE_TIME, false).ToList();
            List<DevSuite> items = new List<DevSuite>();
            foreach (var item in list)
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
                suite.BscDevSuiteParts = context.BSC_DEV_SUITE_PART.Where(t =>
                    t.SUITE_INFO_ID == item.SUITE_INFO_ID).Select(u => new DevSuitePart()
                    {
                        ID = u.ID,
                        SuiteInfoID = u.SUITE_INFO_ID,
                        PartSn = u.PART_SN,
                        Name = u.NAME,
                        Model = u.MODEL,
                        ProduceTime = u.PRODUCE_TIME,
                        PartType = (BscDevSuitePartTypeEnum)u.PART_TYPE
                    }).ToList();
                items.Add(suite);
            }

            return new MultiMessage<DevSuite>(items, totalCount);
        }
    }
}

