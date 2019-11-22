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

namespace Gsafety.PTMS.Bases.Enums
{
    /// <summary>
    /// PlanResultType（0NotAsPlanned，1PerformAsPlanned，2PlanNotPerformed）
    /// </summary>
    public enum PlanResultType
    {
        /// <summary>
        /// NotAsPlanned
        /// </summary>
        NotAsPlanned = 0,
        /// <summary>
        /// PerformAsPlanned
        /// </summary>
        PerformAsPlanned = 1,
        /// <summary>
        /// PlanNotPerformed
        /// </summary>
        PlanNotPerformed = 2
    }
}
