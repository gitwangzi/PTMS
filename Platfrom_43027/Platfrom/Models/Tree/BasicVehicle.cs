/////Copyright (C)2014Microsoft All Rights Reserved.
/////======================================================================
/////                   Guid: 29347af9-5414-4b3b-9453-acb517d32b8e      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-JIANGJ
/////                 Author: TEST(JiangJ)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Bases.Models
/////    Project Description:    
/////             Class Name: BasicVehicle
/////          Class Version: v1.0.0.0
/////            Create Time: 2014/10/31 12:54:04
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2014/10/31 12:54:04
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
using Gsafety.PTMS.Bases.Enums;
using Jounce.Core.Model;
using Gsafety.PTMS.Bases.Librarys;
using System.Collections.ObjectModel;

namespace Gsafety.PTMS.Bases.Models
{
    public class BasicVehicle : BaseNotify 
    {

        public BasicVehicleType Parent;
        public string VehicleId { get; set; }         
        private string _MDVRSN;
        public string MDVRSN
        {
            get
            {
                return _MDVRSN;
            }
            set
            {
                _MDVRSN = value;
            }
        }

        private string _BrandModel;
        public string BrandModel
        {
            get { return _BrandModel; }
            set { _BrandModel = value; }
        }

        private string _EngineId;
        public string EngineId
        {
            get { return _EngineId; }
            set { _EngineId = value; }
        }

        private string _Owner;
        public string Owner
        {
            get { return _Owner; }
            set { _Owner = value; }
        }

        private string _StartYear;
        public string StartYear
        {
            get { return _StartYear; }
            set { _StartYear = value; }
        }

        private string _VehicleSn;
        public string VehicleSn
        {
            get { return _VehicleSn; }
            set { _VehicleSn = value; }
        }

        private string _CityCode;
        public string CityCode
        {
            get
            {
                return _CityCode;
            }
            set
            {
                _CityCode = value;
            }
        }

        private string _CityName;
        public string CityName
        {
            get { return _CityName; }
            set { _CityName = value; }
        }

        private string _ProvinceName;
        public string ProvinceName
        {
            get { return _ProvinceName; }
            set { _ProvinceName = value; }
        }

        private string _GroupId;

        private bool _IsTrace;
        private bool _IsLocate;
        
        private bool m_DriveRoute;
        private bool m_ElectricFence;
        private bool _ControlPoint;
        private bool _StopScheDule;

        #region Attributes
        public string Name
        {
            get
            {
                return VehicleId;
            }
        }

        
              
        public VehicleType VehicleType { get; set; }                  
        public string ID { get; set; }

        /// <summary>
        /// 是否显示路线
        /// </summary>
        public bool DriveRoute
        {
            get { return m_DriveRoute; }
            set
            {
                m_DriveRoute = value;
                RaisePropertyChanged(() => DriveRoute);
            }
        }

        /// <summary>
        /// 是否显示围栏
        /// </summary>
        public bool ElectricFence
        {
            get { return m_ElectricFence; }
            set
            {
                m_ElectricFence = value;
                RaisePropertyChanged(() => ElectricFence);
            }
        }

        /// <summary>
        /// 是否显示监控点
        /// </summary>
        public bool ControlPoint
        {
            get { return _ControlPoint; }
            set
            {
                _ControlPoint = value;
                RaisePropertyChanged(() => ControlPoint);
            }
        }

        /// <summary>
        /// 是否显示行驶计划
        /// </summary>
        public bool StopScheDule
        {
            get { return _StopScheDule; }
            set
            {
                _StopScheDule = value;
                RaisePropertyChanged(() => StopScheDule);
            }
        }
        /// <summary>
        /// 是否定位 
        /// </summary>
        public bool IsLocate
        {
            get { return _IsLocate; }
            set
            {
                _IsLocate = value;
                RaisePropertyChanged(() => IsLocate);

            }
        }

        /// <summary>
        /// 是否可以单独移除定位按钮
        /// </summary>
        //public bool CanRemoveLocate
        //{
        //    get
        //    {
        //        if ((IsMonitor==false) && (IsLocate))
        //        {
        //            return true;
        //        }
        //        else
        //        {
        //            return false;
        //        }
        //    }
        //}

        /// <summary>
        /// 是否跟踪
        /// </summary>
        public bool IsTrace
        {
            get { return _IsTrace; }
            set
            {
                _IsTrace = value;
                if (_IsLocate && _IsTrace)
                {
                    IsLocate = false;
                }
                RaisePropertyChanged(() => IsTrace);
            }
        }


        public string GroupID
        {
            get
            {
                return _GroupId;
            }
            set
            {
                _GroupId = value;
                RaisePropertyChanged(() => GroupID);
                RaisePropertyChanged(() => groupInfo);
            }
        }

        public NodeViewModel<IModelEntity> groupInfo
        {
            get
            {
                return null;
            }
        }

        
        public Visibility IsVisibleToChoose
        {
            get;
            set;
        }
        #endregion
        public BasicVehicle()
        {
            IsMonitor = true;
        }

        private bool _IsOnLine;
        /// 是否在线
        /// </summary>
        public bool IsOnLine
        {
            get { return _IsOnLine; }
            set
            {
                _IsOnLine = value;
                //if (null != _parent) _parent.refresh();
                RaisePropertyChanged(() => IsOnLine);
                RaisePropertyChanged(() => OnLineInfo);
                RaisePropertyChanged(() => CanTrace);
            }
        }

        /// <summary>
        /// 在线信息
        /// </summary>
        public string OnLineInfo
        {
            get
            {
                if (_IsOnLine)
                {
                    return "1$" + (int)VehicleType;
                }
                return "0$" + (int)VehicleType;
            }
        }

        public bool CanTrace
        {
            get
            {
                if ((IsOnLine) && (IsMonitor == true))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        private bool? _IsMonitor = false;
        public bool? IsMonitor
        {
            get
            {
                return _IsMonitor;
            }
            set
            {
                if (value != _IsMonitor)
                {
                    _IsMonitor = value;
                    if (_IsMonitor == false) IsTrace = false;
                    RaisePropertyChanged(() => IsMonitor);
                    RaisePropertyChanged(() => CanTrace);
                    RaisePropertyChanged(() => IsTrace);
                }
            }
        }

        public void InitMonitor(bool _monitor)
        {
            _IsMonitor = _monitor;
        }
    }
}
