/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 6cabc730-2d03-488d-8695-cba2d63b262f      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-DENGZL
/////                 Author: TEST(dengzl)
/////======================================================================
/////           Project Name: Librarys
/////    Project Description:    
/////             Class Name: TreeViewModel
/////          Class Version: v1.0.0.0
/////            Create Time: 11/20/2013 2:31:49 PM
/////      Class Description:  
/////======================================================================
/////          Modified Time: 11/20/2013 2:31:49 PM
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    public class TreeViewModel<T>
    {
        private readonly Func<T, IEnumerable<T>> getChildFunction;

        public TreeViewModel(T root, Func<T, IEnumerable<T>> getChildFunction)
        {
            this.getChildFunction = getChildFunction;
            Nodes = new ObservableCollection<NodeViewModel<T>>();
            Nodes.Add(new NodeViewModel<T>(this, root, 0, getChildFunction));
        }

        public TreeViewModel(ObservableCollection<T> roots, Func<T, IEnumerable<T>> getChildFunction)
        {
            this.getChildFunction = getChildFunction;
            Nodes = new ObservableCollection<NodeViewModel<T>>();
            ThirdNodes = new ObservableCollection<NodeViewModel<T>>();
            if (roots != null)
            {
                foreach (var root in roots)
                {
                    Nodes.Add(new NodeViewModel<T>(this, root, 0, getChildFunction));
                }
            }
        }



        public void ToggleExpanded(NodeViewModel<T> nodeViewModel)
        {
            var index = Nodes.IndexOf(nodeViewModel);
      
            if (nodeViewModel.IsExpanded && nodeViewModel.Children != null)
            {
                foreach (var child in nodeViewModel.Children)
                {
                    index++;
                    NodeViewModel<T> nodeVM = new NodeViewModel<T>(this, nodeViewModel, child, nodeViewModel.Indentation + 1, getChildFunction);
                    if (nodeVM.Indentation == 2)
                    {
                        ThirdNodes.Add(nodeVM);
                    }
                    Nodes.Insert(index, nodeVM);
                }
            }

            if (!nodeViewModel.IsExpanded)
            {
                while (Nodes.Count > index + 1 && nodeViewModel.Indentation < Nodes[index + 1].Indentation)//[index + 1].Indentation)
                {
                    Nodes.RemoveAt(index + 1);
                }
            }
        }

        public void SearchExpanded(int level,ParentLevelTree<T> parentLevelTree)
        {
            if (parentLevelTree == null || parentLevelTree.Parentree == null || parentLevelTree.Parentree.Count == 0)
                return;
            else
            {
                ParentLevelInfo<T> parentLevelInfo = parentLevelTree.Parentree.Where(item => item.ParentLevel.Equals(level)).FirstOrDefault();
                if (parentLevelInfo != null)
                {
                    T model = parentLevelInfo.Model;
                    NodeViewModel<T> nodeViewModel = Nodes.Where(item => item.Model.Equals(model)).FirstOrDefault();
                    if (nodeViewModel.IsExpanded == false)
                    {
                        parentLevelInfo.IsFirstLoad = true;
                        nodeViewModel.IsExpanded = true;
                    }
                    else
                    {
                        parentLevelInfo.IsFirstLoad = false;
                    }
                    level++;
                    SearchExpanded(level, parentLevelTree);
                }
                else
                {
                    return;
                }
            }
        }

        public ObservableCollection<NodeViewModel<T>> ThirdNodes
        {
            get;
            private set;
        }

        public ObservableCollection<NodeViewModel<T>> Nodes
        {
            get;
            private set;
        }
    }
}
