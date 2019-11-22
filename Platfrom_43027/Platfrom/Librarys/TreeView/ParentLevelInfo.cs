/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 212df4e9-66c1-40e4-baec-115ade96fc89      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-DENGZL
/////                 Author: TEST(dengzl)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Bases.Librarys.TreeView
/////    Project Description:    
/////             Class Name: ParentLevelInfo
/////          Class Version: v1.0.0.0
/////            Create Time: 11/27/2013 10:31:04 AM
/////      Class Description:  
/////======================================================================
/////          Modified Time: 11/27/2013 10:31:04 AM
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Gsafety.PTMS.Bases.Librarys
{
    public class ParentLevelInfo<T>
    {
        public string ParentNodeName { get; set; }
        public int ParentLevel { get; set; }
        public T Model { get; set; }
        public bool IsFirstLoad { get; set; }
        public ParentLevelInfo(int level, string name,T model)
        {
            ParentLevel = level;
            ParentNodeName = name;
            Model = model;
        }
    }
}
