using System;
using System.ComponentModel;

namespace Gsafety.PTMS.Manager.Contract
{
    [Flags]
    public enum SuiteStatus : short
    {
        /// <summary>
        /// Initial
        /// </summary>
        [Description("Initial")]
        Initial = 10,
        /// <summary>
        ///  Testing
        /// </summary>
        [Description("Testing")]
        Testing = 22,
        /// <summary>
        /// Running
        /// </summary>
        [Description("Running")]
        Running = 23,
        /// <summary>
        /// Abnormal
        /// </summary>
        [Description( "Abnormal")]
        Abnormal = 24,
        /// <summary>
        /// Maintenance 
        /// </summary>
        [Description("Maintenance")]
        Maintenance = 30,
        /// <summary>
        /// Scrap
        /// </summary>
        [Description("Scrap")]
        Scrap = 40,
    }
}
