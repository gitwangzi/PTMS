
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
    
public partial class MDI_LIVE_VIDEO
{

    public string UUID { get; set; }

    public string MDVR_CORE_SN { get; set; }

    public int CHANNEL_ID { get; set; }

    public int STREAM_ID { get; set; }

    public System.DateTime START_TIME { get; set; }

    public System.DateTime END_TIME { get; set; }

    public Nullable<int> DURATION_TIME { get; set; }

    public string PICTURE_URL { get; set; }

    public Nullable<int> TEST_INSTALL { get; set; }

    public Nullable<System.DateTime> UPDATE_TIME { get; set; }

    public string VIDEO_URL { get; set; }

    public int VIDEO_SIZE { get; set; }

    public string EXT_DATA { get; set; }

    public Nullable<System.DateTime> CREATE_TIME { get; set; }

    public Nullable<int> IS_FINISH { get; set; }

    public string NOTE { get; set; }

}

}