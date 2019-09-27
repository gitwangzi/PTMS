using Gsafety.PTMS.Common.Data;
using Gsafety.PTMS.DBEntity;
using System;
using System.Collections.Generic;
namespace Gsafety.PTMS.Traffic.Repository
{
    public class TrafficFenceUtility
    {

        public static TRF_FENCE UpdateEntity(TRF_FENCE entity, TrafficFence model, bool isAdd)
        {
            if (isAdd)
            {
                entity.ID = model.ID;
                entity.CLIENT_ID = model.ClientID;
            }
            entity.NAME = model.Name;
            entity.FENCE_TYPE = (short)model.FenceType;
            //entity.ALERT_TYPE = (short)model.AlertType;
            //entity.SPEED_LIMIT = model.SpeedLimit;
            //entity.TIME_LIMIT = model.TimeLimit;

            entity.END_TIME = model.EndTime;
            entity.MAX_SPEED = (short)model.MaxSpeed;
            entity.OVER_SPEED_DURATION = model.OverSpeedDuration;
            entity.POINT_COUNT = (short)model.PointCount;
            entity.REGION_PROPERTY = model.RegionProperty;
            entity.START_TIME = model.StartTime;
            entity.PTS = model.Pts;
            entity.RADIUS = model.Radius;
            entity.CIRCLE_CENTER = model.CircleCenter;
            entity.SHAPE = model.Shape;
            entity.CREATOR = model.Creator;
            entity.CREATE_TIME = model.CreateTime;
            entity.ADDRESS = model.Address;
            if (model.Valid)
                entity.VALID = 1;
            else
                entity.VALID = 0;
            return entity;
        }

        public static TrafficFence GetModel(TRF_FENCE entity)
        {
            TrafficFence model = new TrafficFence();
            model.ID = entity.ID;
            model.ClientID = entity.CLIENT_ID;
            model.Name = entity.NAME;
            model.FenceType = entity.FENCE_TYPE.Value;
            //model.AlertType = entity.ALERT_TYPE.Value;
            //model.SpeedLimit = entity.SPEED_LIMIT;
            //model.TimeLimit = entity.TIME_LIMIT;

            model.EndTime = entity.END_TIME;
            if (entity.MAX_SPEED.HasValue)
                model.MaxSpeed = entity.MAX_SPEED.Value;
            if (entity.OVER_SPEED_DURATION.HasValue)
                model.OverSpeedDuration = entity.OVER_SPEED_DURATION.Value;
            if (entity.POINT_COUNT.HasValue)
                model.PointCount = entity.POINT_COUNT.Value;

            model.RegionProperty = entity.REGION_PROPERTY;
            model.StartTime = entity.START_TIME;

            model.Pts = entity.PTS;
            model.Radius = entity.RADIUS.Value;
            model.CircleCenter = entity.CIRCLE_CENTER;
            model.Shape = entity.SHAPE;
            model.Address = entity.ADDRESS;
            model.Creator = entity.CREATOR;
            model.CreateTime = entity.CREATE_TIME.Value;
            if (entity.VALID == 0)
            {
                model.Valid = false;
            }
            else
            {
                model.Valid = true;
            }
            return model;
        }

    }
}

