using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gsafety.PTMS.Common.Data
{
    public enum BussinessAlertType
    {
        OverSpeed = 0,

        InOutAera = 1,

        InOutRoute = 2,

        RouteOffset = 3,

        FatigueDrive = 4,

        DangerWarning = 5,

        OverSpeedWarning = 6,

        FatigueDriveWarning = 7,

        DriveTime = 8,

        VehicleStolen = 9,

        IllegalIgnition = 10,

        IllegalDisplacement = 11,

        CollisionWaring = 12,

        RolloverWaring = 13,
    }
}
