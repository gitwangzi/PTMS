
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
    
public partial class MTN_INSTALLATION_DETAIL
{

    public string ID { get; set; }

    public string VEHICLE_ID { get; set; }

    public string SUITE_INFO_ID { get; set; }

    public string MDVR_CORE_SN { get; set; }

    public Nullable<int> CHECKSTEP { get; set; }

    public string STATION_ID { get; set; }

    public string INSTALL_STAFF { get; set; }

    public string RECORD_STAFF { get; set; }

    public Nullable<System.DateTime> FINISH_TIME { get; set; }

    public string NOTE { get; set; }

    public Nullable<System.DateTime> CREATE_TIME { get; set; }

    public Nullable<int> VALID { get; set; }

}

}