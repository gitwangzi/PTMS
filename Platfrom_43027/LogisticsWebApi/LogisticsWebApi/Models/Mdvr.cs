using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogisticsWebApi.Models
{
    public class Mdvr
    {
        public string SUITE_ID { get; set; }
        public string MDVR_CORE_SN { get; set; }
        public string MDVR_SN { get; set; }
        public string MDVR_SIM { get; set; }
        public string MDVR_SIM_MOBILE { get; set; }
        public string UPS_SN { get; set; }
        public string SD_SN { get; set; }
        public List<MdvrPart> PARTLIST { get; set; }
        public string INSTALL_STATUS { get; set; }
        
       
    }
}
