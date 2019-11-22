/////Copyright (C)2014Microsoft All Rights Reserved.
/////======================================================================
/////                   Guid: f41ff756-0c8a-4d3a-9219-29e7b553cfb4      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-JIANGJ
/////                 Author: TEST(JiangJ)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Bases.Models
/////    Project Description:    
/////             Class Name: VehicleGroup
/////          Class Version: v1.0.0.0
/////            Create Time: 2014/11/18 11:15:28
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2014/11/18 11:15:28
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System;
using System.ComponentModel.DataAnnotations;
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
using System.Linq;

namespace Gsafety.PTMS.Bases.Models
{

    public class UserDefineGroup
    {
        #region Fields
        private string _GroupName;
        private string m_flag;
        private string _Note;
        private string u_isEnabled;
        private string d_isEnabled;
        private ObservableCollection<Vehicle> m_Vehicles = new ObservableCollection<Vehicle>();
        #endregion

        public UserDefineGroup()
        {
        }

        #region Attribute
        public string CreateUser { get; set; }
        public short? GroupIndex { get; set; }
        public string ID { get; set; }

        [RegularExpression(@"^.{0,200}", ErrorMessageResourceType = typeof(Gsafety.Common.Localization.Resource.StringResource),
      ErrorMessageResourceName = "MONITOR_GroupNodeConditionDetail")]
        public string Note
        {
            get { return _Note; }
            set
            {
                _Note = value;
            }
        }

        [Required(ErrorMessageResourceType = typeof(Gsafety.Common.Localization.Resource.StringResource),
    ErrorMessageResourceName = "MONITOR_RequiredError")]
        [RegularExpression(@"^.{1,20}", ErrorMessageResourceType = typeof(Gsafety.Common.Localization.Resource.StringResource),
             ErrorMessageResourceName = "MONITOR_GroupNameConditionDetail")]
        public string GroupName
        {
            get
            {
                return _GroupName;
            }
            set
            {
                if (value != _GroupName)
                {
                    _GroupName = value;
                }
            }
        }

        public string Name
        {
            get { return _GroupName; }

        }

        public ObservableCollection<Vehicle> Vehicles
        {
            get { return m_Vehicles; }
            set { m_Vehicles = value; }
        }

        public string Flag
        {
            get { return m_flag; }
            set
            {
                m_flag = value;
            }
        }

        public string UP_IsEnabled
        {
            get { return u_isEnabled; }
            set
            {
                u_isEnabled = value;
            }
        }
        public string DOWN_IsEnabled
        {
            get { return d_isEnabled; }
            set
            {
                d_isEnabled = value;
            }
        }
        #endregion
    }
}
