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
using LogisticsWebApi.Models;
using LogisticsWebApi.MQHelper;
using ESRI.ArcGIS.Client.Geometry;

namespace LogisticsWebApi.Controllers
{
    public class VehicleIntegrationServiceController : ApiController
    {        
      
        private Location LocationConvert(RUN_VEHICLE_LOCATION result)
        {
            MapPoint mapPoint =
                        LogisticsWebApi.Models.MapTransfer.GeographicToWebMercator(new MapPoint
                            (Convert.ToDouble(result.LONGITUDE, System.Globalization.CultureInfo.InvariantCulture),
                            Convert.ToDouble(result.LATITUDE, System.Globalization.CultureInfo.InvariantCulture)));


            return new Location
            {
                Direction = result.DIRECTION,
                GPSTime = result.GPS_TIME.HasValue ? result.GPS_TIME.Value.ToLocalTime().ToString("yyyy-MM-dd HH:mm:ss") : string.Empty,
                Latitude = mapPoint.Y.ToString(),
                Longitude = mapPoint.X.ToString(),
                Speed = result.SPEED,
            };
        }

        private LogisticsWebApi.Models.BusinessAlert BusinessAlertConvert(ALT_BUSINESS_ALERT result)
        {

            MapPoint mapPoint =
                     LogisticsWebApi.Models.MapTransfer.GeographicToWebMercator(new MapPoint
                         (Convert.ToDouble(result.LONGITUDE, System.Globalization.CultureInfo.InvariantCulture),
                         Convert.ToDouble(result.LATITUDE, System.Globalization.CultureInfo.InvariantCulture)));
            return new LogisticsWebApi.Models.BusinessAlert
            {

                VehicleId = result.VEHICLE_ID,
                //AlertType = result.ALERT_TYPE.ToString(),               
                Direction = result.DIRECTION,
                AlertTime = result.ALERT_TIME.HasValue ? result.ALERT_TIME.Value.ToLocalTime().ToString("yyyy-MM-dd HH:mm:ss") : string.Empty,
                Latitude = mapPoint.Y.ToString(),
                Longitude = mapPoint.X.ToString(),
                Speed = result.SPEED,
            };
        }
        private VehicleLocation VehicleLocationConvert(RUN_VEHICLE_LOCATION result, string onlinestatus, string overspeedstatus)
        {

            MapPoint mapPoint =
                     LogisticsWebApi.Models.MapTransfer.GeographicToWebMercator(new MapPoint
                         (Convert.ToDouble(result.LONGITUDE, System.Globalization.CultureInfo.InvariantCulture),
                         Convert.ToDouble(result.LATITUDE, System.Globalization.CultureInfo.InvariantCulture)));
            return new VehicleLocation
            {
                Direction = result.DIRECTION,
                GPSTime = result.GPS_TIME.HasValue ? result.GPS_TIME.Value.ToLocalTime().ToString("yyyy-MM-dd HH:mm:ss") : string.Empty,
                Latitude = mapPoint.Y.ToString(),
                Longitude = mapPoint.X.ToString(),
                Speed = result.SPEED,
                VehicleId = result.VEHICLE_ID,
                OnlineStatus = onlinestatus,
                OverSpeedStatus = overspeedstatus,
                MdvrCoreId = result.DEVICE_ID
            };
        }

        [HttpPost]
        [Route("api/Vehicle/GetVehicleLocationHistory")]

        public List<Location> GetVehicleLocationHistory(QueryParam param)
        {
            try
            {

                if (string.IsNullOrWhiteSpace(param.VehicleId) || string.IsNullOrWhiteSpace(param.StartTime) || string.IsNullOrWhiteSpace(param.EndTime))
                {
                    return null;
                }
                string msg = "[GetVehicleAlertHistory]Args:";

                msg += string.Format("VEHICLE_ID={0}", param.VehicleId);
                msg += string.Format("startTime={0}", param.StartTime);
                msg += string.Format("endTime={0}", param.EndTime);
                LoggerManager.Logger.Info(msg);
                DateTime start = DateTime.Parse(param.StartTime).ToUniversalTime();
                DateTime end = DateTime.Parse(param.EndTime).ToUniversalTime();          
                List<Location> locations = new List<Location>();
                using (var context = new PTMSEntities())
                {
                    var result = context.RUN_VEHICLE_LOCATION.OrderByDescending(x => x.GPS_TIME)
                          .Where(n => n.GPS_TIME >= start && n.GPS_TIME <= end && n.VEHICLE_ID == param.VehicleId).ToList();
                    foreach (var item in result)
                    {
                        locations.Add(LocationConvert(item));
                    }
                }

                return locations;
            }
            catch (Exception ex)
            {
                
                return null;
            }
        }

        [HttpPost]
        [Route("api/Vehicle/GetVehicleAlertHistory")]

        public List<LogisticsWebApi.Models.BusinessAlert> GetVehicleAlertHistory(QueryParam param)
        {
            try
            {

                if (string.IsNullOrWhiteSpace(param.VehicleId) || string.IsNullOrWhiteSpace(param.StartTime) || string.IsNullOrWhiteSpace(param.EndTime))
                {
                    return null;
                }
                string msg = "[GetVehicleAlertHistory]Args:";

                msg += string.Format("VEHICLE_ID={0}", param.VehicleId);
                msg += string.Format("startTime={0}", param.StartTime);
                msg += string.Format("endTime={0}", param.EndTime);
                LoggerManager.Logger.Info(msg);
                DateTime start = DateTime.Parse(param.StartTime).ToUniversalTime();
                DateTime end = DateTime.Parse(param.EndTime).ToUniversalTime();             
                var source = new List<LogisticsWebApi.Models.BusinessAlert>();
                using (var context = new PTMSEntities())
                {
                    var data = context.ALT_BUSINESS_ALERT.OrderByDescending(x => x.ALERT_TIME)
                          .Where(n => n.GPS_TIME >= start && n.GPS_TIME <= end && n.VEHICLE_ID == param.VehicleId && n.ALERT_TYPE == 0).ToList();
                    if (data != null)
                    {
                        foreach (var item in data)
                        {
                            source.Add(BusinessAlertConvert(item));
                        }
                    }
                }

                return source;
            }
            catch (Exception ex)
            {

                return null;
            }
        }


        [HttpGet]
        [Route("api/Vehicle/InsertVehicleLocation")]

        public void GetVehicleLocation()
        {
            try
            {
                using (var context = new PTMSEntities())
                {
                    DateTime startTime = DateTime.Now.AddDays(-1);
                    string lat = "30.48";
                    string lon = "114.40";
                    for (int i = 0; i <= 15000; i++)
                    {
                        RUN_VEHICLE_LOCATION location = new RUN_VEHICLE_LOCATION();
                        location.CLIENT_ID = "be9df306-e7f7-4275-8ec1-fe5b16808afe";
                        string id = Guid.NewGuid().ToString();
                        location.ID = id;
                        location.VEHICLE_ID = "BMW8888";
                        location.SOURCE = 0;
                        location.GPS_VALID = "A";
                        location.GPS_TIME = startTime.AddSeconds(i);
                        Random r = new Random();
                        int value =r.Next(1000, 9999);
                        string Nlat = lat + value.ToString();
                        location.LATITUDE = Nlat;
                        value = r.Next(1000, 9999);
                        string NLon = lon + value.ToString();
                        location.LONGITUDE = NLon;
                        value = r.Next(0, 100);
                        location.SPEED = value.ToString();
                        value = r.Next(0, 360);
                        location.DIRECTION = value.ToString();
                        location.DISTRICT_CODE = "02-01";
                        location.ALARM_FLAG = 0;
                        location.STATUS_FLAG = 262161;
                        location.SOURCE_MODE = 0;
                        location.DEVICE_ID = "99999BJ88888";
                        context.RUN_VEHICLE_LOCATION.Add(location);
                        context.SaveChanges();
                    }

                }


            }
            catch (Exception ex)
            {


            }
        }

        [HttpGet]
        [Route("api/Vehicle/GetVehicleLocation/{VehicleId?}")]

        public List<VehicleLocation> GetVehicleLocation(string VehicleId = "")
        {
            try
            {

               
                string msg = "[GetVehicleLocation]Args:";
                msg += string.Format("VehicleId={0}", VehicleId);             
                LoggerManager.Logger.Info(msg);
                DateTime start = DateTime.Now.AddDays(-14).ToUniversalTime();
                DateTime end = DateTime.Now.AddHours(1).ToUniversalTime();
                DateTime overstart = DateTime.Now.AddMinutes(-1).ToUniversalTime();
                DateTime overend = DateTime.Now.AddMinutes(1).ToUniversalTime();
                List<VehicleLocation> locations = new List<VehicleLocation>();
                using (var context = new PTMSEntities())
                {
                    if (string.IsNullOrWhiteSpace(VehicleId))
                    {
                        var result = (from w in context.RUN_SUITE_WORKING
                                      where w.VEHICLE_ID != null

                                      select new
                                      {
                                          VehicleId = w.VEHICLE_ID,
                                          Status = w.ONLINE_FLAG
                                      }).ToList();


                        foreach (var item in result)
                        {
                            var data = context.RUN_VEHICLE_LOCATION
                                  .Where(n => n.GPS_TIME >= start && n.GPS_TIME <= end && n.VEHICLE_ID == item.VehicleId).OrderByDescending(x => x.GPS_TIME).FirstOrDefault();


                            string overspeedstatus = "0";
                            if (data != null)
                            {
                                var over = context.ALT_BUSINESS_ALERT
                              .Where(n => n.ALERT_TIME >= overstart && n.ALERT_TIME <= overend && n.VEHICLE_ID == item.VehicleId && n.ALERT_TYPE == 0).OrderByDescending(x => x.ALERT_TIME).FirstOrDefault();
                                if (over != null)
                                {
                                    overspeedstatus = "1";
                                }
                                locations.Add(VehicleLocationConvert(data, item.Status.ToString(), overspeedstatus));
                            }


                        }
                    }
                    else
                    {
                        var data = (from w in context.RUN_SUITE_WORKING
                                    where w.VEHICLE_ID == VehicleId

                                    select new
                                    {
                                        Status = w.ONLINE_FLAG
                                    }).FirstOrDefault();

                        if (data != null)
                        {
                            var result = context.RUN_VEHICLE_LOCATION
                                      .Where(n => n.GPS_TIME >= start && n.GPS_TIME <= end && n.VEHICLE_ID == VehicleId).OrderByDescending(x => x.GPS_TIME).FirstOrDefault();


                            if (result != null)
                            {
                                string overspeedstatus = "0";
                                if (data != null)
                                {
                                    var over = context.ALT_BUSINESS_ALERT
                                  .Where(n => n.ALERT_TIME >= overstart && n.ALERT_TIME <= overend && n.VEHICLE_ID == VehicleId && n.ALERT_TYPE == 0).OrderByDescending(x => x.ALERT_TIME).FirstOrDefault();
                                    if (over != null)
                                    {
                                        overspeedstatus = "1";
                                    }
                                    locations.Add(VehicleLocationConvert(result, data.Status.ToString(), overspeedstatus));
                                }
                               
                            }

                        }

                    }
                }

                return locations;
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        [HttpPost]
        [Route("api/Vehicle/GetBatchVehicleLocation")]

        public List<VehicleLocation> GetVehicleLocation(VehicleIdList VehicleIds)
        {
            try
            {            
                string msg = "[GetVehicleLocation]Args:";
                msg += string.Format("VehicleIds={0}", JsonHelper.ToJsonString(VehicleIds.VehicleIds));
                LoggerManager.Logger.Info(msg);
                DateTime start = DateTime.Now.AddDays(-14).ToUniversalTime();
                DateTime end = DateTime.Now.AddHours(1).ToUniversalTime();
                DateTime overstart = DateTime.Now.AddMinutes(-1).ToUniversalTime();
                DateTime overend = DateTime.Now.AddMinutes(1).ToUniversalTime();
                List<VehicleLocation> locations = new List<VehicleLocation>();
                using (var context = new PTMSEntities())
                {
                    if (VehicleIds.VehicleIds.Count > 0)
                    {
                        var result = (from w in context.RUN_SUITE_WORKING
                                      where VehicleIds.VehicleIds.Contains(w.VEHICLE_ID)

                                      select new
                                      {
                                          VehicleId = w.VEHICLE_ID,
                                          Status = w.ONLINE_FLAG
                                      }).ToList();


                        foreach (var item in result)
                        {
                            var data = context.RUN_VEHICLE_LOCATION
                                  .Where(n => n.GPS_TIME >= start && n.GPS_TIME <= end && n.VEHICLE_ID == item.VehicleId).OrderByDescending(x => x.GPS_TIME).FirstOrDefault();


                            string overspeedstatus = "0";
                            if (data != null)
                            {
                                var over = context.ALT_BUSINESS_ALERT
                              .Where(n => n.ALERT_TIME >= overstart && n.ALERT_TIME <= overend && n.VEHICLE_ID == item.VehicleId && n.ALERT_TYPE == 0).OrderByDescending(x => x.ALERT_TIME).FirstOrDefault();
                                if (over != null)
                                {
                                    overspeedstatus = "1";
                                }
                                locations.Add(VehicleLocationConvert(data, item.Status.ToString(), overspeedstatus));
                            }


                        }
                    }
                    else
                    {
                        return null;                     
                    }
                }

                return locations;
            }
            catch (Exception ex)
            {

                return null;
            }
        }


        private const double EARTH_RADIUS = 6378137;

        private static double Rad(double d)
        {

            return (double)d * Math.PI / 180d;       
        
        }

        public static bool IsInSide(double lat1, double lng1, double lat2, double lng2,double radius)
        {
            double radlat1 = Rad(lat1);
            double radlng1 = Rad(lng1);
            double radlat2 = Rad(lat2);
            double radlng2 = Rad(lng2);

            double a = radlat1 - radlat2;
            double b = radlng1 - radlng2;

            double result = 2*Math.Asin(Math.Sqrt(Math.Pow(Math.Sin(a/2),2)+Math.Cos(radlat1)*Math.Cos(radlat2)*Math.Pow(Math.Sin(b/2),2)))*EARTH_RADIUS;

            if (result <= radius)
            {
                return true;
            
            }

            return false;
        
        }




        
        [HttpPost]
        [Route("api/Vehicle/GetBscVehicleList")]
        public List<VehicleLocation> GetBscVehicleList(VehicleQueryParam param)
        {
            try
            {
                string msg = "[GetBscVehicleList]";
                LoggerManager.Logger.Info(msg);

             


                if (param.Latitude == null)
                {
                    return null;
                
                }
                if (param.Longitude == null)
                {
                    return null;

                }
                if (param.Radius == null)
                {
                    return null;

                }


                if (param.Latitude > 90 || param.Latitude < -90)
                {
                    return null;
                }
                if (param.Longitude > 180 || param.Longitude < -180)
                {
                    return null;
                }
                if (param.Radius <0)
                {
                    return null;
                }

                MapPoint mapPoint =
                     LogisticsWebApi.Models.MapTransfer.WebMercatorToGeographic(new MapPoint
                         (Convert.ToDouble(param.Longitude, System.Globalization.CultureInfo.InvariantCulture),
                         Convert.ToDouble(param.Latitude, System.Globalization.CultureInfo.InvariantCulture)));

                DateTime start = DateTime.Now.AddDays(-7).ToUniversalTime();
                DateTime end = DateTime.Now.AddHours(1).ToUniversalTime();
                DateTime overstart = DateTime.Now.AddMinutes(-1).ToUniversalTime();
                DateTime overend = DateTime.Now.AddMinutes(1).ToUniversalTime();
                List<VehicleLocation> locations = new List<VehicleLocation>();
                using (var context = new PTMSEntities())
                {
                     var result = (from w in context.RUN_SUITE_WORKING
                                      where w.VEHICLE_ID != null

                                      select new
                                      {
                                          VehicleId = w.VEHICLE_ID,
                                          Status = w.ONLINE_FLAG
                                      }).ToList();


                        foreach (var item in result)
                        {
                            var data = context.RUN_VEHICLE_LOCATION
                                  .Where(n => n.GPS_TIME >= start && n.GPS_TIME <= end && n.VEHICLE_ID == item.VehicleId).OrderByDescending(x => x.GPS_TIME).FirstOrDefault();


                            string overspeedstatus = "0";
                            if (data != null)
                            {  
                                double lat;
                                double lng;
                                double.TryParse(data.LATITUDE, out lat);
                                double.TryParse(data.LONGITUDE, out lng);
                                if (IsInSide(lat, lng, mapPoint.Y, mapPoint.X, param.Radius))
                                {
                                    var over = context.ALT_BUSINESS_ALERT
                                        .Where(n => n.ALERT_TIME >= overstart && n.ALERT_TIME <= overend && n.VEHICLE_ID == item.VehicleId && n.ALERT_TYPE == 0).OrderByDescending(x => x.ALERT_TIME).FirstOrDefault();
                                    if (over != null)
                                    {
                                        overspeedstatus = "1";
                                    }
                                    locations.Add(VehicleLocationConvert(data, item.Status.ToString(), overspeedstatus));
                                }
                            }


                        }
                   

                    }
                

                return locations;
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        

        [HttpGet]
        [Route("api/Vehicle/GetBindingList/{VehicleId?}")]
        public List<VehicleMdvr> GetBindingList(string VehicleId = "")
        {
            try
            {
                string msg = "[GetBindingList]"; 
               


                LoggerManager.Logger.Info(msg);
                List<VehicleMdvr> vehicles = new List<VehicleMdvr>();
                using (var context = new PTMSEntities())
                {
                    Expression<Func<RUN_SUITE_WORKING, bool>> filter = f => f.VEHICLE_ID != null;

                    if (!string.IsNullOrWhiteSpace(VehicleId))
                    {
                        filter = filter.And(f => f.VEHICLE_ID.ToLower().Contains(VehicleId.ToLower()));
                    }


                    var source = context.RUN_SUITE_WORKING.Where(filter);
                    var list = source.ToList();
                    var items = from c in list
                                select new LogisticsWebApi.Models.VehicleMdvr()
                                {
                                    VEHICLE_ID = c.VEHICLE_ID,
                                    MDVR_CORE_SN = c.MDVR_CORE_SN


                                };

                    vehicles = items.ToList();
                                

                    return vehicles;
                }
            }
            catch (Exception ex)
            {

                return null;
            }
        }

       
       
    }
}
