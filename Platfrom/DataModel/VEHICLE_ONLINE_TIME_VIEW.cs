//------------------------------------------------------------------------------
// <auto-generated>
//    此代码是根据模板生成的。
//
//    手动更改此文件可能会导致应用程序中发生异常行为。
//    如果重新生成代码，则将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace Gsafety.PTMS.DBEntity
{
    using System;
    using System.Collections.Generic;
    
    public partial class VEHICLE_ONLINE_TIME_VIEW
    {
        public string ID { get; set; }
        public string VEHICLE_ID { get; set; }
        public string MDVR_CORE_SN { get; set; }
        public Nullable<System.DateTime> ONLINE_TIME { get; set; }
        public Nullable<System.DateTime> OFFLINE_TIME { get; set; }
        public Nullable<int> ONLINE_TIMESPAN { get; set; }
        public string DISTRICT_CODE { get; set; }
        public Nullable<int> DISTANCE { get; set; }
    }
}
