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
    
    public partial class TRF_UPGRADE_RECORD
    {
        public string ID { get; set; }
        public string CLIENT_ID { get; set; }
        public string SUITE_INFO_ID { get; set; }
        public string CURR_VERSION { get; set; }
        public string MDVR_CORE_SN { get; set; }
        public string DIRECTION { get; set; }
        public string ERROR_NUMBER { get; set; }
        public Nullable<System.DateTime> GPS_TIME { get; set; }
        public string GPS_VALID { get; set; }
        public Nullable<System.DateTime> LAST_UPDATE_TIME { get; set; }
        public string LAST_VERSION { get; set; }
        public string LONGITUDE { get; set; }
        public string LATITUDE { get; set; }
        public Nullable<System.DateTime> OPER_TIME { get; set; }
        public string OPERATOR { get; set; }
        public string ORIGINAL_CMD { get; set; }
        public string SPEED { get; set; }
        public Nullable<int> STATUS { get; set; }
        public Nullable<System.DateTime> UPDATE_END_TIME { get; set; }
        public string UPDATE_RESULT { get; set; }
        public Nullable<System.DateTime> UPDATE_START_TIME { get; set; }
        public string CONTENT { get; set; }
        public int PACKET_SEQ { get; set; }
    }
}
