using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Gsafety.PTMS.Traffic.Models
{
    public  class ConstDefineModel
    {
        /// <summary>
        /// Plans to apply to vehicles traveling default start time
        /// </summary>
        public static string StopScheDuleVehicleStartTime = DateTime.Today.ToShortDateString();
        /// <summary>
        /// Plans to apply to vehicles traveling default end time
        /// </summary>
        public static string StopScheDuleVehicleEndTime = DateTime.Today.AddMonths(1).ToShortDateString();
        /// <summary>
        /// Travel planning application to date selection of vehicles (all week)
        /// </summary>
        public static  string StopScheDuleVehicleWeekDay = "0,1,2,3,4,5,6";
        /// <summary>
        /// 10 km maximum radius (monitoring points scope of analysis)
        /// </summary>
        public static short MaxBufferRadius = 10000;
        /// <summary>
        /// The maximum time deviation (travel plan) 1 hour
        /// </summary>
        public static short MaxTolerance = 60 * 60;
        /// <summary>
        /// Speed ​​parameter sets the maximum duration of the default parameters for the half-hour
        /// </summary>
        public static short SpeedLimitMaxDuration = 30 * 60;
        /// <summary>
        /// Length of the name of the field of traffic management module default maximum (less than or equal to the maximum value of the data in the field)
        /// </summary>
        public static short NameMaxLength = 99;
        /// <summary>
        /// Traffic Management module addresses the default maximum length
        /// </summary>
        public static short AddressLength = 200;
        /// <summary>
        /// Maximum number of packets entity
        /// </summary>
        public static int PageMaxCount = 100;
    }
}
