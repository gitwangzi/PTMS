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
    
    public partial class RUN_VIDEO_QUERY
    {
        public string ID { get; set; }
        public Nullable<int> PACKAGE_SEQ { get; set; }
        public string USER_ID { get; set; }
        public Nullable<System.DateTime> CREATE_TIME { get; set; }
        public Nullable<System.DateTime> SEND_TIME { get; set; }
        public Nullable<int> STREAM_TYPE { get; set; }
        public Nullable<int> FILET_YPE { get; set; }
        public string CHANNEL { get; set; }
        public string START_TIME { get; set; }
        public string END_TIME { get; set; }
        public string MDVR_CORE_SN { get; set; }
    }
}
