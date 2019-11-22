using Gsafety.PTMS.Bases.Models;
using Gsafety.PTMS.ServiceReference.MessageService;
using Jounce.Core.View;
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
using Gsafety.PTMS.Share;
using Gsafety.PTMS.Monitor;
using Gsafety.PTMS.Monitor.ViewModels;
using Gsafety.PTMS.ServiceReference.MonitorGroupService;
using System.Reflection;

namespace Gsafety.PTMS.Monitor.Views
{
    public delegate void GroupDateRefresh();
    [ExportAsView(MonitorName.MonitorGroupManager, Category = "Navigation", MenuName = "MonitorGroupManager", ToolTip = "Click to view some text.")]
    public partial class MonitorGroupManager : ChildWindow
    {
        private int m_errorCount = 0;
        MonitorGroupServiceClient monitorGroupServiceClient = null;

        public event GroupDateRefresh groupDateRefresh;

        private ObservableCollection<UserDefineGroup> _groups;
        private ObservableCollection<UserDefineGroup> _oldgroups;
        private ObservableCollection<UserDefineGroup> _delGroups;

        public MonitorGroupManager()
        {
            InitializeComponent();
            this.DataContext = this;
            try
            {
                monitorGroupServiceClient = ServiceClientFactory.Create<MonitorGroupServiceClient>();
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
            _groups = ApplicationContext.Instance.BufferManager.VehicleOrganizationManage.Groups;
            buttonSet();
            _delGroups = new ObservableCollection<UserDefineGroup>();
            var ser = new System.Runtime.Serialization.DataContractSerializer(typeof(ObservableCollection<UserDefineGroup>));
            using (var ms = new System.IO.MemoryStream())
            {
                ser.WriteObject(ms, _groups);
                ms.Seek(0, System.IO.SeekOrigin.Begin);
                _oldgroups = (ObservableCollection<UserDefineGroup>)ser.ReadObject(ms); ;
            }

            this.GroupListView.ItemsSource = _groups;
            GroupListView.BindingValidationError += GroupListView_BindingValidationError;
            monitorGroupServiceClient.BatchAddMonitorGroupCompleted += monitorGroupServiceClient_BatchAddMonitorGroupCompleted;
            ApplicationContext.Instance.StringResourceReader.TranslateDataGrid(GroupListView);
        }

        private void monitorGroupServiceClient_BatchAddMonitorGroupCompleted(object sender, BatchAddMonitorGroupCompletedEventArgs e)
        {
            if (e.Result.IsSuccess)
            {
                this.DialogResult = true;
                ApplicationContext.Instance.MessageManager.SendChangeGroupMessage(item);
            }
        }

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
        
        
        public ChangeGroup item { get; set; }

        /// <summary>
        ///Determine
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            item=new ChangeGroup();
            item.GroupId=new ObservableCollection<string>();
            if (CheckDataRepeat()) return;
            for (int i = 0; i < _groups.Count; i++)
            {
                _groups[i].GroupIndex = (short)i;
            }

            ObservableCollection<Gsafety.PTMS.ServiceReference.MonitorGroupService.MonitorGroup> list = new ObservableCollection<Gsafety.PTMS.ServiceReference.MonitorGroupService.MonitorGroup>();
            int j = 0;
            foreach (UserDefineGroup mg in _groups)
            {
                list.Add(new Gsafety.PTMS.ServiceReference.MonitorGroupService.MonitorGroup() { ID = mg.ID, GroupIndex = (short)j, CreateUser = mg.CreateUser, Note = mg.Note, GroupName = mg.GroupName });
                  item.GroupId.Add(mg.ID);
                j++;
            }
            monitorGroupServiceClient.BatchAddMonitorGroupAsync(list, ApplicationContext.Instance.AuthenticationInfo.UserName);
            item.CreatUser = ApplicationContext.Instance.AuthenticationInfo.UserName;
        }

        /// <summary>
        /// Determine whether there are duplicate records
        /// </summary>
        /// <returns></returns>
        private bool CheckDataRepeat()
        {
            for (int i = 0; i < _groups.Count - 1; i++)
            {
                for (int j = i + 1; j < _groups.Count; j++)
                {
                    if (_groups[i].GroupName.Trim().Equals(_groups[j].GroupName.Trim()))
                    {
                        MessageBox.Show(string.Format(ApplicationContext.Instance.StringResourceReader.GetString("MONITOR_GroupRepeatMsg"), _groups[i].GroupName));
                        return true;
                    }

                }
            }

            return false;
        }

        /// <summary>
        /// Cancel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        /// <summary>
        /// Add a group record, click OK to save the last
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnAddExtent_Click(object sender, RoutedEventArgs e)
        {
            if (_groups.Count < 20)
            {
                UserDefineGroup item = new UserDefineGroup() { GroupName = "default", Note = string.Empty, CreateUser = ApplicationContext.Instance.AuthenticationInfo.UserName, GroupIndex = (short)_groups.Count, ID = Guid.NewGuid().ToString() };
                _groups.Add(item);
                buttonSet();
                this.GroupListView.ItemsSource = null;
                this.GroupListView.ItemsSource = _groups;
            }
            else
            {
                MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("MONITOR_GroupCountMsg"), ApplicationContext.Instance.StringResourceReader.GetString("GIS_Notice"), MessageBoxButton.OK);
            }
        }

        /// <summary>
        /// Remove a group record
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnDeleExtent_Click(object sender, RoutedEventArgs e)
        {
            if (null != this.GroupListView.SelectedItem)
            {
                UserDefineGroup selectItem = this.GroupListView.SelectedItem as UserDefineGroup;
                if (null != selectItem)
                {
                    if (selectItem.Vehicles != null && selectItem.Vehicles.Count > 0)
                    {
                        MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("MONITOR_GroupDeleConditionMsg"), ApplicationContext.Instance.StringResourceReader.GetString("GIS_Notice"), MessageBoxButton.OK);//MONITOR_Notice
                        return;
                    }
                    if (MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("MONITOR_GroupDeleConditionQuesitionMsg"), ApplicationContext.Instance.StringResourceReader.GetString("MONITOR_Notice"), MessageBoxButton.OKCancel) == MessageBoxResult.Cancel)
                    {
                        return;
                    }
                    _groups.Remove(selectItem);
                    buttonSet();
                    this.GroupListView.ItemsSource = null;
                    this.GroupListView.ItemsSource = _groups;
                    //_delGroups.Add(selectItem);
                }
            }
        }

        /// <summary>
        /// Forward a packet sorting unit
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnUpExtent_Click(object sender, RoutedEventArgs e)
        {
            if (null != this.GroupListView.SelectedItem)
            {
                UserDefineGroup selectItem = this.GroupListView.SelectedItem as UserDefineGroup;
                if (null != selectItem)
                {
                    int selectIndex = this.GroupListView.SelectedIndex;
                    if (selectIndex != 0)
                    {
                        _groups.Remove(selectItem);
                        _groups.Insert(selectIndex - 1, selectItem);

                        
                        buttonSet();
                        _groups = ApplicationContext.Instance.BufferManager.VehicleOrganizationManage.Groups;
                        this.GroupListView.ItemsSource = null;
                        this.GroupListView.ItemsSource = _groups;
                        this.GroupListView.UpdateLayout();
                        this.GroupListView.SelectedIndex = selectIndex - 1;
                        this.GroupListView.ScrollIntoView(this.GroupListView.SelectedItem, null);
                       
                    }
                    //if (selectIndex == 1)
                    //{
                        //foreach (UserDefineGroup item in _groups)
                        //{
                        //    item.UP_IsEnabled = "true";
                        //    //Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => ExportBtnStatus));
                        //}

                        //buttonSet();
                        //this.GroupListView.ItemsSource = null;
                        //this.GroupListView.ItemsSource = _groups;
                        //Button b = sender as Button;
                        //b.IsEnabled = false;
                    //}
                   
                }
            }

        }

        /// <summary>
        /// After the shift to a packet sorting unit
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnDownExtent_Click(object sender, RoutedEventArgs e)
        {
            if (null != this.GroupListView.SelectedItem)
            {
                UserDefineGroup selectItem = this.GroupListView.SelectedItem as UserDefineGroup;
                if (null != selectItem)
                {
                    int selectIndex = this.GroupListView.SelectedIndex;
                    if (selectIndex != _groups.Count - 1)
                    {
                        _groups.Remove(selectItem);
                        _groups.Insert(selectIndex + 1, selectItem);
                       

                        buttonSet();
                        _groups = ApplicationContext.Instance.BufferManager.VehicleOrganizationManage.Groups;
                        this.GroupListView.ItemsSource = null;
                        this.GroupListView.ItemsSource = _groups;
                        this.GroupListView.UpdateLayout();
                        this.GroupListView.SelectedIndex = selectIndex + 1;
                        this.GroupListView.ScrollIntoView(this.GroupListView.SelectedItem, null);
                      
                    }
                    //if (selectIndex == (_groups.Count-2))
                    //{
                    //    foreach (UserDefineGroup item in _groups)
                    //    {
                    //        item.DOWN_IsEnabled = "true";
                    //    }
                    //buttonSet();
                    //this.GroupListView.ItemsSource = null;
                    //this.GroupListView.ItemsSource = _groups;
                    //    Button b = sender as Button;
                    //    b.IsEnabled = false;
                    //}
                }
            }
        }

        private void buttonSet()
        {
            foreach (UserDefineGroup item in _groups)
            {
                if (item == _groups.First())
                { item.UP_IsEnabled = "false"; }
                else
                { item.UP_IsEnabled = "true"; }
                if (item == _groups.Last())
                {
                    item.DOWN_IsEnabled = "false";
                }
                else
                { item.DOWN_IsEnabled = "true"; }
            }
        }

        /// <summary>
        /// After validation by clicking the OK button before
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LayoutRoot_BindingValidationError(object sender, ValidationErrorEventArgs e)
        {
            if (IsValidation())
            {
                OKButton.IsEnabled = true;
            }
            else
            {
                OKButton.IsEnabled = false;
            }
        }

        /// <summary>
        /// Verify through
        /// </summary>
        /// <returns></returns>
        private bool IsValidation()
        {
            if (m_errorCount > 0)
            {
                return false;
            }
            //bool isok = this.GroupListView.IsValid;

            return true;
        }

        private void ChildWindow_Closing_1(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //if user cancel operator , this data will return back ;JiangJ
            if (DialogResult == false)
            {
                List<string> RemoveGroupIds = new List<string>();
                List<UserDefineGroup> ChangeIndexGroup = new List<UserDefineGroup>();

                //step1  add deleted group
                foreach (var delGroup in _delGroups)
                {
                    _groups.Add(delGroup);
                }

                //step2 find the date which need delete or changeIndex 
                foreach (UserDefineGroup group in _groups)
                {
                    UserDefineGroup record = _oldgroups.Where(item => item.ID == group.ID).FirstOrDefault();
                    if (record != null)
                    {
                        if (_groups.IndexOf(group) != group.GroupIndex)
                        {
                            ChangeIndexGroup.Add(group);
                        }
                        group.GroupName = record.Name;
                        group.Note = record.Note;
                        group.GroupIndex = record.GroupIndex;
                    }
                    else
                    {
                        RemoveGroupIds.Add(group.ID);
                    }
                }

                ///1 delete the data, from JiangJ
                foreach (var removeId in RemoveGroupIds)
                {
                    _groups.Remove(_groups.Where(item => item.ID == removeId).FirstOrDefault());
                }

                ///2 change the index, from JiangJ
                foreach (var group in ChangeIndexGroup)
                {
                    _groups.Remove(group);
                    _groups.Insert((int)group.GroupIndex, group);
                }
            }
            else
            {
                if (null != groupDateRefresh)
                {
                    groupDateRefresh();
                }
            }
        }
    }
}

