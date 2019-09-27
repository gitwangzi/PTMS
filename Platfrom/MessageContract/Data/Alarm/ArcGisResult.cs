/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: c63c53b4-ec93-4d69-a110-84bcfe7fb799      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-LIW
/////                 Author: TEST(liw)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Message.Contract.Data.Alarm
/////    Project Description:    
/////             Class Name: ArcGisResult
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/9/27 16:06:37
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/9/27 16:06:37
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gsafety.PTMS.Message.Contract.Data
{
    public class ArcGisResult
    {
        public address address { get; set; }
        public location location { get; set; }
    }

    public class address
    {
        public string State { get; set; }

    }

    public class location
    {
        public string x { get; set; }
        public string y { get; set; }
        public spatialReference spatialReference { get; set; }
    }

    public class spatialReference
    {
        public string wkid { get; set; }
        public string latestWkid { get; set; }
    }
}
