using Gsafety.PTMS.Common.Data;
using Gsafety.PTMS.DBEntity;
using System;
using System.Collections.Generic;
namespace Gsafety.PTMS.Traffic.Repository
{
    public class RouteQueueUtility
    {

        public static TRF_ROUTE_QUEUE UpdateEntity(TRF_ROUTE_QUEUE entity, RouteQueue model, bool isAdd)
        {
            if (isAdd)
            {
                entity.ID = model.ID;
                entity.CLIENT_ID = model.ClientID;
            }
            entity.RESULT_PACKET = model.ResultPacket;
            entity.PACKET_SEQ = (int)model.PacketSeq;
            entity.VEHICLE_ID = model.VehicleID;
            entity.END_TIME = model.EndTime;
            entity.REGION_ID = (short)model.RegionID;
            entity.SEND_TIME = model.CreateTime;
            if (model.SendTime.HasValue)
            {
                entity.SEND_TIME = model.SendTime.Value;
            }
            entity.MDVR_CORE_SN = model.MdvrCoreSn;
            entity.OPER_TYPE = (short)model.OperType;
            entity.STATUS = (short)model.Status;
            entity.CREATE_TIME = model.CreateTime;
            entity.START_TIME = model.StartTime;
            entity.PTS = model.Pts;
            entity.WIDTH = (short)model.Width;
            entity.ROUTE_PROPERTY = model.RouteProperty;
            entity.ROUTE_SEGMENT_PROPERTY = model.RouteSegmentProperty;
            entity.ROUTE_ID = model.RouteID;
            entity.MAX_SPEED = (short)model.MaxSpeed;
            entity.NAME = model.Name;
            entity.OVER_SPEED_DURATION = (short)model.OverSpeedDuration;
            entity.POINT_COUNT = (short)model.PointCount;
            return entity;
        }

        public static RouteQueue GetModel(TRF_ROUTE_QUEUE entity)
        {
            RouteQueue model = new RouteQueue();
            model.ID = entity.ID;
            model.ClientID = entity.CLIENT_ID;
            model.ResultPacket = entity.RESULT_PACKET;
            if (entity.PACKET_SEQ.HasValue)
                model.PacketSeq = entity.PACKET_SEQ.Value;
            model.VehicleID = entity.VEHICLE_ID;
            model.SendTime = entity.SEND_TIME;
            model.MdvrCoreSn = entity.MDVR_CORE_SN;
            model.OperType = entity.OPER_TYPE;
            model.Pts = entity.PTS;
            model.RouteProperty = entity.ROUTE_PROPERTY;
            model.RouteSegmentProperty = entity.ROUTE_SEGMENT_PROPERTY;
            model.RouteID = entity.ROUTE_ID;
            model.Name = entity.NAME;

            model.EndTime = entity.END_TIME;
            if (entity.REGION_ID.HasValue)
                model.RegionID = entity.REGION_ID.Value;
            if (entity.STATUS.HasValue)
                model.Status = entity.STATUS.Value;

            model.StartTime = entity.START_TIME;
            if (entity.WIDTH.HasValue)
                model.Width = entity.WIDTH.Value;
            if (entity.MAX_SPEED.HasValue)
                model.MaxSpeed = entity.MAX_SPEED.Value;
            if (entity.OVER_SPEED_DURATION.HasValue)
                model.OverSpeedDuration = entity.OVER_SPEED_DURATION.Value;
            if (entity.POINT_COUNT.HasValue)
                model.PointCount = entity.POINT_COUNT.Value;
            if (entity.CREATE_TIME.HasValue)
                model.CreateTime = entity.CREATE_TIME.Value;

            return model;
        }

    }
}

