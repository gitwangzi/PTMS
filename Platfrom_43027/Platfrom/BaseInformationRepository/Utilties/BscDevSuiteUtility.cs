using Gsafety.PTMS.BaseInfo;
using Gsafety.PTMS.BaseInformation.Contract.Data;
using Gsafety.PTMS.Common.Data;
using Gsafety.PTMS.Common.Enum;
using Gsafety.PTMS.DBEntity;
using System;
using System.Collections.Generic;
namespace Gsafety.PTMS.BaseInformation.Repository
{
    public class BscDevSuiteUtility
    {
        public static void UpdateEntity(BSC_DEV_SUITE entity, DevSuite model, bool isAdd)
        {
            if (isAdd)
            {
                entity.SUITE_INFO_ID = model.SuiteInfoID;
                entity.CLIENT_ID = model.ClientID;
                entity.CREATE_TIME = model.CreateTime;
                entity.VALID = (short)ValidEnum.Valid;
                entity.STATUS = (short)model.Status;
            }
            entity.SUITE_ID = model.SuiteID;
            entity.MDVR_CORE_SN = model.MdvrCoreSn;
            entity.MDVR_SN = model.MdvrSn;
            entity.MDVR_SIM = model.MdvrSim;
            entity.MDVR_SIM_MOBILE = model.MdvrSimMobile;
            entity.SD_SN = model.SdSn;
            entity.SOFTWARE_VERSION = model.SoftwareVersion;
            entity.MODEL = model.Model;
            entity.PROTOCOL = (short)model.Protocol;
            entity.NOTE = model.Note;
            entity.UPS_SN = model.UpsSn;
        }

        public static DevSuite GetModel(BSC_DEV_SUITE entity)
        {
            DevSuite model = new DevSuite();
            model.SuiteInfoID = entity.SUITE_INFO_ID;
            model.ClientID = entity.CLIENT_ID;
            model.SuiteID = entity.SUITE_ID;
            model.MdvrCoreSn = entity.MDVR_CORE_SN;
            model.MdvrSn = entity.MDVR_SN;
            model.MdvrSim = entity.MDVR_SIM;
            model.MdvrSimMobile = entity.MDVR_SIM_MOBILE;
            model.SdSn = entity.SD_SN;
            model.SoftwareVersion = entity.SOFTWARE_VERSION;
            model.Model = entity.MODEL;
            model.Protocol = (ProtocolTypeEnum)entity.PROTOCOL;
            model.Status = entity.STATUS;
            model.Note = entity.NOTE;
            model.UpsSn = entity.UPS_SN;
            model.CreateTime = entity.CREATE_TIME;
            switch (entity.STATUS)
            {
                case (short)DeviceSuiteStatus.Initial:
                    model.InstallStatus = InstallStatusType.UnInstall;
                    break;
                    break;
                default:
                    model.InstallStatus = InstallStatusType.Installed;
                    break;
            }
            return model;
        }

    }
}

