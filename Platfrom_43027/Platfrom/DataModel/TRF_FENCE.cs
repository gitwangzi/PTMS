
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
    
public partial class TRF_FENCE
{

    public string ID { get; set; }

    public string CLIENT_ID { get; set; }

    public string NAME { get; set; }

    public Nullable<int> FENCE_TYPE { get; set; }

    public string PTS { get; set; }

    public Nullable<int> RADIUS { get; set; }

    public string CIRCLE_CENTER { get; set; }

    public string SHAPE { get; set; }

    public string CREATOR { get; set; }

    public Nullable<System.DateTime> CREATE_TIME { get; set; }

    public Nullable<int> VALID { get; set; }

    public string ADDRESS { get; set; }

    public string REGION_PROPERTY { get; set; }

    public string START_TIME { get; set; }

    public string END_TIME { get; set; }

    public Nullable<int> MAX_SPEED { get; set; }

    public Nullable<int> OVER_SPEED_DURATION { get; set; }

    public Nullable<int> POINT_COUNT { get; set; }

}

}