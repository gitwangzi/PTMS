using Gsafety.PTMS.Bases.Models;
using Gsafety.PTMS.BasicPage.Model;
using Gsafety.PTMS.ServiceReference.AccountService;
using Gsafety.PTMS.ServiceReference.OrganizationService;
using Gsafety.PTMS.Share;
using Jounce.Core.ViewModel;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
namespace Gsafety.PTMS.BasicPage.ViewModels
{
    public class SelecSignleOrganizationWindowViewModel : BaseViewModel
    {

        #region 属性


        public OrganizationTreeFactory VehicleTreeFactory { get; set; }

        private UserServiceClient client;
        private string currentUserID;

        //private Gsafety.PTMS.Bases.Models.OrganizationEx _selectedItem;
        ///// <summary>
        ///// 选中的组织机构项
        ///// </summary>
        //public Gsafety.PTMS.Bases.Models.OrganizationEx SelectedOrganizationItem
        //{
        //    get
        //    {
        //        return this._selectedItem;
        //    }
        //    set
        //    {
        //        this._selectedItem = value;
        //        RaisePropertyChanged(() => SelectedOrganizationItem);
        //    }
        //}

        private Organization _selectedItem;
        /// <summary>
        /// 选中的组织机构项
        /// </summary>
        public Organization SelectedOrganizationItem
        {
            get
            {
                return this._selectedItem;
            }
            set
            {
                this._selectedItem = value;
                RaisePropertyChanged(() => SelectedOrganizationItem);
            }
        }


        private List<string> _childenNodeIdList;
        /// <summary>
        /// 获取子节点的主键编号列表
        /// </summary>
        public List<string> ChildenNodeIdList
        {
            get
            {
                return this._childenNodeIdList;
            }
            set
            {
                this._childenNodeIdList = value;
                RaisePropertyChanged(() => this.ChildenNodeIdList);
            }
        }


        #endregion


        #region 构造函数

        public SelecSignleOrganizationWindowViewModel(string userID)
        {
            VehicleTreeFactory = new OrganizationTreeFactory();
            this.SelectedOrganizationItem = new Organization();
            InitServiceClient();
            GetUserVehicleOrg(userID);
        }


        #endregion


        #region 方法

        /// <summary>
        /// 获取组织机构的所有下级组织机构，包含自己
        /// </summary>
        /// <param name="rootOrgEx"></param>
        /// <returns></returns>
        private List<OrganizationEx> GetFamilyOrganizations(OrganizationEx rootOrgEx)
        {
            var result = new List<OrganizationEx>();

            if (rootOrgEx == null)
            {
                return result;
            }

            result.Add(rootOrgEx);

            if (rootOrgEx.HasChildren == false)
            {
                return result;
            }

            foreach (var child in rootOrgEx.Children)
            {
                if (child.IsLeaf)
                {
                    continue;
                }

                var temp = GetFamilyOrganizations(child as OrganizationEx);
                result.AddRange(temp);
            }

            return result;
        }

        private void client_GetUserVehicleOrgCompleted(object sender, GetUserVehicleOrgCompletedEventArgs e)
        {
            //try
            //{
            //    foreach(var ite in VehicleTreeFactory.OrgnizationVehicleTrees)
            //    {
            //        foreach(var item in e.Result.Result)
            //        {
            //            if(ite.Organization.ID == item.OrganizationId)
            //            {
            //                ite.IsChecked = true;
            //            }
            //        }
            //    }
            //}
            //catch(Exception ex)
            //{
            //}
            //finally
            //{
            //    CloseClient();
            //}


        }
        private void client_InsertUserVehicleOrgCompleted(object sender, InsertUserVehicleOrgCompletedEventArgs e)
        {
            CloseClient();
        }

        public void InitServiceClient()
        {
            client = ServiceClientFactory.Create<UserServiceClient>();
            ServiceClientFactory.CreateMessageHeader(client.InnerChannel);
            client.GetUserVehicleOrgCompleted += client_GetUserVehicleOrgCompleted;
            client.InsertUserVehicleOrgCompleted += client_InsertUserVehicleOrgCompleted;
        }



        public void GetUserVehicleOrg(string userID)
        {
            currentUserID = null;
            client.GetUserVehicleOrgAsync(userID);
            currentUserID = userID;
        }

        public void SaveVehicleOrg()
        {
            //InitServiceClient();
            //ObservableCollection<OrganizationUser> orgUser = new ObservableCollection<OrganizationUser>();
            //foreach(var vo in VehicleTreeFactory.OrgnizationVehicleTrees)
            //{
            //    if(vo.IsChecked)
            //    {
            //        OrganizationUser ou = new OrganizationUser();
            //        ou.ID = Guid.NewGuid().ToString();
            //        ou.OrganizationId = vo.Organization.ID;
            //        ou.UserId = currentUserID;
            //        ou.CreateTime = DateTime.Now;
            //        orgUser.Add(ou);
            //    }
            //    else
            //    {
            //        OrganizationUser ou = new OrganizationUser();
            //        ou.ID = null;
            //        ou.OrganizationId = vo.Organization.ID;
            //        ou.UserId = currentUserID;
            //        ou.CreateTime = DateTime.Now;
            //        orgUser.Add(ou);
            //    }
            //}
            //client.InsertUserVehicleOrgAsync(orgUser);
            this.GetOrganizationIdList(this.SelectedOrganizationItem.ID, this.VehicleTreeFactory.OrgnizationVehicleTrees);
        }



        private void CloseClient()
        {
            if(this.client != null)
            {
                this.client.CloseAsync();
            }
            this.client = null;
        }

        private List<string> GetOrganizationIdList(string selectedId, ObservableCollection<Gsafety.PTMS.Bases.Models.OrganizationEx> TreeNodeList)
        {
            try
            {
                List<string> TaregetIdList = new List<string>();
                var selectedNode = TreeNodeList.FirstOrDefault(t => t.Organization.ID == selectedId);
                if (selectedNode.HasChildren)
                {
                    var childens = selectedNode.Children;
                    this.GetChildenIdList(selectedNode.Children, ref TaregetIdList);
                }
                else
                {
                    TaregetIdList.Add(selectedId);
                }
                return TaregetIdList;
            }
            catch (System.Exception ex)
            {
                
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
                return null;
            }
        }

        //private List<string> GetChildenIdList(ObservableCollection<Bases.Models.OrganizationEx> childenList,ref List<string> idList)
        private List<string> GetChildenIdList(ObservableCollection<Gsafety.PTMS.Bases.Librarys.TreeNode> childenList, ref List<string> idList)
        {
            //foreach(var item in childenList)
            //{
            //    if(item.HasChildren)
            //    {
            //        this.GetChildenIdList(item.Children, ref idList);
            //    }
            //    else
            //    {
            //        idList.Add(item.);
            //    }


            //}
            return null;
        }



        #endregion


    }
}
