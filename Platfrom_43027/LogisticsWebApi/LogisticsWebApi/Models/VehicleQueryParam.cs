using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LogisticsWebApi.Models
{
    public class VehicleQueryParam
    {


        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double Radius { get; set; }      
     
    }
}