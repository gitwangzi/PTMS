using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogisticsWebApi.Models
{
    public class VehicleList
    {
        public List<Vehicle> Data { get; set; }
        public int TotalCount { get; set; }


    }
}
