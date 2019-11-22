using Gsafety.PTMS.BaseInfo;
using Gsafety.PTMS.BaseInformation.Contract.Data;
using Gsafety.PTMS.Common.Data;
using Gsafety.PTMS.Common.Enum;
using Gsafety.PTMS.DBEntity;
using System;
using System.Collections.Generic;
namespace Gsafety.PTMS.BaseInformation.Repository
{
    public class BscDevSuitePartUtility
    {
        public static void UpdateEntity(BSC_DEV_SUITE_PART entity, DevSuitePart model, bool isAdd)
        {
            if (isAdd)
            {
                entity.ID = model.ID;
                entity.SUITE_INFO_ID = model.SuiteInfoID;
                entity.VALID = (short)ValidEnum.Valid;
                entity.CREATE_TIME = model.CreateTime;
            }

            entity.PART_SN = model.PartSn;
            entity.NAME = model.Name;
            entity.MODEL = model.Model;
            entity.PART_TYPE = (short)model.PartType;
            entity.PRODUCE_TIME = model.ProduceTime;
        }

        public static DevSuitePart GetModel(BSC_DEV_SUITE_PART entity)
        {
            DevSuitePart model = new DevSuitePart();
            model.ID = entity.ID;
            model.PartSn = entity.PART_SN;
            model.Name = entity.NAME;
            model.Model = entity.MODEL;
            model.PartType = (BscDevSuitePartTypeEnum)entity.PART_TYPE;
            model.ProduceTime = entity.PRODUCE_TIME;
            model.SuiteInfoID = entity.SUITE_INFO_ID;
            model.CreateTime = entity.CREATE_TIME;
            return model;
        }

    }
}

