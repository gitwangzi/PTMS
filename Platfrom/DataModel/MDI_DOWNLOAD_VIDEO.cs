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
    
    public partial class MDI_DOWNLOAD_VIDEO
    {
        public string UUID { get; set; }
        public Nullable<int> CHANNEL_ID { get; set; }
        public string MDVR_CORE_SN { get; set; }
        public Nullable<int> STREAM_ID { get; set; }
        public Nullable<System.DateTime> START_TIME { get; set; }
        public Nullable<System.DateTime> END_TIME { get; set; }
        public Nullable<System.DateTime> DOWNLOAD_TIME { get; set; }
        public Nullable<System.DateTime> UPDATE_TIME { get; set; }
        public string FIRST_PTS { get; set; }
        public string DOWNLOAD_PTS { get; set; }
        public string MDVR_FILE { get; set; }
        public Nullable<int> OFFSET_FLAG { get; set; }
        public Nullable<int> OFFSET_START_TIME { get; set; }
        public Nullable<int> OFFSET_END_TIME { get; set; }
        public Nullable<int> DOWNLOAD_STATUS { get; set; }
        public Nullable<int> SOURCE_DOWNLOAD_SIZE { get; set; }
        public Nullable<int> STREAM_CONVERT_SIZE { get; set; }
        public Nullable<int> SOURCE_SIZE { get; set; }
        public Nullable<int> STOP_DOWNLOAD { get; set; }
        public string USER_DEP { get; set; }
        public string USER_NAME { get; set; }
        public string VIDEO_URL { get; set; }
        public Nullable<int> VIDEO_TYPE { get; set; }
        public Nullable<System.DateTime> CREATE_TIME { get; set; }
        public string VIDEO_NAME { get; set; }
        public string NOTE { get; set; }
    }
}
