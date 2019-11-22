/////Copyright (C) Gsafety 2014 .All Rights Reserved.
/////======================================================================
/////                   Guid: b1f11989-ccc1-4a5a-8b4f-a85856b71390      
/////             clrversion: 4.0.30319.34011
/////Registered organization: 
/////           Machine Name: DENGZL
/////                 Author: DENGZL(dzl)
/////======================================================================
/////           Project Name: Gsafety.Ant.Message.Contract.Data.Traffic
/////    Project Description:    
/////             Class Name: ElectricFenceCMDSet
/////          Class Version: v1.0.0.0
/////            Create Time: 2014/2/26 06:44:53
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2014/2/26 06:44:53
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Gsafety.Ant.Message.Contract.Data
{
    [Serializable]
    [CollectionDataContract]
    public class ElectricFenceCMDSet : IEnumerable<ElectricFenceCMD>
    {
        [DataMember]
        public IList<ElectricFenceCMD> FenceCMDSet{get;set;}
        public void Add(ElectricFenceCMD obj)
        {
            FenceCMDSet.Add(obj);
        }
        public IEnumerator<ElectricFenceCMD> GetEnumerator()
        {
            return FenceCMDSet.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return FenceCMDSet.GetEnumerator();
        }
    }
}
