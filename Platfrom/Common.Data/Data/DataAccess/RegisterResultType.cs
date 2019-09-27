using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gsafety.PTMS.Common.Data
{
    public enum RegisterResultType
    {
        Success = 0,
        VehicleRegistered = 1,
        NoVehicle = 2,
        MdvrRegistered = 3,
        NoMdvr = 4,
    }
}
