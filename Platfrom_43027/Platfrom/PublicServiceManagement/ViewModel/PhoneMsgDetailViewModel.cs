using BaseLib.Model;
using BaseLib.ViewModels;
using Gsafety.Common.CommMessage;
using Gsafety.PTMS.Bases.Models;
using Gsafety.PTMS.ServiceReference.PublicService;
using Gsafety.PTMS.Share;
using Jounce.Core.View;
using Jounce.Core.ViewModel;
using System;
using System.Collections.Generic;
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
using System.Linq;
using Gsafety.Common.Controls;

namespace PublicServiceManagement.ViewModel
{
    [ExportAsViewModel(PublicServiceName.PhoneMsgDetailVm)]
    public class PhoneMsgDetailViewModel : DetailViewModel<RunAppMessage>
    {
        

        public event EventHandler<SaveResultArgs> OnSaveResult;

        private RunAppMessageClient InitServiceClient()
        {
            RunAppMessageClient client = ServiceClientFactory.Create<RunAppMessageClient>();
            client.InsertRunAppMessageCompleted += client_InsertRunAppMessageCompleted;
            client.UpdateRunAppMessageCompleted += client_UpdateRunAppMessageCompleted;
            return client;
        }

        void client_UpdateRunAppMessageCompleted(object sender, UpdateRunAppMessageCompletedEventArgs e)
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
                        SaveResultArgs args = new SaveResultArgs();
                        args.Result = true;

                        if (OnSaveResult != null)
                        {
                            OnSaveResult(this, args);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                InitServiceClient();
                ApplicationContext.Instance.Logger.LogException("client_UpdateRunAppMessage", ex);
            }
            finally
            {
                CloseClient(sender);
            }
        }

        void client_InsertRunAppMessageCompleted(object sender, InsertRunAppMessageCompletedEventArgs e)
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
                        SaveResultArgs args = new SaveResultArgs();
                        args.Result = true;

                        if (OnSaveResult != null)
                        {
                            OnSaveResult(this, args);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                InitServiceClient();
                ApplicationContext.Instance.Logger.LogException("client_InsertRunAppMessage", ex);
            }
            finally
            {
                CloseClient(sender);
            }
        }

        private static void CloseClient(object sender)
        {
            RunAppMessageClient client = sender as RunAppMessageClient;
            if (client != null)
            {
                client.CloseAsync();
                client = null;
            }
        }

        public PhoneMsgDetailViewModel()
        {
            ZMessageType.Add(new ComboBoxBasicStruct<EnumModel>()
            {
                Key = new EnumModel() { EnumValue = 1 },
                Value = ApplicationContext.Instance.StringResourceReader.GetString("WeatherMessage"),
            });
            ZMessageType.Add(new ComboBoxBasicStruct<EnumModel>()
            {
                Key = new EnumModel() { EnumValue = 2 },
                Value = ApplicationContext.Instance.StringResourceReader.GetString("TrafficMessage"),
            });
            ZMessageType.Add(new ComboBoxBasicStruct<EnumModel>()
            {
                Key = new EnumModel() { EnumValue = 3 },
                Value = ApplicationContext.Instance.StringResourceReader.GetString("MaintainMessage"),
            });
            ZMessageType.Add(new ComboBoxBasicStruct<EnumModel>()
            {
                Key = new EnumModel() { EnumValue = 4 },
                Value = ApplicationContext.Instance.StringResourceReader.GetString("Security"),
            });
            ZMessageType.Add(new ComboBoxBasicStruct<EnumModel>()
            {
                Key = new EnumModel() { EnumValue = 5 },
                Value = ApplicationContext.Instance.StringResourceReader.GetString("Notice"),
            });
        }

        public new void ActivateView(string viewName, System.Collections.Generic.IDictionary<string, object> viewParameters)
        {
            try
            {
                base.ActivateView(viewName, viewParameters);
                action = viewParameters["action"].ToString();
                switch (action)
                {
                    case "view":
                        Title = ApplicationContext.Instance.StringResourceReader.GetString("Common_View");
                        IsReadOnly = true;
                        ViewVisibility = Visibility.Collapsed;
                        InitialModel = viewParameters["model"] as RunAppMessage;
                        InitialFromInitialModel();
                        break;
                    case "update":
                        Title = ApplicationContext.Instance.StringResourceReader.GetString("Common_Update");
                        IsReadOnly = false;
                        ViewVisibility = Visibility.Visible;
                        InitialModel = viewParameters["model"] as RunAppMessage;
                        InitialFromInitialModel();
                        CurrentModel = new RunAppMessage();
                        break;
                    case "add":
                        Title = ApplicationContext.Instance.StringResourceReader.GetString("Common_Add");
                        IsReadOnly = false;
                        ViewVisibility = Visibility.Visible;
                        CurrentModel = new RunAppMessage();
                        Reset();
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }
        protected override void Reset()
        {
           // ChauffeurId = string.Empty;
           // RecordId = string.Empty;
            if (InitialModel != null)
            {
                InitialFromInitialModel();
            }
            else
            {
                MessageTitle = string.Empty;
                Message = string.Empty;
                VMessageType = ZMessageType.FirstOrDefault();
            }
        }
        public void InitialFromInitialModel()
        {
            MessageTitle = InitialModel.MessageTitle;
            Message = InitialModel.Message;
            VMessageType = ZMessageType.FirstOrDefault(n => n.Key.EnumValue == (int)InitialModel.MessageType);            
            //VehicleId = InitialModel.VehicleId;
        }
        protected override void ValidateAll()
        {
         
            ValidateMessage(ExtractPropertyName(() => Message), _message);
            ValidateMessageTitle(ExtractPropertyName(() => MessageTitle), _messageTitle);           
            
        }
        protected override void OnCommitted()
        {
            try
            {
                CurrentModel.ClientId = ApplicationContext.Instance.AuthenticationInfo.ClientID;
                CurrentModel.Message = Message;
                CurrentModel.MessageTitle = MessageTitle;
                CurrentModel.Creator = ApplicationContext.Instance.AuthenticationInfo.UserID;
                //CurrentModel.VehicleId = VehicleId;
                CurrentModel.MessageType = Convert.ToInt16(VMessageType.Key.EnumValue);
                CurrentModel.CreateTime = DateTime.Now.ToUniversalTime();
                if (action.Equals("update"))
                {
                    Update();
                }
                else
                {
                    Add();
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }
        protected override void Return()
        {
           
        }
        protected void Add()
        {
            RunAppMessageClient client = InitServiceClient();
            CurrentModel.ID = Guid.NewGuid().ToString();           
            client.InsertRunAppMessageAsync(CurrentModel);
        }
        protected void Update()
        {
           RunAppMessageClient client = InitServiceClient();
            CurrentModel.ID = InitialModel.ID;
            client.UpdateRunAppMessageAsync(CurrentModel);
        }

        private List<ComboBoxBasicStruct<EnumModel>> _messageType = new List<ComboBoxBasicStruct<EnumModel>>();
        public List<ComboBoxBasicStruct<EnumModel>> ZMessageType
        {
            get { return _messageType; }
            set
            {
                _messageType = value;
                RaisePropertyChanged(() => ZMessageType);
            }
        }

        private ComboBoxBasicStruct<EnumModel> vMessageType = new ComboBoxBasicStruct<EnumModel>();
        /// <summary>
        /// 选中的车辆服务类型
        /// </summary>
        public ComboBoxBasicStruct<EnumModel> VMessageType
        {
            get { return vMessageType; }
            set
            {
                vMessageType = value;
                RaisePropertyChanged(() => VMessageType);
            }
        }

        private string _messageTitle;
        public string MessageTitle
        {
            get { return _messageTitle; }
            set
            {
                _messageTitle = value == null ? null : value.Trim();
                ValidateMessageTitle(ExtractPropertyName(() => MessageTitle), _messageTitle);
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => MessageTitle));
            }
        }
        private void ValidateMessageTitle(string prop, string value)
        {
            ClearErrors(prop);
            if (string.IsNullOrEmpty(MessageTitle))
            {
                base.SetError(prop, ApplicationContext.Instance.StringResourceReader.GetString(LProxy.NotNull));
            }
        }


        private string _message;
        public string Message
        {
            get { return _message; }
            set
            {
                _message = value == null ? null : value.Trim();
                ValidateMessage(ExtractPropertyName(() => Message), _message);
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => Message));
            }
        }
        private void ValidateMessage(string prop, string value)
        {
            ClearErrors(prop);
            if (string.IsNullOrEmpty(Message))
            {
                base.SetError(prop, ApplicationContext.Instance.StringResourceReader.GetString(LProxy.NotNull));
            }
        }
    }
}
