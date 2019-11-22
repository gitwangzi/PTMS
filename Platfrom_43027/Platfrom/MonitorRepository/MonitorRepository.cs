/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
///// Guid: a2a104b0-cf3f-43b2-b2c8-eefd726551f5      
///// clrversion: 4.0.30319.17929
/////Registered organization: 
///// Machine Name: 
///// Author: Hongsheng Shi
/////======================================================================
/////  Project Name: Gsafety.PTMS.BaseInformation.Contract.Data
/////  Project Description:    
///// Class Name: MonitorRepository
///// Class Version: v1.0.0.0
///// Create Time: 2013/8/8 15:47:53
///// Class Description:  
/////======================================================================
/////  Modified Time: 2013/8/22 15:47:53
/////  Modified by: 
/////  Modified Description: 
/////======================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gsafety.PTMS.DBEntity;
using Gsafety.PTMS.Monitor.Contract.Data;
using System.Transactions;
using Gsafety.PTMS.Message.Contract.Data;
using Gsafety.PTMS.Common.Data;
using Gsafety.PTMS.Common.Enum;

namespace Gsafety.PTMS.Monitor.Repository
{
    public class MonitorRepository
    {
        public MonitorRepository()
        {

        }

        public void AddSuiteOnOffline(PTMSEntities context, OnOfflineEx model)
        {
            TransactionOptions optons = new TransactionOptions();
            optons.IsolationLevel = IsolationLevel.ReadUncommitted;

            var scope = new TransactionScope(TransactionScopeOption.Required, optons);

            try
            {

                RUN_SUITE_ONLINE_RECORD entity = new RUN_SUITE_ONLINE_RECORD();

                entity.ID = Guid.NewGuid().ToString();
                entity.VEHICLE_ID = model.VehicleId;
                entity.SUITE_INFO_ID = model.SuiteInfoID;
                entity.MDVR_CORE_SN = model.UID;
                entity.GPS_TIME = model.OnOffLineTime;
                entity.STATUS = (short)model.IsOnline;
                entity.CLIENT_ID = model.ClientId;

                context.RUN_SUITE_ONLINE_RECORD.Add(entity);

                var result = (from s in context.RUN_SUITE_WORKING
                              where s.VEHICLE_ID == model.VehicleId
                              select s).FirstOrDefault();
                if (result != null)
                {
                    result.SWITCH_TIME = model.OnOffLineTime;
                    result.ONLINE_FLAG = (short)model.IsOnline;
                }

                context.SaveChanges();

                scope.Complete();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                scope.Dispose();
            }
        }

        public void AddGPSOnOffline(PTMSEntities context, OnOfflineEx model)
        {
            TransactionOptions optons = new TransactionOptions();
            optons.IsolationLevel = IsolationLevel.ReadUncommitted;

            var scope = new TransactionScope(TransactionScopeOption.Required, optons);

            try
            {
                RUN_GPS_ONLINE_RECORD entity = new RUN_GPS_ONLINE_RECORD();

                entity.ID = Guid.NewGuid().ToString();
                entity.VEHICLE_ID = model.VehicleId;
                entity.DEV_GPS_ID = model.SuiteInfoID;
                entity.GPS_SN = model.UID;
                entity.GPS_TIME = model.OnOffLineTime;
                entity.STATUS = (short)model.IsOnline;
                entity.CLIENT_ID = model.ClientId;

                context.RUN_GPS_ONLINE_RECORD.Add(entity);

                var result = (from s in context.RUN_GPS_WORKING
                              where s.VEHICLE_ID == model.VehicleId
                              select s).FirstOrDefault();
                if (result != null)
                {
                    result.SWITCH_TIME = model.OnOffLineTime;
                    result.ONLINE_FLAG = (short)model.IsOnline;
                }

                context.SaveChanges();

                scope.Complete();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                scope.Dispose();
            }
        }

        public void AddMoibleOnOffline(PTMSEntities context, OnOfflineEx model)
        {
            TransactionOptions optons = new TransactionOptions();
            optons.IsolationLevel = IsolationLevel.ReadUncommitted;

            var scope = new TransactionScope(TransactionScopeOption.Required, optons);

            try
            {
                RUN_APP_ONLINE_RECORD entity = new RUN_APP_ONLINE_RECORD();

                entity.ID = Guid.NewGuid().ToString();
                entity.VEHICLE_ID = model.VehicleId;
                entity.CHAUFFEUR_ID = model.SuiteInfoID;
                entity.MOBILE_SIM = model.UID;
                entity.GPS_TIME = model.OnOffLineTime;
                entity.STATUS = (short)model.IsOnline;
                entity.CLIENT_ID = model.ClientId;

                context.RUN_APP_ONLINE_RECORD.Add(entity);

                var result = (from s in context.RUN_MOBILE_WORKING
                              where s.VEHICLE_ID == model.VehicleId
                              select s).FirstOrDefault();
                if (result != null)
                {
                    result.SWITCH_TIME = model.OnOffLineTime;
                    result.ONLINE_FLAG = (short)model.IsOnline;
                }

                context.SaveChanges();

                scope.Complete();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                scope.Dispose();
            }
        }

        public AuthenticateResponse Authenticate(PTMSEntities context, Authenticate authenticate)
        {
            AuthenticateResponse response = new AuthenticateResponse();
            response.UID = authenticate.UID;
            response.SerialNo = authenticate.SerialNo;
            response.RegisterNo = authenticate.RegisterNo;
            response.SIM = authenticate.SIM;
            response.IsPassed = (int)IsPassed.Yes;

            var result = context.RUN_SUITE_WORKING.FirstOrDefault(x => x.MDVR_CORE_SN == authenticate.UID);
            if (result == null)
            {
                response.IsPassed = (int)IsPassed.No;
            }
            else
            {
                response.VehicleId = result.VEHICLE_ID;
                response.IsPassed = (int)IsPassed.Yes;
            }

            return response;
        }

        public AuthenticateResponse GPSAuthenticate(PTMSEntities context, Authenticate authenticate)
        {
            AuthenticateResponse response = new AuthenticateResponse();
            response.UID = authenticate.UID;
            response.SerialNo = authenticate.SerialNo;
            response.RegisterNo = authenticate.RegisterNo;
            response.SIM = authenticate.SIM;


            var gpsresult = context.RUN_GPS_WORKING.FirstOrDefault(x => x.GPS_SN == authenticate.UID);
            if (gpsresult != null)
            {
                response.VehicleId = gpsresult.VEHICLE_ID;
                response.IsPassed = (int)IsPassed.Yes;
            }
            else
            {
                response.IsPassed = (int)IsPassed.No;
            }


            return response;
        }

        public Gsafety.PTMS.Common.Data.GPS GetVehicleLastGPS(PTMSEntities context, string vehicleId)
        {
            Gsafety.PTMS.Common.Data.GPS result = null;
            List<int> ValidSource = new List<int>();
            ValidSource.Add((int)GPSSourceEnum.Suite);
            ValidSource.Add((int)GPSSourceEnum.GPS);
            var data = context.RUN_VEHICLE_LOCATION.Where(v => v.VEHICLE_ID == vehicleId && ValidSource.Contains(v.SOURCE) && "A".Equals(v.GPS_VALID.Trim()));
            var gpsData = from gps in data
                          orderby gps.GPS_TIME descending
                          select new Gsafety.PTMS.Common.Data.GPS()
                          {
                              VehicleId = gps.VEHICLE_ID,
                              Longitude = gps.LONGITUDE,
                              Latitude = gps.LATITUDE,
                              Direction = gps.DIRECTION,
                              GpsTime = gps.GPS_TIME.Value,
                              Speed = gps.SPEED,
                              Source = gps.SOURCE,
                              Valid = gps.GPS_VALID
                          };
            result = gpsData.FirstOrDefault();

            if (result == null)
            {
                data = context.RUN_VEHICLE_LOCATION.Where(v => v.VEHICLE_ID == vehicleId && v.SOURCE == 2 && "A".Equals(v.GPS_VALID.Trim()));

                gpsData = from gps in data
                          orderby gps.GPS_TIME descending
                          select new Gsafety.PTMS.Common.Data.GPS()
                           {
                               VehicleId = gps.VEHICLE_ID,
                               Longitude = gps.LONGITUDE,
                               Latitude = gps.LATITUDE,
                               Direction = gps.DIRECTION,
                               GpsTime = gps.GPS_TIME.Value,
                               Speed = gps.SPEED,
                               Source = gps.SOURCE,
                               Valid = gps.GPS_VALID
                           };

                result = gpsData.FirstOrDefault();
            }
            return result;
        }

        public List<VehicleAlert> GetVehicleAlert(PTMSEntities context, string vehicleId, DateTime startDate, DateTime endDate, string alertType)
        {
            //var result = from f in context.DEVICE_ALERT
            //             where f.VEHICLE_ID == vehicleId
            //                     && f.ALERT_TIME > startDate
            //                     && f.ALERT_TIME < endDate
            //                     && f.ALERT_TYPE == Convert.ToDecimal(alertType)
            //             select new VehicleAlert
            //             {
            //                 Id = f.ID,
            //                 VehicleId = f.VEHICLE_ID,
            //                 Longitude = f.LONGITUDE,
            //                 Latitude = f.LATITUDE,
            //                 Direction = f.DIRECTION,
            //                 Speed = Convert.ToDouble(f.SPEED),
            //                 AlertTime = f.ALERT_TIME,
            //                 AlertType = Convert.ToInt32(f.ALERT_TYPE.ToString()),
            //                 GpsTime = f.GPS_TIME,
            //                 MdvrCoreId = f.MDVR_CORE_SN,
            //                 Status = Convert.ToInt32(f.STATUS.ToString()),
            //                 SuiteInfoId = f.SUITE_INFO_ID
            //             };
            //return result.ToList();
            return null;
        }

        //public void AddOnline(PTMSEntities context, OnOffline item)
        //{
        //    TransactionOptions optons = new TransactionOptions();
        //    optons.IsolationLevel = IsolationLevel.ReadUncommitted;

        //    var scope = new TransactionScope(TransactionScopeOption.Required, optons);

        //    try
        //    {

        //        SUITE_ONLINE_RECORD model = new SUITE_ONLINE_RECORD();

        //        model.ID = Guid.NewGuid().ToString();
        //        model.SUITE_INFO_ID = item.SuiteInfoId;
        //        model.MDVR_CORE_SN = item.MdvrCoreId;
        //        model.VEHICLE_ID = item.VehicleId;
        //        model.LONGITUDE = item.Longitude;
        //        model.LATITUDE = item.Latitude;
        //        model.SPEED = item.Speed;
        //        model.DIRECTION = item.Direction;
        //        model.GPS_TIME = item.GPSTime;
        //        model.GPS_VALID = item.GPSValid;
        //        model.STATUS = (short)item.IsOnLine;
        //        context.SUITE_ONLINE_RECORD.Add(model);

        //        var result = (from s in context.SECURITY_SUITE_WORKING
        //                      where s.MDVR_CORE_SN == item.MdvrCoreId
        //                      select s).FirstOrDefault();
        //        if (result != null)
        //        {
        //            result.SWITCH_TIME = item.GPSTime;
        //            result.ONLINE_FLAG = (short)item.IsOnLine;
        //        }

        //        context.SaveChanges();

        //        scope.Complete();
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //    finally
        //    {
        //        scope.Dispose();
        //    }
        //}
        ///// <summary>
        ///// not call this method ,this is for Test
        ///// </summary>
        ///// <param name="item"></param>
        ///// <returns></returns>
        //public bool AddOnlineTest(PTMSEntities context, OnOffline item)
        //{
        //    TransactionOptions optons = new TransactionOptions();
        //    optons.IsolationLevel = IsolationLevel.ReadUncommitted;
        //    int i = 0;
        //    var scope = new TransactionScope(TransactionScopeOption.Required, optons);

        //    try
        //    {

        //        SUITE_ONLINE_RECORD model = new SUITE_ONLINE_RECORD();

        //        model.ID = Guid.NewGuid().ToString();
        //        model.SUITE_INFO_ID = item.SuiteInfoId;
        //        model.MDVR_CORE_SN = item.MdvrCoreId;
        //        model.VEHICLE_ID = item.VehicleId;
        //        model.LONGITUDE = item.Longitude;
        //        model.LATITUDE = item.Latitude;
        //        model.SPEED = item.Speed;
        //        model.DIRECTION = item.Direction;
        //        model.GPS_TIME = item.GPSTime;
        //        model.GPS_VALID = item.GPSValid;
        //        model.STATUS = (short)item.IsOnLine;
        //        context.SUITE_ONLINE_RECORD.Add(model);

        //        var result = (from s in context.SECURITY_SUITE_WORKING
        //                      where s.MDVR_CORE_SN == item.MdvrCoreId
        //                      select s).FirstOrDefault();
        //        if (result != null)
        //        {
        //            result.SWITCH_TIME = item.GPSTime;
        //            result.ONLINE_FLAG = (short)item.IsOnLine;
        //        }

        //        i = context.SaveChanges();

        //        scope.Complete();
        //        return i > 0;
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //    finally
        //    {
        //        scope.Dispose();
        //    }
        //}

        public List<GPS> GetMonitorGPSTrack(PTMSEntities context, string vehicleId, DateTime startTime, DateTime endTime)
        {
            List<int> ValidSource = new List<int>();
            ValidSource.Add((int)GPSSourceEnum.Suite);
            ValidSource.Add((int)GPSSourceEnum.GPS);

            var data = context.RUN_VEHICLE_LOCATION.Where(v => v.VEHICLE_ID == vehicleId && ValidSource.Contains(v.SOURCE) && v.GPS_TIME >= startTime
                             && v.GPS_TIME <= endTime&&v.GPS_VALID=="A");

            if (data.Any())
            {
                var result = from a in context.RUN_VEHICLE_LOCATION
                         .Where(o => o.GPS_TIME >= startTime
                             && o.GPS_TIME <= endTime && o.GPS_VALID == "A"
                             && o.VEHICLE_ID == vehicleId && ValidSource.Contains(o.SOURCE))
                             select new GPS
                             {
                                 Longitude = a.LONGITUDE,
                                 Latitude = a.LATITUDE,
                                 Speed = a.SPEED,
                                 Direction = a.DIRECTION,
                                 //MdvrCoreId = a.MDVR_CORE_SN,
                                 Valid = a.GPS_VALID,
                                 GpsTime = a.GPS_TIME,
                                 VehicleId = a.VEHICLE_ID
                             };

                return result.OrderBy(i => i.GpsTime).Take(1000).ToList();
            }
            else
            {
                var result = from a in context.RUN_VEHICLE_LOCATION
                         .Where(o => o.GPS_TIME >= startTime
                             && o.GPS_TIME <= endTime && o.GPS_VALID == "A"
                             && o.VEHICLE_ID == vehicleId && o.SOURCE == (short)GPSSourceEnum.Mobile)
                             select new GPS
                             {
                                 Longitude = a.LONGITUDE,
                                 Latitude = a.LATITUDE,
                                 Speed = a.SPEED,
                                 Direction = a.DIRECTION,
                                 //MdvrCoreId = a.MDVR_CORE_SN,
                                 Valid = a.GPS_VALID,
                                 GpsTime = a.GPS_TIME,
                                 VehicleId = a.VEHICLE_ID
                             };
                return result.OrderBy(i => i.GpsTime).Take(1000).ToList();
            }
        }


        public int AddVehicleOnOffTime(VehicleOnOffTime item)
        {
            int result = -1;

            try
            {
                using (PTMSEntities context = new PTMSEntities())
                {
                    //VEHICLE_ONLINE_TIME model = new VEHICLE_ONLINE_TIME();
                    //model.ID = Guid.NewGuid().ToString();
                    //model.MDVR_CORE_SN = item.Mdvr_Core_SN;
                    //model.VEHICLE_ID = item.Vehicle_ID;
                    //model.ONLINE_TIME = item.Online_Time;
                    //model.OFFLINE_TIME = item.Offline_Time;
                    //model.ONLINE_TIMESPAN = item.Online_Timespan;
                    //model.DISTANCE = item.Distance;
                    //context.VEHICLE_ONLINE_TIME.Add(model);
                    result = context.SaveChanges();
                }

            }
            catch (Exception)
            {
                throw;
            }

            return result;
        }

        public bool ValidateSuiteGPS(PTMSEntities context, string vehicleId, string mdvrCoreSn)
        {
            DateTime past = DateTime.Now.AddHours(-1).ToUniversalTime();
            bool found = context.RUN_VEHICLE_LOCATION.Any(n => n.VEHICLE_ID == vehicleId && n.GPS_TIME > past && n.SOURCE == (short)GPSSourceEnum.Suite && n.DEVICE_ID == mdvrCoreSn);
            if (found)
            {
                MTN_INSTALLATION_DETAIL detail = context.MTN_INSTALLATION_DETAIL.FirstOrDefault(n => n.VEHICLE_ID == vehicleId && n.MDVR_CORE_SN == mdvrCoreSn);
                if (detail != null)
                {
                    MTN_INSTALLATION_AUDIT audit = context.MTN_INSTALLATION_AUDIT.FirstOrDefault(n => n.INSTALL_ID == detail.ID);
                    if (audit != null)
                    {
                        audit.GPS_CHECK = 1;

                        context.SaveChanges();
                    }
                }
            }

            return found;
        }

        public bool ValidateGPSGPS(PTMSEntities context, string vehicleId, string gpsuid)
        {
            DateTime now = DateTime.Now.AddHours(-1).ToUniversalTime();
            return context.RUN_VEHICLE_LOCATION.Any(n => n.VEHICLE_ID == vehicleId && n.GPS_TIME > now && n.SOURCE == (short)GPSSourceEnum.GPS && n.DEVICE_ID == gpsuid);
        }


    }
}
