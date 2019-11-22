/////Copyright (C)2014Microsoft All Rights Reserved.
/////======================================================================
/////                   Guid: 0039c670-5b0f-418f-8565-eb30d2394b3f      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-JIANGJ
/////                 Author: TEST(JiangJ)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Bases.Models
/////    Project Description:    
/////             Class Name: IModelEntity
/////          Class Version: v1.0.0.0
/////            Create Time: 2014/11/3 11:38:48
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2014/11/3 11:38:48
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
using System.Collections.ObjectModel;

namespace Gsafety.PTMS.Bases.Models
{
    public interface IModelEntity
    {
        ObservableCollection<IModelEntity> GetChilds();
        
        Visibility DiscriptionVisibility { get; set; }
        Visibility FunctionKeyVisibility { get; set; }
        Visibility CheckVisibility { get; set; }

        bool IsChecked { get; set; }
        bool IsSelected { get; set; }
        bool IsExpanded { get; set; }

        string Name { get; set; }
        string Code { get; set; }
    }
}
