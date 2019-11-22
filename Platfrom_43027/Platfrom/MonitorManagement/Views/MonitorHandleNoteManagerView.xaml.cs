using Gsafety.Ant.Monitor.ViewModels;
using Gsafety.Common.Controls;
using Gsafety.PTMS.Constants;
using Gsafety.PTMS.Monitor;
using Gsafety.PTMS.ServiceReference.RunMonitorGroupService;
using Gsafety.PTMS.ServiceReference.VehicleService;
using Gsafety.PTMS.ServiceReference.VehicleAlarmService;
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
    public partial class MonitorHandleNoteManagerView : ChildWindow, INotifyPropertyChanged
    {
        protected string MsgHasChildren = ApplicationContext.Instance.StringResourceReader.GetString("MsgMonitorGroupHasVehicel");
        protected string Caption = ApplicationContext.Instance.StringResourceReader.GetString(LProxy.Caption);


        public ObservableCollection<RunMonitorGroupVehicle> Vehicles { get; set; }

        public RunMonitorGroupVehicle model { get; set; }

       

        public VehicleAlarmServiceClient alarmclient = null;


        /// <summary>
        /// 车牌号
        /// </summary>
        private string _CarNo { get; set; }

        public MonitorHandleNoteManagerView(List<ListKeyValue> notebinding)
        {
            InitializeComponent();
            this.DataContext = this;
            Gsafety.PTMS.Share.ApplicationContext.Instance.StringResourceReader.TranslateDataGrid(GroupListView);
            MonitorNote = notebinding;
            if (MonitorNote.Count > 0)
            {
                SelectedItem = _monitornote[0];
            }

        }

        List<ListKeyValue> _monitornote;
        public List<ListKeyValue> MonitorNote
        {
            get { return _monitornote; }
            set
            {
                _monitornote = value;
                this.RaisePropertyChanged("MonitorNote");
            }
        }

     

        private void ChildWindow_MouseRightButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            e.Handled = true;
        }



        private void BtnAddGroup_Click_1(object sender, RoutedEventArgs e)
        {
            RunMonitorGroup item = new RunMonitorGroup()
            {
                GroupName = "Default",
                Note = string.Empty,
                AdUser = ApplicationContext.Instance.AuthenticationInfo.UserID,
                ID = Guid.NewGuid().ToString(),
                GroupIndex = 0
            };
            MonitorNoteEdit mge = new MonitorNoteEdit();
            mge.Edit(item);
            mge.Closed += (m, n) =>
            {
                if (mge.DialogResult == true)
                {
                  
                    try
                    {
                        alarmclient = ServiceClientFactory.Create<VehicleAlarmServiceClient>();
                        alarmclient.AddAlarmNoteCompleted += ((nsender, ne) =>
                        {

                            if (ne.Error != null || ne.Result.IsSuccess == false)
                            {
                                ApplicationContext.Instance.Logger.LogException("BASEINFO_Operate_Failed", ne.Error);
                                return;
                            }

                            ListKeyValue nudata = new ListKeyValue();
                            nudata.Name = mge.EditRunMonitorGroup.Note;
                            nudata.ID = mge.EditRunMonitorGroup.ID;
                            MonitorNote.Add(nudata);
                            SelectedItem = null;
                            Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged("SelectedItem")); 
                            if (MonitorNote.Count > 0)
                            {
                                SelectedItem = MonitorNote[0];
                            }                         
                            Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged("SelectedItem")); 
                            Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged("MonitorNote")); 
                           

                        });
                        alarmclient.AddAlarmNoteAsync(mge.EditRunMonitorGroup.ID, ApplicationContext.Instance.AuthenticationInfo.ClientID, mge.EditRunMonitorGroup.Note);
                    }
                    catch (Exception ex)
                    {
                        ApplicationContext.Instance.Logger.LogException("BtnAddExtent", ex);
                    }
                    finally
                    {
                        alarmclient.CloseAsync();
                    }     

                }
            };


        }      

        private void BtnDeleGroup_Click_1(object sender, RoutedEventArgs e)
        {
            if (this.GroupListView.SelectedItem != null)
            {
                ListKeyValue selectedItem = this.GroupListView.SelectedItem as ListKeyValue;

                if (selectedItem != null)
                {
                    ChildWindow result;
                   
                    result = (SelfMessageBox)MessageBoxHelper.ShowDialog(Caption, ApplicationContext.Instance.StringResourceReader.GetString("IsDelete"), MessageDialogButton.OkAndCancel);
                    
                    result.Closed += (a, b) =>
                    {
                        if (result.DialogResult == true)
                        {
                            try
                            {
                                alarmclient = ServiceClientFactory.Create<VehicleAlarmServiceClient>();
                                alarmclient.DeleteAlarmNoteCompleted += ((nsender, ne) =>
                                {

                                    if (ne.Error != null || ne.Result.IsSuccess == false)
                                    {
                                        ApplicationContext.Instance.Logger.LogException("BASEINFO_Operate_Failed", ne.Error);
                                        return;
                                    }

                                    MonitorNote.Remove(selectedItem);
                                    if (MonitorNote.Count > 0)
                                    {
                                        SelectedItem = MonitorNote[0];
                                    }
                                    Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged("SelectedItem"));
                                    Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged("MonitorNote")); 

                                });
                                alarmclient.DeleteAlarmNoteAsync(selectedItem.ID);
                            }
                            catch (Exception ex)
                            {
                                ApplicationContext.Instance.Logger.LogException("BtnAddExtent", ex);
                            }
                            finally
                            {
                                alarmclient.CloseAsync();
                            }                        
                           
                        }
                    };
                }
            }
        }

        public ListKeyValue selectedItem = null;

        public ListKeyValue SelectedItem
        {
            get { return selectedItem; }
            set
            {
                selectedItem = value;
                this.RaisePropertyChanged("SelectedItem");
            }
        }

     




        private RunMonitorGroup selectedMg;
        private void OKButton_Click_1(object sender, RoutedEventArgs e)
        {
           


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


                ListKeyValue selectedItem = this.GroupListView.SelectedItem as ListKeyValue;

                RunMonitorGroup item = new RunMonitorGroup()
                {
                    GroupName = selectedItem.Name,
                    Note = selectedItem.Name,
                    AdUser = ApplicationContext.Instance.AuthenticationInfo.UserID,
                    ID = selectedItem.ID,
                    GroupIndex = 0
                };
                MonitorNoteEdit mge = new MonitorNoteEdit();
                mge.Edit(item);
                mge.Closed += (m, n) =>
                {
                    if (mge.DialogResult == true)
                    {
                        try
                        {
                            alarmclient = ServiceClientFactory.Create<VehicleAlarmServiceClient>();
                            alarmclient.UpdateAlarmNoteCompleted += ((nsender, ne) =>
                                {

                                    if (ne.Error != null || ne.Result.IsSuccess == false)
                                    {
                                        ApplicationContext.Instance.Logger.LogException("BASEINFO_Operate_Failed", ne.Error);
                                        return;
                                    }

                                    selectedItem.Name = mge.EditRunMonitorGroup.Note;
                                    selectedItem.ID = mge.EditRunMonitorGroup.ID;
                                    Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged("SelectedItem"));
                                    Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged("MonitorNote")); 

                                });
                            alarmclient.UpdateAlarmNoteAsync(mge.EditRunMonitorGroup.ID, mge.EditRunMonitorGroup.Note);
                        }
                        catch (Exception ex)
                        {
                            ApplicationContext.Instance.Logger.LogException("BtnAddExtent", ex);
                        }
                        finally
                        {
                            alarmclient.CloseAsync();
                        }
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

