using BaseLib.Model;
using Gsafety.Ant.Monitor.Models;
using Gsafety.Common.Controls;
using Gsafety.PTMS.Bases.Enums;
using Gsafety.PTMS.Monitor;
using Gsafety.PTMS.ServiceReference.VehicleAlarmService;
using Gsafety.PTMS.ServiceReference.VehicleAlertService;
using Gsafety.PTMS.Share;
using Jounce.Core.Event;
using Jounce.Core.ViewModel;
using Jounce.Framework.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace Gsafety.Ant.Monitor.ViewModels
{
    [ExportAsViewModel(MonitorName.MonitorAlertHandleViewModel)]
    public class AlertHandleViewModel : BaseViewModel, IEventSink<MonitorAlertInfoDisplay>, IPartImportsSatisfiedNotification, IEventSink<AlertHandleDisplayArgs>
    {
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

        public AlertHandleViewModel()
        {
            okcommand = new ActionCommand<string>(obj => HandleAlert(obj));
            cancelcommand = new ActionCommand<string>(obj => Cancel(obj));
            AddEmailCommand = new ActionCommand<string>(obj => AddEmail(obj));
            UpdateEmailCommand = new ActionCommand<string>(obj => UpdateEmail(obj));
            DeleteEmailCommand = new ActionCommand<string>(obj => DeleteEmail(obj));
            FirstCheck = true;
            InilitClient();
        }

        private void Cancel(string obj)
        {
            Note = string.Empty;
            IsMail = false;
            IsVisual = false;
        }

        private void HandleAlert(string obj)
        {
            try
            {
                BusinessAlertHandle alerthandler = new BusinessAlertHandle();
                if (FirstCheck)
                {
                    alerthandler.AlertLevel = (int)AlertLevelEnum.Normal;
                }
                else if (SecondCheck)
                {
                    alerthandler.AlertLevel = (int)AlertLevelEnum.Medium;
                }
                else if (ThirdCheck)
                {
                    alerthandler.AlertLevel = (int)AlertLevelEnum.High;
                }
                else if(FourthCheck)
                {
                    alerthandler.AlertLevel = (int)AlertLevelEnum.Serious;
                }
                alerthandler.BusinessAlertID = alertinfo.Id;
                alerthandler.ID = Guid.NewGuid().ToString();
                alerthandler.HandleUser = ApplicationContext.Instance.AuthenticationInfo.Account;
                alerthandler.HandleTime = HandleTime.Value.ToUniversalTime();
                alerthandler.Content = Note;

                VehicleAlertServiceClient vehicleAlertServiceClient = InilitClient();
                vehicleAlertServiceClient.InsertBusinessAlertHandleAsync(alerthandler);

                if (IsMail)
                {

                    AlertSendEmail_Event();

                }

                IsCommitEnable = false;
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }

        private void AlertSendEmail_Event()
        {
            try
            {

                EmailInfo email = new EmailInfo();
                string spaces = "  :  ";
                string br1 = "<br>";
                string br2 = "</br>";
                if (alertinfo != null)
                {
                    StringBuilder strBuilder = new System.Text.StringBuilder();



                    string strVehicleType = string.Empty;
                    if (alertinfo.VehicleType != null)
                    {
                        strVehicleType = alertinfo.VehicleType;

                    }

                    strBuilder.Append("<table width=680>");
                    strBuilder.Append("<tr><td>");
                    strBuilder.Append(ApplicationContext.Instance.StringResourceReader.GetString("ALERT_ProvinceName"));
                    strBuilder.Append(spaces);
                    strBuilder.Append(alertinfo.Province);
                    strBuilder.Append("</td>");
                    strBuilder.Append("<td>");
                    strBuilder.Append(ApplicationContext.Instance.StringResourceReader.GetString("ALERT_CityName"));
                    strBuilder.Append(spaces);
                    strBuilder.Append(alertinfo.City);
                    strBuilder.Append("</td></tr>");

                    strBuilder.Append("<tr><td>");
                    strBuilder.Append(ApplicationContext.Instance.StringResourceReader.GetString("ALERT_VehicleType"));
                    strBuilder.Append(spaces);
                    strBuilder.Append(strVehicleType);
                    strBuilder.Append("</td>");
                    strBuilder.Append("<td>");
                    strBuilder.Append(ApplicationContext.Instance.StringResourceReader.GetString("ALARM_VehicleId"));
                    strBuilder.Append(spaces);
                    strBuilder.Append(alertinfo.VehicleId);
                    strBuilder.Append("</td></tr>");

                    strBuilder.Append("<tr><td>");
                    strBuilder.Append(ApplicationContext.Instance.StringResourceReader.GetString("ALERT_SecuritySuitID"));
                    strBuilder.Append(spaces);
                    strBuilder.Append(alertinfo.MdvrCoreId);
                    strBuilder.Append("</td>");
                    strBuilder.Append("<td>");
                    strBuilder.Append(ApplicationContext.Instance.StringResourceReader.GetString("ALARM_AlarmTime"));
                    strBuilder.Append(spaces);
                    strBuilder.Append(alertinfo.AlertTime.Value.ToString());
                    strBuilder.Append("</td></tr>");

                    strBuilder.Append("<tr><td>");
                    strBuilder.Append(ApplicationContext.Instance.StringResourceReader.GetString("GIS_Lon"));
                    strBuilder.Append(spaces);
                    strBuilder.Append(alertinfo.Longitude);
                    strBuilder.Append("</td>");
                    strBuilder.Append("<td>");
                    strBuilder.Append(ApplicationContext.Instance.StringResourceReader.GetString("GIS_Lat"));
                    strBuilder.Append(spaces);
                    strBuilder.Append(alertinfo.Latitude);
                    strBuilder.Append("</td></tr>");

                    strBuilder.Append("<tr><td>");
                    strBuilder.Append(ApplicationContext.Instance.StringResourceReader.GetString("ALERT_Speed"));
                    strBuilder.Append(spaces);
                    strBuilder.Append(alertinfo.Speed);
                    strBuilder.Append("</td>");
                    strBuilder.Append("<td>");
                    strBuilder.Append(ApplicationContext.Instance.StringResourceReader.GetString("ALERT_Dir"));
                    strBuilder.Append(spaces);
                    strBuilder.Append(alertinfo.Direction);
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
                    if (alertinfo.AlertLevel >= 1)
                    {
                        if (alertinfo.AlertLevel == 1)
                        {
                            level = ApplicationContext.Instance.StringResourceReader.GetString("IncidentCommon");
                        }
                        if (alertinfo.AlertLevel == 2)
                        {
                            level = ApplicationContext.Instance.StringResourceReader.GetString("IncidentLarger");

                        }
                        if (alertinfo.AlertLevel == 3)
                        {
                            level = ApplicationContext.Instance.StringResourceReader.GetString("IncidentMajor");

                        }
                        if(alertinfo.AlertLevel == 4)
                        {
                            level = ApplicationContext.Instance.StringResourceReader.GetString("IncidentSpecialSignificant");
                        }
                      
                    }

                    strBuilder.Append("<tr><td>");
                    strBuilder.Append(ApplicationContext.Instance.StringResourceReader.GetString("AlertLevel"));
                    strBuilder.Append(spaces);
                    strBuilder.Append(level);
                    strBuilder.Append("</td>");
                    strBuilder.Append("<td>");            
                    strBuilder.Append(ApplicationContext.Instance.StringResourceReader.GetString("Note"));
                    strBuilder.Append(spaces);
                    strBuilder.Append(Note);
                    strBuilder.Append("</td></tr>");

                    strBuilder.Append("</table>");

                    email.MailBody = strBuilder.ToString();

                    email.IsbodyHtml = true; //Whether it is HTML

                    email.MailSubject = "Alert " + alertinfo.VehicleId + " " + alertinfo.AlertTime.Value.ToString();

                    email.MailToArray = new ObservableCollection<string> { };

                    foreach (var item in SendPersons)
                    {

                        if (item.Level == alertinfo.AlertLevel)
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
                     MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString("Tip"), ApplicationContext.Instance.StringResourceReader.GetString("ALARM_SendSuc"), MessageDialogButton.Ok);

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

        private ObservableCollection<PTMS.ServiceReference.VehicleAlertService.AlarmEmailInfo> _sendpersons;
        public ObservableCollection<PTMS.ServiceReference.VehicleAlertService.AlarmEmailInfo> SendPersons
        {
            get { return _sendpersons; }
            set
            {
                _sendpersons = value;
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => SendPersons));
            }
        }

        private PTMS.ServiceReference.VehicleAlertService.AlarmEmailInfo _selectedperson;
        public PTMS.ServiceReference.VehicleAlertService.AlarmEmailInfo SelectedPerson
        {
            get { return _selectedperson; }
            set
            {
                _selectedperson = value;
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => SelectedPerson));
            }
        }
        
        private bool m_IsOpen = true;
        public bool IsOpen
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
        public bool IsVisual
        {
            get
            {
                return _IsVisual;
            }
            set
            {
                if (IsOpen != value)
                {
                    _IsVisual = value;
                }
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => IsVisual));
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
        private DateTime? _alerttime;
        /// <summary>
        /// 
        /// </summary>
        public DateTime? AlertTime
        {
            get
            {
                return this._alerttime;
            }
            set
            {
                this._alerttime = value;
                RaisePropertyChanged(() => AlertTime);
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

        string alertid = string.Empty;

        BusinessAlertEx alertinfo = null;

        private EmailServiceClient emailclient = null;
        private VehicleAlertServiceClient vehicleclient = null;
        private VehicleAlertServiceClient InilitClient()
        {
            VehicleAlertServiceClient vehicleAlertServiceClient = ServiceClientFactory.Create<VehicleAlertServiceClient>();
            vehicleAlertServiceClient.GetAllAlertEmailCompleted += VehicleAlertServiceClient_GetAllAlertEmailCompleted;
            vehicleAlertServiceClient.InsertBusinessAlertHandleCompleted += vehicleAlertServiceClient_InsertBusinessAlertHandleCompleted;
            vehicleAlertServiceClient.GetAllAlertEmailAsync(ApplicationContext.Instance.AuthenticationInfo.ClientID);
            return vehicleAlertServiceClient;
        }

        private void VehicleAlertServiceClient_GetAllAlertEmailCompleted(object sender, GetAllAlertEmailCompletedEventArgs e)
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

        void vehicleAlertServiceClient_InsertBusinessAlertHandleCompleted(object sender, InsertBusinessAlertHandleCompletedEventArgs e)
        {
            try
            {
                if (e.Error != null)
                {
                    ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), e.Error);
                    MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString(LProxy.ServerError));
                }
                else
                {
                    if (e.Result.IsSuccess == false)
                    {
                        if (!string.IsNullOrEmpty(e.Result.ErrorMsg))
                        {
                            ApplicationContext.Instance.Logger.LogError(MethodBase.GetCurrentMethod().ToString(),
                                e.Result.ErrorMsg);
                            MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString(e.Result.ErrorMsg));
                        }
                        else
                        {
                            MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString(LProxy.OperatorServiceError));
                            ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(),
                                e.Result.ExceptionMessage);
                        }
                    }
                    else
                    {
                        EventAggregator.Publish<AlertHandleResult>(e.Result.Result);
                        IsVisual = false;
                    }
                }
            }
            finally
            {
                VehicleAlertServiceClient vehicleAlertServiceClient = sender as VehicleAlertServiceClient;
                if (vehicleAlertServiceClient != null)
                {
                    vehicleAlertServiceClient.CloseAsync();
                    vehicleAlertServiceClient = null;
                }
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

        public void OnImportsSatisfied()
        {
            EventAggregator.SubscribeOnDispatcher<AlertHandleDisplayArgs>(this);
            EventAggregator.SubscribeOnDispatcher<MonitorAlertInfoDisplay>(this);
        }

        public void HandleEvent(AlertHandleDisplayArgs publishedEvent)
        {
            IsVisual = publishedEvent.Show;
            FirstCheck = true;
            Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => FirstCheck));
        }

        public void HandleEvent(MonitorAlertInfoDisplay publishedEvent)
        {
            try
            {
                if (publishedEvent.DisPlayInfo != null)
                {
                    alertinfo = publishedEvent.DisPlayInfo;
                    alertid = publishedEvent.DisPlayInfo.Id;

                    Handler = ApplicationContext.Instance.AuthenticationInfo.Account;
                    VehicleID = publishedEvent.DisPlayInfo.VehicleId;
                    AlertTime = publishedEvent.DisPlayInfo.AlertTime;

                    FirstCheck = true;
                    Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => FirstCheck));

                    if (publishedEvent.DisPlayInfo.Status != 0)
                    {
                        IsCommitEnable = false;
                        Note = publishedEvent.DisPlayInfo.Note;
                        HandleTime = publishedEvent.DisPlayInfo.HandleTime;
                    }
                    else
                    {
                        HandleTime = DateTime.Now;
                        Note = string.Empty;
                        IsCommitEnable = true;
                    }
                }
                else
                {
                    IsCommitEnable = false;
                    alertid = string.Empty;
                    AlertTime = null;
                    Handler = null;
                    HandleTime = null;
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }

        private void AddEmail(string obj)
        {
            PTMS.ServiceReference.VehicleAlarmService.AlarmEmailInfo email = new PTMS.ServiceReference.VehicleAlarmService.AlarmEmailInfo
            {
                ClientId = ApplicationContext.Instance.AuthenticationInfo.ClientID,
                ID = Guid.NewGuid().ToString(),
                Level = 1,
                Name = "",
                Mail = "",
            };

            ChildSendMail child = new ChildSendMail(ApplicationContext.Instance.StringResourceReader.GetString("MONITOR_AddEmail"));         
            child.Edit(email);

            child.Closed += (m, n) =>
            {
                if (child.DialogResult == true)
                {

                    try
                    {
                        vehicleclient = ServiceClientFactory.Create<VehicleAlertServiceClient>();
                        vehicleclient.AddAlertEmailCompleted += ((nsender, ne) =>
                        {

                            if (ne.Error != null || ne.Result.IsSuccess == false)
                            {
                                ApplicationContext.Instance.Logger.LogException("BASEINFO_Operate_Failed", ne.Error);
                                return;
                            }

                            PTMS.ServiceReference.VehicleAlertService.AlarmEmailInfo tt = new PTMS.ServiceReference.VehicleAlertService.AlarmEmailInfo
                            {
                                Level = child.mail.Level,
                                Name = child.mail.Name,
                                Mail = child.mail.Mail,
                            };

                            SendPersons.Add(tt);
                            SelectedPerson = null;
                            Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => SendPersons));
                            if (SendPersons.Count > 0)
                            {
                                SelectedPerson = SendPersons[0];
                                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => SendPersons));

                            }
                            Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => SelectedPerson));

                        });

                        PTMS.ServiceReference.VehicleAlertService.AlarmEmailInfo alertEmail = new PTMS.ServiceReference.VehicleAlertService.AlarmEmailInfo
                        {
                            ClientId = child.mail.ClientId,
                            ID = child.mail.ID,
                            Level = child.mail.Level,
                            Name = child.mail.Name,
                            Mail = child.mail.Mail,
                        };
                        vehicleclient.AddAlertEmailAsync(alertEmail);

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
                PTMS.ServiceReference.VehicleAlertService.AlarmEmailInfo email = SelectedPerson;


                ChildSendMail child = new ChildSendMail(ApplicationContext.Instance.StringResourceReader.GetString("MONITOR_EditEmail"));
                PTMS.ServiceReference.VehicleAlarmService.AlarmEmailInfo alarmEmail = new PTMS.ServiceReference.VehicleAlarmService.AlarmEmailInfo
                {
                    Level = email.Level,
                    Name = email.Name,
                    Mail = email.Mail,
                };
                child.Edit(alarmEmail);
                child.Closed += (m, n) =>
                {
                    if (child.DialogResult == true)
                    {

                        try
                        {
                            vehicleclient = ServiceClientFactory.Create<VehicleAlertServiceClient>();
                            vehicleclient.UpdateAlertEmailCompleted += ((nsender, ne) =>
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
                            vehicleclient.UpdateAlertEmailAsync(SelectedPerson);

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
                            vehicleclient = ServiceClientFactory.Create<VehicleAlertServiceClient>();
                            vehicleclient.DeleteAlertEmailCompleted += ((nsender, ne) =>
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
                            vehicleclient.DeleteAlertEmailAsync(SelectedPerson.ID);
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

    }
}
