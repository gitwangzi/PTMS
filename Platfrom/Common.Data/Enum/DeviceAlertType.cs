using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gsafety.PTMS.Common.Data
{
    public enum DeviceAlertType
    {
        GNSS_ModuleBroken = 0,

        GNSS_AerialNoExist = 1,

        GNSS_AerialBroken = 2,

        PowerUndervoltage = 3,

        PowerLost = 4,

        LED_Broken = 5,

        TTS_ModuleBroken = 6,

        CameraBroken = 7,
    }
}
