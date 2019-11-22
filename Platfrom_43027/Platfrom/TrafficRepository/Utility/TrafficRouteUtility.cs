using Gsafety.PTMS.Common.Data;
using Gsafety.PTMS.DBEntity;
using System;
using System.Collections.Generic;
namespace Gsafety.PTMS.Traffic.Repository
{
    public class TrafficRouteUtility
    {
        public static TRF_ROUTE UpdateEntity(TRF_ROUTE entity, TrafficRoute model, bool isAdd)
        {
            if (isAdd)
            {
                entity.ID = model.ID;
                entity.CLIENT_ID = model.ClientID;
            }
            entity.MAX_SPEED = model.MaxSpeed;
            entity.NAME = model.Name;
            entity.PTS = model.Pts;
            entity.ROUTE_SEGMENT_PROPERTY = model.RouteSegmentProperty;
            entity.WIDTH = model.Width;
            entity.ROUTE_PROPERTY = model.RouteProperty;
            entity.OVER_SPEED_DURATION = model.OverSpeedDuration;
            entity.START_TIME = model.StartTime;
            entity.END_TIME = model.EndTime;
            entity.CREATOR = model.Creator;
            entity.CREATE_TIME = model.CreateTime;
            entity.POINT_COUNT = (short)model.PointCount;

            if (model.Valid)
                entity.VALID = 1;
            else
                entity.VALID = 0;
            return entity;
        }

        public static TrafficRoute GetModel(TRF_ROUTE entity)
        {
            TrafficRoute model = new TrafficRoute();
            model.ID = entity.ID;
            model.ClientID = entity.CLIENT_ID;
            model.EndTime = entity.END_TIME;
            if (entity.MAX_SPEED.HasValue)
                model.MaxSpeed = entity.MAX_SPEED.Value;
            model.RouteSegmentProperty = entity.ROUTE_SEGMENT_PROPERTY;
            if (entity.WIDTH.HasValue)
                model.Width = entity.WIDTH.Value;
            model.RouteProperty = entity.ROUTE_PROPERTY;
            if (entity.OVER_SPEED_DURATION.HasValue)
                model.OverSpeedDuration = entity.OVER_SPEED_DURATION.Value;
            model.StartTime = entity.START_TIME;

            if (entity.POINT_COUNT.HasValue)
                model.TurningCount = entity.POINT_COUNT.Value;

            model.Name = entity.NAME;
            model.Pts = entity.PTS;
            model.Creator = entity.CREATOR;
            model.CreateTime = entity.CREATE_TIME.Value;
            model.PointCount = entity.POINT_COUNT.Value;
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

