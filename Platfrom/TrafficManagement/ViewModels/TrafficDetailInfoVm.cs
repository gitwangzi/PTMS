/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: e8cf90c5-87a8-47e4-bddb-21ac3ec336e4      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-WANGYL
/////                 Author: TEST(wangyl)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Traffic.ViewModels
/////    Project Description:    
/////             Class Name: TrafficDetailInfoVm
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/10/25 10:43:16
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/10/25 10:43:16
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Linq;
using Jounce.Core.Model;
using System.ComponentModel;
using Gsafety.PTMS.Traffic.Models;
using System.IO;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using Gsafety.Common.CommMessage.Controls;
using System.Globalization;
using Gsafety.PTMS.Bases.Models;
using Gsafety.PTMS.ServiceReference.MessageService;
using Gsafety.PTMS.ServiceReference.TrafficManageService;
using Gsafety.PTMS.Share;
using Gsafety.PTMS.Bases.Enums;
using Gsafety.PTMS.Traffic.Views;
using Gsafety.Common.CommMessage;
using Jounce.Core.Command;
using Jounce.Core.Event;
using Jounce.Core.ViewModel;
using Jounce.Framework.Command;
using System.Reflection;
using System.Text;
using Gsafety.Common.Controls;

namespace Gsafety.PTMS.Traffic.ViewModels
{
    [ExportAsViewModel(TrafficName.TrafficManagerDetailInfoViewModel)]
    public class TrafficDetailInfoVm : BaseViewModel,
                IEventSink<ShowFenceInfoArgs>,
        IEventSink<ShowTrafficeMangerDetailInfoArgs>,
        //IEventSink<RefreshTrafficManagerListArgs>,
        IPartImportsSatisfiedNotification
    {

        #region Type enumeration fence
        /// <summary>
        /// Fence type structure
        /// </summary>
        public class TypeValue
        {
            /// <summary>
            /// Type Name
            /// </summary>
            public string strName;
            /// <summary>
            /// Type Value
            /// </summary>
            public int strValue;
            /// <summary>
            /// override ToString, returns the name
            /// </summary>
            /// <returns></returns>
            public override string ToString()
            {
                return strName;
            }
        }
        #endregion

        private TrafficFence _trafficFence;
        public TrafficFence TrafficFence
        {
            get { return _trafficFence; }
            set
            {
                _trafficFence = value;
                RaisePropertyChanged(() => TrafficFence);
                RaisePropertyChanged(() => IsControlSpeed);
                RaisePropertyChanged(() => IsControlTime);
                RaisePropertyChanged(() => InFenceAlarm);
                RaisePropertyChanged(() => OutFenceAlarm);
            }
        }


        public void OnImportsSatisfied()
        {
            EventAggregator.SubscribeOnDispatcher<ShowFenceInfoArgs>(this);
            EventAggregator.SubscribeOnDispatcher<ShowTrafficeMangerDetailInfoArgs>(this);
            //EventAggregator.SubscribeOnDispatcher<RefreshTrafficManagerListArgs>(this);
        }


        #region Control interface displays detailed information

        protected override void ActivateView(string viewName, IDictionary<string, object> viewParameters)
        {
            base.ActivateView(viewName, viewParameters);
            //IsVisual = _IsVisual;
        }

        private bool _IsVisual = false;
        public bool IsVisual
        {
            get { return _IsVisual; }
            set
            {
                if (IsOpen)
                {
                    IsOpen = _IsVisual = false;
                }
                else
                {
                    IsOpen = _IsVisual = true;
                }
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => IsVisual));
            }
        }

        private bool m_IsOpen = true;

        public bool IsOpen
        {
            get { return m_IsOpen; }
            set
            {
                m_IsOpen = value;
            }
        }

        public string Title { get; set; }
        public string PicUrl { get; set; }

        public void HandleEvent(ShowTrafficeMangerDetailInfoArgs publishedEvent)
        {
            if (publishedEvent.IsSpeedClick)
            {
                _IsVisual = false;
                IsOpen = false;
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => IsVisual));
            }
            else
            {
                IsVisual = publishedEvent.bShow;
            }

        }
        #endregion
        #region Display Control window title
        private string _DetailWindwoInfo = "";
        public string DetailWindwoInfo
        {
            get { return _DetailWindwoInfo; }
            set
            {
                _DetailWindwoInfo = value;
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => DetailWindwoInfo));
            }
        }
        #endregion

        #region Show details fence
        /// <summary>
        /// ShowFenceInfoArgs
        /// </summary>
        /// <param name="publishedEvent"></param>
        public void HandleEvent(ShowFenceInfoArgs publishedEvent)
        {
            try
            {
                //fence
                if (publishedEvent.Featuetype == TrafficFeature.Traffic_PolygonFence)
                {
                    if (publishedEvent.selectEleFence != null)
                    {
                        TrafficFence = publishedEvent.selectEleFence;
                    }
                }

            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("HandleEvent(ShowFenceInfoArgs publishedEvent)", ex);
            }
        }

        #endregion

        //public void HandleEvent(RefreshTrafficManagerListArgs publishedEvent)
        //{
        //    if (publishedEvent.nType == TrafficFeature.Traffic_PolygonFence)
        //    {
        //        TrafficFence = publishedEvent.UpdateItemInfo as TrafficFence;
        //    }
        //}

        public bool InFenceAlarm
        {
            get
            {
                if (TrafficFence == null || TrafficFence.RegionProperty == null)
                {
                    return false;
                }

                List<string> lst = TrafficFence.RegionProperty.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries).ToList<string>();
                return (lst.IndexOf((((int)(FENCE_RegionProperty.In_AlertToPlatform))).ToString()) > -1);
            }

        }

        public bool OutFenceAlarm
        {
            get
            {
                if (TrafficFence == null || TrafficFence.RegionProperty == null)
                {
                    return false;
                }

                List<string> lst = TrafficFence.RegionProperty.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries).ToList<string>();
                return (lst.IndexOf((((int)(FENCE_RegionProperty.Out_AlertToPlatform))).ToString()) > -1);
            }
        }

        public bool IsControlSpeed
        {
            get
            {
                if (TrafficFence == null || TrafficFence.RegionProperty == null)
                {
                    return false;
                }

                List<string> lst = TrafficFence.RegionProperty.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries).ToList<string>();
                return (lst.IndexOf((((int)(FENCE_RegionProperty.Speed_Limit))).ToString()) > -1);
            }
        }

        public bool IsControlTime
        {
            get
            {
                if (TrafficFence == null || TrafficFence.RegionProperty == null)
                {
                    return false;
                }

                List<string> lst = TrafficFence.RegionProperty.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries).ToList<string>();
                return (lst.IndexOf((((int)(FENCE_RegionProperty.Time_Limit))).ToString()) > -1);
            }
        }
    }
}
