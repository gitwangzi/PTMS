using Gsafety.PTMS.DBEntity;
using Gsafety.PTMS.Integration.Contract;
/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 83c0ad9d-0e11-4102-bf03-b7b183be8a38      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: WANGJINCAI
/////                 Author: TEST(wangjc)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Integration.Repository
/////    Project Description:    
/////             Class Name: IEcu911Repository
/////          Class Version: v1.0.0.0
/////            Create Time: 2013-08-28 17:00:45
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013-08-28 17:00:45
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityFramework.Extensions;
using Gsafety.PTMS.Integration.Contract.Data;
using Gsafety.Common.Util;

namespace Gsafety.PTMS.Integration.Repository
{
    public class Ecu911Repository : IEcu911Service
    {
        public Ecu911Repository()
        {

        }

        public Location GetLocation(PTMSEntities _context, string vehicleId)
        {

            //var result = _context.VEHICLE_LOCATION.OrderByDescending(x => x.GPS_TIME)
            //         .FirstOrDefault(x => x.VEHICLE_ID == vehicleId);
            //if (result != null)
            //{
            //    return LocationConvert(result);
            //}

            return null;

        }

        public IEnumerable<Location> GetLocationHistory(PTMSEntities _context, string vehicleId, DateTime fromDate, DateTime toDate)
        {

            //var quey = _context.RUN_VEHICLE_LOCATION.Where(x => x.VEHICLE_ID == vehicleId
            //                                          && x.GPS_TIME >= fromDate
            //                                          && x.GPS_TIME <= toDate
            ////                                       );
            //var result = quey.ToArray()
            //            .Select(q => LocationConvert(q));

            //return result;
            return null;

        }
        public VehicleInfo GetVehicleInfo(PTMSEntities _context, string vehicleId)
        {

            //var q = _context.VEHICLE.Where(x => x.VEHICLE_ID == vehicleId)

            //           .Join(_context.SECURITY_SUITE_WORKING, x => x.VEHICLE_ID, y => y.VEHICLE_ID, (a, b) => new { A = a, C = b })
            //           .Join(_context.DEV_SUITE, x => x.C.SUITE_INFO_ID, y => y.SUITE_INFO_ID, (a, b) => new { A = a.A, C = a.C, D = b })
            //           .Join(_context.DISTRICT, x => x.A.DISTRICT_CODE, y => y.CODE, (a, b) => new { A = a.A, C = a.C, D = a.D, E = b })
            //           .ToArray()
            //           .Select(x => new VehicleInfo
            //           {
            //               BrandModel = x.A.BRAND_MODEL,

            //               Mobile = x.A.OWNER_PHONE,
            //               District = x.E.NAME,
            //               mdvr_core_sn = x.D.MDVR_CORE_SN,
            //               OperationLincese = x.A.OPERATION_LICENSE,
            //               Owner = x.A.OWNER,
            //               StartYear = x.A.START_YEAR,
            //               VehicleSn = x.A.VEHICLE_SN,
            //               VehicleType = (short)x.A.VEHICLE_TYPE,
            //               CarNumber = x.A.VEHICLE_ID,
            //               Note = x.A.NOTE,
            //           });
            //return q.FirstOrDefault();
            return null;
        }

        public int EndAlarm(PTMSEntities _context, AlarmArgs args)
        {
            if (args == null)
            {
                return 0;
            }

            var result = _context.ALM_911_DISPOSE.FirstOrDefault(x => x.ALARM_ID == args.AlarmId);
            if (result != null)
            {
                result.CONTENT = args.Content;
                result.DISPOSE_STAFF = args.Dispatcher;
                result.FORWARDED_FLAG = (short)args.AlarmType;
                result.DISPOSE_TIME = args.DispatchEndTime;
                _context.SaveChanges();
            }

            return result != null ? 1 : 0;

        }


        //private Location LocationConvert(VEHICLE_LOCATION result)
        //{
        //    return new Location
        //    {
        //        DIRECTION = Conv_Imp(result.DIRECTION, 3),
        //        GPS_TIME = result.GPS_TIME ?? DateTime.MinValue,
        //        LATITUDE = Conv_Imp(result.LATITUDE, 1),
        //        LONGITUDE = Conv_Imp(result.LONGITUDE, 0),
        //        SPEED = Conv_Imp(result.SPEED, 2),
        //        CAR_NUMBER = result.VEHICLE_ID,
        //    };
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="option">0-经度,1-纬度,2-速度,3-方向</param>
        /// <returns></returns>
        private decimal Conv_Imp(string value, int option)
        {
            System.Globalization.CultureInfo culture = new System.Globalization.CultureInfo("es-ES");

            decimal result = -1;
            switch (option)
            {
                case 0:
                    decimal.TryParse(GISConvertHelper.GetLongitude(value), out result);
                    break;
                case 1:
                    decimal.TryParse(GISConvertHelper.GetLatitude(value), out result);
                    break;
                case 2:
                    decimal.TryParse(GISConvertHelper.GetSpeed(value, culture), out result);
                    break;
                case 3:
                    decimal.TryParse(GISConvertHelper.GetDirection(value, culture), out result);
                    break;
            }

            return result;
        }


    }
}
