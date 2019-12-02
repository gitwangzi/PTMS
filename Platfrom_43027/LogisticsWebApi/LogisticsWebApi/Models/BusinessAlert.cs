using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LogisticsWebApi.Models
{
    public class BusinessAlert
    {


        public string AlertTime { get; set; }
       // public string AlertType { get; set; }     
        public string Direction { get; set; }      
        public string Latitude { get; set; }
        public string Longitude { get; set; }       
        public string Speed { get; set; }      
        public string VehicleId { get; set; }
     
    }
}