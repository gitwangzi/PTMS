using Gsafety.PTMS.Bases.Models;
using Gsafety.PTMS.ServiceReference.MessageService;
using Gsafety.PTMS.Share;
using Jounce.Core.Event;
using Jounce.Core.Model;
using Jounce.Core.ViewModel;
using System;
using System.Collections.Generic;
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
using System.Linq;
using System.Threading;
using Gsafety.PTMS.Bases.Librarys;
using Gsafety.PTMS.ServiceReference.OrganizationService;
using System.Reflection;
using Gsafety.PTMS.Bases.Enums;

namespace Gsafety.PTMS.BasicPage.Model
{
    public class OrganizationTreeFactory : BaseNotify
    {
        /// <summary>
        /// 数据加载完毕回调
        /// </summary>
        private Action _dataLoadCompleteAction;

        #region 属性
        private ObservableCollection<Organization> _authorityVehicleOrgs = new ObservableCollection<Organization>();

        private ObservableCollection<OrganizationEx> _vehicleTrees;
        /// <summary>
        /// 只包含组织机构的树
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

        private ObservableCollection<OrganizationEx> _orgnizationVehicleTrees;
        /// <summary>
        /// 组织机构的一元列表
        /// </summary>
        public ObservableCollection<OrganizationEx> OrgnizationVehicleTrees
        {
            get
            {
                return _orgnizationVehicleTrees;
            }
            set
            {
                _orgnizationVehicleTrees = value;
                RaisePropertyChanged(() => OrgnizationVehicleTrees);
            }
        }


        #endregion

        #region 构造函数
        public OrganizationTreeFactory(Action dataLoadCompleteAction = null)
        {
            _dataLoadCompleteAction = dataLoadCompleteAction;

            CreatingVehicleTree();
        }
        #endregion

        #region 方法
        private void CreatingVehicleTree()
        {
            var orgClient = ServiceClientFactory.Create<OrganizationClient>();

            if (ApplicationContext.Instance.AuthenticationInfo.Role.RoleCategory == (short)RoleCategory.SecurityAdmin || ApplicationContext.Instance.AuthenticationInfo.Role.RoleCategory == (short)RoleCategory.SecurityMonitor)
            {
                orgClient.GetOrganizationByUserCompleted += orgClient_GetOrganizationByUserCompleted;
                orgClient.GetOrganizationByUserAsync(ApplicationContext.Instance.AuthenticationInfo.UserID);
            }
            else if (ApplicationContext.Instance.AuthenticationInfo.Role.RoleCategory == (short)RoleCategory.MaintainAdmin || ApplicationContext.Instance.AuthenticationInfo.Role.RoleCategory == (short)RoleCategory.MaintainMonitor)
            {
                orgClient.GetOrganizationByUserCompleted += orgClient_GetOrganizationByUserCompleted;
                orgClient.GetOrganizationByUserAsync(ApplicationContext.Instance.AuthenticationInfo.UserID);
            }
        }

        void orgClient_GetAllOrganizationCompleted(object sender, GetAllOrganizationCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                return;
            }

            if (e.Error != null)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), e.Error);
                return;
            }

            try
            {
                var result = e.Result;
                if (result.IsSuccess == false)
                {
                    if (string.IsNullOrWhiteSpace(result.ErrorMsg) == false)
                    {
                        ApplicationContext.Instance.Logger.LogError(MethodBase.GetCurrentMethod().ToString(), result.ErrorMsg);
                    }

                    if (result.ExceptionMessage != null)
                    {
                        ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), result.ExceptionMessage);
                    }
                }

                if (result.Result != null)
                {
                    _authorityVehicleOrgs = result.Result;

                    var orgs = _authorityVehicleOrgs;
                    var orgExs = orgs.Select(org => new OrganizationEx(org)).ToList();

                    _orgnizationVehicleTrees = new ObservableCollection<OrganizationEx>(orgExs);
                    VehicleTrees = TreeNodeFactory.CreateTreeForestFromModelList<OrganizationEx>(orgExs, orgEx => orgEx.Organization.ID, orgEx => orgEx.Organization.ParentID, orgEx => _orgnizationVehicleTrees.Any(t => t.Organization.ID == orgEx.Organization.ParentID) == false);
                }
            }
            catch (System.Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
            finally
            {
                ApplicationContext.Instance.Logger.LogInforMession(MethodBase.GetCurrentMethod().ToString(), "end GetOrganizationByUserCompleted");
                if (_dataLoadCompleteAction != null)
                {
                    _dataLoadCompleteAction();
                }
            }
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
            Func<TreeNode, bool> childContainFunc = org => ((VehicleEx)org).VehicleInfo.Name.Contains(filterInfo);

            TreeNodeFactory.SearchTreeWithVisibility(vehicleTrees, nodeContainFunc, childContainFunc, isContain);
        }

        void orgClient_GetOrganizationByUserCompleted(object sender, GetOrganizationByUserCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                return;
            }

            if (e.Error != null)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), e.Error);
                return;
            }

            try
            {
                var result = e.Result;
                if (result.IsSuccess == false)
                {
                    if (string.IsNullOrWhiteSpace(result.ErrorMsg) == false)
                    {
                        ApplicationContext.Instance.Logger.LogError(MethodBase.GetCurrentMethod().ToString(), result.ErrorMsg);
                    }

                    if (result.ExceptionMessage != null)
                    {
                        ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), result.ExceptionMessage);
                    }
                }

                if (result.Result != null)
                {
                    _authorityVehicleOrgs = result.Result;

                    var orgs = _authorityVehicleOrgs;
                    var orgExs = orgs.Select(org => new OrganizationEx(org)).ToList();

                    _orgnizationVehicleTrees = new ObservableCollection<OrganizationEx>(orgExs);
                    VehicleTrees = TreeNodeFactory.CreateTreeForestFromModelList<OrganizationEx>(orgExs, orgEx => orgEx.Organization.ID, orgEx => orgEx.Organization.ParentID, orgEx => _orgnizationVehicleTrees.Any(t => t.Organization.ID == orgEx.Organization.ParentID) == false);
                }
            }
            catch (System.Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
            finally
            {
                ApplicationContext.Instance.Logger.LogInforMession(MethodBase.GetCurrentMethod().ToString(), "end GetOrganizationByUserCompleted");
                if (_dataLoadCompleteAction != null)
                {
                    _dataLoadCompleteAction();
                }
            }
        }

        #endregion
    }

}
