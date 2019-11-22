using Gsafety.PTMS.Common.Data;
using Gsafety.PTMS.Common.Data.Enum;
using Gsafety.PTMS.DBEntity;
using Gsafety.PTMS.Manager.Contract.Data.CommandManage;
using System;
using System.Collections.Generic;
namespace Gsafety.PTMS.Manager.Repository
{
    public class LocationReportRuleUtility
    {

        public static TRF_COMMAND_PARAM UpdateEntity(TRF_COMMAND_PARAM entity, LocationReportRule model, bool isAdd)
        {
            if (isAdd)
            {
                entity.ID = model.ID;
            }
            entity.CLIENT_ID = model.ClientID;
            entity.REPORT_STRATEGY = (short)model.ReportStrategy;
            entity.LOCATION_INTERVAL = model.Interval;
            entity.LOCATION_LENGTH = model.Length;
            entity.CREATOR = model.Creator;
            entity.CREATE_TIME = model.CreateTime;
            entity.NAME = model.Name;
            entity.TYPE = (short)CommandParaEnum.ReportStrategy;
            if (model.Valid == 1)
                entity.VALID = 1;
            else
                entity.VALID = 0;
            return entity;
        }

        public static LocationReportRule GetModel(TRF_COMMAND_PARAM entity)
        {
            LocationReportRule model = new LocationReportRule();
            model.ID = entity.ID;
            model.ClientID = entity.CLIENT_ID;
            model.ReportStrategy = (short)entity.REPORT_STRATEGY;
            model.Interval = (int)entity.LOCATION_INTERVAL;
            model.Length = (int)entity.LOCATION_LENGTH;
            model.Creator = entity.CREATOR;
            if (entity.CREATE_TIME.HasValue)
                model.CreateTime = entity.CREATE_TIME.Value;
            model.Name = entity.NAME;
            if (entity.VALID == 0)
            {
                model.Valid = 0;
            }
            else
            {
                model.Valid = 1;
            }
            return model;
        }

    }
}

