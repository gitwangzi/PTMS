/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 3ee65ddc-888f-487f-950e-dd9cacf2b83f      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-DENGZL
/////                 Author: TEST(dengzl)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Share.Model
/////    Project Description:    
/////             Class Name: City
/////          Class Version: v1.0.0.0
/////            Create Time: 9/2/2013 11:57:45 AM
/////      Class Description:  
/////======================================================================
/////          Modified Time: 9/2/2013 11:57:45 AM
/////            Modified by:
/////   Modified Description: 
/////======================================================================

namespace Gsafety.PTMS.Bases.Models
{

    public class City
    {
        public City(Province parent)
        {
            this.Parent = parent;
        }

        public string Code { get; set; }
        public string Name { get; set; }
        public Province Parent { get; set; }

    }
}
