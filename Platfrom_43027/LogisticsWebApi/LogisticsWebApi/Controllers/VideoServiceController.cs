using Gsafety.Common.Logging;
using Gsafety.Common.Util;
using Gsafety.PTMS.Common.Data;
using Gsafety.PTMS.Common.Data.Enum;
using Gsafety.PTMS.DBEntity;
using Gsafety.PTMS.BaseInfo;
using Gsafety.PTMS.BaseInfo.Conditions;
using Gsafety.PTMS.BaseInfo.Conditions.QueryFiler;
using Gsafety.PTMS.BaseInfo.Conditons.QueryFiler;
using Gsafety.PTMS.BaseInfo.MakerContions.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Configuration;
using LogisticsWebApi.Models;
using LogisticsWebApi.MQHelper;

namespace LogisticsWebApi.Controllers
{
    public class VideoIntegrationServiceController : ApiController
    {


        private ReturnVideoInfo VideoInfoConvert(VideoInfo result)
        {
            return new ReturnVideoInfo
            {
                Channel = result.Channel.ToString(),
                StartTime = result.StartTime.ToLocalTime().ToString("yyyy-MM-dd HH:mm:ss"),
                EndTime = result.EndTime.ToLocalTime().ToString("yyyy-MM-dd HH:mm:ss"),
                FileId = result.FileId.Trim(),
                FileSize = result.FileSize.ToString(),
                VideoUrl = result.VideoUrl.Trim(),
            };
        }


        [HttpPost]
        [Route("api/Video/GetVehicleVideoHistory")]

        public List<LogisticsWebApi.Models.ReturnVideoInfo> GetVehicleVideoHistory(QueryParam param)
        {
            try
            {
                string baseUrl = "http://" + ConfigurationManager.AppSettings["videoServiceFileServerIp"] + ":" + ConfigurationManager.AppSettings["videoServiceFileServerPort"] + "/";

                if (string.IsNullOrWhiteSpace(param.VehicleId) || string.IsNullOrWhiteSpace(param.StartTime) || string.IsNullOrWhiteSpace(param.EndTime))
                {
                    return null;
                }
                string msg = "[GetVehicleVideoHistory]Args:";

                msg += string.Format("VEHICLE_ID={0}", param.VehicleId);
                msg += string.Format("startTime={0}", param.StartTime);
                msg += string.Format("endTime={0}", param.EndTime);
                LoggerManager.Logger.Info(msg);
                DateTime start = DateTime.Parse(param.StartTime).ToUniversalTime();
                DateTime end = DateTime.Parse(param.EndTime).ToUniversalTime();
                var source = new List<LogisticsWebApi.Models.VideoInfo>();
                var returnsource = new List<LogisticsWebApi.Models.ReturnVideoInfo>();

                using (var entity = new PTMSEntities())
                {
                    var working = entity.RUN_SUITE_WORKING.FirstOrDefault(n => n.VEHICLE_ID == param.VehicleId);
                    if (working != null)
                    {
                        string mdvrsn = working.MDVR_CORE_SN;

                        var vehicle = entity.BSC_VEHICLE.FirstOrDefault(n => n.VEHICLE_ID == param.VehicleId && n.VALID == 1);
                        string vehiclesn = vehicle.VEHICLE_SN;
                        if (vehicle != null)
                        {                        
                            
                                var tempalarm = from A in entity.MDI_ALARM_VIDEO
                                                where A.MDVR_CORE_SN == mdvrsn && A.START_TIME >= start && A.START_TIME <= end
                                                && A.START_TIME < A.END_TIME
                                                select new VideoInfo
                                                {
                                                    Channel = A.CHANNEL_ID,
                                                    StartTime = A.START_TIME == null ? DateTime.MinValue : (DateTime)A.START_TIME,
                                                    EndTime = A.END_TIME == null ? DateTime.MinValue : (DateTime)A.END_TIME,
                                                    FileId = A.VIDEO_URL,
                                                    FileSize = A.VIDEO_SIZE,
                                                    VideoUrl = baseUrl + A.VIDEO_URL
                                                   
                                                };

                                //select from alive video
                                var tempalive = from B in entity.MDI_LIVE_VIDEO
                                                where B.MDVR_CORE_SN == mdvrsn && B.START_TIME >= start && B.START_TIME <= end
                                                && B.START_TIME < B.END_TIME
                                                && B.IS_FINISH == 1
                                                select new VideoInfo
                                                {
                                                    Channel = B.CHANNEL_ID,
                                                    StartTime = B.START_TIME == null ? DateTime.MinValue : (DateTime)B.START_TIME,
                                                    EndTime = B.END_TIME == null ? DateTime.MinValue : (DateTime)B.END_TIME,
                                                    FileId = B.VIDEO_URL,
                                                    FileSize = B.VIDEO_SIZE,
                                                    VideoUrl = baseUrl + B.VIDEO_URL
                                                   
                                                };
                                var tempdown = from D in entity.MDI_DOWNLOAD_VIDEO
                                               where D.MDVR_CORE_SN == mdvrsn && D.START_TIME != null && D.START_TIME >= start && D.START_TIME <= end
                                                && D.START_TIME < D.END_TIME
                                               select new VideoInfo
                                               {
                                                   Channel = D.CHANNEL_ID.HasValue ? D.CHANNEL_ID.Value : 0,
                                                   StartTime = D.START_TIME == null ? DateTime.MinValue : (DateTime)D.START_TIME,
                                                   EndTime = D.END_TIME == null ? DateTime.MinValue : (DateTime)D.END_TIME,
                                                   FileId = D.VIDEO_URL,
                                                   FileSize = D.SOURCE_SIZE.Value,
                                                   VideoUrl = baseUrl + D.VIDEO_URL
                                                  
                                               };
                                source = tempalarm.Concat(tempalive).Concat(tempdown).ToList();


                                foreach (var item in source)
                                {
                                    returnsource.Add(VideoInfoConvert(item));
                                    
                                
                                }
                                
                            }                        
                    }
                   
                }

                return returnsource;
            }
            catch (Exception ex)
            {

                return null;
            }
        }
       
    }
}
