using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gsafety.PTMS.DBEntity;
using Gsafety.PTMS.Leader.Contract.Data;


namespace Gsafety.PTMS.Integration.Repository
{
    public class LeaderTerminalRepository
    {


        public static List<GPSModel> getGPSList(PTMSEntities ptms)
        {

            var VehicleInfo = from a in ptms.VEHICLE_LOCATION
                              group a by a.MDVR_CORE_SN
                                  into g
                                  select new VehicleModel
                                         {
                                             Mdvr_Id = g.Key,
                                             GpsTime =
                                                 g.Max(n => n.GPS_TIME != null ? n.GPS_TIME.Value : new DateTime())
                                         };
            var GPSInfo = from b in ptms.VEHICLE_LOCATION
                          from c in VehicleInfo
                          where b.GPS_TIME == c.GpsTime && b.MDVR_CORE_SN == c.Mdvr_Id
                          select new GPSModel
                                 {
                                     Longitude = b.LONGITUDE,
                                     Latitude = b.LATITUDE,
                                     Mdvr_Id = b.MDVR_CORE_SN,
                                     Vehcle_Id = b.VEHICLE_ID
                                 };

            List<GPSModel> lm = GPSInfo.ToList();

            for (int i = 0; i < lm.ToList().Count; i++)
            {
                lm[i].Latitude = ConvertBack(lm[i].Latitude, null, null, null);
                lm[i].Longitude = ConvertBack(lm[i].Longitude, null, null, null);
            }


            return lm;


        }
        public static string ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string temp = value.ToString().Replace(".", System.Globalization.NumberFormatInfo.CurrentInfo.NumberDecimalSeparator);
            int indflag = temp.IndexOf("-");
            temp = temp.Replace("-", "");

            int du = 0;
            double fen = 0;

            int ind = temp.IndexOf(System.Globalization.NumberFormatInfo.CurrentInfo.NumberDecimalSeparator);
            if (ind == -1)
            {
                ind = temp.Length;
            }
            if ((ind - 3 + 1) >= 1)
            {
                if (temp.Substring(0, ind - 3 + 1) != "") du = int.Parse(temp.Substring(0, ind - 3 + 1));
                if ((temp.Substring(ind - 2)) != "") fen = double.Parse(temp.Substring(ind - 2));
            }
            else
            {
                fen = double.Parse(temp);
            }

            if (indflag > -1) return (-du - fen / 60).ToString();
            else return (du + fen / 60).ToString();
        }
        public static int getMdvrCount(PTMSEntities ptms)
        {

            var Count = from a in ptms.SECURITY_SUITE_INFO
                        select a;

            return Count.ToList().Count;

        }
    }
}

