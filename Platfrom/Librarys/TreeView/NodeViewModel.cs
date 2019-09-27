/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: c864a51b-c1cb-4fc4-9103-17f3458e3360      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-DENGZL
/////                 Author: TEST(dengzl)
/////======================================================================
/////           Project Name: Librarys
/////    Project Description:    
/////             Class Name: NodeViewModel
/////          Class Version: v1.0.0.0
/////            Create Time: 11/20/2013 2:28:53 PM
/////      Class Description:  
/////======================================================================
/////          Modified Time: 11/20/2013 2:28:53 PM
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
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
    public class NodeViewModel<T> : INotifyPropertyChanged
    {
        private TreeViewModel<T> parentViewModel;
        private NodeViewModel<T> parentNodeViewModel;

        private bool _IsOldExpanded = false;


        private readonly T model;
        private readonly int indentation;
        private readonly Func<T, IEnumerable<T>> getChildrenFunction;
        bool isExpanded = false;


        public NodeViewModel(TreeViewModel<T> parentModel, T model, int indentation, Func<T, IEnumerable<T>> getChildrenFunction)
        {
            this.parentViewModel = parentModel;
            this.model = model;
            this.indentation = indentation;
            this.getChildrenFunction = getChildrenFunction;
        }
        public NodeViewModel(TreeViewModel<T> parentModel, NodeViewModel<T> parentNode, T model, int indentation, Func<T, IEnumerable<T>> getChildrenFunction)
        {
            this.parentViewModel = parentModel;
            this.model = model;
            this.indentation = indentation;
            this.getChildrenFunction = getChildrenFunction;
            this.parentNodeViewModel = parentNode;
        }
        public int Indentation { get { return indentation; } }
        public double IndentationDistance { get { return indentation * 15; } }
        public T Model { get { return model; } }
        public bool IsOldExpanded
        {
            get { return _IsOldExpanded; }
            set { _IsOldExpanded = value; }
        }
        public NodeViewModel<T> ParentNodeViewModel
        {
            get { return parentNodeViewModel; }
            set { parentNodeViewModel = value; }
        }

        public bool IsExpanded
        {
            get
            {
                return isExpanded;
            }
            set
            {
                if (isExpanded != value)
                {
                    _IsOldExpanded = isExpanded;
                    isExpanded = value;
                    RaisePropertyChanged("IsExpanded");
                    RaisePropertyChanged("ArrowAngle");
                    _IsOldExpanded = isExpanded;
                    parentViewModel.ToggleExpanded(this);
                }
            }
        }


        public double ArrowAngle
        {
            get
            {
                if (isExpanded)
                {
                    return 225;
                }
                return 135;
            }
        }

        public Visibility ExpanderVisibility
        {
            get
            {
                if (this.Children != null && this.Children.Count() > 0)
                {
                    return Visibility.Visible;
                }
                else
                {
                    return Visibility.Collapsed;
                }
            }
            set
            {
                RaisePropertyChanged("ExpanderVisibility");
            }
        }

        public int ChildrenCount
        {
            get
            {
                return Children.Count();
            }
        }

        public IEnumerable<T> Children
        {
            get { return getChildrenFunction(this.model); }
        }

        protected void RaisePropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
