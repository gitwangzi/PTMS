using Gsafety.Ant.Monitor.ViewModels;
using Gsafety.Common.Controls;
using Gsafety.PTMS.Constants;
using Gsafety.PTMS.Monitor;
using Gsafety.PTMS.ServiceReference.MonitorGroupVehicleService;
using Gsafety.PTMS.ServiceReference.RunMonitorGroupService;
using Gsafety.PTMS.ServiceReference.VehicleService;
using Gsafety.PTMS.Share;
using Jounce.Core.View;
using Jounce.Regions.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    [ExportAsView(MonitorName.AddMonitorGroupV, Category = "Navigation")]
    //[ExportViewToRegion(MonitorName.AntProductMonitorMainPageV, ViewContainer.CentralMainContainer2)]
    public partial class AddMonitorGroup : ChildWindow
    {
        protected string MsgHasChildren = ApplicationContext.Instance.StringResourceReader.GetString("MsgMonitorGroupHasVehicel");
        protected string Caption = ApplicationContext.Instance.StringResourceReader.GetString(LProxy.Caption);

        //private ObservableCollection<RunMonitorGroup> monitorGroupManagerOc = new ObservableCollection<RunMonitorGroup>();
        //public ObservableCollection<RunMonitorGroup> _MoniterGroupManagerOC;

        private ObservableCollection<RunMonitorGroupVehicle> vehicles;
        public ObservableCollection<RunMonitorGroupVehicle> Vehicles { get; set; }

        public RunMonitorGroupVehicle model { get; set; }


        public RunMonitorGroupServiceClient client = null;

        /// <summary>
        /// The list of the number of input errors 
        /// </summary>
        private int m_errorCount = 0;

        /// <summary>
        /// 车牌号
        /// </summary>
        public string CarNo { get; set; }
        public object MonitorObject { get; set; }

        public AddMonitorGroup(string csCarNo, object obj)
        {
            InitializeComponent();
            this.CarNo = csCarNo;
            this.MonitorObject = obj;
            this.DialogResult = false;

            this.GroupListView.ItemsSource = ApplicationContext.Instance.BufferManager.MonitorGroupManager._MoniterGroupManagerOC;

            RunMonitorGroup selectedItem = new RunMonitorGroup();

            foreach (var item in ApplicationContext.Instance.BufferManager.MonitorGroupManager._MoniterGroupManagerOC)
            {
                foreach (var car in item.GroupVehicle)
                {
                    if (car.VehicleId == this.CarNo.Trim())
                    {
                        selectedItem = item;
                        this.GroupListView.SelectedItem = selectedItem;
                        break;
                    }
                }
            }
        }


        private void client_GetRunMonitorGroupListCompleted(object sender, GetRunMonitorGroupListCompletedEventArgs e)
        {
            //try
            //{
            //    if (e.Result.Result.Count() > 0)
            //    {
            //        _MoniterGroupManagerOC = e.Result.Result;
            //        this.GroupListView.ItemsSource = _MoniterGroupManagerOC;

            //        RunMonitorGroup selectedItem = new RunMonitorGroup();

            //        foreach (var item in _MoniterGroupManagerOC)
            //        {
            //            foreach (var car in item.GroupVehicle)
            //            {
            //                if (car.VehicleId == this.CarNo.Trim())
            //                {
            //                    selectedItem = item;
            //                    this.GroupListView.SelectedItem = selectedItem;
            //                    //this.GroupListView.ScrollIntoView(GroupListView.SelectedItem, null);
            //                    break;
            //                }
            //            }
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //    ApplicationContext.Instance.Logger.LogException("GetRunMonitorGroupListCompleted", ex);
            //}
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void GroupListView_BindingValidationError(object sender, ValidationErrorEventArgs e)
        {
            if (e.Action == ValidationErrorEventAction.Added)
            {
                m_errorCount++;
            }
            else
            {
                m_errorCount--;
            }
        }

        private void ChildWindow_Closing_1(object sender, System.ComponentModel.CancelEventArgs e)
        {

        }


        private void BtnAddExtent_Click_1(object sender, RoutedEventArgs e)
        {
            if (ApplicationContext.Instance.BufferManager.MonitorGroupManager._MoniterGroupManagerOC == null)
            {
                ApplicationContext.Instance.BufferManager.MonitorGroupManager._MoniterGroupManagerOC = new ObservableCollection<RunMonitorGroup>();
                this.GroupListView.ItemsSource = ApplicationContext.Instance.BufferManager.MonitorGroupManager._MoniterGroupManagerOC;
            }


            if (ApplicationContext.Instance.BufferManager.MonitorGroupManager._MoniterGroupManagerOC.Count < 20)
            {
                RunMonitorGroup item = new RunMonitorGroup()
                {
                    GroupName = "Default",
                    Note = string.Empty,
                    AdUser = ApplicationContext.Instance.AuthenticationInfo.UserID,
                    ID = Guid.NewGuid().ToString(),
                    GroupIndex = (short)ApplicationContext.Instance.BufferManager.MonitorGroupManager._MoniterGroupManagerOC.Count
                };
                ApplicationContext.Instance.BufferManager.MonitorGroupManager._MoniterGroupManagerOC.Add(item);
            }
            else
            {
                MessageBoxHelper.ShowDialog(Caption, ApplicationContext.Instance.StringResourceReader.GetString("MONITOR_GroupCountMsg"), MessageDialogButton.OkAndCancel);
            }
        }

        private void BtnDeleExtent_Click_1(object sender, RoutedEventArgs e)
        {
            if (this.GroupListView.SelectedItem != null)
            {
                RunMonitorGroup selectedItem = this.GroupListView.SelectedItem as RunMonitorGroup;

                if (selectedItem != null)
                {
                    var result = (SelfMessageBox)MessageBoxHelper.ShowDialog(Caption, ApplicationContext.Instance.StringResourceReader.GetString("MONITOR_GroupDeleConditionQuesitionMsg"), MessageDialogButton.OkAndCancel);

                    if (result.DialogResult == true)
                        //需要判断监控组下是否有车辆
                        if (selectedItem.GroupVehicle != null && selectedItem.GroupVehicle.Count() > 0)
                        {
                            result = (SelfMessageBox)MessageBoxHelper.ShowDialog(Caption, ApplicationContext.Instance.StringResourceReader.GetString("MONITOR_GroupDeleConditionMsg"), MessageDialogButton.OkAndCancel);

                            if (result.DialogResult == true)
                            {
                                for (int i = 0; i < selectedItem.GroupVehicle.Count; i++)
                                {
                                    try
                                    {
                                        client = ServiceClientFactory.Create<RunMonitorGroupServiceClient>();
                                        client.DeleteRunMonitorGroupVehicleByObjAsync(selectedItem.GroupVehicle);
                                    }
                                    catch (Exception)
                                    {
                                        client = ServiceClientFactory.Create<RunMonitorGroupServiceClient>();
                                    }
                                    finally
                                    {
                                        client.CloseAsync();
                                    }
                                }

                                for (int i = 0; i < selectedItem.GroupVehicle.Count; i++)
                                {
                                    selectedItem.GroupVehicle.RemoveAt(i);
                                    //selectedItem.GroupVehicle = new ObservableCollection<RunMonitorGroupVehicle>();
                                }

                                this.GroupListView.ItemsSource = null;
                                ApplicationContext.Instance.BufferManager.MonitorGroupManager._MoniterGroupManagerOC.Remove(selectedItem);
                                this.GroupListView.ItemsSource = ApplicationContext.Instance.BufferManager.MonitorGroupManager._MoniterGroupManagerOC;
                            }
                        }
                        else
                        {
                            try
                            {
                                if (ApplicationContext.Instance.BufferManager.MonitorGroupManager._MoniterGroupManagerOC.Count() > 0 && ApplicationContext.Instance.BufferManager.MonitorGroupManager._MoniterGroupManagerOC.Contains(selectedItem))
                                {
                                    GroupListView.ItemsSource = null;
                                    ApplicationContext.Instance.BufferManager.MonitorGroupManager._MoniterGroupManagerOC.Remove(selectedItem);
                                    GroupListView.ItemsSource = ApplicationContext.Instance.BufferManager.MonitorGroupManager._MoniterGroupManagerOC;

                                    client = ServiceClientFactory.Create<RunMonitorGroupServiceClient>();
                                    client.DeleteRunMonitorGroupByIDAsync(selectedItem.ID);
                                }
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
                }
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="mg"></param>
        /// <returns></returns>
        private ObservableCollection<RunMonitorGroup> RemoveSeletedItem(ObservableCollection<RunMonitorGroup> obj, RunMonitorGroup mg)
        {
            for (int i = 0; i < obj.Count(); i++)
            {
                if (obj[i] == mg)
                {
                    obj.RemoveAt(i);
                    break;
                }
            }
            return obj;
        }


        private void BtnUpExtent_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                if (null != this.GroupListView.SelectedItem)
                {
                    RunMonitorGroup selectItem = this.GroupListView.SelectedItem as RunMonitorGroup;

                    int selectIndex = this.GroupListView.SelectedIndex;
                    if (selectIndex != 0)
                    {
                        ApplicationContext.Instance.BufferManager.MonitorGroupManager._MoniterGroupManagerOC.Single(x => x.GroupIndex == selectItem.GroupIndex - 1).GroupIndex = selectItem.GroupIndex;
                        selectItem.GroupIndex = selectItem.GroupIndex - 1;

                        this.GroupListView.ItemsSource = null;
                        ApplicationContext.Instance.BufferManager.MonitorGroupManager._MoniterGroupManagerOC.Remove(selectItem);
                        ApplicationContext.Instance.BufferManager.MonitorGroupManager._MoniterGroupManagerOC.Insert(selectIndex - 1, selectItem);
                        this.GroupListView.ItemsSource = ApplicationContext.Instance.BufferManager.MonitorGroupManager._MoniterGroupManagerOC;
                        this.GroupListView.SelectedItem = selectItem;
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
                    if (selectIndex != ApplicationContext.Instance.BufferManager.MonitorGroupManager._MoniterGroupManagerOC.Count - 1)
                    {
                        ApplicationContext.Instance.BufferManager.MonitorGroupManager._MoniterGroupManagerOC.Single(x => x.GroupIndex == selectItem.GroupIndex + 1).GroupIndex = selectItem.GroupIndex;
                        selectItem.GroupIndex = selectItem.GroupIndex + 1;

                        this.GroupListView.ItemsSource = null;
                        ApplicationContext.Instance.BufferManager.MonitorGroupManager._MoniterGroupManagerOC.Remove(selectItem);
                        ApplicationContext.Instance.BufferManager.MonitorGroupManager._MoniterGroupManagerOC.Insert(selectIndex + 1, selectItem);
                        this.GroupListView.ItemsSource = ApplicationContext.Instance.BufferManager.MonitorGroupManager._MoniterGroupManagerOC;
                        this.GroupListView.SelectedItem = selectItem;
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
        }

        private void OKButton_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                RunMonitorGroup selectedMg = GroupListView.SelectedItem as RunMonitorGroup;
                string groupId = selectedMg.ID;
                string carNo = this.CarNo;

                ////这个方法需要清理，暂时有用
                //int monitorVehicleCount = 1;
                //foreach (var item in _MoniterGroupManagerOC)
                //{
                //    monitorVehicleCount += item.GroupVehicle.Count();
                //}

                foreach (var item in ApplicationContext.Instance.BufferManager.MonitorGroupManager._MoniterGroupManagerOC)
                {
                    if (item.GroupVehicle != null && item.GroupVehicle.Count() > 0)
                    {
                        foreach (var v in item.GroupVehicle)
                        {
                            if (v.VehicleId == carNo)
                            {
                                item.GroupVehicle.Remove(v);
                                break;
                            }
                        }
                    }
                }

                RunMonitorGroupVehicle vehicle = new RunMonitorGroupVehicle();
                foreach (var item in ApplicationContext.Instance.BufferManager.MonitorGroupManager._MoniterGroupManagerOC)
                {
                    if (item == selectedMg)
                    {
                        if (item.GroupVehicle != null && item.GroupVehicle.Count() > 0)
                        {
                            //组下有车
                            item.GroupVehicle = new ObservableCollection<RunMonitorGroupVehicle>();

                            //找到那辆车
                            foreach (var v in item.GroupVehicle)
                            {
                                //已经存在该车
                                if (v.VehicleId == carNo.Trim())
                                {
                                    vehicle = v;
                                    vehicle.GroupId = item.ID;
                                    vehicle.VehicleId = carNo;
                                    item.GroupVehicle.Add(vehicle);
                                    break;
                                }
                                //else
                                //{
                                //    //不存在该车
                                //    vehicle.GroupId = item.ID;
                                //    vehicle.VehicleId = carNo;
                                //}
                            }

                            vehicle.GroupId = item.ID;
                            vehicle.VehicleId = carNo;
                            item.GroupVehicle.Add(vehicle);
                            break;
                        }
                        else
                        {
                            //组下无车
                            vehicle.GroupId = item.ID;
                            vehicle.VehicleId = carNo;

                            item.GroupVehicle.Add(vehicle);
                            break;
                        }
                    }
                }

                client = ServiceClientFactory.Create<RunMonitorGroupServiceClient>();
                client.InsertRunMonitorGroupCompleted += client_InsertRunMonitorGroupCompleted;
                client.InsertRunMonitorGroupAsync(ApplicationContext.Instance.BufferManager.MonitorGroupManager._MoniterGroupManagerOC, carNo, vehicle);
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

            this.DialogResult = true;
        }


        /// <summary>
        /// 保存完成
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void client_InsertRunMonitorGroupCompleted(object sender, InsertRunMonitorGroupCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString("Tip"), ApplicationContext.Instance.StringResourceReader.GetString("OperatorServiceError"), MessageDialogButton.OkAndCancel);
                ApplicationContext.Instance.Logger.LogException("UpdateUserGroup", e.Result.ExceptionMessage);
            }

            AntProductMonitorMainPageViewModel obj = new AntProductMonitorMainPageViewModel(MonitorObject);
        }


        private void ExitGroup_Click_1(object sender, RoutedEventArgs e)
        {
            model.GroupId = null;
            var s = this.GroupListView.SelectedItem;
        }
    }


}

