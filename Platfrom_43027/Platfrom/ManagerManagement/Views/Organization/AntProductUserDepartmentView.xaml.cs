using Gsafety.Common.Controls;
using Gsafety.PTMS.Bases.Librarys;
using Gsafety.PTMS.ServiceReference.AccountService;
using Gsafety.PTMS.Share;
using Jounce.Core.Event;
using Jounce.Core.View;
using Jounce.Regions.Core;
using Microsoft.Windows;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using DragEventArgs = System.Windows.DragEventArgs;
using System.Linq;

namespace Gsafety.PTMS.Manager.Views.Organization
{
    /// <summary>
    /// 人员部门管理视图
    /// </summary>
    [ExportAsView(ManagerName.AntProductUserDepartmentV, Category = ManagerName.CategoryName, MenuName = ManagerName.UserMangeMenuName)]
    [ExportViewToRegion(ManagerName.AntProductUserDepartmentV, ManagerName.ManagerContainer)]
    public partial class UserOrganizationManageView : UserControl
    {
        private string SelectDepartmentID = string.Empty;
        private UserServiceClient client;
        public UserOrganizationManageView()
        {
            InitializeComponent();
            this.client = ServiceClientFactory.Create<UserServiceClient>();
            ServiceClientFactory.CreateMessageHeader(this.client.InnerChannel);
            client.UpdateDepartmentCompleted += client_UpdateDepartmentCompleted;
        }

        void client_UpdateDepartmentCompleted(object sender, UpdateDepartmentCompletedEventArgs e)
        {
            try
            {
                if (e.Cancelled == true)
                {
                    return;
                }
                if (e.Error != null)
                {
                    MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString("ID_INSTALL_ServiceError"));
                    ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), e.Error);
                    return;
                }
                if (e.Result.IsSuccess == false)
                {
                    if (string.IsNullOrEmpty(e.Result.ErrorMsg))
                    {
                        MessageBoxHelper.ShowDialog(e.Result.ErrorMsg);
                        ApplicationContext.Instance.Logger.LogError("UpdateUsrDepartment", e.Result.ErrorMsg);
                    }
                    else
                    {
                        MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString("ID_INSTALL_ServiceError"));
                        ApplicationContext.Instance.Logger.LogException("UpdateUsrDepartment", e.Result.ExceptionMessage);
                    }
                }
                else
                {
                    MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString("OperatedSuccessed"));
                    //this.EventAggregator.Publish(new ViewNavigationArgs(ManagerName.AntProductUserManageV, new Dictionary<string, object>(){
                    //    {"userDepartmentID",this.SelectDepartmentID}}));
                    foreach (var item in this.TreeView.GetItemsAndContainers())
                    {
                        var isFind = SetItemSelectByDepartmentID(item, SelectDepartmentID);
                        if (isFind)
                        {
                            break;
                        }
                    }

                    SelectDepartmentID = string.Empty;
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("UserOrganizationManageView.client_UpdateDepartmentCompleted", ex);
            }
        }

        private bool SetItemSelectByDepartmentID(KeyValuePair<object, TreeViewItem> item, string departmentID)
        {
            var treeLevel = item.Key as TreeNode<UsrDepartment>;
            if (treeLevel.Model.ID == departmentID)
            {
                item.Value.IsSelected = true;
                return true;
            }
            else
            {
                foreach (var child in item.Value.GetItemsAndContainers())
                {
                    var isFind = SetItemSelectByDepartmentID(child, departmentID);
                    if (isFind)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        [Import]
        public IEventAggregator EventAggregator { get; set; }

        private Brush defaultBackground;
        private TreeViewItem currentSelectContainer;

        private void TreeView_OnSelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            var treeView = sender as TreeView;
            if (treeView != null)
            {
                var container = treeView.GetSelectedContainer();

                if (currentSelectContainer == null)
                {
                    defaultBackground = container.Background;
                }
                else
                {
                    currentSelectContainer.Background = defaultBackground;
                }

                if (container != null)
                {
                    currentSelectContainer = container;
                    //container.Background = new SolidColorBrush("#1c1f23");
                    //container.Background = new SolidColorBrush(Color.FromArgb());
                    SolidColorBrush solidColorBrush = new SolidColorBrush();
                    solidColorBrush.Color = "#1c1f23".ToColor();
                    container.Background = solidColorBrush;
                }

                var selectedItem = treeView.SelectedItem;
                var target = selectedItem as TreeNode<UsrDepartment>;
                if (target != null)
                {
                    this.EventAggregator.Publish(new ViewNavigationArgs(ManagerName.AntProductUserManageV, new Dictionary<string, object>(){
                        {"userDepartmentID",target.Model.ID}}));
                }
                else
                {
                    try
                    {
                        EventAggregator.Publish(new ViewNavigationArgs(ManagerName.EmptyView));
                    }
                    catch (System.Exception ex)
                    {                    	
                    }
                }
            }

        }

        private void Link1_MouseWheel(object sender, System.Windows.Input.MouseWheelEventArgs e)
        {

        }

        private void Link1_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            var hy = sender as HyperlinkButton;
            if (hy != null)
            {
                //SelectDepartmentID = hy.Tag.ToString();
            }
        }

        private void Link1_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            var hy = sender as HyperlinkButton;
            if (hy != null)
            {
                if (SelectDepartmentID == hy.Tag.ToString())
                {
                    SelectDepartmentID = string.Empty;
                }
            }
        }

        private void TreeViewDragDropTargetControl_OnItemDragStarting(object sender, ItemDragEventArgs e)
        {
            e.Cancel = true;
            e.Handled = true;
        }

        private void Link1_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as HyperlinkButton;
            if (btn != null)
            {
                SolidColorBrush solidColorBrush = new SolidColorBrush();
                solidColorBrush.Color = "#1c1f23".ToColor();
                btn.Background = solidColorBrush;
            }
        }
    }
}
