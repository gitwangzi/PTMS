using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogisticsWebApi.Models
{
    public class Vehicle
    {
        public string VEHICLE_ID { get; set; }
        public string VEHICLE_SN { get; set; }
        public string ENGINE_ID { get; set; }  
        public string OPERATION_LICENSE { get; set; }
        public string OWNER { get; set; }
        public string CONTACT_ADDRESS { get; set; } 
        public string CONTACT_EMAIL { get; set; }
        public string CONTACT { get; set; }
        public string CONTACT_PHONE { get; set; }
        public string REGION { get; set; }
        public string NOTE { get; set; }
        public string ORGNIZATION_ID { get; set; }
        public string CREATOR { get; set; }
        public bool ISINSTALL  {get; set; }
        

    }
}
