/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 57b13764-abe3-4809-8fbe-1c45c7f52b32      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-DENGZL
/////                 Author: TEST(dengzl)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Share.Model
/////    Project Description:    
/////             Class Name: MonitorEntity
/////          Class Version: v1.0.0.0
/////            Create Time: 11/20/2013 3:23:42 PM
/////      Class Description:  
/////======================================================================
/////          Modified Time: 11/20/2013 3:23:42 PM
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System;
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
using Jounce.Core.Model;
using Microsoft.Expression.Interactivity.Core;

namespace Gsafety.PTMS.Bases.Models
{
    public abstract class MonitorEntity : BaseNotify
    {
        public abstract ObservableCollection<MonitorEntity> GetChilds();
        public Visibility DiscriptionVisibility { get; protected set; }
        public Visibility FunctionKeyVisibility { get; protected set; }

        public Visibility CheckVisibility { get; protected set; }               

        public virtual string Name { get; set; }
        public ICommand CheckedCommand { get; set; }

        public virtual bool IsChecked { get; set; }
    }
}
