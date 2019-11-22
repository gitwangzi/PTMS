using Gsafety.Ant.Monitor.ViewModels;
using Gsafety.Common.Controls;
using Gsafety.PTMS.Constants;
using Gsafety.PTMS.Monitor;
using Gsafety.PTMS.ServiceReference.RunMonitorGroupService;
using Gsafety.PTMS.ServiceReference.VehicleService;
using Gsafety.PTMS.Share;
using Jounce.Core.View;
using Jounce.Regions.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Gsafety.Ant.Monitor.Views
{
    public partial class MonitorGroupManager : ChildWindow, INotifyPropertyChanged
    {
        protected string MsgHasChildren = ApplicationContext.Instance.StringResourceReader.GetString("MsgMonitorGroupHasVehicel");
        protected string Caption = ApplicationContext.Instance.StringResourceReader.GetString(LProxy.Caption);


        public ObservableCollection<RunMonitorGroupVehicle> Vehicles { get; set; }

        public RunMonitorGroupVehicle model { get; set; }

        public RunMonitorGroupServiceClient client = null;


        /// <summary>
        /// 车牌号
        /// </summary>
        private string _CarNo { get; set; }

        public MonitorGroupManager()
        {
            InitializeComponent();
            foreach (var item in ApplicationContext.Instance.BufferManager.MonitorGroupManager.MoniterGroupManagerOC)
            {
                item.IsSelected = false;
            }

            this.DataContext = this;
            if (ApplicationContext.Instance.BufferManager.MonitorGroupManager.MoniterGroupManagerOC.Count > 0)
            {
                this.GroupListView.SelectedItem = ApplicationContext.Instance.BufferManager.MonitorGroupManager.MoniterGroupManagerOC[0];
            }
            Gsafety.PTMS.Share.ApplicationContext.Instance.StringResourceReader.TranslateDataGrid(GroupListView);
            this.MouseRightButtonDown += ChildWindow_MouseRightButtonDown;
        }

        public ObservableCollection<RunMonitorGroup> MonitorGroup
        {
            get
            {
                return ApplicationContext.Instance.BufferManager.MonitorGroupManager.MoniterGroupManagerOC;
            }
        }

        private void ChildWindow_MouseRightButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            e.Handled = true;
        }

        public void AddVechileToGroup(string csCarNo)
        {
            this._CarNo = csCarNo;
            this.DialogResult = false;

            foreach (var item in ApplicationContext.Instance.BufferManager.MonitorGroupManager.MoniterGroupManagerOC)
            {
                if (item.GroupVehicle != null)
                {
                    foreach (var car in item.GroupVehicle)
                    {
                        if (car.VehicleId == this._CarNo.Trim())
                        {
                            item.IsSelected = true;
                            SelectedItem = item;
                            RaisePropertyChanged("SelectedItem");
                            break;
                        }
                    }
                }
            }
            Show();
        }



        private void BtnAddGroup_Click_1(object sender, RoutedEventArgs e)
        {
            if (ApplicationContext.Instance.BufferManager.MonitorGroupManager.MoniterGroupManagerOC.Count < 20)
            {
                RunMonitorGroup item = new RunMonitorGroup()
                {
                    GroupName = "Default",
                    Note = string.Empty,
                    AdUser = ApplicationContext.Instance.AuthenticationInfo.UserID,
                    ID = Guid.NewGuid().ToString(),
                    GroupIndex = (short)ApplicationContext.Instance.BufferManager.MonitorGroupManager.MoniterGroupManagerOC.Count
                };
                MonitorGroupEdit mge = new MonitorGroupEdit();
                mge.Edit(item);
                mge.Closed += (m, n) =>
                {
                    if (mge.DialogResult == true)
                    {
                        ApplicationContext.Instance.BufferManager.MonitorGroupManager.MoniterGroupManagerOC.Add(item);
                    }
                };

            }
            else
            {
                MessageBoxHelper.ShowDialog(Caption, ApplicationContext.Instance.StringResourceReader.GetString("MONITOR_GroupCountMsg"), MessageDialogButton.Ok);
            }
        }


        private void BtnDeleGroup_Click_1(object sender, RoutedEventArgs e)
        {
            if (this.GroupListView.SelectedItem != null)
            {
                RunMonitorGroup selectedItem = this.GroupListView.SelectedItem as RunMonitorGroup;

                if (selectedItem != null)
                {
                    ChildWindow result;
                    if (selectedItem.GroupVehicle != null && selectedItem.GroupVehicle.Count() > 0)//需要判断监控组下是否有车辆
                    {
                        result = (SelfMessageBox)MessageBoxHelper.ShowDialog(Caption, ApplicationContext.Instance.StringResourceReader.GetString("MONITOR_GroupDeleConditionMsg"), MessageDialogButton.OkAndCancel);
                    }
                    else
                    {
                        result = (SelfMessageBox)MessageBoxHelper.ShowDialog(Caption, ApplicationContext.Instance.StringResourceReader.GetString("MONITOR_GroupDeleConditionQuesitionMsg"), MessageDialogButton.OkAndCancel);
                    }
                    result.Closed += (a, b) =>
                    {
                        if (result.DialogResult == true)
                        {
                            //执行删除操作
                            try
                            {
                                ApplicationContext.Instance.BufferManager.MonitorGroupManager.MoniterGroupManagerOC.Remove(selectedItem);
                                SelectedItem = null;
                                RaisePropertyChanged("SelectedItem");
                                client = ServiceClientFactory.Create<RunMonitorGroupServiceClient>();
                                client.DeleteRunMonitorGroupByIDAsync(selectedItem.ID);
                            }
                            catch (Exception ex)
                            {
                                client = ServiceClientFactory.Create<RunMonitorGroupServiceClient>();
                                ApplicationContext.Instance.Logger.LogException("BtnDeleExtent", ex);
                            }
                            finally
                            {
                                client.CloseAsync();
                            }
                        }
                    };
                }
            }
        }

        public RunMonitorGroup SelectedItem { get; set; }

        private void BtnUpGroup_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                if (null != this.GroupListView.SelectedItem)
                {
                    RunMonitorGroup selectItem = this.GroupListView.SelectedItem as RunMonitorGroup;

                    int selectIndex = this.GroupListView.SelectedIndex;
                    if (selectIndex > 0)
                    {
                        ApplicationContext.Instance.BufferManager.MonitorGroupManager.MoniterGroupManagerOC.Remove(selectItem);
                        ApplicationContext.Instance.BufferManager.MonitorGroupManager.MoniterGroupManagerOC.Insert(selectIndex - 1, selectItem);
                        for (int i = 0; i < ApplicationContext.Instance.BufferManager.MonitorGroupManager.MoniterGroupManagerOC.Count; i++)
                        {
                            ApplicationContext.Instance.BufferManager.MonitorGroupManager.MoniterGroupManagerOC[i].GroupIndex = i;
                        }
                        SelectedItem = selectItem;
                        RaisePropertyChanged("SelectedItem");
                        GroupListView.ScrollIntoView(this.GroupListView.SelectedItem, null);
                    }
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("UserGroupBtnUpExtent", ex);
            }
        }


        private void BtnDownExtent_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                if (this.GroupListView.SelectedItem != null)
                {
                    RunMonitorGroup selectItem = this.GroupListView.SelectedItem as RunMonitorGroup;

                    int selectIndex = this.GroupListView.SelectedIndex;
                    if (selectIndex != ApplicationContext.Instance.BufferManager.MonitorGroupManager.MoniterGroupManagerOC.Count - 1)
                    {
                        selectItem.GroupIndex = selectItem.GroupIndex + 1;
                        ApplicationContext.Instance.BufferManager.MonitorGroupManager.MoniterGroupManagerOC.Remove(selectItem);
                        ApplicationContext.Instance.BufferManager.MonitorGroupManager.MoniterGroupManagerOC.Insert(selectIndex + 1, selectItem);
                        for (int i = 0; i < ApplicationContext.Instance.BufferManager.MonitorGroupManager.MoniterGroupManagerOC.Count; i++)
                        {
                            ApplicationContext.Instance.BufferManager.MonitorGroupManager.MoniterGroupManagerOC[i].GroupIndex = i;
                        }
                        SelectedItem = selectItem;
                        RaisePropertyChanged("SelectedItem");
                        this.GroupListView.ScrollIntoView(GroupListView.SelectedItem, null);
                    }
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("UserGroupBtnDownExtent", ex);
            }
        }

        private void CancelButton_Click_1(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            try
            {
                client = ServiceClientFactory.Create<RunMonitorGroupServiceClient>();
                client.ChangeRunMonitorGroupCompleted += client_ChangeRunMonitorGroupCompleted;
                client.ChangeRunMonitorGroupAsync(ApplicationContext.Instance.BufferManager.MonitorGroupManager.MoniterGroupManagerOC, ApplicationContext.Instance.AuthenticationInfo.UserID);
            }
            catch (Exception ex)
            {
                client = ServiceClientFactory.Create<RunMonitorGroupServiceClient>();
                ApplicationContext.Instance.Logger.LogException("UserGroupOkButton", ex);
            }
            finally
            {
                client.CloseAsync();
            }

        }

        private RunMonitorGroup selectedMg;
        private void OKButton_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                if (GroupListView.SelectedItem == null)
                {
                    return;
                }

                foreach (var item in ApplicationContext.Instance.BufferManager.MonitorGroupManager.MoniterGroupManagerOC)
                {
                    if (item.IsSelected)
                    {
                        selectedMg = item;
                        break;
                    }
                }


                //从原来的组里删除掉车
                foreach (var item in ApplicationContext.Instance.BufferManager.MonitorGroupManager.MoniterGroupManagerOC)
                {
                    if (item.GroupVehicle != null && item.GroupVehicle.Count() > 0)
                    {
                        foreach (var v in item.GroupVehicle)
                        {
                            if (v.VehicleId == this._CarNo)
                            {
                                item.GroupVehicle.Remove(v);
                                break;
                            }
                        }
                    }
                }

                RunMonitorGroupVehicle vehicle = new RunMonitorGroupVehicle();

                foreach (var item in ApplicationContext.Instance.BufferManager.MonitorGroupManager.MoniterGroupManagerOC)
                {
                    if (item == selectedMg)
                    {
                        if (item.GroupVehicle == null) item.GroupVehicle = new ObservableCollection<RunMonitorGroupVehicle>();
                        vehicle.GroupId = item.ID;
                        vehicle.VehicleId = this._CarNo;
                        item.GroupVehicle.Add(vehicle);
                        break;
                    }
                }


                //执行数据库操作（如果已经存在则更新）
                client = ServiceClientFactory.Create<RunMonitorGroupServiceClient>();
                client.ChangeRunMonitorGroupCompleted += client_ChangeRunMonitorGroupCompleted;
                client.ChangeRunMonitorGroupAsync(ApplicationContext.Instance.BufferManager.MonitorGroupManager.MoniterGroupManagerOC, ApplicationContext.Instance.AuthenticationInfo.UserID);

                client.ChangeRunMonitorGroupVehicleCompleted += client_ChangeRunMonitorGroupVehicleCompleted;
                client.ChangeRunMonitorGroupVehicleAsync(this._CarNo, selectedMg.ID);

            }
            catch (Exception ex)
            {
                client = ServiceClientFactory.Create<RunMonitorGroupServiceClient>();
                ApplicationContext.Instance.Logger.LogException("UserGroupOkButton", ex);
            }


            this.DialogResult = true;
        }

        void client_ChangeRunMonitorGroupVehicleCompleted(object sender, ChangeRunMonitorGroupVehicleCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString("Tip"), ApplicationContext.Instance.StringResourceReader.GetString("OperatorServiceError"), MessageDialogButton.Ok);
                ApplicationContext.Instance.Logger.LogException("UpdateVechileGroup", e.Result.ExceptionMessage);
            }
        }

        void client_ChangeRunMonitorGroupCompleted(object sender, ChangeRunMonitorGroupCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString("Tip"), ApplicationContext.Instance.StringResourceReader.GetString("OperatorServiceError"), MessageDialogButton.Ok);
                ApplicationContext.Instance.Logger.LogException("UpdateUserGroup", e.Result.ExceptionMessage);
            }
        }

        private void BtnEditdExtent_Click(object sender, RoutedEventArgs e)
        {
            if (this.GroupListView.SelectedItem != null)
            {
                RunMonitorGroup selectedItem = this.GroupListView.SelectedItem as RunMonitorGroup;

                RunMonitorGroup item = new RunMonitorGroup()
                {
                    GroupName = selectedItem.GroupName,
                    Note = selectedItem.Note,
                    AdUser = selectedItem.AdUser,
                    ID = selectedItem.ID,
                    GroupIndex = selectedItem.GroupIndex
                };
                MonitorGroupEdit mge = new MonitorGroupEdit();
                mge.Edit(item);
                mge.Closed += (m, n) =>
                {
                    if (mge.DialogResult == true)
                    {
                        selectedItem.GroupName = mge.EditRunMonitorGroup.GroupName;
                        selectedItem.Note = mge.EditRunMonitorGroup.Note;
                    }
                };
            }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
}

