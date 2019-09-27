/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 31ffacb7-006d-4c93-b6e4-8cab231ad05d      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-WANGZS
/////                 Author: TEST(wangzs)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Manager.Test.TestDataModel
/////    Project Description:    
/////             Class Name: ADAccountTestInfo
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/8/2 15:02:46
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/8/2 15:02:46
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Gsafety.PTMS.Manager.Test.TestDataModel
{
    public class ADAccountTestInfo
    {
        [XmlAttribute("add")]
        public string Addusername { get; set; }
        public string Addusername2 { get; set; }
        public string Adduserpwd { get; set; }
        public string Addgroupname { get; set; }
        public string Addgroupname1 { get; set; }
        public string Addcompany { get; set; }
        public string Adddesciption { get; set; }
        public string Addphone { get; set; }
        public string Deleteuser { get; set; }
        public string Updateusername { get; set; }
        public string Updatephone { get; set; }
        public string Updatedescription { get; set; }
       


        
    }
}
