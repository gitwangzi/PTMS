using Gsafety.Common.Logging;
using Gsafety.Common.Util;
using Gsafety.MQ;
using Gsafety.PTMS.Common.Data;
using Gsafety.PTMS.Common.Data.Enum;
using Gsafety.PTMS.DBEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gsafety.PTMS.AnalysisLib.Command
{
    public class TrafficeFenceCommand : CommandBase
    {
        public override void Send(DBEntity.PTMSEntities context)
        {
            var sending = from fq in context.TRF_FENCE_QUEUE
                          where fq.STATUS == (short)CommandStateEnum.Delivering
                          select fq.ID;
            List<string> sendinglist = sending.ToList();

            var hvs = from fq in context.TRF_FENCE_QUEUE
                      join w in context.RUN_SUITE_WORKING on fq.VEHICLE_ID equals w.VEHICLE_ID
                      where (fq.STATUS == (short)CommandStateEnum.WaitForDeliver || fq.STATUS == (short)CommandStateEnum.Failed) && w.ONLINE_FLAG == 1
                      group fq by fq.VEHICLE_ID into grp
                      let createtime = grp.Min(n => n.CREATE_TIME)
                      from row in grp
                      where row.CREATE_TIME == createtime
                      select row;


            List<TRF_FENCE_QUEUE> tosendlist = hvs.ToList();

            foreach (var item in tosendlist)
            {
                if (CommandBase.ShouldRun())
                {
                    try
                    {
                        if (item.OPER_TYPE == 0)
                        {
                            PolygonsRegion pg = new PolygonsRegion();
                            pg.RegionID = item.REGION_ID.Value;
                            string[] fields = item.REGION_PROPERTY.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

                            pg.RegionProperty = new List<int>();
                            foreach (string field in fields)
                            {
                                pg.RegionProperty.Add(Convert.ToInt32(field));
                            }

                            if (pg.RegionProperty.Contains(0))
                            {
                                if (!string.IsNullOrEmpty(item.START_TIME))
                                {
                                    pg.StartTime = item.START_TIME;
                                }

                                if (!string.IsNullOrEmpty(item.END_TIME))
                                {
                                    pg.EndTime = item.END_TIME;
                                }
                            }
                            else
                            {
                                pg.StartTime = string.Empty;
                                pg.EndTime = string.Empty;
                            }

                            if (pg.RegionProperty.Contains(1))
                            {
                                pg.MaxSpeed = item.MAX_SPEED.Value;
                                pg.OverSpeedDuration = item.OVER_SPEED_DURATION.Value;
                            }

                            pg.PointsList = new List<Point>();
                            string[] points = item.PTS.Split(";".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                            foreach (string ptspoint in points)
                            {
                                string[] coordinates = ptspoint.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                                Point point = new Point();
                                point.Latitude = Math.Abs(Convert.ToDouble(coordinates[1])).ToString();
                                point.Longitude = Math.Abs(Convert.ToDouble(coordinates[0])).ToString();

                                pg.PointsList.Add(point);
                            }

                            pg.PointCount = pg.PointsList.Count;

                            pg.RegionCount = 1;

                            pg.UID = item.MDVR_CORE_SN;

                            string json = ConvertHelper.ConvertModelToJson(pg);
                            byte[] sendbytes = System.Text.UnicodeEncoding.UTF8.GetBytes(json);

                            item.PACKET_SEQ = pg.SerialNo;
                            item.SEND_TIME = DateTime.Now.ToUniversalTime();
                            item.STATUS = (short)CommandStateEnum.Delivering;

                            TransforMessage.PublishMessage(Constdefine.MDVREXCHANGE, "MDVR.PolygonsRegion." + pg.UID, sendbytes);

                            context.SaveChanges();
                        }
                        else if (item.OPER_TYPE == 2)
                        {
                            DelPolygonsRegion dpr = new DelPolygonsRegion();
                            dpr.SerialNo = SerialNoHelper.Create();
                            dpr.RegionCount = 1;
                            dpr.RegionList = new List<int>();
                            dpr.RegionList.Add(item.REGION_ID.Value);
                            dpr.UID = item.MDVR_CORE_SN;

                            string json = ConvertHelper.ConvertModelToJson(dpr);
                            byte[] sendbytes = System.Text.UnicodeEncoding.UTF8.GetBytes(json);
                            item.PACKET_SEQ = dpr.SerialNo;
                            item.SEND_TIME = DateTime.Now.ToUniversalTime();
                            item.STATUS = (short)CommandStateEnum.Delivering;

                            TransforMessage.PublishMessage(Constdefine.MDVREXCHANGE, "MDVR.DelPolygonsRegion." + dpr.UID, sendbytes);
                            context.SaveChanges();
                        }
                    }
                    catch (Exception ex)
                    {
                        LoggerManager.Logger.Error(ex);
                    }
                }
            }

            var templist = from fq in context.TRF_FENCE_QUEUE
                           where fq.STATUS == (short)CommandStateEnum.Delivering && sendinglist.Contains(fq.ID)
                           select fq;

            foreach (var item in templist.ToList())
            {
                item.STATUS = (short)CommandStateEnum.Failed;
            }

            context.SaveChanges();
        }

        public override void OnReply(DBEntity.PTMSEntities context, byte[] bytes, string key)
        {
            string str = System.Text.UnicodeEncoding.UTF8.GetString(bytes);
            GenenalResponseEx response = JsonHelper.FromJsonString<GenenalResponseEx>(str);
            TRF_FENCE_QUEUE record = context.TRF_FENCE_QUEUE.Where(n => n.PACKET_SEQ == response.ResponseSerialNo && n.MDVR_CORE_SN == response.UID).OrderBy(n => n.CREATE_TIME).FirstOrDefault();
            if (record != null && record.STATUS == (short)CommandStateEnum.Delivering)
            {
                if (response.Result == 0)
                {
                    //update the record status
                    record.STATUS = (short)CommandStateEnum.Succeed;
                }
                else
                {
                    //update the record status
                    record.STATUS = (short)CommandStateEnum.Failed;
                }

                context.SaveChanges();
            }
        }
    }
}
