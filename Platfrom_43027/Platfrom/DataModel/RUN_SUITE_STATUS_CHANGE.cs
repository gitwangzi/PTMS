
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
    
public partial class RUN_SUITE_STATUS_CHANGE
{

    public string ID { get; set; }

    public string CLIENT_ID { get; set; }

    public string SUITE_INFO_ID { get; set; }

    public Nullable<int> OLD_STATUS { get; set; }

    public Nullable<int> NEW_STATUS { get; set; }

    public Nullable<System.DateTime> OPERATING_TIME { get; set; }

    public string OPERATING_PERSON { get; set; }

    public string CHANGE_REASON { get; set; }

}

}
