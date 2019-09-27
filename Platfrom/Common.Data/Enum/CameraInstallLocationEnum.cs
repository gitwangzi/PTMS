using Gsafety.PTMS.Bases.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gs.PTMS.Common.Data.Enum
{
    public enum CameraInstallLocationEnum
    {
        [EnumAttribute(Description = "CameraInstallLocation_OuterBefore", ResourceName = "CameraInstallLocation_OuterBefore")]
        OuterBefore = 0,
        [EnumAttribute(Description = "CameraInstallLocation_InnerLeftDriver", ResourceName = "CameraInstallLocation_InnerLeftDriver")]
        InnerLeftDriver,
        [EnumAttribute(Description = "CameraInstallLocation_InnerRightDriver", ResourceName = "CameraInstallLocation_InnerRightDriver")]
        InnerRightDriver,
        [EnumAttribute(Description = "CameraInstallLocation_InnerCenter", ResourceName = "CameraInstallLocation_InnerCenter")]
        InnerCenter,
        [EnumAttribute(Description = "CameraInstallLocation_OuterLeft", ResourceName = "CameraInstallLocation_OuterLeft")]
        OuterLeft,
        [EnumAttribute(Description = "CameraInstallLocation_OuterRight", ResourceName = "CameraInstallLocation_OuterRight")]
        OuterRight,
        [EnumAttribute(Description = "CameraInstallLocation_InnerBehind", ResourceName = "CameraInstallLocation_InnerBehind")]
        InnerBehind,
        [EnumAttribute(Description = "CameraInstallLocation_OuterBehind", ResourceName = "CameraInstallLocation_OuterBehind")]
        OuterBehind,
    }
}
