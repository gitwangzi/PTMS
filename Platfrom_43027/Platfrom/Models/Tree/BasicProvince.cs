/////Copyright (C)2014Microsoft All Rights Reserved.
/////======================================================================
/////                   Guid: a3c53a43-595b-47c2-a318-c2d83bfdd9e7      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-JIANGJ
/////                 Author: TEST(JiangJ)
/////======================================================================
/////           Project Name: Gsafety.Ant.Bases.Models
/////    Project Description:    
/////             Class Name: BasicProvince
/////          Class Version: v1.0.0.0
/////            Create Time: 2014/10/31 10:09:41
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2014/10/31 10:09:41
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
using Jounce.Core.Model;
using System.Collections.ObjectModel;


namespace Gsafety.PTMS.Bases.Models
{
    public class BasicProvince 
    {
        #region Attributes

        public  string Code { get; set; }
        public string Name { get; set; }

        public ObservableCollection<BasicCity> _bCitys;

        #endregion


        public BasicProvince()
        {
                _bCitys = new ObservableCollection<BasicCity>();
        }
    }
}
