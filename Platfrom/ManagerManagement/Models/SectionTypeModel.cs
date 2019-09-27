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
using System.Linq;
using System.Collections.Generic;

namespace Gsafety.PTMS.Manager.Model
{
    public class SectionTypeModel:ModelBase 
    {
        
        #region Init

        public SectionTypeModel()
        {
            CollectItemControls();
        }

        private void CollectItemControls()
        {
            Items = typeof(ItemTypeDefine).Assembly.GetTypes()
                      .Where(x => x.IsSubclassOf(typeof(ItemTypeDefine))
                              && x.GetCustomAttributes(false)
                              .Any(y => y.GetType() == typeof(AppConfigControlAttribute))
                              && x.GetConstructor(System.Type.EmptyTypes) != null
                          )
                        .Select(x =>
                        {
                            var att = (AppConfigControlAttribute)x.GetCustomAttributes(typeof(AppConfigControlAttribute), false)[0];
                            var result = new TypeModelItem
                            {
                                Control = x,
                                Desc = att.Desc,
                                IsDefault = att.IsDefault,
                                Name = att.Name,
                            };

                            return (SerializableItem)result;
                        })
                         .ToList()
                         ;
        }
        #endregion

    }

    public class TypeModelItem:SerializableItem 
    {
        public Type Control { get; set; }
    }
}
