using Gsafety.PTMS.Bases.Librarys;
using Gsafety.PTMS.Bases.Models;
using Gsafety.PTMS.BasicPage.Monitor.ViewModels;
using Gsafety.PTMS.Share;
using Jounce.Core.Event;
using Jounce.Core.Model;
/////Copyright (C) Gsafety 2014 .All Rights Reserved.
/////======================================================================
/////                   Guid: 83a5d64a-e294-4b93-81b6-73a3b6515c11      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-ZHOUDD
/////                 Author: GJSY(zhoudd)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Manager.Models
/////    Project Description:    
/////             Class Name: VehicleManage
/////          Class Version: v1.0.0.0
/////            Create Time: 2014/10/27 9:31:11
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2014/10/27 9:31:11
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Gsafety.PTMS.Manager.Models
{
    /// <summary>
    /// Vehicle Management
    /// </summary>
    public class VehicleManage : BaseNotify
    {
        #region Fields
        [Import]
        public IEventAggregator EventAggregator { get; set; }
        private TreeViewModel<MonitorEntity> _VehicleTreeViews;
        public TreeViewModel<MonitorEntity> VehicleTreeViews
        {
            get
            {
                //if (_VehicleTreeViews == null || _VehicleTreeViews.Nodes.Count == 0)
                //    _VehicleTreeViews = new TreeViewModel<MonitorEntity>(new ObservableCollection<MonitorEntity>(ApplicationContext.Instance.BufferManager.DistrictManager.Provinces), (MonitorEntity e) => e.GetChilds());
                return _VehicleTreeViews;

            }
        }


        #endregion

        #region Attributes

        #endregion

        public VehicleManage()
        {
            CompositionInitializer.SatisfyImports(this);
        }
    }
}
