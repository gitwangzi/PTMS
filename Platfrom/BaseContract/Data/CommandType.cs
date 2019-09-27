/////Copyright (C) Gsafety 2014 .All Rights Reserved.
/////======================================================================
/////                   Guid: 52f210eb-85de-42ec-b353-edf26832627b      
/////             clrversion: 4.0.30319.34011
/////Registered organization: 
/////           Machine Name: DENGZL
/////                 Author: DENGZL(dzl)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Base.Contract.Data
/////    Project Description:    
/////             Class Name: CommandType
/////          Class Version: v1.0.0.0
/////            Create Time: 2014/3/14 03:22:40
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2014/3/14 03:22:40
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gsafety.PTMS.Base.Contract.Data
{
    public class CommandType
    {
        public const string PolygonsRegion = "PolygonsRegion";
        public const string RouteInfo = "RouteInfo";
        public const string SetTermParam = "SetTermParam";
        public const string QueryPartParam = "QueryPartParam";

        public const string C30 = "C30";
        public const string ElectronicFence = "C107";
        public const string OverSpeed = "C68";
        public const string C64 = "C64";
        public const string C78 = "C78";
        public const string C80 = "C80";
        public const string C82 = "C82";
    }
}
