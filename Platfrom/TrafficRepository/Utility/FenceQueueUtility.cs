using Gsafety.PTMS.Common.Data;
using Gsafety.PTMS.DBEntity;
using System;
using System.Collections.Generic;
namespace Gsafety.PTMS.Traffic.Repository
{
    public class FenceQueueUtility
    {
        public static TRF_FENCE_QUEUE UpdateEntity(TRF_FENCE_QUEUE entity, FenceQueue model, bool isAdd)
        {
            if (isAdd)
            {
                entity.ID = model.ID;
                entity.CLIENT_ID = model.ClientID;
            }
            entity.FENCE_ID = model.FenceID;
            entity.CLIENT_ID = model.ClientID;
            entity.VEHICLE_ID = model.VehicleID;
            entity.NAME = model.Name;
            entity.FENCE_TYPE = model.FenceType;
            entity.MDVR_CORE_SN = model.MdvrCoreSn;
            entity.RESULT_PACKET = model.ResultPacket;
            entity.PTS = model.Pts;
            entity.RADIUS = model.Radius;
            entity.CIRCLE_CENTER = model.CircleCenter;
            entity.CREATE_TIME = model.CreateTime;
            entity.PACKET_SEQ = (int)model.PacketSeq;
            entity.SEND_TIME = model.CreateTime;
            if (model.SendTime.HasValue)
            {
                entity.SEND_TIME = model.SendTime.Value;
            }
            entity.STATUS = model.Status;
            entity.OPER_TYPE = model.OperType;
            entity.MAX_SPEED = (short)model.MaxSpeed;
            entity.OVER_SPEED_DURATION = model.OverSpeedDuration;
            entity.POINT_COUNT = (short)model.PointCount;
            entity.END_TIME = model.EndTime;
            entity.START_TIME = model.StartTime;
            entity.REGION_PROPERTY = model.RegionProperty;
            return entity;
        }

        public static FenceQueue GetModel(TRF_FENCE_QUEUE entity)
        {
            FenceQueue model = new FenceQueue();
            model.ID = entity.ID;
            model.ClientID = entity.CLIENT_ID;
            model.FenceID = entity.FENCE_ID;
            model.ClientID = entity.CLIENT_ID;
            model.VehicleID = entity.VEHICLE_ID;
            model.Name = entity.NAME;
            if (entity.FENCE_TYPE.HasValue)
                model.FenceType = entity.FENCE_TYPE.Value;
            model.MdvrCoreSn = entity.MDVR_CORE_SN;
            model.ResultPacket = entity.RESULT_PACKET;
            model.Pts = entity.PTS;
            if (entity.RADIUS.HasValue)
                model.Radius = entity.RADIUS.Value;
            model.CircleCenter = entity.CIRCLE_CENTER;
            if (entity.CREATE_TIME.HasValue)
                model.CreateTime = entity.CREATE_TIME.Value;
            if (entity.PACKET_SEQ.HasValue)
                model.PacketSeq = entity.PACKET_SEQ.Value;
            model.SendTime = entity.SEND_TIME;
            if (entity.STATUS.HasValue)
                model.Status = entity.STATUS.Value;
            model.OperType = entity.OPER_TYPE;
            if (entity.MAX_SPEED.HasValue)
                model.MaxSpeed = entity.MAX_SPEED.Value;
            if (entity.OVER_SPEED_DURATION.HasValue)
                model.OverSpeedDuration = entity.OVER_SPEED_DURATION.Value;
            if (entity.POINT_COUNT.HasValue)
                model.PointCount = entity.POINT_COUNT.Value;
            model.EndTime = entity.END_TIME;
            model.StartTime = entity.START_TIME;
            model.RegionProperty = entity.REGION_PROPERTY;


            return model;
        }

    }
}

