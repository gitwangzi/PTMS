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
using Gsafety.PTMS.Bases.Librarys;
using Gsafety.PTMS.Bases.Models;
using Jounce.Core.Model;
using System.ComponentModel.Composition;
using Jounce.Core.Event;
using System.Collections.ObjectModel;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using Gsafety.PTMS.ServiceReference.VehicleMonitorService;
using Gsafety.PTMS.ServiceReference.MessageService;
using Gsafety.PTMS.ServiceReference.MessageServiceExt;
using System.Linq.Expressions;
using Jounce.Core.ViewModel;
using Jounce.Framework;

namespace Gsafety.PTMS.Share
{
    public class VehicleTreeFactory : BaseNotify, IEventSink<DeviceInstallVehicle>, IEventSink<UpdateVehicle>
    {
        #region 属性
        private ObservableCollection<OrganizationEx> _vehicleTrees;
        /// <summary>
        /// 组织机构和车辆的树
        /// </summary>
        public ObservableCollection<OrganizationEx> VehicleTrees
        {
            get
            {
                return _vehicleTrees;
            }
            set
            {
                _vehicleTrees = value;
                RaisePropertyChanged(() => VehicleTrees);
            }
        }

        private List<VehicleEx> _vehicleList = new List<VehicleEx>();
        /// <summary>
        /// 树中选中的车辆列表
        /// </summary>
        public List<VehicleEx> SelectVehicleList
        {
            get
            {
                return _vehicleList.Where(t => t.IsChecked).ToList();
            }
        }

        /// <summary>
        /// 树中所有车辆的列表
        /// </summary>
        public List<VehicleEx> VehicleList
        {
            get
            {
                return _vehicleList;
            }
        }
        #endregion


        #region 构造函数
        public VehicleTreeFactory(Func<Gsafety.PTMS.Bases.Models.Vehicle, bool> filter = null)
        {
            CreatingVehicleTree(filter);

            ApplicationContext.Instance.EventAggregator.Subscribe<DeviceInstallVehicle>(this);
            ApplicationContext.Instance.EventAggregator.Subscribe<UpdateVehicle>(this);
        }
        #endregion


        #region 方法
        List<OrganizationEx> orgExs;
        private void CreatingVehicleTree(Func<Gsafety.PTMS.Bases.Models.Vehicle, bool> filter)
        {

            _vehicleTrees = new ObservableCollection<OrganizationEx>();
            var orgs = ApplicationContext.Instance.BufferManager.VehicleOrganizationManage.AuthorityVehicleOrgs;
            orgExs = orgs.Select(org => new OrganizationEx(org)).ToList();

            _vehicleTrees = TreeNodeFactory.CreateTreeForestFromModelList<OrganizationEx>(orgExs, orgEx => orgEx.Organization.ID, orgEx => orgEx.Organization.ParentID, orgEx => orgExs.Any(t => t.Organization.ID == orgEx.Organization.ParentID) == false);

            var vehicles = ApplicationContext.Instance.BufferManager.VehicleOrganizationManage.VehicleList.ToList();
            if (filter != null)
            {
                vehicles = vehicles.Where(filter).ToList();
            }

            var vehicleDic = vehicles.GroupBy(a => a.OrganizationID).ToDictionary(a => a.Key, b => b.ToList());
            try
            {
                foreach (OrganizationEx org in _vehicleTrees)
                {
                    FillVehicle(vehicleDic, org);
                }
                JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => VehicleTrees));
            }
            catch (System.Exception ex)
            {
            }
        }

        private void FillVehicle(Dictionary<string, List<Gsafety.PTMS.Bases.Models.Vehicle>> vehicleDic, OrganizationEx org)
        {
            if (vehicleDic.ContainsKey(org.Organization.ID))
            {
                var currentVehicles = vehicleDic[org.Organization.ID];
                foreach (var v in currentVehicles)
                {
                    //v.IsOnLine = false;
                    var ex = new VehicleEx(v)
                    {
                        Parent = org,
                    };

                    _vehicleList.Add(ex);
                    org.Children.Add(ex);
                }
            }

            foreach (var item in org.Children)
            {
                var s = item as OrganizationEx;
                if (s == null)
                {
                    continue;
                }
                FillVehicle(vehicleDic, s);
            }
        }

        public void SearchVehicleTree(string filterInfo)
        {
            //CreatingVehicleTree();
            SearchVehicleTreeWithVisibility(_vehicleTrees, filterInfo);
        }

        private void SearchVehicleTreeWithVisibility(ObservableCollection<OrganizationEx> vehicleTrees, string filterInfo)
        {
            bool isContain = false;

            if (string.IsNullOrWhiteSpace(filterInfo))
            {
                isContain = true;
            }
            else
            {
                filterInfo = filterInfo.Trim();
            }

            Func<TreeNode, bool> nodeContainFunc = org => ((OrganizationEx)org).Organization.Name.Contains(filterInfo);
            Func<TreeNode, bool> childContainFunc = org => ((VehicleEx)org).VehicleInfo.Name.Contains(filterInfo) || ((VehicleEx)org).VehicleInfo.VehicleTypeDescribe.Contains(filterInfo);

            TreeNodeFactory.SearchTreeWithVisibility(vehicleTrees, nodeContainFunc, childContainFunc, isContain);
        }

        public void Add(Gsafety.PTMS.Bases.Models.Vehicle v)
        {
            var org = orgExs.FirstOrDefault(n => n.Organization.ID == v.OrganizationID);
            if (org != null)
            {
                VehicleEx ex = new VehicleEx(v) { Parent = org };
                _vehicleList.Add(ex);
                org.Children.Add(ex);
            }
        }

        public void Update(Gsafety.PTMS.Bases.Models.Vehicle v)
        {
            var oldve = ApplicationContext.Instance.BufferManager.VehicleOrganizationManage.VehicleList.FirstOrDefault(n => n.VehicleId == v.VehicleId);
            if (oldve != null)
            {
                oldve.OrganizationID = v.OrganizationID;
                oldve.OrganizationName = v.OrganizationName;
                //车牌号
                oldve.VehicleId = v.VehicleId;//非空
                oldve.VehicleSn = v.VehicleSn;//车架号
                oldve.EngineId = v.EngineId;//发动机号
                oldve.DistrictCode = v.DistrictCode;//所属区划
                oldve.ProvinceName = v.ProvinceName;

                if (v.CityCode != null)
                {

                    oldve.CityCode = v.CityCode;
                }

                if (v.CityName != null)
                {
                    oldve.CityName = v.CityName;

                }
                oldve.BrandModel = v.BrandModel;
                oldve.Owner = v.Owner;//车主
                oldve.VehicleTypeDescribe = v.VehicleTypeDescribe;
                oldve.VehicleTypeImage = v.VehicleTypeImage;
                oldve.ContactPhone = v.ContactPhone;
                oldve.StartYear = v.StartYear;
                oldve.VehicleServiceType = v.VehicleServiceType;
                CreatingVehicleTree(null);
            
            }

            //var org = orgExs.FirstOrDefault(n => n.Organization.ID == v.OrganizationID);
            //if (org != null)
            //{
            //    var oldve = _vehicleList.FirstOrDefault(n => n.VehicleId == v.VehicleId);
            //    if (oldve != null)
            //    {
            //        _vehicleList.Remove(oldve);
            //        VehicleTrees.FirstOrDefault(n => n.Organization.ID == v.OrganizationID).Children.Remove();
            //        orgExs.FirstOrDefault(n => n.Organization.ID == v.OrganizationID).Children.Remove(oldve);

            //        Gsafety.PTMS.Bases.Models.Vehicle newve = oldve.VehicleInfo;
            //        newve.OrganizationID = v.OrganizationID;
            //        newve.OrganizationName = v.OrganizationName;
            //        //车牌号
            //        newve.VehicleId = v.VehicleId;//非空
            //        newve.VehicleSn = v.VehicleSn;//车架号
            //        newve.EngineId = v.EngineId;//发动机号
            //        newve.DistrictCode = v.DistrictCode;//所属区划
            //        newve.ProvinceName = v.ProvinceName;

            //        if (v.CityCode != null)
            //        {

            //            oldve.VehicleInfo.CityCode = v.CityCode;
            //        }

            //        if (v.CityName != null)
            //        {
            //            oldve.VehicleInfo.CityName = v.CityName;

            //        }
            //        newve.BrandModel = v.BrandModel;
            //        newve.Owner = v.Owner;//车主
            //        newve.VehicleTypeDescribe = v.VehicleTypeDescribe;
            //        newve.ContactPhone = v.ContactPhone;
            //        newve.StartYear = v.StartYear;
            //        newve.VehicleServiceType = v.VehicleServiceType;

            //        VehicleEx ex = new VehicleEx(newve) { Parent = org };
            //        _vehicleList.Add(ex);
            //        org.Children.Add(ex);


            //    }
            //}
        }

        #endregion


        public void HandleEvent(DeviceInstallVehicle publishedEvent)
        {
            try
            {
                var v = publishedEvent.Vehicle as Gsafety.PTMS.Bases.Models.Vehicle;
                if (v != null)
                {
                    Add(v);
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("VehicleTreeFactory.HandleEvent", ex);
            }
        }

        public void HandleEvent(UpdateVehicle publishedEvent)
        {
            try
            {
                var v = publishedEvent.Vehicle as Gsafety.PTMS.Bases.Models.Vehicle;
                if (v != null)
                {
                    Update(v);
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("VehicleTreeFactory.HandleEvent", ex);
            }
        }
    }
}
