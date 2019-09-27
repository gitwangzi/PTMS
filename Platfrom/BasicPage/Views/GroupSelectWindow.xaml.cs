using Gsafety.PTMS.ServiceReference.MessageService;
using Jounce.Framework;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Gsafety.PTMS.Bases.Models;
using Gsafety.PTMS.Share;
using Gsafety.PTMS.ServiceReference.RunMonitorGroupService;
namespace Gsafety.PTMS.BasicPage.Monitor.Views
{
    public partial class GroupSelectWindow : ChildWindow
    {
        public Vehicle model { get; set; }
        public string OldGroup;
        private UserDefineGroup _selectGroup;
        private ObservableCollection<UserDefineGroup> _groups;
        private System.Threading.Thread dataLoad;
        private RunMonitorGroupServiceClient monitorGroupVehicleServiceClient = ServiceClientFactory.Create<RunMonitorGroupServiceClient>();

        public GroupSelectWindow(Vehicle v)
        {
            InitializeComponent();
            //监控组车辆操作
            monitorGroupVehicleServiceClient.InsertRunMonitorGroupVehicleCompleted += monitorGroupVehicleServiceClient_InsertRunMonitorGroupVehicleCompleted; //monitorGroupVehicleServiceClient_AddMonitorGroupsVehicleCompleted;
            monitorGroupVehicleServiceClient.DeleteRunMonitorGroupVehicleByIDCompleted += monitorGroupVehicleServiceClient_DeleteRunMonitorGroupVehicleByIDCompleted; //monitorGroupVehicleServiceClient_RemoveMonitorGroupsVehicleCompleted;
            monitorGroupVehicleServiceClient.UpdateRunMonitorGroupVehicleCompleted += monitorGroupVehicleServiceClient_UpdateRunMonitorGroupVehicleCompleted; //monitorGroupVehicleServiceClient_MoveMonitorGroupsVehicleCompleted;
            model = v;
            OldGroup = v.GroupID;
            this.GroupListView.ItemsSource = _groups = ApplicationContext.Instance.BufferManager.VehicleOrganizationManage.Groups;
            ApplicationContext.Instance.StringResourceReader.TranslateDataGrid(GroupListView);
            if (!string.IsNullOrEmpty(v.GroupID))
            {
                UserDefineGroup group = _groups.Where(m => m.ID == v.GroupID).FirstOrDefault();

                if (null != group)
                {
                    GroupListView.SelectedItem = group;
                    dataLoad = new System.Threading.Thread(ScrollIntoView);
                    dataLoad.Start();
                }
            }
            else
            {
                ClearButton.IsEnabled = false;
            }
            if (_groups == null || _groups.Count == 0)
            {
                OKButton.IsEnabled = false;
            }
        }

        void monitorGroupVehicleServiceClient_UpdateRunMonitorGroupVehicleCompleted(object sender, UpdateRunMonitorGroupVehicleCompletedEventArgs e)
        {
            if (e.Result != null)
            {
                if (e.Result.IsSuccess == true)
                {
                    this.DialogResult = true;
                }
                else
                {
                    this.DialogResult = false;
                }
            }
        }

        void monitorGroupVehicleServiceClient_DeleteRunMonitorGroupVehicleByIDCompleted(object sender, DeleteRunMonitorGroupVehicleByIDCompletedEventArgs e)
        {
            if (e.Result != null)
            {
                if (e.Result.IsSuccess == true)
                {
                    this.DialogResult = true;
                }
                else
                {
                    this.DialogResult = false;
                }
            }
        }

        void monitorGroupVehicleServiceClient_InsertRunMonitorGroupVehicleCompleted(object sender, InsertRunMonitorGroupVehicleCompletedEventArgs e)
        {
            if (e.Result != null)
            {
                if (e.Result.IsSuccess == true)
                {
                    this.DialogResult = true;
                }
                else
                {
                    this.DialogResult = false;
                }
            }
        }


        private void ScrollIntoView()
        {
            JounceHelper.ExecuteOnUI(() =>
            {
                GroupListView.ScrollIntoView(GroupListView.SelectedItem, null);
            }
            );
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            _selectGroup = GroupListView.SelectedItem as UserDefineGroup;

            if (null != _selectGroup)
            {
                if (string.IsNullOrEmpty(model.GroupID) || !model.GroupID.Equals(_selectGroup.ID))
                {
                    model.GroupID = _selectGroup.ID;
                    this.UpdateData();
                }
                else
                {
                    this.DialogResult = false;
                }
            }
            else
            {
                this.DialogResult = false;
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void ClearButton_Click_1(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(OldGroup))
            {
                UserDefineGroup group = _groups.Where(item => item.ID == OldGroup).FirstOrDefault();
                group.Vehicles.Remove(group.Vehicles.Where(item => item.VehicleId == model.VehicleId).FirstOrDefault());
                monitorGroupVehicleServiceClient.DeleteRunMonitorGroupVehicleByIDAsync(model.GroupID);
                model.GroupID = null;
            }
            else
            {
                this.DialogResult = false;
            }
        }

        private void UpdateData()
        {
            if ((string.IsNullOrEmpty(OldGroup) && string.IsNullOrEmpty(model.GroupID)) || (OldGroup == model.GroupID))
            {
                this.DialogResult = true;
            }
            else if (string.IsNullOrEmpty(OldGroup) && !string.IsNullOrEmpty(model.GroupID))
            {
                UserDefineGroup group = _groups.Where(item => item.ID == model.GroupID).FirstOrDefault();
                group.Vehicles.Add(model);
                monitorGroupVehicleServiceClient.InsertRunMonitorGroupVehicleAsync(new RunMonitorGroupVehicle()
                {

                }, null, null);

                //model.GroupID, model.VehicleId, model.IsMonitor == true ? (short)1 : (short)0, _selectGroup.Vehicles.Count,ApplicationContext.Instance.AuthenticationInfo.ClientID
            }
            else if (!string.IsNullOrEmpty(OldGroup) && string.IsNullOrEmpty(model.GroupID))
            {
                UserDefineGroup group = _groups.Where(item => item.ID == OldGroup).FirstOrDefault();
                group.Vehicles.Remove(group.Vehicles.Where(item => item.VehicleId == model.VehicleId).FirstOrDefault());
                monitorGroupVehicleServiceClient.DeleteRunMonitorGroupVehicleByIDAsync(model.GroupID);
            }
            else
            {
                UserDefineGroup group1 = _groups.Where(item => item.ID == OldGroup).FirstOrDefault();
                group1.Vehicles.Remove(group1.Vehicles.Where(item => item.VehicleId == model.VehicleId).FirstOrDefault());
                UserDefineGroup group2 = _groups.Where(item => item.ID == model.GroupID).FirstOrDefault();
                group2.Vehicles.Add(model);
                monitorGroupVehicleServiceClient.UpdateRunMonitorGroupVehicleAsync(new RunMonitorGroupVehicle()
                {

                });

                //OldGroup, model.GroupID, model.VehicleId, model.IsMonitor == true ? (short)1 : (short)0, _selectGroup.Vehicles.Count
            }
            ChangeGroupVehicle newitem = new ChangeGroupVehicle();
            newitem.MdvrCoreId = model.UniqueId;
            newitem.VehicleId = model.VehicleId;
            newitem.TargetGroupId = model.GroupID;
            newitem.CreateUser = ApplicationContext.Instance.AuthenticationInfo.UserName;
            ApplicationContext.Instance.MessageManager.SendChangeGroupVehicleMessage(newitem);

        }

        private void GroupListView_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            //ClearButton.IsEnabled = true;
        }
    }
}

