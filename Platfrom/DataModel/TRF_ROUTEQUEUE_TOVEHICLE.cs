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
    
    public partial class TRF_ROUTEQUEUE_TOVEHICLE
    {
        public string ID { get; set; }
        public string CLIENT_ID { get; set; }
        public string RESULT_PACKET { get; set; }
        public Nullable<int> PACKET_SEQ { get; set; }
        public string VEHICLE_ID { get; set; }
        public string END_TIME { get; set; }
        public Nullable<int> REGION_ID { get; set; }
        public System.DateTime SEND_TIME { get; set; }
        public string MDVR_CORE_SN { get; set; }
        public int OPER_TYPE { get; set; }
        public Nullable<int> STATUS { get; set; }
        public Nullable<System.DateTime> CREATE_TIME { get; set; }
        public string START_TIME { get; set; }
        public string PTS { get; set; }
        public string ROUTE_PROPERTY { get; set; }
        public string ROUTE_SEGMENT_PROPERTY { get; set; }
        public Nullable<int> WIDTH { get; set; }
        public Nullable<int> MAX_SPEED { get; set; }
        public string NAME { get; set; }
        public Nullable<int> OVER_SPEED_DURATION { get; set; }
        public Nullable<int> POINT_COUNT { get; set; }
        public string ROUTE_ID { get; set; }
    }
}
