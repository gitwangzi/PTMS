/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 3e844145-8535-40d1-aa0d-fe0a0901891f      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-DENGZL
/////                 Author: TEST(dengzl)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Bases.Librarys.TreeView
/////    Project Description:    
/////             Class Name: ParentLevelTree
/////          Class Version: v1.0.0.0
/////            Create Time: 11/27/2013 10:28:32 AM
/////      Class Description:  
/////======================================================================
/////          Modified Time: 11/27/2013 10:28:32 AM
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System;
using System.Collections.Generic;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Linq;

namespace Gsafety.PTMS.Bases.Librarys
{
    public class ParentLevelTree<T>
    {
        List<ParentLevelInfo<T>> _ParentTree;
        int _MaxLevel = 0;

        public int MaxLevel
        {
            get { return _MaxLevel; }

        }


        public List<ParentLevelInfo<T>> Parentree
        {
            get
            {
                return _ParentTree;
            }
        }

        public ParentLevelTree()
        {
            _ParentTree = new List<ParentLevelInfo<T>>();
        }

        public void AddParentLevelInfo(ParentLevelInfo<T> parentLevelInfo)
        {
            ParentLevelInfo<T> existparent = _ParentTree.Where(item => item.ParentLevel.Equals(parentLevelInfo.ParentLevel)).FirstOrDefault();
            if (existparent != null)
            {
                existparent.ParentNodeName = parentLevelInfo.ParentNodeName;
            }
            else
            {
                _ParentTree.Add(parentLevelInfo);
                if (_MaxLevel < parentLevelInfo.ParentLevel)
                    _MaxLevel = parentLevelInfo.ParentLevel;
            }
        }


    }
}
