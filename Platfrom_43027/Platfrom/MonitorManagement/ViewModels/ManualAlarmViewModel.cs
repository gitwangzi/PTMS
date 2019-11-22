using BaseLib.Model;
using Gsafety.Ant.Monitor.Models;
using Gsafety.Common.Controls;
using Gsafety.PTMS.Monitor;
using Gsafety.PTMS.ServiceReference.VehicleAlarmService;
using Gsafety.PTMS.Share;
using Jounce.Core.Event;
using System.Linq;
using Gsafety.PTMS.ServiceReference.VehicleMonitorService;
using Jounce.Core.ViewModel;
using Jounce.Framework.Command;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Net;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Gsafety.PTMS.ServiceReference.EmailService;
using System.Text;
using Gsafety.PTMS.Monitor.Views;
using System.Collections.ObjectModel;
using Gsafety.PTMS.Bases.Enums;

namespace Gsafety.Ant.Monitor.ViewModels
{
    [ExportAsViewModel(MonitorName.MonitorManualAlarmHandleViewModel)]
    public class ManualAlarmViewModel : BaseViewModel, IPartImportsSatisfiedNotification, IEventSink<ManualAlarmHandleDisplayArgs>
    {
        public void OnImportsSatisfied()
        {
            EventAggregator.SubscribeOnDispatcher<ManualAlarmHandleDisplayArgs>(this);
        }

        public void HandleEvent(ManualAlarmHandleDisplayArgs publishedEvent)
        {
            try
            {
                if (publishedEvent.Show.HasValue)
                {
                    AlarmHandleIsVisual = publishedEvent.Show.Value;
                }
              
                VehicleID = publishedEvent.VehicleID;
                vehicleMonitorServiceClient = ServiceClientFactory.Create<VehicleMonitorServiceClient>();
                vehicleMonitorServiceClient.GetLastMonitorGPSCompleted += vehicleMonitorServiceClient_GetLastMonitorGPSCompleted;
                vehicleMonitorServiceClient.GetLastMonitorGPSAsync(VehicleID);
                HandleTime = DateTime.Now;
                Handler = ApplicationContext.Instance.AuthenticationInfo.Account;
                AlarmTime = DateTime.Now;
                AlarmIsTrue = true;

                FirstCheck = true;
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => FirstCheck));
                Note = string.Empty;
                IsTransfer = false;
                TransferMode = null;
                FirstCheck = true;
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => FirstCheck));

               
               
                Note = string.Empty;
                IncidentAddress = string.Empty;
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }

        }

        private string _incidentAddress;
        /// <summary>
        /// 
        /// </summary>
        public string IncidentAddress
        {
            get
            {
                return this._incidentAddress;
            }
            set
            {
                this._incidentAddress = value;
                RaisePropertyChanged(() => IncidentAddress);
            }
        }

        private string _title = ApplicationContext.Instance.StringResourceReader.GetString("MONITOR_AlarmByMonitor");
        /// <summary>
        /// 
        /// </summary>
        public string Title
        {
            get
            {
                return this._title;
            }
            set
            {
                this._title = value;
                RaisePropertyChanged(() => Title);
            }
        }

        ICommand okcommand = null;

        public ICommand OKCommand
        {
            get { return okcommand; }
            set { okcommand = value; }
        }
        ICommand cancelcommand = null;

        public ICommand CancelCommand
        {
            get { return cancelcommand; }
            set { cancelcommand = value; }
        }
        ICommand addemailcommand = null;

        public ICommand AddEmailCommand
        {
            get { return addemailcommand; }
            set { addemailcommand = value; }
        }

        ICommand updateemailcommand = null;

        public ICommand UpdateEmailCommand
        {
            get { return updateemailcommand; }
            set { updateemailcommand = value; }
        }

        ICommand deleteemailcommand = null;

        public ICommand DeleteEmailCommand
        {
            get { return deleteemailcommand; }
            set { deleteemailcommand = value; }
        }

        ICommand addnotecommand = null;

        public ICommand AddNoteCommand
        {
            get { return addnotecommand; }
            set { addnotecommand = value; }
        }
        public bool AlarmTrueEnable
        {
            get
            {
                return false;
            }
        }

        List<ListKeyValue> _notebinding;
        public List<ListKeyValue> NoteBinding
        {
            get { return _notebinding; }
            set { _notebinding = value; }
        }

        ListKeyValue _currentnote;
        public ListKeyValue CurrentNote
        {
            get { return _currentnote; }
            set { _currentnote = value; }
        }
        List<ListKeyValue> _typebinding;
        public List<ListKeyValue> TypeBinding
        {
            get { return _typebinding; }
            set { _typebinding = value; }
        }

        ListKeyValue _currentType;
        public ListKeyValue CurrentType
        {
            get { return _currentType; }
            set { _currentType = value; }
        }

        private ObservableCollection<AlarmEmailInfo> _sendpersons;
        public ObservableCollection<AlarmEmailInfo> SendPersons
        {
            get { return _sendpersons; }
            set
            {
                _sendpersons = value;
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => SendPersons));
            }
        }

        private AlarmEmailInfo _selectedperson;
        public AlarmEmailInfo SelectedPerson
        {
            get { return _selectedperson; }
            set
            {
                _selectedperson = value;
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => SelectedPerson));
            }
        }

        private int IncidentLevel { get; set; }

        #region RadioButton
        public bool FirstCheck
        {
            get;
            set;
        }

        public bool SecondCheck
        {
            get;
            set;
        }

        public bool ThirdCheck
        {
            get;
            set;
        }

        public bool FourthCheck
        {
            get;
            set;
        }
        #endregion

        public ManualAlarmViewModel()
        {
            okcommand = new ActionCommand<string>(obj => HandleAlarm(obj));
            cancelcommand = new ActionCommand<string>(obj => Cancel(obj));
            AddNoteCommand = new ActionCommand<string>(obj => AddAlarmNote(obj));
            AddEmailCommand = new ActionCommand<string>(obj => AddEmail(obj));
            UpdateEmailCommand = new ActionCommand<string>(obj => UpdateEmail(obj));
            DeleteEmailCommand = new ActionCommand<string>(obj => DeleteEmail(obj));
            TransferModes = new List<NameValueModel<int>>();
            TransferModes.Add(new NameValueModel<int>() { Name = "911", Value = 0 });
            FirstCheck = true;
            InilitClient();
            //TransferModes.Add(new NameValueModel<int>() { Name = "120", Value = 1 });
        }
        private EmailServiceClient emailclient = null;
        private VehicleAlarmServiceClient vehicleclient = null;
        private VehicleAlarmServiceClient InilitClient()
        {
           
            VehicleAlarmServiceClient vehicleAlarmServiceClient = ServiceClientFactory.Create<VehicleAlarmServiceClient>();
            vehicleAlarmServiceClient.GetAllAlarmNoteCompleted += vehicleAlarmServiceClient_GetAllAlarmNoteCompleted;
            vehicleAlarmServiceClient.GetAllAlarmEmailCompleted += vehicleAlarmServiceClient_GetAllAlarmEmailCompleted;
            vehicleAlarmServiceClient.GetAllAlarmNoteAsync(ApplicationContext.Instance.AuthenticationInfo.ClientID);
            vehicleAlarmServiceClient.GetAllAlarmEmailAsync(ApplicationContext.Instance.AuthenticationInfo.ClientID);
            vehicleAlarmServiceClient.GetAllAlarmTypeCompleted += vehicleAlarmServiceClient_GetAllAlarmTypeCompleted;          
            vehicleAlarmServiceClient.GetAllAlarmTypeAsync();
            return vehicleAlarmServiceClient;
        }


        void vehicleAlarmServiceClient_GetAllAlarmTypeCompleted(object sender, GetAllAlarmTypeCompletedEventArgs e)
        {
            try
            {
                if (e.Error == null && e.Result.Result != null)
                {

                    TypeBinding = new List<ListKeyValue>();


                    for (int i = 0; i < e.Result.Result.Count; i++)
                    {
                        var item = e.Result.Result[i];
                        ListKeyValue data = new ListKeyValue();
                        data.Name = item.name;
                        data.ID = item.incidentTypeId;
                        TypeBinding.Add(data);

                    }


                    CurrentType = TypeBinding[0];

                    Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => TypeBinding));

                    Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => CurrentType));
                }
                else
                {
                    MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString(LProxy.Caption), ApplicationContext.Instance.StringResourceReader.GetString(LProxy.ServerError));
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("vehicleAlarmServiceClient_GetApealDisposeByAlarmIDCompleted", ex);
            }
            finally
            {
                CloseClient(sender);
            }
        }
        private static void CloseClient(object sender)
        {
            VehicleAlarmServiceClient client = sender as VehicleAlarmServiceClient;
            if (client != null)
            {
                client.CloseAsync();
                client = null;
            }
        }
        public static class GPSState
        {
            public static bool Valid(string csvalid)
            {
                //2016.8.2
                if ((csvalid == "N") || (csvalid == null)) return false;
                //if ((str == "A") || (str == "V")) return true;
                return true;
            }
        }

        private Gsafety.PTMS.ServiceReference.VehicleMonitorService.GPS antgps = null;
        private VehicleMonitorServiceClient vehicleMonitorServiceClient = null;
        /// <summary>
        /// GPS coordinates obtained finally filled into the memory, and positioning
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void vehicleMonitorServiceClient_GetLastMonitorGPSCompleted(object sender, GetLastMonitorGPSCompletedEventArgs e)
        {
            try
            {
                if ((e.Result != null) && (e.Result.Result != null))
                {
                    antgps = e.Result.Result;
                   
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                if (vehicleMonitorServiceClient != null)
                {

                    vehicleMonitorServiceClient.CloseAsync();
                }
            
            }
        }


        void vehicleAlarmServiceClient_GetAllAlarmEmailCompleted(object sender, GetAllAlarmEmailCompletedEventArgs e)
        {
            try
            {
                if (e.Error == null && e.Result.Result != null)
                {
                    SendPersons = e.Result.Result;
                    if (SendPersons.Count > 0)
                    {
                        SelectedPerson = SendPersons[0];

                    }

                    Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => SendPersons));

                    Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => SelectedPerson));
                }
                else
                {
                    MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString(LProxy.Caption), ApplicationContext.Instance.StringResourceReader.GetString(LProxy.ServerError));
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("vehicleAlarmServiceClient_GetApealDisposeByAlarmIDCompleted", ex);
            }
            finally
            {
                CloseClient(sender);
            }
        }

        void vehicleAlarmServiceClient_GetAllAlarmNoteCompleted(object sender, GetAllAlarmNoteCompletedEventArgs e)
        {
            try
            {
                if (e.Error == null && e.Result.Result != null)
                {

                    NoteBinding = new List<ListKeyValue>();
                    ListKeyValue nudata = new ListKeyValue();
                    nudata.Name = ApplicationContext.Instance.StringResourceReader.GetString("BASEINFO_NULL");
                    nudata.ID = "BASEINFO_NULL";
                    NoteBinding.Add(nudata);

                    for (int i = 0; i < e.Result.Result.Count; i++)
                    {
                        var item = e.Result.Result[i];
                        ListKeyValue data = new ListKeyValue();
                        data.Name = item.Note;
                        data.ID = item.ID;
                        NoteBinding.Add(data);

                    }


                    CurrentNote = NoteBinding[0];

                    Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => NoteBinding));

                    Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => CurrentNote));
                }
                else
                {
                    MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString(LProxy.Caption), ApplicationContext.Instance.StringResourceReader.GetString(LProxy.ServerError));
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("vehicleAlarmServiceClient_GetApealDisposeByAlarmIDCompleted", ex);
            }
            finally
            {
                CloseClient(sender);
            }
        }

        private void AddEmail(string obj)
        {
            AlarmEmailInfo email = new AlarmEmailInfo();
            email.ClientId = ApplicationContext.Instance.AuthenticationInfo.ClientID;
            email.ID = Guid.NewGuid().ToString();
            email.Level = 1;
            email.Name = "";
            email.Mail = "";

            ChildSendMail child = new ChildSendMail(ApplicationContext.Instance.StringResourceReader.GetString("MONITOR_AddEmail"));
            child.Edit(email);
            child.Closed += (m, n) =>
            {
                if (child.DialogResult == true)
                {

                    try
                    {
                        vehicleclient = ServiceClientFactory.Create<VehicleAlarmServiceClient>();
                        vehicleclient.AddAlarmEmailCompleted += ((nsender, ne) =>
                        {

                            if (ne.Error != null || ne.Result.IsSuccess == false)
                            {
                                ApplicationContext.Instance.Logger.LogException("BASEINFO_Operate_Failed", ne.Error);
                                return;
                            }

                            SendPersons.Add(child.mail);
                            SelectedPerson = null;
                            Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => SendPersons));
                            if (SendPersons.Count > 0)
                            {
                                SelectedPerson = SendPersons[0];
                                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => SendPersons));

                            }
                            Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => SelectedPerson));

                        });
                        vehicleclient.AddAlarmEmailAsync(child.mail);

                    }
                    catch (Exception ex)
                    {

                    }
                    finally
                    {
                        vehicleclient.CloseAsync();

                    }


                }
            };
            child.Show();


        }

        private void UpdateEmail(string obj)
        {
            if (SelectedPerson != null)
            {
                AlarmEmailInfo email = SelectedPerson;


                ChildSendMail child = new ChildSendMail(ApplicationContext.Instance.StringResourceReader.GetString("MONITOR_EditEmail"));
                child.Edit(email);
                child.Closed += (m, n) =>
                {
                    if (child.DialogResult == true)
                    {

                        try
                        {
                            vehicleclient = ServiceClientFactory.Create<VehicleAlarmServiceClient>();
                            vehicleclient.UpdateAlarmEmailCompleted += ((nsender, ne) =>
                            {

                                if (ne.Error != null || ne.Result.IsSuccess == false)
                                {
                                    ApplicationContext.Instance.Logger.LogException("BASEINFO_Operate_Failed", ne.Error);
                                    return;
                                }

                                SelectedPerson.Level = child.mail.Level;
                                SelectedPerson.Mail = child.mail.Mail;
                                SelectedPerson.Name = child.mail.Name;

                                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => SendPersons));



                            });
                            vehicleclient.UpdateAlarmEmailAsync(SelectedPerson);

                        }
                        catch (Exception ex)
                        {

                        }
                        finally
                        {
                            vehicleclient.CloseAsync();

                        }


                    }
                };
                child.Show();

            }


        }

        private void DeleteEmail(string obj)
        {
            string Caption = ApplicationContext.Instance.StringResourceReader.GetString(LProxy.Caption);

            if (SelectedPerson != null)
            {
                ChildWindow result;

                result = (SelfMessageBox)MessageBoxHelper.ShowDialog(Caption, ApplicationContext.Instance.StringResourceReader.GetString("IsDelete"), MessageDialogButton.OkAndCancel);

                result.Closed += (a, b) =>
                {
                    if (result.DialogResult == true)
                    {
                        try
                        {
                            vehicleclient = ServiceClientFactory.Create<VehicleAlarmServiceClient>();
                            vehicleclient.DeleteAlarmEmailCompleted += ((nsender, ne) =>
                            {

                                if (ne.Error != null || ne.Result.IsSuccess == false)
                                {
                                    ApplicationContext.Instance.Logger.LogException("BASEINFO_Operate_Failed", ne.Error);
                                    return;
                                }

                                SendPersons.Remove(SelectedPerson);
                                SelectedPerson = null;
                                if (SendPersons.Count > 0)
                                {
                                    SelectedPerson = SendPersons[0];
                                }
                                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => SendPersons));
                                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => SelectedPerson));

                            });
                            vehicleclient.DeleteAlarmEmailAsync(SelectedPerson.ID);
                        }
                        catch (Exception ex)
                        {
                            ApplicationContext.Instance.Logger.LogException("BtnAddExtent", ex);
                        }
                        finally
                        {
                            vehicleclient.CloseAsync();
                        }

                    }
                };
            }


        }

        void emailclient_SendEmailCompleted(object sender, SendEmailCompletedEventArgs e)
        {
            try
            {
                if (e.Cancelled == true)
                {
                    return;
                }

                if (e.Error != null)
                {
                    MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString("Tip"), ApplicationContext.Instance.StringResourceReader.GetString("ALARM_SendEmailFailure"), MessageDialogButton.Ok);
                    return;
                }

                var result = e.Result;
                if (result.IsSuccess == false)
                {

                    MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString("Tip"), ApplicationContext.Instance.StringResourceReader.GetString("ALARM_SendEmailFailure"), MessageDialogButton.Ok);

                }
                else
                {
                    //MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString("Tip"), ApplicationContext.Instance.StringResourceReader.GetString("ALARM_SendSuc"), MessageDialogButton.Ok);

                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
            finally
            {
                if (emailclient != null)
                {
                    emailclient.CloseAsync();
                }
            }
        }


        private void AlarmSendEmail_Event()
        {
            try
            {

                EmailInfo email = new EmailInfo();
                string spaces = "  :  ";
                string br1 = "<br>";
                string br2 = "</br>";
                if (VehicleID != null)
                {
                    var vehicleInfo = ApplicationContext.Instance.BufferManager.VehicleOrganizationManage.VehicleList.FirstOrDefault(t => t.VehicleId == VehicleID);
                    

                    StringBuilder strBuilder = new System.Text.StringBuilder();


                   
                    string strVehicleType = string.Empty;

                    if (vehicleInfo != null)
                    {

                        strVehicleType = vehicleInfo.VehicleTypeDescribe;
                    }
                  

                    strBuilder.Append("<table width=680>");
                    strBuilder.Append("<tr><td>");
                    strBuilder.Append(ApplicationContext.Instance.StringResourceReader.GetString("ALERT_ProvinceName"));
                    strBuilder.Append(spaces);
                    strBuilder.Append(vehicleInfo.ProvinceName);
                    strBuilder.Append("</td>");
                    strBuilder.Append("<td>");
                    strBuilder.Append(ApplicationContext.Instance.StringResourceReader.GetString("ALERT_CityName"));
                    strBuilder.Append(spaces);
                    strBuilder.Append(vehicleInfo.CityName);
                    strBuilder.Append("</td></tr>");

                    strBuilder.Append("<tr><td>");
                    strBuilder.Append(ApplicationContext.Instance.StringResourceReader.GetString("ALERT_VehicleType"));
                    strBuilder.Append(spaces);
                    strBuilder.Append(strVehicleType);
                    strBuilder.Append("</td>");
                    strBuilder.Append("<td>");
                    strBuilder.Append(ApplicationContext.Instance.StringResourceReader.GetString("ALARM_VehicleId"));
                    strBuilder.Append(spaces);
                    strBuilder.Append(vehicleInfo.VehicleId);
                    strBuilder.Append("</td></tr>");

                    strBuilder.Append("<tr><td>");
                    strBuilder.Append(ApplicationContext.Instance.StringResourceReader.GetString("Note"));
                    strBuilder.Append(spaces);
                    if (CurrentNote != null)
                    {
                       
                        strBuilder.Append(CurrentNote.Name);
                    }
                    else
                    {                      
                        strBuilder.Append(ApplicationContext.Instance.StringResourceReader.GetString("BASEINFO_NULL"));
                    }
                    strBuilder.Append("</td>");
                    strBuilder.Append("<td>");
                    strBuilder.Append(ApplicationContext.Instance.StringResourceReader.GetString("ALARM_AlarmTime"));
                    strBuilder.Append(spaces);
                    strBuilder.Append(AlarmTime.Value.ToString());
                    strBuilder.Append("</td></tr>");

                    strBuilder.Append("<tr><td>");
                    strBuilder.Append(ApplicationContext.Instance.StringResourceReader.GetString("GIS_Lon"));
                    strBuilder.Append(spaces);
                    strBuilder.Append(antgps.Longitude);
                    strBuilder.Append("</td>");
                    strBuilder.Append("<td>");
                    strBuilder.Append(ApplicationContext.Instance.StringResourceReader.GetString("GIS_Lat"));
                    strBuilder.Append(spaces);
                    strBuilder.Append(antgps.Latitude);
                    strBuilder.Append("</td></tr>");

                    strBuilder.Append("<tr><td>");
                    strBuilder.Append(ApplicationContext.Instance.StringResourceReader.GetString("ALERT_Speed"));
                    strBuilder.Append(spaces);
                    strBuilder.Append(antgps.Speed);
                    strBuilder.Append("</td>");
                    strBuilder.Append("<td>");
                    strBuilder.Append(ApplicationContext.Instance.StringResourceReader.GetString("ALERT_Dir"));
                    strBuilder.Append(spaces);
                    strBuilder.Append(antgps.Direction);
                    strBuilder.Append("</td></tr>");

                    strBuilder.Append("<tr><td>");
                    strBuilder.Append(ApplicationContext.Instance.StringResourceReader.GetString("ALARM_HandlePerson"));
                    strBuilder.Append(spaces);
                    strBuilder.Append(Handler);
                    strBuilder.Append("</td>");
                    strBuilder.Append("<td>");
                    strBuilder.Append(ApplicationContext.Instance.StringResourceReader.GetString("ALARM_TrealTime"));
                    strBuilder.Append(spaces);
                    strBuilder.Append(HandleTime.Value.ToString());
                    strBuilder.Append("</td></tr>");

                    string level = string.Empty;
                    if (IncidentLevel >= 1)
                    {
                        if (IncidentLevel == 1)
                        {
                            level = ApplicationContext.Instance.StringResourceReader.GetString("IncidentCommon");
                        }
                        if (IncidentLevel == 2)
                        {
                            level = ApplicationContext.Instance.StringResourceReader.GetString("IncidentLarger");

                        }
                        if (IncidentLevel == 3)
                        {
                            level = ApplicationContext.Instance.StringResourceReader.GetString("IncidentMajor");

                        }
                        if (IncidentLevel == 4)
                        {
                            level = ApplicationContext.Instance.StringResourceReader.GetString("IncidentSpecialSignificant");

                        }

                    }

                    strBuilder.Append("<tr><td>");
                    strBuilder.Append(ApplicationContext.Instance.StringResourceReader.GetString("IncidentLevel"));
                    strBuilder.Append(spaces);
                    strBuilder.Append(level);
                    strBuilder.Append("</td>");
                    strBuilder.Append("<td>");
                    strBuilder.Append(ApplicationContext.Instance.StringResourceReader.GetString("IncidentAddress"));
                    strBuilder.Append(spaces);
                    strBuilder.Append(IncidentAddress);
                    strBuilder.Append("</td></tr>");

                    strBuilder.Append("<tr><td>");
                    strBuilder.Append(ApplicationContext.Instance.StringResourceReader.GetString("MONITOR_IncidentType"));
                    strBuilder.Append(spaces);

                    if (CurrentType != null)
                    {

                        strBuilder.Append(CurrentType.Name);
                    }
                    else
                    {
                        strBuilder.Append(ApplicationContext.Instance.StringResourceReader.GetString("BASEINFO_NULL"));
                    }
                    strBuilder.Append("</td></tr>");
                   
                   

                 


                    strBuilder.Append("</table>");

                    email.MailBody = strBuilder.ToString();

                    email.IsbodyHtml = true; //Whether it is HTML

                    email.MailSubject = "ManualAlarm " + VehicleID + " " + AlarmTime.Value.ToString();

                    email.MailToArray = new ObservableCollection<string> { };

                    foreach (var item in SendPersons)
                    {

                        if (item.Level == IncidentLevel)
                        {

                            email.MailToArray.Add(item.Mail);

                        }
                    }


                    emailclient = ServiceClientFactory.Create<EmailServiceClient>();
                    emailclient.SendEmailCompleted += emailclient_SendEmailCompleted;
                    emailclient.SendEmailAsync(email);

                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                if (emailclient != null)
                {

                    emailclient.CloseAsync();
                }

            }
        }

        private void AddAlarmNote(string obj)
        {

            List<ListKeyValue> data = NoteBinding;

            if (data.Count > 0)
            {
                data.RemoveAt(0);
            }

            Views.MonitorHandleNoteManagerView addGroup = new Views.MonitorHandleNoteManagerView(data);
            addGroup.Show();

            addGroup.Closed += (a, b) =>
            {
                if (addGroup.DialogResult == true)
                {
                    VehicleAlarmServiceClient vehicleAlarmServiceClient = InilitClient();

                }

            };

        }
        private bool _ismail;
        /// <summary>
        /// 
        /// </summary>
        public bool IsMail
        {
            get
            {
                return this._ismail;
            }
            set
            {
                this._ismail = value;
                RaisePropertyChanged(() => IsMail);
            }
        }
        private void Cancel(string obj)
        {
            AlarmHandleIsVisual = false;
        }

        private void HandleAlarm(string obj)
        {
            try
            {
                if (FirstCheck)
                {
                    IncidentLevel = (int)IncidentLevelEnum.Common;
                }
                else if (SecondCheck)
                {
                    IncidentLevel = (int)IncidentLevelEnum.Bigger;
                }
                else if (ThirdCheck)
                {
                    IncidentLevel = (int)IncidentLevelEnum.Major;
                }
                else
                {
                    IncidentLevel = (int)IncidentLevelEnum.EspcialMajor;
                }

                if (CurrentNote == null)
                {
                    if (NoteBinding.Count >= 0)
                    {
                        CurrentNote = NoteBinding[0];

                        Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => CurrentNote));
                    }
                }

                if (CurrentType == null)
                {
                    if (TypeBinding.Count >= 0)
                    {
                        CurrentType = TypeBinding[0];

                        Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => CurrentType));
                    }
                }
                ManualAlarmInfo manualinfo = new ManualAlarmInfo();
                manualinfo.Account = ApplicationContext.Instance.AuthenticationInfo.Account;
                manualinfo.UserID = ApplicationContext.Instance.AuthenticationInfo.UserID;
                manualinfo.ClientID = ApplicationContext.Instance.AuthenticationInfo.ClientID;
                manualinfo.GPSTime = this.AlarmTime.Value.ToUniversalTime();
                manualinfo.ID = Guid.NewGuid().ToString();

                if (CurrentNote != null)
                {
                    manualinfo.Note = CurrentNote.Name;
                }

                manualinfo.IncidentAddress = IncidentAddress;
                manualinfo.IncidentLevel = IncidentLevel;
                if (CurrentType != null)
                {
                    manualinfo.IncidentType = CurrentType.ID;
                }              
                manualinfo.Source = 1;
                manualinfo.VehicleID = VehicleID;
                manualinfo.IsTransfer = IsTransfer;
                if (TransferMode != null)
                {
                    manualinfo.TransferMode = (short)TransferMode.Value;
                }
                VehicleAlarmServiceClient vehicleAlarmServiceClient = InitialClient();
                vehicleAlarmServiceClient.InsertManualAlarmAsync(manualinfo);
                IsCommitEnable = false;
                if (IsMail)
                {

                    AlarmSendEmail_Event();

                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }


        private bool _iscommitenable = true;
        public bool IsCommitEnable
        {
            get
            {
                return _iscommitenable;
            }
            set
            {
                _iscommitenable = value;
                RaisePropertyChanged(() => IsCommitEnable);
            }
        }

        private bool m_IsOpen = true;
        public bool AlarmHandleIsOpen
        {
            get
            {
                return m_IsOpen;
            }
            set
            {
                m_IsOpen = value;
            }
        }

        private bool _IsVisual = true;
        public bool AlarmHandleIsVisual
        {
            get
            {
                return _IsVisual;
            }
            set
            {

                _IsVisual = value;
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => AlarmHandleIsVisual));
            }
        }

        private string _vehicleid;
        /// <summary>
        /// 
        /// </summary>
        public string VehicleID
        {
            get
            {
                return this._vehicleid;
            }
            set
            {
                this._vehicleid = value;
                RaisePropertyChanged(() => VehicleID);
            }
        }

        private string _note;
        /// <summary>
        /// 
        /// </summary>
        public string Note
        {
            get
            {
                return this._note;
            }
            set
            {
                this._note = value;
                RaisePropertyChanged(() => Note);
            }
        }


        private string _handler;
        /// <summary>
        /// 
        /// </summary>
        public string Handler
        {
            get
            {
                return this._handler;
            }
            set
            {
                this._handler = value;
                RaisePropertyChanged(() => Handler);
            }
        }
        private DateTime? _alarmtime;
        /// <summary>
        /// 
        /// </summary>
        public DateTime? AlarmTime
        {
            get
            {
                return this._alarmtime;
            }
            set
            {
                this._alarmtime = value;
                RaisePropertyChanged(() => AlarmTime);
            }
        }
        private DateTime? _handletime;
        /// <summary>
        /// 
        /// </summary>
        public DateTime? HandleTime
        {
            get
            {
                return this._handletime;
            }
            set
            {
                this._handletime = value;
                RaisePropertyChanged(() => HandleTime);
            }
        }
        private bool _alarmistrue;
        /// <summary>
        /// 
        /// </summary>
        public bool AlarmIsTrue
        {
            get
            {
                return this._alarmistrue;
            }
            set
            {
                this._alarmistrue = value;
                if (_alarmistrue == false)
                {
                    IsTransfer = false;
                }
                RaisePropertyChanged(() => AlarmIsTrue);
            }
        }
        private bool _istransfer;
        /// <summary>
        /// 
        /// </summary>
        public bool IsTransfer
        {
            get
            {
                return this._istransfer;
            }
            set
            {
                this._istransfer = value;
                RaisePropertyChanged(() => IsTransfer);
            }
        }
        private List<NameValueModel<int>> _transfermodes;
        /// <summary>
        /// 
        /// </summary>
        public List<NameValueModel<int>> TransferModes
        {
            get
            {
                return this._transfermodes;
            }
            set
            {
                this._transfermodes = value;
                RaisePropertyChanged(() => TransferModes);
            }
        }
        private NameValueModel<int> _transfermode;
        /// <summary>
        /// 
        /// </summary>
        public NameValueModel<int> TransferMode
        {
            get
            {
                return this._transfermode;
            }
            set
            {
                this._transfermode = value;
                RaisePropertyChanged(() => TransferMode);
            }
        }

        private VehicleAlarmServiceClient InitialClient()
        {
            VehicleAlarmServiceClient vehicleAlarmServiceClient = ServiceClientFactory.Create<VehicleAlarmServiceClient>();
            vehicleAlarmServiceClient.InsertManualAlarmCompleted += vehicleAlarmServiceClient_InsertManualAlarmCompleted;
            return vehicleAlarmServiceClient;
        }

        void vehicleAlarmServiceClient_InsertManualAlarmCompleted(object sender, InsertManualAlarmCompletedEventArgs e)
        {
            try
            {
                if (e.Error == null)
                {
                    if (e.Result.IsSuccess == true)
                    {
                        MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString("Tip"), ApplicationContext.Instance.StringResourceReader.GetString("ALARM_SendSuc"), MessageDialogButton.Ok);
                        //e.Result.Result.AlarmTime = e.Result.Result.AlarmTime.Value.ToLocalTime();

                        ApplicationContext.Instance.BufferManager.AlarmManager.HandleManualAlarm(e.Result.Result);
                        AlarmHandleIsVisual = false;
                        IsCommitEnable = true;
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(e.Result.ErrorMsg))
                        {
                            MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString(LProxy.Caption), ApplicationContext.Instance.StringResourceReader.GetString(LProxy.OperatorServiceError));
                        }
                        else
                        {
                            MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString(LProxy.Caption), ApplicationContext.Instance.StringResourceReader.GetString(e.Result.ErrorMsg));
                        }
                    }
                }
                else
                {
                    MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString(LProxy.Caption), ApplicationContext.Instance.StringResourceReader.GetString(LProxy.ServerError));
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
            finally
            {
                VehicleAlarmServiceClient client = sender as VehicleAlarmServiceClient;
                client.CloseAsync();
                client = null;
            }
        }
    }
}
