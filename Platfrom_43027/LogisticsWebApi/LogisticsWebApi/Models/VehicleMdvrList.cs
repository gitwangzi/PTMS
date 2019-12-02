using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogisticsWebApi.Models
{
    public class VehicleMdvrList
    {
        public List<VehicleMdvr> Data { get; set; }
        public int TotalCount { get; set; }
       
    }
}
