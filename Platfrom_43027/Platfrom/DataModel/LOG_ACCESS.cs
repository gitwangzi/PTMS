
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
    
public partial class LOG_ACCESS
{

    public string ID { get; set; }

    public string CLIENT_ID { get; set; }

    public Nullable<System.DateTime> LOGIN_TIME { get; set; }

    public Nullable<System.DateTime> LOGOUT_TIME { get; set; }

    public string LOGIN_USER { get; set; }

    public Nullable<int> USER_TYPE { get; set; }

    public string SESSION_ID { get; set; }

    public string USER_ID { get; set; }

}

}