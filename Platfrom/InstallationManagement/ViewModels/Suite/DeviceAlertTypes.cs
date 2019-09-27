using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Gsafety.Ant.Installation.ViewModels.Suite
{
    public enum DeviceAlertTypes
    {
        MAINTAIN_Full_N=-1,
        GNSSModelError=0,
        GNSSNoAntenna=1,
        GNSSCircuit=2,
        PowerSourceNoVoltage=3,
        PowerSourceNoPower=4,
        LEDError=5,
        TTSError=6,
        VidiconError=7,

    }
}
