
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
    
public partial class RUN_MDVRMESSAGE_VEHICLE
{

    public string ID { get; set; }

    public string MESSAGE_ID { get; set; }

    public string VEHICLE_ID { get; set; }

    public System.DateTime SEND_TIME { get; set; }

    public Nullable<int> PACKET_SEQ { get; set; }

    public int STATUS { get; set; }

    public System.DateTime CREATE_TIME { get; set; }

    public string MDVR_CORE_SN { get; set; }

    public string CONTENT { get; set; }

    public Nullable<int> MESSAGE_TYPE { get; set; }

}

}
