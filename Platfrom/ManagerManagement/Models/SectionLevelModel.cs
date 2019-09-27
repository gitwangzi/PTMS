/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: ebe8e57b-7f1d-449c-b241-a8ae8d3d0a45      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-WANGJINCAI
/////                 Author: TEST(JinCaiWang)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Alarm.Contract.Data
/////    Project Description:    
/////             Class Name: AlarmInfo
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/8/8 11:07:41
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/8/8 11:07:41
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
using System.Collections.Generic;

namespace Gsafety.PTMS.Manager.Model
{
    public class SectionLevelModel : ModelBase
    {
      
        #region Init

        public SectionLevelModel()
        {
            this.Items = new List<SerializableItem>();
            this.Items.Add(new LevelModelItem
            {
                Desc = "System",
                Name = "System",
                IsDefault = true,

            });

            this.Items.Add(new LevelModelItem
            {
                Desc = "User",
                Name = "User"
            });
        }

        #endregion
    }
    public class LevelModelItem:SerializableItem 
    {
        
    }
}
