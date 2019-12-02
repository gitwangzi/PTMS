using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogisticsWebApi.Models
{
    public class TrafficRoute
    {
     
       
        public string NAME { get; set; }
        public string PTS { get; set; }
        public string CREATOR { get; set; }
        public string POINT_COUNT { get; set; }
        public string WIDTH { get; set; }
        public string MAX_SPEED { get; set; }
        public string OVER_SPEED_DURATION { get; set; }
        
       
    }
}
